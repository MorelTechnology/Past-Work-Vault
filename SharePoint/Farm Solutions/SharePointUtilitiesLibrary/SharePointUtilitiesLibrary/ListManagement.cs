using Microsoft.SharePoint;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppsToOranges.Extensions;
using static AppsToOranges.Utility;


namespace AppsToOranges.SharePointUtilities
{
    public static class ListManagement
    {
        static EventLogger log = new EventLogger("Apps To Oranges SharePoint Utilities", "Application");
        public static void addListItem(string listURL, Hashtable keyValuePairTable)
        {
            try
            {
                SPList list = GetListByUrl(listURL);
                SPListItemCollection listItems = list.Items;
                SPListItem item = listItems.Add();
                foreach (string key in keyValuePairTable.Keys)
                {
                    item[key] = keyValuePairTable.GetProperty(key);
                }
                item.Update();
            }
            catch(Exception ex)
            {
                log.addError(ex.ToString());                
                throw;
            }
        }

        public static SPList GetListByUrl(string url)
        {
            using (var site = new SPSite(url))
            {
                using (var web = site.OpenWeb())
                {
                   return web.GetList(url);
                }
            }
        }


    }
}
