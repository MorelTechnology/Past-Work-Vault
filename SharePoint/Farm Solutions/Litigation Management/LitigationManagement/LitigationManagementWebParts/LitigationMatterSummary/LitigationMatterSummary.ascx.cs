using Microsoft.SharePoint;
using System;
using System.ComponentModel;
using System.Data;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

namespace LitigationManagementWebParts.LitigationMatterSummary
{
    [ToolboxItemAttribute(false)]
    public partial class LitigationMatterSummary : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public LitigationMatterSummary()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            EnsureChildControls();
            if (!this.Page.IsPostBack)
            {
                RenderContents();
            }
        }

        protected void RenderContents()
        {
            getSummary();
        }

        protected void getSummary()
        {
            string[] properties = _props.Split(',');
            string[] titles = _headers.Split(',');
            DataTable table = new DataTable();

            try
            {
                if (properties.Length != titles.Length)
                    throw new AmbiguousMatchException("The number of properties and titles are not equal. Unsure what to display!");
            }
            catch(Exception ex) { HandleException(ex); }

            try
            {
                DataRow row = table.NewRow();
                int count = 0;
                foreach(string title in titles)
                {
                    table.Columns.Add(title, typeof(string));
                    row[title] = SPContext.Current.Web.AllProperties[properties[count]];
                    count++;
                }
                table.Rows.Add(row);
                Summary.DataSource = table;
                Summary.DataBind();
                if (SPContext.Current.Web.AllProperties["isLinkedMatter"].ToString().ToLower() !="true")
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    editLink.Text = "<a href = 'javascript:window.spPropertyBag.init(" + js.Serialize(properties) + ");'>Edit properties</a>";
                }
            }
            catch (Exception ex) { HandleException(ex); }
        }

        [WebBrowsable(true),
        WebDisplayName("Internal Properties (Comma Separated)"),
        WebDescription("Enter the names of the internal site properties to display, in order you wish for them to be displayed."),
        Personalizable(PersonalizationScope.Shared),
        Category("Display Fields")]
        public string _props { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Display Titles (Comma Separated)"),
        WebDescription("Enter Titles you wish to display above each field, in the same order as the properties specified above."),
        Personalizable(PersonalizationScope.Shared),
        Category("Display Fields")]
        public string _headers { get; set; }

        private void HandleException(Exception ex)
        {
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }

    }
}
