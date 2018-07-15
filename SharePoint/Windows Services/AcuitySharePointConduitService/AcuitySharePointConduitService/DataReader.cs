using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AcuitySharePointConduitService
{
    class DataReader
    {
        public EventLogger log = new EventLogger();
        private enum SharePointOperation { add, delete, update }

        private string acuityMatterId { get; set; }
        private string acuityMatterName { get; set; }
        private string alertId { get; set; }
        private int count { get; set; }
        private string docketId { get; set; }
        private string emailAddress { get; set; }
        private string litigationMatterId { get; set; }
        private string title { get; set; }
        private string type { get; set; }
        private string url { get; set; }
        private string workMatterId { get; set; }



        public void sendTasksToSharePoint(string filename)
        {
            SharePointConnector sp = new SharePointConnector();
            XDocument xdoc = XDocument.Load(filename);

            log.addInformation("Acuity SharePoint Conduit Service", "Application", "Processing " + xdoc.Descendants("Task").Count()
                + " tasks identified in file " + filename + " to list " + Properties.Settings.Default.SPSiteURL + Properties.Settings.Default.TasksListName);

            foreach (XElement xe in xdoc.Descendants("Task"))
            {
                workMatterId = xe.Element("WorkMatterID").Value;
                litigationMatterId = xe.Element("LitigationMatterID").Value;
                docketId = xe.Element("DocketID").Value;
                emailAddress = xe.Element("EmailAddress").Value;
                type = xe.Element("Type").Value;
                count = Convert.ToInt32(xe.Element("Count").Value);
                url = xe.Element("URL").Value;
                sp.addTask(filename.ToString(), type, count, emailAddress, url, null, workMatterId, litigationMatterId, docketId);
            }
        }

        public void sendAlertsToSharePoint(string filename)
        {
            SharePointConnector sp = new SharePointConnector();

            XDocument xdoc = XDocument.Load(filename);
            log.addInformation("Acuity SharePoint Conduit Service", "Application", "Processing " + xdoc.Descendants("AlertRecord").Count()
                + " Alerts identified in file " + filename +" to list " + Properties.Settings.Default.SPSiteURL + Properties.Settings.Default.AlertsListName);
            foreach (XElement xe in xdoc.Descendants("AlertRecord"))
            {
                acuityMatterId = xe.Element("WorkMatterID").Value;
                alertId = xe.Element("AlertId").Value;
                acuityMatterName = xe.Element("AcuityMatterName").Value;
                workMatterId = xe.Element("WorkMatterID").Value;
                litigationMatterId = xe.Element("LitigationMatterID").Value;
                docketId = xe.Element("DocketID").Value;
                emailAddress = xe.Element("EmailAddress").Value;
                type = xe.Element("AlertType").Value;
                title = xe.Element("AlertTitle").Value;
                url = xe.Element("URL").Value;
                sp.addAlert(emailAddress, acuityMatterName, title);
            }
        }

        public void sendEventsToSharePoint(string filename)
        {
            throw new NotImplementedException();
        }


        public void monitorIncomingTasks()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Properties.Settings.Default.DataFilePath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.Filter = Properties.Settings.Default.TasksFilenameMask;
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public void monitorIncomingAlerts()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Properties.Settings.Default.DataFilePath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.Filter = Properties.Settings.Default.AlertsFilenameMask;
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        public void monitorIncomingEvents()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Properties.Settings.Default.DataFilePath;
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName;
            watcher.Filter = Properties.Settings.Default.EventsFilenameMask;
            watcher.Created += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            var isReady = false;
            while (!isReady) { isReady = IsFileReady(e.FullPath); } // ensure we're not messing with a file until the copy is complete.


            if (FitsMask(e.Name.ToString(), Properties.Settings.Default.AlertsFilenameMask))
            {
                // this is an alertXML
                sendAlertsToSharePoint(e.FullPath);
            }

            if (FitsMask(e.Name.ToString(), Properties.Settings.Default.TasksFilenameMask))
            {
                //this is a taskXML
                sendTasksToSharePoint(e.FullPath);
            }

            if (FitsMask(e.Name.ToString(), Properties.Settings.Default.EventsFilenameMask))
            {
                //this is an EventXML
                sendEventsToSharePoint(e.FullPath);
            }

        }

        private bool FitsMask(string sFileName, string sFileMask)
        {
            Regex mask = new Regex(sFileMask.Replace(".", "[.]").Replace("*", ".*").Replace("?", "."));
            return mask.IsMatch(sFileName);
        }

        private bool IsFileReady(String sFilename)
        {
            // If the file can be opened for exclusive access it means that the file
            // is no longer locked by another process.
            try
            {
                using (FileStream inputStream = File.Open(sFilename, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    if (inputStream.Length > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
