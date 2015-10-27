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

using Octokit;
using Packager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Permissions;

namespace Packager
{
    internal class Program
    {
        private static Database.PackagerDatabase _database;

        public static Database.PackagerDatabase database
        {
            get
            {
                if (_database == null)
                {
                    DirectoryInfo dir = new DirectoryInfo("Setting");
                    if (!dir.Exists)
                    {
                        dir.Create();
                    }
                    var location = Path.Combine(dir.FullName, "settings.dat");
                    _database = new Database.PackagerDatabase(location);
                }
                return _database;
            }
        }

        [STAThread]
        public static void Main(string[] args)
		{
			// Show starting message to the user
			// LogAttrbute.log.Info("Starting up packager program...");
			Console.Write ("\nPackager v" + typeof(Program).Assembly.GetName ().Version.ToString ());
			string os_distribution;
			#if LINUX
			os_distribution = " - Linux, powered by Mono v4.5";
			#elif WIN
			os_distribution = " - Windows, powered by Microsoft .NET freamwork";
			#endif
			Console.Write (os_distribution);
			Console.WriteLine ();
			Console.WriteLine ("Packager  Copyright (C) 2015 by Mohamed Alkindi\n\tThis program comes with ABSOLUTELY NO WARRANTY; for details type `-l'.\n\tThis is free software, and you are welcome to redistribute it\n\tunder certain conditions.");
			Console.WriteLine ();
			// Check if there are any arguments
			// Load Application Data
			if (args.Length > 0) {
				// No argument
				ArgumnentsAhead (args);
				#if WIN
                Settings.Default.Save();
				#endif
				return;
			}
			// Check release location if it null throw error

			if (Program.database.ReleaseDirectories.Count <= 0) {
				Console.WriteLine ("Welcome to release maker.\n" +
				"If this is your first time. Please type -h for more information");
				Console.Error.WriteLine ("The release directory list is empty, please enter -h for instructions");

				return;
			}
			Console.WriteLine ("Starting release maker");
			foreach (var itemdir in Program.database.ReleaseDirectories) {
				Console.WriteLine ("[{0}\t] Processing release directory \"{1}\"", itemdir.Id, itemdir.ReleaseDirectoryLocation);
				DirectoryInfo info = new DirectoryInfo ((string)itemdir.ReleaseDirectoryLocation);
				IList<string> files = new List<string> ();
				foreach (var item in info.GetFiles()) {
					files.Add (item.FullName);
				}
				var f = Program.database.ExcludedFiles;
				f.Add (new Database.ExcludedFiles ("release.zip"));
				foreach (var item in f) {
					foreach (string item2 in files) {
						if (item2.Contains (item.Filename)) {
							Console.WriteLine ("[{0}\t] Excluding \"" + item2 + "\"", itemdir.Id);
							files.Remove (item2);
							break;
						}
					}
				}
				var releasezipname = "release.zip";
				var releasefile = Path.Combine (info.FullName, releasezipname);
				if (File.Exists (releasezipname)) {
					Console.WriteLine ("[{0}\t] Release file is exist", itemdir.Id);
				}
				Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile (releasefile);
				foreach (var file in files) {
					if (zip.ContainsEntry (Path.GetFileName (file))) {
						zip.RemoveEntry (Path.GetFileName (file));
						Console.WriteLine ("[{0}\t] Removing from zip entry \"" + Path.GetFileName (file) + "\"", itemdir.Id);
					}

					FileIOPermission parm = new FileIOPermission (FileIOPermissionAccess.AllAccess, file);
					parm.AllLocalFiles = FileIOPermissionAccess.AllAccess;
					try {
						parm.Demand ();
					} catch (Exception) {
						// LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Exception, sx.ToString());
						Console.Error.WriteLine ("[{0}\t] Cannot add file name {1}, because you do not have the permission to read it, so this file is skip and not added to the zip file", itemdir.Id, file);
						// LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Error, string.Format("Cannot add file name {0}, because you do not have the permission to read it, so this file is skip and not added to the zip file", file));
						continue;
					}
					zip.AddFile (file, "/");
					
					Console.WriteLine ("[{0}\t] Adding to zip \"" + Path.GetFileName (file) + "\"", itemdir.Id);
					
				}
				Console.WriteLine ("[{0}\t] Creating zip file...", itemdir.Id);
				try {
					zip.Save ();
				} catch (Exception) {
					// LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Exception, sx.ToString());
					// LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Error, "Cannot add the zip file into the directory because you do not have the permission to write the file");
					Console.Error.WriteLine ("[{0}\t] Cannot add the zip file into the directory because you do not have the permission to write the file", itemdir.Id);
					continue;
				}
				Console.WriteLine ("[{0}\t] Creating zip file successful, produced file is located in \"" + releasefile + "\"", itemdir.Id);
			}
			Console.WriteLine ();
			Console.WriteLine ("Package maker is exit, Thank you for using");

			Console.WriteLine ();
		}

