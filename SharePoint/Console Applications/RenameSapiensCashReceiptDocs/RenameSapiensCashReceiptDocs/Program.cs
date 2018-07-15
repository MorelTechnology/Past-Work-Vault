using Microsoft.SharePoint;
using System;
using System.Configuration;

namespace RenameSapiensCashReceiptDocs
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateDocuments();
        }

        private static void UpdateDocuments()
        {
            try
            {
                using (SPSite site = new SPSite(ConfigurationManager.AppSettings["SharePointSiteUrl"]))
                {
                    Console.WriteLine("Successfully connected to site at " + site.Url);
                    using (SPWeb web = site.OpenWeb())
                    {
                        Console.WriteLine("Successfully opened SPWeb at " + web.Url);
                        string targetFileExtension = ConfigurationManager.AppSettings["TargetFileExtension"];
                        SPList documentLibrary = web.Lists[ConfigurationManager.AppSettings["LibraryName"]];
                        SPQuery query = new SPQuery();
                        query.ViewXml = Util.GetViewQuery();
                        query.QueryThrottleMode = SPQueryThrottleOption.Override;
                        do
                        {
                            SPListItemCollection items = documentLibrary.GetItems(query);
                            int totalItems = items.Count;
                            Console.WriteLine("Processing items " + (query.ListItemCollectionPosition != null ? query.ListItemCollectionPosition.ToString() : "0") + " to " + query.ListItemCollectionPosition + totalItems);
                            query.ListItemCollectionPosition = items.ListItemCollectionPosition;
                            for (int i = 0; i < items.Count; i++)
                            {
                                SPListItem item = items[i];
                                try
                                {
                                    string currentFileName = item[SPBuiltInFieldId.FileLeafRef].ToString();
                                    string currentFileNameNoExtension = System.IO.Path.GetFileNameWithoutExtension(currentFileName);
                                    string newFileName = currentFileNameNoExtension + "." + targetFileExtension;
                                    var currentEditor = item["Editor"];
                                    var currentModified = item["Modified"];
                                    SPFile file = item.File;
                                    item.File.MoveTo(item.ParentList.RootFolder.Url + "/" + newFileName, true);
                                    file.Item["Editor"] = currentEditor;
                                    file.Item["Modified"] = currentModified;
                                    file.Item.UpdateOverwriteVersion();
                                    Console.WriteLine("Document updated. ItemId=" + file.Item.ID);
                                }
                                catch (Exception fileUpdateException)
                                {
                                    Util.LogError(string.Format("There was an error updating item with ID {0}. Exception details: {1}", item.ID, fileUpdateException.Message));
                                }
                            }
                        }
                        while (query.ListItemCollectionPosition != null);
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogError(string.Format("Error connecting to SharePoint. Exception details: {0}", ex.Message));
            }
        }
    }
}
