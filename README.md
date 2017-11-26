# DTAInstaller
Dawn of the Tiberium Age Self-Extracting Installer

A simple installer for [Dawn of the Tiberium Age](http://www.moddb.com/mods/the-dawn-of-the-tiberium-age), but it can be easily modified to fit any project.

Created with Visual Studio 2017. Compiling may also work with older versions, but it hasn't been tested.

How it works
------------

After compiling, you can simply take DTAInstaller.exe and distribute it. When run, it'll display the window below. After the user clicks Install, the installer

1. creates the specified directory, if it doesn't already exist. If the directory exists and is not empty, the installer informs the user about it and asks whether they wish to proceed.
2. extracts the Resources\Installer.zip file (which is embedded in the compiled `DTAInstaller.exe` as a resource) to the specified installation directory
3. if the "Create Desktop Shortcut" option was checked, a desktop shortcut to the `target directory\DTA.exe` is created
4. the installer starts `DTA.exe` from the target directory

The code is quite simple and all the functionality is in `Form1.cs`, so the filenames etc. can be easily modified to fit any project.

![Screenshot](screenshot.png?raw=true "DTA Installer")
