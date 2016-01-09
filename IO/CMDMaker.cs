//
//  CMDMaker.cs
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


#if WIN

using System;
using System.IO;

namespace Packager.IO
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