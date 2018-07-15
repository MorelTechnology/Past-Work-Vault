//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Web.UI.WebControls.WebParts;
//using System.Web.UI.WebControls;
//using System.Web.UI;
//using Microsoft.SharePoint;

//namespace LitigationManagementSiteAdministration.AddProjectSite
//{

//public class AddProjectSiteEditorPart : EditorPart
//    {
//        // Reference to the web part that uses this editor part, the parent web part needs to implement the IWebEditable interface to support custom editing controls
//        protected AddProjectSite ParentWebPart { get; set; }
//        protected DropDownList ddlTemplate { get; set; }
//        protected TextBox txtCreateBelow { get; set; }

//        /// <summary>
//        /// Does editor part init settings
//        /// </summary>
//        public AddProjectSiteEditorPart()
//        {
//            Title = "Configuration Details";
//            Description = "Some Description";
           
//        }

//        protected override void CreateChildControls()
//        {
            
//            // Web Part Property Controls
//            Label lblTemplate = new Label();
//            lblTemplate.CssClass = "trgWpProperty";
//            lblTemplate.Text  = "Create new sites using:";
//            Controls.Add(lblTemplate);

//            ddlTemplate = new DropDownList();
//            ddlTemplate.CssClass = "trgWpProperty";
//            ddlTemplate.DataSource = SPContext.Current.Site.RootWeb.GetAvailableWebTemplates(1033);
//            ddlTemplate.DataTextField = "Title";
//            ddlTemplate.DataValueField = "Name";
//            ddlTemplate.DataBind();
//            ddlTemplate.Items.Insert(0, new ListItem("Select a template...", String.Empty));            
//            Controls.Add(ddlTemplate);

//            Label lblCreateBelow = new Label();
//            lblCreateBelow.CssClass = "trgWpProperty";
//            lblCreateBelow.Text = "Parent Site Url:";
//            Controls.Add(lblCreateBelow);

//            txtCreateBelow = new TextBox();
//            txtCreateBelow.CssClass = "trgWpProperty";
//            Controls.Add(txtCreateBelow);

//            Controls.Add(new LiteralControl(
//                "<style>.trgWpProperty{ display:block; margin: 2.5px 0px 2.5px 0px; width: 95%;}</style>"));
//            base.CreateChildControls();
//            ChildControlsCreated = true;
//        }

//        /// <summary>
//        /// Reads current value from parent web part sets that in fields
//        /// </summary>
//        public override void SyncChanges()
//        {
//            EnsureChildControls();
//            ParentWebPart = WebPartToEdit as AddProjectSite;

//            if (ParentWebPart != null && WebPartManager.Personalization.Scope == PersonalizationScope.Shared)
//            {
//                ListItem item = ddlTemplate.Items.FindByValue(ParentWebPart.TemplateSolutionName);
//                if (item != null) item.Selected = true;

//                txtCreateBelow.Text = ParentWebPart.ParentWebUrl;
//            }
//        }

//        /// <summary>
//        /// Applies change in editor part to the parent web part
//        /// </summary>
//        /// <returns></returns>
//        public override bool ApplyChanges()
//        {
//            try
//            {
//                EnsureChildControls();
//                ParentWebPart = WebPartToEdit as AddProjectSite;

//                if (ParentWebPart != null && WebPartManager.Personalization.Scope == PersonalizationScope.Shared)
//                {
//                    ParentWebPart.TemplateSolutionName = ddlTemplate.SelectedValue;
//                    ParentWebPart.ParentWebUrl = txtCreateBelow.Text;
//                }

//                // The operation was succesful
//                return true;
//            }
//            catch
//            {
//                // Because an error has occurred, the SyncChanges() method won’t be invoked.
//                return false;
//            }
//        }
//    }
//}
