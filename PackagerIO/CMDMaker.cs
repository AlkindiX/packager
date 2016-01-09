#if WIN

using System;
using System.IO;

namespace Packager.PackagerIO
{
    public sealed class CMDMaker
    {
        public const string cmd_show_gui =
@"
REM Show Packager GUI window for Windows platform.
echo off
echo .
cls
.\Packager.exe -s --g
";

        public const string cmd_show_gui_name = "Packager GUI.cmd";
        public const string cmd_show_help_name = "Packager help.cmd";

        public const string cmd_show_help =
@"
REM Show Packager help command line
echo off
echo .
cls
.\Packager.exe -h
pause
";

        public static void CreateCMDFiles()
        {
            if (!File.Exists(cmd_show_gui_name))
            {
                File.WriteAllText(cmd_show_gui_name, cmd_show_gui);
            }
            if (!File.Exists(cmd_show_help_name))
            {
                File.WriteAllText(cmd_show_help_name, cmd_show_help);
            }
            Console.WriteLine("You can access the Packager easily. Click \"" + cmd_show_help_name + "\" to show GUI setting, Click \"" + cmd_show_help_name + "\" to show help");
        }
    }
}

#endif