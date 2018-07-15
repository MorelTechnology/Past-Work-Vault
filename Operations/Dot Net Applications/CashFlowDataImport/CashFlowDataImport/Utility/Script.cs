using System;
using System.Data;
using System.IO;
using System.Linq;

namespace CashFlowDataImport.Utility
{
    class Script
    {
        public class ScriptEngine
        {
            public enum environment { DEV, TEST, PROD };
            public static environment envCurrent = environment.DEV;

            public static DataTable runScript(DataTable myInputTable, string script, string appInstance, string outputTable = "")
            {
                try
                {
                    DataTable myParams = new DataTable("Params");
                    myParams.Columns.Add("name");
                    myParams.Columns.Add("value");
                    myParams.Rows.Add("@Script", script);
                    myParams.Rows.Add("@App", appInstance);
                    myParams.Rows.Add("@UserName", "trg\\jmore");
                    myParams.Rows.Add("@Environment", @"DEFAULT");

                    DataSet mySet = new DataSet("test");
                    mySet.Tables.Add(myParams);
                    if (myInputTable != null)
                        mySet.Tables.Add(myInputTable);

                    rsseDEV.clsScriptEngineSoapClient se = new rsseDEV.clsScriptEngineSoapClient();
                    System.ServiceModel.BasicHttpBinding x = (System.ServiceModel.BasicHttpBinding)se.ChannelFactory.Endpoint.Binding;
                    x.MaxBufferSize = 1000000000;
                    x.MaxReceivedMessageSize = 1000000000;

                    StringWriter writer = new StringWriter();
                    mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
                    string xml = writer.ToString();
                    var ds = se.ExecuteScript(xml);

                    if ((ds.Tables.Count >= 1) && ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
                    {
                       Console.Write("An error has occurred processing the script: " + script + ":" + Environment.NewLine + ds.Tables[0].Rows[0][0].ToString());
                        return null;
                    }

                    DataTable output = (outputTable != "") ? ds.Tables[outputTable] : ds.Tables[0];
                    return output;
                }
                catch (Exception ex)
                {
                    string error = "An error has been thrown while processing the script: " + script + ":" + Environment.NewLine + Environment.NewLine + "The error is: " + Environment.NewLine + ex.ToString();
                    Console.Write(error);
                    return new DataTable();
                }
            }

            public static DataSet runScript(environment env, DataTable myInputTable, string script, string appInstance, bool auth = false, string extraParamName = "", string extraParamValue = "", string ePN2 = "", string ePV2 = "", string ePN3 = "", string ePV3 = "", string ePN4 = "", string ePV4 = "")
            {
                try
                {
                    DataTable myParams = new DataTable("Params");
                    myParams.Columns.Add("name");
                    myParams.Columns.Add("value");
                    myParams.Rows.Add("@Script", script);
                    myParams.Rows.Add("@App", appInstance);
                    myParams.Rows.Add("@UserName", "trg\\jmore");
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

                    rsseDEV.clsScriptEngineSoapClient se = new rsseDEV.clsScriptEngineSoapClient();
                    System.ServiceModel.BasicHttpBinding x = (System.ServiceModel.BasicHttpBinding)se.ChannelFactory.Endpoint.Binding;
                    x.MaxBufferSize = 1000000000;
                    x.MaxReceivedMessageSize = 1000000000;

                    StringWriter writer = new StringWriter();
                    mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
                    string xml = writer.ToString();
                    var ds = se.ExecuteScript(xml);

                    if ((ds.Tables.Count >= 1) && ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
                    {
                        Console.Write("An error has occurred processing the script: " + script + ":" + Environment.NewLine + ds.Tables[0].Rows[0][0].ToString());
                        return null;
                    }

                    return ds;
                }
                catch (Exception ex)
                {
                    string error = "An error has been thrown while processing the script: " + script + ":" + Environment.NewLine + Environment.NewLine + "The error is: " + Environment.NewLine + ex.ToString();
                    Console.Write(error);
                    return new DataSet();
                }
            }
        }
         
        public class Actions
        {
            public static string getAccountByLastName(string lastName)
            {
                // Attempt to resolve last name to userlogin. 
                // an ambiguous result returns the lastname passed in, since something's better than nothing, right?

                // Hack - some users have an appostrophe in their last name, so this escapes it.  It also takes out
                // extra spaces at front and back, but leaves possibly valid ones in the middle.
                lastName = lastName.Replace("'","''").TrimStart().TrimEnd();
                DataTable Users = ScriptEngine.runScript(new DataTable(), "CASH_GETUSERS", "OPSCONSOLE");
                var searchUser = Users.Select("DisplayName like '" + lastName + "%'");
                return (searchUser.Count() != 1) ? lastName : @"trg\" + searchUser[0]["SamAccountName"].ToString();
            }

            public static string getSid(string samAccountName)
            {
                // Attempt to resolve AD User SID from userlogin. 
                // an ambiguous result returns string.empty

                DataTable Users = ScriptEngine.runScript(new DataTable(), "CASH_GETUSERS", "OPSCONSOLE");
                var searchUser = Users.Select("SamAccountName = '"+ samAccountName+"'");
                return (searchUser.Count() != 1) ? String.Empty : searchUser[0]["ActiveDirectoryID"].ToString();
            }

        }
    }
}
