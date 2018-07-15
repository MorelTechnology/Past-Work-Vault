using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using Microsoft.SharePoint.Taxonomy;

namespace LitigationManagementSiteAdministration
{
    class Utility
    {

        public static void Clear(ControlCollection controls)
        {
            foreach (Control ctrl in controls)
            {
                // recursive call incase controls are nested inside other controls
                if (ctrl.HasControls())
                {
                    Clear(ctrl.Controls);
                }
                if (ctrl is TextBox)
                {
                    ((TextBox)ctrl).Text = string.Empty;
                }
                else if (ctrl is DropDownList)
                {
                    ((DropDownList)ctrl).SelectedIndex = -1;   // -1 is the value to use for none selected in a drop down list
                }
                else if (ctrl is CheckBoxList)
                {
                    ((CheckBoxList)ctrl).SelectedIndex = 0;
                }
                else if (ctrl is ListBox)
                {
                    ((ListBox)ctrl).SelectedIndex = -1;     // -1 is the value to use for none selected in a list box
                }
                else if (ctrl is RadioButtonList)
                {
                    ((RadioButtonList)ctrl).SelectedIndex = 0;
                }
                else if (ctrl is TaxonomyWebTaggingControl)
                {
                    ((TaxonomyWebTaggingControl)ctrl).Text = string.Empty;
                }
                else if (ctrl is ClientPeoplePicker)
                {
                    ((ClientPeoplePicker)ctrl).AllEntities.Clear();
                }
                

            }
        }

        /// <summary>
        /// site IDs for manually created sites are simply created using datestamp data; 
        /// however as a failsafe we should check to see if a site exists before taking
        /// it for granted. The ID is still considered unique as long as it does not 
        /// exist already under a given parent web location. 
        /// </summary>
        /// <param name="parentWebUrl">The URL beneath which we intend to generate a site.</param>
        /// <returns></returns>
        public static string generateSiteId(string parentWebUrl)
        {
            const string PREFIX = "MC";
            string id = PREFIX + DateTime.Now.ToString("MMddyyhhmm");
            int i = 0;
            do
                if (i++ == 10)
                {
                    throw new SPException("Encountered an error while generating a unique site id. " +
                    "After multiple attempts... Something may be wrong!");
                }
                else { if(i > 1) id += "_" + i; }
            while (WebExists(parentWebUrl + "/" + id));
            return id as string;
        }
        public static void HandleException(Exception ex, ControlCollection Controls)
        {
            Controls.Clear();
            Controls.Add(new LiteralControl("<font style='color: red;'>Error: </font>" +
                ex.Message + "<br /><br />Share These Details with Support Staff:<br />" +
                parseException(ex)));
        }

        public static List<SPUser> retrieveUsersFromPeoplePicker(ClientPeoplePicker picker, string parentWebUrl)
        {
            List<SPUser> users = new List<SPUser>();
            foreach (PickerEntity entity in picker.ResolvedEntities)
            { users.Add(SPContext.Current.Site.RootWeb.EnsureUser(entity.Key)); }
            return users;
        }

        public static bool WebExists(string absoluteUrl)
        {
            try
            {
                using (var web = new SPSite(absoluteUrl).OpenWeb(absoluteUrl, true))
                { return web.Exists; }
            }
            catch (FileNotFoundException) { return false; }
        }

        private static string parseException(Exception ex)
        {
            string message =
                "Exception type: " + ex.GetType() + "<br />" +
                "Exception message: " + ex.Message + "<br />" +
                "Stack trace: " + ex.StackTrace + "<br />";
            if (ex.InnerException != null)
            {
                message += "---BEGIN InnerException--- " + "<br />" +
                           "Exception type " + ex.InnerException.GetType() + "<br />" +
                           "Exception message: " + ex.InnerException.Message + "<br />" +
                           "Stack trace: " + ex.InnerException.StackTrace + "<br />" +
                           "---END Inner Exception";
            }
            return message;
        }

    }
}
