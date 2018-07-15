using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Client;
using SP = Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Administration;
using System.Diagnostics;

namespace AcuitySharePointConduitService
{
    class SharePointConnector
    {
        EventLogger log = new EventLogger();
        public void addAlert(string assignedToEmail, string subject, string body)
        {
            using (SPSite site = new SPSite(Properties.Settings.Default.SPSiteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;

                    // Fetch the List
                    SPList list = web.Lists[Properties.Settings.Default.AlertsListName];

                    //Add a new item in the List
                    SPListItem listItem = list.Items.Add();
                    listItem["Expires"] = DateTime.Now.AddDays(Properties.Settings.Default.DefaultAlertExpiresInDays);
                    listItem["Subject"] = subject;
                    listItem["Body"] = body;
                    try
                    {
                        //listItem["Alert User"] = web.EnsureUser(assignedToEmail);
                        listItem["Alert User"] = web.SiteUsers.GetByEmail(assignedToEmail);
                    }
                    catch (Exception ex)
                    {
                        log.addWarning("Acuity SharePoint Conduit Service", "Application", "User could not be obtained from the Email Address '" + assignedToEmail +
                            "'.  The extended exception reported was: " + ex.ToString());
                        listItem["Invalid Assignee"] = assignedToEmail;
                    }
                    listItem.Update();
                    web.AllowUnsafeUpdates = false;
                }
            }

        }


        public void updateAlert(int alertId, int taksId, string assignedToEmail, string AlertType, int AlertCount, string AlertHyperlink = null)
        {
            //TODO implement updateAlert
        }

        public void deleteAlert(int alertId)
        {
            //TODO implement deleteAlert
            throw new NotImplementedException();

        }

        public void addTask(
            string taskSource, string taskType, int taskCount, string taskAssignedToEmail,
            string taskHyperlink = null, string taskDueDate = null,
            string taskWorkMatterId = null, string taskLitigationMatterId = null, string taskDocketId = null)
        {
            DateTime due;
            string acuityTaskMsg = "You have " + taskCount + " " + taskType + (taskCount > 1 ? "s" : "") + " awaiting your approval.";

            if (!string.IsNullOrEmpty(taskDueDate))
            { due = Convert.ToDateTime(taskDueDate); }
            else // No due date specified.
            { due = DateTime.Now.AddDays(Properties.Settings.Default.DefaultTaskDueInDays); }

            using (SPSite site = new SPSite(Properties.Settings.Default.SPSiteURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    web.AllowUnsafeUpdates = true;

                    // Fetch the List
                    SPList list = web.Lists[Properties.Settings.Default.TasksListName];

                    //Add a new item in the List
                    SPListItem listItem = list.Items.Add();
                    listItem["Data Source"] = "Acuity-SharePoint Conduit Service";
                    listItem["Additional Source Information"] = taskSource;
                    listItem["Due Date"] = due;
                    listItem["Matter Name"] = "Acuity Task";
                    listItem["WorkMatter ID"] = taskWorkMatterId;
                    listItem["Task Description"] = acuityTaskMsg;
                    try
                    {
                        //listItem["Assigned To"] = web.EnsureUser(taskAssignedToEmail);
                        listItem["Assigned To"] = web.SiteUsers.GetByEmail(taskAssignedToEmail);
                    }
                    catch (Exception ex)
                    {
                        log.addWarning("Acuity SharePoint Conduit Service", "Application", "Error while creating task.  User could not be obtained from the Email Address '" + taskAssignedToEmail +
                            "'.  The extended exception reported was: " + ex.ToString());
                        listItem["Invalid Assignee"] = taskAssignedToEmail;
                    }
                    listItem["Related URL"] = taskHyperlink;
                    listItem["Litigation Matter ID"] = taskLitigationMatterId;
                    listItem["Docket ID"] = taskDocketId;
                    listItem.Update();
                    web.AllowUnsafeUpdates = false;
                }
            }

     }
       

        public void updateTask(int taskId, string taskType, int taskCount, string taskAssignedToEmail,
            string taskHyperlink = null, string taskDueDate = null,
            string taskWorkMatterId = null, string taskLitigationMatterId = null, string taskDocketId = null)
        {
            //TODO implement updateTask
            throw new NotImplementedException();

        }


        public void deleteTask(int taskId)
        {
            //TODO implement deleteTask
            throw new NotImplementedException();
 
        }
    }

}
