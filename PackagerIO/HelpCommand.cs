using System;

namespace Packager
{
    public sealed class HelpCommand
    {
        public static void Intro()
        {
            ShowVersion();
            Console.WriteLine("Packager  Copyright (C) 2015 by Mohamed Alkindi\n\tThis program comes with ABSOLUTELY NO WARRANTY; for details type `-l'.\n\tThis is free software, and you are welcome to redistribute it\n\tunder certain conditions.");
            Console.WriteLine();
        }

        public static void ShowVersion()
        {
            Console.Write("Packager v" + typeof(Program).Assembly.GetName().Version.ToString());
            string os_distribution;
#if LINUX
			os_distribution = " - Linux, powered by Mono";
#elif WIN
            os_distribution = " - Windows, powered by Microsoft .NET freamwork";
#endif
            Console.Write(os_distribution);
            Console.WriteLine();
        }

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
#if WIN
                + @"LICENCE ARGUMENTS
-l                        :Show license arguments
  --g                     :Show license argument in graphical user interface window

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
        }
    }
}