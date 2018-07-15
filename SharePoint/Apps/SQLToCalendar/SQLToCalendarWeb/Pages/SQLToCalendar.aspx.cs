using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.SharePoint.Client;
using System.Data.SqlClient;
using Newtonsoft.Json;

namespace SQLToCalendarWeb.Pages
{
    public partial class SQLToCalendar : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // The following code gets the client context and Title property by using TokenHelper.
            // To access other properties, the app may need to request permissions on the host web.
            var spContext = SharePointContextProvider.Current.GetSharePointContext(Context);

            using (var clientContext = spContext.CreateUserClientContextForSPAppWeb())
            {
                GetEvents(clientContext);
                if (!IsPostBack)
                {
                    GetDepartments(clientContext);
                }
                /*string sqlConnectionString = Util.CreateConnectionString(clientContext);
                if (!string.IsNullOrEmpty(sqlConnectionString))
                {
                    try
                    {
                        using (SqlConnection conn = new SqlConnection(sqlConnectionString + "Async=True;"))
                        {
                            conn.Open();
                            string sqlQuery = Util.GetQuery(clientContext, "Default SQL Query");

                            if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue))
                            {
                                sqlQuery += string.Format(" WHERE Department = '{0}'", ddlDepartment.SelectedValue);
                            }
                            if (!string.IsNullOrEmpty(sqlQuery))
                            {
                                SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                                using (SqlDataReader reader = cmd.ExecuteReader())
                                {
                                    IEnumerable<Dictionary<string, object>> r = Util.Serialize(reader);
                                    string json = JsonConvert.SerializeObject(r, Formatting.Indented);
                                    eventsJsonArray.Value = json;
                                }
                            }

                            if (!IsPostBack)
                            {
                                sqlQuery = null;
                                sqlQuery = Util.GetQuery(clientContext, "Department Listing Query");

                                if (!string.IsNullOrEmpty(sqlQuery))
                                {
                                    SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                                    using (SqlDataReader reader = cmd.ExecuteReader())
                                    {
                                        ddlDepartment.Items.Clear();
                                        while (reader.Read())
                                        {
                                            System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(reader["Department"].ToString(), reader["Department"].ToString());
                                            ddlDepartment.Items.Add(item);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Util.WriteError(Response, "There was an error connecting to the SQL database. Please check the App's config. Message details: " + ex.Message);
                    }
                }*/
            }
        }

        private void GetEvents(ClientContext clientContext)
        {
            string sqlConnectionString = Util.CreateConnectionString(clientContext);
            if (!string.IsNullOrEmpty(sqlConnectionString))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(sqlConnectionString + "Async=True;"))
                    {
                        conn.Open();
                        string sqlQuery = Util.GetQuery(clientContext, "Default SQL Query");

                        if (!string.IsNullOrEmpty(ddlDepartment.SelectedValue))
                        {
                            sqlQuery += string.Format(" WHERE Department = '{0}'", ddlDepartment.SelectedValue);
                        }
                        if (!string.IsNullOrEmpty(sqlQuery))
                        {
                            SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                IEnumerable<Dictionary<string, object>> r = Util.Serialize(reader);
                                string json = JsonConvert.SerializeObject(r, Formatting.Indented);
                                eventsJsonArray.Value = json;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Util.WriteError(Response, "There was an error connecting to the SQL database. Please check the App's config. Message details: " + ex.Message);
                }
            }
            else
            {
                Util.WriteError(Response, "There was an error connecting to the SQL database. Please check the App's config.");
            }
        }

        private void GetDepartments(ClientContext clientContext)
        {
            string sqlConnectionString = Util.CreateConnectionString(clientContext);
            if (!string.IsNullOrEmpty(sqlConnectionString))
            {
                try
                {
                    using (SqlConnection conn = new SqlConnection(sqlConnectionString + "Async=True;"))
                    {
                        conn.Open();
                        string sqlQuery = Util.GetQuery(clientContext, "Department Listing Query");
                        if (!string.IsNullOrEmpty(sqlQuery))
                        {
                            SqlCommand cmd = new SqlCommand(sqlQuery, conn);

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                ddlDepartment.Items.Clear();
                                while (reader.Read())
                                {
                                    System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem(reader["Department"].ToString(), reader["Department"].ToString());
                                    ddlDepartment.Items.Add(item);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Util.WriteError(Response, "There was an error connecting to the SQL database. Please check the App's config. Message details: " + ex.Message);
                }
            }
        }

        protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
    }
}