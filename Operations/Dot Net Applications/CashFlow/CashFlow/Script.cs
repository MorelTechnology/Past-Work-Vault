using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CashFlow
{
    public class ScriptEngine
    {
        public enum environemnt { DEV, TEST, PROD };
        public static environemnt envCurrent = environemnt.TEST;
        
        //public DataTable runScript(DataTable myInputTable, string script, string appInstance, string outputTable = "")
        //{
        //    MessageBox.Show("Attept to run RSSE script " + script);
        //    return new DataTable();
        //    try
        //    {
        //        DataTable myParams = new DataTable("Params");
        //        myParams.Columns.Add("name");
        //        myParams.Columns.Add("value");
        //        myParams.Rows.Add("@Script", script);
        //        myParams.Rows.Add("@App", appInstance);

        //        // NOTE NOTE FIX HELP BAD - MAKE THE REAL USER'S NAME
        //        myParams.Rows.Add("@UserName", "trg\\smarc");
        //        myParams.Rows.Add("@Environment", @"DEFAULT");

        //        DataSet mySet = new DataSet("test");
        //        mySet.Tables.Add(myParams);
        //        if (myInputTable != null)
        //            mySet.Tables.Add(myInputTable);

        //        // DEV TEST

        //        StringWriter writer = new StringWriter();
        //        mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
        //        string xml = writer.ToString();
        //        DataSet ds = new DataSet();

        //        if (envCurrent == environemnt.DEV)
        //        {
        //            rsseDEV.clsScriptEngineSoapClient se = new rsseDEV.clsScriptEngineSoapClient();
        //            System.ServiceModel.BasicHttpBinding x = (System.ServiceModel.BasicHttpBinding)se.ChannelFactory.Endpoint.Binding;
        //            x.MaxBufferSize = 1000000000;
        //            x.MaxReceivedMessageSize = 1000000000;
        //            ds = se.ExecuteScript(xml);
        //        }

        //        if (envCurrent == environemnt.TEST)
        //        {
        //            rsseTEST.clsScriptEngineSoapClient se = new rsseTEST.clsScriptEngineSoapClient();
        //            System.ServiceModel.BasicHttpBinding x = (System.ServiceModel.BasicHttpBinding)se.ChannelFactory.Endpoint.Binding;
        //            x.MaxBufferSize = 1000000000;
        //            x.MaxReceivedMessageSize = 1000000000;
        //            ds = se.ExecuteScript(xml);
        //        }


        //        if ((ds.Tables.Count >= 1) && ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
        //        {
        //            MessageBox.Show("An error has occurred processing the script: " + script + ":" + Environment.NewLine + ds.Tables[0].Rows[0][0].ToString());
        //            return null;
        //        }

        //        DataTable output = (outputTable != "") ? ds.Tables[outputTable] : ds.Tables[0];
        //        return output;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = "An error has been thrown while processing the script: " + script + ":" + Environment.NewLine + Environment.NewLine + "The error is: " + Environment.NewLine + ex.ToString();
        //        MessageBox.Show(error);
        //        return new DataTable();
        //    }
        //}

        //public DataSet runScript(environemnt env, DataTable myInputTable, string script, string appInstance, bool auth = false, string extraParamName = "", string extraParamValue = "", string ePN2 = "", string ePV2 = "", string ePN3 = "", string ePV3 = "", string ePN4 = "", string ePV4 = "")
        //{
        //    MessageBox.Show("Attept to run RSSE script " + script);
        //    return new DataSet();
        //    try
        //    {
        //        DataTable myParams = new DataTable("Params");
        //        myParams.Columns.Add("name");
        //        myParams.Columns.Add("value");
        //        myParams.Rows.Add("@Script", script);
        //        myParams.Rows.Add("@App", appInstance);
        //        myParams.Rows.Add("@UserName", "trg\\smarc");
        //        myParams.Rows.Add("@Environment", @"DEFAULT");
        //        if (extraParamName != "")
        //            myParams.Rows.Add(extraParamName, extraParamValue);
        //        if (ePN2 != "")
        //            myParams.Rows.Add(ePN2, ePV2);
        //        if (ePN3 != "")
        //            myParams.Rows.Add(ePN3, ePV3);
        //        if (ePN4 != "")
        //            myParams.Rows.Add(ePN4, ePV4);

        //        DataSet mySet = new DataSet("test");
        //        mySet.Tables.Add(myParams);
        //        if (myInputTable != null)
        //            mySet.Tables.Add(myInputTable);

        //        StringWriter writer = new StringWriter();
        //        mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
        //        string xml = writer.ToString();

        //        DataSet ds = new DataSet();

        //        if (envCurrent == environemnt.DEV)
        //        {
        //            rsseDEV.clsScriptEngineSoapClient se = new rsseDEV.clsScriptEngineSoapClient();
        //            System.ServiceModel.BasicHttpBinding x = (System.ServiceModel.BasicHttpBinding)se.ChannelFactory.Endpoint.Binding;
        //            x.MaxBufferSize = 1000000000;
        //            x.MaxReceivedMessageSize = 1000000000;
        //            ds = se.ExecuteScript(xml);
        //        }

        //        if (envCurrent == environemnt.TEST)
        //        {
        //            rsseTEST.clsScriptEngineSoapClient se = new rsseTEST.clsScriptEngineSoapClient();
        //            System.ServiceModel.BasicHttpBinding x = (System.ServiceModel.BasicHttpBinding)se.ChannelFactory.Endpoint.Binding;
        //            x.MaxBufferSize = 1000000000;
        //            x.MaxReceivedMessageSize = 1000000000;
        //            ds = se.ExecuteScript(xml);
        //        }

        //        if ((ds.Tables.Count >= 1) && ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
        //        {
        //            MessageBox.Show("An error has occurred processing the script: " + script + ":" + Environment.NewLine + ds.Tables[0].Rows[0][0].ToString());
        //            return null;
        //        }

        //        return ds;
        //    }
        //    catch (Exception ex)
        //    {
        //        string error = "An error has been thrown while processing the script: " + script + ":" + Environment.NewLine + Environment.NewLine + "The error is: " + Environment.NewLine + ex.ToString();
        //        MessageBox.Show(error);
        //        return new DataSet();
        //    }
        //}
    }
}
