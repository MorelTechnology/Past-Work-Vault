using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;


namespace ApplicationAccess
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

        public static bool showTicket(TextBlock tbNote, string ticketNumber)
        {
            SDTicket.RequestLongForm ticketInfo = SDTicket.getServiceTicketEntry(ticketNumber);
            if (ticketInfo != null)
            {
                tbNote.Inlines.Clear();
                tbNote.Inlines.Add(new Run("Category: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CATEGORY + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("By: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CREATEDBY + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Time: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CREATEDTIME + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Dept: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.DEPARTMENT + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Requester: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.REQUESTER + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("E-Mail: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.REQUESTEREMAIL + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Subject: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.SUBJECT + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Desc: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.SHORTDESCRIPTION.TrimStart().Replace("&nbsp;", " ") + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Technician: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.TECHNICIAN + Environment.NewLine) { Foreground = Brushes.MidnightBlue });
                return true;
            }
            else
            {
                tbNote.Inlines.Clear();
                tbNote.Inlines.Add(new Run("Unable to find ticket") { FontWeight = FontWeights.Bold });
                return false;
                // MarkTicketAsBad(ticketNumber);
            }
        }

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

    }
}

