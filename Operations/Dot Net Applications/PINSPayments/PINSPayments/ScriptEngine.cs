using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RSSEDLL;

namespace PINSPayments
{
    class ScriptEngine
    {
        public enum environemnt { DEV, TEST, PROD };
        public static environemnt envCurrent = environemnt.DEV;

        public static ScriptEngine script = new ScriptEngine();

        public DataTable runScript(DataTable myInputTable, string script, string appInstance, string outputTable = "")
        {
            try
            {
                RSSEController rc = new RSSEController();

                rc.AddParamsEntry("@Script", script);
                rc.AddParamsEntry("@App", appInstance);
                rc.AddParamsEntry("@UserName", MainWindow.currentUser);
                rc.AddParamsEntry("@Environment", @"DEFAULT");

                if (myInputTable != null)
                    rc.AddInputTable(myInputTable);

                string target = "DEV";
                if (envCurrent == environemnt.TEST)
                    target = "TEST";
                if (envCurrent == environemnt.PROD)
                    target = "PROD";

                bool authenticate = false;

                DataSet ds = rc.CallWS(target, authenticate);

                if ((ds.Tables.Count >= 1) && ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
                {
                    MessageBox.Show("An error has occurred processing the script: " + script + ":" + Environment.NewLine + ds.Tables[0].Rows[0][0].ToString());
                    return null;
                }

                DataTable output = (outputTable != "") ? ds.Tables[outputTable] : ds.Tables[0];
                return output;
            }
            catch (Exception ex)
            {
                string error = "An error has been thrown while processing the script: " + script + ":" + Environment.NewLine + Environment.NewLine + "The error is: " + Environment.NewLine + ex.ToString();
                MessageBox.Show(error);
                return new DataTable();
            }
        }

        // I never really understood why the signature did not include the return type, that doesn't make any sense to me.
        // Shouldn't you be able to overload a function by just the return type?
        public DataSet runScript(environemnt env, DataTable myInputTable, string script, string appInstance, bool auth=false, string extraParamName="", string extraParamValue="", string epn2="", string epv2="", string epn3 = "", string epv3 = "", string epn4 = "", string epv4 = "", string epn5 = "", string epv5 = "", string epn6 = "", string epv6 = "")
        {
            try
            {
                RSSEController rc = new RSSEController();

                //script = "TEST_HANDSHAKE";
                //appInstance = "GENERAL";

                rc.AddParamsEntry("@Script", script);
                rc.AddParamsEntry("@App", appInstance);
                rc.AddParamsEntry("@UserName", MainWindow.currentUser);
                rc.AddParamsEntry("@Environment", @"DEFAULT");

                if (extraParamName != "")
                    rc.AddParamsEntry(extraParamName, extraParamValue);
                if (epn2 != "")
                    rc.AddParamsEntry(epn2, epv2);
                if (epn3 != "")
                    rc.AddParamsEntry(epn3, epv3);
                if (epn4 != "")
                    rc.AddParamsEntry(epn4, epv4);
                if (epn5 != "")
                    rc.AddParamsEntry(epn5, epv5);
                if (epn6 != "")
                    rc.AddParamsEntry(epn6, epv6);

                if (myInputTable != null)
                    rc.AddInputTable(myInputTable);

                string target = "DEV";
                if (env == environemnt.TEST)
                    target = "TEST";
                if (env == environemnt.PROD)
                    target = "PROD";

                bool authenticate = auth;

                DataSet ds = rc.CallWS(target, authenticate);

                if ((ds.Tables.Count >= 1) && ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
                {
 
                    if (ds.Tables[0].Rows[0]["SE_CustomErrorCode"].ToString().Contains("login failed"))
                    {
                        string message = ds.Tables[0].Rows[0]["SE_CustomErrorCode"].ToString();
                        string username = "";
                        string database = "";
                        int apostrophe = message.IndexOf("'");
                        if (apostrophe > 0)
                        {
                            username = message.Substring(apostrophe + 1);
                            int closingapostrophe = username.IndexOf("'");
                            if (closingapostrophe > 1)
                                username = username.Substring(0, closingapostrophe);
                        }
                        int datasource = message.IndexOf("Data Source");
                        if (datasource > 1)
                        {
                            database = message.Substring(datasource);
                        }
                        MessageBox.Show("Please submit a DBA ServiceDesk ticket requesting the login:" +Environment.NewLine + username + Environment.NewLine + Environment.NewLine + "Be added to the database:" + Environment.NewLine + database, "The database permissions have been lost");
                    }
                    MessageBox.Show("An error has occurred processing the script: " + script + ":" + Environment.NewLine + ds.Tables[0].Rows[0][0].ToString());
                    return null;
                }

                return ds;
            }
            catch (Exception ex)
            {
                string error = "An error has been thrown while processing the script: " + script + ":" + Environment.NewLine + Environment.NewLine + "The error is: " + Environment.NewLine + ex.ToString();
                MessageBox.Show(error);
                return new DataSet();
            }
        }

        public bool executeScript(environemnt env, DataTable myInputTable, string script, string appInstance, bool auth = false, string extraParamName = "", string extraParamValue = "", string epn2 = "", string epv2 = "", string epn3 = "", string epv3 = "", string epn4 = "", string epv4 = "", string epn5 = "", string epv5 = "", string epn6 = "", string epv6 = "")
        {
            try
            {
                RSSEController rc = new RSSEController();

                //script = "TEST_HANDSHAKE";
                //appInstance = "GENERAL";

                rc.AddParamsEntry("@Script", script);
                rc.AddParamsEntry("@App", appInstance);
                rc.AddParamsEntry("@UserName", MainWindow.currentUser);
                rc.AddParamsEntry("@Environment", @"DEFAULT");

                if (extraParamName != "")
                    rc.AddParamsEntry(extraParamName, extraParamValue);
                if (epn2 != "")
                    rc.AddParamsEntry(epn2, epv2);
                if (epn3 != "")
                    rc.AddParamsEntry(epn3, epv3);
                if (epn4 != "")
                    rc.AddParamsEntry(epn4, epv4);
                if (epn5 != "")
                    rc.AddParamsEntry(epn5, epv5);
                if (epn6 != "")
                    rc.AddParamsEntry(epn6, epv6);

                if (myInputTable != null)
                    rc.AddInputTable(myInputTable);

                string target = "DEV";
                if (env == environemnt.TEST)
                    target = "TEST";
                if (env == environemnt.PROD)
                    target = "PROD";

                bool authenticate = auth;

                DataSet ds = rc.CallWS(target, authenticate);

                return true;
            }
            catch (Exception ex)
            {
                string error = "An error has been thrown while processing the script: " + script + ":" + Environment.NewLine + Environment.NewLine + "The error is: " + Environment.NewLine + ex.ToString();
                MessageBox.Show(error);
                return false;
            }
        }

        #region Obsolete RSSE calls
        [Obsolete]
        public DataTable OLDrunScript(DataTable myInputTable, string script, string appInstance, string outputTable = "")
        {
            DataTable myParams = new DataTable("Params");
            myParams.Columns.Add("name");
            myParams.Columns.Add("value");
            myParams.Rows.Add("@Script", script);
            myParams.Rows.Add("@App", appInstance);
            myParams.Rows.Add("@UserName", MainWindow.currentUser);
            myParams.Rows.Add("@Environment", @"DEFAULT");
            myParams.Rows.Add("@NetworkUser", MainWindow.currentUser);
            //myParams.Rows.Add("@NetworkUser", System.Security.Principal.WindowsIdentity.GetCurrent().Name);
            
            DataSet mySet = new DataSet("test");
            mySet.Tables.Add(myParams);
            if (myInputTable != null)
                mySet.Tables.Add(myInputTable);

            StringWriter writer = new StringWriter();
            mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
            string xml = writer.ToString();

            DataSet ds = new DataSet();
            if (envCurrent == environemnt.DEV)
            {
                //rsseDev.clsScriptEngineSoapClient se = new rsseDev.clsScriptEngineSoapClient();
                //ds = se.ExecuteScript(xml);
            }

            if (envCurrent == environemnt.TEST)
            {
                //rsseTest.clsScriptEngineSoapClient se = new rsseTest.clsScriptEngineSoapClient();
                //ds = se.ExecuteScript(xml);
            }

            if (envCurrent == environemnt.PROD)
            {
                //rsseProd.clsScriptEngineSoapClient se = new rsseProd.clsScriptEngineSoapClient();
                //ds = se.ExecuteScript(xml);
            }

            if ((ds.Tables.Count >= 1) && ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
            {
                MessageBox.Show("An error has occurred processing the script: " + script + ":" + Environment.NewLine + ds.Tables[0].Rows[0][0].ToString());
                return null;
            }

            DataTable output = (outputTable != "") ? ds.Tables[outputTable] : ds.Tables[0];
            return output;
        }

        // I never really understood why the signature did not include the return type, that doesn't make any sense to me.
        // Shouldn't you be able to overload a function by just the return type?
        [Obsolete]
        public DataSet OLDrunScript(environemnt env, DataTable myInputTable, string script, string appInstance)
        {
            DataTable myParams = new DataTable("Params");
            myParams.Columns.Add("name");
            myParams.Columns.Add("value");
            myParams.Rows.Add("@Script", script);
            myParams.Rows.Add("@App", appInstance);
            myParams.Rows.Add("@UserName", MainWindow.currentUser);
            myParams.Rows.Add("@Environment", @"DEFAULT");
            myParams.Rows.Add("@NetworkUser", MainWindow.currentUser);

            DataSet mySet = new DataSet("test");
            mySet.Tables.Add(myParams);
            if (myInputTable != null)
                mySet.Tables.Add(myInputTable);

            StringWriter writer = new StringWriter();
            mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
            string xml = writer.ToString();

            if (env == environemnt.DEV)
            {
                //rsseDev.clsScriptEngineSoapClient se = new rsseDev.clsScriptEngineSoapClient();
                //return se.ExecuteScript(xml);
            }

            if (env == environemnt.TEST)
            {
                //rsseTest.clsScriptEngineSoapClient se = new rsseTest.clsScriptEngineSoapClient();
                //return se.ExecuteScript(xml);
            }

            if (env == environemnt.PROD)
            {
                //rsseProd.clsScriptEngineSoapClient se = new rsseProd.clsScriptEngineSoapClient();
                //return se.ExecuteScript(xml);
            }

            return new DataSet();
        }
        #endregion

    }
}
