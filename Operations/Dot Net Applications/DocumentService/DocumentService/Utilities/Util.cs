using DocumentService.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DocumentService.Utilities
{
    public class Util
    {
        #region Public Enums

        public enum ErrorLevel
        {
            Info,
            Warning,
            Error
        }

        #endregion Public Enums

        #region Public Methods

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
        /// <param name="message">The string message to write</param>
        /// <param name="errorLevel">The level of error that should be logged</param>
        public static void LogError(String message, Util.ErrorLevel errorLevel)
        {
            // Map the event log entry type to the error level
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
                // Write to the application log
                var appLog = new EventLog { Source = "DocumentService" };
                appLog.WriteEntry(message, logErrorLevel, 42);
            }
            catch
            {
                // Can't log anything, because we, uh, can't log anything.
            }
        }

        /// <summary>
        /// Uses the MIMETypes class to match extensions to known MimeTypes from Guidewire. If changes are made to the document name, the new document URL is returned
        /// </summary>
        /// <param name="documentUrl">The URL of the document to resolve</param>
        /// <param name="mimeType">The MimeType as it exists in ClaimCenter</param>
        /// <returns></returns>
        public static String ResolveMissingExtension(string documentUrl, string mimeType)
        {
            List<string> extension = new List<string>();

            //if (Path.HasExtension(newDocUrl)) return newDocUrl;

            // Get the list of known mime types/extensions
            using (MIMETypes mimetypes = new MIMETypes())
            {
                if (mimetypes.ContainsMimetype(mimeType))
                    extension = mimetypes.ExtensionMapping[mimeType];
            }

            //TODO - Add logic to update the document name in the Content Server so that this doesn't happen again.

            return documentUrl + extension.First();
        }

        #endregion Public Methods
    }
}