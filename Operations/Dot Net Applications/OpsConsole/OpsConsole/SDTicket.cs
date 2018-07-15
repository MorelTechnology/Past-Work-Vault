using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace OpsConsole
{
    class SDTicket
    {
        [DataContract]
        internal class RequestLongForm
        {
            [DataMember]
            internal string WORKORDERID = "";

            [DataMember]
            internal string REQUESTER = "";

            [DataMember]
            internal string REQUESTEREMAIL = "";

            [DataMember]
            internal string CREATEDBY = "";

            [DataMember]
            internal string CREATEDTIME = "";

            [DataMember]
            internal string DUEBYTIME = "";

            [DataMember]
            internal string RESPONDEDTIME = "";

            [DataMember]
            internal string COMPLETEDTIME = "";

            [DataMember]
            internal string SHORTDESCRIPTION = "";

            [DataMember]
            internal string TIMESPENTONREQ = "";

            [DataMember]
            internal string SUBJECT = "";

            [DataMember]
            internal string REQUESTTEMPLATE = "";

            [DataMember]
            internal string TEMPLATEID = "";

            [DataMember]
            internal string SLA = "";

            [DataMember]
            internal string ASSET = "";

            [DataMember]
            internal string DEPARTMENT = "";

            [DataMember]
            internal string SITE = "";

            [DataMember]
            internal string CATEGORY = "";

            [DataMember]
            internal string SUBCATEGORY = "";

            [DataMember]
            internal string ITEM = "";

            [DataMember]
            internal string TECHNICIAN = "";

            [DataMember]
            internal string STATUS = "";

            [DataMember]
            internal string PRIORITY = "";

            [DataMember]
            internal string REQUESTTYPE = "";

            [DataMember]
            internal string HASATTACHMENTS = "";

            [DataMember]
            internal string HASNOTES = "";

            [DataMember]
            internal string HASCONVERSATION = "";

            [DataMember]
            internal string GROUP = "";
        }



        const string workOrderIDtag = "<name>WORKORDERID</name><value>";

        public static string createTicket(string requester, string tech, string desc, string subj)
        {
            string input = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";
            input += "<operation>  ";
            input += "  <Details>";
            input += "    <parameter>";
            input += "      <name>REQUESTER</name>";
            input += "      <value>" + requester + "</value>";
            input += "    </parameter>";
            input += "    <parameter>";
            input += "      <name>SUBJECT</name>";
            input += "      <value>" + subj + "</value>";
            input += "    </parameter>";
            input += "    <parameter>";
            input += "      <name>REQUESTTEMPLATE</name>";
            input += "      <value>Default Request</value>";
            input += "    </parameter>";
            input += "    <parameter>";
            input += "      <name>PRIORITY</name>";
            input += "      <value>Low</value>";
            input += "    </parameter>";
            input += "    <parameter>";
            input += "      <name>IMPACT</name>";
            input += "      <value>None</value>";
            input += "    </parameter>";
            input += "    <parameter>";
            input += "      <name>URGENCY</name>";
            input += "      <value>Low</value>";
            input += "    </parameter>";
            input += "    <parameter>";
            input += "      <name>DESCRIPTION</name>";
            input += "      <value>" + desc + "</value>";
            input += "    </parameter>";
            input += "    <parameter>";
            //input += "      <name>TECHNICIAN</name>";
            input += "      <name>GROUP</name>";
            input += "      <value>Production Support</value>";
            //input += "      <value>" + tech + "</value>";
            input += "    </parameter>";
            input += "    <parameter>";
            input += "      <name>SITE</name>";
            input += "      <value>Manchester</value>";
            input += "    </parameter>";
            input += "  </Details>";
            input += "</operation>";

            // ee0eb78335d1e4ea02add34a18be2607
            // 0b63f1cf2ce5a0f63307bc53836cd910
            // ebd6098dbe251cc26eccf68617c8ed49
            // string json = GetData(@"https://sdpondemand.manageengine.com/api/request", "?scope=sdpodapi&authtoken=0b63f1cf2ce5a0f63307bc53836cd910&OPERATION_NAME=ADD_REQUEST&INPUT_DATA=" + System.Web.HttpUtility.UrlEncode(input));

            string json = GetData(@"https://sdpondemand.manageengine.com/api/request", "?scope=sdpodapi&authtoken=0b63f1cf2ce5a0f63307bc53836cd910&OPERATION_NAME=ADD_REQUEST&INPUT_DATA=" + Uri.EscapeUriString(input));
            return json;

        }

        static string GetData(string url, string Parameters)
        {
            try
            {
                WebRequest request = WebRequest.Create(url + Parameters);
                request.Method = "POST";
                string postData = "";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                return "ERROR " + ex.ToString();
            }
        }

        public static string extractTicketNumberFromResults(string result)
        {
            int idat = result.IndexOf(workOrderIDtag);
            if (idat > 0)
            {
                string temp = result.Substring(idat + workOrderIDtag.Length);
                int close = temp.IndexOf("<");
                return temp.Substring(0, close);
            }
            return "unknown";
        }


        public static RootObject readCategorySubcategoryAndItem()
        {
            RootObject sdCategories;

            string json = GetData(@"https://sdpondemand.manageengine.com/api/json/admin/category?scope=sdpodapi&authtoken=ebd6098dbe251cc26eccf68617c8ed49&OPERATION_NAME=GET_CATEGORY", "");

            byte[] byteArray = Encoding.ASCII.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);
            stream.Position = 0;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            sdCategories = (RootObject)serializer.ReadObject(stream);
            return sdCategories;
        }
        // https://sdpondemand.manageengine.com/api/json/admin/category?scope=sdpodapi&authtoken=ebd6098dbe251cc26eccf68617c8ed49&OPERATION_NAME=GET_CATEGORY


        public static RequestLongForm getServiceTicketEntry(string s)
        {
            int testint;
            if (int.TryParse(s, out testint) == false)
                return null;

            string json = GetData(@"https://sdpondemand.manageengine.com/api/json/request/" + s, "?scope=sdpodapi&authtoken=ebd6098dbe251cc26eccf68617c8ed49&OPERATION_NAME=GET_REQUEST");
            int first = json.IndexOf("\"Details\":");
            int last = json.LastIndexOf('}');

            // Fast fail on end of list or failure
            if (first < 0)
                return null;

            first += 10;
            json = "[" + json.Substring(first, last - first + 1) + "]";
            json = json.Replace(",\"name\":\"GET_REQUEST\"}}", "");
            json = json.Replace("}}}]", "}]");

            // ServiceDesk's JSON is not so .net compatible, not that .net's json serializer can't do it, it just sucks at it
            byte[] byteArray = Encoding.ASCII.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);
            stream.Position = 0;
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RequestLongForm[]));
            RequestLongForm[] req = (RequestLongForm[])serializer.ReadObject(stream);
            return req[0];
        }


        //static string GetData(string url, string Parameters)
        //{
        //    try
        //    {
        //        WebRequest request = WebRequest.Create(url + Parameters);
        //        request.Method = "POST";
        //        string postData = "";
        //        byte[] byteArray = Encoding.UTF8.GetBytes(postData);
        //        request.ContentType = "application/x-www-form-urlencoded";
        //        request.ContentLength = byteArray.Length;
        //        Stream dataStream = request.GetRequestStream();
        //        dataStream.Write(byteArray, 0, byteArray.Length);
        //        dataStream.Close();
        //        WebResponse response = request.GetResponse();
        //        Console.WriteLine(((HttpWebResponse)response).StatusDescription);
        //        dataStream = response.GetResponseStream();
        //        StreamReader reader = new StreamReader(dataStream);
        //        string responseFromServer = reader.ReadToEnd();
        //        reader.Close();
        //        dataStream.Close();
        //        response.Close();
        //        return responseFromServer;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.InnerException.ToString());
        //        return "";
        //    }
        //}



    }
}
