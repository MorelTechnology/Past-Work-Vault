using MatterProvisioningLibrary;
using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using System.Timers;
using static AppsToOranges.Utility;

namespace LitMatterSyncService
{
    public partial class LitMatterSyncService : ServiceBase
    {
        private static EventLogger log = new EventLogger("Litigation Matter Synchronization Service", "Application");
        private Database.Read dataReader = new Database.Read();
        private Database.Write dataWriter = new Database.Write();
        private MatterProvisioning matterProvisioning = new MatterProvisioning(Settings.Default.LitigationMattersRootWeb);

        // This is a flag to indicate the service status
        private bool serviceStarted = false;

        // This flag indicates whether we should regenerate the template (on the first run only).
        private bool newServiceInstance = true;

        // the thread that will do the work
        Thread workerThread; 
        public LitMatterSyncService()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            newServiceInstance = false; // skips the template generation.

            // JMM: This allows us to debug the service without needing to re-register as a windows service every time.
            // (OnStart can't be called directly because it's protected.)
            OnStart(null);
        }

        internal void processMatters()
        {
            log.addInformation("Checking ClaimCenter Staging Database for matters which require processing..." + Thread.CurrentThread.Name);
            DataTable litMatters = dataReader.queryForMattersToProcess();
            if (litMatters == null || litMatters.Rows.Count == 0)
            {
                log.addInformation("There were no matters found to process." + Thread.CurrentThread.Name);
                return;
            }

            List<String> validUsers = matterProvisioning.getLMUserIDs(Settings.Default.LitigationMattersRootWeb, Settings.Default.LitigationManagementMembersGroup);
            List<Matter> mattersToProcess = matterProvisioning.mattersToProcess(litMatters, validUsers);
            int numberOfMattersSkipped = mattersToProcess.Count(x => x.IsMatterProcessed);
            
            if(mattersToProcess.Count == numberOfMattersSkipped)
            {   
                // Verify that the removal of invalid users hasn't run the well dry, and only continue if so.
                log.addInformation("After removing matters assigned to users outside of Litigation Management, there were no matters found to process." + Thread.CurrentThread.Name);
                dataWriter.markAsProcessed(mattersToProcess.Select(x => x.LMNumber).ToArray());
                return;
            }
            int numberOfSitesRequired = mattersToProcess.Count(x => x.SiteNeeded);
            int numberOfSitesNeedingUpdate = mattersToProcess.Count - (numberOfSitesRequired + numberOfMattersSkipped);
            
            // Write pre-process status to log file.
            log.addInformation("Identified a total of " + (numberOfSitesRequired + numberOfSitesNeedingUpdate) +
                " Matters which require processing. \n\n" +
                " - " + numberOfSitesRequired + " new sites will be created \n" +
                " - " + numberOfSitesNeedingUpdate + " existing sites will have their properties updated." + Thread.CurrentThread.Name);

            // Begin processing
            foreach (Matter matter in mattersToProcess)
            {
                if (matter.IsMatterProcessed) continue;
                if (matter.SiteNeeded)
                {
                    matter.IsLinkedMatter = "TRUE";  // This is used to identify whether the matter is linked to ClaimCenter
                                                     // -- and yes, it's supposed to be a string.  
                                                     // Long-story-short: SharePoint property bags are "special" hashtables
                                                     // and Microsoft advises against using [bool] in user-defined properties.
                                                     // Much more to say on the matter, but I've got miles of code before I sleep... 
                                                     // ...miles of code before I sleep...

                    processNewSite(matter);
                    matter.IsMatterProcessed = true; // no earlier exception thrown, so mark this matter as processed.
                }
                else
                {
                    // The "updatesOnly" group represents sites which have been found to already exist.
                    // As a safety measure, run a check to ensure there has not been a duplication of Litigation Matters in Claimcenter.
                    checkForDuplication(matter.LMNumber);
                    matter.IsLinkedMatter = "TRUE";
                    processUpdatesToExistingSite(matter);
                    matter.IsMatterProcessed = true; // no earlier exception thrown, so mark this matter as processed.
                }
            }

            // Update database for matters which were successfully processed.
            List<Matter> processedMatters = mattersToProcess.Where(x => x.IsMatterProcessed).ToList();
            dataWriter.markAsProcessed(processedMatters.Select(x => x.LMNumber).ToArray());

            // Write post-processing status to log file.
            if (litMatters.Rows.Count != processedMatters.Count)
            {
                log.addWarning("Warning: Database indicated " + litMatters.Rows.Count + " required " +
                    "processing, however " + processedMatters.Count + " were processed.  Any matters which were not " +
                    "processed will remain queued in the database until issue(s) are resolved.  See log for further details." + Thread.CurrentThread.Name);
            }
            else
            {
                log.addInformation("All Matters have been successfully processed." + Thread.CurrentThread.Name);
            }
        }

