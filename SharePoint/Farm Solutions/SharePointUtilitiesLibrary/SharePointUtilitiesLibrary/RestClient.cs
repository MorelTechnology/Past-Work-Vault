using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Text;
using static AppsToOranges.Utility;

namespace AppsToOranges.SharePointUtilities
{
    public class RestClient
    {
        static EventLogger log = new EventLogger("Apps To Oranges SharePoint Utilities", "Application");
        public enum AuthType { Basic, NTLM, Integrated };
        public static dynamic getJson(string url, AuthType authType = AuthType.Integrated)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/json; odata=verbose";
            request.ContentType = "application/json; odata=verbose";
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return JObject.Parse(reader.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    string errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
        public static dynamic getJson(string url, string userName, string password, AuthType authType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Accept = "application/json; odata=verbose";
            request.ContentType = "application/json; odata=verbose";
            switch (authType)
            {
                case AuthType.Basic:
                    {
                        string authInfo = string.Format("{0}:{1}", userName, password);
                        authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
                        request.Headers["Authorization"] = "Basic " + authInfo;
                        break;
                    }
                case AuthType.NTLM:
                    {
                        if (userName.Contains("@"))
                        {
                            try { request.Credentials = new NetworkCredential(userName.Split('@')[0], password, userName.Split('@')[1]); }
                            catch (Exception) { /* Swallow Exception */ }
                        }
                        else if (userName.Contains("\\"))
                        {
                            try { request.Credentials = new NetworkCredential(userName.Split('\\')[1], password, userName.Split('\\')[0]); }
                            catch (Exception) { /* Swallow Exception */ }
                        }
                        else
                        {
                            // when all else fails...
                            request.Credentials = new NetworkCredential(userName, password);
                        }
                        break;
                    }
            }
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
                    return JObject.Parse(reader.ReadToEnd());
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
        public static void postJson(string url, string jsonContent, AuthType authType = AuthType.Integrated)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Credentials = System.Net.CredentialCache.DefaultCredentials;

            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }
        public static void postJson(string url, string jsonContent, string userName, string password, AuthType authType)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            switch (authType)
            {
                case AuthType.Basic:
                    {
                        string authInfo = string.Format("{0}:{1}", userName, password);
                        authInfo = Convert.ToBase64String(Encoding.UTF8.GetBytes(authInfo));
                        request.Headers["Authorization"] = "Basic " + authInfo;
                        break;
                    }
                case AuthType.NTLM:
                    {
                        if (userName.Contains("@"))
                        {
                            try { request.Credentials = new NetworkCredential(userName.Split('@')[0], password, userName.Split('@')[1]); }
                            catch (Exception) { /* Swallow Exception */ }
                        }
                        else if (userName.Contains("\\"))
                        {
                            try { request.Credentials = new NetworkCredential(userName.Split('\\')[1], password, userName.Split('\\')[0]); }
                            catch (Exception) { /* Swallow Exception */ }
                        }
                        else
                        {
                            // when all else fails...
                            request.Credentials = new NetworkCredential(userName, password);
                        }
                        break;
                    }
            }
            System.Text.UTF8Encoding encoding = new System.Text.UTF8Encoding();
            Byte[] byteArray = encoding.GetBytes(jsonContent);

            request.ContentLength = byteArray.Length;
            request.ContentType = @"application/json";

            using (Stream dataStream = request.GetRequestStream())
            {
                dataStream.Write(byteArray, 0, byteArray.Length);
            }
            long length = 0;
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    length = response.ContentLength;
                }
            }
            catch (WebException ex)
            {
                WebResponse errorResponse = ex.Response;
                using (Stream responseStream = errorResponse.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
                    String errorText = reader.ReadToEnd();
                    // log errorText
                }
                throw;
            }
        }

    }
}
