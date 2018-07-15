using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Publishing;

namespace Riverstone_Team_Site_Branding.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("9c39a61c-7e5e-4442-acbb-c483ddf4e1c0")]
    public class Feature1EventReceiver : SPFeatureReceiver
    {
        // Uncomment the method below to handle the event raised after a feature has been activated.

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPSite siteCollection = properties.Feature.Parent as SPSite;
            if (siteCollection != null)
            {
                SPWeb topLevelSite = siteCollection.RootWeb;

                Guid sitePublishingGuid = new Guid("f6924d36-2fa8-4f0b-b16d-06b7250180fa");
                Guid webPublishingGuid = new Guid("94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb");

                if (siteCollection.Features[sitePublishingGuid] == null)
                {
                    siteCollection.Features.Add(sitePublishingGuid);
                }
                String WebAppRelativePath = topLevelSite.ServerRelativeUrl;
                if (!WebAppRelativePath.EndsWith("/"))
                {
                    WebAppRelativePath += "/";
                }

                foreach (SPWeb site in siteCollection.AllWebs)
                {
                    if (!site.WebTemplate.Contains("APP"))
                    {
                        if (site.Features[webPublishingGuid] == null)
                        {
                            site.Features.Add(webPublishingGuid);
                        }
                        var pubWeb = PublishingWeb.GetPublishingWeb(site);
                        var webNavigationSettings = new Microsoft.SharePoint.Publishing.Navigation.WebNavigationSettings(site);
                        webNavigationSettings.GlobalNavigation.Source = Microsoft.SharePoint.Publishing.Navigation.StandardNavigationSource.PortalProvider;
                        webNavigationSettings.CurrentNavigation.Source = Microsoft.SharePoint.Publishing.Navigation.StandardNavigationSource.PortalProvider;
                        webNavigationSettings.Update();
                        pubWeb.Update();
                        site.MasterUrl = WebAppRelativePath + "_catalogs/masterpage/RivernetTeamSite.master";
                        site.CustomMasterUrl = WebAppRelativePath + "_catalogs/masterpage/RivernetTeamSite.master";
                        site.SiteLogoUrl = WebAppRelativePath + "_layouts/15/images/RiverStonelogotransparent.png";
                        site.Update();
                    }
                }
            }
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            SPSite siteCollection = properties.Feature.Parent as SPSite;
            if (siteCollection != null)
            {
                SPWeb topLevelSite = siteCollection.RootWeb;
                String WebAppRelativePath = topLevelSite.ServerRelativeUrl;
                if (!WebAppRelativePath.EndsWith("/"))
                {
                    WebAppRelativePath += "/";
                }
                foreach (SPWeb site in siteCollection.AllWebs)
                {
                    if (!site.WebTemplate.Contains("APP"))
                    {
                        var pubWeb = PublishingWeb.GetPublishingWeb(site);
                        var webNavigationSettings = new Microsoft.SharePoint.Publishing.Navigation.WebNavigationSettings(site);
                        webNavigationSettings.GlobalNavigation.Source = Microsoft.SharePoint.Publishing.Navigation.StandardNavigationSource.PortalProvider;
                        webNavigationSettings.CurrentNavigation.Source = Microsoft.SharePoint.Publishing.Navigation.StandardNavigationSource.PortalProvider;
                        webNavigationSettings.Update();
                        pubWeb.Update();
                        site.MasterUrl = WebAppRelativePath + "_catalogs/masterpage/seattle.master";
                        site.CustomMasterUrl = WebAppRelativePath + "_catalogs/masterpage/seattle.master";
                        site.SiteLogoUrl = "";
                        site.Update();
                    }
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
