using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace SPGuidewireService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class GuidewireService : IGuidewire
    {
        public List<WorkMatter> ExecuteWorkMatterFind(string queryText)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                List<WorkMatter> workMatters = new List<WorkMatter>();

                foreach (DataRow row in dt.Rows)
                {
                    WorkMatter matter = new WorkMatter()
                    {
                        ClaimNumber = row["ClaimNumber"].ToString(),
                        NameDenorm = row["NameDenorm"].ToString(),
                        trg_WorkMatterDescription = row["trg_WorkMatterDescription"].ToString()
                    };

                    workMatters.Add(matter);
                }

                return workMatters;
            }
        }

        public List<WorkMatter> GetAllWorkMatters(string ClaimNumber, string NameDenorm, string trg_WorkMatterDescription)
        {
            string queryText = ConfigurationManager.AppSettings["BaseWorkMatterSelectStatement"];
            queryText += " WHERE (1=1)";
            if (!string.IsNullOrEmpty(ClaimNumber))
            {
                queryText += " AND (C1.ClaimNumber LIKE '%" + ClaimNumber + "%')";
            }
            if (!string.IsNullOrEmpty(NameDenorm))
            {
                queryText += " AND (C2.NameDenorm LIKE '%" + NameDenorm + "%')";
            }
            if (!string.IsNullOrEmpty(trg_WorkMatterDescription))
            {
                queryText += " AND (C1.trg_WorkMatterDescription LIKE '%" + trg_WorkMatterDescription + "%')";
            }

            List<WorkMatter> workMatters = ExecuteWorkMatterFind(queryText);

            return workMatters;
        }

        public WorkMatter GetWorkMatterByID(string ClaimNumber)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            string queryText = ConfigurationManager.AppSettings["BaseSelectStatement"];
            queryText += " WHERE (1=1) AND (C1.ClaimNumber = '" + ClaimNumber + "')";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                WorkMatter matchingClaim = new WorkMatter();
                if (dt.Rows.Count > 0)
                {
                    matchingClaim.ClaimNumber = dt.Rows[0]["ClaimNumber"].ToString();
                    matchingClaim.trg_WorkMatterDescription = dt.Rows[0]["trg_WorkMatterDescription"].ToString();
                    matchingClaim.NameDenorm = dt.Rows[0]["NameDenorm"].ToString();

                    return matchingClaim;
                }
                return null;
            }
        }

        public List<Vendor> ExecuteVendorFind(string queryText)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                List<Vendor> vendors = new List<Vendor>();

                foreach (DataRow row in dt.Rows)
                {
                    Vendor vendor = new Vendor()
                    {
                        ContactID = row["ContactID"].ToString(),
                        ContactName = row["ContactName"].ToString()
                    };

                    vendors.Add(vendor);
                }

                return vendors;
            }
        }

        public List<Vendor> GetAllVendors(string ContactID, string ContactName)
        {
            string queryText = ConfigurationManager.AppSettings["BaseVendorSelectStatement"];
            queryText += " WHERE (1=1)";
            if (!string.IsNullOrEmpty(ContactID))
            {
                queryText += " AND (Contact.AddressBookUID LIKE '%" + ContactID + "%')";
            }
            if (!string.IsNullOrEmpty(ContactName))
            {
                queryText += " AND ((Contact.NameDenorm like '%" + ContactName + "%') or (Contact.FirstNameDenorm + ' ' + Contact.LastNameDenorm like '%" + ContactName + "%'))";
            }

            List<Vendor> vendors = ExecuteVendorFind(queryText);

            return vendors;
        }

        public Vendor GetVendorByID(string ContactID)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            string queryText = ConfigurationManager.AppSettings["BaseVendorSelectStatement"];
            queryText += " WHERE (1=1) AND (Contact.AddressBookUID = '" + ContactID + "')";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                Vendor matchingVendor = new Vendor();
                if (dt.Rows.Count > 0)
                {
                    matchingVendor.ContactID = dt.Rows[0]["ContactID"].ToString();
                    matchingVendor.ContactName = dt.Rows[0]["ContactName"].ToString();

                    return matchingVendor;
                }
                return null;
            }
        }

        public List<WorkMatterDocument> ExecuteWorkMatterDocumentFind(string queryText)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                List<WorkMatterDocument> workMatterDocuments = new List<WorkMatterDocument>();

                foreach (DataRow row in dt.Rows)
                {
                    WorkMatterDocument document = new WorkMatterDocument()
                    {
                        AccountName = row["AccountName"].ToString(),
                        Category = row["Category"].ToString(),
                        Filename = row["Filename"].ToString(),
                        PublicID = row["PublicID"].ToString(),
                        SPID = row["SPID"].ToString(),
                        Status = row["Status"].ToString(),
                        Subcategory = row["Subcategory"].ToString(),
                        WorkMatterDesc = row["WorkMatterDesc"].ToString(),
                        ClaimNumber = row["ClaimNumber"].ToString(),
                        Description = row["Description"].ToString(),
                    };

                    workMatterDocuments.Add(document);
                }

                return workMatterDocuments;
            }
        }

        public List<WorkMatterDocument> GetAllWorkMatterDocuments(string PublicID, string SPID, string Filename, string ClaimNumber, string WorkMatterDesc, string AccountName)
        {
            string queryText = ConfigurationManager.AppSettings["BaseWorkMatterDocumentSelectStatement"];
            if (!string.IsNullOrEmpty(PublicID))
            {
                queryText += " AND (Doc.PublicID LIKE '%" + PublicID + "%')";
            }
            if (!string.IsNullOrEmpty(SPID))
            {
                queryText += " AND (Doc.DocUID LIKE '%" + SPID + "%')";
            }
            if (!string.IsNullOrEmpty(Filename))
            {
                queryText += " AND (Doc.NameDenorm LIKE '%" + Filename + "%')";
            }
            if (!string.IsNullOrEmpty(ClaimNumber))
            {
                queryText += " AND (Claim.ClaimNumber LIKE '%" + ClaimNumber + "%')";
            }
            if (!string.IsNullOrEmpty(WorkMatterDesc))
            {
                queryText += " AND (Claim.trg_WorkMatterDescription LIKE '%" + WorkMatterDesc + "%')";
            }
            if (!string.IsNullOrEmpty(AccountName))
            {
                queryText += " AND (Contact.NameDenorm LIKE '%" + AccountName + "%')";
            }

            List<WorkMatterDocument> workMatterDocuments = ExecuteWorkMatterDocumentFind(queryText);

            return workMatterDocuments;
        }

        public WorkMatterDocument GetWorkMatterDocumentBySPID(string SPID)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            string queryText = ConfigurationManager.AppSettings["BaseWorkMatterDocumentSelectStatement"];
            queryText += " AND (Doc.DocUID = '" + SPID + "')";
            queryText += " ORDER BY Doc.PublicID desc";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                WorkMatterDocument matchingDocument = new WorkMatterDocument();
                if (dt.Rows.Count > 0)
                {
                    matchingDocument.AccountName = dt.Rows[0]["AccountName"].ToString();
                    matchingDocument.Category = dt.Rows[0]["Category"].ToString();
                    matchingDocument.ClaimNumber = dt.Rows[0]["ClaimNumber"].ToString();
                    matchingDocument.Filename = dt.Rows[0]["Filename"].ToString();
                    matchingDocument.PublicID = dt.Rows[0]["PublicID"].ToString();
                    matchingDocument.SPID = dt.Rows[0]["SPID"].ToString();
                    matchingDocument.Status = dt.Rows[0]["Status"].ToString();
                    matchingDocument.Subcategory = dt.Rows[0]["Subcategory"].ToString();
                    matchingDocument.WorkMatterDesc = dt.Rows[0]["WorkMatterDesc"].ToString();
                    matchingDocument.Description = dt.Rows[0]["Description"].ToString();

                    return matchingDocument;
                }
                return null;
            }
        }

        public WorkMatterDocument GetWorkMatterDocumentByPublicID(string PublicID)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            string queryText = ConfigurationManager.AppSettings["BaseWorkMatterDocumentSelectStatement"];
            queryText += " AND (Doc.PublicID = '" + PublicID + "')";
            queryText += " ORDER BY Doc.PublicID desc";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                WorkMatterDocument matchingDocument = new WorkMatterDocument();
                if (dt.Rows.Count > 0)
                {
                    matchingDocument.AccountName = dt.Rows[0]["AccountName"].ToString();
                    matchingDocument.Category = dt.Rows[0]["Category"].ToString();
                    matchingDocument.ClaimNumber = dt.Rows[0]["ClaimNumber"].ToString();
                    matchingDocument.Filename = dt.Rows[0]["Filename"].ToString();
                    matchingDocument.PublicID = dt.Rows[0]["PublicID"].ToString();
                    matchingDocument.SPID = dt.Rows[0]["SPID"].ToString();
                    matchingDocument.Status = dt.Rows[0]["Status"].ToString();
                    matchingDocument.Subcategory = dt.Rows[0]["Subcategory"].ToString();
                    matchingDocument.WorkMatterDesc = dt.Rows[0]["WorkMatterDesc"].ToString();
                    matchingDocument.Description = dt.Rows[0]["Description"].ToString();

                    return matchingDocument;
                }
                return null;
            }
        }

        public List<VendorDocument> ExecuteVendorDocumentFind(string queryText)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                List<VendorDocument> vendorDocuments = new List<VendorDocument>();

                foreach (DataRow row in dt.Rows)
                {
                    VendorDocument document = new VendorDocument()
                    {
                        ContactID = row["ContactID"].ToString(),
                        ContactName = row["ContactName"].ToString(),
                        Category = row["Category"].ToString(),
                        Filename = row["Filename"].ToString(),
                        PublicID = row["PublicID"].ToString(),
                        SPID = row["SPID"].ToString(),
                        Status = row["Status"].ToString(),
                        Subcategory = row["Subcategory"].ToString(),
                        Description = row["Description"].ToString(),
                    };

                    vendorDocuments.Add(document);
                }

                return vendorDocuments;
            }
        }

        public List<VendorDocument> GetAllVendorDocuments(string PublicID, string SPID, string Filename, string ContactID, string ContactName)
        {
            string queryText = ConfigurationManager.AppSettings["BaseVendorDocumentSelectStatement"];
            if (!string.IsNullOrEmpty(PublicID))
            {
                queryText += " AND (Doc.PublicID LIKE '%" + PublicID + "%')";
            }
            if (!string.IsNullOrEmpty(SPID))
            {
                queryText += " AND (Doc.DocUID LIKE '%" + SPID + "%')";
            }
            if (!string.IsNullOrEmpty(Filename))
            {
                queryText += " AND (Doc.NameDenorm LIKE '%" + Filename + "%')";
            }
            if (!string.IsNullOrEmpty(ContactID))
            {
                queryText += " AND (Contact.AddressBookUID LIKE '%" + ContactID + "%')";
            }
            if (!string.IsNullOrEmpty(ContactName))
            {
                queryText += " AND (Contact.NameDenorm LIKE '%" + ContactName + "%' OR (Contact.FirstNameDenorm + ' ' + Contact.LastNameDenorm LIKE '%" + ContactName + "%'))";
            }

            List<VendorDocument> vendorDocuments = ExecuteVendorDocumentFind(queryText);

            return vendorDocuments;
        }

        public VendorDocument GetVendorDocumentBySPID(string SPID)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            string queryText = ConfigurationManager.AppSettings["BaseVendorDocumentSelectStatement"];
            queryText += " AND (Doc.DocUID = '" + SPID + "')";
            queryText += " ORDER BY Doc.PublicID desc";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                VendorDocument matchingDocument = new VendorDocument();
                if (dt.Rows.Count > 0)
                {
                    matchingDocument.ContactID = dt.Rows[0]["ContactID"].ToString();
                    matchingDocument.ContactName = dt.Rows[0]["ContactName"].ToString();
                    matchingDocument.Category = dt.Rows[0]["Category"].ToString();
                    matchingDocument.Filename = dt.Rows[0]["Filename"].ToString();
                    matchingDocument.PublicID = dt.Rows[0]["PublicID"].ToString();
                    matchingDocument.SPID = dt.Rows[0]["SPID"].ToString();
                    matchingDocument.Status = dt.Rows[0]["Status"].ToString();
                    matchingDocument.Subcategory = dt.Rows[0]["Subcategory"].ToString();
                    matchingDocument.Description = dt.Rows[0]["Description"].ToString();

                    return matchingDocument;
                }
                return null;
            }
        }

        public VendorDocument GetVendorDocumentByPublicID(string PublicID)
        {
            string connectionString = ConfigurationManager.AppSettings["SQLConnectionString"];
            string queryText = ConfigurationManager.AppSettings["BaseVendorDocumentSelectStatement"];
            queryText += " AND (Doc.PublicID = '" + PublicID + "')";
            queryText += " ORDER BY Doc.PublicID desc";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(queryText, conn);
                conn.Open();

                SqlDataAdapter da = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                da.Dispose();

                VendorDocument matchingDocument = new VendorDocument();
                if (dt.Rows.Count > 0)
                {
                    matchingDocument.ContactID = dt.Rows[0]["ContactID"].ToString();
                    matchingDocument.ContactName = dt.Rows[0]["ContactName"].ToString();
                    matchingDocument.Category = dt.Rows[0]["Category"].ToString();
                    matchingDocument.Filename = dt.Rows[0]["Filename"].ToString();
                    matchingDocument.PublicID = dt.Rows[0]["PublicID"].ToString();
                    matchingDocument.SPID = dt.Rows[0]["SPID"].ToString();
                    matchingDocument.Status = dt.Rows[0]["Status"].ToString();
                    matchingDocument.Subcategory = dt.Rows[0]["Subcategory"].ToString();
                    matchingDocument.Description = dt.Rows[0]["Description"].ToString();

                    return matchingDocument;
                }
                return null;
            }
        }
    }
}
