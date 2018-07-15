using System;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;
using WorkRequestDataService.Models;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Admin
{
    /// <summary>
    /// Internal Page Methods
    /// </summary>
    /// <seealso cref="System.Web.UI.Page" />
    [Authorize]
    public partial class EditDenials : System.Web.UI.Page
    {
        #region Protected Methods

        /// <exclude />
        protected void btnReactivate_Click(object sender, EventArgs e)
        {
            WorkRequestService workRequestService = new WorkRequestService();
            NotificationService notificationService = new NotificationService();
            Status.Visible = true;
            int id = Int32.Parse(((Button)sender).CommandArgument);
            WorkRequest workRequest = workRequestService.GetWorkRequests(new Models.WorkRequest { RequestID = id }).First();
            if (workRequest.RequestID == id)
                workRequest.Status = WorkRequestStatus.SubmittedToPortfolioManager.Description();
            workRequestService.UpdateWorkRequest(workRequest, false);
            Notification notify = new Notification
            {
                WorkRequestId = id,
                Template = Template.Restored
            };
            notify.NotifyUser = workRequest.Requestor;  // email requestor
            notificationService.Send(notify);
            notify.NotifyUser = workRequest.Manager; // email manager
            notificationService.Send(notify);

            // refresh the page to show the result
            Response.Redirect("/Admin");
        }

        /// <exclude />
        protected void btnViewDetails_Click(object sender, EventArgs e)
        {
            string id = ((Button)sender).CommandArgument;
            string url = String.Format(new ConfigurationService().GetConfigurationProperty<string>("Single_Request_Url"), id);

            Response.Redirect(url);
        }

        /// <exclude />
        protected void DeniedRequests_Init(object sender, EventArgs e)
        {
            WorkRequestService workRequestService = new WorkRequestService();
            UserService userService = new UserService();

            // Get Data
            var source = workRequestService.GetWorkRequests(new Models.WorkRequest { Status = "Denied" });
            DataTable dt = Utility.ArrayToDataTable(source);

            //Trim and manipulate data to a new table
            DataTable results = new DataTable();
            results.Columns.Add("RequestID");
            results.Columns.Add("Title");
            results.Columns.Add("Requestor");
            results.Columns.Add("Manager");
            results.Columns.Add("LastModified");
            foreach (DataRow row in dt.Rows)
            {
                results.Rows.Add(row["RequestID"],
                                  row["Title"],
                                  userService.GetUserInfo(row["Requestor"] as string)["displayName"],
                                  userService.GetUserInfo(row["Manager"] as string)["displayName"],
                                  DateTime.Parse(row["LastModified"] as string).ToString("g"));
            }

            DeniedRequests.DataSource = results;
            DeniedRequests.DataBind();
        }

        /// <exclude />
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    if (!new UserService().CurrentUserIsConfigurationAdmin())
        //    {
        //        var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        //        { ReasonPhrase = "User '" + Page.User.Identity.Name + "' is not an admin." };
        //        var ex = new HttpResponseException(msg);

        //        Models.Error.Log(ex, ex.Response.ReasonPhrase);
        //        Response.Write("Message: " + ex.Message + "<br>Reason: " + ex.Response.ReasonPhrase);
        //        Response.End();
        //    }
        //}

        #endregion Protected Methods
    }
}