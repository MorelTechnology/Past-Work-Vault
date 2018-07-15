using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;

namespace TimeOffCalendarSync.Features.TimeOffCalendarSync
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("1b989a3a-5a9b-47e4-8fa7-578c6e8fe11c")]
    public class TimeOffCalendarSyncEventReceiver : SPFeatureReceiver
    {
        const string JobName = "Time Off Calendar Synchronization";
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            /* Create the Time Off Calendar Sync Job */
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                        DeleteExistingJob(JobName, webApp);
                        CreateTimeOffCalendarSyncJob(webApp);
                });
            }
            catch (Exception ex)
            {
                Util.LogError("Error activating Time Off Calendar Sync job with message: " + ex.Message);
            }
        }

        private bool CreateTimeOffCalendarSyncJob(SPWebApplication webApp)
        {
            bool jobCreated = false;
            try
            {
                CalendarSyncJob job = new CalendarSyncJob(JobName, webApp);
                SPMinuteSchedule schedule = new SPMinuteSchedule();
                schedule.BeginSecond = 0;
                schedule.Interval = 15;
                job.Schedule = schedule;

                job.Update();
                jobCreated = true;
            }
            catch (Exception ex)
            {
                Util.LogError("Error creating time off calendar sync job: " + ex.Message);
            }
            return jobCreated;
        }

        private bool DeleteExistingJob(string jobName, SPWebApplication webApp)
        {
            bool jobDeleted = false;
            try
            {
                foreach (SPJobDefinition job in webApp.JobDefinitions)
                {
                    if (job.Name == jobName)
                    {
                        job.Delete();
                        jobDeleted = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogError("DeleteExistingJob failed with message: " + ex.Message);
            }
            return jobDeleted;
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            try
            {
                SPSecurity.RunWithElevatedPrivileges(delegate()
                {
                    SPWebApplication webApp = properties.Feature.Parent as SPWebApplication;
                    DeleteExistingJob(JobName, webApp);
                });
            }
            catch (Exception ex)
            {
                Util.LogError("Error deactivating activating Time Off Calendar Sync job with message: " + ex.Message);
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
