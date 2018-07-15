using System;
using System.Diagnostics;
using System.Xml.Linq;
using System.Configuration;

namespace RenameSapiensCashReceiptDocs
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
                Console.WriteLine(message);
                var appLog = new EventLog { Source = ConfigurationManager.AppSettings["LogSource"] };
                appLog.WriteEntry(message, logErrorLevel, 42);
            }
            catch (Exception e)
            {
                //Unable to write to event log
            }
        }

        public static string GetViewQuery()
        {
            XDocument fileXml = XDocument.Load(@"ViewQuery.xml");
            return fileXml.ToString(SaveOptions.DisableFormatting);
        }
    }
}
