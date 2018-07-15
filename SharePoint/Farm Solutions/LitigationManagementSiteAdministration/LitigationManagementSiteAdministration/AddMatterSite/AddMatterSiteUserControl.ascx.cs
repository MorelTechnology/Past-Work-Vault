using MatterProvisioningLibrary;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using Microsoft.SharePoint.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LitigationManagementSiteAdministration.AddMatterSite
{
    public partial class AddMatterSiteUserControl : UserControl
    {
        public string mm_service_appName
        {
            get { return _mm_service_appName; }
            set { _mm_service_appName = value; }
        }
        public string mm_termset_groupName
        {
            get { return _mm_termset_groupName; }
            set { _mm_termset_groupName = value; }
        }
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

        private static string _mm_service_appName;
        private static string _mm_termset_groupName;
        private static string _parentWebUrl;
        private static string _templateSolutionName;
        private MatterProvisioning provisioning = new MatterProvisioning(_parentWebUrl);

        protected void Page_Load(object sender, EventArgs e)
        {
            validateSettings();
        }
        protected void ppAdditionalContributors_Init(object sender, EventArgs e)
        {
            ppAdditionalContributors.Required = false;
            ppAdditionalContributors.ValidationEnabled = true;
            ppAdditionalContributors.VisibleSuggestions = 3;
            ppAdditionalContributors.Rows = 3;
            ppAdditionalContributors.AllowMultipleEntities = true;
            ppAdditionalContributors.PrincipalAccountType = "User,SPGroup";
            ppAdditionalContributors.CssClass = "ms-input ms-long ms-spellcheck-true";
        }
        protected void ppLitigationManager_Init(object sender, EventArgs e)
        {
            ppLitigationManager.Required = true;
            ppLitigationManager.ValidationEnabled = true;
            ppLitigationManager.VisibleSuggestions = 3;
            ppLitigationManager.Rows = 1;
            ppLitigationManager.AllowMultipleEntities = false;
            ppLitigationManager.PrincipalAccountType = "User";
            ppLitigationManager.CssClass = "ms-input ms-long ms-spellcheck-true";
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

                    Matter matter = new Matter();

                    SPSecurity.RunWithElevatedPrivileges(delegate ()
                    {
                        try
                        {
                            bool siteCreated = false;
                            matter.LMNumber = Utility.generateSiteId(_parentWebUrl);
                            matter.MatterName = txtMatterName.Text;
                            matter.AccountName = txtAccountName.Text;
                            matter.Affiliate = GetMMValue(mmAffiliate);
                            matter.CaseCaption = txtCaseCaption.Text;
                            matter.Country = txtCountry.Text;
                            matter.DocketNumber = txtDocketNumber.Text;
                            matter.LitigationManagerSPUser = Utility.retrieveUsersFromPeoplePicker(ppLitigationManager, _parentWebUrl).FirstOrDefault();
                            matter.LitigationManagerName = matter.LitigationManagerSPUser.Name;
                            matter.LitigationManagerUserId = matter.LitigationManagerSPUser.LoginName.ToString().Split('\\')[1].ToUpper();
                            matter.LitigationType = GetMMValue(mmLitigationType);
                            matter.MatterStatus = ddStatus.Text.ToString(); 
                            matter.StateFiled = GetMMValue(mmStateFiled);
                            matter.Venue = GetMMValue(mmVenue);
                            matter.WorkMatterType = GetMMValue(mmWorkMatterType);
                            List<SPUser> additionalContributors = Utility.retrieveUsersFromPeoplePicker(ppAdditionalContributors, _parentWebUrl);
                            
#if DEBUG
                            matter.LMNumber = "test"; siteCreated = true;
#endif

#if !DEBUG
                    SPUtility.ValidateFormDigest();
                    using (var web = new SPSite(_parentWebUrl).OpenWeb())
                    {
                        try
                        {
                            web.AllowUnsafeUpdates = true;
                                    web.Webs.Add(
                            siteCreated = provisioning.createSiteFromTemplateSolution(matter, _templateSolutionName);
                            web.AllowUnsafeUpdates = false;
                        }
                        catch (Exception ex)
                        { throw new SPException("Your request to create Matter Site '" + txtMatterName.Text.ToString() + "' could not be completed.", ex); }
                    }
#endif
                            if (siteCreated)
                            {
                                using (var web = new SPSite(_parentWebUrl + "/" + matter.LMNumber).OpenWeb())
                                {
                                    siteIsProvisioning.LeadingHTML = "Setting site properties...";
                                    SPUtility.ValidateFormDigest();
                                    web.AllowUnsafeUpdates = true;
                                    web.AllProperties["Site_Created"] = DateTime.Now.ToString("g");
                                    web.AllProperties["Matter_Number"] = matter.LMNumber;
                                    web.AllProperties["Affiliate"] = matter.Affiliate;
                                    web.AllProperties["Case_Caption"] = matter.CaseCaption;
                                    web.AllProperties["Matter_Name"] = matter.MatterName;
                                    web.AllProperties["Account_Name"] = matter.AccountName;
                                    web.AllProperties["Litigation_Manager"] = matter.LitigationManagerName;
                                    web.AllProperties["LMUserID"] = matter.LitigationManagerUserId;
                                    web.AllProperties["Matter_Status"] = matter.MatterStatus;
                                    web.AllProperties["Docket_Number"] = matter.DocketNumber;
                                    web.AllProperties["Litigation_Type"] = matter.LitigationType;
                                    web.AllProperties["State_Filed"] = matter.StateFiled;
                                    web.AllProperties["Venue"] = matter.Venue;
                                    web.AllProperties["Country"] = matter.Country;
                                    web.AllProperties["Work_Matter_Type"] = matter.WorkMatterType;

                                    // provisioning.updateProperties(matter);  //not working?
                                    web.Update();
                                    siteIsProvisioning.LeadingHTML = "Assigning users to site...";
                                    provisioning.setSiteSecurity(matter,
                                                            "Litigation Management Owners",
                                                            "Site Manager",
                                                            "Read Only Users",
                                                            "Additional Contributors");

                                    if (additionalContributors.Count > 0)
                                    {
                                        SPUtility.ValidateFormDigest();
                                        SPGroup grpAdditionalContributors = web.SiteGroups[matter.LMNumber + " - Additional Contributors"];
                                        foreach (SPUser user in additionalContributors) { grpAdditionalContributors.AddUser(user); }
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
                    siteIsProvisioning.End(_parentWebUrl + "/" + matter.LMNumber);
                }
        }
        protected void btnReset_Click(object sender, EventArgs e)
        {
            Utility.Clear(Controls);
        }
        protected string GetMMValue(TaxonomyWebTaggingControl tc)
        {
            StringBuilder mm = new StringBuilder();
            try
            {
                var values = new TaxonomyFieldValueCollection(string.Empty);
                values.PopulateFromLabelGuidPairs(tc.Text);
                foreach (TaxonomyFieldValue value in values)
                {
                    if (!string.IsNullOrEmpty(mm.ToString())) mm.Append("; ");
                    mm.Append(value.Label);
                }
            }
            catch (Exception ex)
            {
                Utility.HandleException(ex, Controls);
            }
            return mm.ToString();
        }
        protected void TaxonomyControl_Init(object sender, EventArgs e)
        {
            TaxonomySession taxonomySession = new TaxonomySession(SPContext.Current.Site);
            TermStore store = taxonomySession.TermStores[mm_service_appName];
            Group group = store.Groups[mm_termset_groupName];

            var tc = sender as TaxonomyWebTaggingControl;
            tc.SspId.Add(store.Id);
            tc.SSPList = store.Id.ToString();
            tc.TermSetId.Add(group.TermSets[tc.FieldId].Id);
            tc.TermSetList = group.TermSets[tc.FieldId].Id.ToString();

            tc.Visible = true;
            tc.IsDisplayPickerButton = true;
            tc.AllowFillIn = false;
            tc.IsAddTerms = false;
            tc.IsIncludePathData = false;
        }
        protected void TaxonomyControl_Validate(object sender, System.Web.UI.WebControls.ServerValidateEventArgs args)
        {
            var validator = sender as CustomValidator;
            var tc = FindControl(validator.ID.ToString().Split('_')[0]) as TaxonomyWebTaggingControl;
            string reason;
            if (!tc.Validate(out reason))
            {
                args.IsValid = false;
                validator.ErrorMessage = reason;
                return;
            }
            
            if (string.IsNullOrWhiteSpace(tc.Text)) 
            {
                args.IsValid = false;
                validator.ErrorMessage = "You must specify a value.";
            }
              
        }
        protected void validateSettings()
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
