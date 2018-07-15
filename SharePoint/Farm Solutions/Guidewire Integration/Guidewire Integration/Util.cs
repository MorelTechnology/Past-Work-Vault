using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using System.Xml.Linq;
using System.Diagnostics;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Globalization;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint.BusinessData.SharedService;
using Microsoft.SharePoint.Administration;
using Microsoft.BusinessData.Runtime;
using Microsoft.SharePoint.BusinessData.Infrastructure;
using Microsoft.BusinessData.MetadataModel.Collections;
using System.Text.RegularExpressions;
using System.Xml.Schema;
using System.Xml;
using System.Collections.Specialized;
using System.Xml.Serialization;
using System.IO;
using System.Reflection;

namespace Guidewire_Integration
{
    class Util
    {
        #region Private Properties
        private static SPListItem thisItem;
        #endregion

        #region Enums
        public enum GuidewireOperationType
        {
            New = 1,
            Update = 2,
            Delete = 3
        }

        public enum ErrorLevel
        {
            Info,
            Warning,
            Error
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Checks if a given item has a duplicate that is marked as final in either SharePoint or ClaimCenter.
        /// </summary>
        /// <param name="item">The SPListItem to check</param>
        /// <param name="existingDocId">If a duplicate is found in SharePoint, writes out the document ID</param>
        /// <param name="existingDocIdUrl">f a duplicate is found in SharePoint, writes out the document ID URL</param>
        /// <returns></returns>
        public static GuidewireOperationType CheckDuplicate(SPListItem item, out string existingDocId, out string existingDocIdUrl)
        {
            // Check if the document exists in SharePoint. If there is a duplicate, and the duplicate is already final, an exception is thrown
            GuidewireOperationType operation = SPDocExistsInDestinationLibrary(item, out existingDocId, out existingDocIdUrl);
            thisItem = item;
            string docIdToCheck = item["_dlc_DocId"].ToString();
            // If there was a document found in SharePoint, we will be checking that document's ID
            if (existingDocId != null) { docIdToCheck = existingDocId; }
            switch (item["Content Type"].ToString())
            {
                case "Guidewire Work Matter Document":
                    // Check if the document exists in Guidewire. Throw an exception if it already exists and is marked as final
                    GWBCS.WorkMatterDocument wmDoc = GetGWWorkMatterDocById(docIdToCheck);
                    if (wmDoc != null)
                    {
                        if (wmDoc.Status == "Final")
                        {
                            throw new Exception(string.Format("Document with ID {0} already exists in ClaimCenter and is marked as Final. It cannot be overwritten.", item["_dlc_DocId"].ToString()));
                        }
                        else
                        {
                            operation = GuidewireOperationType.Update;
                        }
                    }
                    break;
                case "Guidewire Vendor Document":
                    // Check if the document exists in Guidewire. Throw an exception if it already exists and is marked as final
                    GWBCS.VendorDocument vendorDoc = GetGWVendorDocById(docIdToCheck);
                    if (vendorDoc != null)
                    {
                        if (vendorDoc.Status == "Final")
                        {
                            throw new Exception(string.Format("Document with ID {0} already exists in ContactManager and is marked as Final. It cannot be overwritten.", item["_dlc_DocId"].ToString()));
                        }
                        else
                        {
                            operation = GuidewireOperationType.Update;
                        }
                    }
                    break;
                default:
                    throw new SPException("Content type was not recognized");
            }

            return operation;
        }

