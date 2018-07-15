using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using System.Web;
using System.Reflection;

namespace Guidewire_Integration.DropOffLibraryItemChanged
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class DropOffLibraryItemChanged : SPItemEventReceiver
    {
        // Class-level variable for capturing the current execution context. This is needed to determine if updates/check-ins are happening from MacroView or the web UI
        private HttpContext currentContext;
        public DropOffLibraryItemChanged()
        {
            currentContext = HttpContext.Current;
        }

        /// <summary>
        /// An item is updating
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            try
            {
                // This should filter out ItemUpdating events that are not tied to check-ins. We want this code to fire as an item is checking in.
                // ItemCheckingIn does not allow modifying AfterProperties, which is why ItemUpdating is being used. This IF statement should prevent multiple ItemUpdating from running
                if (properties.AfterProperties["vti_sourcecontrolcheckedoutby"] == null && properties.BeforeProperties["vti_sourcecontrolcheckedoutby"] != null)
                {
                    SPListItem item = properties.ListItem;
                    // Store the HttpContext request execution path in the item's property bag. This seems to be the most reliable way to share a parameter between ItemUpdating and ItemCheckedIn
                    properties.AfterProperties["UploadExecutionPath"] = currentContext.Request.CurrentExecutionFilePath;
                    // Set the title to the document filename if it's missing. This shouldn't be the case anymore, since Title is a required field.
                    // I'm leaving this in here because MacroView unpredictably sets titles (or omits them) when uploading multiple documents at a time
                    if (item["Title"] == null)
                    {
                        properties.AfterProperties["Title"] = item.File.Name;
                    }

                    // See if there's a duplicate object in either SharePoint or Guidewire. Duplicates marked as final will throw an inner exception.
                    string existingDocId = null;
                    string existingDocIdUrl = null;
                    Util.GuidewireOperationType operation = Util.CheckDuplicate(item, out existingDocId, out existingDocIdUrl);
                    // If a duplicate is found, but it's not marked as final, we want to reuse that document's DocId, so it is set in AfterProperties
                    if (existingDocId != null)
                    {
                        properties.AfterProperties["_dlc_DocId"] = existingDocId;
                        properties.AfterProperties["_dlc_DocIdUrl"] = existingDocIdUrl;
                    }
                    // Call Guidewire, now that the correct operation type and document ID are known. An exception is thrown Guidewire reports 'false'
                    bool gwSuccess = Util.CallGuideWire(item, operation, existingDocId);
                    if (!gwSuccess)
                    {
                        throw new Exception(string.Format("Error in ItemCheckingIn: There was an error when submitting the document to Guidewire. Please have an administrator check SharePoint and Guidewire event logs for errors regarding a document with ID {0} and with a time of {1}", item["_dlc_DocId"].ToString(), DateTime.Now));
                    }
                }
            }
            // For exceptions, the inner exception message is logged, and then the SharePoint event is cancelled
            catch (Exception ex)
            {
                Util.LogError("ItemCheckingIn failed with exception: " + ex.Message);
                properties.ErrorMessage = ex.Message;
                properties.Status = SPEventReceiverStatus.CancelWithError;
                // The item is deleted, because we don't want people to have to navigate to the drop off library to finish the operation. They should just start over in MacroView
                properties.ListItem.Delete();
            }
        }

        /// <summary>
        /// An item was checked in
        /// </summary>
        public override void ItemCheckedIn(SPItemEventProperties properties)
        {
            try
            {
                // This custom ItemCheckedIn event should only happen when the update is not occurring through the SharePoint UI (i.e. MacroView)
                // In other cases, the content organizer routers should deal with the file
                // TODO: Refactor this for better DRY code
                SPListItem item = properties.ListItem;
                if (item.Properties.ContainsKey("UploadExecutionPath") && item.Properties["UploadExecutionPath"].ToString().Contains("_vti_bin"))
                {
                    // Split up the filename so that it can be reformatted into the proper naming convention
                    string fileExtension = "." + item[SPBuiltInFieldId.File_x0020_Type].ToString().ToLower();
                    string fileBaseName = item.File.Name.Substring(0, item.File.Name.ToLower().LastIndexOf(fileExtension));
                    string newFileName = fileBaseName;
                    string destinationListUrl = item.Web.Url + "/";

                    // Depending on the item's content type, it should go to it's respective document library
                    switch (item.ContentType.Name)
                    {
                        case "Guidewire Work Matter Document":
                            newFileName += " - " + item[Resource.FieldWorkMatter].ToString() + fileExtension;
                            destinationListUrl += item.Web.Lists[Resource.WorkMatterDocumentsLibrary].RootFolder.Url;
                            break;
                        case "Guidewire Vendor Document":
                            newFileName += " - " + item[Resource.FieldVendor].ToString() + fileExtension;
                            destinationListUrl += item.Web.Lists[Resource.VendorDocumentsLibrary].RootFolder.Url;
                            break;
                        default:
                            throw new SPException(string.Format("There was an error processing a file submitted to the drop off library. File: {0} Message: {1}", item.File.Url, "Content Type was not recognized."));
                    }

                    // Attempt to route the file to it's proper destination library
                    bool fileRouteSuccess = Util.RouteFile(item, destinationListUrl + "/" + newFileName);
                    // If there is an error moving the file, throw an exception
                    if (!fileRouteSuccess)
                    {
                        throw new SPException("There was an error routing the file {0}. The file is likely still in the Drop Off Library");
                    }
                }
            }
            // On exception, cancel the event and log the error. Unfortunately, the document will probably be trapped in the drop off library
            // Since the rules that are evaluated in ItemCheckedIn are also evaluated in ItemUpdating, exceptions should be caught early. Hopefully, exceptions in here are very rare.
            catch(Exception ex)
            {
                Util.LogError("ItemCheckedIn failed with exception: " + ex.Message);
                properties.ErrorMessage = ex.Message;
                properties.Status = SPEventReceiverStatus.CancelWithError;
            }
        }
    }
}