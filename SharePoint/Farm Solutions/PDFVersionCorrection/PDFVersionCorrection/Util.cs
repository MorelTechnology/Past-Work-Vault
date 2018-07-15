using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using System.Diagnostics;
using System.IO;

namespace PDFVersionCorrection
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
        /// <param name="message">The string message to write</param>
        /// <param name="errorLevel">The level of error that should be logged</param>
        public static void LogError(String message, Util.ErrorLevel errorLevel)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
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
                    var appLog = new EventLog { Source = "PDF Correction Utility" };
                    appLog.WriteEntry(message, logErrorLevel, 42);
                }
                catch (Exception e)
                {
                    Microsoft.Office.Server.Diagnostics.PortalLog.LogString("PDF Correction Utility error occurred: " + e.Message + " Original message: " + message);
                }
            });
        }

        public static bool InstantiateLicense(SPWeb web)
        {
            bool boolSuccess = false;
            try
            {
                Aspose.Pdf.License pdfLicense = new Aspose.Pdf.License();
                using (var stream = web.GetFile("License/Aspose.Total.lic").OpenBinaryStream())
                {
                    pdfLicense.SetLicense(stream);
                }
                boolSuccess = true;
            }
            catch (Exception ex)
            {
                boolSuccess = false;
                LogError(ex.Message);
            }
            return boolSuccess;
        }
    }
}
