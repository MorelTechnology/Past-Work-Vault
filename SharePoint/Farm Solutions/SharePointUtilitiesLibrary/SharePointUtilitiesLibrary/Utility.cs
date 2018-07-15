using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsToOranges
{
    public class Utility
    {
        /// <summary>
        /// Windows Event Logger.
        /// </summary>
        public class EventLogger
        {
            private string source;
            private string logType;

            public EventLogger(string source, string logType)
            {
                this.source = source;
                this.logType = logType;
            }

            public void addError(string message)
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, logType);
                EventLog.WriteEntry(source, message, EventLogEntryType.Error);
            }

            public void addInformation(string message)
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, logType);
                EventLog.WriteEntry(source, message, EventLogEntryType.Information);
            }

            public void addWarning(string message)
            {
                if (!EventLog.SourceExists(source))
                    EventLog.CreateEventSource(source, logType);
                EventLog.WriteEntry(source, message, EventLogEntryType.Warning);
            }

            public string parseException(Exception ex)
            {
                string message =
                    "Exception type " + ex.GetType() + Environment.NewLine +
                    "Exception message: " + ex.Message + Environment.NewLine +
                    "Stack trace: " + ex.StackTrace + Environment.NewLine;
                if (ex.InnerException != null)
                {
                    message += "---BEGIN InnerException--- " + Environment.NewLine +
                               "Exception type " + ex.InnerException.GetType() + Environment.NewLine +
                               "Exception message: " + ex.InnerException.Message + Environment.NewLine +
                               "Stack trace: " + ex.InnerException.StackTrace + Environment.NewLine +
                               "---END Inner Exception";
                }
                return message;
            }
            public void sendEmailMessage(string smtpRelayServer, int smtpPortNumber, string from, string to, string subject, string body)
            {
                new System.Net.Mail.SmtpClient(smtpRelayServer, smtpPortNumber).Send(from, to, subject, body);
            }
        }
    }
}
