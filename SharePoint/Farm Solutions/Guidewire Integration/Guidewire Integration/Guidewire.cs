using System;
using System.ServiceModel.Activation;
using Microsoft.SharePoint.Client.Services;
using Microsoft.SharePoint;
using Microsoft.Office.DocumentManagement;
using System.IO;
using System.ServiceModel;

namespace Guidewire_Integration
{
    [BasicHttpBindingServiceMetadataExchangeEndpoint]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Guidewire : IGuidewire
    {
        #region Public Methods
        /// <summary>
        /// Creates a document in SharePoint
        /// </summary>
        /// <param name="documentMetadata">The metadata the document is to be created with</param>
        /// <param name="documentData">Byte array containing the document contents</param>
        /// <returns></returns>
        public DocumentResult CreateDocument(DocumentMetadata documentMetadata, byte[] documentData)
        {
            SPFile file = null;
            SPListItem item = null;
            DocumentResult result = null;
            Guid siteId = SPContext.Current.Site.ID;
            Guid webId = SPContext.Current.Web.ID;

            // To have access to taxonomy and DocumentManagement namespaces, the site needs to be accessed with elevated privileges
            SPSecurity.RunWithElevatedPrivileges(delegate ()
            {
                result = new DocumentResult { Success = false };
                using (SPSite site = new SPSite(siteId))
                {
                    using (SPWeb web = site.OpenWeb(webId))
                    {
                        web.AllowUnsafeUpdates = true;

                        /* Use the appropriate document library based on the type of the document */
                        SPList documentList = null;
                        if (documentMetadata.DocumentType == DocumentType.VendorDocument)
                        {
                            documentList = web.Lists[Resource.VendorDocumentsLibrary];
                        }
                        if (documentMetadata.DocumentType == DocumentType.WorkMatterDocument)
                        {
                            documentList = web.Lists[Resource.WorkMatterDocumentsLibrary];
                        }

                        if (documentList != null)
                        {
                            string tempId = Guid.NewGuid().ToString();
                            try
                            {
                                // If there's a document ID then the user is doing an update.
                                // When the user changes the document name in ClaimCenter, SharePoint sees it as a new document
                                if (documentMetadata.DocumentID != null)
                                {
                                    string documentUrl = GetDocumentUrlFromId(site, documentMetadata.DocumentID);
                                    file = web.GetFile(documentUrl);
                                    file.SaveBinary(documentData, false);
                                }
                                else
                                {
                                    // To allow for similarly named files in different work matters, we can append the Claim Number or Contact ID
                                    string fileName = "";
                                    if (documentMetadata.DocumentType == DocumentType.VendorDocument)
                                    {
                                        string contactID = Util.RemoveMetaCharacters(documentMetadata.ContactID);
                                        fileName = documentMetadata.FileName.Substring(0, documentMetadata.FileName.LastIndexOf(".")) + " - " + contactID + documentMetadata.FileName.Substring(documentMetadata.FileName.LastIndexOf("."));
                                    }
                                    else if (documentMetadata.DocumentType == DocumentType.WorkMatterDocument)
                                    {
                                        fileName = documentMetadata.FileName.Substring(0, documentMetadata.FileName.LastIndexOf(".")) + " - " + documentMetadata.ClaimNumber + documentMetadata.FileName.Substring(documentMetadata.FileName.LastIndexOf("."));
                                    }

                                    // This hashtable isn't really used, it's just a required parameter for the Files.Add overload being used
                                    System.Collections.Hashtable additionalProperties = new System.Collections.Hashtable();

                                    // Try to get the author, but fall back to the service account if necessary
                                    SPUser author = Util.GetUserFromLogin(documentMetadata.Author);
                                    if (author == null)
                                    {
                                        author = SPContext.Current.Web.CurrentUser;
                                    }
                                    file = documentList.RootFolder.Files.Add(fileName, documentData, additionalProperties, author, author, DateTime.Now, DateTime.Now.ToUniversalTime(), true);
                                }
                                // Check in the file if it is checked out
                                if (file.CheckOutType != SPFile.SPCheckOutType.None)
                                {
                                    if (documentList.EnableMinorVersions)
                                        file.CheckIn(string.Empty, SPCheckinType.MinorCheckIn);
                                    else
                                        file.CheckIn(string.Empty, SPCheckinType.MajorCheckIn);
                                }
                                item = file.Item;

                                result.Success = true;
                                item.SystemUpdate(false);
                                result.DocumentId = Util.GetDocumentId(item);
                            }
                            catch (Exception e)
                            {
                                Util.LogError("CreateDocument failed with exception: " + e.Message, Util.ErrorLevel.Error);
                                result.Success = false;
                                result.ErrorMessage = e.Message;
                            }
                            finally
                            {
                                web.AllowUnsafeUpdates = false;
                            }
                        }
                    }
                }
            });
            return result;
        }

        /// <summary>
        /// Gets the contents of a document given a specified document ID
        /// </summary>
        /// <param name="documentId">The document ID to retrieve</param>
        /// <returns></returns>
        public GetDocumentContentResult GetDocumentContent(string documentId)
        {
            GetDocumentContentResult result = new GetDocumentContentResult { Success = false };
            // Retrieve the document using the document ID, then populate result properties
            SPListItem documentItem = GetDocumentItemFromId(documentId);
            if (documentItem != null && documentItem.File != null)
            {
                result.DocumentContent = documentItem.File.OpenBinary();
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = string.Format("The document ID service was unable to find a document for the ID {0}", documentId == null ? "null" : documentId);
                Util.LogError(result.ErrorMessage, Util.ErrorLevel.Warning);
            }

            return result;
        }

        /// <summary>
        /// Gets the URL to a document
        /// </summary>
        /// <param name="documentId">The document ID to get the URL for</param>
        /// <returns></returns>
        public GetDocumentUrlResult GetDocumentUrl(string documentId)
        {
            GetDocumentUrlResult result = new GetDocumentUrlResult();
            result.Success = false;
            try
            {
                using (SPSite site = SPContext.Current.Site)
                {
                    result.DocumentUrl = GetDocumentUrlFromId(site, documentId);
                    if (!string.IsNullOrEmpty(result.DocumentUrl))
                        result.Success = true;
                    else
                    {
                        result.ErrorMessage = string.Format("The document id service was unable to find a document for the id {0}", documentId == null ? "null" : documentId);
                        Util.LogError(result.ErrorMessage, Util.ErrorLevel.Warning);
                    }
                }
            }
            catch (Exception e)
            {
                Util.LogError("GetDocumentUrl errored with exception: " + e.Message);
                result.ErrorMessage = e.Message;
            }
            return result;
        }

        /// <summary>
        /// Gets the URL to a document. MimeType is used to assemble a URL that is compatible with Guidewire
        /// </summary>
        /// <param name="documentId">The document ID to get the URL for</param>
        /// <param name="mimeType">The MimeType as it is represented in ClaimCenter</param>
        /// <returns></returns>
        public GetDocumentUrlResult GetDocumentUrl(string documentId, string mimeType)
        {
            GetDocumentUrlResult result = GetDocumentUrl(documentId);
            if (result.Success)
            {
                // Check the extension of the document, and do additional processing if the extension is missing, contains a space or doesn't match a known extension from ClaimCenter
                string currentExtension = Path.GetExtension(result.DocumentUrl);
                using (MIMETypes mimeTypes = new MIMETypes())
                {
                    if (string.IsNullOrEmpty(currentExtension) || currentExtension.Contains(" "))
                    {
                        //Document doesn't have an extension.  Resolve this before attempting to access it.
                        result.DocumentUrl = Util.ResolveMissingExtension(result.DocumentUrl, mimeType);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Sets the metadata on a given document
        /// </summary>
        /// <param name="documentId">The document ID to set metadata on</param>
        /// <param name="documentMetadata">A DocumentMetadata object to apply to the document</param>
        /// <returns></returns>
        public DocumentResult UpdateDocumentMetadata(string documentId, DocumentMetadata documentMetadata)
        {
            DocumentResult result = new DocumentResult { Success = false, DocumentId = documentId };

            // Get the item by using its ID
            SPListItem item = GetDocumentItemFromId(documentId);
            if (item != null)
            {
                // Get the author from the metadata object, update the author and modified date on the SharePoint side
                SPUser author = Util.GetUserFromLogin(documentMetadata.Author);
                if (author != null)
                {
                    DateTime currentTime = DateTime.Now.ToLocalTime();
                    item[Resource.FieldModifiedBy] = author;
                    item[Resource.FieldModifiedDate] = currentTime;
                }
                // Update the item
                item.UpdateOverwriteVersion();
                result.Success = true;
            }
            else
            {
                result.ErrorMessage = string.Format("The document with id {0} could not be located for update.", documentId);
                Util.LogError(result.ErrorMessage, Util.ErrorLevel.Warning);
            }

            return result;
        }

        /// <summary>
        /// Deletes a document from SharePoint
        /// </summary>
        /// <param name="documentId">The ID of the document to delete</param>
        /// <returns></returns>
        public DocumentResult DeleteDocument(string documentId)
        {
            DocumentResult result = new DocumentResult { Success = false, DocumentId = documentId };

            // Get the item by using its ID
            SPListItem documentItem = GetDocumentItemFromId(documentId);
            if (documentItem != null)
            {
                string docGuid = documentItem.UniqueId.ToString();
                using (DisabledEventsScope scope = new DisabledEventsScope())
                {
                    try
                    {
                        documentItem.Recycle();
                        result.Success = true;
                    }
                    catch (Exception e)
                    {
                        Util.LogError("DeleteDocument failed with exception:  " + e.Message);
                        result.Success = false;
                    }
                }
            }
            else
            {
                result.ErrorMessage = string.Format("The document with id {0} could not be located for delete.", documentId);
                Util.LogError(result.ErrorMessage, Util.ErrorLevel.Warning);
            }

            return result;
        }

        /// <summary>
        /// Reports back true if the method is called successfully (i.e. SharePoint is up)
        /// </summary>
        /// <returns></returns>
        public OperationResult IsAlive()
        {
            OperationResult result = new OperationResult { Success = true };
            return result;
        }

        /// <summary>
        /// Flags a document for metadata update for the document update service
        /// </summary>
        /// <param name="documentId">The ID of the document to set the flag on</param>
        /// <returns></returns>
        public OperationResult FlagDocumentForUpdate(string documentId)
        {
            OperationResult result = new OperationResult { Success = false };
            SPListItem item = GetDocumentItemFromId(documentId);
            if (item != null)
            {
                try
                {
                    // Set the flag to 'Yes' and set the modified date to the current time
                    item[Resource.FieldUpdateRequired] = "Yes";
                    item["Modified"] = DateTime.Now;
                    item.UpdateOverwriteVersion();
                    result.Success = true;
                }
                catch (Exception ex)
                {
                    result.ErrorMessage = string.Format("The document with id {0} could not be updated. Exception message: {1}", documentId, ex.Message);
                }
            }
            else
            {
                result.ErrorMessage = string.Format("The document with id {0} could not be located for update.", documentId);
                Util.LogError(result.ErrorMessage, Util.ErrorLevel.Warning);
            }
            return result;
        }
        #endregion Public Methods

        #region Private Methods
        /// <summary>
        /// Gets the SPListItem associated to a document
        /// </summary>
        /// <param name="documentId">The ID of the document to get the SPListItem for</param>
        /// <returns></returns>
        private SPListItem GetDocumentItemFromId(string documentId)
        {
            SPListItem documentItem = null;
            using (SPSite site = SPContext.Current.Site)
            {
                string documentUrl = GetDocumentUrlFromId(site, documentId);
                if (!string.IsNullOrEmpty(documentUrl))
                {
                    using (SPWeb web = SPContext.Current.Web)
                    {
                        try
                        {
                            documentItem = web.GetListItem(documentUrl);
                        }
                        catch (Exception e)
                        {
                            Util.LogError("GetDocumentItemFromId errored with exception: " + e.Message);
                        }
                    }
                }
                else
                {
                    Util.LogError(String.Format("GetDocumentItemFromID could not locate the document - {0}", documentId), Util.ErrorLevel.Warning);
                }
            }
            return documentItem;
        }

        /// <summary>
        /// Gets the URL of a document from its ID
        /// </summary>
        /// <param name="site">The SPSite that the document is part of</param>
        /// <param name="documentId">The ID of the document to retrieve</param>
        /// <returns></returns>
        private string GetDocumentUrlFromId(SPSite site, string documentId)
        {
            string documentUrl = string.Empty;
            try
            {
                // Use the DocumentIdProvider of the site to retrieve the document URL
                DocumentIdProvider provider = DocumentId.GetProvider(site);
                string[] match = provider.GetDocumentUrlsById(site, documentId);
                if (match.Length > 0)
                    documentUrl = match[0];
            }
            catch (Exception e)
            {
                Util.LogError(String.Format("The document id service was unable to find a document for the id {0}", documentId));
            }
            return documentUrl;
        }
        #endregion Private Methods
    }
}
