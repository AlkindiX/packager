//
//  Program.cs
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
using Packager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Permissions;
using Packager.IO;
namespace Packager
{
    internal class Program
    {

        [STAThread]
        public static void Main(string[] args)
        {
            PackagerWorker worker = new PackagerWorker ("Output", "setting.db");
            // Check if there are any arguments
            // Load Application Data
            if (args.Length > 0)
            {
                // No argument
                HelpCommand.ShowVersion();

                CommandLineArgument.ArgumnentsAhead(args, ref worker._Database, ref worker);
#if WIN
                Settings.Default.Save();
#endif
                return;
            }
            else
            {
                HelpCommand.Intro();
            }
            // Check release location if it null throw error
#if WIN
            Packager.IO.CMDMaker.CreateCMDFiles();
#endif

            if (worker.IsNewDatabase())
            {
                Console.WriteLine("Welcome to release maker.\n" +
                "If this is your first time. Please type -h for more information");
                Console.Error.WriteLine("The release directory list is empty, please enter -h for instructions");

                return;
            }
            Console.WriteLine("Starting release maker");
            Console.WriteLine();
            worker.StartPackaging ();
            worker.CreateDatabaseBackup();
            worker.CloseDatabase ();

        }
    }
}