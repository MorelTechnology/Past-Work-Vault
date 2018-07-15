using System;
using System.ComponentModel;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls.WebParts;

namespace SharePointTabify.Tabify
{
    [ToolboxItemAttribute(false)]
    public partial class Tabify : WebPart
    {
        // Uncomment the following SecurityPermission attribute only when doing Performance Profiling on a farm solution
        // using the Instrumentation method, and then remove the SecurityPermission attribute when the code is ready
        // for production. Because the SecurityPermission attribute bypasses the security check for callers of
        // your constructor, it's not recommended for production purposes.
        // [System.Security.Permissions.SecurityPermission(System.Security.Permissions.SecurityAction.Assert, UnmanagedCode = true)]
        public Tabify()
        {
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            InitializeControl();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
                this.Title = "Tab Controls (Hidden when viewing page)";
                this.ChromeType = PartChromeType.None;
                RenderContents();
        }

        protected void RenderContents()
        {
            // Ensure we have at least the base JQuery UI by loading the internal copy.  This prevents broken tabs in case
            // the CDN is unreachable.
               Page.Header.Controls.Add(
               new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("/_layouts/15/SharePointTabify/css/JQuery-UI-base.1.12.1.css") + "\" />"));

            if (_theme == cssOptions.Custom)
            {
                Page.Header.Controls.Add(
      new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl(_customCss) + "\" />"));
            }
            else
            {
                Page.Header.Controls.Add(
                new System.Web.UI.LiteralControl("<link rel=\"stylesheet\" type=\"text/css\" href=\"" + ResolveUrl("//ajax.aspnetcdn.com/ajax/jquery.ui/1.12.1/themes/" + _theme.ToString().Replace("_", "-") + "/jquery-ui.css") + "\" />"));
            }

            StringBuilder TabCommand = new StringBuilder();
            if (!String.IsNullOrEmpty(_tab1Name))
            {
                if (String.IsNullOrEmpty(_tab1Webparts)) { _tab1Webparts = "Warning: No Value Specified."; }
                TabCommand.Append('"' + _tab1Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab1Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab2Name))
            {
                if(String.IsNullOrEmpty(_tab2Webparts)) { _tab2Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab2Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab2Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab3Name))
            {
                if (String.IsNullOrEmpty(_tab3Webparts)) { _tab3Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab3Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab3Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab4Name))
            {
                if (String.IsNullOrEmpty(_tab4Webparts)) { _tab4Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab4Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab4Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab5Name))
            {
                if (String.IsNullOrEmpty(_tab5Webparts)) { _tab5Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab5Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab5Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab6Name))
            {
                if (String.IsNullOrEmpty(_tab6Webparts)) { _tab6Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab6Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab6Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab7Name))
            {
                if (String.IsNullOrEmpty(_tab7Webparts)) { _tab7Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab7Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab7Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab8Name))
            {
                if (String.IsNullOrEmpty(_tab8Webparts)) { _tab8Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab8Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab8Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab9Name))
            {
                if (String.IsNullOrEmpty(_tab9Webparts)) { _tab9Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab9Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab9Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }
            if (!String.IsNullOrEmpty(_tab10Name))
            {
                if (String.IsNullOrEmpty(_tab10Webparts)) { _tab10Webparts = "Warning: No Value Specified.";}
                TabCommand.Append(',');
                TabCommand.Append('"' + _tab10Name);
                TabCommand.Append(';');
                TabCommand.Append('#' + (_tab10Webparts.Replace(", ", ",").Replace(",", ";#")) + '"');
            }


            // Create a new client script instance on the page.
            String scriptName = "Invoke_" + ID; // this needs to be unique, in order to support multiple instances per page.
            Type scriptType = this.GetType();
            // Get a ClientScriptManager reference from the Page class.
            ClientScriptManager scriptInstance = Page.ClientScript;
            // Check to see if the script is already registered.
            if (!scriptInstance.IsStartupScriptRegistered(scriptType, scriptName))
            {
                StringBuilder scriptPayload = new StringBuilder();
                scriptPayload.Append("<script type=text/javascript>");
                scriptPayload.Append("var TabArray = [" + TabCommand + "];");
                scriptPayload.Append("jqueryTabs(TabArray);");
                scriptPayload.Append("</script>");
                scriptInstance.RegisterStartupScript(scriptType, scriptName, scriptPayload.ToString());
            }
        }

