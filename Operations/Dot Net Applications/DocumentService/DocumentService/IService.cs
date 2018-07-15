using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DocumentService
{
    [ServiceContract]
    public interface IService
    {
        #region Method: CreateDocument

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped,
                    Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "/Document/Create")]
        DocumentResult CreateDocument(DocumentMetadata documentMetadata, byte[] documentData);

        #endregion Method: CreateDocument
        #region Method: DeleteDocument

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, 
                 ResponseFormat = WebMessageFormat.Json, 
                 UriTemplate = "/Document/Delete/{documentId}")]
        DocumentResult DeleteDocument(string documentId);

        #endregion Method: DeleteDocument
        #region Method: UpdateDocumentMetadata

        [OperationContract]
        [WebInvoke(BodyStyle = WebMessageBodyStyle.Wrapped,
                    Method = "POST",
                    RequestFormat = WebMessageFormat.Json,
                    ResponseFormat = WebMessageFormat.Json,
                    UriTemplate = "/Document/UpdateMetadata/{documentId}")]
        DocumentResult UpdateDocumentMetadata(string documentId, DocumentMetadata documentMetadata);

        #endregion Method: UpdateDocumentMetadata
        #region Method: GetDocumentUrl

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, 
                 ResponseFormat = WebMessageFormat.Json, 
                 UriTemplate = "/Document/GetDocumentUrl/{documentId}")]
        GetDocumentUrlResult GetDocumentUrl(string documentId);

        #endregion Method: GetDocumentUrl
        #region Method: GetDocumentUrlWithMimeType

        [OperationContract(Name = "GetDocumentUrlWithMimeType")]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, 
                 ResponseFormat = WebMessageFormat.Json, 
                 UriTemplate = "/Document/GetDocumentUrlWithMimeType/{documentId}/{mimeType}")]
        GetDocumentUrlResult GetDocumentUrl(string documentId, string mimeType);

        #endregion Method: GetDocumentUrlWithMimeType
        #region Method: GetDocumentContent

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, 
                 ResponseFormat = WebMessageFormat.Json, 
                 UriTemplate = "/Document/GetDocumentContent/{documentId}")]
        GetDocumentContentResult GetDocumentContent(string documentId);

        #endregion Method: GetDocumentContent
        #region Method: IsAlive

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, 
                 ResponseFormat = WebMessageFormat.Json, 
                 UriTemplate = "/IsAlive")]
        OperationResult IsAlive();

        #endregion Method: IsAlive
        #region Method: FlagDocumentForUpdate

        [OperationContract]
        [WebGet(BodyStyle = WebMessageBodyStyle.Wrapped, 
                 ResponseFormat = WebMessageFormat.Json, 
                 UriTemplate = "/Document/FlagDocumentForUpdate/{documentId}")]
        OperationResult FlagDocumentForUpdate(string documentId);

        #endregion Method: FlagDocumentForUpdate
    }
    [DataContract]
    public enum DocumentType
    {
        [EnumMember]
        WorkMatterDocument,

        [EnumMember]
        VendorDocument,

        [EnumMember]
        PortalSubmission
        
    }

    [DataContract]
    public class DocumentMetadata
    {
        #region Public Properties

        [DataMember]
        public string Author { get; set; }

        [DataMember]
        public string ClaimNumber { get; set; }

        [DataMember]
        public string ContactID { get; set; }

        [DataMember]
        public string DocumentID { get; set; }

        [DataMember]
        public DocumentType DocumentType { get; set; }

        [DataMember]
        public string FileName { get; set; }

        #endregion Public Properties
    }

    [DataContract]
    public class DocumentResult : OperationResult
    {
        #region Public Properties

        [DataMember]
        public string DocumentId { get; set; }

        #endregion Public Properties
    }

    [DataContract]
    public class GetDocumentContentResult : OperationResult
    {
        #region Public Properties

        [DataMember]
        public byte[] DocumentContent { get; set; }

        #endregion Public Properties
    }

    [DataContract]
    public class GetDocumentUrlResult : OperationResult
    {
        #region Public Properties

        [DataMember]
        public string DocumentUrl { get; set; }

        #endregion Public Properties
    }

    [DataContract]
    public class OperationResult
    {
        #region Public Properties

        [DataMember]
        public string ErrorMessage { get; set; }

        [DataMember]
        public bool Success { get; set; }

        #endregion Public Properties
    }
}