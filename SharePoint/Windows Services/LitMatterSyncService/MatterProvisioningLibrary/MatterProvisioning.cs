using AppsToOranges.Extensions;
using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using static AppsToOranges.SharePointUtilities.ListManagement;
using static AppsToOranges.SharePointUtilities.Provisioning;
using static AppsToOranges.SharePointUtilities.Security;
using static AppsToOranges.Utility;

namespace MatterProvisioningLibrary
{
    public class MatterProvisioning
    {
        private static EventLogger log = new EventLogger("Litigation Matter Synchronization Service", "Application");
        private string parentWebUrl;
        public MatterProvisioning(string parentWebUrl)
        {
            this.parentWebUrl = parentWebUrl;
        }
        public bool createSite(Matter matter, string templateFilePath)
        {
            bool result = false;
            log.addInformation("Creating New Litigation Matter site for " + matter.LMNumber + ".");
            if (importSpWeb(parentWebUrl, matter.LMNumber, templateFilePath)) result = true;
            return result;
        }
        
        public bool createSite(string copyFromSite, string newUrl)
        {
            bool result = false;
            log.addInformation("Manually Creating Site with URL " + newUrl +
                " at request of " + SPContext.Current.Web.CurrentUser.Name);
            SPWeb copyWeb = new SPSite(copyFromSite).OpenWeb();
            exportSpWeb(copyWeb, "temporaryTemplate");
            importSpWeb(parentWebUrl, newUrl, "temporaryTemplate");
            return result;
        }

        public bool createSiteFromTemplateSolution(Matter matter, string templateSolutionName)
        {
            bool result = false;
            using (SPWeb web = new SPSite(parentWebUrl).OpenWeb())
            {
                string template = GetTemplate(templateSolutionName, web.Site);
                web.Webs.Add(matter.LMNumber, "Temporary, Generated from template on " + DateTime.Now.ToString("F"),
                    matter.LMNumber,
                    (uint)web.Locale.LCID,
                    template, true, false);
                result = true;
            }
            return result;
        }
        public bool createSiteFromTemplateSolution(Project project, string templateSolutionName)
        {
            bool result = false;
            using (SPWeb web = new SPSite(parentWebUrl).OpenWeb())
            {
                string template = GetTemplate(templateSolutionName, web.Site);
                web.Webs.Add(project.ProjectId, "Temporary, Generated from template on " + DateTime.Now.ToString("F"),
                   project.ProjectId,
                    (uint)web.Locale.LCID,
                    template, true, false);
                result = true;
            }
            return result;
        }

        public List<string> getLMUserIDs(string url, string group)
        {
            List<string> ids = new List<String>();
            try
            {
                using (SPWeb web = new SPSite(url).OpenWeb())
                {
                        foreach (SPUser user in web.SiteGroups[group].Users)
                        ids.Add(user.LoginName.Split('\\')[1].ToUpper());
                }
            }
            catch(Exception ex) { log.addError("Unable to identify the users for which Matter sites should be created.  The process cannot continue. " + ex.ToString()); }
            return ids;
        }

