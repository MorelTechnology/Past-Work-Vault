using Microsoft.SharePoint;
using Microsoft.SharePoint.Deployment;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AppsToOranges.Utility;
using AppsToOranges.Extensions;
using System.Collections;

namespace AppsToOranges.SharePointUtilities
{
    public static class Provisioning
    {
        static EventLogger log = new EventLogger("Apps To Oranges SharePoint Utilities", "Application");
        
        /// <summary>
        /// Creates a Template file (.cmp file) based on the specified web site.  
        /// File is stored in the specified location.
        /// </summary>
        /// <param name="spWeb"></param>
        /// <param name="pathForTemplateFile"></param>
        public static void exportSpWeb(SPWeb spWeb, string pathForTemplateFile)
        {
            try
            {
                SPExportObject spExportObject = new SPExportObject();
                spExportObject.Id = spWeb.ID;
                spExportObject.IncludeDescendants = SPIncludeDescendants.All;
                spExportObject.Type = SPDeploymentObjectType.Web;
                SPExportSettings exportSettings = new SPExportSettings();
                exportSettings.SiteUrl = spWeb.Url;
                exportSettings.ExportMethod = SPExportMethodType.ExportAll;
                exportSettings.FileLocation = Path.GetDirectoryName(pathForTemplateFile);
                exportSettings.BaseFileName = Path.GetFileNameWithoutExtension(pathForTemplateFile);
                exportSettings.FileCompression = true;
                exportSettings.IncludeSecurity = SPIncludeSecurity.All;
                exportSettings.ExcludeDependencies = true;
                exportSettings.OverwriteExistingDataFile = true;
                exportSettings.ExportObjects.Add(spExportObject);
                SPExport export = new SPExport(exportSettings);
                export.Run();
            }
            catch (Exception ex)
            {
                log.addError("Fatal Error: Could not export specified site. " + ex.ToString());
                throw;
            }
        }


        public static String GetTemplate(string templateSolutionName, SPSite siteCollection)
        {
            var queryResult = string.Empty;
            try
            {
                queryResult = (from SPWebTemplate template in siteCollection.GetWebTemplates((uint)siteCollection.RootWeb.Locale.LCID)
                               where template.Title == templateSolutionName
                               select template.Name).First();
            }
            catch (Exception ex)
            {
                // log error 
                log.addError(ex.ToString() + "Unable to identify a template using the supplied values:\n"
                    + "Template Name: "+ templateSolutionName+"\n"
                    + "Site Collection: " + siteCollection.Url);
                throw;
            }
            return (string)queryResult;
        }



        /// <summary>
        /// Provisions a Subweb at the specified location, based on a previously exported template file (.cmp)
        /// in the path specified.
        /// </summary>
        /// <param name="destinationSiteUrl">The Parent Website the new site should be created under, i.e.: 'http://sharepoint/sites/MyCollection/MyParentWeb'</param>
        /// <param name="destinationWebUrl">The relative URL to assign to the imported website.  i.e.: 'MyNewWebSite'</param>
        /// <param name="pathToTemplateFile">The File Location where an existing template file with extension .CMP can be found. i.e.: @"C:\MyPath\Templates\SiteTemplate.cmp"</param>
        /// <returns>Boolean, True or False, indicative of success.</returns>
        public static bool importSpWeb(string destinationSiteUrl, string destinationWebUrl, string pathToTemplateFile)
        {
            bool success = false;
            string importedSite = null;
            try
            {
                using (SPSite site = new SPSite(destinationSiteUrl))
                {
                    using (SPWeb web = site.OpenWeb())
                    {
                        SPImportSettings importSettings = new SPImportSettings();
                        importSettings.CommandLineVerbose = true; // uncomment for testing
                        importSettings.SiteUrl = site.Url;
                        importSettings.WebUrl = web.Url;
                        importSettings.FileLocation = Path.GetDirectoryName(pathToTemplateFile);
                        importSettings.BaseFileName = Path.GetFileName(pathToTemplateFile);
                        importSettings.FileCompression = true;
                        importSettings.IncludeSecurity = SPIncludeSecurity.All;
                        importSettings.Validate();
                        SPImport import = new SPImport(importSettings);

                        import.ObjectImported += delegate (object delegateSender, SPObjectImportedEventArgs delegateArgs)
                        {
                            if (string.IsNullOrEmpty(importedSite)) importedSite = delegateArgs.TargetUrl;
                            return;
                        };
                        import.Run();

                        // The above brought in a new web, but it's identical to the template, including the URL and title.
                        // So let's get the new web and set its URL appropriately, and change the title to a temporary one.

                        using (SPWeb newWeb = site.OpenWeb(importedSite, true))
                        {
                            newWeb.Name = destinationWebUrl;
                            newWeb.AllProperties.SetProperty("Site_Created", DateTime.Now.ToString());
                            newWeb.Title = "Temporary Import Generated at " + newWeb.AllProperties["Site_Created"].ToString();
                            newWeb.Update();
                        }

                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                success = false;
                log.addError("Could not create site from imported template file: " + ex.ToString());
                return success;
            }
            return success;
        }

        public static string setWebProperties(SPWeb web, Hashtable properties, bool castNewValueAsExistingType = false)
        {
            StringBuilder propertyUpdatesPerformed = new StringBuilder(); // used to collect results

            try
            {
                Hashtable siteProperties = web.AllProperties;
                web.AllowUnsafeUpdates = true;
                foreach (string key in properties.Keys)
                {
                    var newValue = properties.GetProperty(key);
                    var existingValue = siteProperties.GetProperty(key);
                    if (existingValue != null)
                    {
                        if(existingValue as string != newValue as string)
                        {
                            propertyUpdatesPerformed.AppendLine("Setting new value for property [" + key + "]... " +
                               " Processing \"" + existingValue + "\" --> \"" + newValue + "\"");

                            if (castNewValueAsExistingType)
                            {
                                if (existingValue.GetType() != newValue.GetType())
                                    propertyUpdatesPerformed.AppendLine(" (Recasting incoming type " +
                                        "[" + newValue.GetType() + "] => [" + existingValue.GetType() + "])");
                                siteProperties.Remove(key); web.Update();

                            }
                            siteProperties.SetProperty(key, newValue); // set the new value
                        }
                    }
                    else
                    {
                        propertyUpdatesPerformed.AppendLine("Adding new property property [" + key + "]... " +
                            "Value: \"" + newValue + "\".");
                        siteProperties.SetProperty(key, newValue); // property didn't exist, so added it.
                    }
                }
                web.Update(); // must update the web in order to set the properties.
                // No Error thrown...
                propertyUpdatesPerformed.AppendLine();
                propertyUpdatesPerformed.AppendLine("The update completed successfully.");
                web.AllowUnsafeUpdates = false;
            }
            catch (Exception ex)
            {
                log.addError("An issue occured while trying to set properties for a site. ("
                    + web.ServerRelativeUrl + "). Attempted Transaction:\n" + propertyUpdatesPerformed.ToString() +
                    "\nError returned by the host:\nException: " + log.parseException(ex));
            }
            return propertyUpdatesPerformed.ToString();

        }

    }
}
