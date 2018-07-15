using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AcuitySharePointConduitService
{
    public class EventLogger
    {
        /**
         * Created 10/1/2013 - Jeremy Morel, AppsToOranges, LLC
         * Quick and Dirty Windows Event log Writer
         * **/
        public void addError(string source, string log, string message)
        {
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, log);
            EventLog.WriteEntry(source, message, EventLogEntryType.Error);
        }

        public void addInformation(string source, string log, string message)
        {
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, log);
            EventLog.WriteEntry(source, message, EventLogEntryType.Information);
        }

        public void addWarning(string source, string log, string message)
        {
            if (!EventLog.SourceExists(source))
                EventLog.CreateEventSource(source, log);
            EventLog.WriteEntry(source, message, EventLogEntryType.Warning);
        }

    }
}
