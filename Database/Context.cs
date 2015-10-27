using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packager.Database
{
    public class PackagerDatabase : SQLiteConnection
    {
        public PackagerDatabase(string location) : base(location)
        {
            InitVars();
            InitData();
        }

        public void SaveChanges()
        {
			DropAllTables ();
			CreateTables ();
            this.InsertAll(ReleaseDirectories);
            this.InsertAll(ExcludedFiles);
            this.InsertAll(CommandsRun);
        }
		public void DropAllTables(){

			this.DropTable<RelaseDirectoy>();
			this.DropTable<ExcludedFiles>();
			this.DropTable<CommandRun>();
		}
        public void RejectChanges()
        {
            InitVars();
            InitData();
        }

        private void InitData()
        {
            ReleaseDirectories = Table<RelaseDirectoy>().ToList();
            ExcludedFiles = Table<ExcludedFiles>().ToList();
            CommandsRun = Table<CommandRun>().ToList();
        }
		private void CreateTables(){
			try
			{

				CreateTable<RelaseDirectoy>();
				CreateTable<ExcludedFiles>();
				CreateTable<CommandRun>();
			}
			#if WIN
			catch (SQLiteException sqlex) when(sqlex.HResult == -2146233088) // This error refer that the database file is damaged, encrypted or not sql file 
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
			CreateTables ();
			ReleaseDirectories = new List<RelaseDirectoy> ();
			ExcludedFiles = new List<Database.ExcludedFiles> ();
			CommandsRun = new List<CommandRun> ();
		}

        public List<RelaseDirectoy> ReleaseDirectories { get; protected set; }
        public List<ExcludedFiles> ExcludedFiles { get; protected set; }
        public List<CommandRun> CommandsRun { get; protected set; }
            }
    [Table("RelaseDirectories")]
    public class RelaseDirectoy
    {
        [PrimaryKey(), AutoIncrement()]
        public int Id { get; set; }

        [Column("ReleaseDirectoryLocation")]
        public string ReleaseDirectoryLocation { get; set; }
        public int CommandRunId { get; set; }
        public RelaseDirectoy(string releaselocation,int commandrunid)
        {
            ReleaseDirectoryLocation = releaselocation;
            CommandRunId = commandrunid;
        }

        public RelaseDirectoy()
        {
        }
    }

    public class ExcludedFiles
    {
        [PrimaryKey(), AutoIncrement()]
        public int Id { get; set; }

        [Column("Filename")]
        public string Filename { get; set; }

        public ExcludedFiles(string file)
        {
            Filename = file;
        }

        public ExcludedFiles()
        {
        }
    }
    [Table("CommandBeforeRun")]
    public class CommandRun
    {
        [PrimaryKey(),AutoIncrement()]
        public int Id { get; set; }
        public int uniqueId { get; set; }
        public string Command { get; set; }
        public ExecuteEvent ExecuteEvent { get; set; }
        public CommandRun(string commandstring, ExecuteEvent excecuteevent,int uniqueid) {
            Command = commandstring;
            ExecuteEvent = excecuteevent;
        }
        public CommandRun() { }
    }

}