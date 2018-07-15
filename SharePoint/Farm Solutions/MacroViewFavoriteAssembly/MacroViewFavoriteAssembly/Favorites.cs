using DMFUtilities;
using System.Collections.Generic;
using System.Xml;
using Microsoft.SharePoint;
using static MacroViewFavoriteAssembly.Util;

namespace MacroViewFavoriteAssembly
{

    public class Favorites
    {
        Configuration config = new Configuration();

        /// <summary>
        /// Assembles a list of favorites sites for use in the MacroView Desktop Client. 
        /// Currently this is only implemented for the Litigation Management Site, but may be
        /// extended in the future.
        /// Favorite Sites are assembled in the following Groups:
        /// * Assigned To Current User
        /// * Shared with Current User
        /// * Sites Pending Close (Deprecated due to archival process implemented 3/2017.)
        /// 
        /// Generation of site objects is performed using the SPWeb.GetSubwebsForCurrentUser method.
        /// Note that this method will only return subwebs for one level deep.  
        /// </summary>
        /// <param name="UserName"></param>
        /// <param name="SiteUrl"></param>
        /// <returns>An XmlDocument object for processing by the client</returns>

        public XmlDocument GetFavorites(string UserName, string SiteUrl)
        {
            config.webAppUrl = SiteUrl;
            config.currentUser = sAMAccountName(UserName);
            string mattersUrl = config.webAppUrl + config.siteCollectionUrl + config.mattersWeb;
            string projectsUrl = config.webAppUrl + config.siteCollectionUrl + config.projectsWeb;

            // TODO - Extend this method to other users beyond litigation management by 
            // retrieving their parent site from a custom user property.

            // Create Empty Matter Group and Empty Project Groups to populate
            Group grpMyMatters = createGroup("Litigation Matters");
            Group grpMyProjects = createGroup("Project Sites");

            #region Logic: Create Litigation Matters Group
            // Open the Site which contains Litigation Matters and retrieve the webs for the current user
            // add each to the matters group.
            using (SPWeb matterRootWeb = new SPSite(mattersUrl).OpenWeb())
            {
                // Check if Site Has a Configuration Property for Limit.  If so, set it.  If not, use the default value.
                int siteFavoriteLimit = 0;
                try { siteFavoriteLimit = (int)matterRootWeb.AllProperties["MacroView_Favorite_Limit"]; } catch { }
                if (siteFavoriteLimit > 0) config.maxFavorites = siteFavoriteLimit;

                // Get a collection which contains all sites the user has access to.
                SPWebCollection webs = matterRootWeb.GetSubwebsForCurrentUser();

                // initialize Limit Counter
                int max = (config.maxFavorites), count = 0;
                foreach (SPWeb web in webs)
                {
                    if (count >= max + 1) { count++; continue; }
                    if (count == max) { truncateGroup(config, grpMyMatters, mattersUrl); count++; continue; }
                    else { addFavorite(web, grpMyMatters, false, false, false); count++; }
                }
            }
            #endregion

            #region Logic: Create Project Sites Group
            // Open the Site which contains Litigation Projects and retrieve the webs for the current user
            // add each to the projects group.
            using (SPWeb projectRootWeb = new SPSite(projectsUrl).OpenWeb())
            {
                // Check if Site Has a Configuration Property for Limit.  If so, set it.  If not, use the default value.
                int siteFavoriteLimit = 0;
                try { siteFavoriteLimit = (int)projectRootWeb.AllProperties["MacroView_Favorite_Limit"]; } catch { }
                if (siteFavoriteLimit > 0) config.maxFavorites = siteFavoriteLimit;

                // Get a collection which contains all sites the user has access to.
                SPWebCollection webs = projectRootWeb.GetSubwebsForCurrentUser();

                // initialize Limit Counter
                int max = (config.maxFavorites), count = 0;
                foreach (SPWeb web in webs)
                {
                    if (count >= max + 1) { count++; continue; }
                    if (count == max) { truncateGroup(config, grpMyProjects, projectsUrl); count++; continue; }
                    else { addFavorite(web, grpMyProjects, false, false, true); count++; }
                }
            }
            #endregion

            // Create wrapper list to hold groups which should be displayed.
            List<Group> Groups = new List<Group>();
                Groups.Add(grpMyMatters);
                Groups.Add(grpMyProjects);

             // Sort and Sanitize the groups, then serialize the object as XML to the client.
                return generateXml(sanitize(Groups));
            }
      
        /// <summary>
        /// Not Currently Implemented, but mirrors the Method used in the MacroView Core Class.
        /// Could be used to accomodates user adding a custom favorite from another site, such as
        /// a favorites overload list.
        /// </summary>
        public void SaveFavorites()
        { }
    }
}


