//
//  HelpCommand.cs
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


using System;

namespace Packager.IO
{
    /// <summary>
    /// Help command.
    /// </summary>
    public sealed class HelpCommand
    {
        /// <summary>
        /// Intro this instance.
        /// </summary>
        public static void Intro()
        {
            ShowVersion();
            Console.WriteLine("Packager  Copyright (C) 2015 by Mohamed Alkindi\n\tThis program comes with ABSOLUTELY NO WARRANTY; for details type `-l'.\n\tThis is free software, and you are welcome to redistribute it\n\tunder certain conditions.");
            Console.WriteLine();
        }
        /// <summary>
        /// Shows the version.
        /// </summary>
        public static void ShowVersion()
        {
            Tools.ConsoleColor.Write(ConsoleColor.Blue,"Packager v" + typeof(Program).Assembly.GetName().Version.ToString());
            string os_distribution;
#if LINUX
			os_distribution = " - Linux, powered by Mono";
#elif WIN
            os_distribution = " - Windows, powered by Microsoft .NET freamwork";
#endif
            Console.Write(os_distribution);
            Console.WriteLine();
        }
        /// <summary>
        /// Shows the help for command line
        /// </summary>
        public static void ShowHelp()
        {
            Console.WriteLine(
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
  ---add[directory name]  :Add release directory location,
     [zip name]           :zip name is the output zipped file
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

                + @"                          :
 --log [true|false]       :Enable Logging" +
#elif LINUX
				+ "                          :" +
#endif
                @"
                          :
 --clear                  :Clear all settings
                          :"
                + @"LICENCE ARGUMENTS
-l                        :Show license arguments
  --g                     :Show license argument in graphical user interface window

" +
                @"EXAMPLES
"
                + @"  -s --g                                    :Show GUI setting" +

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
        }
    }
}