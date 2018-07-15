using Microsoft.SharePoint;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharePointUtilitiesLibraryTests;
using System.Collections;
using AppsToOranges.Extensions;

namespace AppsToOranges.SharePointUtilities.Tests
{
    [TestClass()]
    public class ProvisioningTests
    {
        [TestMethod()]
        public void setWebPropertiesTest()
        {
            Hashtable properties = new Hashtable();
            properties.SetProperty("isMatterActive", "1");

            

            using (SPWeb web = new SPSite(UnitTest.Default.testWebUrl).OpenWeb())
            {
                Provisioning.setWebProperties(web, properties);
            }
        }
    }
}