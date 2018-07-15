using System;
using System.Collections.Generic;
using Microsoft.SharePoint.Publishing;
using Microsoft.SharePoint.Publishing.Navigation;
using System.Web;
using Microsoft.SharePoint;
using System.Linq;


namespace CustomMenuProvider
{
    public class CustomMenuProvider : PortalSiteMapProvider
    {
        public override SiteMapNodeCollection GetChildNodes(System.Web.SiteMapNode node)
        {
            PortalSiteMapNode pNode = node as PortalSiteMapNode;
            if (pNode != null)
            {
                if (pNode.Type == NodeTypes.Area)
                {

                    SiteMapNodeCollection nodeColl = new SiteMapNodeCollection();

                    //get and add each node
                    List<MenuItem> menuItems = GetMenuItemsForCurrentUser();
                    //get the top level nodes
                    var topNodes = from m in menuItems where String.IsNullOrEmpty(m.ParentItem) orderby m.SortOrder ascending select m;
                    //add topNodes
                    foreach (MenuItem menu in topNodes)
                    {
                        SiteMapNode cNode = new SiteMapNode(this, menu.Name, menu.URL, menu.Name);
                        cNode.ChildNodes = GetSubNodes(menuItems, menu);
                        nodeColl.Add(cNode);
                    }
                    return nodeColl;
                }
                else
                    return base.GetChildNodes(pNode);
            }
            else
                return new SiteMapNodeCollection();
        }
        public List<MenuItem> GetMenuItemsForCurrentUser()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            try
            {
                //Can switch back to hard-coded root collection URL
                using (SPSite site = new SPSite(SPContext.Current.Web.Site.WebApplication.Sites[0].Url))
                {
                    using (SPWeb web = site.RootWeb)
                    {
                        SPList menuList = web.Lists.TryGetList("NavigationMenuList");
                        if (menuList != null)
                        {
                            foreach (SPListItem item in menuList.Items)
                            {
                                SPUser user = web.CurrentUser;
                                if (item.DoesUserHavePermissions(SPContext.Current.Web.CurrentUser, SPBasePermissions.Open))
                                {
                                    //add into the custom list
                                    MenuItem menuItem = new MenuItem();
                                    menuItem.ID = item.ID;//menu item id
                                    menuItem.Name = item.Title;//menu item name
                                    //parent menu item lookup value
                                    if (item["ParentMenuItem"] != null)
                                    {
                                        SPFieldLookup parentItemLookup = (SPFieldLookup)item.Fields.GetFieldByInternalName("ParentMenuItem");
                                        SPFieldLookupValue parentItemValue = (SPFieldLookupValue)parentItemLookup.GetFieldValue(item["ParentMenuItem"].ToString());
                                        menuItem.ParentItem = parentItemValue.LookupValue;
                                    }
                                    else
                                    {
                                        menuItem.ParentItem = String.Empty;
                                    }
                                    menuItem.SortOrder = Convert.ToInt32(item["SortOrder"].ToString());//sort order
                                    menuItem.URL = item["MenuItemUrl"] != null ? item["MenuItemUrl"].ToString() : "#";//menu item URL
                                    menuItems.Add(menuItem);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return menuItems;
        }

        public SiteMapNodeCollection GetSubNodes(List<MenuItem> menuItems, MenuItem currentMenu)
        {

            //add child nodes
            var subNodes = from m in menuItems where String.Compare(m.ParentItem.Trim(), currentMenu.Name.Trim(), false) == 0 orderby m.SortOrder ascending select m;
            SiteMapNodeCollection childNodecollection = new SiteMapNodeCollection();
            foreach (MenuItem menu in subNodes)
            {

                SiteMapNode subNodeItem = new SiteMapNode(this, menu.Name, menu.URL, menu.Name);
                subNodeItem.ChildNodes = GetSubNodes(menuItems, menu);
                childNodecollection.Add(subNodeItem);
            }
            return childNodecollection;
        }
    }
    public class MenuItem
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string URL { get; set; }
        public string ParentItem { get; set; }
        public int SortOrder { get; set; }
    }

}
