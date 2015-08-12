/*
    Packager - Simple tool for packaging binaries
    Copyright (C) 2015 by Mohamed Alkindi

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using Microsoft.VisualBasic;
using Octokit;
using Packager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Windows.Forms;

namespace Packager
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            LogAttrbute.log.Info("Starting up packager program...");
            Console.WriteLine("Packager v" + typeof(Program).Assembly.GetName().Version.ToString());
            Console.WriteLine("Packager  Copyright (C) 2015 by Mohamed Alkindi\n\tThis program comes with ABSOLUTELY NO WARRANTY; for details type `-l'.\n\tThis is free software, and you are welcome to redistribute it\n\tunder certain conditions.");
            Console.WriteLine();
            if (Information.IsNothing(Settings.Default.ExcludeFileList))
            {
                LogAttrbute.log.Info("Exclude file list value is null, create it");
                Settings.Default.ExcludeFileList = new System.Collections.Specialized.StringCollection();
            }
            if (Information.IsNothing(Settings.Default.ReleaseLocation))
            {
                LogAttrbute.log.Info("Release directory list is null, create it");
                Settings.Default.ReleaseLocation = new System.Collections.Specialized.StringCollection();
            }
            LogAttrbute.log.Info("Save settings");
            Settings.Default.Save();
            if (args.Length > 0)
            {
                ArgumnentsAhead(args);
                Settings.Default.Save();

                return;
            }
            if (Settings.Default.ReleaseLocation == null)
            {
                Console.Error.WriteLine("Release location is not defined. Please locate the release location at least one by -h for instructions");

                return;
            }
            if (Settings.Default.ExcludeFileList.Count <= 0)
            {
                Console.WriteLine("Warning: Exclude list is empty, type -h for argument list");
            }
            if (Settings.Default.ReleaseLocation.Count <= 0)
            {
                Console.WriteLine("Welcome to the release maker program, if this is your first time to open this program, this program help you to packaging your file into zip. when you execute this program, the program is immediately packeging your binaries or your produced files into zip format easily, but first , you must add your release directory list from where you went , you can use gui by type -e --g to open it, or you can use the command line to configuring your custom configuration by type -h for more instructions");
                Console.Error.WriteLine("The release directory list is empty, please enter -h for instructions");

                return;
            }
            foreach (var itemdir in Settings.Default.ReleaseLocation)
            {
                Console.WriteLine("Processing release directory \"" + itemdir);
                DirectoryInfo info = new DirectoryInfo((string)itemdir);
                IList<string> files = new List<string>();
                foreach (var item in info.GetFiles())
                {
                    files.Add(item.FullName);
                }
                var f = Settings.Default.ExcludeFileList;
                f.Add("release.zip");
                Console.WriteLine("Starting release maker");
                foreach (string item in f)
                {
                    foreach (string item2 in files)
                    {
                        if (item2.Contains(item))
                        {
                            Console.WriteLine("INFO: Excluding \"" + item2 + "\" from the list");
                            files.Remove(item2);
                            break;
                        }
                    }
                }
                var releasezipname = "release.zip";
                var releasefile = Path.Combine(info.FullName, releasezipname);
                if (File.Exists(releasezipname))
                {
                    Console.WriteLine("Warning: Release file is exist");
                }
                Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(releasefile);
                foreach (var file in files)
                {
                    if (zip.ContainsEntry(Path.GetFileName(file)))
                    {
                        zip.RemoveEntry(Path.GetFileName(file));
                        Console.WriteLine("Removing \"" + Path.GetFileName(file) + "\" from zip entry");
                    }
                    FileIOPermission parm = new FileIOPermission(FileIOPermissionAccess.AllAccess, file);
                    parm.AllLocalFiles = FileIOPermissionAccess.AllAccess;
                    try
                    {
                        parm.Demand();
                    }
                    catch (Exception sx)
                    {
                        LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Exception, sx.ToString());
                        Console.Error.WriteLine("Cannot add file name {0}, because you do not have the permission to read it, so this file is skip and not added to the zip file", file);
                        LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Error, string.Format("Cannot add file name {0}, because you do not have the permission to read it, so this file is skip and not added to the zip file", file));
                        continue;
                    }
                    zip.AddFile(file, "/");
                    Console.WriteLine("Adding \"" + Path.GetFileName(file) + "\" in the zip");
                }
                Console.WriteLine("Creating zip file...");
                try
                {
                    zip.Save();
                }
                catch (Exception sx)
                {
                    LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Exception, sx.ToString());
                    LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Error, "Cannot add the zip file into the directory because you do not have the permission to write the file");
                    Console.Error.WriteLine("Cannot add the zip file into the directory because you do not have the permission to write the file");
                    return;
                }
                Console.WriteLine("Creating zip file successful, produced file is located in \"" + releasefile + "\"");
            }
            Console.WriteLine();
        }

        private static void ArgumnentsAhead(string[] args)
        {
            LogAttrbute.log.Info("There are top level argument has been caught to read it");
            switch (args[0])
            {
                case "-h":
                    LogAttrbute.log.Info("top level argument for help, showing help");
                    Console.WriteLine(
@"Top level Arguments:-
 -h                              Show help
 -s                              Settings
 -c                              Check for updates
 -v                              Version
 -l                              License

Settings:-
 --r                             Release directory setting

  ---add[directory name]         Add release directory location
  ---remove [directory name]     Remove exist directory form release directory list
  ---show                        Show All release directories from the list
  ---clear                       Clear all release directories list

 --e                             Exclude list settings:-

  ---add[filename]               Add file to be excluded from packaging
  ---remove [filename]           Remove exist exclude from the database to be included to the list
  ---show                        Show All exclude files list
  ---clear                       Clear all exclude file list

 --g                             Show GUI setting

 --log [true|false]              Enable Logging

 --clear	                     Clear all settings

License arguments
  --g   Show graphical user interface window"
);
                    break;

                case "-v":
                    LogAttrbute.log.Info("Top level argument for getting version, getting version");
                    Console.WriteLine("Packager v" + typeof(Program).Assembly.GetName().Version.ToString());
                    break;

                case "-l":
                    LogAttrbute.log.Info("Top level argument for getting license of the project");
                    if (args.Length < 2)
                    {
                        LogAttrbute.log.Info("License getting from LICENSE file");
                        FileInfo lic = new FileInfo("LICENSE");
                        LogAttrbute.log.Info("Checking if the license file is available or not");
                        if (!lic.Exists)
                        {
                            LogAttrbute.log.Info("License file not found, showing warning message");
                            Console.WriteLine("Warning :License not found, you can view it at http://www.gnu.org/licenses/gpl-3.0.en.html");
                            LogAttrbute.log.Info("Exit application with file not found existing for LICENSE file");
                            return;
                        }
                        LogAttrbute.log.Info("File founded and now using read only stream file");
                        StreamReader strmreader = new StreamReader(lic.OpenRead());
                        LogAttrbute.log.Info("Writing a little bit big output for LICENSE file");
                        Console.WriteLine(strmreader.ReadToEnd());
                        LogAttrbute.log.Info("Writing output completed");
                        LogAttrbute.log.Info("Exit application");

                        return;
                    }

                    LogAttrbute.log.Info("Second level argument for checking argument");
                    switch (args[1])
                    {
                        case "--g":
                            LogAttrbute.log.Info("Argument is getting gui window for showing license");
                            var s = new Packager.License();
                            LogAttrbute.log.Info("Showing license window..");
                            s.ShowDialog();
                            LogAttrbute.log.Info("The user closed the window");
                            break;

                        default:
                            LogAttrbute.log.Info("The input arg is invalid , showing Error message");
                            Console.Error.WriteLine("Argument unknown, please type -h for argument list");
                            break;
                    }
                    break;

                case "-c":
                    LogAttrbute.log.Info("Top level arg for checking for update on the GitHub repo");
                    LogAttrbute.log.Info("Calling managed code...");
                    if (!CheckNet())
                    {
                        LogAttrbute.log.Info("No connection to the Internet, or Internet is unavailable, because CheckNet() call  InternetGetConnectedState(out desc, 0); for if there is Internet and returned that there are no Internet access");
                        Console.Error.WriteLine("Please check your Internet connection in order to check the update. Or Go to https://github.com/AlkindiX/packager/releases");

                        return;
                    }
                    LogAttrbute.log.Info("There are an Internet connection, processing to the update checker...");
                    LogAttrbute.log.Info("Preparing GitHubClient from OctaKit using GitHub API");
                    GitHubClient clinet = new GitHubClient(new ProductHeaderValue("packager"));
                    LogAttrbute.log.Info("Preparing to invoke for all releases from GitHub release from project packager repo by the creator of the progrma AlkindiX ");
                    var release = clinet.Release.GetAll("AlkindiX", "packager");
                    Release re = null;
                    LogAttrbute.log.Info("The release checking is invoked, now waiting for result form GitHub...");
                    Console.WriteLine("Checking for update...");
                    release.Wait();
                    LogAttrbute.log.Info("Result getting successfully");
                    foreach (var item in release.Result)
                    {
                        LogAttrbute.log.Info("Getting the latest release at the top of the list and break the loop");
                        re = item;
                        break;
                    }
                    Console.WriteLine("The current release of packager is {0} released at {1}, you can download it at \n{2}", re.TagName, re.PublishedAt.ToString(), re.HtmlUrl);
                    Console.WriteLine("Change log:- \n{0}", re.Body);
                    LogAttrbute.log.Info("Invoking GetAllRelease for getting files inside the release");
                    var ass = clinet.Release.GetAllAssets("AlkindiX", "packager", re.Id);
                    Console.WriteLine("Reading files on the current update...");
                    ass.Wait();
                    LogAttrbute.log.Info("Checking if dir updates is exist");
                    DirectoryInfo updatedir = new DirectoryInfo("updates");
                    if (!updatedir.Exists)
                    {
                        LogAttrbute.log.Info("Creating update dir");
                        updatedir.Create();
                    }
                    LogAttrbute.log.Info("Create version file");
                    DirectoryInfo tagdir = new DirectoryInfo(Path.Combine(updatedir.FullName, re.TagName));
                    if (!tagdir.Exists)
                    {
                        tagdir.Create();
                    }
                    LogAttrbute.log.Info("Begin perform download operation");
                    int x = 0;
                    foreach (var item in ass.Result)
                    {
                        x++;
                        Console.WriteLine("{0} {1}", x.ToString(), item.Name);
                        Console.WriteLine("Downloading {0}", item.Name);
                        WebClient clin = new WebClient();
                        var file_full_path = Path.Combine(tagdir.FullName, item.Name);
                        if (File.Exists(file_full_path))
                        {
                            Console.WriteLine("Warning: The release is already exist {0}", file_full_path);
                            File.Delete(file_full_path);
                        }
                        clin.DownloadFile(item.BrowserDownloadUrl, file_full_path);
                        Console.WriteLine("Downloading complete at {0}", file_full_path);
                    }
                    LogAttrbute.log.Info("End download operation");

                    break;

                case "-s":
                    if (args.Length < 2)
                    {
                        Console.Error.WriteLine("Arguments are missing, type -h for argument list");

                        return;
                    }
                    switch (args[1])
                    {
                        case "--log":
                            LogAttrbute.log.Info("Second level for setting the logging value");
                            if (args.Length < 3)
                            {
                                LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Error, "Missing argument for logging value");
                                Console.Error.WriteLine("Missing argument for enable logging");
                                return;
                            }
                            try
                            {
                                switch (bool.Parse(args[2]))
                                {
                                    case true:
                                        LogAttrbute.log.Info("Logging is enabled");
                                        Settings.Default.EnableLog = true;
                                        Console.WriteLine("Logging is enabled");
                                        break;

                                    case false:
                                        LogAttrbute.log.Info("Logging is disabled");
                                        Settings.Default.EnableLog = false;
                                        Console.WriteLine("Logging is disabled");
                                        break;
                                }
                            }
                            catch (FormatException fx)
                            {
                                LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Exception, fx.ToString());
                                Console.Error.WriteLine("The format given is invalid");
                            }
                            break;

                        case "--clear":
                            Console.WriteLine("Reseting data...");
                            Settings.Default.Reset();
                            Console.WriteLine("All data cleared");
                            break;

                        case "--g":
                            System.Windows.Forms.Application.EnableVisualStyles();
                            FormConfiguration config = new FormConfiguration();
                            config.ShowDialog();
                            break;

                        case "--r":
                            if (args.Length < 3)
                            {
                                Console.WriteLine("Missing arguments, please type -h for more information");

                                return;
                            }
                            switch (args[2])
                            {
                                case "---add":
                                    if (args.Length < 4)
                                    {
                                        Console.Error.WriteLine("Missing argument, please enter the directory name to be added");
                                    }
                                    try
                                    {
                                        DirectoryInfo info = new DirectoryInfo(args[3]);
                                        if (!info.Exists)
                                        {
                                            Console.Error.WriteLine("The directory is not founded");

                                            return;
                                        }
                                        if (Settings.Default.ReleaseLocation.Contains(info.FullName))
                                        {
                                            Console.Error.WriteLine("The directory is already added to the release location list");

                                            return;
                                        }
                                        Settings.Default.ReleaseLocation.Add(info.FullName);
                                        Console.WriteLine("The directory name \"" + info.FullName + "has been added to the release location list");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine(ex.Message);
                                    }
                                    break;

                                case "---rm":
                                    if (args.Length < 4)
                                    {
                                        Console.Error.WriteLine("Missing argument, please enter the directory name to be removed");

                                        return;
                                    }
                                    try
                                    {
                                        DirectoryInfo info = new DirectoryInfo(args[3]);
                                        if (!Settings.Default.ReleaseLocation.Contains(info.FullName))
                                        {
                                            Console.Error.WriteLine("The directory you have entered is not on the release directory database");

                                            return;
                                        }
                                        Settings.Default.ReleaseLocation.Remove(info.FullName);
                                        Console.WriteLine("The directory name \"" + info.FullName + "has been removed from the release location list");
                                    }
                                    catch (Exception ex)
                                    {
                                        Console.Error.WriteLine(ex.Message);
                                    }
                                    break;

                                case "---show":
                                    foreach (var item in Settings.Default.ReleaseLocation)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    break;

                                case "---clear":
                                    Settings.Default.ReleaseLocation.Clear();
                                    break;

                                default:
                                    Console.WriteLine("Missing arguments, please enter -h for argument list");
                                    break;
                            }
                            break;

                        case "--e":
                            if (args.Length < 3)
                            {
                                Console.WriteLine("Missing arguments, type -h for argument list");

                                return;
                            }
                            switch (args[2])
                            {
                                case "---add":
                                    if (args.Length < 4)
                                    {
                                        Console.WriteLine("You didn't write the file to be exclude");
                                    }
                                    else
                                    {
                                        if (!Settings.Default.ExcludeFileList.Contains(args[3]))
                                        {
                                            Settings.Default.ExcludeFileList.Add(args[3]);
                                            Console.WriteLine("The filename: \"" + args[3] + "\" Added to the exclude list");
                                        }
                                        else
                                        {
                                            Console.WriteLine("The filename: \"" + args[3] + "\" is already included in the exclude list");
                                        }
                                    }
                                    break;

                                case "---rm":
                                    if (args.Length < 4)
                                    {
                                        Console.WriteLine("You didn't write the file to be include");
                                    }
                                    else
                                    {
                                        if (!Settings.Default.ExcludeFileList.Contains(args[3]))
                                        {
                                            Console.Error.WriteLine("File is not found in the list");
                                        }
                                        else
                                        {
                                            Settings.Default.ExcludeFileList.Remove(args[3]);
                                            Console.WriteLine("The filename: \"" + args[3] + "\" removed from the exclude list");
                                        }
                                    }
                                    break;

                                case "---show":
                                    if (Settings.Default.ExcludeFileList.Count == 0)
                                    {
                                        Console.WriteLine("The list is empty");
                                    }
                                    foreach (var item in Settings.Default.ExcludeFileList)
                                    {
                                        Console.WriteLine(item);
                                    }
                                    break;

                                case "---clear":

                                    if (Settings.Default.ExcludeFileList.Count == 0)
                                    {
                                        Console.WriteLine("The list is empty and nothing to be cleared");
                                    }
                                    Settings.Default.ExcludeFileList.Clear();
                                    break;

                                default:
                                    Console.Error.WriteLine("Unknown arguments, -h for argument list");
                                    break;
                            }
                            break;

                        default:
                            Console.WriteLine("Unknown argument, type -h for argument list");
                            break;
                    }
                    break;

                default:
                    Console.Error.WriteLine("Argument is invalid, type -h for more informations");
                    break;
            }
        }

        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool CheckNet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
        }
    }
}