        /// <summary>
        /// Checks if a document from the drop off library exists in its respective destination library
        /// </summary>
        /// <param name="item">The SPListItem to check</param>
        /// <param name="existingDocId">If a duplicate is found, writes out the existing document's ID</param>
        /// <param name="existingDocIdUrl">If a duplicate is found, writes out the existing document's ID URL</param>
        /// <returns></returns>
        public static GuidewireOperationType SPDocExistsInDestinationLibrary(SPListItem item, out string existingDocId, out string existingDocIdUrl)
        {
            // Assumed that this document doesn't exist yet, and that the operation type is 'New'
            GuidewireOperationType operation = GuidewireOperationType.New;
            existingDocId = null;
            existingDocIdUrl = null;

            // Split up the document name and extension so it can be formatted using a proper naming convention
            string fileExtension = "." + item[SPBuiltInFieldId.File_x0020_Type].ToString().ToLower();
            string fileBaseName = item.File.Name.Substring(0, item.File.Name.ToLower().LastIndexOf(fileExtension));
            string newFileName = fileBaseName;
            string destinationListUrl = item.Web.Url + "/";
            string statusField;
            switch (item.ContentType.Name)
            {
                case "Guidewire Work Matter Document":
                    newFileName += " - " + item[Resource.FieldWorkMatter].ToString() + fileExtension;
                    destinationListUrl += item.Web.Lists[Resource.WorkMatterDocumentsLibrary].RootFolder.Url;
                    statusField = Resource.FieldBCSWorkMatterDocumentStatus;
                    break;
                case "Guidewire Vendor Document":
                    newFileName += " - " + item[Resource.FieldVendor].ToString() + fileExtension;
                    destinationListUrl += item.Web.Lists[Resource.VendorDocumentsLibrary].RootFolder.Url;
                    statusField = Resource.FieldBCSVendorDocumentStatus;
                    break;
                default:
                    throw new SPException("Document was not an accepted content type");
            }

            // Try to get the possible existing file at the expected destination
            SPFile existingFile = item.Web.GetFile(destinationListUrl + "/" + newFileName);
            if (existingFile.Exists)
            {
                // Throw an exception if the document exists and it's marked as final
                if ((existingFile.Item.Fields.ContainsField(statusField) && existingFile.Item[statusField] != null && existingFile.Item[statusField].ToString() == "Final") || Records.IsRecord(existingFile.Item))
                {
                    throw new Exception(string.Format("Document named {0} has already been uploaded to SharePoint and has been declared as a record. It cannot be overwritten.", newFileName));
                }
                else
                {
                    // Document exists but it's not marked as final. We should be able to overwrite it
                    operation = GuidewireOperationType.Update;
                    existingDocId = existingFile.Item["_dlc_DocId"].ToString();
                    existingDocIdUrl = existingFile.Item["_dlc_DocIdUrl"].ToString();
                }
            }

            return operation;
        }

        /// <summary>
        /// Gets a Work Matter document
        /// </summary>
        /// <param name="docId">The document ID to retrieve</param>
        /// <returns></returns>
        public static GWBCS.WorkMatterDocument GetGWWorkMatterDocById(string docId)
        {
            // Get the WCF endpoint address from the configuration SPList
            EndpointAddress endpointAddress = new EndpointAddress(Configuration.GetConfigurationValue(thisItem.Web, Resource.ConfigGWBCSEndpoint));

            // Establish a binding, and the execute the find method
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            GWBCS.GuidewireClient client = new GWBCS.GuidewireClient(binding, endpointAddress);
            return client.GetWorkMatterDocumentBySPID(docId);
        }

        /// <summary>
        /// Gets a Vendor Document
        /// </summary>
        /// <param name="docId">The document ID to retrieve</param>
        /// <returns></returns>
        public static GWBCS.VendorDocument GetGWVendorDocById(string docId)
        {
            // Get the WCF endpoint address from the configuration SPList
            EndpointAddress endpointAddress = new EndpointAddress(Configuration.GetConfigurationValue(thisItem.Web, Resource.ConfigGWBCSEndpoint));

            // Establish a binding, and the execute the find method
            BasicHttpBinding binding = new BasicHttpBinding();
            binding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            binding.Security.Transport.ClientCredentialType = HttpClientCredentialType.None;
            GWBCS.GuidewireClient client = new GWBCS.GuidewireClient(binding, endpointAddress);
            return client.GetVendorDocumentBySPID(docId);
        }

