using Microsoft.SharePoint;
using Microsoft.SharePoint.Taxonomy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace LitigationManagementSiteAdministration.AddMatterSite
{
   internal class customProperties : EditorPart
    {
        // Reference to the web part that uses this editor part, the parent web part needs to implement the IWebEditable interface to support custom editing controls
        protected AddMatterSite ParentWebPart { get; set; }
        protected DropDownList ddl_Template { get; set; }
        protected TextBox txt_CreateBelow { get; set; }
        protected DropDownList ddl_MMApplication { get; set; }
        protected DropDownList ddl_MMGroupName { get; set; }

        /// <summary>
        /// Does editor part init settings
        /// </summary>
        public customProperties()
        {
            Title = "Configuration Details";
        }

        protected override void CreateChildControls()
        {
            // Web Part Property Controls
            System.Web.UI.WebControls.Label lbl_Template = new System.Web.UI.WebControls.Label();
            lbl_Template.CssClass = "trgWpProperty";
            lbl_Template.Text = "Create new sites using:";
            Controls.Add(lbl_Template);

            ddl_Template = new DropDownList();
            ddl_Template.CssClass = "trgWpProperty";
            ddl_Template.DataSource = SPContext.Current.Site.RootWeb.GetAvailableWebTemplates(1033);
            ddl_Template.DataTextField = "Title";
            ddl_Template.DataValueField = "Name";
            ddl_Template.DataBind();
            ddl_Template.Items.Insert(0, new ListItem("Select a template...", String.Empty));
            Controls.Add(ddl_Template);

            System.Web.UI.WebControls.Label lbl_CreateBelow = new System.Web.UI.WebControls.Label();
            lbl_CreateBelow.CssClass = "trgWpProperty";
            lbl_CreateBelow.Text = "Parent Site Url:";
            Controls.Add(lbl_CreateBelow);

            txt_CreateBelow = new TextBox();
            txt_CreateBelow.CssClass = "trgWpProperty";
            Controls.Add(txt_CreateBelow);

            System.Web.UI.WebControls.Label lbl_MMApplication = new System.Web.UI.WebControls.Label();
            lbl_MMApplication.CssClass = "trgWpProperty";
            lbl_MMApplication.Text = "Managed Metadata Service Name:";
            Controls.Add(lbl_MMApplication);

            ddl_MMApplication = new DropDownList();
            ddl_MMApplication.CssClass = "trgWpProperty";
            ddl_MMApplication.DataSource = new TaxonomySession(SPContext.Current.Site).TermStores;
            ddl_MMApplication.DataTextField = "Name";
            ddl_MMApplication.DataValueField = "Name";
            ddl_MMApplication.AutoPostBack = true; // This needs to happen so dependencies won't choke.
            ddl_MMApplication.DataBind();
            ddl_MMApplication.SelectedIndex = 0;
            Controls.Add(ddl_MMApplication);

            System.Web.UI.WebControls.Label lbl_MMGroupName = new System.Web.UI.WebControls.Label();
            lbl_MMGroupName.CssClass = "trgWpProperty";
            lbl_MMGroupName.Text = "Term Set Group:";
            Controls.Add(lbl_MMGroupName);

            ddl_MMGroupName = new DropDownList();
            ddl_MMGroupName.CssClass = "trgWpProperty";
            if (!String.IsNullOrEmpty(ddl_MMApplication.SelectedValue))
            {
                ddl_MMGroupName.DataSource = new TaxonomySession(SPContext.Current.Site).TermStores[ddl_MMApplication.SelectedValue].Groups;
                ddl_MMGroupName.DataTextField = "Name";
                ddl_MMGroupName.DataValueField = "Name";
                ddl_MMGroupName.DataBind();
                ddl_MMGroupName.Items.Insert(0, new ListItem("Select a Value...", String.Empty));
            }
            else
            {
                ddl_MMGroupName.Items.Insert(0, new ListItem("Select a value above and click apply...", String.Empty));
            }
            Controls.Add(ddl_MMGroupName);
            Controls.Add(new LiteralControl(
                "<style>.trgWpProperty{ display:block; margin: 2.5px 0px 2.5px 0px; width: 95%;}</style>"));
            base.CreateChildControls();
            ChildControlsCreated = true;
        }

        /// <summary>
        /// Reads current value from parent web part sets that in fields
        /// </summary>
        public override void SyncChanges()
        {
            EnsureChildControls();
            ParentWebPart = WebPartToEdit as AddMatterSite;

            if (ParentWebPart != null && WebPartManager.Personalization.Scope == PersonalizationScope.Shared)
            {
                ListItem templateItem = ddl_Template.Items.FindByValue(ParentWebPart.TemplateSolutionName);
                if (templateItem != null) templateItem.Selected = true;

                txt_CreateBelow.Text = ParentWebPart.ParentWebUrl;

                ListItem mmApplicationItem = ddl_MMApplication.Items.FindByValue(ParentWebPart.mm_service_appName);
                if (mmApplicationItem != null) mmApplicationItem.Selected = true;

                ListItem mmGroupItem = ddl_MMGroupName.Items.FindByValue(ParentWebPart.mm_termset_groupName);
                if (mmGroupItem != null) mmGroupItem.Selected = true;
            }
        }

        /// <summary>
        /// Applies change in editor part to the parent web part
        /// </summary>
        /// <returns></returns>
        public override bool ApplyChanges()
        {
            try
            {
                EnsureChildControls();
                ParentWebPart = WebPartToEdit as AddMatterSite;

                if (ParentWebPart != null && WebPartManager.Personalization.Scope == PersonalizationScope.Shared)
                {
                    ParentWebPart.TemplateSolutionName = ddl_Template.SelectedValue;
                    ParentWebPart.ParentWebUrl = txt_CreateBelow.Text;
                    ParentWebPart.mm_service_appName = ddl_MMApplication.SelectedValue;
                    ParentWebPart.mm_termset_groupName = ddl_MMGroupName.SelectedValue;
                }
                // The operation was succesful
                return true;
            }
            catch
            {
                // Because an error has occurred, the SyncChanges() method won’t be invoked.
                return false;
            }
        }

    }
}
