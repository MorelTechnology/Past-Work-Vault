using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.Office.RecordsManagement.RecordsRepository;

namespace Guidewire_Integration.Features.GuidewireContentOrganizerEvents
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("7e17cc69-83f0-47ee-89d9-458e17f9c61d")]
    public class GuidewireContentOrganizerEventsEventReceiver : SPFeatureReceiver
    {
        const string workMatterDocumentsRouterName = "Work Matter Documents Router";
        const string workMatterDocumentsRouterAssemblyName = "Guidewire Integration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=16de6da3c950bc62";
        const string workMatterdocumentsRouterClassName = "Guidewire_Integration.WorkMatterDocumentRouter";
        const string vendorDocumentsRouterName = "Vendor Documents Router";
        const string vendorDocumentsRouterAssemblyName = "Guidewire Integration, Version=1.0.0.0, Culture=neutral, PublicKeyToken=16de6da3c950bc62";
        const string vendorDocumentsRouterClassName = "Guidewire_Integration.VendorDocumentRouter";

        private SPWeb GetWeb(SPFeatureReceiverProperties properties)
        {
            SPWeb web = null;
            if (properties.Feature.Parent is SPSite)
            {
                SPSite sites = (SPSite)properties.Feature.Parent;
                web = sites.RootWeb;
            }
            else
            {
                web = (SPWeb)properties.Feature.Parent;
            }
            return web;
        }

        private SPSite GetSite(SPFeatureReceiverProperties properties)
        {
            SPSite site = null;
            if (properties.Feature.Parent is SPSite)
            {
                site = (SPSite)properties.Feature.Parent;
            }
            else
            {
                SPWeb web = (SPWeb)properties.Feature.Parent;
                site = web.Site;
            }
            return site;
        }

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (SPWeb web = GetWeb(properties))
            {
                /* Activate the custom routers for Vendor documents and Work Matter documents */
                EcmDocumentRoutingWeb contentOrganizer = new EcmDocumentRoutingWeb(web);
                try
                {
                    contentOrganizer.AddCustomRouter(workMatterDocumentsRouterName, workMatterDocumentsRouterAssemblyName, workMatterdocumentsRouterClassName);
                    contentOrganizer.AddCustomRouter(vendorDocumentsRouterName, vendorDocumentsRouterAssemblyName, vendorDocumentsRouterClassName);
                }
                catch (Exception ex)
                {
                    Util.LogError("AddCustomRouter failed with message: " + ex.Message);
                }
            }
        }


        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            using (SPWeb web = GetWeb(properties))
            {
                /* Deactivate the custom routers for Vendor documents and Work Matter documents */
                EcmDocumentRoutingWeb contentOrganizer = new EcmDocumentRoutingWeb(web);
                try
                {
                    contentOrganizer.RemoveCustomRouter(workMatterDocumentsRouterName);
                    contentOrganizer.RemoveCustomRouter(vendorDocumentsRouterName);
                }
                catch (Exception ex)
                {
                    Util.LogError("RemoveCustomRouter failed with message: " + ex.Message);
                }
            }
        }


        // Uncomment the method below to handle the event raised after a feature has been installed.

        //public override void FeatureInstalled(SPFeatureReceiverProperties properties)
        //{
        //}


        // Uncomment the method below to handle the event raised before a feature is uninstalled.

        //public override void FeatureUninstalling(SPFeatureReceiverProperties properties)
        //{
        //}

        // Uncomment the method below to handle the event raised when a feature is upgrading.

        //public override void FeatureUpgrading(SPFeatureReceiverProperties properties, string upgradeActionName, System.Collections.Generic.IDictionary<string, string> parameters)
        //{
        //}
    }
}