        /// <summary>
        /// Call the Guidewire Update Metadata service for a SPListItem and an operation type
        /// </summary>
        /// <param name="item">The SPListItem to send to ClaimCenter</param>
        /// <param name="type">The Guidewire Operation Type</param>
        /// <param name="manualDocId">Optional. An overridable document ID in the case that the original document ID is changing</param>
        /// <returns></returns>
        public static Boolean CallGuideWire(SPListItem item, GuidewireOperationType type, string manualDocId = null)
        {
            Boolean success = false;
            // Create the Guidewire WS Client
            EndpointAddress endpointAddress = new EndpointAddress(Configuration.GetConfigurationValue(item.ParentList.ParentWeb, Resource.ConfigGWWsEndpoint));
            System.ServiceModel.Channels.Binding binding = CreateBinding();
            GuidewireWS.trg_UpdateMetadataAPIPortTypeClient client = new GuidewireWS.trg_UpdateMetadataAPIPortTypeClient(binding, endpointAddress);
            // Create authentication
            GuidewireWS.authentication authentication = CreateAuthentication(item.ParentList.ParentWeb);
            GuidewireWS.locale locale = new GuidewireWS.locale();
            locale.Value = "en_US"; //CultureInfo.CurrentCulture.ToString(); // 
            // Create document info
            GuidewireWS.trg_DocumentInfo documentInfo = new GuidewireWS.trg_DocumentInfo();
            documentInfo.operation = (int)type;
            if (manualDocId != null)
            {
                documentInfo.documentID = manualDocId;
            }
            else
            {
                documentInfo.documentID = Util.GetDocumentId(item);
            }
            List<GuidewireWS.trg_MetadataPair> metadata = CreateMetadata(item);
            if (metadata != null)
            {
                documentInfo.metadata = metadata.ToArray();
                // Call Guidewire
                try
                {
                    success = client.updateMetadata(authentication, locale, documentInfo);
                    if (!success)
                        Util.LogError("Call to Guidewire was unsuccessful operation type = " + type.ToString());
                }
                catch (Exception e)
                {
                    success = false;
                    LogError("CallGuidewire failed with exception:  " + e.Message);
                }
            }
            else
                return false;

            return success;
        }

        /// <summary>
        /// Gets the document ID on a given SPListItem
        /// </summary>
        /// <param name="listItem">The SPListItem to get the document ID for</param>
        /// <returns></returns>
        public static String GetDocumentId(SPListItem listItem)
        {
            string documentId = string.Empty;
            if (listItem.Fields.ContainsField(Resource.FieldDocumentId) && listItem.Fields[Resource.FieldDocumentId].Type == SPFieldType.URL)
            {
                SPFieldUrlValue urlValue = new SPFieldUrlValue((string)listItem[Resource.FieldDocumentId]);
                documentId = urlValue.Description;
            }
            return documentId;
        }

        /// <summary>
        /// Gets a SPUser object for a given login name
        /// </summary>
        /// <param name="userLogin">The loginName to look for</param>
        /// <returns></returns>
        public static SPUser GetUserFromLogin(string userLogin)
        {
            // TODO: We get a lot of warnings on these, and the Guidewire Service Account is often reflected in SharePoint. This method could use some work. Not critical ATM
            SPUser user = null;
            if (userLogin != null)
            {
                try
                {
                    SPUserCollection allUsers = SPContext.Current.Web.AllUsers;
                    var xml = XDocument.Parse(allUsers.Xml);
                    var xmlUsers = (from item in xml.Descendants("User")
                                    where (String)item.Attribute("LoginName") == userLogin
                                    select item);
                    if (xmlUsers.Count<XElement>() > 0) // should only be one, if less than zero user could not be found
                    {
                        int userID = (int)xmlUsers.First().Attribute("ID");
                        user = allUsers.GetByID(userID);
                    }
                    else
                        LogError(String.Format("Login {0} could not be located in SharePoint Users", userLogin == null ? "null" : userLogin), ErrorLevel.Warning);
                }
                catch (Exception e)
                {
                    LogError("GetUserFromLogin errored with the exception: " + e.Message);
                }
            }
            return user;
        }

