using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using System.Web;

namespace Guidewire_Integration
{
    class Configuration
    {
        private const string QUERY = @"<Where><BeginsWith><FieldRef Name=""Property""></FieldRef><Value Type=""Text"">{0}</Value></BeginsWith></Where>";

        public static void AddDefaultConfigurationValue(SPWeb web, string property, string setting)
        {
            SPList configurationList = web.Lists.TryGetList(Resource.ConfigurationList);
            if (configurationList != null)
            {
                SPListItem item = configurationList.Items.Add();
                item["Title"] = property;
                item["Property"] = property;
                item["Setting"] = setting;
                item.Update();
            }
        }

        public static string GetConfigurationValue(SPWeb web, string property)
        {
            SPSite site = web.Site;
            string setting = string.Empty;
            SPSecurity.RunWithElevatedPrivileges(delegate()
            {
                using (SPSite elevatedSite = new SPSite(site.ID))
                {
                    using (SPWeb elevatedWeb = elevatedSite.OpenWeb(web.ID))
                    {
                        SPList configurationList = elevatedWeb.Lists.TryGetList(Resource.ConfigurationList);
                        if (configurationList != null && configurationList.Fields.ContainsField("Property"))
                        {
                            SPQuery query = new SPQuery();
                            query.Query = string.Format(QUERY, property);

                            SPListItemCollection settings = configurationList.GetItems(query);
                            if (settings != null && settings.Count > 0)
                            {
                                SPListItem settingItem = settings[0];
                                setting = (string)settingItem["Setting"];
                            }
                        }
                    }
                }
            });
            return setting;
        }
    }
}
