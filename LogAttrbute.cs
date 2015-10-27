using Common.Logging;
#if WIN
using Microsoft.VisualBasic;
#endif
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
						DateTime.Now.Year.ToString() +
                        "_" +
						DateTime.Now.Month.ToString() +
                        "_" +
						DateTime.Now.Day.ToString() +
                        "-" +
						DateTime.Now.Hour.ToString() +
                        "_" +
						DateTime.Now.Minute.ToString()
                        )), "log");

                    _log = new Common.Logging.Logging(log_session_file_name);
                    _log.AutoSave = false;
                    _log.AutoSaveInterval = 1;
					#if WIN
                    _log.StopLogging = Settings.Default.EnableLog;
					#elif LINUX
					_log.StopLogging = false;
					#endif
                    _log.AssertToConsoleIO = false;
                }
                return _log;
            }
        }
    }
}
