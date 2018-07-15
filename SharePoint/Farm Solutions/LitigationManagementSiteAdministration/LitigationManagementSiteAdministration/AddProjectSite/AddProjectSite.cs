using Microsoft.SharePoint;
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace LitigationManagementSiteAdministration.AddProjectSite
{
    [ToolboxItemAttribute(false)]
    public class AddProjectSite : WebPart, IWebEditable
    {
        // Visual Studio might automatically update this path when you change the Visual Web Part project item.
        private const string _ascxPath = @"~/_CONTROLTEMPLATES/15/LitigationManagementSiteAdministration/AddProjectSite/AddProjectSiteUserControl.ascx";

        private static string _parentWebUrl; 
        private static string _templateSolutionName;

        [Personalizable(PersonalizationScope.Shared)]
        public string ParentWebUrl
        {
            get { return _parentWebUrl; }
            set { _parentWebUrl = value; }
        }
        [Personalizable(PersonalizationScope.Shared)]
        public string TemplateSolutionName
        {
            get { return _templateSolutionName; }
            set { _templateSolutionName = value; }
        }

        protected override void CreateChildControls()
        {
            //property binding.
            var templateWarning = new Label();
            if ( string.IsNullOrWhiteSpace(TemplateSolutionName) || string.IsNullOrWhiteSpace(ParentWebUrl) )
            {
                templateWarning.Text = "<span style='color:red;'>Warning: No Template and/or Parent Web has been specified.  Please review the webpart settings.</span>";
                Controls.Add(templateWarning);
            }
            else
            {
                base.CreateChildControls();
                AddProjectSiteUserControl uc = (AddProjectSiteUserControl)Page.LoadControl(_ascxPath);
                uc.ParentWebUrl = ParentWebUrl;
                uc.TemplateSolutionName = TemplateSolutionName;
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
