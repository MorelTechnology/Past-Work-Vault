using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using System.Configuration;
using Topshelf;
using System.Timers;

namespace Link_GW_Docs
{
    class Program
    {
        public static bool inProgress = false;
        static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<TimerService>(s =>
                {
                    s.ConstructUsing(name => new TimerService());
                    s.WhenStarted(ts => ts.Start());
                    s.WhenStopped(ts => ts.Stop());
                });
                x.RunAs(Settings.Default.ServiceUsername, Settings.Default.ServicePassword);

                x.SetDescription("Polls SharePoint for Guidewire documents that need updated metadata");
                x.SetDisplayName("Guidewire Document Update Service");
                x.SetServiceName("Guidewire Document Update Service");
            });
        }

        private static bool ProcessWorkMatterDocuments()
        {
            inProgress = true;
            bool success = false;
            try
            {
                foreach(string libraryName in Settings.Default.LibrariesToProcess)
                {
                    using (SPSite site = new SPSite(ConfigurationManager.AppSettings["SiteUrl"], SPUserToken.SystemAccount))
                    {
                        Console.WriteLine("Successfully connected to site at " + site.Url);
                        using (SPWeb web = site.OpenWeb())
                        {
                            Console.WriteLine("Successfully opened SPWeb at " + web.Url);
                            SPList workMatterDocumentLibrary = web.Lists[libraryName];
                            SPQuery query = new SPQuery();
                            query.ViewXml = Util.GetViewQuery();
                            query.QueryThrottleMode = SPQueryThrottleOption.Override;
                            do
                            {
                                SPListItemCollection items = workMatterDocumentLibrary.GetItems(query);
                                int totalItems = items.Count;
                                Console.WriteLine("Processing items " + (query.ListItemCollectionPosition != null ? query.ListItemCollectionPosition.ToString() : "0") + " to " + query.ListItemCollectionPosition + totalItems);
                                query.ListItemCollectionPosition = items.ListItemCollectionPosition;
                                for (int i = 0; i < items.Count; i++)
                                {
                                    SPListItem item = items[i];

                                    try
                                    {
                                        web.AllowUnsafeUpdates = true;
                                        Records.BypassLocks(item, delegate (SPListItem delegateItem)
                                        {
                                            using (DisabledEventsScope scope = new DisabledEventsScope())
                                            {

                                                SPBusinessDataField field = delegateItem.Fields[Resource.FieldBCSWorkMatterDocument] as SPBusinessDataField;
                                                string documentId = Util.GetDocumentId(delegateItem);
                                                using (SPServiceContextScope ctxScope = new SPServiceContextScope(SPServiceContext.GetContext(site)))
                                                {
                                                    try
                                                    {
                                                        Util.SetBCSField(delegateItem, field, documentId, "GetWorkMatterDocumentBySPID");
                                                        Util.ClearFields(item);
                                                        if (delegateItem.Fields.ContainsField(Resource.FieldBCSWorkMatterDocumentStatus) && delegateItem[Resource.FieldBCSWorkMatterDocumentStatus] != null && delegateItem[Resource.FieldBCSWorkMatterDocumentStatus].ToString() == "Final")
                                                        {
                                                            Util.LockItem(delegateItem);
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        // Swallow the error at this level
                                                    }
                                                }

                                                Console.WriteLine("Document updated. ItemId=" + delegateItem.ID);
                                            }
                                        });
                                    }
                                    catch (Exception ex)
                                    {
                                        Util.LogError("Error updating document: " + ex.Message);
                                        Console.WriteLine("Error updating document: " + ex.Message);
                                    }
                                    finally
                                    {
                                        web.AllowUnsafeUpdates = false;
                                    }
                                }
                            }
                            while (query.ListItemCollectionPosition != null);
                        }
                    }
                }
                success = true;
            }
            catch (Exception ex)
            {
                success = false;
                Util.LogError("Error in ProcessWorkMatterDocuments(): " + ex.Message);
                Console.WriteLine("Error in ProcessWorkMatterDocuments(): " + ex.Message);
            }
            inProgress = false;
            return success;
        }

        public class TimerService
        {
            readonly Timer _timer;
            public TimerService()
            {
                _timer = new Timer(Settings.Default.TimerInterval) { AutoReset = true };
                _timer.Elapsed += (sender, eventArgs) => {
                    if (!inProgress)
                    {
                        Program.ProcessWorkMatterDocuments();
                    }
                };
            }
            public void Start() { _timer.Start(); }
            public void Stop() { _timer.Stop(); }
        }
    }
}