        /// <summary>
        /// Declares an SPListItem as a record
        /// </summary>
        /// <param name="item">The SPListItem to declare as a record</param>
        public static void LockItem(SPListItem item)
        {
            // Declare the item a record if it isn't already
            if (!Records.IsLocked(item))
            {
                Records.DeclareItemAsRecord(item);
            }
        }

        /// <summary>
        /// Uses the MIMETypes class to match extensions to known MimeTypes from Guidewire. If changes are made to the document name, the new document URL is returned
        /// </summary>
        /// <param name="documentUrl">The URL of the document to resolve</param>
        /// <param name="mimeType">The MimeType as it exists in ClaimCenter</param>
        /// <returns></returns>
        public static String ResolveMissingExtension(string documentUrl, string mimeType)
        {
            List<string> extension = new List<string>();
            string newDocUrl = documentUrl;

            //if (Path.HasExtension(newDocUrl)) return newDocUrl;

            // Get the list of known mime types/extensions
            using (MIMETypes mimetypes = new MIMETypes())
            {
                if (mimetypes.ContainsMimetype(mimeType))
                    extension = mimetypes.ExtensionMapping[mimeType];
            }

            // Update the file in SharePoint if no matching extensions were found, or if the mimetype wasn't unknown
            // (mimetypes of unknown should be left alone, Guidewire doesn't know what they are, and we should leave it up to SharePoint to figure out what the file is)
            if (extension.Count > 0 || !extension.Contains("unknown"))
            {
                try
                {
                    using (SPWeb shellWeb = SPContext.Current.Web)
                    {
                        SPSecurity.RunWithElevatedPrivileges(delegate ()
                        {
                            using (SPSite site = new SPSite(shellWeb.Site.Url))
                            {
                                using (SPWeb web = site.OpenWeb())
                                {
                                    web.AllowUnsafeUpdates = true;
                                    SPListItem item = web.GetListItem(documentUrl);
                                    // Bypass the record if needed to, and use the MoveTo method to rename the document
                                    Records.BypassLocks(item, delegate (SPListItem bypassItem)
                                    {
                                        bypassItem.File.MoveTo((documentUrl + extension.First()), true);
                                    });
                                    newDocUrl = documentUrl + extension.First();
                                    web.AllowUnsafeUpdates = false;
                                }
                            }
                        });
                    }
                }
                catch (Exception e)
                {
                    Util.LogError("Adding an extension to the document {1} failed with exception: " + e.Message);
                }
            }

            return newDocUrl;
        }

        /// <summary>
        /// Write message to Event Viewer Application log as Error
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(String message)
        {
            LogError(message, ErrorLevel.Error);
        }

        /// <summary>
        /// Write message to Event Viewer Application log as Error, Warning or Information
        /// </summary>
        /// <param name="message">The string message to write</param>
        /// <param name="errorLevel">The level of error that should be logged</param>
        public static void LogError(String message, Util.ErrorLevel errorLevel)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
                {
                    // Map the event log entry type to the error level
                    EventLogEntryType logErrorLevel;
                    switch (errorLevel)
                    {
                        case ErrorLevel.Error:
                            logErrorLevel = EventLogEntryType.Error;
                            break;
                        case ErrorLevel.Info:
                            logErrorLevel = EventLogEntryType.Information;
                            break;
                        case ErrorLevel.Warning:
                            logErrorLevel = EventLogEntryType.Warning;
                            break;
                        default:
                            logErrorLevel = EventLogEntryType.Error;
                            break;
                    }
                    try
                    {
                        // Write to the application log
                        var appLog = new EventLog { Source = Resource.ErrorSource };
                        appLog.WriteEntry(message, logErrorLevel, 42);
                    }
                    catch (Exception e)
                    {
                        Microsoft.Office.Server.Diagnostics.PortalLog.LogString("Guidewire Integration errored: " + e.Message + " Original message: " + message);
                    }
                });
        }

