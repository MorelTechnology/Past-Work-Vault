using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI;
using WorkRequestDataService.Models;
using WorkRequestDataService.Services;

namespace WorkRequestDataService
{
#pragma warning disable 1591

    [Authorize]
    public partial class EditConfig : System.Web.UI.Page
    {
        private ConfigurationService configurationService = new ConfigurationService();

        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    if (!new UserService().CurrentUserIsEnvironmentAdmin())
        //    {
        //        var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        //        { ReasonPhrase = "User '" + Page.User.Identity.Name + "' is not a super-admin." };
        //        var ex = new HttpResponseException(msg);

        //        Models.Error.Log(ex, ex.Response.ReasonPhrase);
        //        Response.Write("Message: " + ex.Message + "<br>Reason: " + ex.Response.ReasonPhrase);
        //        Response.End();
        //    }
        //}

        //protected void btnAddGroup_Click(object sender, EventArgs e)
        //{
        //    listAllowedUserGroups.Items.Add(txtAllowedUserGroup.Text);
        //    txtAllowedUserGroup.Text = null;
        //}

        ///// <exclude />
        //protected void btnDeleteGroup_Click(object sender, EventArgs e)
        //{
        //    listAllowedUserGroups.Items.Remove(listAllowedUserGroups.SelectedItem);
        //}

        ///// <exclude />
        //protected void btnAddQA_Click(object sender, EventArgs e)
        //{
        //    listQA.Items.Add(txtAllowedQA.Text);
        //    txtAllowedQA.Text = null;
        //}

        ///// <exclude />
        //protected void btnDeleteQA_Click(object sender, EventArgs e)
        //{
        //    listQA.Items.Remove(listQA.SelectedItem);
        //}

        /// <exclude />
        //protected void listAllowedUserGroups_Init(object sender, EventArgs e)
        //{
        //    listAllowedUserGroups.DataSource = configurationService.GetConfigurationProperty<string[]>("Allowed_User_Groups");
        //    listAllowedUserGroups.DataBind();
        //}

        ///// <exclude />
        //protected void listQA_Init(object sender, EventArgs e)
        //{
        //    listQA.DataSource = configurationService.GetConfigurationProperty<string[]>("Allowed_QA_Users");
        //    listQA.DataBind();
        //}

        /// <exclude />
        protected void txtSMTPServer_Init(object sender, EventArgs e)
        {
            txtSMTPServer.Text = configurationService.GetConfigurationProperty<string>("SMTP_Server");
        }

        /// <exclude />
        protected void txtSMTPPort_Init(object sender, EventArgs e)
        {
            txtSMTPPort.Text = configurationService.GetConfigurationProperty("SMTP_Port").ToString();
        }

        /// <exclude />
        protected void Save_Click(object sender, EventArgs e)
        {
            configurationService.SetConfig(new ConfigurationProperty { Key = "SMTP_Server", Value = txtSMTPServer.Text });
            configurationService.SetConfig(new ConfigurationProperty { Key = "SMTP_Port", Value = Int32.Parse(txtSMTPPort.Text) });

            statusMessage.Text = "Configuration Updated!";
            statusMessage.Visible = true;
        }
    }
}