using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AcuitySharePointConduitService
{
    public partial class AcuitySharePointConduitService : ServiceBase
    {
        public AcuitySharePointConduitService()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            // JMM: This allows us to debug the service without needing to re-register as a windows service every time.
            // (OnStart can't be called directly because it's protected.)
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            
            DataReader datareader = new DataReader();
            EventLogger log = new EventLogger();
            if (Properties.Settings.Default.EnableTaskMonitoring)
            {
                try
                {
                    datareader.monitorIncomingTasks();
                    log.addInformation("Acuity SharePoint Conduit Service", "Application", "Monitoring "
                        + Properties.Settings.Default.DataFilePath + " for new Task feeds matching filemask '" 
                        + Properties.Settings.Default.TasksFilenameMask + "'.");
                }
                catch (Exception ex)
                {
                    log.addError("Acuity SharePoint Conduit Service", "Application", "Exception encountered while attempting to monitor for new tasks: " + ex.ToString());
                }
            }

            if (Properties.Settings.Default.EnableAlertMonitoring)
            {
                try
                {
                    datareader.monitorIncomingAlerts();
                    log.addInformation("Acuity SharePoint Conduit Service", "Application", "Monitoring "
                        + Properties.Settings.Default.DataFilePath + " for new Alert feeds matching filemask '"
                        + Properties.Settings.Default.AlertsFilenameMask + "'.");

                }
                catch (Exception ex)
                {
                    log.addError("Acuity SharePoint Conduit Service", "Application", "Exception encountered while attempting to monitor for new Alerts: " + ex.ToString());
                }
                
            }

            if (Properties.Settings.Default.EnableEventMonitoring)
            {
                try
                {
                    datareader.monitorIncomingEvents();
                    log.addInformation("Acuity SharePoint Conduit Service", "Application", "Monitoring "
                        + Properties.Settings.Default.DataFilePath + " for new Event feeds matching filemask '"
                        + Properties.Settings.Default.EventsFilenameMask + "'.");

                }
                catch (Exception ex)
                {
                    log.addError("Acuity SharePoint Conduit Service", "Application", "Exception encountered while attempting to monitor for new Events: " + ex.ToString());
                }
                
            }
        }

        protected override void OnStop()
        {
           // Stopped.
        }
    }
}
