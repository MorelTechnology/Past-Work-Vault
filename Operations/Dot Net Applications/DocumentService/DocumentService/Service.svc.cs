using DocumentService.Models;
using DocumentService.Utilities;
using System;
using System.IO;
using System.ServiceModel.Activation;

namespace DocumentService
{
    // The attribute AspNetCompabilityRequirements is used for specifying
    // an ASP.NET compatible environment for WCF service execution.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
    public class Service : IService
    {
        #region Public Methods

        /// <summary>
        /// Adds/Creates a document in Content Server
        /// </summary>
        /// <param name="documentMetadata">The metadata the document is to be created with</param>
        /// <param name="documentData">Byte array containing the document contents</param>
        /// <returns></returns>
        public DocumentResult CreateDocument(DocumentMetadata documentMetadata, byte[] documentData)
        {
            using (OpenText ot = new OpenText())
                return ot.AddDocument(ot.GetSession(), documentMetadata, documentData);
        }

        /// <summary>
        /// Deletes a document from Content Server
        /// </summary>
        /// <param name="documentId">The ID of the document to delete</param>
        /// <returns></returns>
        public DocumentResult DeleteDocument(string documentId)
        {
            using (OpenText ot = new OpenText())
                return ot.DeleteDocument(ot.GetSession(), documentId);
        }

        /// <summary>
        /// This will likely never be implemented as it was a specific requirement of the legacy system.
        /// </summary>
        /// <param name="documentId">The document ID to retrieve</param>
        /// <returns></returns>
        public OperationResult FlagDocumentForUpdate(string documentId)
        {
            //This will likely never be implemented as it was a specific requirement of the legacy system.
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the contents of a document given a specified document ID
        /// </summary>
        /// <param name="documentId">The document ID to retrieve</param>
        /// <returns></returns>
        public GetDocumentContentResult GetDocumentContent(string documentId)
        {
            using (OpenText ot = new OpenText())
                return ot.GetDocumentContent(ot.GetSession(), documentId);
        }

        /// <summary>
        /// Gets the URL to a document
        /// </summary>
        /// <param name="documentId">The document ID to get the URL for</param>
        /// <returns></returns>
        public GetDocumentUrlResult GetDocumentUrl(string documentId)
        {
            using (OpenText ot = new OpenText())
                return ot.GetDocumentUrl(ot.GetSession(), documentId);
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
        /// Reports back true if the method is called successfully (heartbeat test).
        /// </summary>
        /// <returns></returns>
        public OperationResult IsAlive()
        {
            return new OperationResult() { Success = true };
        }

        /// <summary>
        /// Sets the metadata on a given document
        /// </summary>
        /// <param name="documentId">The document ID to set metadata on</param>
        /// <param name="documentMetadata">A DocumentMetadata object to apply to the document</param>
        /// <returns></returns>
        public DocumentResult UpdateDocumentMetadata(string documentId, DocumentMetadata documentMetadata)
        {
            using (OpenText ot = new OpenText())
                return ot.UpdateDocumentMetadata(ot.GetSession(), documentId, documentMetadata);
        }

        #endregion Public Methods
    }
}