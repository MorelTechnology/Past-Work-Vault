using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using System.Diagnostics;

namespace TimeOffCalendarSync
{
    class Util
    {
        public enum ErrorLevel
        {
            Info,
            Warning,
            Error
        }

        /// <summary>
        /// Write message to Event Viewer Application log as Error
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(String message)
        {
            LogError(message, ErrorLevel.Error);
        }

        /// <summary>
        /// Write message to Event Viewer Application log as Error, Warning or Information
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorLevel"></param>
        public static void LogError(String message, Util.ErrorLevel errorLevel)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                EventLogEntryType logErrorLevel;
                switch (errorLevel)
                {
                    case ErrorLevel.Error:
                        logErrorLevel = EventLogEntryType.Error;
                        break;
                    case ErrorLevel.Info:
                        logErrorLevel = EventLogEntryType.Information;
                        break;
                    case ErrorLevel.Warning:
                        logErrorLevel = EventLogEntryType.Warning;
                        break;
                    default:
                        logErrorLevel = EventLogEntryType.Error;
                        break;
                }
                try
                {
                    var appLog = new EventLog { Source = Resources.ErrorSource };
                    appLog.WriteEntry(message, logErrorLevel, 42);
                }
                catch (Exception e)
                {
                    Microsoft.Office.Server.Diagnostics.PortalLog.LogString("Guidewire Integration errored: " + e.Message + " Original message: " + message);
                }
            });
        }
    }
}
