﻿using Ionic.Zip;
using IWshRuntimeLibrary;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace DTAInstaller
{
    public partial class Form1 : Form
    {
        private const string INSTALLER_ZIP_NAME = "Installer.zip";

        public Form1()
        {
            InitializeComponent();
            tbInstallationDir.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + Path.DirectorySeparatorChar + "Dawn of the Tiberium Age";
        }

        private void btnAbort_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            folderBrowserDialog.SelectedPath = tbInstallationDir.Text;
            folderBrowserDialog.ShowNewFolderButton = true;
            folderBrowserDialog.Description = "Select installation directory...";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                tbInstallationDir.Text = folderBrowserDialog.SelectedPath;
            }
            folderBrowserDialog.Dispose();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            if (tbInstallationDir.Text.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                tbInstallationDir.Text = tbInstallationDir.Text.Substring(0, tbInstallationDir.Text.Length - 1);
            }

            var invalidChars = Path.GetInvalidPathChars();
            foreach (char invalidChar in invalidChars)
            {
                foreach (char character in tbInstallationDir.Text)
                {
                    if (invalidChar == character)
                        MessageBox.Show("Your directory path has invalid characters in it.", "Invalid directory");
                }
            }

            if (Directory.Exists(tbInstallationDir.Text))
            {
                if (Directory.GetFiles(tbInstallationDir.Text).Length > 0)
                {
                    if (MessageBox.Show("The selected directory is not empty. Do you wish to continue installing there?",
                        "Directory Not Empty", MessageBoxButtons.YesNo) == DialogResult.No)
                        return;
                }
            }
            else
            {
                try
                {
                    Directory.CreateDirectory(tbInstallationDir.Text);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Creating directory failed! Error message: " + ex.Message, "Creating directory failed");
                    return;
                }
            }

            string destDir = tbInstallationDir.Text + Path.DirectorySeparatorChar;

            System.IO.File.WriteAllBytes(destDir + INSTALLER_ZIP_NAME,
                Properties.Resources.Installer);

            using (ZipFile zf = ZipFile.Read(destDir + INSTALLER_ZIP_NAME))
            {
                foreach (ZipEntry entry in zf)
                {
                    entry.Extract(destDir, ExtractExistingFileAction.OverwriteSilently);
                }
            }

            if (checkBox1.Checked)
            {
                object shDesktop = (object)"Desktop";
                WshShell shell = new WshShell();
                string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + Path.DirectorySeparatorChar + "Dawn of the Tiberium Age.lnk";
                IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
                shortcut.Description = "Play Dawn of the Tiberium Age";
                shortcut.TargetPath = tbInstallationDir.Text + Path.DirectorySeparatorChar + "DTA.exe";
                shortcut.WorkingDirectory = tbInstallationDir.Text;
                shortcut.Save();
            }

            System.IO.File.Delete(destDir + INSTALLER_ZIP_NAME);

            ProcessStartInfo startInfo = new ProcessStartInfo(destDir + "DTA.exe");
            startInfo.WorkingDirectory = destDir;
            Process.Start(startInfo);
            Application.Exit();
        }
    }
}
