using Microsoft.SharePoint;
using Microsoft.Web.Hosting.Administration;
using System;
using System.ComponentModel;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.UI.WebControls.WebParts;

namespace SharePointSiteCreator.SiteCreator
{
    [ToolboxItemAttribute(false)]
    public partial class SiteCreator : WebPart
    {
        public SiteCreator()
        {
        }
        public string MakeStringSPSafe(string name)
        {
            name = name.Replace(" ", "-"); // Replaces any spaces with a hyphen
            name = Regex.Replace(name, "[^a-zA-Z0-9\\-]", ""); //remove anything not alphanumeric
            return name;
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        void btnAddSite_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtNewSiteTitle.Text.ToString()))
            {
                //         SP.UI.Notify.addNotification('<img src="/layours/images/loadinccirclests16.gif" style="vertical-align:top;"/> Operation in progress...',true);
                try
                {
                    SPWeb thisSite = SPContext.Current.Web;
                    SPWebCollection subSites = thisSite.Webs;
                    string currentTemplate = thisSite.WebTemplate;
                    string user = thisSite.CurrentUser.Name.ToString();

                    string siteUrl = MakeStringSPSafe(txtNewSiteTitle.Text.ToString());
                    string siteTitle = txtNewSiteTitle.Text.ToString();
                    string siteDescription = "Site " + txtNewSiteTitle.Text.ToString() +
                        ", Created by " + user + " on " + DateTime.Today.ToString("MM/dd/yyyy");
                    subSites.Add(siteUrl, siteTitle, siteDescription, 1033,
                       currentTemplate, true, false);
                    string newSiteAbsoluteUrl = thisSite.Url + "/" + siteUrl;

                    // reset thisSite to scope of new site 
                    thisSite = subSites[siteUrl];
                    // Create a members group for the newly created site
                    string membersGroupName = MakeStringSPSafe(siteTitle) + " Members";
                    thisSite.SiteGroups.Add(membersGroupName, thisSite.CurrentUser, thisSite.CurrentUser, "Members of the site " + siteTitle);
                    thisSite.AssociatedGroups.Add(thisSite.SiteGroups[membersGroupName]);
                    thisSite.Update();
                    SPRoleAssignment assignment = new SPRoleAssignment(thisSite.SiteGroups[membersGroupName]);
                    SPRoleDefinition _role = thisSite.RoleDefinitions["Contribute"];
                    assignment.RoleDefinitionBindings.Add(_role);
                    thisSite.RoleAssignments.Add(assignment);
                    string membersGroupId = thisSite.SiteGroups[membersGroupName].ID.ToString();
                    string manageMembersUrl = newSiteAbsoluteUrl + "/_layouts/15/people.aspx?MembershipGroupId=" + membersGroupId;

                    lblResult.Text = "Site Successfully Created at <a href='" + newSiteAbsoluteUrl + "'>" + newSiteAbsoluteUrl + "</a>";
                    lblResult.Text = lblResult.Text + "<br><a href='" + manageMembersUrl + "'> Add Members to Site";
                }
                catch (Exception ex)
                {
                    lblResult.Text = "An Error Occured: " + ex.Message;
                }
                finally
                {
                    txtNewSiteTitle.Text = null;
                    PleaseWait.Visible = false;
                }

            }
        }
    }
}