        private static void ArgumnentsAhead(string[] args)
		{

			// LogAttrbute.log.Info("There are top level argument has been caught to read it");
			switch (args [0].ToLower ()) {
			case "-h":
                    // LogAttrbute.log.Info("top level argument for help, showing help");
				Console.WriteLine (
					@"TOP LEVEL ARGUMENTS:-     :
 -h                       :Show help
 -s                       :Settings
"
					#if WIN
+ @" -c                       :Check for updates" +
					#elif LINUX
					+ "" +
					#endif
					@" -v                       :Version
 -l                       :License
                          :
SETTINGS (-s) OPTIONS     :
 --r                      :Release directory setting
                          :
  ---add[directory name]  :Add release directory location
  ---remove 
     [dir name|dir Id]    :Remove exist directory using its name or id
                          :
  ---show                 :Show All release directories as 
                          :[Id	] - [dir]
  ---clear                :Clear all release directories list
                          :
 --e                      :Exclude list settings:-
                          :
  ---add[filename]        :Add file to exclude list
  ---remove [filename]    :Remove file from exclude list
  ---show                 :Show All exclude files list
  ---clear                :Clear all exclude file list
                          :
 --c                      :Command Run
                          :
  ---add[release dir,..]  :Add new command line for execution. Put
     [Command run]        :Free   space   between   every   block.
     [before|after|pre]   :[release dir] -> exist release directory,
                          :[Command run] -> cmd command line. Put \n
                          :For       every       command       line
                          :[before|after|pre] -> Before --> before
                          :performing  zipping.  After  -->  After
                          :perform   zipping   for   current   dir
                          :pre --> On performing zipping for current
                          :dir.
  ---remove [release dir] :Remove all entered command line for current
                          :release directory
  ---remove [command id]  :Remove specific command line by command line
                          :ID
  ---show                 :Show all command line and its details
  ---clear                :Clear all command line data
                          :
"
					#if WIN

+ @"--g                      :Show GUI setting
--log [true|false]       :Enable Logging" + 
					#elif LINUX
					+ "                          :" +
					#endif
					@"
                          :
 --clear                  :Clear all settings
                          :"
					#if WIN
+@"LICENCE ARGUMENTS         :
  --g                     :Show graphical user interface window

" + 
					#elif LINUX
					+ @"
" +
					#endif
					@"EXAMPLES
"
					#if WIN
+ @"  -s --g                                    :Show GUI setting" + 
					#elif LINUX
					+ "" +
					#endif
					@"
  -s --r ---add ""C:\ReleaseDir""           :Add release dir
  -s --e ---add ""C:\ReleaseDir\mango.txt"" :Exclude this file

BUG & TRACKING
  If you have any problem with this program
  on this application, feel free to  report
  your  problem  into  my  project  url  at
  https://github.com/AlkindiX/packager
  What I have from your report is all cons-
  ole output and your brief information ab-
  out the error.
"
				);
				break;

			case "-v":
                    // LogAttrbute.log.Info("Top level argument for getting version, getting version");
				Console.WriteLine ("Packager v" + typeof(Program).Assembly.GetName ().Version.ToString ());
				break;

			case "-l":
                    // LogAttrbute.log.Info("Top level argument for getting license of the project");
				if (args.Length < 2) {
					// LogAttrbute.log.Info("License getting from LICENSE file");
					FileInfo lic = new FileInfo ("LICENSE");
					// LogAttrbute.log.Info("Checking if the license file is available or not");
					if (!lic.Exists) {
						// LogAttrbute.log.Info("License file not found, showing warning message");
						Console.WriteLine ("Warning :License not found, you can view it at http://www.gnu.org/licenses/gpl-3.0.en.html");
						// LogAttrbute.log.Info("Exit application with file not found existing for LICENSE file");
						return;
					}
					// LogAttrbute.log.Info("File founded and now using read only stream file");
					StreamReader strmreader = new StreamReader (lic.OpenRead ());
					// LogAttrbute.log.Info("Writing a little bit big output for LICENSE file");
					Console.WriteLine (strmreader.ReadToEnd ());
					// LogAttrbute.log.Info("Writing output completed");
					// LogAttrbute.log.Info("Exit application");

					return;
				}

                    // LogAttrbute.log.Info("Second level argument for checking argument");
				switch (args [1].ToLower ()) {
				#if WIN
                        case "--g":
                            // LogAttrbute.log.Info("Argument is getting gui window for showing license");

                            var s = new Packager.License();
					// LogAttrbute.log.Info("Showing license window..");
					s.ShowDialog();
					// LogAttrbute.log.Info("The user closed the window");
					break;
				#endif
				default:
                            // LogAttrbute.log.Info("The input arg is invalid , showing Error message");
					Console.Error.WriteLine ("Argument unknown, please type -h for argument list");
					break;
				}
				break;
				#if WIN
                case "-c":
                    // LogAttrbute.log.Info("Top level arg for checking for update on the GitHub repo");
                    // LogAttrbute.log.Info("Calling managed code...");
                    if (!CheckNet())
                    {
                        // LogAttrbute.log.Info("No connection to the Internet, or Internet is unavailable, because CheckNet() call  InternetGetConnectedState(out desc, 0); for if there is Internet and returned that there are no Internet access");
                        Console.Error.WriteLine("Please check your Internet connection in order to check the update. Or Go to https://github.com/AlkindiX/packager/releases");

                        return;
                    }
                    // LogAttrbute.log.Info("There are an Internet connection, processing to the update checker...");
                    // LogAttrbute.log.Info("Preparing GitHubClient from OctaKit using GitHub API");
                    GitHubClient clinet = new GitHubClient(new ProductHeaderValue("packager"));
                    // LogAttrbute.log.Info("Preparing to invoke for all releases from GitHub release from project packager repo by the creator of the progrma AlkindiX ");
                    var release = clinet.Release.GetAll("AlkindiX", "packager");
                    Release re = null;
                    // LogAttrbute.log.Info("The release checking is invoked, now waiting for result form GitHub...");
                    Console.WriteLine("Checking for update...");
                    release.Wait();
                    // LogAttrbute.log.Info("Result getting successfully");
                    foreach (var item in release.Result)
                    {
                        // LogAttrbute.log.Info("Getting the latest release at the top of the list and break the loop");
                        re = item;
                        break;
                    }
                    Console.WriteLine("The current release of packager is {0} released at {1}, you can download it at \n{2}", re.TagName, re.PublishedAt.ToString(), re.HtmlUrl);
                    Console.WriteLine("Change log:- \n{0}", re.Body);
                    // LogAttrbute.log.Info("Invoking GetAllRelease for getting files inside the release");
                    var ass = clinet.Release.GetAllAssets("AlkindiX", "packager", re.Id);
                    Console.WriteLine("Reading files on the current update...");
                    ass.Wait();
                    // LogAttrbute.log.Info("Checking if dir updates is exist");
                    DirectoryInfo updatedir = new DirectoryInfo("updates");
                    if (!updatedir.Exists)
                    {
                        // LogAttrbute.log.Info("Creating update dir");
                        updatedir.Create();
                    }
                    // LogAttrbute.log.Info("Create version file");
                    DirectoryInfo tagdir = new DirectoryInfo(Path.Combine(updatedir.FullName, re.TagName));
                    if (!tagdir.Exists)
                    {
                        tagdir.Create();
                    }
                    // LogAttrbute.log.Info("Begin perform download operation");
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
                    // LogAttrbute.log.Info("End download operation");

                    break;
				#endif
			case "-s":
				if (args.Length < 2) {
					Console.Error.WriteLine ("Arguments are missing, type -h for argument list");

					return;
				}
				switch (args [1].ToLower ()) {
				case "--c":
					if (args.Length < 2) {
						Console.Error.WriteLine ("Missing argument for command line configuration. Type -h for information");
						return;
					}
					switch (args [2].ToLower ()) {
					case "---add":
						if (args.Length < 6) {
							Console.WriteLine ("arguments are missing. please type -h for information");
							return;
						}
						var releasedir = args [3];
						var commandrun = args [4];
						var commandevent = args [5];
						var releasedirfromdatabase = ReleaseDirectoryExist (releasedir);
						if (releasedirfromdatabase == null) {
							Console.Error.WriteLine ("The release directory you have entered it is not exist in the application database");
							return;
						}
						commandrun = commandrun.Replace ("\\n", "\n");
						var commandeventargumented = Database.ExecuteEvent.BeforePackaging;
						switch (commandevent.ToLower ()) {
						case "before":
							commandeventargumented = Database.ExecuteEvent.BeforePackaging;
							break;

						case "after":
							commandeventargumented = Database.ExecuteEvent.AfterPackaging;
							break;

						case "pre":
							commandeventargumented = Database.ExecuteEvent.PrePackaging;
							break;

						default:
							Console.Error.WriteLine ("the command line event argument is unknown. Please put on of [before|after|pre]");
							return;
						}
						Database.CommandRun cmd = new Database.CommandRun (commandrun, commandeventargumented, releasedirfromdatabase.CommandRunId);
						Program.AddCommandRun (releasedirfromdatabase, cmd);
						Console.WriteLine ("Command line successful added to the database as :{0}, with unique id: ", commandeventargumented.ToString (), releasedirfromdatabase.CommandRunId);
						break;

					case "---remove":
						break;

					case "---show":
						Console.WriteLine ("[{0}\t][{1}\t][{2}\t]- \"{3}\"", "Id", "release dir Id", "When", "Command line");

						foreach (var item in database.CommandsRun) {
							var crp = "[{0}\t][{1}\t][{2}\t]- \"{3}\"";
							Console.WriteLine (crp, item.Id, item.uniqueId, item.ExecuteEvent, item.Command);
						}
						break;

					case "---clear":
						break;

					default:
						Console.WriteLine ("unknown argument, please enter -h for argument list");
						break;
					}
					break;

					#if WIN
                        case "--log":
                            // LogAttrbute.log.Info("Second level for setting the logging value");
                            if (args.Length < 3)
                            {
                                // LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Error, "Missing argument for logging value");
                                Console.Error.WriteLine("Missing argument for enable logging");
                                return;
                            }
                            try
                            {
                                switch (bool.Parse(args[2].ToLower()))
                                {
                                    case true:
                                        // LogAttrbute.log.Info("Logging is enabled");
                                        Settings.Default.EnableLog = true;
                                        Console.WriteLine("Logging is enabled");
                                        break;

                                    case false:
                                        // LogAttrbute.log.Info("Logging is disabled");
                                        Settings.Default.EnableLog = false;
                                        Console.WriteLine("Logging is disabled");
                                        break;
                                }
                            }
                            catch (FormatException fx)
                            {
                                // LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Exception, fx.ToString());
                                Console.Error.WriteLine("The format given is invalid");
                            }
                            break;
					#endif
				case "--clear":
					Console.WriteLine ("Reseting data...");
					#if WIN
                            Settings.Default.Reset();
					#endif
					database.ReleaseDirectories.Clear ();
					database.ExcludedFiles.Clear ();
					database.SaveChanges ();
					Console.WriteLine ("All data cleared");
					return;

					#if WIN
                        case "--g":
                            System.Windows.Forms.Application.EnableVisualStyles();
                            FormConfiguration config = new FormConfiguration();
                            config.ShowDialog();
                            break;
					#endif
				case "--r":
					if (args.Length < 3) {
						Console.WriteLine ("Missing arguments, please type -h for more information");

						return;
					}
					switch (args [2].ToLower ()) {
					case "---add":
						if (args.Length < 4) {
							Console.Error.WriteLine ("Missing argument, please enter the directory name to be added");
							return;
						}
						// TODO : Make this function easier to use by allowing addaing more than one directory in one command line only This make this program much useful for this situatons
						try {
							var dir = args [3];
							DirectoryInfo info;
							if (Path.IsPathRooted (dir)) {
								info = new DirectoryInfo (dir);
							} else {
								info = new DirectoryInfo (Path.Combine (Environment.CurrentDirectory, dir));
							}
							if (!info.Exists) {
								Console.Error.WriteLine ("The directory is not exist");

								return;
							}
							if (ReleaseDirectoryExist (info.FullName) != null) {
								Console.Error.WriteLine ("The directory is already added to the release location list");
								return;
							}

							var res = from sp in database.ReleaseDirectories orderby sp.CommandRunId descending
							          select sp;
							var uniqid = 1;
							if (res.Count () <= 0) {
								uniqid = 1;
							} else {
								uniqid = res.First ().CommandRunId + 1;
							}
							Program.database.ReleaseDirectories.Add (new Database.RelaseDirectoy (info.FullName, uniqid));

							Console.WriteLine ("The directory name \"" + info.FullName + "\" has been added to the release location list as command line id unique: {0}", uniqid);
						} catch (Exception ex) {
							Console.Error.WriteLine (ex.ToString ());
						}

						break;

					case "---remove":
						if (args.Length < 4) {
							Console.Error.WriteLine ("Missing argument, please enter the directory name or directorty Id");
							return;
						}
						int releaseId;
						if (int.TryParse ((string)args [3], out releaseId)) {
							var release_data = ReleaseDirectoryExist (releaseId);
							if (release_data == null) {
								Console.Error.WriteLine ("The directory Id you have entered is not on the release directory database");
								return;
							}
							ReleaseDirectoryRemove (releaseId);
							Console.WriteLine ("The directory name \"" + release_data.ReleaseDirectoryLocation + "\" has been removed from the release location list");

						} else {
							try {
								DirectoryInfo info = new DirectoryInfo (args [3]);
								if (ReleaseDirectoryExist (info.FullName) == null) {
									Console.Error.WriteLine ("The directory you have entered is not on the release directory database");
									return;
								}
								ReleaseDirectoryRemove (info.FullName);
								Console.WriteLine ("The directory name \"" + info.FullName + "\" has been removed from the release location list");
							} catch (Exception ex) {
								Console.Error.WriteLine (ex.Message);
							}
						}
						break;

					case "---show":
						var rs = "[{0}\t] - \"{1}\"";
						foreach (var item in database.ReleaseDirectories) {
							Console.WriteLine (string.Format(rs,item.Id,item.ReleaseDirectoryLocation));
						}
						break;

					case "---clear":
						database.ReleaseDirectories.Clear ();
						Console.WriteLine ("Release directories are cleared");
						break;

					default:
						Console.WriteLine ("argument is understandable, please enter -h for argument list");
						break;
					}
					break;

				case "--e":
					if (args.Length < 3) {
						Console.WriteLine ("Missing arguments, type -h for argument list");

						return;
					}
					switch (args [2].ToLower ()) {
					case "---add":
						if (args.Length < 4) {
							Console.WriteLine ("You didn't write the file to be exclude");
						} else {
							if (ExcludeFileExist (args [3]) == null) {
								database.ExcludedFiles.Add (new Database.ExcludedFiles (args [3]));
								Console.WriteLine ("The filename: \"" + args [3] + "\" Added to the exclude list");
							}
							if (ExcludeFileExist (args [3]) == null) {
								Console.WriteLine ("The filename: \"" + args [3] + "\" is already included in the exclude list");
							}
						}
						break;

					case "---remove":
						if (args.Length < 4) {
							Console.WriteLine ("You didn't write the file to be include");
						} else {
							if (ExcludeFileExist (args [3]) == null) {
								Console.Error.WriteLine ("File is not found in the list");
							} else {
								ExcludeFileRemove (args [3]);
								Console.WriteLine ("The filename: \"" + args [3] + "\" removed from the exclude list");
							}
						}
						break;

					case "---show":
						if (database.ExcludedFiles.Count == 0) {
							Console.WriteLine ("The list is empty");
						}
						foreach (var item in database.ExcludedFiles) {
							Console.WriteLine ("[{0}\t] - \"{1}\"",item.Id,item.Filename);
						}
						break;

					case "---clear":

						if (database.ExcludedFiles.Count == 0) {
							Console.WriteLine ("The list is empty and nothing to be cleared");
						}
						database.ExcludedFiles.Clear ();
						Console.WriteLine ("Excluded Files are cleared");
						break;

					default:
						Console.Error.WriteLine ("Unknown arguments, -h for argument list");
						break;
					}
					break;

				default:
					Console.WriteLine ("Unknown argument, type -h for argument list");
					break;
				}
				break;

			default:
				Console.Error.WriteLine ("Argument is invalid, type -h for more informations");
				break;
			}
			database.SaveChanges ();
		}