        public void generateInitialTasks(Matter matter, string taskListLocation)
        {
            StringBuilder tasksCreated  = new StringBuilder(); // Grab the tasks created for log output.

            // read LitigationTasks.config (custom XML file) to dataset for value retrieval.
            DataSet ds = new DataSet();
            try { ds.ReadXml("LitigationTasks.config"); }
            catch(Exception ex) { log.addError(ex.ToString()); }

            #region Get the litigation manager user as SPFieldUserValue 
            // This strange articulation must be done because the task list is located in a different web, and therefore the user object has a different key/id
            // While SharePoint could perform an internal cast of SPUser.ToString(), it seems retrival from the hashtable using the addListItem method prevents this.
            SPFieldUserValue taskAssignedTo = null;
            try
            {
                using (SPWeb tasksWeb = new SPSite(taskListLocation).OpenWeb())
                {
                    // Ensure the Litigation Manager has access to the web containing tasks, or SharePoint might kibby and die.
                    SPUser user = tasksWeb.EnsureUser(matter.LitigationManagerSPUser.ToString());
                    taskAssignedTo = new SPFieldUserValue(tasksWeb, user.ID, user.LoginName);
                }
            }
            catch (Exception ex)
            {
                log.addWarning("Unable to determine an appropriate user to assign tasks to for matter " + matter.LMNumber + ", so they will need to be assigned manually." +
                    "  The Exception was: " + ex.ToString());
            }
            #endregion

            foreach (DataRow task in ds.Tables[0].Rows)
            {
                var description = task["description"];
                var associatedURL = task["url"];
                var daysUntilDue = Convert.ToInt32(task["daysuntildue"]);

                Hashtable taskFields = new Hashtable();
                taskFields.SetProperty("Due Date", DateTime.Now.AddDays(daysUntilDue));
                taskFields.SetProperty("Matter Name", matter.MatterName);
                taskFields.SetProperty("Task Description", description);
                taskFields.SetProperty("Assigned To", taskAssignedTo);
                taskFields.SetProperty("Allow Manual Completion", true);
                taskFields.SetProperty("Related URL", associatedURL);
                taskFields.SetProperty("Associated Site ID", matter.LMNumber);
                taskFields.SetProperty("Data Source", "LitMatterSyncService");
                taskFields.SetProperty("Additional Source Information", "Provisioned with Site");

                addListItem(taskListLocation, taskFields);
                tasksCreated.AppendLine(description.ToString() + "(Due in " + daysUntilDue.ToString() + " days)");
            }
            log.addInformation("Generated the following initial tasks for matter " + matter.LMNumber + "\n" + tasksCreated);
        }

        public Matter matterFromDataRow(DataRow row)
        {
            Matter matter = new Matter
            {
                AccountName = row["AccountName"].ToString(),
                Affiliate = row["Affiliate"].ToString(),
                CaseCaption = row["CaseCaption"].ToString(),
                Country = row["Country"].ToString(),
                DocketNumber = row["DocketNumber"].ToString(),
                IsMatterActive = Boolean.Parse(row["IsMatterActive"].ToString()),
                IsMatterProcessed = Boolean.Parse(row["IsMatterProcessed"].ToString()),
                LitigationManagerName = row["LitigationManager"].ToString(),
                LitigationManagerUserId = row["LMUserID"].ToString(),
                LitigationType = row["LitigationType"].ToString(),
                LMNumber = row["trg_MatterNumber"].ToString(),
                MatterName = row["MatterName"].ToString(),
                MatterStatus = row["MatterStatus"].ToString(),
                SiteNeeded = false,
                StateFiled = row["StateFiled"].ToString(),
                SysCreateDate = row["Sys_Create_Dt"].ToString(),
                Venue = row["Venue"].ToString(),
                WorkMatterType = row["WorkMatterType"].ToString()
            };

            // Get Litigation Manager as SPUser.  If it can't be retrieved, use the service user so the
            // process can continue, but log an error.
            using (SPWeb web = new SPSite(parentWebUrl).OpenWeb())
            {
                matter.LitigationManagerSPUser = getLitigationManagerSPUser(matter.LitigationManagerUserId, web);
                if (matter.LitigationManagerSPUser == null)
                {
                    matter.LitigationManagerSPUser = web.CurrentUser;
                    log.addError("There was an error trying to establish the Manager for site " + matter.LMNumber + ". " +
                        "The site will still be created, but needs to have a valid user assigned to the matter.");
                }
            }
            return matter;
        }

        public List<Matter> mattersToProcess(DataTable table, List<string>validUsers)
        {
            StringBuilder logMessage = new StringBuilder();

            //Take the table passed in, check each row against SharePoint to determine if a site is needed and build a Matter instance accordingly.
            List<Matter> matters = new List<Matter>();
            if (table != null && table.Rows.Count > 0)
            {
                foreach (DataRow row in table.Rows)
                {
                    if (row != null)
                    {
                        Matter matter = matterFromDataRow(row);
                        if(!(validUsers.Contains(matter.LitigationManagerUserId, StringComparer.OrdinalIgnoreCase)))
                        { // Skip Matters which have a user not found in the valid members list.
                            logMessage.AppendLine(
                                String.Format("{0}, ({1}), is not a valid member. {2} is being ignored.", 
                                matter.LitigationManagerName, matter.LitigationManagerUserId, matter.LMNumber));
                            matter.IsMatterProcessed = true;
                            matters.Add(matter);
                        }
                        else
                        {
                            matter.SiteNeeded = (!siteExists(matter.LMNumber));
                            matters.Add(matter);
                        }
                    }
                }
            }
            if(logMessage.Length > 0) log.addWarning(logMessage.ToString());
            return matters;
        }

