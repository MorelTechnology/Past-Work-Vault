using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.Office.DocumentManagement;

namespace Guidewire_Integration.Features.GuidewireIntegration
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("e84fa201-8c57-4a76-bbb1-d4d67431eeab")]
    public class GuidewireIntegrationEventReceiver : SPFeatureReceiver
    {
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
                /* Activate the Guidewire Integration Provisioning Feature and Content Organizer */
                if (web.Features[new Guid(Resource.GuidewireIntegrationProvisioningFeatureGuid)] == null)
                {
                    web.Features.Add(new Guid(Resource.GuidewireIntegrationProvisioningFeatureGuid));
                }
                if (web.Features[new Guid(Resource.DocumentRoutingFeatureGuid)] == null)
                {
                    web.Features.Add(new Guid(Resource.DocumentRoutingFeatureGuid));
                }
            }
            using (SPSite site = GetSite(properties))
            {
                /* Activate the Document ID service and set the ID provider to the custom Guidewire provider. Activate In Place Records Management */
                if (site.Features[new Guid(Resource.DocIdFeatureGuid)] == null)
                {
                    site.Features.Add(new Guid(Resource.DocIdFeatureGuid));
                }
                if (site.Features[new Guid(Resource.InPlaceRecordsFeatureGuid)] == null)
                {
                    site.Features.Add(new Guid(Resource.InPlaceRecordsFeatureGuid));
                }
                DocumentId.SetProvider(site, new GuidewireDocumentIDProvider());
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.
        // This seems like overkill. We want to be able to deactivate this without spiraling the SPSite into the 7th circle of hell
        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
        //    using (SPWeb web = GetWeb(properties))
        //    {
        //        // Deactivate the Guidewire Integration Provisioning feature, set the document ID provider back to default
        //        foreach (SPFeatureDefinition featureDef in web.FeatureDefinitions)
        //        {
        //            if (featureDef.DisplayName == "Guidewire Integration - Provision Lists" && featureDef.Scope == SPFeatureScope.Web)
        //            {
        //                Guid featureGuid = featureDef.Id;
        //                web.Features.Remove(featureGuid);
        //            }
        //        }
        //    }
        //    using (SPSite site = GetSite(properties))
        //    {
        //        DocumentId.SetDefaultProvider(site);
        //    }
        //}


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