        private static Database.ExcludedFiles ExcludeFileExist(string file)
		{
			var result = from sp in database.ExcludedFiles
			                      where sp.Filename == file
			                      select sp;
			if (result.Count () <= 0) {
				return null;
			} else {
				return result.First ();
			}
		}

        private static void ExcludeFileRemove(string file)
		{
			var result = from sp in database.ExcludedFiles
			                      where sp.Filename == file
			                      select sp;
			if (result.Count () <= 0) {
				return;
			}
			database.ExcludedFiles.Remove (result.First ());
		}

        private static void ReleaseDirectoryRemove(string fullName)
		{
			var result = from sp in database.ReleaseDirectories
			                      where sp.ReleaseDirectoryLocation == fullName
			                      select sp;
			if (result != null) {
				database.ReleaseDirectories.Remove (result.First ());
			}
		}
		private static void ReleaseDirectoryRemove(int Id){
			var result = from sp in database.ReleaseDirectories
			             where sp.Id == Id
			             select sp;
			if (result != null) {
				database.ReleaseDirectories.Remove (result.First ());
			}
		}

        private static Database.RelaseDirectoy ReleaseDirectoryExist(string fullName)
		{
			var result = from sp in database.ReleaseDirectories
			                      where sp.ReleaseDirectoryLocation == fullName
			                      select sp;
			if (result.Count () <= 0) {
				return null;
			}

			return result.First ();
		}
		private static Database.RelaseDirectoy ReleaseDirectoryExist(int Id){
			var result = from sp in database.ReleaseDirectories
					where sp.Id == Id
			             select sp;
			if (result.Count () <= 0) {
				return null;
			}

			return result.First ();
		}

		private static IEnumerable<Database.CommandRun> GetCommandRunById(int commandrunId)
		{
			var result = from sp in database.ReleaseDirectories
			                      where sp.CommandRunId == commandrunId
			                      select sp;
			if (result.Count () <= 0) {
				return null;
			}
			var result2 = from sp in database.CommandsRun
			                       where sp.Id == result.First ().CommandRunId
			                       select sp;
			if (result2.Count () <= 0) {
				return null;
			}
			return result2;
		}

        public static void AddCommandRun(Database.RelaseDirectoy releasedirectory, params Database.CommandRun[] commandparams)
		{
			foreach (var item in commandparams) {
				database.CommandsRun.Add (new Database.CommandRun (item.Command, item.ExecuteEvent, releasedirectory.CommandRunId));
			}
		}
		#if WIN
        [System.Runtime.InteropServices.DllImport("wininet.dll")]
        private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);

        public static bool CheckNet()
        {
            int desc;
            return InternetGetConnectedState(out desc, 0);
		}
		#endif
	}
}