        /// <summary>
        /// Clears the flag indicating a document should be updated. Might not need
        /// </summary>
        /// <param name="item"></param>
        public static void ClearFields(SPListItem item)
        {
            // TODO: Can this be removed?
            if (item.Fields.ContainsField(Resource.FieldUpdateRequired))
            {
                item[Resource.FieldUpdateRequired] = "No";
            }
            item.SystemUpdate(false);
        }

        /// <summary>
        /// Routes a file to its destination
        /// </summary>
        /// <param name="item">The item to move</param>
        /// <param name="destination">The destination URL to move the item to</param>
        /// <returns></returns>
        public static bool RouteFile(SPListItem item, string destination)
        {
            bool success = false;
            SPWeb web = item.Web;
            // Check if there's already a file in the destination
            SPFile existingFile = web.GetFile(destination);
            if (existingFile.Exists)
            {
                // If there's a file there already and it's marked as a record, throw an exception
                if (Records.IsRecord(existingFile.Item))
                {
                    throw new Exception(string.Format("File {0} could not be routed to destination. An identical file already exists for the work matter/vendor and has been marked as final. The file cannot be overwritten.", item.File.Url));
                }

            }
            // Move the file
            item.File.MoveTo(destination, true);
            success = true;
            return success;
        }

        /// <summary>
        /// Clears illegal character names from a string
        /// </summary>
        /// <param name="strInput">The string to process</param>
        /// <returns></returns>
        public static string RemoveMetaCharacters(string strInput)
        {
            // The regex of invalid characters
            Regex invalidCharsRegex = new Regex(@"[\*\?\|\\\t/:""'<>#{}%~&]", RegexOptions.Compiled);
            // Replace those invalid characters with empties
            String str = invalidCharsRegex.Replace(strInput, String.Empty).Replace("\r", String.Empty).Replace("\n", String.Empty).Replace("\\", String.Empty).Trim();
            return str;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Creates a binding for the Guidewire update metadata service
        /// </summary>
        /// <returns></returns>
        private static Binding CreateBinding()
        {
            // Using no security for HTTP, this would need to change to transport security when calling HTTPS
            CustomBinding binding = new CustomBinding();

            // Create the message encoding
            TextMessageEncodingBindingElement encoding = new TextMessageEncodingBindingElement();
            encoding.MessageVersion = MessageVersion.Soap12;
            binding.Elements.Add(encoding);

            // Create the transport
            HttpTransportBindingElement transport = new HttpTransportBindingElement();
            binding.Elements.Add(transport);

            return binding;
        }

        /// <summary>
        /// Creates the authentication component used for calling the Guidewire Metadata Update service
        /// </summary>
        /// <param name="web">The SPWeb that the configuration objects are stored in</param>
        /// <returns></returns>
        private static GuidewireWS.authentication CreateAuthentication(SPWeb web)
        {
            GuidewireWS.authentication authentication = new GuidewireWS.authentication();
            // Use the configured values in the web's configuration list
            authentication.username = Configuration.GetConfigurationValue(web, Resource.ConfigGWWsUserName);
            authentication.password = Configuration.GetConfigurationValue(web, Resource.ConfigGWWsPassword);
            return authentication;
        }

        /// <summary>
        /// Maps metadata from a SPListItem to the metadata update message to be sent to the Guidewire metadata update service
        /// </summary>
        /// <param name="item">The SPListItem to get metadata from</param>
        /// <returns></returns>
        private static List<GuidewireWS.trg_MetadataPair> CreateMetadata(SPListItem item)
        {
            List<GuidewireWS.trg_MetadataPair> metadata = new List<GuidewireWS.trg_MetadataPair>();
            CreateMetadataPair(metadata, "name", "Title", item);
            if (item.Fields.ContainsField(Resource.FieldWorkMatter))
                CreateMetadataPair(metadata, "workmatter", Resource.FieldWorkMatter, item);
            else
                CreateMetadataPair(metadata, "trg_ContactID", Resource.FieldVendor, item);

            CreateMetadataPair(metadata, "author", "Created By", item);

            if (item.Fields.ContainsField(Resource.FieldCategory) && item[Resource.FieldCategory] != null)
            {
                CreateMetadataPair(metadata, "trg_Category", Resource.FieldCategory, item);
            }
            else if (item.Fields.ContainsField(Resource.FieldBCSWorkMatterDocument + ": Category") && item[Resource.FieldBCSWorkMatterDocument + ": Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_Category", Resource.FieldBCSWorkMatterDocument + ": Category", item);
            }
            else if (item.Fields.ContainsField(Resource.FieldBCSVendorDocument + ": Category") && item[Resource.FieldBCSVendorDocument + ": Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_Category", Resource.FieldBCSVendorDocument + ": Category", item);
            }
            if (item.Fields.ContainsField(Resource.FieldSubCategory) && item[Resource.FieldSubCategory] != null)
            {
                CreateMetadataPair(metadata, "trg_SubCategory", Resource.FieldSubCategory, item);
            }
            else if (item.Fields.ContainsField(Resource.FieldBCSWorkMatterDocument + ": Sub Category") && item[Resource.FieldBCSWorkMatterDocument + ": Sub Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_SubCategory", Resource.FieldBCSWorkMatterDocument + ": Sub Category", item);
            }
            else if (item.Fields.ContainsField(Resource.FieldBCSVendorDocument + ": Sub Category") && item[Resource.FieldBCSVendorDocument + ": Sub Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_SubCategory", Resource.FieldBCSVendorDocument + ": Sub Category", item);
            }
            if (item.Fields.ContainsField(Resource.FieldDocumentStatus) && item[Resource.FieldDocumentStatus] != null)
            {
                CreateMetadataPair(metadata, "status", Resource.FieldDocumentStatus, item);
            }
            else if (item.Fields.ContainsField(Resource.FieldBCSWorkMatterDocument + ": Status") && item[Resource.FieldBCSWorkMatterDocument + ": Status"] != null)
            {
                CreateMetadataPair(metadata, "status", Resource.FieldBCSWorkMatterDocument + ": Status", item);
            }
            else if (item.Fields.ContainsField(Resource.FieldBCSVendorDocument + ": Status") && item[Resource.FieldBCSVendorDocument + ": Status"] != null)
            {
                CreateMetadataPair(metadata, "status", Resource.FieldBCSVendorDocument + ": Status", item);
            }
            if (item.Fields.ContainsField(Resource.FieldDescription) && item[Resource.FieldDescription] != null)
            {
                CreateMetadataPair(metadata, "description", Resource.FieldDescription, item);
            }
            else if (item.Fields.ContainsField(Resource.FieldBCSWorkMatterDocument + ": Description") && item[Resource.FieldBCSWorkMatterDocument + ": Description"] != null)
            {
                CreateMetadataPair(metadata, "description", Resource.FieldBCSWorkMatterDocument + ": Description", item);
            }
            else if (item.Fields.ContainsField(Resource.FieldBCSVendorDocument + ": Description") && item[Resource.FieldBCSVendorDocument + ": Description"] != null)
            {
                CreateMetadataPair(metadata, "description", Resource.FieldBCSVendorDocument + ": Description", item);
            }

            CreateMetadataPair(metadata, "mimetype", "File Type", item);
            CreateMetadataPair(metadata, "dateModified", Resource.FieldModifiedDate, item);
            CreateMetadataPair(metadata, "dateCreated", Resource.FieldCreatedDate, item);
            
            return metadata;
        }

        /// <summary>
        /// Creates a metadata mapping for an individual property on a SPListItem
        /// </summary>
        /// <param name="metadata">The metadata collection to assemble</param>
        /// <param name="key">The key to use to send up to Guidewire</param>
        /// <param name="propertyName">The name of the property in SharePoint</param>
        /// <param name="item">The SPListItem to retrieve the property from</param>
        private static void CreateMetadataPair(List<GuidewireWS.trg_MetadataPair> metadata, string key, string propertyName, SPListItem item)
        {
            try
            {
                // Set up a new metadata pair
                GuidewireWS.trg_MetadataPair metadataPair = new GuidewireWS.trg_MetadataPair();
                SPListItem listItem = item;
                // Make sure the list item contains the field in question
                if (listItem.Fields.ContainsField(propertyName))
                {
                    metadataPair.key = key;
                    SPField propertyField = listItem.Fields[propertyName];
                    // Each field type in SharePoint requires slightly different handling to retrieve strings or inner properties
                    switch (propertyField.Type)
                    {
                        case SPFieldType.User:
                            {
                                SPFieldUser userField = propertyField as SPFieldUser;
                                SPFieldUserValue userFieldValue = (SPFieldUserValue)userField.GetFieldValue(listItem[propertyName].ToString());
                                if (userFieldValue != null && userFieldValue.User != null)
                                {
                                    metadataPair.value = userFieldValue.User.Name;
                                }
                                else
                                    metadataPair.value = (string)listItem[propertyName];
                            }
                            break;
                        case SPFieldType.Lookup:
                            {
                                SPFieldLookup lookupField = propertyField as SPFieldLookup;
                                SPFieldLookupValue lookupFieldValue = (SPFieldLookupValue)lookupField.GetFieldValue(listItem[propertyName].ToString());
                                if (lookupFieldValue != null && lookupFieldValue.LookupValue != null)
                                {
                                    metadataPair.value = lookupFieldValue.LookupValue;
                                }
                            }
                            break;
                        // Bamboo selectors have type of invalid but work like lookup
                        case SPFieldType.Invalid:
                            {
                                if (listItem[propertyName] != null)
                                {
                                    if (propertyField.TypeAsString == "BusinessData")
                                    {
                                        metadataPair.value = (string)listItem[propertyName];
                                    }
                                    else
                                    {
                                        SPFieldLookup lookupField = propertyField as SPFieldLookup;
                                        SPFieldLookupValue lookupFieldValue = (SPFieldLookupValue)lookupField.GetFieldValue(listItem[propertyName].ToString());
                                        if (lookupFieldValue != null && lookupFieldValue.LookupValue != null)
                                        {
                                            metadataPair.value = lookupFieldValue.LookupValue;
                                        }
                                    }
                                }
                            }
                            break;
                        case SPFieldType.DateTime:
                            {
                                metadataPair.value = listItem[propertyName].ToString();
                            }
                            break;
                        default:
                            if (listItem[propertyName] != null)
                            {
                                if (propertyName == "Name")
                                {
                                    String fileName = (string)listItem[propertyName];
                                    if (fileName.Length > 80)
                                    {
                                        String extension = fileName.Substring(fileName.LastIndexOf("."));
                                        String name = fileName.Substring(0, fileName.Length - extension.Length);
                                        fileName = name.Substring(0, 80 - extension.Length) + extension;
                                    }
                                    metadataPair.value = fileName;
                                }
                                else
                                    metadataPair.value = (string)listItem[propertyName];
                            }
                            break;
                    }
                    metadata.Add(metadataPair);
                }
            }
            catch (Exception e)
            {
                LogError("CreateMetadataPair failed with exception: " + e.Message);
            }
        }
        #endregion


        /// <summary>
        /// Serializes XML to a string dictionary
        /// </summary>
        public class SerializableStringDictionary : StringDictionary, IXmlSerializable
        {
            public XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(XmlReader reader)
            {
                while (reader.Read() &&
                    !(reader.NodeType == XmlNodeType.EndElement && reader.LocalName == this.GetType().Name))
                {
                    var name = reader["Name"];
                    if (name == null)
                        throw new FormatException();

                    var value = reader["Value"];
                    this[name] = value;
                }
            }

            public void WriteXml(XmlWriter writer)
            {
                foreach (var key in Keys)
                {
                    writer.WriteStartElement("Pair");
                    writer.WriteAttributeString("Name", (string)key);
                    writer.WriteAttributeString("Value", this[(string)key]);
                    writer.WriteEndElement();
                }
            }
        }
    }
}
