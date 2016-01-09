//
//  PackagerWorker.cs
//
//  Author:
//       Mohammed Alkindi <mohammedalkindi2015@yahoo.com>
//
//  Copyright (c) 2016 mohammed
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
using Packager.Database.Tables;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Packager.IO
{
    /// <summary>
    /// The place that all works are comming here
    /// </summary>
     public class PackagerWorker
    {
        /// <summary>
        /// Gets the name of the database.
        /// </summary>
        /// <value>The name of the database.</value>
        public string DatabaseName
        {
            get;
            private set;
        }
        /// <summary>
        /// Database only var for ref
        /// </summary>
        public Database.PackagerDatabase _Database;
        /// <summary>
        /// Gets the database.
        /// </summary>
        /// <value>The database.</value>
        public Database.PackagerDatabase Database
        {
            get
            {
                return _Database;
            }
        }
        /// <summary>
        /// Gets or sets the output dir.
        /// </summary>
        /// <value>The output dir.</value>
        public string OutputDir
        {
            get;
            protected set;
        }
        /// <summary>
        /// Determines whether this instance is new database.
        /// </summary>
        /// <returns><c>true</c> if this instance is new database; otherwise, <c>false</c>.</returns>
        public bool IsNewDatabase()
        {
            if (Database.ReleaseDirectories.Count <= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Start the operation
        /// </summary>
        public void StartPackaging()
        {
            DirectoryInfo outputdir;
            List<string> releasefiles;
            string old_output_path;

            outputdir = new DirectoryInfo(Path.Combine(OutputDir));
            releasefiles = new List<string>();
            old_output_path = Path.Combine(
                outputdir.FullName,
                "Old releases",
                string.Join("-",
                    DateTime.Now.Year,
                    DateTime.Now.Month,
                    DateTime.Now.Day,
                    DateTime.Now.Hour,
                    DateTime.Now.Minute,
                    DateTime.Now.Second)
            );

            foreach (var itemdir in Database.ReleaseDirectories)
            {
                DirectoryInfo info;
                IList<string> files;
                this.WorkerCommandWriter(itemdir.Id, string.Format("Processing release directory \"{0}\"", itemdir.ReleaseDirectoryLocation));
                // Get directory info where the files are in and get them
                info = new DirectoryInfo(itemdir.ReleaseDirectoryLocation);
                if (!info.Exists)
                {
                    this.WorkerCommandWriter(itemdir.Id, string.Format("The directory \"{1}\" is not exist on the disk \"{0}\"", info.FullName));
                    continue;
                }
                files = new List<string>();
                // Get all files
                foreach (var item in info.GetFiles())
                {
                    files.Add(item.FullName);
                }
                // Define zip name
                var releasezipname = itemdir.ReleaseName;
                var f = Database.ExcludedFiles;
                var releasefile = Path.Combine(outputdir.FullName, releasezipname);
                f.Add(new ExcludedFiles("release.zip"));
                f.Add(new ExcludedFiles(releasezipname));
                /*
                * Excluding files that user select them whether if they are not needed
                */
                foreach (var item in f)
                {
                    foreach (string item2 in files)
                    {
                        if (item2.Contains(item.Filename))
                        {
                            this.WorkerCommandWriter(itemdir.Id, string.Format("Excluding \"" + item2 + "\"", itemdir.Id));
                            files.Remove(item2);
                            break;
                        }
                    }
                }
                releasefiles.Add(releasefile);
                // Deprecated: No longer needed!
                if (File.Exists(releasezipname))
                {
                    this.WorkerCommandWriter (itemdir.Id, "Release file is exist");
                }

                if (!outputdir.Exists)
                {
                    outputdir.Create();
                }
                if (File.Exists(releasefile))
                {
                    // if not old_output_path exist, create it
                    if (!Directory.Exists(old_output_path))
                    {
                        Directory.CreateDirectory(old_output_path);
                    }
                    File.Move(releasefile, Path.Combine(old_output_path, Path.GetFileName(releasefile)));
                }
                // Create the zip file
                Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(releasefile);
                foreach (var file in files)
                {
                    if (zip.ContainsEntry(Path.GetFileName(file)))
                    {
                        // !Deprecated: Not needed anymore!
                        zip.RemoveEntry(Path.GetFileName(file));
                        this.WorkerCommandWriter (itemdir.Id, string.Format ("Removing from zip entry \"" + Path.GetFileName(file) + "\"", itemdir.Id));
                    }
                    zip.AddFile(file, "/");
                    this.WorkerCommandWriter (itemdir.Id, " Adding to zip \"" + Path.GetFileName (file) + "\"");
                }
                    this.WorkerCommandWriter(itemdir.Id, "Creating zip file...");
                try
                {
                    zip.Save();
                }
                catch (UnauthorizedAccessException)
                {
                    // LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Exception, sx.ToString());
                    // LogAttrbute.log.LogFast(Common.Logging.Logging.LogFastInfo.Error, "Cannot add the zip file into the directory because you do not have the permission to write the file");
                    Console.Error.WriteLine("[{0}\t] Cannot add the zip file into the directory because you do not have the permission to write the file", itemdir.Id);
                    continue;
#if WIN
                }
                catch (IOException ioex) /*when (ioex.HResult == -2147024864)*/
                {
#elif LINUX
                }catch(IOException ioex)
                {
#endif
                    Console.Error.WriteLine("[{0}\t] Cannot add the zip file because {1}", itemdir.Id, ioex.Message);
                    continue;
                }
                Console.WriteLine("[{0}\t] Creating zip file successful, produced file is located in \"" + releasefile + "\"", itemdir.Id);
            }
            Console.WriteLine();
            Console.WriteLine("All your zipped files have been collected on \"{0}\"", outputdir.FullName);
            Console.WriteLine();
            Console.WriteLine("Package maker is exit, Thank you for using");
        }

        private void InitDatabase()
        {
            if (_Database == null)
            {
                var setting_location = "";
                var application_location = "";
                DirectoryInfo dir;

#if LINUX
                    application_location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal),".packager");

                    setting_location = Path.Combine(application_location, DatabaseName);

#elif WIN
                application_location = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Packager");
                setting_location = Path.Combine(application_location, DatabaseName);
#endif
                dir = new DirectoryInfo(application_location);
                if (!dir.Exists)
                {
                    dir.Create();
                }
                _Database = new Database.PackagerDatabase(setting_location);
                SQLite.SQLite3.Config (SQLite.SQLite3.ConfigOption.MultiThread);
                SQLite.SQLite3.Threadsafe ();
                

            }
        }
        private void WorkerCommandWriter(int id, string command){
            Tools.ConsoleColor.Write(ConsoleColor.DarkBlue, "[");
            Tools.ConsoleColor.Write (ConsoleColor.Blue, id);
            Console.Write ("\t");
            Tools.ConsoleColor.Write (ConsoleColor.DarkBlue, "] ");
            Tools.ConsoleColor.WriteLine (ConsoleColor.Green, command);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.IO.PackagerWorker"/> class.
        /// </summary>
        /// <param name="OutputDir">Output dir.</param>
        /// <param name="DatabaseName">Database name.</param>
        public PackagerWorker(string OutputDir, string DatabaseName)
        {
            this.OutputDir = OutputDir;
            this.DatabaseName = DatabaseName;
            InitDatabase();
            switch (CheckDatabase())
            {
                case CheckDatabaseReturn.NoSignOfBackup:
                    RestoreBackup();
                    break;
                case CheckDatabaseReturn.SignOfBackup:
                    break;
                default:
                    break;
            }
            CreateDatabaseBackup();
            CreateSignOfBackup();
        }

        private void CreateSignOfBackup()
        {
            Database.Configuration.Add(new Configuration(config_backupcreated, "true", "Important if the database have been destroyed"));
            Database.SaveChanges();
        }

        private enum CheckDatabaseReturn
        {
            NoSignOfBackup,
            SignOfBackup
        }

        private const string config_backupcreated = "BackupCreated";
        private CheckDatabaseReturn CheckDatabase()
        {
            var result = from sp in Database.Configuration
                         where sp.ConfigurationName == config_backupcreated
                         select sp;
            if (result.Count() <= 0)
            {
                return CheckDatabaseReturn.NoSignOfBackup;
            }

            return CheckDatabaseReturn.SignOfBackup;
        }
        /// <summary>
        /// Closes the database.
        /// </summary>
        public void CloseDatabase()
        {
            Database.Close();
            _Database = null;
        }
        /// <summary>
        /// Backup return.
        /// </summary>
        public enum BackupReturn
        {
            /// <summary>
            /// The backup performed successful.
            /// </summary>
            BackupSuccessful,
            /// /// <summary>
            /// The backup fail.
            /// </summary>
            BackupFail
        }
        /// <summary>
        /// Restore backup return.
        /// </summary>
        public enum RestoreBackupReturn
        {
            /// <summary>
            /// The backup restore successful.
            /// </summary>
            BackupRestoreSuccessful,
            /// /// <summary>
            /// The backup restore fail.
            /// </summary>
            BackupRestoreFail
        }
        /// <summary>
        /// Creates the database backup.
        /// </summary>
        /// <returns>The database backup.</returns>
        public BackupReturn CreateDatabaseBackup()
        {
            var databasepath = Database.DatabasePath;
            var backuppath = Path.ChangeExtension(databasepath, Path.GetExtension(databasepath) + ".backup");
            if (!File.Exists(databasepath))
            {
                return BackupReturn.BackupFail;
            }
            FileInfo info = new FileInfo(backuppath);
            if (info.Exists)
            {
                info.Delete();
            }
            info = new FileInfo(databasepath);
            info.CopyTo(backuppath);
            return BackupReturn.BackupSuccessful;
        }
        /// <summary>
        /// Restores the backup.
        /// </summary>
        /// <returns>The backup.</returns>
        public RestoreBackupReturn RestoreBackup()
        {
            var databasepath = Database.DatabasePath;
            var backuppath = Path.ChangeExtension(databasepath, Path.GetExtension(databasepath) + ".backup");
            FileInfo info = new FileInfo(backuppath);
            if (!info.Exists)
            {
                return RestoreBackupReturn.BackupRestoreFail;
            }
            CloseDatabase();
            if (File.Exists(databasepath))
            {
                File.Delete(databasepath);
            }
            info.CopyTo(databasepath);
            InitDatabase();
            return RestoreBackupReturn.BackupRestoreSuccessful;
        }
    }
}