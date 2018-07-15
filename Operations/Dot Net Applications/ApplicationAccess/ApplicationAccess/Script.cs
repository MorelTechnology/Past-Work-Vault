using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//ing RSSEDLL;
using System.Windows;
using System.IO;

namespace ApplicationAccess
{
    public class ScriptEngine
    {
        public enum environemnt { DEV, TEST, PROD };
        public static environemnt envCurrent = environemnt.DEV;

        // public static ScriptEngine script = new ScriptEngine();

        public DataTable runScript(DataTable myInputTable, string script, string appInstance, string outputTable = "")
        {
            try
            {
                DataTable myParams = new DataTable("Params");
                myParams.Columns.Add("name");
                myParams.Columns.Add("value");
                myParams.Rows.Add("@Script", script);
                myParams.Rows.Add("@App", appInstance);
                myParams.Rows.Add("@UserName", "trg\\smarc");
                myParams.Rows.Add("@Environment", @"DEFAULT");

                DataSet mySet = new DataSet("test");
                mySet.Tables.Add(myParams);
                if (myInputTable != null)
                    mySet.Tables.Add(myInputTable);

                srRSSE.clsScriptEngineSoapClient se = new srRSSE.clsScriptEngineSoapClient();
                System.ServiceModel.BasicHttpBinding x = (System.ServiceModel.BasicHttpBinding) se.ChannelFactory.Endpoint.Binding;
                x.MaxBufferSize = 1000000000;
                x.MaxReceivedMessageSize = 1000000000;

                StringWriter writer = new StringWriter();
                mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
                string xml = writer.ToString();
                var ds = se.ExecuteScript(xml);





                // RSSEController rc = new RSSEController();

                //rc.AddParamsEntry("@Script", script);
                //rc.AddParamsEntry("@App", appInstance);
                //rc.AddParamsEntry("@UserName", "trg\\smarc");
                //// rc.AddParamsEntry("@UserName", MainWindow.currentUser);
                //rc.AddParamsEntry("@Environment", @"DEFAULT");

                //if (myInputTable != null)
                //    rc.AddInputTable(myInputTable);

                //string target = "DEV";
                ////if (envCurrent == environemnt.TEST)
                ////    target = "TEST";
                ////if (envCurrent == environemnt.PROD)
                ////    target = "PROD";

                //bool authenticate = false;

                //DataSet ds = rc.CallWS(target, authenticate);

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
        public DataSet runScript(environemnt env, DataTable myInputTable, string script, string appInstance, bool auth = false, string extraParamName = "", string extraParamValue = "", string ePN2="", string ePV2="", string ePN3="", string ePV3="", string ePN4="", string ePV4="")
        {
            try
            {
                DataTable myParams = new DataTable("Params");
                myParams.Columns.Add("name");
                myParams.Columns.Add("value");
                myParams.Rows.Add("@Script", script);
                myParams.Rows.Add("@App", appInstance);
                myParams.Rows.Add("@UserName", "trg\\smarc");
                myParams.Rows.Add("@Environment", @"DEFAULT");
                if (extraParamName != "")
                    myParams.Rows.Add(extraParamName, extraParamValue);
                if (ePN2 != "")
                    myParams.Rows.Add(ePN2, ePV2);
                if (ePN3 != "")
                    myParams.Rows.Add(ePN3, ePV3);
                if (ePN4 != "")
                    myParams.Rows.Add(ePN4, ePV4);

                DataSet mySet = new DataSet("test");
                mySet.Tables.Add(myParams);
                if (myInputTable != null)
                    mySet.Tables.Add(myInputTable);

                srRSSE.clsScriptEngineSoapClient se = new srRSSE.clsScriptEngineSoapClient();
                System.ServiceModel.BasicHttpBinding x = (System.ServiceModel.BasicHttpBinding)se.ChannelFactory.Endpoint.Binding;
                x.MaxBufferSize = 1000000000;
                x.MaxReceivedMessageSize = 1000000000;

                StringWriter writer = new StringWriter();
                mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
                string xml = writer.ToString();
                var ds = se.ExecuteScript(xml);




                // RSSEController rc = new RSSEController();

                //script = "TEST_HANDSHAKE";
                //appInstance = "GENERAL";

                //rc.AddParamsEntry("@Script", script);
                //rc.AddParamsEntry("@App", appInstance);
                //rc.AddParamsEntry("@UserName", "trg\\smarc");
                ////                rc.AddParamsEntry("@UserName", MainWindow.currentUser);
                //rc.AddParamsEntry("@Environment", @"DEFAULT");

                //if (extraParamName != "")
                //    rc.AddParamsEntry(extraParamName, extraParamValue);

                //if (myInputTable != null)
                //    rc.AddInputTable(myInputTable);

                //string target = "DEV";
                //if (env == environemnt.TEST)
                //    target = "TEST";
                //if (env == environemnt.PROD)
                //    target = "PROD";

                //bool authenticate = auth;

                //DataSet ds = rc.CallWS(target, authenticate);
                //srRSSE.


                if ((ds.Tables.Count >= 1) && ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
                {
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
    }
}
