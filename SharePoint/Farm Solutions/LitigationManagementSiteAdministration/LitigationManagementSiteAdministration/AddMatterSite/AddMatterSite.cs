using Microsoft.SharePoint;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace LitigationManagementSiteAdministration.AddMatterSite
{
    [ToolboxItemAttribute(false)]
    public class AddMatterSite : WebPart, IWebEditable
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/LitigationManagementSiteAdministration/AddMatterSite/AddMatterSiteUserControl.ascx";
        private static string _parentWebUrl = SPContext.Current.Web.Url;
        private static string _templateSolutionName;
        private static string _mm_service_appName;
        private static string _mm_termset_groupName;

        [Personalizable(PersonalizationScope.Shared)]
        //[WebBrowsable(true)]
        //[Category("Configuration")]
        //[WebDisplayName("Create sub-sites below")]
        //[WebDescription("Enter the URL of the SharePoint location, beneath which new sub sites should be created.")]
        public string ParentWebUrl 
        {
            get { return _parentWebUrl; }
            set { _parentWebUrl = value; }
        }
        [Personalizable(PersonalizationScope.Shared)]
        //[WebBrowsable(true)]
        //[Category("Configuration")]
        //[WebDisplayName("Template Solution Name")]
        //[WebDescription("Enter the Name of the SharePoint Site Template Solution you wish to use to provision new sites.")]
        public string TemplateSolutionName
        {
            get { return _templateSolutionName; }
            set { _templateSolutionName = value; }
        }
        [Personalizable(PersonalizationScope.Shared)]
        //[WebBrowsable(true)]
        //[Category("Configuration")]
        //[WebDisplayName("Managed Metadata Service Application Name")]
        //[WebDescription("Enter the name of the Managed Metadata Service Application which houses Litigitation Matter Terms")]
        public string mm_service_appName
        {
            get { return _mm_service_appName; }
            set { _mm_service_appName = value; }
        }
        [Personalizable(PersonalizationScope.Shared)]
        //[WebBrowsable(true)]
        //[Category("Configuration")]
        //[WebDisplayName("Group name for Litigation Term Sets")]
        //[WebDescription("Enter the group name where Litigation Term Sets are stored.")]
        public string mm_termset_groupName
        {
            get { return _mm_termset_groupName; }
            set { _mm_termset_groupName = value; }
        }

        protected override void CreateChildControls()
        {
            //property binding.
            var templateWarning = new Label();
            if (string.IsNullOrWhiteSpace(TemplateSolutionName) || string.IsNullOrWhiteSpace(ParentWebUrl))
            {
                templateWarning.Text = "<span style='color:red;'>Warning: No Template and/or Parent Web has been specified.  Please review the webpart settings.</span>";
                Controls.Add(templateWarning);
            }
            else
            {
                base.CreateChildControls();
                AddMatterSiteUserControl uc = (AddMatterSiteUserControl)Page.LoadControl(_ascxPath);
                uc.ParentWebUrl = ParentWebUrl;
                uc.TemplateSolutionName = TemplateSolutionName;
                uc.mm_service_appName = mm_service_appName;
                uc.mm_termset_groupName = mm_termset_groupName;
                Controls.Add(uc);
            }
        }
        /// <summary>
        /// Creates custom editor parts here and assigns a unique id to each part
        /// </summary>
        /// <returns>All custom editor parts used by this web part</returns>
        EditorPartCollection IWebEditable.CreateEditorParts()
        {
            var editors = new List<EditorPart>();
            var editorPart = new customProperties();
            editorPart.ID = ID + "_editorPart";
            editors.Add(editorPart);

            return new EditorPartCollection(editors);
        }

        /// <summary>
        /// Returns parent web part to editor part
        /// </summary>
        object IWebEditable.WebBrowsableObject
        {
            get { return this; }
        }
    }
}