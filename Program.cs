using System;
using System.Reflection;
using System.Windows.Forms;

namespace DTAInstaller
{
    static class Program
    {
        static Program()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.Contains("Ionic.Zip"))
                return Assembly.Load(Properties.Resources.Ionic_Zip);

            if (args.Name.Contains("IWshRuntimeLibrary"))
                return Assembly.Load(Properties.Resources.Interop_IWshRuntimeLibrary);

            return null;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
