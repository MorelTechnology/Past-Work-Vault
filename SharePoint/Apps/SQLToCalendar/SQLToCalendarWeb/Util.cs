using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.SharePoint.Client;
using System.Data.SqlClient;

namespace SQLToCalendarWeb
{
    public class Util
    {
        public static string CreateConnectionString(ClientContext clientContext)
        {
            string connectionString = "";
            try
            {
                List sqlConfigList = clientContext.Web.Lists.GetByTitle("SQL Configuration");
                CamlQuery query = new CamlQuery();
                query.ViewXml = @"<View>
                                <Query>
                                    <Where>
                                        <In>
                                            <FieldRef Name='Title'/>
                                            <Values>
                                                <Value Type='Text'>Server</Value>
                                                <Value Type='Text'>Database</Value>
                                                <Value Type='Text'>Integrated Security</Value>
                                            </Values>
                                        </In>
                                    </Where>
                                </Query>
                                <ViewFields>
                                    <FieldRef Name='Title'/>
                                    <FieldRef Name='Value1'/>
                                </ViewFields>
                            </View>";
                Microsoft.SharePoint.Client.ListItemCollection items = sqlConfigList.GetItems(query);

                clientContext.Load(items);
                clientContext.ExecuteQuery();
                foreach (Microsoft.SharePoint.Client.ListItem item in items)
                {
                    connectionString += item["Title"] + "=" + item["Value1"] + ";";
                }
            }
            catch (Exception ex)
            {
                WriteError(HttpContext.Current.Response, "Could not retrieve this App's settings. Please check the App's config! Message details:\n" + ex.Message);
            }
            return connectionString;
        }

        public static string GetQuery(ClientContext clientContext, string queryName)
        {
            string sqlQuery = "";
            try
            {
                List sqlConfigList = clientContext.Web.Lists.GetByTitle("SQL Configuration");
                CamlQuery query = new CamlQuery();
                query.ViewXml = @"<View>
                                <Query>
                                    <Where>
                                        <Eq>
                                            <FieldRef Name='Title'/>
                                            <Value Type='Text'>" + queryName + @"</Value>
                                        </Eq>
                                    </Where>
                                </Query>
                            </View>";
                ListItemCollection items = sqlConfigList.GetItems(query);
                clientContext.Load(items);
                clientContext.ExecuteQuery();
                if (items.Count > 0)
                {
                    sqlQuery = items[0]["Value1"].ToString();
                }
            }
            catch (Exception ex)
            {
                WriteError(HttpContext.Current.Response, "Could not retrieve this App's settings. Please check the App's config! Message details:\n" + ex.Message);
            }
            return sqlQuery;
        }

        public static void WriteError(HttpResponse Response, string message)
        {
            Response.Write("<h2 style='color: red'>" + message + "</h2>");
        }

        public static IEnumerable<Dictionary<string, object>> Serialize(SqlDataReader reader)
        {
            List<Dictionary<string, object>> results = new List<Dictionary<string, object>>();
            List<string> cols = new List<string>();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                cols.Add(reader.GetName(i));
            }

            while (reader.Read())
            {
                results.Add(SerializeRow(cols, reader));
            }

            return results;
        }

        private static Dictionary<string, object> SerializeRow(IEnumerable<string> cols, SqlDataReader reader)
        {
            Dictionary<string, object> result = new Dictionary<string, object>();
            foreach (string col in cols)
            {
                result.Add(col, reader[col]);
            }

            return result;
        }
    }
}