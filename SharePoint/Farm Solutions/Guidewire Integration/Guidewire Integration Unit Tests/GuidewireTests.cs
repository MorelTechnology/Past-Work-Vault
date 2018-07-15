using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Xml.Linq;

namespace Guidewire_Integration.Tests
{
    [TestClass()]

    public class GuidewireTests
    {
        public static SPSite site;
        public static SPWeb web;
        public Guidewire guidewire = new Guidewire();

        // Test Values
        public const string SHAREPOINT_DOCUMENT_ID_NUMBER = "GW-998c6e33-67a2-4004-aaa5-eec3f4aa3e84";
        public const string SHAREPOINT_CONTEXT_UNDER_TEST = "http://recordsqa/sites/Guidewire";

        [ClassInitialize]
        public static void ClassInitialize(TestContext tc)
        {
            // Any site or list creation for all tests in the class would go here
        }

        [TestInitialize]
        public void TestInitialize()
        {
            /** 
             * The following directives establish a SharePoint context which would not otherwise exist in a unit test.
             * Microsoft's Unit tests do not provide a facility to consume a SharePoint context.  The recommended approach 
             * is to use ASP.NET simulators to create mocks of context under test.  (Ain't nobody got time for that.)
             * Instead, here's my hack, which allows tests and benchmarking using a true model. 
             **/
            // Create forced SP Context
            site = new SPSite(SHAREPOINT_CONTEXT_UNDER_TEST);
            web = site.RootWeb;
            HttpRequest request = new HttpRequest("", web.Url, "");
            request.Browser = new HttpBrowserCapabilities();
            HttpContext.Current = new HttpContext(request, new HttpResponse(new StringWriter()));
            HttpContext.Current.Items["HttpHandlerSPWeb"] = web;
        }
        [TestCleanup]
        public void TestCleanup()
        {
            // Destroy Forced SP Context
            site.Dispose();
            site = null;
        }

        [TestMethod()]
        public void GetDocumentUrlTest()
        {
            GetDocumentUrlResult myResult = new GetDocumentUrlResult();
            myResult = guidewire.GetDocumentUrl(SHAREPOINT_DOCUMENT_ID_NUMBER);
            Assert.IsTrue(myResult.Success, myResult.ErrorMessage);
            Console.Write("URL: " + myResult.DocumentUrl);
        }

        [TestMethod()]
        public void SQLTest()
        {
            //POC testing for data mock due to problems with WCF method.

            // Configuration
            string connectionString = @"Server=MANSQLQA;Initial Catalog=ClaimCenter;uid=rivernet;pwd=rivernet;";
            string sqlQuery = "SELECT MimeType FROM cc_document WHERE DocUID = @DocumentID";
            string documentId = SHAREPOINT_DOCUMENT_ID_NUMBER;

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                SqlParameter param = new SqlParameter();
                param.ParameterName = "@DocumentID";
                param.Value = documentId;
                db.Open();
                using (SqlCommand query = new SqlCommand(sqlQuery, db))
                {
                    query.Parameters.Add(param);
                    using (SqlDataReader reader = query.ExecuteReader())
                    {
                        if (reader.FieldCount > 1)
                        {
                            throw new Exception("Query which should have given single value instead returned " + reader.FieldCount + ".");
                        }
                        else
                        {
                            string finalvalue = string.Empty;
                            while (reader.Read())
                            {
                                //loop just in case of multiple values, but this should have been tossed by exception earlier
                                //just in case, merge any extra data to a single string.
                                finalvalue = finalvalue + reader.GetString(0);
                            }
                            Console.Write("Query Result: " + finalvalue);
                        }
                    }
                }
            }
        }

    }
}
