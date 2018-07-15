using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI;
using WorkRequestDataService.Models;
using WorkRequestDataService.Services;
using static WorkRequestDataService.Services.Utility;

namespace WorkRequestDataService.Admin
{
#pragma warning disable 1591

    [Authorize]
    public partial class EditSettings : System.Web.UI.Page
    {
        private ConfigurationService configurationService = new ConfigurationService();

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

        protected void listCorporateGoals_Init(object sender, EventArgs e)
        {
            listCorporateGoals.DataSource = configurationService.GetConfigurationProperty("Corporate_Goals");
            listCorporateGoals.DataBind();
        }

        /// <exclude />
        protected void btnAddCorporateGoal_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(txtCorporateGoals.Text))
                listCorporateGoals.Items.Add(escape(txtCorporateGoals.Text));
            txtCorporateGoals.Text = "";
            configurationService.SetConfig(new ConfigurationProperty { Key = "Corporate_Goals", Value = listCorporateGoals.Items });
        }

        /// <exclude />
        protected void btnDeleteCorporateGoal_Click(object sender, EventArgs e)
        {
            listCorporateGoals.Items.Remove(listCorporateGoals.SelectedItem);
            configurationService.SetConfig(new ConfigurationProperty { Key = "Corporate_Goals", Value = listCorporateGoals.Items });
        }
    }
}