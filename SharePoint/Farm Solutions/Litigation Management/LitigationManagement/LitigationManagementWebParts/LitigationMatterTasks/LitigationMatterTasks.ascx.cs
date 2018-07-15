using HtmlAgilityPack;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;

namespace LitigationManagementWebParts.LitigationMatterTasks
{

    [ToolboxItemAttribute(false)]
    public partial class LitigationMatterTasks : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public LitigationMatterTasks()
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
                getTasks();
        }

        private void getTasks()
        {
            string html = null;
            bool tryAgain = true;
            while (tryAgain)
            {

                try
                {
                    using (SPSite site = new SPSite(_sourceSiteUrl))
                    {
                        using (SPWeb web = site.OpenWeb())
                        {
                            SPList list = web.Lists[_globalTasksListName];
                            SPQuery view = new SPQuery(list.Views[_listViewTitle]);
                            if (!string.IsNullOrEmpty(_advancedCaml))
                            {
                                view.Query = _advancedCaml;
                            }
                            else
                            {
                                view.Query = "<Where><Eq><FieldRef Name='" + XmlConvert.EncodeName("Associated Site ID") + "' /><Value Type='Text'>" + 
                                    SPContext.Current.Web.AllProperties["Matter_Number"] +"</Value></Eq></Where>";
                            }

                            html = "<div id='" + ClientID + "'> ";
                            html += list.RenderAsHtml(view);
                            html += "</div>";
                            // fix any malformed hyperlinks possibly contained in the view
                            html = html.Replace("&lt;", "<").Replace("&gt;", ">").Replace("&#39;", "'");
                            tryAgain = false;
                        }
                    }
                    if (_linksInNewWindow) html = setLinkTarget(html, "_blank");
                    htmlTasks.Text = html;
                }

                catch (Exception ex) { HandleException(ex); }
            }
            if (_displayOnlyData)
            {
                string thisWebPart = "#" + ClientID;
                string styleTag = "<style>" + thisWebPart + " .ms-viewheadertr,.ms-vb-firstCell{display:none;} " +
                thisWebPart + " .ms-listviewtable .ms-itmhover {height: 0px!important;} " +
                thisWebPart + " .ms-listviewtable {width: auto; margin: 5px;} " +
                thisWebPart + " .ms-vb2 img {display: block; margin: auto;} " +
                //Move Webpart title out of viewport, but don't hide it, since other page elements need to interact with it.
                "#WebPartTitle" + ClientID + " .ms-webpart-titleText { position: absolute!important; top: -9999px; left: -9999px;} " + 
                "</style>";
                css.Text = styleTag;
            }

            if (_showRefreshLink) { refreshLink.Text = "<a href = '#'>Refresh</a><br>"; }
            if (_usersCanAddTasks) { addTask.Text="<br><a href = '#'>Add a new task</a>"; }
        }
        private static string setLinkTarget(string html, string target)
        {
            var htmlContent = new HtmlDocument();
            htmlContent.LoadHtml(html);
            var links = htmlContent.DocumentNode.SelectNodes("//a");
            foreach (var link in links)
            {
                if (link.Attributes["href"].Value.ToLower().Contains("javascript:"))
                {
                    // Javascript links should always stay in _self
                    link.SetAttributeValue("target", "_self");
                }
                else
                { link.SetAttributeValue("target", target); }
            }
            return htmlContent.DocumentNode.OuterHtml;
        }
        private void HandleException(Exception ex)
        {
            this.Controls.Clear();
            this.Controls.Add(new LiteralControl(ex.Message));
        }



        [WebBrowsable(true),
        WebDisplayName("Source Site URL"),
        WebDescription("Enter the URL of the site which contains the Global Task list."),
        Personalizable(PersonalizationScope.Shared),
        Category("Source Settings")]
        public string _sourceSiteUrl { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Global Task List Name"),
        WebDescription("Enter the name of the list or library which contains data that you wish to import. (Case Sensitive)"),
        Personalizable(PersonalizationScope.Shared),
        Category("Source Settings")]
        public string _globalTasksListName { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Title of View to Display (Case Sensitive)"),
        WebDescription("Enter the name of an existing view in the Global Tasks list."),
        Personalizable(PersonalizationScope.Shared),
        Category("Source Settings")]
        public string _listViewTitle { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Advanced Query"),
        WebDescription("Results returned by this webpart are filtered based on the Litigaton " + 
            "Matter number stored in this site's properties.  You can override the results returned " +
            "By supplying an advanced CAML query here."),
        Personalizable(PersonalizationScope.Shared),
        Category("Advanced Rendering Settings")]
        public string _advancedCaml { get; set; }
        
        private Boolean _lnkNW = false;
        [WebBrowsable(true),
         WebDisplayName("Force all links to open in new tab/window."),
         WebDescription("This option will modify any links in the data to open " +
            "in a new tab or window, based on browser setting.  (This excludes 'javascript:' links.)"),
         Personalizable(PersonalizationScope.Shared),
         Category("Advanced Rendering Settings")]
        public Boolean _linksInNewWindow
        {
            get { return _lnkNW; }
            set { _lnkNW = value; }
        }

        private Boolean _dataOnly = false;
        [WebBrowsable(true),
         WebDisplayName("Display only data"),
         WebDescription("This option will modify the view display to show " +
            "only the view data, suppressing webpart title (regardless of chrome setting), " +
            "column headers, item selection checkboxes and " +
            "any extra whitespace padding between entries."),
         Personalizable(PersonalizationScope.Shared),
         Category("Advanced Rendering Settings")]
        public Boolean _displayOnlyData
        {
            get { return _dataOnly; }
            set { _dataOnly = value; }
        }

        private Boolean _addTasks = false;
        [WebBrowsable(true),
         WebDisplayName("Users Can Add Tasks"),
         WebDescription("Allow Users to add new tasks. Users must have permission to contribute to Global Task List."),
         Personalizable(PersonalizationScope.Shared),
         Category("Advanced Rendering Settings")]
        public Boolean _usersCanAddTasks
        {
            get { return _addTasks; }
            set { _addTasks = value; }
        }

        private Boolean _shwRef = false;
        [WebBrowsable(true),
         WebDisplayName("Show Asynchronous Refresh Link"),
         WebDescription("Show a link in the webpart which asynchronously refreshes the view, without refreshing the rest of the page."),
         Personalizable(PersonalizationScope.Shared),
         Category("Advanced Rendering Settings")]
        public Boolean _showRefreshLink
        {
            get { return _shwRef; }
            set { _shwRef = value; }
        }


    }
}
