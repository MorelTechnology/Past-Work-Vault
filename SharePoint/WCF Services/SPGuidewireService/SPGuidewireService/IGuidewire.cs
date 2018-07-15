using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SPGuidewireService
{
    [ServiceContract]
    public interface IGuidewire
    {
        [OperationContract]
        List<WorkMatter> GetAllWorkMatters(string ClaimNumber, string NameDenorm, string trg_WorkMatterDescription);
        [OperationContract]
        WorkMatter GetWorkMatterByID(string ClaimNumber);

        [OperationContract]
        List<Vendor> GetAllVendors(string ContactID, string ContactName);
        [OperationContract]
        Vendor GetVendorByID(string ContactID);

        [OperationContract]
        List<WorkMatterDocument> GetAllWorkMatterDocuments(string PublicID, string SPID, string Filename, string ClaimNumber, string WorkMatterDesc, string AccountName);
        [OperationContract]
        WorkMatterDocument GetWorkMatterDocumentBySPID(string SPID);
        [OperationContract]
        WorkMatterDocument GetWorkMatterDocumentByPublicID(string PublicID);

        [OperationContract]
        List<VendorDocument> GetAllVendorDocuments(string PublicID, string SPID, string Filename, string ContactID, string ContactName);
        [OperationContract]
        VendorDocument GetVendorDocumentBySPID(string SPID);
        [OperationContract]
        VendorDocument GetVendorDocumentByPublicID(string PublicID);
    }

    [DataContract]
    public class WorkMatter
    {
        [DataMember]
        public string ClaimNumber { get; set; }
        [DataMember]
        public string trg_WorkMatterDescription { get; set; }
        [DataMember]
        public string NameDenorm { get; set; }
    }

    [DataContract]
    public class Vendor
    {
        [DataMember]
        public string ContactID { get; set; }
        [DataMember]
        public string ContactName { get; set; }
    }

    [DataContract]
    public class WorkMatterDocument
    {
        [DataMember]
        public string PublicID { get; set; }
        [DataMember]
        public string SPID { get; set; }
        [DataMember]
        public string Filename { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Subcategory { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string ClaimNumber { get; set; }
        [DataMember]
        public string WorkMatterDesc { get; set; }
        [DataMember]
        public string AccountName { get; set; }
        [DataMember]
        public string Description { get; set; }
    }

    [DataContract]
    public class VendorDocument
    {
        [DataMember]
        public string PublicID { get; set; }
        [DataMember]
        public string SPID { get; set; }
        [DataMember]
        public string Filename { get; set; }
        [DataMember]
        public string Category { get; set; }
        [DataMember]
        public string Subcategory { get; set; }
        [DataMember]
        public string Status { get; set; }
        [DataMember]
        public string ContactID { get; set; }
        [DataMember]
        public string ContactName { get; set; }
        [DataMember]
        public string Description { get; set; }
    }
}
