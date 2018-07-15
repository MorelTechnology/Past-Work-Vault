using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Guidewire_Integration
{
    [ServiceContract]
    public interface IGuidewire
    {
        [OperationContract]
        DocumentResult CreateDocument(DocumentMetadata documentMetadata, byte[] documentData);

        [OperationContract]
        DocumentResult DeleteDocument(string documentId);

        [OperationContract]
        DocumentResult UpdateDocumentMetadata(string documentId, DocumentMetadata documentMetadata);

        [OperationContract]
        GetDocumentUrlResult GetDocumentUrl(string documentId);

        [OperationContract(Name = "GetDocumentUrlWithMimeType")]
        GetDocumentUrlResult GetDocumentUrl(string documentId, string mimeType);

        [OperationContract]
        GetDocumentContentResult GetDocumentContent(string documentId);

        [OperationContract]
        OperationResult IsAlive();

        [OperationContract]
        OperationResult FlagDocumentForUpdate(string documentId);
    }

    [DataContract]
    public enum DocumentType
    {
        [EnumMember]
        WorkMatterDocument,
        [EnumMember]
        VendorDocument,
    }

    [DataContract]
    public class DocumentMetadata
    {
        [DataMember]
        public string DocumentID { get; set; }

        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public string ClaimNumber { get; set; }

        [DataMember]
        public string ContactID { get; set; }

        [DataMember]
        public DocumentType DocumentType { get; set; }

        [DataMember]
        public string Author { get; set; }
    }

    [DataContract]
    public class OperationResult
    {
        [DataMember]
        public bool Success { get; set; }

        [DataMember]
        public string ErrorMessage { get; set; }
    }

    [DataContract]
    public class GetDocumentUrlResult : OperationResult
    {
        [DataMember]
        public string DocumentUrl { get; set; }
    }

    [DataContract]
    public class GetDocumentContentResult : OperationResult
    {
        [DataMember]
        public byte[] DocumentContent { get; set; }
    }

    [DataContract]
    public class DocumentResult : OperationResult
    {
        [DataMember]
        public string DocumentId { get; set; }
    }
}