        protected override void OnStart(string[] args)
        {
            // Create worker thread; this will invoke the WorkerFunction
            // when we start it. Since we use a separate worker thread, the main service
            // thread will return quickly, telling Windows that service has started.
            ThreadStart threadStart = new ThreadStart(Worker);
            workerThread = new Thread(threadStart);
            // Generate some tracking details for the thread, and jam them into the thread name.
            // This might help troubleshoot, should we ever decide to run multiple instances during a large migration.
            workerThread.Name = string.Format("{0}{0}Current Thread Details:{0}" + 
                                              "{0}Thread ID: {1}" + 
                                              "{0}Operation: {2}" + 
                                              "{0}Thread running since: {3}",
                                              "\r\n",
                                              Convert.ToBase64String(Guid.NewGuid().ToByteArray()).Substring(0, 8),
                                              "Litigation Matter Synchronization Service", DateTime.Now.ToString("F") );
            // set flag to indicate worker thread is active
            serviceStarted = true; workerThread.Start();

        }

        protected override void OnStop()
        {
            serviceStarted = false; // flag to tell the worker process to stop

            // Allow 60 seconds to finish any pending work.  If still running, workerThread.Join clause will terminate it.
            log.addInformation("Service Stop request has been recieved.  Initiating a 60-second delay to allow current thread to empty its buffer. " +
                "NOTE: Litigation Matter Sites will not be synchronized until service starts again." + Thread.CurrentThread.Name);
            workerThread.Join(60000);
        }

