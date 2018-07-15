using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SharePoint;
using Microsoft.SharePoint.BusinessData.SharedService;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.BusinessData.MetadataModel.Collections;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.BusinessData.Runtime;
using System.Data.SqlClient;
using System.Data;

namespace LitMatterSynchronization
{
    class Program
    {
        static void Main(string[] args)
        {
            string logPath = System.Configuration.ConfigurationManager.AppSettings["EventLogLocation"];
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }
            if (!logPath.EndsWith("\\"))
            {
                logPath += "\\";
            }
            string currentLogFile = logPath + "SyncLog_" + DateTime.Now.ToString("MMddyyyyHHmmss") + ".txt";
            StreamWriter sw = new StreamWriter(currentLogFile, true);

            //Set up SQL connection to staging table, load query results into memory
            SqlConnection connection = new SqlConnection();
            connection.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["ClaimCenterConnectionString"].ToString();

            try
            {
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandText = System.Configuration.ConfigurationManager.AppSettings["StagingQuery"];
                SqlDataReader result = command.ExecuteReader();
                DataTable table = new DataTable();
                table.Load(result);

                try
                {
                    //Loop through the results coming in from the staging table
                    try
                    {
                        if (table.Rows.Count == 0)
                        {
                            LogEvent(sw, "No records to process", DateTime.Now, "Information");
                        }
                        else
                        {
                            foreach (DataRow datarow in table.Rows)
                            {
                                try
                                {
                                    //Open up SP Site
                                    using (SPSite site = new SPSite(System.Configuration.ConfigurationManager.AppSettings["SPSite"]))
                                    {
                                        //Open up SP Web
                                        using (SPWeb web = site.RootWeb)
                                        {
                                            //Must allow unsafe updates to write directly to BDC columns
                                            web.AllowUnsafeUpdates = true;

                                            //Get the SP list and narrow the results down using a query
                                            SPList list = web.Lists[System.Configuration.ConfigurationManager.AppSettings["ListName"]];
                                            SPView view = list.Views[System.Configuration.ConfigurationManager.AppSettings["ViewName"]];
                                            string query = view.Query;
                                            SPQuery spQuery = new SPQuery();
                                            spQuery.Query = query;
                                            SPContentType itemType = list.ContentTypes[System.Configuration.ConfigurationManager.AppSettings["ContentType"]];

                                            //Get a new reference to the current items in the list
                                            SPListItemCollection itemCollection = list.GetItems(spQuery);

                                            //Also make references for non-linked matters in the event that non-linked matters need to be converted to linked matters
                                            SPView nonLinkedView = list.Views[System.Configuration.ConfigurationManager.AppSettings["NonLinkedViewName"]];
                                            string nonLinkedQuery = nonLinkedView.Query;
                                            SPQuery nonLinkedSPQuery = new SPQuery();
                                            nonLinkedSPQuery.Query = nonLinkedQuery;
                                            SPContentType nonLinkedItemType = list.ContentTypes[System.Configuration.ConfigurationManager.AppSettings["NonLinkedContentType"]];
                                            SPListItemCollection nonLinkedItemCollection = list.GetItems(nonLinkedSPQuery);

                                            //Get the matter number, the create date and the active flag
                                            string matterNumber = datarow["trg_MatterNumber"].ToString();
                                            string isMatterActive = datarow["IsMatterActive"].ToString();
                                            string sysCreateDate = Convert.ToDateTime(datarow["Sys_Create_Dt"]).ToString("yyyy-MM-dd HH:mm:ss.fff");
                                            string updateTable = System.Configuration.ConfigurationManager.AppSettings["UpdateDB"];
                                            string updateAffiliateTable = System.Configuration.ConfigurationManager.AppSettings["UpdateAffiliateDB"];
                                            string updateStatement = "UPDATE " + updateTable + " SET IsMatterProcessed = 1 WHERE trg_MatterNumber = '" + matterNumber + "' AND Sys_Create_Dt = '" + sysCreateDate + "'";
                                            string updateAffiliateStatement = "UPDATE " + updateAffiliateTable + " SET IsMatterProcessed = 1 WHERE trg_MatterNumber = '" + matterNumber + "' AND Sys_Create_Dt = '" + sysCreateDate + "'";

                                            //Start processing the row
                                            LogEvent(sw, "Processing " + matterNumber, DateTime.Now, "Information");
                                            Console.WriteLine("Processing " + matterNumber);
                                            bool matchingItems = false;
                                            //If there are no items in the view (i.e. first run) we know the first row will not find a match
                                            if (itemCollection.Count == 0)
                                            {
                                                LogEvent(sw, "Record does not exist for " + matterNumber, DateTime.Now, "Information");
                                                Console.WriteLine("Record does not exist for " + matterNumber);

                                                // Check to see if the user ID for the current record is a litigation manager
                                                if (IsLitigationManager(web, datarow["LMUserID"].ToString()))
                                                {
                                                    //Create a new SP List Item if the matter is active
                                                    //5/25/2016 Always process
                                                    if (/*isMatterActive == "True"*/!string.IsNullOrEmpty(isMatterActive))
                                                    {
                                                        LogEvent(sw, "Creating record for " + matterNumber, DateTime.Now, "Information");
                                                        Console.WriteLine("Creating record for " + matterNumber);
                                                        SPListItem item = CreateSPListItem(site, matterNumber, list, itemType);
                                                        LogEvent(sw, "Setting permissions for " + matterNumber, DateTime.Now, "Information");
                                                        Console.WriteLine("Setting permissions for " + matterNumber);
                                                        UpdateSPPermissions(item, datarow["LMUserID"].ToString());
                                                        LogEvent(sw, "Creating subsite for " + matterNumber, DateTime.Now, "Information");
                                                        Console.WriteLine("Creating subsite for " + matterNumber);
                                                        CreateSPWeb(item, datarow["LMUserID"].ToString());

                                                        LogEvent(sw, "Item processed. Updating staging table", DateTime.Now, "Information");
                                                        Console.WriteLine("Item processed. Updating staging table.");
                                                        SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
                                                        updateCommand.ExecuteNonQuery();
                                                    }
                                                    //Skip creating the item if the matter is not active (will only happen if the very first record
                                                    //coming in is an inactive matter and there is no data in the SP view)
                                                    else
                                                    {
                                                        LogEvent(sw, matterNumber + "is marked as not active, skipping", DateTime.Now, "Information");
                                                        Console.WriteLine(matterNumber + " is marked as not active, skipping");
                                                        SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
                                                        updateCommand.ExecuteNonQuery();
                                                    }
                                                }
                                                else
                                                {
                                                    LogEvent(sw, matterNumber + "is being skipped because " + datarow["LMUserID"].ToString() + " is not a litigation manager", DateTime.Now, "Warning");
                                                    Console.WriteLine(matterNumber + "is being skipped because " + datarow["LMUserID"].ToString() + " is not a litigation manager");
                                                    SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
                                                    updateCommand.ExecuteNonQuery();
                                                }
                                            }
                                            else
                                            //For subsequent runs when there is data in the SP views...
                                            {
                                                if (IsLitigationManager(web, datarow["LMUserID"].ToString()))
                                                {
                                                    SPListItem matchedItem = null;
                                                    //Loop through the SP items to try and find a match on the matter number
                                                    if (!matchingItems)
                                                    {
                                                        foreach (SPListItem listItem in itemCollection)
                                                        {
                                                            if (listItem["Matter Number"].ToString() == matterNumber)
                                                            {
                                                                matchingItems = true;
                                                                matchedItem = listItem;
                                                            }
                                                        }
                                                    }
                                                    //Loop through non-linked SP items to try and find a match on the docket number
                                                    if (!matchingItems)
                                                    {
                                                        foreach (SPListItem listItem in nonLinkedItemCollection)
                                                        {
                                                            if (listItem["Docket Number"].ToString() == datarow["DocketNumber"].ToString())
                                                            {
                                                                matchingItems = true;
                                                                matchedItem = listItem;
                                                            }
                                                        }
                                                    }

                                                    //If there is a match, update the SP List Item with 
                                                    if (matchingItems)
                                                    {
                                                        LogEvent(sw, "Record already exists for " + matterNumber, DateTime.Now, "Information");
                                                        Console.WriteLine("Record already exists for " + matterNumber);
                                                        LogEvent(sw, "Synchronizing new data for " + matterNumber, DateTime.Now, "Information");
                                                        Console.WriteLine("Synchronizing new data for " + matterNumber);
                                                        matchedItem = UpdateSPListItem(site, matterNumber, list, matchedItem, itemType);
                                                        LogEvent(sw, "Updating permissions for " + matterNumber, DateTime.Now, "Information");
                                                        Console.WriteLine("Updating permissions for " + matterNumber);
                                                        UpdateSPPermissions(matchedItem, datarow["LMUserID"].ToString());
                                                        LogEvent(sw, "Updating subsite for " + matterNumber, DateTime.Now, "Information");
                                                        Console.WriteLine("Updating subsite for " + matterNumber);
                                                        UpdateSPWeb(matchedItem, datarow["LMUserID"].ToString());

                                                        LogEvent(sw, "Item processed. Updating staging table", DateTime.Now, "Information");
                                                        Console.WriteLine("Item processed. Updating staging table.");
                                                        SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
                                                        updateCommand.ExecuteNonQuery();
                                                    }
                                                    //If there is no match, create the SP List Item
                                                    else
                                                    {
                                                        LogEvent(sw, "Record does not exist for " + matterNumber, DateTime.Now, "Information");
                                                        Console.WriteLine("Record does not exist for " + matterNumber);
                                                        //5/25/2016 Always process items
                                                        if (/*isMatterActive == "True"*/!string.IsNullOrEmpty(isMatterActive))
                                                        {
                                                            LogEvent(sw, "Creating record for " + matterNumber, DateTime.Now, "Information");
                                                            Console.WriteLine("Creating record for " + matterNumber);
                                                            SPListItem item = CreateSPListItem(site, matterNumber, list, itemType);
                                                            LogEvent(sw, "Setting permissions for " + matterNumber, DateTime.Now, "Information");
                                                            Console.WriteLine("Setting permissions for " + matterNumber);
                                                            UpdateSPPermissions(item, datarow["LMUserID"].ToString());
                                                            LogEvent(sw, "Creating subsite for " + matterNumber, DateTime.Now, "Information");
                                                            Console.WriteLine("Creating subsite for " + matterNumber);
                                                            CreateSPWeb(item, datarow["LMUserID"].ToString());

                                                            LogEvent(sw, "Item processed. Updating staging table", DateTime.Now, "Information");
                                                            Console.WriteLine("Item processed. Updating staging table.");
                                                            SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
                                                            updateCommand.ExecuteNonQuery();
                                                        }
                                                        //If there is no match, but the record is marked as inactive, it can be skipped
                                                        else
                                                        {
                                                            LogEvent(sw, matterNumber + " is marked as not active, skipping", DateTime.Now, "Information");
                                                            Console.WriteLine(matterNumber + " is marked as not active, skipping");
                                                            SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
                                                            updateCommand.ExecuteNonQuery();
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    LogEvent(sw, matterNumber + "is being skipped because " + datarow["LMUserID"].ToString() + " is not a litigation manager", DateTime.Now, "Warning");
                                                    Console.WriteLine(matterNumber + "is being skipped because " + datarow["LMUserID"].ToString() + " is not a litigation manager");
                                                    SqlCommand updateCommand = new SqlCommand(updateStatement, connection);
                                                    updateCommand.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    LogEvent(sw, ex.Message, DateTime.Now, "Warning");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        LogEvent(sw, ex.Message, DateTime.Now, "Warning");
                    }
                }
                catch (Exception ex)
                {
                    LogEvent(sw, ex.Message, DateTime.Now, "Critical");
                }
            }
            catch (Exception ex)
            {
                LogEvent(sw, ex.Message, DateTime.Now, "Critical");
            }

            connection.Close();
            sw.Close();
            Console.WriteLine("Successfully completed");
        }

        /// <summary>
        /// Create an SPListItem that has a BDC column, and update the secondary columns
        /// </summary>
        /// <param name="site"></param>
        /// <param name="matterNumber"></param>
        /// <param name="list"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static SPListItem CreateSPListItem(SPSite site, string matterNumber, SPList list, SPContentType contentType)
        {
            //Create the item and set the content type
            SPListItem item = list.Items.Add();
            item["ContentTypeId"] = contentType.Id;
            //Get a reference to the BCS column
            SPBusinessDataField dataField = item.Fields["Matter Number"] as SPBusinessDataField;
            //Set all of the secondary fields underneath the primary BCS column
            SetSecondaryFields(item, dataField, GetEntityInstance(site, System.Configuration.ConfigurationManager.AppSettings["BDCNamespace"], "Litigation Matter", matterNumber));
            item.Update();
            //Set the matter site URL
            item["Matter Site"] = "/sites/litman/" + item.ID;
            item.Update();
            return item;
        }

        /// <summary>
        /// Update an SPListItem that has a BDC column, and update the secondary columns
        /// </summary>
        /// <param name="site"></param>
        /// <param name="matterNumber"></param>
        /// <param name="list"></param>
        /// <param name="item"></param>
        /// <param name="contentType"></param>
        /// <returns></returns>
        public static SPListItem UpdateSPListItem(SPSite site, string matterNumber, SPList list, SPListItem item, SPContentType contentType)
        {
            //Set the content type of the matter item
            item["ContentTypeId"] = contentType.Id;
            //Get a reference to the BCS column
            SPBusinessDataField dataField = item.Fields["Matter Number"] as SPBusinessDataField;
            //Set all of the secondary fields underneath the primary BCS column
            SetSecondaryFields(item, dataField, GetEntityInstance(site, System.Configuration.ConfigurationManager.AppSettings["BDCNamespace"], "Litigation Matter", matterNumber));
            item.Update();
            //Set the matter site URL
            item["Matter Site"] = "/sites/litman/" + item.ID;
            item.Update();
            return item;
        }

        public static bool IsLitigationManager(SPWeb web, string userName)
        {
            // Get a reference to the lookup list that matches domain accounts to SP groups
            SPList lookupList = web.Lists[System.Configuration.ConfigurationManager.AppSettings["PermissionListLookup"]];

            // Construct a query to get the mapping from the current username to the appropriate group
            string lookupQuery = "<OrderBy>"
                                    + "<FieldRef Name=\"ID\"/>"
                                + "</OrderBy>"
                                + "<Where>"
                                    + "<Eq>"
                                        + "<FieldRef Name=\"Title\"/>"
                                        + "<Value Type=\"Text\">" + userName + "</Value>"
                                    + "</Eq>"
                                + "</Where>";
            SPQuery spLookupQuery = new SPQuery();
            spLookupQuery.Query = lookupQuery;
            SPListItemCollection itemCollection = lookupList.GetItems(spLookupQuery);
            if (itemCollection.Count < 1)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Update the permissions on the SP list item
        /// </summary>
        /// <param name="item"></param>
        /// <param name="userName"></param>
        public static void UpdateSPPermissions(SPListItem item, string userName)
        {
            //Get a reference to the lookup list that matches domain accounts to SP groups
            SPList lookupList = item.Web.Lists[System.Configuration.ConfigurationManager.AppSettings["PermissionListLookup"]];

            //Construct a query to get the mapping from the current username to the appropriate group
            string lookupQuery = "<OrderBy>"
                                    + "<FieldRef Name=\"ID\"/>"
                               + "</OrderBy>"
                               + "<Where>"
                                    + "<Eq>"
                                        + "<FieldRef Name=\"Title\"/>"
                                        + "<Value Type=\"Text\">" + userName + "</Value>"
                                    + "</Eq>"
                               + "</Where>";
            SPQuery spLookupQuery = new SPQuery();
            spLookupQuery.Query = lookupQuery;
            SPListItemCollection itemCollection = lookupList.GetItems(spLookupQuery);
            string groupName = itemCollection[0]["Group Name"].ToString();
            string adminGroupName = System.Configuration.ConfigurationManager.AppSettings["AdminGroupName"];

            //Set the permissions on the item for the current litigation manager's group, as well as the admin group
            SPGroup managerGroup = item.Web.Groups[groupName];
            SPGroup adminGroup = item.Web.Groups[adminGroupName];

            item.BreakRoleInheritance(false);
            SPRoleDefinition managerRole = item.Web.RoleDefinitions["Contribute"];
            SPRoleDefinition adminRole = item.Web.RoleDefinitions["Full Control"];
            SPRoleAssignment managerAssignment = new SPRoleAssignment(managerGroup);
            SPRoleAssignment adminAssignment = new SPRoleAssignment(adminGroup);
            managerAssignment.RoleDefinitionBindings.Add(managerRole);
            adminAssignment.RoleDefinitionBindings.Add(adminRole);

            item.RoleAssignments.Add(managerAssignment);
            item.RoleAssignments.Add(adminAssignment);
            item.Update();
        }

        /// <summary>
        /// Creates an SPWeb object
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public static void CreateSPWeb(SPListItem item, string managerUserName)
        {
            //Construct the Url, display name and language ID for the site
            string newWebUrl = item.ID.ToString();
            string newWebTitle = null;
            if (item["Matter Number: Matter Name"] != null)
            {
                newWebTitle = item["Matter Number: Matter Name"].ToString();
            }
            else if (item["Matter Number: Account Name"] != null)
            {
                newWebTitle = item["Matter Number: Account Name"].ToString();
            }
            string newWebDescription = string.Empty;
            uint newWebLcid = item.Web.Language;

            //Get the ID of the web template for Matter sites
            SPWebTemplateCollection templateCollection = item.Web.GetAvailableWebTemplates(item.Web.Language);
            SPWebTemplate newWebTemplate = null;
            foreach (SPWebTemplate currentTemplate in templateCollection)
            {
                if (currentTemplate.Title == "Matter Template")
                {
                    newWebTemplate = currentTemplate;
                }
            }

            //Create the new web using the Matter Template as a web template
            using (SPWeb newWeb = item.Web.Webs.Add(newWebUrl, newWebTitle, string.Empty, newWebLcid, newWebTemplate, true, false))
            {

                //Apply the same permissions that are on the list item to the subsite
                SPRoleAssignmentCollection newWebRoles = item.RoleAssignments;
                foreach (SPRoleAssignment currentRole in newWebRoles)
                {
                    newWeb.RoleAssignments.Add(currentRole);
                }
                newWeb.Update();

                //Create a matter summary object on the subsite
                SPListItem matterSummary = newWeb.Lists["Matter summary"].AddItem();
                matterSummary["Account Name"] = item["Matter Number: Account Name"];

                //Affiliate is a multi-value managed metadata value column, can't update display value directly
                if (item["Matter Number: Affiliate"] != null)
                {
                    string affiliatesConcat = item["Matter Number: Affiliate"].ToString();
                    string[] affiliates = affiliatesConcat.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    UpdateMultiMMField(affiliates, matterSummary, "Affiliate");
                }

                matterSummary["Case Caption"] = item["Matter Number: Case Caption"];
                matterSummary["Docket Number"] = item["Matter Number: Docket Number"];

                //Manager is a person/group field, must update with an actual SPUser object
                if (!string.IsNullOrEmpty(managerUserName))
                {
                    if (!managerUserName.StartsWith(@"i:0#.w|TRG"))
                    {
                        managerUserName = @"i:0#.w|TRG\" + managerUserName;
                    }
                    SPUser manager = null;
                    try
                    {
                        manager = item.Web.AllUsers[managerUserName];
                    }
                    catch (Exception ex)
                    {
                        manager = item.Web.EnsureUser(@"i:0#.w|TRG\Legacy");
                    }
                    matterSummary["Litigation Manager"] = manager;
                }

                //Litigation type is a managed metadata value column, can't update display value directly
                if (item["Matter Number: Litigation Type"] != null)
                {
                    UpdateMMField(item["Matter Number: Litigation Type"].ToString(), matterSummary, "Litigation Type");
                }

                matterSummary["Matter Name"] = item["Matter Number: Matter Name"];
                matterSummary["Matter Status"] = item["Matter Number: Matter Status"];

                //State filed is a managed metadata value column, can't update display value directly
                if (item["Matter Number: State Filed"] != null)
                {
                    UpdateMMField(item["Matter Number: State Filed"].ToString(), matterSummary, "State Filed");
                }

                //Venue is a managed metadata value column, can't update display value directly
                if (item["Matter Number: Venue"] != null)
                {
                    UpdateMMField(item["Matter Number: Venue"].ToString(), matterSummary, "Venue");
                }

                //Work matter type is a multi-value managed metadata value column, can't update display value directly
                if (item["Matter Number: Work/Matter Type"] != null)
                {
                    string workmatterTypeConcat = item["Matter Number: Work/Matter Type"].ToString();
                    string[] workmatterTypes = workmatterTypeConcat.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    UpdateMultiMMField(workmatterTypes, matterSummary, "Work/Matter Type");
                }
                matterSummary.Update();
            }
        }

        public static SPWeb UpdateSPWeb(SPListItem item, string managerUserName)
        {
            //Update the subsite's title
            string url = item["Matter Site"].ToString().Substring(item["Matter Site"].ToString().IndexOf(',') + 2);
            using (SPWeb updateWeb = item.Web.Site.AllWebs[url])
            {
                if (!updateWeb.Exists)
                {
                    CreateSPWeb(item, managerUserName);
                }
            }
            using (SPWeb updateWeb = item.Web.Site.AllWebs[url])
            {
                string newWebTitle = null;
                if (item["Matter Number: Matter Name"] != null)
                {
                    newWebTitle = item["Matter Number: Matter Name"].ToString();
                }
                else if (item["Matter Number: Account Name"] != null)
                {
                    newWebTitle = item["Matter Number: Account Name"].ToString();
                }
                updateWeb.Title = newWebTitle;

                //Apply the same permissions that are on the list item to the subsite
                SPRoleAssignmentCollection newWebRoles = item.RoleAssignments;
                foreach (SPRoleAssignment currentRole in newWebRoles)
                {
                    updateWeb.RoleAssignments.Add(currentRole);
                }
                updateWeb.Update();

                //Update the matter summary list item on the subsite
                SPListItem matterSummary = null;
                if (updateWeb.Lists["Matter summary"].ItemCount > 0)
                {
                    matterSummary = updateWeb.Lists["Matter summary"].Items[0];
                }
                else matterSummary = updateWeb.Lists["Matter summary"].AddItem();
                matterSummary["Account Name"] = item["Matter Number: Account Name"];

                //Affiliate is a multi-value managed metadata value column, can't update display value directly
                if (item["Matter Number: Affiliate"] != null)
                {
                    string affiliatesConcat = item["Matter Number: Affiliate"].ToString();
                    string[] affiliates = affiliatesConcat.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    UpdateMultiMMField(affiliates, matterSummary, "Affiliate");
                }

                matterSummary["Case Caption"] = item["Matter Number: Case Caption"];
                matterSummary["Docket Number"] = item["Matter Number: Docket Number"];

                //Manager is a person/group field, must update with an actual SPUser object
                if (!string.IsNullOrEmpty(managerUserName))
                {
                    if (!managerUserName.StartsWith(@"i:0#.w|TRG"))
                    {
                        managerUserName = @"i:0#.w|TRG\" + managerUserName;
                    }
                    SPUser manager = null;
                    try
                    {
                        manager = item.Web.AllUsers[managerUserName];
                    }
                    catch (Exception ex)
                    {
                        manager = item.Web.EnsureUser(@"i:0#.w|TRG\Legacy");
                    }
                    matterSummary["Litigation Manager"] = manager;
                }

                //Litigation type is a managed metadata value column, can't update display value directly
                if (item["Matter Number: Litigation Type"] != null)
                {
                    UpdateMMField(item["Matter Number: Litigation Type"].ToString(), matterSummary, "Litigation Type");
                }

                matterSummary["Matter Name"] = item["Matter Number: Matter Name"];
                matterSummary["Matter Status"] = item["Matter Number: Matter Status"];

                //State filed is a managed metadata value column, can't update display value directly
                if (item["Matter Number: State Filed"] != null)
                {
                    UpdateMMField(item["Matter Number: State Filed"].ToString(), matterSummary, "State Filed");
                }

                //Venue is a managed metadata value column, can't update display value directly
                if (item["Matter Number: Venue"] != null)
                {
                    UpdateMMField(item["Matter Number: Venue"].ToString(), matterSummary, "Venue");
                }

                //Work matter type is a multi-value managed metadata value column, can't update display value directly
                if (item["Matter Number: Work/Matter Type"] != null)
                {
                    string workmatterTypeConcat = item["Matter Number: Work/Matter Type"].ToString();
                    string[] workmatterTypes = workmatterTypeConcat.Split(',').Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).ToArray();
                    UpdateMultiMMField(workmatterTypes, matterSummary, "Work/Matter Type");
                }
                matterSummary.Update();
                return updateWeb;
            }
        }
        /// <summary>
        /// Updates a single-value managed metatada column
        /// </summary>
        /// <param name="taxonomyTerm"></param>
        /// <param name="item"></param>
        /// <param name="fieldToUpdate"></param>
        public static void UpdateMMField(string taxonomyTerm, SPListItem item, string fieldToUpdate)
        {
            //Get the metadata taxonomy field, a taxonomy session, the term store, and a collection of term sets in the store
            TaxonomyField managedMetadataField = item.ParentList.Fields[fieldToUpdate] as TaxonomyField;
            Guid tsId = managedMetadataField.TermSetId;
            Guid termStoreId = managedMetadataField.SspId;
            TaxonomySession tSession = new TaxonomySession(item.ParentList.ParentWeb.Site);
            TermStore tStore = tSession.TermStores[termStoreId];
            TermSet tSet = tStore.GetTermSet(tsId);
            TermCollection terms = tSet.GetTerms(taxonomyTerm, false);
            Term term = null;

            //If term doesn't exist, create it in the term store
            if (terms.Count == 0)
            {
                Console.WriteLine("Creating term in managed metadata, {0}", taxonomyTerm);
                term = tSet.CreateTerm(taxonomyTerm, tStore.Languages[0]);
                tStore.CommitAll();
            }
            else
            {
                term = terms[0];
            }

            //Set the managed metadata field to the term retrieved from the term store
            managedMetadataField.SetFieldValue(item, term);
            item.Update();
        }

        /// <summary>
        /// Updates a multi-value managed metatada column
        /// </summary>
        /// <param name="taxonomyTerms"></param>
        /// <param name="item"></param>
        /// <param name="fieldToUpdate"></param>
        public static void UpdateMultiMMField(string[] taxonomyTerms, SPListItem item, string fieldToUpdate)
        {
            //Get the metadata taxonomy field, a taxonomy session, the term store, and a collection of term sets in the store
            TaxonomyField multipleManagedMetadataField = item.ParentList.Fields[fieldToUpdate] as TaxonomyField;
            Guid tsId = multipleManagedMetadataField.TermSetId;
            Guid termStoreId = multipleManagedMetadataField.SspId;
            TaxonomySession tSession = new TaxonomySession(item.ParentList.ParentWeb.Site);
            TermStore tStore = tSession.TermStores[termStoreId];
            TermSet tSet = tStore.GetTermSet(tsId);
            TaxonomyFieldValueCollection termCollection = new TaxonomyFieldValueCollection(multipleManagedMetadataField);

            //Loop through each value being added to the metadata field
            foreach (string t in taxonomyTerms)
            {
                TermCollection terms = tSet.GetTerms(t, false);
                Term term = null;
                //If there are no matching terms in the term store, create one
                if (terms.Count == 0)
                {
                    Console.WriteLine("Creating term in managed metadata, {0}", t);
                    term = tSet.CreateTerm(t, tStore.Languages[0]);
                    tStore.CommitAll();
                }
                else
                {
                    term = terms[0];
                }

                //Add the current term to a term collection
                TaxonomyFieldValue termValue = new TaxonomyFieldValue(multipleManagedMetadataField);
                termValue.TermGuid = term.Id.ToString();
                termValue.Label = term.Name;
                termCollection.Add(termValue);
            }

            //Add the term collection to the metadata field
            multipleManagedMetadataField.SetFieldValue(item, termCollection);
            item.Update();
        }

        /// <summary>
        /// Get the instance of a Business Data Connectivity entity
        /// </summary>
        /// <param name="site"></param>
        /// <param name="nameSpace"></param>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <returns></returns>
        private static IEntityInstance GetEntityInstance(SPSite site, string nameSpace, string entityName, string entityId)
        {
            //Use the scope of the currently opened site
            SPServiceContext ctx = SPServiceContext.GetContext(site);
            SPServiceContextScope scope = new SPServiceContextScope(ctx);

            //Get the BDC service of the local SP farm
            BdcService service = SPFarm.Local.Services.GetValue<BdcService>();
            IMetadataCatalog catalog = service.GetDatabaseBackedMetadataCatalog(ctx);
            IEntity entity = catalog.GetEntity(nameSpace, entityName);
            ILobSystemInstance LobSysteminstance = entity.GetLobSystem().GetLobSystemInstances()[0].Value;
            IEntityInstance entInstance = null;

            //Loop through the methods defined in the LOB
            foreach (KeyValuePair<string, IMethod> method in entity.GetMethods())
            {
                IMethodInstance methodInstance = method.Value.GetMethodInstances()[method.Key];
                //Get the Specific Finder method of the LOB
                if (methodInstance.MethodInstanceType == MethodInstanceType.SpecificFinder)
                {
                    //Find the record with the ID from the datasource
                    Microsoft.BusinessData.Runtime.Identity id = new Microsoft.BusinessData.Runtime.Identity(entityId);
                    entInstance = entity.FindSpecific(id, entity.GetLobSystem().GetLobSystemInstances()[0].Value);
                }
            }

            return entInstance;
        }

        /// <summary>
        /// Set the secondary fields of a BDC field
        /// </summary>
        /// <param name="listItem"></param>
        /// <param name="dataField"></param>
        /// <param name="entityInstance"></param>
        private static void SetSecondaryFields(SPListItem listItem, SPBusinessDataField dataField, IEntityInstance entityInstance)
        {
            //Copy the fieldset to a datatable
            DataTable dtBDCData = entityInstance.EntityAsFormattedDataTable;

            //Update the BDC primary field ID with the primary field in the datatable
            listItem[dataField.Id] = dtBDCData.Rows[0][dataField.BdcFieldName].ToString();

            //Get the Specific Finder method of the BDC entity and get the collection of columns/descriptors
            IMethodInstance method = entityInstance.Entity.GetMethodInstances(MethodInstanceType.SpecificFinder)[0].Value;
            ITypeDescriptorCollection oDescriptors = method.GetReturnTypeDescriptor().GetChildTypeDescriptors()[0].GetChildTypeDescriptors();

            //Loop through each Type Descriptor in the specific finder instance and update the local datatable column names appropriately
            foreach (ITypeDescriptor oType in oDescriptors)
            {
                if (oType.ContainsLocalizedDisplayName())
                {
                    if (dtBDCData.Columns.Contains(oType.Name))
                    {
                        dtBDCData.Columns[oType.Name].ColumnName = oType.GetLocalizedDisplayName();
                    }
                }
            }

            //Loop through each column name in the data source and update the corresponding list item column
            string[] sSecondaryFieldsDisplayNames = dataField.GetSecondaryFieldsNames();
            foreach (string columnNameint in sSecondaryFieldsDisplayNames)
            {
                Guid gFieldID = listItem.Fields[String.Format("{0}: {1}", dataField.Title, dtBDCData.Columns[columnNameint].Caption)].Id;
                listItem[gFieldID] = dtBDCData.Rows[0][columnNameint].ToString();
            }

            //Set the final values of the list item linking the item to its actual BDC Identity (allows for manually refreshing the list)
            listItem[dataField.Id] = dtBDCData.Rows[0][dataField.BdcFieldName].ToString();
            listItem[dataField.RelatedField] = dtBDCData.Rows[0]["BdcIdentity"].ToString();
        }

        public static void LogEvent(StreamWriter stream, string message, DateTime date, string category)
        {
            string messageToLog = DateTime.Now.ToLongTimeString() + " - " + category + " - " + message;
            stream.WriteLine(messageToLog);
        }
    }
}
