using DMFUtilities;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration.Claims;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace MacroViewFavoriteAssembly
{
    public class Util
    {
        public enum ErrorLevel
        {
            Info,
            Warning,
            Error
        }
        
        /// <summary>
        /// Write message to Event Viewer Application log as Error
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(String message)
        {
            LogError(message, ErrorLevel.Error);
        }
        
        /// <summary>
        /// Write message to Event Viewer Application log as Error, Warning or Information
        /// </summary>
        /// <param name="message"></param>
        /// <param name="errorLevel"></param>
        public static void LogError(String message, Util.ErrorLevel errorLevel)
        {
            SPSecurity.RunWithElevatedPrivileges(() =>
            {
                EventLogEntryType logErrorLevel;
                switch (errorLevel)
                {
                    case ErrorLevel.Error:
                        logErrorLevel = EventLogEntryType.Error;
                        break;
                    case ErrorLevel.Info:
                        logErrorLevel = EventLogEntryType.Information;
                        break;
                    case ErrorLevel.Warning:
                        logErrorLevel = EventLogEntryType.Warning;
                        break;
                    default:
                        logErrorLevel = EventLogEntryType.Error;
                        break;
                }
                try
                {
                    var appLog = new EventLog { Source = "MacroViewFavoriteAssembly" };
                    appLog.WriteEntry(message, logErrorLevel, 42);
                }
                catch (Exception e)
                {
                    Microsoft.Office.Server.Diagnostics.PortalLog.LogString("Guidewire Integration errored: " + e.Message + " Original message: " + message);
                }
            });
        }
        
        /// <summary>
        /// Creates a Favorite item from the exisintg website and adds it to the group.  
        /// A Favorite denotes an entry in the MacroView Favorites list.
        /// </summary>
        /// <param name="web">SPWeb Object. A SharePoint site for which a favorite should be created.</param>
        /// <param name="group">DMFUtilities.Group Object. Used to contain favorite items.</param>
        /// <param name="showLastActive">Boolean. Indicates whether the entry should list the last activity date in its title.</param>
        /// <param name="showManager">Indicates whether the entry title should include the assigned site manager. (Shows unknown if unresolvable.) </param>
        internal static void addFavorite(SPWeb web, Group group, bool showLastActive = false, bool showManager = false, bool showProjectLead = false)
        {
            try
            {
                // Build a new favorite and set its values
                Favorite favorite = new Favorite();
                favorite.Name = !showLastActive ? web.Title : web.Title +
                                "\n(Last activity: " + web.LastItemModifiedDate.ToShortDateString() + ")";
                if (showManager)
                {
                    string siteManager = "Unavailable";
                    try { siteManager = web.AllProperties["Litigation_Manager"].ToString(); }
                    catch (Exception) { }
                    favorite.Name += "\n(Manager: " + siteManager + ")";
                }
                if (showProjectLead)
                {
                    string projectLead = "Unavailable";
                    try { projectLead = web.AllProperties["Project_Lead"].ToString(); }
                    catch (Exception) { }
                    favorite.Name += "\n(Project Lead: " + projectLead + ")";
                }
                favorite.ServerURL = web.Site.Url;
                favorite.SiteURL = web.Url;
                favorite.Type = "site";
                favorite.WebTitle = web.Title;
                favorite.WebID = web.ID;
                favorite.SiteID = web.Site.ID;
                favorite.Description = "";
                favorite.Libraries = new List<Library>();

                // Retrieve document libraries in the site and add them to the favorite item.
                foreach (SPList oList in web.Lists)
                {
                    // Only get list objects that are document libraries and not hidden
                    if (oList.BaseType == SPBaseType.DocumentLibrary && !oList.Hidden)
                    {
                        // Create a library object and append it to the favorite
                        Library library = new Library
                        {
                            LibraryURL = oList.ParentWeb.Url + "/" + oList.RootFolder.Url,
                            ListID = oList.ID,
                            ListTitle = oList.Title,
                            Name = oList.Title,
                            ServerURL = oList.ParentWeb.Site.Url,
                            SiteURL = oList.ParentWeb.Url,
                            Type = "doclib"
                        };
                        favorite.Libraries.Add(library);
                    }
                }
                // Add the favorite to the list of favorites
                group.Favorites.Add(favorite);
            }
            catch (Exception ex)
            {
                Util.LogError(string.Format("Error creating favorite for Site {0}, ({1}). Exception details: {2}", web.Title, web.Url, ex.Message));
            }
        }

        /// <summary>
        /// Creates a DMFUtilities.Group object used to hold Favorites.
        /// </summary>
        /// <param name="groupName">String.  Name for the group object</param>
        /// <returns>DMFUtilities.Group Object</returns>
        internal static Group createGroup(string groupName)
        {
            return new Group
            {
                Name = groupName,
                //HasNewChildren = true
            };
        }
        
        /// <summary>
        /// Used to Serialize MacroView Favorite objects to an XML Document which is readable by the Desktop Client.
        /// </summary>
        /// <param name="Groups">List of DMFUtilities.Group objects intended to display to end users in the client.</param>
        /// <returns></returns>
        internal static XmlDocument generateXml(List<Group> Groups)
        {
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode docNode = xmlDoc.CreateXmlDeclaration("1.0", "UTF-8", null);
            xmlDoc.AppendChild(docNode);
            XmlDocumentFragment groupsNode = xmlDoc.CreateDocumentFragment();
            groupsNode.InnerXml = objectToXml(Groups, true);
            xmlDoc.AppendChild(groupsNode);
            return xmlDoc;
        }
        
        internal static void truncateGroup(Configuration config, Group group, string siteListUrl )
        {
           group.Favorites = new List<Favorite>
            {
                new Favorite
                {
                    Name = config.maxWarning,
                    ServerURL = siteListUrl,
                    SiteURL = siteListUrl,
                    Type = "site",
                    WebTitle = config.maxWarning,
                    WebID = new Guid("00000000-0000-0000-0000-000000000000"),
                    SiteID = new Guid("00000000-0000-0000-0000-000000000000"),
                    Description = "",
                    Libraries = new List<Library>() // blank
                }
            };

            
        }

        /// <summary>
        /// Attempts to resolve a given SharePoint Claims Identity string to a sAMAccountName / Windows Login ID.
        /// </summary>
        /// <param name="userName">String. User Identity Claim string.</param>
        /// <returns></returns>
        internal static string sAMAccountName(string userName)
        {
            SPClaimProviderManager mgr = SPClaimProviderManager.Local;
            if (mgr != null)
            {
                string domainUser = mgr.DecodeClaim(userName).Value;
                return domainUser.Substring(domainUser.LastIndexOf('\\') + 1);
            }
            return "unknown"; // user could not be determined from string.
        }
        
        /// <summary>
        /// Tidys up MacroView Favorite Objects, prior to sending them to the Client.
        /// Removes zero-item groups, and sorts favorites by site title.  Additionally, it implements a safety mechanism
        /// for groups which contain too many favorites for the client to display.  Large groups will present a warning message
        /// and direct users to results, external to the client.
        /// </summary>
        /// <param name="Groups"></param>
        /// <returns>List of [DMFUtilities.Group] objects.</returns>
        internal static List<Group> sanitize(List<Group> Groups)
        {
            // Remove zero-item groups. Sort Groups by site title.

            List<Group> sanitizedGroups = new List<Group>();

            foreach (Group group in Groups)
            {
                var count = group.Favorites.Count;
                if (count == 0) { continue; } // Group has no favorite content.  Skip it.
                group.Name += " (" + count + ")"; // Add a Total Item Count next to the name.
                { group.Favorites.Sort((a, b) => a.Name.CompareTo(b.Name)); } // Alpha Sort
                sanitizedGroups.Add(group); // add the cleaned-up group to the list we'll return.
            }
            return sanitizedGroups;
        }
        
        /// <summary>
        /// Searches the given SharePoint site for the isSiteActive property and returns the result.
        /// </summary>
        /// <param name="web">SPWeb object. SharePoint Site to inspect.</param>
        /// <returns>True or False</returns>
        internal static bool siteIsActive(SPWeb web)
        {
            try { if (web.AllProperties["isMatterActive"].ToString().ToLower() == "0" || web.AllProperties["isMatterActive"].ToString().ToLower() == "false") return false; }
            catch (Exception) { return true; } // Couldn't read property, can't deterime, assume true.
            return true;
        }
        
        /// <summary>
        /// Searches the given SharePoint site for the Matter_Status property and returns true if the status is 'closed'.
        /// </summary>
        /// <param name="web">SPWeb object. SharePoint Site to inspect.</param>
        /// <returns>True or False</returns>
        internal static bool siteIsMarkedClosed(SPWeb web)
        {
            try { if (web.AllProperties.ContainsKey("Matter_Status") && web.AllProperties["Matter_Status"].ToString().ToLower() == "closed") return true; }
            catch (Exception) { return false; } // Couldn't read property, can't determine, assume false.
            return false;
        }

        /// <summary>
        /// Searches the given SharePoint site for the LMUserID property and compares it to a given userId to determine a match.
        /// </summary>
        /// <param name="web">SPWeb object. SharePoint Site to inspect.</param>
        /// <param name="UserName">The User ID to compare against the site's manager user.</param>
        /// <returns>True or False</returns>
        internal static bool siteIsAssignedToUser(SPWeb web, string UserName)
        {
            try { if (web.AllProperties["LMUserID"].ToString().ToLower() == UserName.ToLower()) return true; }
            catch (Exception) { return false; } // Couldn't read property, can't determine, assume false.
            return false;
        }

        /// <summary>
        /// Used to prevent large favorite groups from being returned to the client.
        /// </summary>
        /// <param name="warningMessage">String. Message to display, which indicates that results could not be returned.</param>
        /// <param name="siteForUserRemediation">Optional: The site the user should be directed to for remediation.</param>
        /// <returns>List of one single DMFUtilities.Favorite, which is used to display an error message in the client.</returns>
 
        /// <summary>
        /// Used to convert an object to an XML Fragment.  While similar to Serializing an object,
        /// this method will can return an XML Fragment, which is necessary in this application to avoid
        /// creating an end-product document with Declaration and/or namespace collisions.
        /// </summary>
        /// <param name="obj">Object to Serialize to XML</param>
        /// <param name="returnFragment">Boolean.  Specify whether the returned result should
        /// be a fully qualified XML document or only a fragment.</param>
        /// <returns>The Object Contents, serialized as either an XML Document or Fragment.</returns>
        private static string objectToXml(object obj, bool returnFragment)
        {
            XmlSerializer serializer = new XmlSerializer(obj.GetType());
            StringBuilder xml = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            if (returnFragment)
            {
                settings.OmitXmlDeclaration = true;
                settings.ConformanceLevel = ConformanceLevel.Auto;
                namespaces.Add(string.Empty, string.Empty);
            }
            XmlWriter xmlWriter = XmlWriter.Create(xml, settings);
            if (returnFragment)
            { serializer.Serialize(xmlWriter, obj, namespaces); }
            else
            { serializer.Serialize(xmlWriter, obj); }
            xmlWriter.Close();
            xmlWriter.Dispose();
            return xml.ToString();
        }
    }
}
