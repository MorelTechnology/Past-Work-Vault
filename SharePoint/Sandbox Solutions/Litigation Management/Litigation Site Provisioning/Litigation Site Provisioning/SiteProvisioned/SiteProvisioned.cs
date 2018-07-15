using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Litigation_Site_Provisioning.SiteProvisioned
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class SiteProvisioned : SPWebEventReceiver
    {
        /// <summary>
        /// A site was provisioned.
        /// </summary>
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            using (SPWeb web = properties.Web)
            {
                string webId = web.ServerRelativeUrl.Substring(web.ServerRelativeUrl.LastIndexOf("/") + 1);
                SPListItem parentListItem = web.ParentWeb.Lists["Matters"].GetItemById(int.Parse(webId));

                //See if the current site has a "Matter Summary" list
                SPList matterSummaryList = web.Lists.TryGetList("Matter Summary");
                if (matterSummaryList != null && parentListItem != null)
                {
                    //Only process items where the parent content type was "matter". Linked matters are handled through the synchronization process
                    if (parentListItem["Content Type"].ToString() == "Matter")
                    {
                        SPListItem newSummaryItem = matterSummaryList.AddItem();
                        newSummaryItem["Matter Name"] = parentListItem["Matter Name"];
                        newSummaryItem["Account Name"] = parentListItem["Account Name"];
                        newSummaryItem["Litigation Manager"] = parentListItem["Litigation Manager"];
                        newSummaryItem["Matter Status"] = parentListItem["Matter Status"];
                        newSummaryItem["Case Caption"] = parentListItem["Case Caption"];
                        newSummaryItem["Docket Number"] = parentListItem["Docket Number"];
                        UpdateMMField(newSummaryItem, "Litigation Type", parentListItem["Litigation Type_0"].ToString(), "Litigation Type_0");
                        UpdateMMField(newSummaryItem, "State Filed", parentListItem["State Filed_0"].ToString(), "State Filed_0");
                        UpdateMMField(newSummaryItem, "Venue", parentListItem["Venue_0"].ToString(), "Venue_0");
                        UpdateMMField(newSummaryItem, "Affiliate", parentListItem["Affiliate_0"].ToString(), "Affiliate_0");
                        UpdateMMField(newSummaryItem, "Work/Matter Type", parentListItem["Claim Type_0"].ToString(), "Claim Type_0");
                        newSummaryItem["Claim Group ID"] = parentListItem["Claim Group ID"];
                        newSummaryItem["External Firm Name"] = parentListItem["External Firm Name"];
                        newSummaryItem.Update();
                        return;
                    }
                }

                //See if the current site has a "Project Summary" list
                SPList projectSummaryList = web.Lists.TryGetList("Project Summary");
                if (projectSummaryList != null)
                {
                    //Add a project summary item to the list
                    if (parentListItem["Content Type"].ToString() == "Project")
                    {
                        SPListItem newSummaryItem = projectSummaryList.AddItem();
                        newSummaryItem["Project Name"] = parentListItem["Project Name"];
                        newSummaryItem["Project Description"] = parentListItem["Project Description"];
                        newSummaryItem["Project Lead"] = parentListItem["Project Lead"];
                        newSummaryItem["Project Status"] = parentListItem["Project Status"];
                        newSummaryItem["Project Site"] = parentListItem["Project Site"];
                        newSummaryItem.Update();
                        return;
                    }
                }
            }
        }

        private static void UpdateMMField(SPListItem item, string fieldName, string value, string fieldHiddenNoteName)
        {
            if (value != null)
            {
                item[fieldName] = string.Format("-1;#{0}", value);
            }
            else
            {
                item[fieldName] = null;
            }
            item[fieldHiddenNoteName] = value;
        }
    }
}