        public enum cssOptions
        { @base, black_tie, blitzer, cupertino, dark_hive, dot_luv, eggplant, excite_bike, flick, hot_sneaks, humanity, le_frog,
          mint_choc, overcast, pepper_grinder, redmond, smoothness, south_street, start, sunny, swanky_purse, trontastic,
          ui_darkness, ui_lightness, Custom }

        private cssOptions _thm = cssOptions.start; // Set theme to 'start' by default.
        [WebBrowsable(true),
        WebDisplayName("Tab Styling"),
        WebDescription("Select a theme for tabs, or choose 'Custom' to supply your own JQuery-UI stylesheet.  " +
            "The value set here will apply to the entire page. NOTE: If you have multiple Tabify controls on a page, " +
            "the bottom-most control's value will be the effective theme for all Tabify controls on the page!"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab Styles")]
        public cssOptions _theme
        {
            get { return _thm; }
            set { _thm = value; }
        }
       
        [WebBrowsable(true),
        WebDisplayName("Custom JQuery-UI Theme URL (.css)"),
        WebDescription("If you've selected 'Custom' theme above, specify a URL to a JQuery-UI Theme. " +
            "(make your own at http://jqueryui.com/themeroller)"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab Styles")]
        public string _customCss { get; set; }
            
        [WebBrowsable(true),
        WebDisplayName("Tab #1 Display Name"),
        WebDescription("Enter a title to display on Tab #1"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #1 Settings")]
        public string _tab1Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #1 Settings")]
        public string _tab1Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #2 Display Name"),
        WebDescription("Enter a title to display on Tab #2"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #2 Settings")]
        public string _tab2Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #2 Settings")]
        public string _tab2Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #3 Display Name"),
        WebDescription("Enter a title to display on Tab #3"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #3 Settings")]
        public string _tab3Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #3 Settings")]
        public string _tab3Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #4 Display Name"),
        WebDescription("Enter a title to display on Tab #4"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #4 Settings")]
        public string _tab4Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #4 Settings")]
        public string _tab4Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #5 Display Name"),
        WebDescription("Enter a title to display on Tab #5"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #5 Settings")]
        public string _tab5Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #5 Settings")]
        public string _tab5Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #6 Display Name"),
        WebDescription("Enter a title to display on Tab #6"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #6 Settings")]
        public string _tab6Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #6 Settings")]
        public string _tab6Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #7 Display Name"),
        WebDescription("Enter a title to display on Tab #7"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #7 Settings")]
        public string _tab7Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #7 Settings")]
        public string _tab7Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #8 Display Name"),
        WebDescription("Enter a title to display on Tab #8"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #8 Settings")]
        public string _tab8Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #8 Settings")]
        public string _tab8Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #9 Display Name"),
        WebDescription("Enter a title to display on Tab #2"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #9 Settings")]
        public string _tab9Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #9 Settings")]
        public string _tab9Webparts { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Tab #10 Display Name"),
        WebDescription("Enter a title to display on Tab #2"),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #10 Settings")]
        public string _tab10Name { get; set; }

        [WebBrowsable(true),
        WebDisplayName("Webparts to include in this tab. (List of titles, comma separated.)"),
        WebDescription("Enter the titles of webparts in this zone, which shoud be included in this tab."),
        Personalizable(PersonalizationScope.Shared),
        Category("Tab #10 Settings")]
        public string _tab10Webparts { get; set; }

    }
}
