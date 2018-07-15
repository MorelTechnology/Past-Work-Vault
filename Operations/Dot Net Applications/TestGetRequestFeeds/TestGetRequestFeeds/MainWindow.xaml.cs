using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TestGetRequestFeeds
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Data
        static DataTable requests = new DataTable();
        static DataTable DST = new DataTable();
        Dictionary<string, int> processed = new Dictionary<string, int>();

        #endregion

        #region Class members
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        #endregion

        #region Constants
        const int numHeaderRows = 5;
        const int numColsInCSV = 79;
        static int msBetweenAPIRequests = 1000;
        static string reportName = "";
        static string filePath = "";
        static string authToken = "ee0eb78335d1e4ea02add34a18be2607";
        public static string connectionStringData = "Data Source=SQLTEST2012R2;Initial Catalog=ServiceDesk;Integrated Security=True";
        static string connectionStringFile = "";
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        #endregion
        
        #region Data Contract class for Long form of Request
        [DataContract]
        internal class RequestLongForm
        {
#pragma warning disable 0649

            [DataMember]
            internal string WORKORDERID;

            [DataMember]
            internal string REQUESTER;

            [DataMember]
            internal string REQUESTEREMAIL;

            [DataMember]
            internal string CREATEDBY;

            [DataMember]
            internal string CREATEDTIME;

            [DataMember]
            internal string DUEBYTIME;

            [DataMember]
            internal string RESPONDEDTIME;

            [DataMember]
            internal string COMPLETEDTIME;

            [DataMember]
            internal string SHORTDESCRIPTION;

            [DataMember]
            internal string TIMESPENTONREQ;

            [DataMember]
            internal string SUBJECT;

            [DataMember]
            internal string REQUESTTEMPLATE;

            [DataMember]
            internal string TEMPLATEID;

            [DataMember]
            internal string SLA;

            [DataMember]
            internal string ASSET;

            [DataMember]
            internal string DEPARTMENT;

            [DataMember]
            internal string SITE;

            [DataMember]
            internal string CATEGORY;

            [DataMember]
            internal string SUBCATEGORY;

            [DataMember]
            internal string ITEM;

            [DataMember]
            internal string TECHNICIAN;

            [DataMember]
            internal string STATUS;

            [DataMember]
            internal string PRIORITY;

            [DataMember]
            internal string REQUESTTYPE;

            [DataMember]
            internal string HASATTACHMENTS;

            [DataMember]
            internal string HASNOTES;

            [DataMember]
            internal string HASCONVERSATION;

            [DataMember]
            internal string GROUP;

            [DataMember]
            internal string UDF_CHAR1;

            [DataMember]
            internal string UDF_CHAR2;

            [DataMember]
            internal string UDF_CHAR3;

            [DataMember]
            internal string UDF_CHAR4;

            [DataMember]
            internal string UDF_CHAR5;

            [DataMember]
            internal string UDF_CHAR6;

            [DataMember]
            internal string UDF_CHAR7;

            [DataMember]
            internal string UDF_CHAR8;

            [DataMember]
            internal string UDF_CHAR9;

            [DataMember]
            internal string UDF_CHAR10;

            [DataMember]
            internal string UDF_CHAR11;

            [DataMember]
            internal string UDF_CHAR12;

            [DataMember]
            internal string UDF_CHAR13;

            [DataMember]
            internal string UDF_CHAR14;

            [DataMember]
            internal string UDF_CHAR15;

            [DataMember]
            internal string UDF_CHAR16;

            [DataMember]
            internal string UDF_CHAR17;

            [DataMember]
            internal string UDF_CHAR18;

            [DataMember]
            internal string UDF_CHAR19;

            [DataMember]
            internal string UDF_CHAR20;

            [DataMember]
            internal string UDF_CHAR21;

            [DataMember]
            internal string UDF_CHAR22;

            [DataMember]
            internal string UDF_CHAR23;

            [DataMember]
            internal string UDF_CHAR24;

            [DataMember]
            internal string UDF_DATE1;

            [DataMember]
            internal string UDF_DATE2;

            [DataMember]
            internal string UDF_DATE3;

            [DataMember]
            internal string UDF_DATE4;

            [DataMember]
            internal string LONG_REQUESTID;

#pragma warning restore 0649
        }




        [DataContract]
        internal class RequestFeed
        {
#pragma warning disable 0649

            [DataMember]
            internal string UPDATEDTIME;

            [DataMember]
            internal string WORKORDERID;

            [DataMember]
            internal string SUBJECT;

            [DataMember]
            internal string CREATEDTIME;

            [DataMember]
            internal string LONG_REQUESTID;

#pragma warning restore 0649
        }



        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            getDSTDates();
            createFeedTable();
            processRequestFeeds();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 30, 0);
            dispatcherTimer.Start();
        }
        #endregion

        #region Core Logic
        private void createFeedTable()
        {
            requests = new DataTable();
            requests.Columns.Add("WorkOrder");
            requests.Columns.Add("Updated");
            requests.Columns.Add("Subject");
            requests.Columns.Add("Created");
            requests.Columns.Add("LongID");
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            processRequestFeeds();
        }

        private void processRequestFeeds()
        {
            ////// GET CURRENT FEEDS //////
            getRequestFeeds();

            ////// PROCESS TICKETS THAT HAVEN'T BEEN //////
            foreach (DataRow drTicket in requests.Rows)
            {
                if (ticketNeedsProcessing(drTicket["WORKORDER"].ToString(), UnixTimeinMStoDateTime(drTicket["UPDATED"].ToString())))
                    UpdateOrInsertTicket(drTicket["WORKORDER"].ToString());
            }
        }

        private bool ticketNeedsProcessing(string workorderid, DateTime updatedTime)
        {
            return true;
        }

        private void UpdateOrInsertTicket(string workorderid)
        {
            try
            {
                readLongFormRequest(workorderid);
                // readHistory(drfound["WorkOrderID"].ToString());
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region ManageEngine API
        private void readLongFormRequest(string s)
        {
            string json = "";
            try
            {
                json = GetData(@"https://sdpondemand.manageengine.com/api/json/request/" + s, "?scope=sdpodapi&authtoken=" + authToken + "&OPERATION_NAME=GET_REQUEST");
            }
            catch (Exception ex)
            {
                // WriteLog(s, "ERROR IN readLongFormRequest GetData(): " + ex.ToString());
                throw ex;
            }

            try
            {
                int first = json.IndexOf("\"Details\":");
                int last = json.LastIndexOf('}');

                ////// Fast fail on end of list or failure //////
                if (first < 0)
                    return;

                first += 10;
                json = "[" + json.Substring(first, last - first + 1) + "]";

                json = json.Replace(",\"name\":\"GET_REQUEST\"}}", "");
                json = json.Replace("}}}]", "}]");


                ////// ServiceDesk's JSON is not so .net compatible, not that .net's json serializer can't do it, it just sucks at it //////
                byte[] byteArray = Encoding.ASCII.GetBytes(json);
                MemoryStream stream = new MemoryStream(byteArray);
                stream.Position = 0;
                //DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RequestFeed[]));
                //RequestFeed[] req = (RequestFeed[])serializer.ReadObject(stream);
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RequestLongForm[]));
                RequestLongForm[] req = (RequestLongForm[])serializer.ReadObject(stream);


                ////// CREATE TICKET IF IT DOESN'T EXIST
                if (!isTicketInDatabase(req[0].WORKORDERID.ToString()))
                    createNewTicket(req[0]);

                ////// THEN UPDATE IT //////
                updateLongRequestToDatabase(req[0]);
            }
            catch (Exception ex)
            {
                //                WriteLog(s, "ERROR IN readLongFormRequest: " + ex.ToString());
                throw ex;
            }
        }

        private void getRequestFeeds()
        {
            try
            {
                requests.Rows.Clear();
                string json = GetData(@"https://sdpondemand.manageengine.com/api/json/request_feeds", "?scope=sdpodapi&authtoken=" + authToken + "&OPERATION_NAME=GET_REQUEST_FEEDS");
                try
                {
                    int first = json.IndexOf("\"Details\":");
                    int last = json.LastIndexOf(']');

                    ////// Fast fail on end of list or failure //////
                    if (first < 0)
                        return;

                    first += 10;
                    json = "[" + json.Substring(first, last - first + 1) + "]";

                    json = json.Replace("[[", "[");
                    json = json.Replace("]]", "]");

                    //json = json.Replace(",\"name\":\"GET_REQUEST_FEEDS\"", "");
                    //json = json.Replace("}}}]", "}]");

                    ////// ServiceDesk's JSON is not so .net compatible, not that .net's json serializer can't do it, it just sucks at it //////
                    byte[] byteArray = Encoding.ASCII.GetBytes(json);
                    MemoryStream stream = new MemoryStream(byteArray);
                    stream.Position = 0;

                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RequestFeed[]));
                    RequestFeed[] req = (RequestFeed[])serializer.ReadObject(stream);


                    foreach (RequestFeed r in req)
                        requests.Rows.Add(r.WORKORDERID, r.UPDATEDTIME, r.SUBJECT, r.CREATEDTIME, r.LONG_REQUESTID);

                    dg1.ItemsSource = requests.DefaultView;
                }
                catch (Exception ex)
                {
                    dg1.ItemsSource = null;
                    //                WriteLog(s, "ERROR IN readLongFormRequest: " + ex.ToString());
                    return;
                }
            }
            catch (Exception ex)
            {
                //WriteLog("0", "Exception in readHistory: url=" + Sanitize(s + Environment.NewLine + ex.ToString()));
            }
        }


        private static void readHistory(string s)
        {
            try
            {
                string json = GetData(@"https://sdpondemand.manageengine.com/api/json/request/" + s, "?scope=sdpodapi&authtoken=" + authToken + "&OPERATION_NAME=GET_HISTORY");
                updateSingleFieldToDatabase(s, "History", json);
            }
            catch (Exception ex)
            {
                WriteLog("0", "Exception in readHistory: url=" + Sanitize(s + Environment.NewLine + ex.ToString()));
            }
        }

        private static void updateSingleFieldToDatabase(string id, string field, string json)
        {
            string sql = "update Request set ";

            sql += "[" + field + "]='" + Sanitize(json) + "' ";
            sql += "WHERE WorkOrderID=" + id;
            executeSQL(sql, connectionStringData);
            WriteLog(id, "UPDATE " + field.ToUpper());
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
                WriteLog("0", "Exception in GetData: url=" + url + Parameters + Environment.NewLine + ex.ToString());
                return "";
            }
        }
        #endregion
        
        #region Database routines
        private static void executeSQL(string sql, string connectionString)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = con.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("0", "SQL Exception: " + ex.ToString());
            }

        }

        private void updateLongRequestToDatabase(RequestLongForm r)
        {
            //string completeTime = (r.COMPLETEDTIME.Length < 10) ? "null" : ("'" + (UnixEpoch.AddMilliseconds(Convert.ToDouble(r.COMPLETEDTIME))).ToShortTimeString() + "'");
            //string respondedTime = (r.RESPONDEDTIME.Length < 10) ? "null" : ("'" + (UnixEpoch.AddMilliseconds(Convert.ToDouble(r.RESPONDEDTIME))).ToShortTimeString() +"'");
            //string dueByTime = (r.DUEBYTIME.Length < 10) ? "null" : "'" + (UnixEpoch.AddMilliseconds(Convert.ToDouble(r.DUEBYTIME))).ToShortTimeString() + "'";

            string sql = "update Request set ";

            sql += "[RequesterEmail]='" + Sanitize(r.REQUESTEREMAIL) + "',";
            sql += "[CreatedTime]=" + UnixTimeinMStoSQLTime(r.CREATEDTIME) + ",";
            sql += "[DueByTime]=" + UnixTimeinMStoSQLTime(r.DUEBYTIME) + ",";
            sql += "[RespondedTime]=" + UnixTimeinMStoSQLTime(r.RESPONDEDTIME) + ",";
            sql += "[CompletedTime]=" + UnixTimeinMStoSQLTime(r.COMPLETEDTIME) + ",";
            sql += "[ShortDescription]='" + Sanitize(r.SHORTDESCRIPTION) + "',";
            sql += "[TimeSpentOnReqInMinutes]=" + ConvertHrsMnsStringToMinutes(r.TIMESPENTONREQ) + ",";
            sql += "[RequestTemplate]='" + r.REQUESTTEMPLATE + "',";
            sql += "[TemplateID]='" + r.TEMPLATEID + "',";
            sql += "[SLA]='" + r.SLA + "',";
            //sql += "[NotificationStatus]='" + r.Not + "',";
            sql += "[Asset]='" + Sanitize(r.ASSET) + "',";
            sql += "[HasAttachments]=" + ((r.HASATTACHMENTS.ToLower() == "true") ? "1" : "0") + ",";
            sql += "[HasNotes]=" + ((r.HASNOTES.ToLower() == "true") ? "1" : "0") + ",";
            sql += "[HasConversation]=" + ((r.HASCONVERSATION.ToLower() == "true") ? "1" : "0") + ",";
            sql += "[UDF_DATE1]=" + UnixTimeinMStoSQLTime(r.UDF_DATE1) + ",";
            sql += "[UDF_DATE2]=" + UnixTimeinMStoSQLTime(r.UDF_DATE2) + ",";
            sql += "[UDF_DATE3]=" + UnixTimeinMStoSQLTime(r.UDF_DATE3) + ",";
            sql += "[UDF_DATE4]=" + UnixTimeinMStoSQLTime(r.UDF_DATE4) + ",";
            sql += "[LongRequestID]='" + r.LONG_REQUESTID + "', ";
            sql += "[HasFullInfo]=1 ";
            sql += "WHERE WorkOrderID=" + r.WORKORDERID.ToString();

            executeSQL(sql, connectionStringData);
            WriteLog(r.WORKORDERID, "UPDATE FULL");
        }

        private void createNewTicket(RequestLongForm r)
        {
            string sql = "insert into [ServiceDesk].[dbo].[Request] (WorkOrderID) VALUES(" + r.WORKORDERID.ToString() + ")";
            executeSQL(sql, connectionStringData);
            // WriteLog(r.WORKORDERID, "UPDATE FULL");
        }


        private bool isTicketInDatabase(string workorderid)
        {
            try
            {
                DataTable dtInDB = getData("select top 1 WorkOrderID FROM [ServiceDesk].[dbo].[Request] where WorkOrderID=" + workorderid, connectionStringData);
                return (dtInDB.Rows.Count > 0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        private static DataTable getData(string sql, string connectionString)
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connectionString))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        // This version of .net does not support default parameters, so.......
        private static string Sanitize(string s)
        {
            return Sanitize(s, false);
        }

        private static string Sanitize(string s, bool partial)
        {
            if (s == null)
                return "";

            s = s.Replace("&rsquo;", "'");
            s = s.Replace("&rdquo;", "\"");
            s = s.Replace("&#92", "\\");
            s = s.Replace("&gt", ">");
            s = s.Replace("&lt", "<");

            if (!partial)
            {
                s = s.Replace("'", "''");
                s = s.Replace("\"\"\"", "");
            }
            return s;
        }

        #endregion

        #region Finding, Parsing and Logging helper routines
        private static DataRow findTicket(string workOrderID)
        {
            foreach (DataRow dr in requests.Rows)
                if (dr["WorkOrderID"].ToString() == workOrderID)
                    return dr;
            return null;
        }
        static int colExcelColnameToOffset(string colname)
        {
            return (colname.Length == 1) ? (colname[0] - 'A') : ((colname[0] - 'A' + 1) * 26 + colname[1] - 'A');
        }

        private static string getColData(string[] rd, string colname)
        {
            // TEMP
            //foreach (map m in mymap)
            //    if (m.dbField == colname)
            //    {
            //        string val = rd[colExcelColnameToOffset(m.column)];

            //        if (colname == "Requester")
            //            val = LCFtoFSL(val);

            //        return ((val == "-") ? "" : val);
            //    }
            return "";
        }

        // April 4, 2017
        // Convert "Marcus, Scott" to "Scott Marcus"
        static string LCFtoFSL(string name)
        {
            int comma = name.IndexOf(',');
            if (comma <= 0)
                return name;

            return name.Substring(comma + 2) + " " + name.Substring(0, comma);
        }

        //private static string[] splitCSVRow(string row)
        //{
        //    // TEMP
        //    return new string[];
        //    //using (Microsoft.VisualBasic.FileIO.TextFieldParser tfp = new Microsoft.VisualBasic.FileIO.TextFieldParser(new MemoryStream(Encoding.UTF8.GetBytes(row ?? ""))))
        //    //{
        //    //    tfp.Delimiters = new string[] { "," };
        //    //    tfp.HasFieldsEnclosedInQuotes = true;
        //    //    string[] output = tfp.ReadFields();
        //    //    return output;
        //    //}
        //}

        static void WriteLog(string workorderid, string action)
        {
            executeSQL("insert into ImportHistory ([Date], [WorkOrderID], [Action], [Comment]) VALUES(GETDATE(), " + workorderid + ", '" + action + "', '')", connectionStringData);
        }
        #endregion

        #region Unix Time with milliseconds Pacific Time Zone to a SQL date/time in Eastern Time
        private static int ConvertHrsMnsStringToMinutes(string weirdTimeString)
        {
            int hrsTag = weirdTimeString.IndexOf("hrs");
            if (hrsTag <= 0)
                return 0;

            int hours = Convert.ToInt32(weirdTimeString.Substring(0, hrsTag));
            int minTag = weirdTimeString.IndexOf("min");
            int min = Convert.ToInt32(weirdTimeString.Substring(hrsTag + 4, minTag - (hrsTag + 4)));
            int minutes = hours * 60 + min;
            return minutes;
        }

        private DateTime UnixTimeinMStoDateTime(string unixtime)
        {
            string datetime = UnixTimeinMStoSQLTime(unixtime);

            DateTime ret = new DateTime();
            if (DateTime.TryParse(datetime, out ret))
                return ret;
            return DateTime.Now;
        }

        private string UnixTimeinMStoSQLTime(string unixtime)
        {
            if (unixtime == null)
                return "null";

            if (unixtime.ToLower() == "null")
                return "null";

            if (unixtime == "")
                return "null";

            DateTime ourTime = (UnixEpoch.AddMilliseconds(Convert.ToDouble(unixtime)));


            ourTime = ourTime.AddHours(-5);
            if (isThisTimeInDST(ourTime))
                ourTime = ourTime.AddHours(+1);

            string ourTimeString = "'" + ourTime.ToShortDateString() + " " + ourTime.ToShortTimeString() + "'";
            return ourTimeString;
        }

        private void getDSTDates()
        {
            DST = getData("select StartDate, EndDate from dbo.DaylightSavingsTime", connectionStringData);
        }

        private bool isThisTimeInDST(DateTime day)
        {
            foreach (DataRow dr in DST.Rows)
            {
                DateTime dtStart = Convert.ToDateTime(dr["StartDate"].ToString());
                DateTime dtEnd = Convert.ToDateTime(dr["EndDate"].ToString());

                if ((day >= dtStart) && (day <= dtEnd))
                    return true;
            }
            return false;
        }

        #endregion

    }
}
