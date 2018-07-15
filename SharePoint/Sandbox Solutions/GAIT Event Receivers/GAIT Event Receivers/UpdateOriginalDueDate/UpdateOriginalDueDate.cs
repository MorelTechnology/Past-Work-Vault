using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace GAIT_Event_Receivers.UpdateOriginalDueDate
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class UpdateOriginalDueDate : SPItemEventReceiver
    {
        /// <summary>
        /// An item is being updated.
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            bool containsDueDate = properties.ListItem.Fields.ContainsField("DueDate");
            bool containsOriginalDueDate = properties.ListItem.Fields.ContainsField("Original_x0020_Due_x0020_Date");

            if (containsDueDate && containsOriginalDueDate)
            {
                if (properties.ListItem["Original_x0020_Due_x0020_Date"] == null)
                {
                    if (properties.AfterProperties["DueDate"] != null)
                    {
                        properties.AfterProperties["Original_x0020_Due_x0020_Date"] = properties.AfterProperties["DueDate"].ToString();
                    }
                    else if (properties.ListItem["DueDate"] != null)
                    {
                        properties.AfterProperties["Original_x0020_Due_x0020_Date"] = SPUtility.CreateISO8601DateTimeFromSystemDateTime(DateTime.Parse(properties.ListItem["DueDate"].ToString()).ToUniversalTime());
                    }
                }
                else
                {
                    if (properties.AfterProperties["Original_x0020_Due_x0020_Date"] != null)
                    {
                        if (properties.AfterProperties["Original_x0020_Due_x0020_Date"].ToString() == "")
                        {
                            TerminateUpdate(properties, "'Original Due Date' is a read-only field (new original due date field was null)");
                            return;
                        }
                        else
                        {
                            DateTime currentOrigDueDate = DateTime.Parse(properties.ListItem["Original_x0020_Due_x0020_Date"].ToString());
                            DateTime afterOrigDueDate = DateTime.Parse(properties.AfterProperties["Original_x0020_Due_x0020_Date"].ToString());
                            if (currentOrigDueDate != afterOrigDueDate)
                            {
                                TerminateUpdate(properties, "'Original Due Date' is a read-only field (New original due date was different than previous)");
                                return;
                            }
                        }
                    }
                    
                }
            }
        }

        public override void ItemAdding(SPItemEventProperties properties)
        {
                if (properties.AfterProperties["DueDate"] != null)
                {
                    properties.AfterProperties["Original_x0020_Due_x0020_Date"] = properties.AfterProperties["DueDate"].ToString();
                }
        }

        public void TerminateUpdate(SPItemEventProperties properties, string errorMessage)
        {
            properties.ErrorMessage = errorMessage;
            properties.Cancel = true;
        }
    }
}