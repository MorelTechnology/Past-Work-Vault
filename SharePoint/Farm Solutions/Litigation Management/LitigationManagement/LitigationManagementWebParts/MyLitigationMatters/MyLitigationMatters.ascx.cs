using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration.Claims;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace LitigationManagementWebParts.MyLitigationMatters
{
    [ToolboxItemAttribute(false)]
    public partial class MyLitigationMatters : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public MyLitigationMatters()
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
                getMatters();
        }
        [WebBrowsable(true),
        WebDisplayName("Litigation Management Site URL"),
        WebDescription("Enter the URL of the root Litigation Management Site."),
        Personalizable(PersonalizationScope.Shared),
        Category("Source Settings")]
        public string _sourceSiteUrl { get; set; }

        private void getMatters()
        {
            try
            {
                using (SPWeb web = new SPSite(_sourceSiteUrl).OpenWeb())
                {
                    SPWebCollection allAccessibleMatters = web.GetSubwebsForCurrentUser();
                    foreach(SPWeb matter in allAccessibleMatters)
                    {
                        //TODO - Put all matters in one list, then use Datatables.net to sort them.
                        if (matter.AllProperties.ContainsKey("LMUserID"))
                        {
                            if (matter.AllProperties["LMUserID"].ToString() == getUserId())
                                addEntry(matter, true);
                            else
                                addEntry(matter, false);
                        }
                        else
                            addEntry(matter, false, false);
                    }
                }
            }
            catch (Exception ex) { HandleException(ex); }
        }

        private void addEntry(SPWeb matter, Boolean isMyMatter, Boolean isAssigned = true)
        {
            if(isMyMatter)
            {
                if(matter.AllProperties["isMatterActive"].ToString() == "true")
                MyMatters.Text +=
                    matter.Title + " " +
                    "<a href = '" + matter.Url + "'>Open</a>" +
                    "<br>";
            }
            else
            {
                //if(matter.AllProperties["isMatterActive"].ToString() == "true")
                NotMyMatters.Text +=
                    matter.Title + " ";
                    if (isAssigned)
                    {
                        try { NotMyMatters.Text += ("<b>Assigned to:</b> " + matter.AllProperties["Litigation_Manager"].ToString() + " "); }
                        catch { NotMyMatters.Text += "<b>Assigned to:</b> Unknown "; }
                    } 
                    else NotMyMatters.Text += "<b>Assigned to:</b> Unknown ";
                    NotMyMatters.Text += "<a href = '" + matter.Url + "'>Open</a>" +
                    "<br>";
            }
        }
        private string getUserId()
        {
            string userId = string.Empty;
            try
            {
                SPClaimProviderManager mgr = SPClaimProviderManager.Local;
                if(mgr !=null)
                {
                    userId = mgr.DecodeClaim(SPContext.Current.Web.CurrentUser.LoginName).Value;
                }
            }
            catch (Exception ex) { HandleException(ex); }
            return userId.Substring(userId.IndexOf("\\") + 1);
        }


        private void HandleException(Exception ex)
        {
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }

    }
}
