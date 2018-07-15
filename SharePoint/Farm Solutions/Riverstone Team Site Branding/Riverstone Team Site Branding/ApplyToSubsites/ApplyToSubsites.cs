using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using Microsoft.SharePoint.Publishing;

namespace Riverstone_Team_Site_Branding.ApplyToSubsites
{
    /// <summary>
    /// Web Events
    /// </summary>
    public class ApplyToSubsites : SPWebEventReceiver
    {
        /// <summary>
        /// A site was provisioned.
        /// </summary>
        public override void WebProvisioned(SPWebEventProperties properties)
        {
            base.WebProvisioned(properties);
            SPSite siteCollection = properties.Web.Site;
            SPWeb subSite = properties.Web;

            if (!subSite.WebTemplate.Contains("APP"))
            {
                Guid sitePublishingGuid = new Guid("f6924d36-2fa8-4f0b-b16d-06b7250180fa");
                Guid webPublishingGuid = new Guid("94c94ca6-b32f-4da9-a9e3-1f3d343d7ecb");

                EnsureSiteFeatureActivated(sitePublishingGuid, siteCollection);
                EnsureWebFeatureActivated(webPublishingGuid, siteCollection, subSite);

                var pubWeb = PublishingWeb.GetPublishingWeb(subSite);
                var webNavigationSettings = new Microsoft.SharePoint.Publishing.Navigation.WebNavigationSettings(subSite);
                webNavigationSettings.GlobalNavigation.Source = Microsoft.SharePoint.Publishing.Navigation.StandardNavigationSource.InheritFromParentWeb;
                webNavigationSettings.CurrentNavigation.Source = Microsoft.SharePoint.Publishing.Navigation.StandardNavigationSource.PortalProvider;
                webNavigationSettings.Update();
                pubWeb.Update();

                subSite.MasterUrl = siteCollection.RootWeb.MasterUrl;
                subSite.CustomMasterUrl = siteCollection.RootWeb.CustomMasterUrl;
                subSite.SiteLogoUrl = siteCollection.RootWeb.SiteLogoUrl;
                subSite.Update();
            }
        }

        private void EnsureSiteFeatureActivated(Guid featureGuid, SPSite lSite)
        {
            using (SPSite site = new SPSite(lSite.ID))
            {
                site.AllowUnsafeUpdates = true;
                SPFeature feature = null;
                try
                {
                    feature = site.Features[featureGuid];
                }
                catch
                {
                    site.Features.Add(featureGuid);
                }
                site.AllowUnsafeUpdates = false;
            }
        }

        private void EnsureWebFeatureActivated(Guid featureGuid, SPSite lSite, SPWeb lWeb)
        {
            using (SPSite site = new SPSite(lSite.ID))
            {
                using (SPWeb web = site.OpenWeb(lWeb.ID))
                {
                    site.AllowUnsafeUpdates = true;
                    web.AllowUnsafeUpdates = true;
                    SPFeature feature = null;
                    try
                    {
                        feature = web.Features[featureGuid];
                    }
                    catch
                    {
                        web.Features.Add(featureGuid);
                    }
                    site.AllowUnsafeUpdates = false;
                    web.AllowUnsafeUpdates = false;
                }
            }
        }
    }
}