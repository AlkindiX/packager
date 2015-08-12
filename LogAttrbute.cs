using Common.Logging;
using Microsoft.VisualBasic;
using Packager.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packager
{
    internal class LogAttrbute
    {
        private static Logging _log;

        public static Logging log
        {
            get
            {
                if ((object)_log == null)
                {
                    DirectoryInfo logdir = new DirectoryInfo("Log");
                    if (!logdir.Exists)
                    {
                        logdir.Create();
                    }
                    var log_session_file_name = Path.ChangeExtension(Path.Combine(logdir.FullName, (
                        DateAndTime.Now.Year.ToString() +
                        "_" +
                        DateAndTime.Now.Month.ToString() +
                        "_" +
                        DateAndTime.Now.Day.ToString() +
                        "-" +
                        DateAndTime.Now.Hour.ToString() +
                        "_" +
                        DateAndTime.Now.Minute.ToString()
                        )), "log");
                    _log = new Common.Logging.Logging(log_session_file_name);
                    _log.AutoSave = false;
                    _log.AutoSaveInterval = 1;
                    _log.StopLogging = Settings.Default.EnableLog;
                    _log.AssertToConsoleIO = false;
                }
                return _log;
            }
        }
    }
}