//
//  Context.cs
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


using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using Packager.Database.Tables;

namespace Packager.Database
{
    /// <summary>
    /// Packager database.
    /// </summary>
    public class PackagerDatabase : SQLiteConnection
    {
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Packager.Database.PackagerDatabase"/> class.
        /// </summary>
        /// <param name="location">Location.</param>
        public PackagerDatabase(string location)
            : base(location)
        {
            InitVars();
            InitData();
            
        }
        /// <summary>
        /// Save all database changes from all tables.
        /// </summary>
        public void SaveChanges()
        {
            
            BeginTransaction ();
            DropAllTables();
            CreateTables();
            this.InsertAll(ReleaseDirectories);
            this.InsertAll(ExcludedFiles);
            this.InsertAll(CommandsRun);
            this.InsertAll(Logging);
            this.InsertAll(Configuration);

            Commit ();
        }
        /// <summary>
        /// Drops all tables.
        /// </summary>
        public void DropAllTables()
        {
            this.DropTable<RelaseDirectoy>();
            this.DropTable<ExcludedFiles>();
            this.DropTable<CommandRun>();
            this.DropTable<Logging>();
            this.DropTable<Configuration>();
        }
        /// <summary>
        /// Rejects the changes.
        /// </summary>
        public void RejectChanges()
        {
            InitVars();
            InitData();
        }
        /// <summary>
        /// Inits the data.
        /// </summary>
        private void InitData()
        {
            ReleaseDirectories = Table<RelaseDirectoy>().ToList();
            ExcludedFiles = Table<ExcludedFiles>().ToList();
            CommandsRun = Table<CommandRun>().ToList();
            Logging = Table<Logging>().ToList();
            Configuration = Table<Configuration>().ToList();
        }
        /// <summary>
        /// Creates tables if they are not exist.
        /// </summary>
        private void CreateTables()
        {
            try
            {
                CreateTable<RelaseDirectoy>();
                CreateTable<ExcludedFiles>();
                CreateTable<CommandRun>();
                CreateTable<Logging>();
                CreateTable<Configuration>();
            }
#if WIN
            catch (SQLiteException sqlex) 
            {
#elif LINUX
			catch (SQLiteException sqlex) // This error refer that the database file is damaged, encrypted or not sql file
            {
#endif
                Console.Error.WriteLine("Database loading failed");
                Console.Error.WriteLine("The \"{0}\" is damaged, not a SQLite data file. Please delete it or restore the last backup\n", this.DatabasePath);
                Console.Error.WriteLine(sqlex.ToString());
                Environment.Exit(sqlex.HResult);
            }
        }

        private void InitVars()
        {
            BeginTransaction ();
            CreateTables();
            ReleaseDirectories = new List<RelaseDirectoy>();
            ExcludedFiles = new List<ExcludedFiles>();
            CommandsRun = new List<CommandRun>();
            Logging = new List<Logging>();
            Configuration = new List<Tables.Configuration>();
            Commit ();
        }
        /// <summary>
        /// Gets or sets the release directories.
        /// </summary>
        /// <value>The release directories.</value>
        public List<RelaseDirectoy> ReleaseDirectories { get; protected set; }
        /// <summary>
        /// Gets or sets the excluded files.
        /// </summary>
        /// <value>The excluded files.</value>
        public List<ExcludedFiles> ExcludedFiles { get; protected set; }
        /// <summary>
        /// Gets or sets the commands run.
        /// </summary>
        /// <value>The commands run.</value>
        public List<CommandRun> CommandsRun { get; protected set; }
        /// <summary>
        /// Gets or sets the logging.
        /// </summary>
        /// <value>The logging.</value>
        public List<Logging> Logging { get; protected set; }
        /// <summary>
        /// Gets or sets the Configuration.
        /// </summary>
        public List<Configuration> Configuration { get; protected set; }
    }



}