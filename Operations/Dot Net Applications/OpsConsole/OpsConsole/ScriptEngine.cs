using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using RSSEDLL;

namespace OpsConsole
{
    class ScriptEngine
    {
        public enum environemnt { DEV, TEST, PROD };
        public static environemnt envCurrent = environemnt.PROD;

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
        public DataSet runScript(environemnt env, DataTable myInputTable, string script, string appInstance, bool auth=false, string extraParamName="", string extraParamValue="")
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
