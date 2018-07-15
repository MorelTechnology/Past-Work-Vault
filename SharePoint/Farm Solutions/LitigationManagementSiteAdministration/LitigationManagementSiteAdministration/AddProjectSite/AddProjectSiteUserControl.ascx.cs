using MatterProvisioningLibrary;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace LitigationManagementSiteAdministration.AddProjectSite
{
    public partial class AddProjectSiteUserControl : UserControl
    {
        public string ParentWebUrl
        {
            get { return _parentWebUrl; }
            set { _parentWebUrl = value; }
        }
        public string TemplateSolutionName
        {
            get { return _templateSolutionName; }
            set { _templateSolutionName = value; }
        }

        private static string _parentWebUrl;
        private static string _templateSolutionName;
        private MatterProvisioning provisioning = new MatterProvisioning(_parentWebUrl);

        protected void Page_Load(object sender, EventArgs e)
        {
            validateSettings();
        }
        protected void ppAdditionalMembers_Init(object sender, EventArgs e)
        {
            ppAdditionalMembers.Required = false;
            ppAdditionalMembers.ValidationEnabled = true;
            ppAdditionalMembers.VisibleSuggestions = 3;
            ppAdditionalMembers.Rows = 3;
            ppAdditionalMembers.AllowMultipleEntities = true;
            ppAdditionalMembers.PrincipalAccountType = "User,SPGroup";
            ppAdditionalMembers.CssClass = "ms-input ms-long ms-spellcheck-true";
        }
        protected void ppProjectLead_Init(object sender, EventArgs e)
        {
            ppProjectLead.Required = true;
            ppProjectLead.ValidationEnabled = true;
            ppProjectLead.VisibleSuggestions = 3;
            ppProjectLead.Rows = 1;
            ppProjectLead.AllowMultipleEntities = false;
            ppProjectLead.PrincipalAccountType = "User";
            ppProjectLead.CssClass = "ms-input ms-long ms-spellcheck-true";
        }
        protected void btnCreate_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
                btnCreate.Text = "One Moment..."; 
                using (SPLongOperation siteIsProvisioning = new SPLongOperation(this.Page))
                {
                    siteIsProvisioning.LeadingHTML = "Provisioning your new site...";
                    siteIsProvisioning.TrailingHTML = "You will be directed to your site shortly.";
                    siteIsProvisioning.Begin();

                    Project project = new Project();

                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        try
                        {
                            bool siteCreated = false;
                            project.ProjectId = Utility.generateSiteId(_parentWebUrl);
                            project.ProjectName = txtProjectName.Text.ToString();
                            project.ProjectDescription = txtProjectDesc.Text.ToString();
                            project.ProjectLeadSPUser = Utility.retrieveUsersFromPeoplePicker(ppProjectLead, _parentWebUrl).FirstOrDefault();
                            project.ProjectLead = project.ProjectLeadSPUser.Name;
                            project.ProjectStatus = ddStatus.Text.ToString();
                            List<SPUser> additionalUsers = Utility.retrieveUsersFromPeoplePicker(ppAdditionalMembers, _parentWebUrl);
#if DEBUG
                    project.ProjectId = "test"; siteCreated = true;
#endif

#if !DEBUG
                    SPUtility.ValidateFormDigest();
                    using (var web = new SPSite(_parentWebUrl).OpenWeb())
                    {
                        try
                        {
                            web.AllowUnsafeUpdates = true;
                            siteCreated = provisioning.createSiteFromTemplateSolution(project, _templateSolutionName);
                            web.AllowUnsafeUpdates = false;
                        }
                        catch (Exception ex)
                        { throw new SPException("Your request to create a Project Site '" + txtProjectName.Text.ToString() + "' could not be completed.", ex); }
                    }
#endif
                    if (siteCreated)
                            {
                                using (var web = new SPSite(_parentWebUrl + "/" + project.ProjectId).OpenWeb())
                                {
                                    siteIsProvisioning.LeadingHTML = "Setting site properties...";
                                    SPUtility.ValidateFormDigest();
                                    web.AllowUnsafeUpdates = true;
                                    web.AllProperties["Site_Created"] = DateTime.Now.ToString("g");
                                    web.AllProperties["Project_ID"] = project.ProjectId;
                                    web.AllProperties["Project_Name"] = project.ProjectName;
                                    web.AllProperties["Project_Description"] = project.ProjectDescription;
                                    web.AllProperties["Project_Status"] = project.ProjectStatus;
                                    web.AllProperties["Project_Lead"] = project.ProjectLead;

                                    //provisioning.updateProperties(project);  //not working?
                                    web.Update();
                                    siteIsProvisioning.LeadingHTML = "Assigning users to site...";
                                    provisioning.setSiteSecurity(project,
                                                            "Litigation Management Owners",
                                                            "Site Manager",
                                                            "Read Only Users",
                                                            "Additional Contributors");

                                    if (additionalUsers.Count > 0)
                                    {
                                        SPUtility.ValidateFormDigest();
                                        SPGroup grpAdditionalUsers = web.SiteGroups[project.ProjectId + " - Additional Contributors"];
                                        foreach (SPUser user in additionalUsers) { grpAdditionalUsers.AddUser(user); }
                                        web.AllowUnsafeUpdates = false;
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Utility.HandleException(ex, Controls);
                        }
                    });
                    siteIsProvisioning.End(_parentWebUrl + "/" + project.ProjectId);
                }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Utility.Clear(Controls);
        }
        private void validateSettings()
        {
            try
            {
                using (var web = new SPSite(_parentWebUrl).OpenWeb())
                {
                    if (!web.Exists || String.IsNullOrWhiteSpace(_templateSolutionName))
                    {
                        throw new SPException();
                    }
                }
            }
            catch (Exception ex)
            {
                SPException mergedException = new SPException("Problem with webpart settings. Verify that you have specified a valid URL and Template Solution Name.", ex);
                Utility.HandleException(mergedException, Controls);
            }
        }
    }
}