        public void resetTemplate(string templateURL, string outputFilePath)
        {
            using (SPSite site = new SPSite(templateURL))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    exportSpWeb(web, outputFilePath);
                }
            }
        }

        public void setSiteSecurity(Matter matter, string SiteOwnersGroupName, string SiteManagerGroupName, string SiteReadOnlyUsersGroupName, string SiteAdditionalContributorsGroupName)
        {
            using (SPWeb web = safelyGetWeb(parentWebUrl, matter.LMNumber))
            {
                // break any existing Security role inheritance and don't copy permissions from the parent.
                web.BreakRoleInheritance(false);

                // Remove any groups already on the site, start with a clean slate.
                securityWipe(web);

                // Create or attach existing Security Groups. 

                #region Logic: Site Owners Group
                string ownersGroupName = SiteOwnersGroupName; 
                // Generally, the owners group is global to the collection, so check to see if it exists first. 
                // if it doesn't, assume it should be site-specific and prepend LM Number to it.
                try
                {
                    SPGroup owners = web.SiteGroups[SiteOwnersGroupName]; // It Existed, add it as is.
                    setupWebUserGroup(web, ownersGroupName, SPRoleType.Administrator);
                }
                catch
                {
                    // It didn't exist.  Make a new site-specific group.
                    ownersGroupName = matter.LMNumber + " - " + SiteOwnersGroupName; // LM0001234 - Site Owners
                    setupWebUserGroup(web, ownersGroupName, SPRoleType.Administrator, "Site Owners for " + matter.LMNumber);
                }
                #endregion

                #region Logic: Site Manager Group
                string managerGroupName = matter.LMNumber + " - " + SiteManagerGroupName; // LM0001234 - Site Manager
                setupWebUserGroup(web, managerGroupName, SPRoleType.Contributor, "This group should contain ONLY ONE " +
                    "user who is designated as the manager of " + matter.LMNumber);

                // Ensure only the current Litigation Manager is placed inside the intended security group.
                // Reset Site Manager groups which contain more than one person.

                SPGroup siteManagerGroup = web.SiteGroups[managerGroupName];
                siteManagerGroup.AddUser(matter.LitigationManagerSPUser);
                if (siteManagerGroup.Users.Count > 1)
                {
                    log.addWarning("Conflicting assignments exist in the '" + siteManagerGroup.Name + "' group for site " + matter.LMNumber + 
                        ". If the matter is currently being reassigned, this is normal. The group membership has been reset to contain only [" +
                         matter.LitigationManagerSPUser.Name +  "].  Additional participants, (rare), should be added to one of the following security groups:\n\n"+
                         matter.LMNumber + " - " + SiteAdditionalContributorsGroupName + " (Contribute Access)\n" + matter.LMNumber + " - " + SiteReadOnlyUsersGroupName + " (Read-Only Access)\n" + 
                         SiteOwnersGroupName + " (Administrators Only)");
                    // Empty the group, and put only the site manager back in.
                    emptySPGroup(siteManagerGroup);
                    siteManagerGroup.AddUser(matter.LitigationManagerSPUser);
                }
                #endregion

                #region Logic: Read Only Users Group
                string readOnlyUsersGroupName = matter.LMNumber + " - " + SiteReadOnlyUsersGroupName; // LM0001234 - Read Only Users
                setupWebUserGroup(web, readOnlyUsersGroupName, SPRoleType.Reader, "This group contains users with Read-Only access to " + matter.LMNumber);
                #endregion

                #region Logic: Additional Contributors Group
                string additionalContributorsGroupName = matter.LMNumber + " - " + SiteAdditionalContributorsGroupName; //LM0001234 - Additional Contributors
                setupWebUserGroup(web, additionalContributorsGroupName, SPRoleType.Contributor, "This group contains users, who, in addition to the Site Manager, can contribute to " + matter.LMNumber +
                        ". Use of this group should be limited to rare exceptions where more than one person requires control of the site.");
                #endregion
                // Save Changes and close out
                web.Update();
            }
        }

        public void setSiteSecurity(Project project, string SiteOwnersGroupName, string SiteManagerGroupName, string SiteReadOnlyUsersGroupName, string SiteAdditionalContributorsGroupName)
        {
            using (SPWeb web = safelyGetWeb(parentWebUrl, project.ProjectId))
            {
                // break any existing Security role inheritance and don't copy permissions from the parent.
                web.BreakRoleInheritance(false);

                // Remove any groups already on the site, start with a clean slate.
                securityWipe(web);

                // Create or attach existing Security Groups. 

                #region Logic: Site Owners Group
                string ownersGroupName = SiteOwnersGroupName;
                // Generally, the owners group is global to the collection, so check to see if it exists first. 
                // if it doesn't, assume it should be site-specific and prepend Project ID to it.
                try
                {
                    SPGroup owners = web.SiteGroups[SiteOwnersGroupName]; // It Existed, add it as is.
                    setupWebUserGroup(web, ownersGroupName, SPRoleType.Administrator);
                }
                catch
                {
                    // It didn't exist.  Make a new site-specific group.
                    ownersGroupName = project.ProjectId + " - " + SiteOwnersGroupName; // MC0101019999 - Site Owners
                    setupWebUserGroup(web, ownersGroupName, SPRoleType.Administrator, "Site Owners for " + project.ProjectId);
                }
                #endregion

                #region Logic: Site Manager Group
                string managerGroupName = project.ProjectId + " - " + SiteManagerGroupName; // MC0101019999 - Site Manager
                setupWebUserGroup(web, managerGroupName, SPRoleType.Contributor, "This group should contain ONLY ONE " +
                    "user who is designated as the manager of " + project.ProjectId);

                // Ensure only the current Litigation Manager is placed inside the intended security group.
                // Reset Site Manager groups which contain more than one person.

                SPGroup siteManagerGroup = web.SiteGroups[managerGroupName];
                siteManagerGroup.AddUser(project.ProjectLeadSPUser);
                if (siteManagerGroup.Users.Count > 1)
                {
                    log.addWarning("Conflicting assignments exist in the '" + siteManagerGroup.Name + "' group for site " + project.ProjectId +
                        ". If the matter is currently being reassigned, this is normal. The group membership has been reset to contain only [" +
                         project.ProjectLeadSPUser.Name + "].  Additional participants, (rare), should be added to one of the following security groups:\n\n" +
                         project.ProjectId + " - " + SiteAdditionalContributorsGroupName + " (Contribute Access)\n" + project.ProjectId + " - " + SiteReadOnlyUsersGroupName + " (Read-Only Access)\n" +
                         SiteOwnersGroupName + " (Administrators Only)");
                    // Empty the group, and put only the site manager back in.
                    emptySPGroup(siteManagerGroup);
                    siteManagerGroup.AddUser(project.ProjectLeadSPUser);
                }
                #endregion

                #region Logic: Read Only Users Group
                string readOnlyUsersGroupName = project.ProjectId + " - " + SiteReadOnlyUsersGroupName; // MC0101019999 - Read Only Users
                setupWebUserGroup(web, readOnlyUsersGroupName, SPRoleType.Reader, "This group contains users with Read-Only access to " + project.ProjectId);
                #endregion

                #region Logic: Additional Contributors Group
                string additionalContributorsGroupName = project.ProjectId + " - " + SiteAdditionalContributorsGroupName; // MC0101019999 - Additional Contributors
                setupWebUserGroup(web, additionalContributorsGroupName, SPRoleType.Contributor, "This group contains users, who, in addition to the Site Manager, can contribute to " + project.ProjectId +
                        ". Use of this group should be limited to rare exceptions where more than one person requires control of the site.");
                #endregion
                // Save Changes and close out
                web.Update();
            }
        }

        public void updateProperties(Matter matter)
        {
            // Given a matter, this method will create properties of a new site or or replace all properties of an existing site.
            try
            {
                log.addInformation("Attempting to update the properties on " + matter.LMNumber);
                // Get all new properties to a hashtable required in setWebProperties method
                Hashtable properties = new Hashtable();
                properties.SetProperty("Matter_Number", matter.LMNumber);
                properties.SetProperty("Affiliate", matter.Affiliate);
                properties.SetProperty("Case_Caption", matter.CaseCaption);
                properties.SetProperty("Matter_Name", matter.MatterName);
                properties.SetProperty("Account_Name", matter.AccountName);
                properties.SetProperty("Litigation_Manager", matter.LitigationManagerName);
                properties.SetProperty("LMUserID", matter.LitigationManagerUserId);
                properties.SetProperty("Matter_Status", matter.MatterStatus);
                properties.SetProperty("Docket_Number", matter.DocketNumber);
                properties.SetProperty("Litigation_Type", matter.LitigationType);
                properties.SetProperty("State_Filed", matter.StateFiled);
                properties.SetProperty("Venue", matter.Venue);
                properties.SetProperty("Country", matter.Country);
                properties.SetProperty("Work_Matter_Type", matter.WorkMatterType);
                properties.SetProperty("Last_Synchronized", DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                properties.SetProperty("isMatterActive", matter.IsMatterActive);
                properties.SetProperty("isLinkedMatter", matter.IsLinkedMatter);

                using (SPWeb web = safelyGetWeb(parentWebUrl, matter.LMNumber))
                {
                    // Set (or reset) the site's title
                    string siteTitle = (string)properties.GetProperty("Matter_Name");
                    if (string.IsNullOrEmpty(siteTitle)) siteTitle = (string)properties.GetProperty("Account_Name");
                    if (string.IsNullOrEmpty(siteTitle)) siteTitle = matter.LMNumber;
                    web.Title = siteTitle;

                    // Determine if this matter is being closed during this transaction.
                    if (matter.MatterStatus.ToLower() == "closed" && !web.AllProperties.ContainsKey("Close_Requested"))
                    {
                        log.addWarning(matter.LMNumber + " is being closed during this transaction.");
                        properties.SetProperty("Close_Requested", DateTime.Now.ToString("MM/dd/yyyy h:mm tt"));
                    }

                    // Determine if this matter was previously closed and is being re-opened during this transaction.
                    if (matter.MatterStatus.ToLower() != "closed" && web.AllProperties.ContainsKey("Close_Requested"))
                    {
                        if (web.AllProperties["Close_Requested"].ToString().Length > 1) // ensure there's something in the field, before we report.
                        {
                            log.addWarning(matter.LMNumber + ", which was previously requested to be closed on "
                                + web.AllProperties["Close_Requested"].ToString() + ", has had its status changed from "
                                + web.AllProperties["Matter_Status"].ToString() + " to " + matter.MatterStatus + ".");
                        }
                        web.AllProperties.Remove("Close_Requested");  // remove the Close_Requested property, since this matter isn't closed.
                    }
                    log.addInformation("The site properties for Litigation Matter \"" + siteTitle + "\", (" + matter.LMNumber + "), were updated " +
                        "as follows: \n\n" + setWebProperties(web, properties, true));
                }
            }
            catch (Exception ex)
            {
                log.addError("Unable to set the Properties for " + matter.LMNumber + ".  Exception:" + ex.ToString() +
                    "Inner Exception: " + ex.InnerException.ToString() + ex.Data);
            }
        }

        public void updateProperties(Project project)
        {
            // Given a project, this method will create properties of a new site or or replace all properties of an existing site.

            // Get all new properties to a hashtable required in setWebProperties method
            Hashtable properties = new Hashtable();
            properties.SetProperty("Project_ID", project.ProjectId);
            properties.SetProperty("Project_Name", project.ProjectName);
            properties.SetProperty("Project_Description", project.ProjectDescription);
            properties.SetProperty("Project_Status", project.ProjectStatus);
            properties.SetProperty("Project_Lead", project.ProjectLead);

            using (SPWeb web = safelyGetWeb(parentWebUrl, project.ProjectId))
            {
                // Set (or reset) the site's title
                web.Title = (string)properties.GetProperty("Project_Name");

                // Determine if this project is being closed during this transaction.
                if (project.ProjectStatus.ToLower() == "closed" && !web.AllProperties.ContainsKey("Close_Requested"))
                {
                    log.addWarning(project.ProjectName +",(" + project.ProjectId + "), is being closed during this transaction.");
                    properties.SetProperty("Close_Requested", DateTime.Now.ToString("f"));
                }

                // Determine if this matter was previously closed and is being re-opened during this transaction.
                if (project.ProjectStatus.ToLower() != "closed" && web.AllProperties.ContainsKey("Close_Requested"))
                {
                    if (web.AllProperties["Close_Requested"].ToString().Length > 1) // ensure there's something in the field, before we report.
                    {
                        log.addWarning(project.ProjectName + ",(" + project.ProjectId + "), which was previously requested to be closed on "
                            + web.AllProperties["Close_Requested"].ToString() + ", has had its status changed from "
                            + web.AllProperties["Matter_Status"].ToString() + " to " + project.ProjectStatus + ".");
                    }
                    web.AllProperties.Remove("Close_Requested");  // remove the Close_Requested property, since this matter isn't closed.
                }
                log.addInformation("The site properties for Project " + project.ProjectName + ",(" + project.ProjectId + "), were updated " +
                    "as follows: \n\n" + setWebProperties(web, properties));
            }
        }

        private SPUser getLitigationManagerSPUser(string rawUserId, SPWeb web)
        {
            string userId = null;

            if (rawUserId.Contains("\\")) userId = rawUserId; // assume id is already in the form domain\user
            else userId = Environment.UserDomainName + "\\" + rawUserId; // detected domain\user;
            try
            {
                SPUser user = web.EnsureUser(userId);
                return user;
            }
            catch (Exception ex)
            {
                log.addWarning("There was an error trying to establish a SharePoint identity for " + userId + ". " +
                    "Additional information :" + ex.ToString());
                return null;
            }
        }

        private SPWeb safelyGetWeb(string parentWebUrl, string lmNumber)
        {
            // The OpenWeb method has an odd behavior of returning back the first subweb it finds. (which is both dumb and dangerous.)
            // Although a boolean in the second paramerter of the OpenWeb method is supposed to ensure that we return back the desired subweb,
            // it's been problematic with multi-layered subwebs.  

            SPWeb web = null;
            try
            {
                web = new SPSite(parentWebUrl + "/" + lmNumber).OpenWeb();
                if (!web.Url.Contains(lmNumber)) throw new Microsoft.SharePoint.SPEndpointAddressNotFoundException();
                return web;
            }
            catch (Exception ex)
            {
                log.addWarning("Attempted to retrieve a site for Litigation Matter " + lmNumber +
                    ", but a corresponding site could not be retrieved.  Update could not be processed. The exception returned was " + ex.ToString());
                if (web != null) web.Dispose();
                return web;
            }
        }

        private bool siteExists(string lmNumber)
        {
            try
            {
                using (SPSite site = new SPSite(parentWebUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPWeb foundSite = web.Webs.FirstOrDefault(x => x.Name == lmNumber);
                        if (foundSite != null)
                        {
                            return true;
                        }
                        else { return false; }
                    }
                }
            }
            catch (SPDuplicateValuesFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                log.addError("Problem verifying the existance of a work matter site for " + lmNumber +
                    ".  No new site will be created. " + ex.ToString());
                return true; // suggest site exists to allow process to continue, though it likely may not.
            }
        }

        private void setupWebUserGroup(SPWeb web, string groupName, SPRoleType role, string groupDescription = null)
        {
            SPGroup group = null;
            try
            {
                group = web.SiteGroups[groupName];
                log.addInformation("Existing Group '" + group.Name + "' found.  Will attach to site " + web.ServerRelativeUrl.ToString());
            }
            catch
            {
                log.addWarning("'" + groupName + "' Group wasn't found.  Will be created.");
                web.SiteGroups.Add(groupName, web.CurrentUser, web.CurrentUser, groupDescription);
                group = web.SiteGroups[groupName];
                group.RemoveUser(web.CurrentUser);
            }
            finally
            {
                SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(role);
                SPRoleAssignment roleAssignment = new SPRoleAssignment(group);
                roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                web.RoleAssignments.Add(roleAssignment);
                web.Update();
            }
        }

        private void securityWipe(SPWeb web)
        {
            // Build a list of groups and users currently on the site, so we can delete them.  
            // Using a foreach loop here is problematic, since it would update the index while iterating.
            if (web.Groups.Count > 0)
            {
                List<SPGroup> webGroups = new List<SPGroup>();
                foreach (SPGroup group in web.Groups) webGroups.Add(group);
                foreach (SPGroup group in webGroups) web.Groups.Remove(group.Name);
            }
            if (web.Users.Count > 0)
            {
                List<SPUser> webUsers = new List<SPUser>();
                foreach (SPUser user in web.Users) webUsers.Add(user);
                foreach (SPUser user in webUsers) web.Users.Remove(user.LoginName);
            }
        }

    }
}