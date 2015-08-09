using Microsoft.VisualBasic;
using Octokit;
using Packager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace ReleaseMakerScript
{
    internal class Script
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("Packager v" + typeof(Script).Assembly.GetName().Version.ToString());
            Console.WriteLine("Copyright 2015 by Mohamed Alkindi, This software is licensed under GPL 3.0");
            Console.WriteLine();
            if (Information.IsNothing(Settings.Default.ExcludeFileList))
            {
                Settings.Default.ExcludeFileList = new System.Collections.Specialized.StringCollection();
            }
            if (Information.IsNothing(Settings.Default.ReleaseLocation))
            {
                Settings.Default.ReleaseLocation = new System.Collections.Specialized.StringCollection();
            }
            Settings.Default.Save();
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "-h":
                        Console.WriteLine(
    @"Top level Arguments:-
 -h	Show help
 -s	Settings
 -c Check for updates
Settings:-
 --r	Release directory setting
  ---add[directory name]		Add release directory location
  ---remove [directory name]	Remove exist directory form release directory list
  ---show						Show All release directories from the list
  ---clear					Clear all release directories list
 --e	Exclude list settings:-
  ---add[Real filename | Global file name]	Add file to be excluded from packaging
   [Real filename]							Real filename is excluded from specific file from specific directory in specific directory location, this option requiring a full physical path to the file, for example ""C:\\example1\\example2\\example3.txt""
   [Global filename]						Exclude file have the same name and over all packaging list, this option dos not require full physical path of the file, for example ""example.txt""
  ---remove [filename]	Remove exist exclude from the database to be included to the list
  ---show					Show All exclude files list
  ---clear				Clear all exclude file list
 --g	Show GUI setting, this require System.Windows.Forms assembly to be included, if this argument executed in server running in CLI then this will make problems
 --clear	Clear all settings"
    );
                        break;

                    case "-c":
                        if (!CheckNet())
                        {
                            Console.Error.WriteLine("Please check your Internet connection");
                        }
                        GitHubClient clinet = new GitHubClient(new ProductHeaderValue("packager"));
                        var release = clinet.Release.GetAll("AlkindiX", "packager");
                        Release re = null;
                        foreach (var item in release.Result)
                        {
                            re = item;
                            break;
                        }
                        Console.WriteLine("Checking for update...");
                        release.Wait();
                        Console.WriteLine("The current release of packager is {0} released at {1}, you can download it at \n{2}", re.TagName, re.PublishedAt.ToString(), re.HtmlUrl);
                        var ass = clinet.Release.GetAllAssets("AlkindiX", "packager", re.Id);
                        Console.WriteLine("Reading files on the current update...");
                        ass.Wait();
                        DirectoryInfo updatedir = new DirectoryInfo("updates");
                        if (!updatedir.Exists)
                        {
                            updatedir.Create();
                        }
                        DirectoryInfo tagdir = new DirectoryInfo(Path.Combine(updatedir.FullName, re.TagName));
                        if (!tagdir.Exists)
                        {
                            tagdir.Create();
                        }

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
                        break;

                    case "-s":
                        if (args.Length < 2)
                        {
                            Console.Error.WriteLine("Arguments are missing, type -h for argument list");
                            return;
                        }
                        switch (args[1])
                        {
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
                    zip.AddFile(file, "/");
                    Console.WriteLine("Adding \"" + Path.GetFileName(file) + "\" in the zip");
                }
                Console.WriteLine("Creating zip file...");
                zip.Save();
                Console.WriteLine("Creating zip file successful, produced file is located in \"" + releasefile + "\"");
            }
            Console.WriteLine();
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