using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Configuration;
using System.Web.SessionState;

namespace DocumentService.Models
{
    public class OpenText : IDisposable
    {
        #region Private Fields

        private static readonly NameValueCollection config = WebConfigurationManager.AppSettings;

        #endregion Private Fields

        #region Public Methods

        void IDisposable.Dispose()
        {
        }

        public Session GetSession()
        {
            return new Session();
        }

        #endregion Public Methods

        #region Internal Methods

        internal DocumentResult AddDocument(Session session, DocumentMetadata documentMetadata, byte[] documentData)
        {
            var result = new DocumentResult { Success = false };
            try
            {
                // Determine where the document should be placed.

                //TODO - logic
            }
            catch (Exception ex)
            {
                Utilities.Util.LogError(ex.Message); //TODO - Replace with unified logging service.
                throw;
            }
            return result;
        }

        internal DocumentResult DeleteDocument(Session session, string documentId)
        {
            var result = new DocumentResult { Success = false };
            try
            {
                //TODO - logic
            }
            catch (Exception e)
            {
                //TODO - Log this.
                var log = e.Message;
            }
            return result;
        }

        internal GetDocumentContentResult GetDocumentContent(Session session, string documentId)
        {
            var result = new GetDocumentContentResult { Success = false };
            try
            {
                result.DocumentContent = GetNodeContentAsync(documentId).Result;
                result.Success = true;
            }
            catch (Exception ex)
            {
                Utilities.Util.LogError(ex.Message); //TODO - Replace with unified logging service.
                throw;
            }
            return result;
        }

        internal GetDocumentUrlResult GetDocumentUrl(Session session, string documentId)
        {
            var result = new GetDocumentUrlResult { Success = false };
            try
            {
                result.DocumentUrl = GetOpenLink(documentId);
                result.Success = true;
            }
            catch (Exception ex)
            {
                Utilities.Util.LogError(ex.Message); //TODO - Replace with unified logging service.
                throw;
            }
            return result;
        }

        internal DocumentResult UpdateDocumentMetadata(Session session, string documentId, DocumentMetadata documentMetadata)
        {
            var result = new DocumentResult { Success = false };
            try
            {
                //TODO - logic
            }
            catch (Exception ex)
            {
                Utilities.Util.LogError(ex.Message); //TODO - Replace with unified logging service.
                throw;
            }
            return result;
        }

        #endregion Internal Methods

        #region Private Methods

        private static void Authenticate(HttpSessionState currentSession)
        {
            try
            {
                var client = new RestClient(config["OpenTextApiUrl"]);
                var request = new RestRequest("/v1/Auth", Method.POST);
                request.RequestFormat = DataFormat.Json;
                request.AddParameter("username", config["ApiUser"]);
                request.AddParameter("password", config["ApiPw"]);
                var response = client.Execute<dynamic>(request);
                currentSession["Token"] = response.Data["ticket"];
            }
            catch
            {
                currentSession["Token"] = "Invalid"; // Mark bad attempt as invalid,
                                                     // so it will halt further authentication attempts
                                                     // in the same thread.
            }
        }

        private string GetFolderId([Optional]string rootNodeId, string folderName)
        {
            try
            {
                string rootNode = String.IsNullOrEmpty(rootNodeId) ? config["RootNodeId"] : rootNodeId;
                var client = new RestClient(config["OpenTextApiUrl"]);
                var request = new RestRequest("/v1/nodes/" + rootNode + "/" + "nodes", Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("OTCSTicket", new Session().Token);
                request.AddParameter("where_name", folderName);
                request.AddParameter("where_type", "0");

                string response = client.Execute(request).Content;
                JToken data = JObject.Parse(response)["data"];

                if (data.Count() > 1) throw new AmbiguousMatchException(
                    String.Format("Searching within node {0} for a folder named '{1}' returned more than one result.\n" +
                    "Matching node IDs were: {2} ", rootNode, folderName, string.Join(", ", data.Select(x => x["id"]))));
                if (data.Count() < 1) throw new KeyNotFoundException(String.Format("Searching within node {0} for a folder named '{1}' returned zero results.", rootNode, folderName));
                return data.Select(x => x["id"]).First().ToString();
            }
            catch (Exception ex)
            {
                Utilities.Util.LogError(ex.Message); //TODO - Replace with unified logging service.
                throw;
            }
        }

        private async System.Threading.Tasks.Task<byte[]> GetNodeContentAsync(string nodeId)
        {
            try
            {
                // this must be asynchronous since it's relying on a download to happen.

                var client = new RestClient(config["OpenTextApiUrl"]);
                var request = new RestRequest("/v1/nodes/" + nodeId + "/" + "content", Method.GET);
                request.AddHeader("OTCSTicket", new Session().Token);
                var response = await client.ExecuteTaskAsync(request);
                if (response.StatusCode != HttpStatusCode.OK)
                    throw new Exception(String.Format("Unable to get content for node id: {0}", nodeId));
                return response.RawBytes;
            }
            catch (Exception ex)
            {
                Utilities.Util.LogError(ex.Message); //TODO - Replace with unified logging service.
                throw;
            }
        }

        private string GetOpenLink(string nodeId)
        {
            try
            {
                var client = new RestClient(config["OpenTextApiUrl"]);
                var request = new RestRequest("/v1/nodes/" + nodeId + "/" + "actions", Method.GET);
                request.RequestFormat = DataFormat.Json;
                request.AddHeader("OTCSTicket", new Session().Token);
                request.AddParameter("fields", "actions");
                string response = client.Execute(request).Content;
                var openLink = JObject.Parse(response).SelectToken("$.actions[?(@.name == 'Open')]")["url"];

                if (openLink == null) throw new ArgumentException(
                     String.Format("Unable to get 'Open' action for node id: {0}", nodeId));

                return new Uri(config["OpenTextApiUrl"]).GetLeftPart(UriPartial.Authority) + openLink.ToString();
            }
            catch (Exception ex)
            {
                Utilities.Util.LogError(ex.Message); //TODO - Replace with unified logging service.
                throw;
            }
        }

        #endregion Private Methods

        #region Public Classes

        public class Session : IDisposable
        {
            #region Public Constructors

            public Session()
            {
                var currentSession = HttpContext.Current.Session;
                if (currentSession["Token"] == null) Authenticate(currentSession);
                Token = (string)currentSession["Token"];
            }

            #endregion Public Constructors

            #region Public Properties

            public string Token { get; set; }

            #endregion Public Properties

            #region Public Methods

            void IDisposable.Dispose()
            {
            }

            #endregion Public Methods
        }

        #endregion Public Classes
    }
}