        private void checkForDuplication(string lmNumber)
        {
            // Before updating a Litigation Matter site's properties, let's verify that this litigation matter is unique in Claimcenter.
            // if it's not, crash this service.
            if (!dataReader.isMatterUnique(lmNumber))
            {
                Exception ex = new InvalidOperationException("\nWARNING: " +
                    "\nA possible duplicate Litigation Matter has been detected in ClaimCenter.  As a safety measure, synchronization services have halted. " +
                    "After resolution, these services will require a manual restart.\n\n  Matter identified as conflicting: " + lmNumber);
                log.addError(ex.ToString() + Thread.CurrentThread.Name);
                log.sendEmailMessage(Settings.Default.SMTPServer, Settings.Default.SMTPServerPort,
                    "CCNofifier@GWCC", Settings.Default.AlertEmailAddress,
                    "*URGENT*: Possible ClaimCenter Duplication", ex.Message + "\n" + ex.GetType().ToString() + Thread.CurrentThread.Name);
                throw ex;
            }
        }
        private void ensureThreadLimit(int maxThreads)
        {
            // Must use System.Diagnostics and not System.Threading, since we need the start time.
            var activeThreads = ((IEnumerable)System.Diagnostics.Process.GetCurrentProcess().Threads)
                .OfType<System.Diagnostics.ProcessThread>()
                .Where(t => t.ThreadState == System.Diagnostics.ThreadState.Running)
                .Count();
            
            if (activeThreads > maxThreads)
            {
                log.addWarning("Aborting attempt to start another thread, " + 
                    "as this process is currently running the maximum number "+
                    "of allowed threads. (" + maxThreads + ") The aborted thread is below." + Thread.CurrentThread.Name);
                Thread.CurrentThread.Abort();
            }
        }
        private void processNewSite(Matter matter)
        {
            bool success = false;
            if (Settings.Default.UseTemplateExportMethod)
                success = matterProvisioning.createSite(
                matter, 
                AppDomain.CurrentDomain.BaseDirectory + Settings.Default.MatterTemplateStorageName + ".cmp"
                );
            else
                success = matterProvisioning.createSiteFromTemplateSolution(
                matter, 
                Settings.Default.ExistingTemplateSolutionName
                );

            if (success)
            {
                log.addInformation("Successfully Created a new Litigation Matter site: \"" + matter.MatterName + "\", (" + matter.LMNumber + ")");
                // Set the properties of the new site, set the site's security and generate initial tasks for the matter.
                matterProvisioning.updateProperties(matter);
                if (Settings.Default.EnableTaskGeneraton) matterProvisioning.generateInitialTasks(matter, Settings.Default.TaskListLocation);
                matterProvisioning.setSiteSecurity(matter, Settings.Default.SiteOwnersGroupName, Settings.Default.SiteManagerGroupName, Settings.Default.SiteReadOnlyUsersGroupName, Settings.Default.SiteAdditionalContributorsGroupName);
            }
        }
        private void processUpdatesToExistingSite(Matter matter)
        {
            // Update the properties on existing site and adjust site security if needed.
            matterProvisioning.updateProperties(matter);
            matterProvisioning.setSiteSecurity(
                                               matter, 
                                               Settings.Default.SiteOwnersGroupName, 
                                               Settings.Default.SiteManagerGroupName, 
                                               Settings.Default.SiteReadOnlyUsersGroupName, 
                                               Settings.Default.SiteAdditionalContributorsGroupName
                                               );
        }

        /// <summary>
        /// Thread Worker handles all work.  Once it is done with its tasks, 
        /// it will sleep for the interval specified, repeating until the service is stopped.
        /// </summary>
        private void Worker()
        {

            // start an endless loop; loop will abort only when "serviceStarted" flag = false
            while (serviceStarted)
            {
                ensureThreadLimit(Settings.Default.MaxAllowedThreads); 

                string threadStatusInfo = newServiceInstance ? "Spawning New Worker Thread." : "Worker Thread Awakened.";
                log.addInformation( threadStatusInfo +" Beginning to process matters." + Thread.CurrentThread.Name);
                if (newServiceInstance)
                {
                    if (Settings.Default.UseTemplateExportMethod)
                    {
                        log.addInformation("Regenerating the Matter Site Template using the site found at: " + Settings.Default.LitMatterTemplateURL + ".\n" +
                            "NOTE: If changes are made to this template, this service must be restarted in order for them to take effect." + Thread.CurrentThread.Name);
                        matterProvisioning.resetTemplate(Settings.Default.LitMatterTemplateURL,
                            AppDomain.CurrentDomain.BaseDirectory + Settings.Default.MatterTemplateStorageName + ".cmp");
                    }
                    newServiceInstance = false;
                }
                processMatters();
                // yield
                if (serviceStarted)
                {
                    double pollingInterval = Settings.Default.HourlyPollingInterval;
                    int sleepyTime = (int)(pollingInterval * 3600000);
                    log.addInformation("Thread will now sleep for " + pollingInterval + 
                        " hour(s) and resume at approximately " + DateTime.Now.AddHours(pollingInterval) + Thread.CurrentThread.Name);
                    Thread.Sleep(sleepyTime); // hours to miliseconds
                }
            }

            // End the thread
            log.addInformation("Thread has been successfully stopped." + Thread.CurrentThread.Name);
            Thread.CurrentThread.Abort();
        }
    }
}