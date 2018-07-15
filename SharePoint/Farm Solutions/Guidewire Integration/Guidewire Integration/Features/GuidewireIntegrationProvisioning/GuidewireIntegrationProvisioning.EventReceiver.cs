using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.WebControls;

namespace Guidewire_Integration.Features.Feature1
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("fb090c23-52bd-407a-b39a-2a3edb105173")]
    public class Feature1EventReceiver : SPFeatureReceiver
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

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            using (SPWeb web = GetWeb(properties))
            {
                /* Create site columns */
                if (!web.Fields.ContainsField(Resource.FieldDocumentStatus))
                {
                    string documentStatus = web.Fields.Add(Resource.FieldDocumentStatus, SPFieldType.Choice, false);
                    SPFieldChoice documentStatusField = (SPFieldChoice)web.Fields.GetFieldByInternalName(documentStatus);
                    documentStatusField.Choices.Add("Draft");
                    documentStatusField.DefaultValue = "Draft";
                    documentStatusField.Choices.Add("Final");
                    documentStatusField.Group = Resource.RiverstoneColumnGroup;
                    documentStatusField.Update();
                }
                
                /* Create work matter library */
                SPContentType workMatterDocumentContentType = CreateWorkMatterDocumentContentType(web);
                CreateWorkMatterDocumentsLibrary(web, workMatterDocumentContentType);

                /* Create vendor library */
                SPContentType vendorDocumentContentType = CreateVendorContentType(web);
                CreateVendorDocumentsLibrary(web, vendorDocumentContentType);

                /* Create configuration list and category/subcategory list */
                CreateCategoryList(web);
                CreateConfigurationList(web);
                CreateSpecialPermissions(web);
            }
        }

        private SPContentType CreateWorkMatterDocumentContentType(SPWeb web)
        {
            // Create the content type
            // Get a content type to be the parent of a new work matter document content type
            SPContentType documentCType = web.AvailableContentTypes[SPBuiltInContentTypeId.Document];
            SPContentType workMatterDocumentContentType = web.ContentTypes[Resource.WorkMatterDocumentContentType];
            if (workMatterDocumentContentType == null)
            {
                workMatterDocumentContentType = new SPContentType(documentCType, web.ContentTypes, Resource.WorkMatterDocumentContentType);
                workMatterDocumentContentType = web.ContentTypes.Add(workMatterDocumentContentType);
                workMatterDocumentContentType.Group = Resource.RiverStoneContentTypeGroup;
            }

            AddFieldLink(web, workMatterDocumentContentType, Resource.FieldDocumentStatus, true);

            // Commit changes
            workMatterDocumentContentType.Update(false, false);

            return workMatterDocumentContentType;
        }

        private SPContentType CreateVendorContentType(SPWeb web)
        {
            // Create the content type
            // Get a content type to be the parent of a new vendor document content type
            SPContentType documentCType = web.AvailableContentTypes[SPBuiltInContentTypeId.Document];
            SPContentType vendorDocumentContentType = web.ContentTypes[Resource.VendorDocumentContentType];
            if (vendorDocumentContentType == null)
            {
                vendorDocumentContentType = new SPContentType(documentCType, web.ContentTypes, Resource.VendorDocumentContentType);
                vendorDocumentContentType = web.ContentTypes.Add(vendorDocumentContentType);
                vendorDocumentContentType.Group = Resource.RiverStoneContentTypeGroup;
            }

            AddFieldLink(web, vendorDocumentContentType, Resource.FieldDocumentStatus, true);

            // Commit changes
            vendorDocumentContentType.Update(false, false);
            return vendorDocumentContentType;
        }

        private void CreateWorkMatterDocumentsLibrary(SPWeb web, SPContentType workMatterDocumentContentType)
        {
            SPList workMatterDocumentsLibrary = web.Lists.TryGetList(Resource.WorkMatterDocumentsLibrary);
            if (workMatterDocumentsLibrary == null)
            {
                // Create library
                Guid listGuid = web.Lists.Add(Resource.WorkMatterDocumentsLibrary, Resource.WorkMatterDocumentsLibraryDesc, SPListTemplateType.DocumentLibrary);
                workMatterDocumentsLibrary = web.Lists[listGuid];
                workMatterDocumentsLibrary.ContentTypesEnabled = true;
                workMatterDocumentsLibrary.EnableVersioning = true;
                workMatterDocumentsLibrary.OnQuickLaunch = true;
                workMatterDocumentsLibrary.ContentTypes.Add(workMatterDocumentContentType);
                workMatterDocumentsLibrary.ContentTypes["Document"].Delete();
                workMatterDocumentsLibrary.Update();
            }

            if (!workMatterDocumentsLibrary.Fields.ContainsField("Work Matter"))
            {
                // Create the hidden note field needed for BCS
                SPField noteWorkMatterField = workMatterDocumentsLibrary.Fields.CreateNewField("Note", "Work Matter_ID");
                noteWorkMatterField.Hidden = true;
                noteWorkMatterField.ReadOnlyField = true;
                noteWorkMatterField.SetCustomProperty("BdcField", "Work Matter_ID");
                noteWorkMatterField.StaticName = "Work_x0020_Matter_ID";
                workMatterDocumentsLibrary.Fields.Add(noteWorkMatterField);

                // Create the work matter field using BCS
                SPBusinessDataField workMatterField = workMatterDocumentsLibrary.Fields.CreateNewField("BusinessData", "Work Matter") as SPBusinessDataField;
                workMatterField.SystemInstanceName = "Guidewire";
                workMatterField.EntityNamespace = "Guidewire";
                workMatterField.EntityName = "Work Matter";
                workMatterField.StaticName = "Work_x0020_Matter";
                workMatterField.BdcFieldName = "ClaimNumber";
                workMatterField.HasActions = false;
                workMatterField.Required = true;
                workMatterField.RelatedField = "Work_x0020_Matter_ID";
                workMatterDocumentsLibrary.Fields.Add(workMatterField);
            }

            if (!workMatterDocumentsLibrary.Fields.ContainsField("Work Matter Document"))
            {
                // Create the hidden note field needed for BCS
                SPField noteWorkMatterDocumentField = workMatterDocumentsLibrary.Fields.CreateNewField("Note", "Work Matter Document_ID");
                noteWorkMatterDocumentField.Hidden = true;
                noteWorkMatterDocumentField.ReadOnlyField = true;
                noteWorkMatterDocumentField.SetCustomProperty("BdcField", "Work Matter Document_ID");
                noteWorkMatterDocumentField.StaticName = "Work_x0020_Matter_x0020_Document_ID";
                workMatterDocumentsLibrary.Fields.Add(noteWorkMatterDocumentField);

                // Create the work matter document field using BCS
                SPBusinessDataField workMatterDocumentField = workMatterDocumentsLibrary.Fields.CreateNewField("BusinessData", "Work Matter Document") as SPBusinessDataField;
                workMatterDocumentField.SystemInstanceName = "Guidewire";
                workMatterDocumentField.EntityNamespace = "Guidewire";
                workMatterDocumentField.EntityName = "Work Matter Document";
                workMatterDocumentField.StaticName = "Work_x0020_Matter_x0020_Document";
                workMatterDocumentField.BdcFieldName = "ClaimNumber";
                workMatterDocumentField.HasActions = false;
                workMatterDocumentField.RelatedField = "Work_x0020_Matter_x0020_Document_ID";
                workMatterDocumentField.SetSecondaryFieldsNames(new string[]{"AccountName","Category","Status","Subcategory","WorkMatterDesc","ClaimNumber","PublicID"});
                workMatterDocumentsLibrary.Fields.Add(workMatterDocumentField);
            }
        }

        private void CreateVendorDocumentsLibrary(SPWeb web, SPContentType vendorDocumentContentType)
        {
            SPList vendorDocumentsLibrary = web.Lists.TryGetList(Resource.VendorDocumentsLibrary);
            if (vendorDocumentsLibrary == null)
            {
                // Create library
                Guid listGuid = web.Lists.Add(Resource.VendorDocumentsLibrary, Resource.VendorDocumentsLibraryDesc, SPListTemplateType.DocumentLibrary);
                vendorDocumentsLibrary = web.Lists[listGuid];
                vendorDocumentsLibrary.ContentTypesEnabled = true;
                vendorDocumentsLibrary.EnableVersioning = true;
                vendorDocumentsLibrary.OnQuickLaunch = true;
                vendorDocumentsLibrary.ContentTypes.Add(vendorDocumentContentType);
                vendorDocumentsLibrary.ContentTypes["Document"].Delete();
                vendorDocumentsLibrary.Update();
            }

            if (!vendorDocumentsLibrary.Fields.ContainsField("Vendor"))
            {
                // Create the hidden note field needed for BCS
                SPField noteVendorField = vendorDocumentsLibrary.Fields.CreateNewField("Note", "Vendor_ID");
                noteVendorField.Hidden = true;
                noteVendorField.ReadOnlyField = true;
                noteVendorField.SetCustomProperty("BdcField", "Vendor_ID");
                noteVendorField.StaticName = "Vendor_ID";
                vendorDocumentsLibrary.Fields.Add(noteVendorField);

                // Create the vendor field using BCS
                SPBusinessDataField vendorField = vendorDocumentsLibrary.Fields.CreateNewField("BusinessData", "Vendor") as SPBusinessDataField;
                vendorField.SystemInstanceName = "Guidewire";
                vendorField.EntityNamespace = "Guidewire";
                vendorField.EntityName = "Vendor";
                vendorField.StaticName = "Vendor";
                vendorField.BdcFieldName = "ContactID";
                vendorField.HasActions = false;
                vendorField.Required = true;
                vendorField.RelatedField = "Vendor_ID";
                vendorDocumentsLibrary.Fields.Add(vendorField);
            }
            if (!vendorDocumentsLibrary.Fields.ContainsField("Vendor Document"))
            {
                // Create the hidden note field needed for BCS
                SPField noteVendorDocumentField = vendorDocumentsLibrary.Fields.CreateNewField("Note", "Vendor Document_ID");
                noteVendorDocumentField.Hidden = true;
                noteVendorDocumentField.ReadOnlyField = true;
                noteVendorDocumentField.SetCustomProperty("BdcField", "Vendor Document_ID");
                noteVendorDocumentField.StaticName = "Vendor_x0020_Document_ID";

                // Create the vendor document field using BCS
                SPBusinessDataField vendorDocumentField = vendorDocumentsLibrary.Fields.CreateNewField("BusinessData", "Vendor Document") as SPBusinessDataField;
                vendorDocumentField.SystemInstanceName = "Guidewire";
                vendorDocumentField.EntityNamespace = "Guidewire";
                vendorDocumentField.EntityName = "Vendor Document";
                vendorDocumentField.StaticName = "Vendor_x0020_Document";
                vendorDocumentField.BdcFieldName = "ContactID";
                vendorDocumentField.HasActions = false;
                vendorDocumentField.RelatedField = "Vendor_x0020_Document_ID";
                vendorDocumentField.SetSecondaryFieldsNames(new string[] { "Category", "ContactID", "ContactName", "Status", "Subcategory","PublicID" });
                vendorDocumentsLibrary.Fields.Add(vendorDocumentField);
            }
        }

        private void CreateConfigurationList(SPWeb web)
        {
            SPList configurationList = web.Lists.TryGetList(Resource.ConfigurationList);
            if (configurationList == null)
            {
                Guid configurationListGuid = web.Lists.Add(Resource.ConfigurationList, Resource.ConfigurationListDesc, SPListTemplateType.GenericList);
                configurationList = web.Lists[configurationListGuid];
                configurationList.Fields.Add("Property", SPFieldType.Text, true);
                configurationList.Fields.Add("Setting", SPFieldType.Text, true);
                configurationList.EnableVersioning = true;

                // Only system admins should be able to edit this, but everybody needs to view it
                configurationList.BreakRoleInheritance(false);
                SPPrincipal group = web.SiteGroups["Guidewire Owners"];
                if (group != null)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(group);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Administrator);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                    configurationList.RoleAssignments.Add(roleAssignment);
                }
                configurationList.Update();

                Configuration.AddDefaultConfigurationValue(web, Resource.ConfigGWWsUserName, "GWSharePointIntegration");
                Configuration.AddDefaultConfigurationValue(web, Resource.ConfigGWWsPassword, "gw");
                Configuration.AddDefaultConfigurationValue(web, Resource.ConfigGWWsEndpoint, "http://manclredev02:8080/cc/ws/riverstone/webservice/document/trg_UpdateMetadataAPI");
            }
        }

        private void CreateCategoryList(SPWeb web)
        {
            SPList categoryList = web.Lists.TryGetList(Resource.CategoryList);
            if (categoryList == null)
            {
                Guid categoryListGuid = web.Lists.Add(Resource.CategoryList, Resource.CategoryListDesc, SPListTemplateType.GenericList);
                categoryList = web.Lists[categoryListGuid];
                categoryList.Fields.Add(Resource.FieldCategory, SPFieldType.Text, true);
                categoryList.Fields.Add(Resource.FieldSubCategory, SPFieldType.Text, true);
                categoryList.EnableVersioning = true;

                // Only system admins should be able to edit this, but everybody needs to view it
                categoryList.BreakRoleInheritance(false);
                SPPrincipal visitor = web.SiteGroups["Guidewire Visitors"];
                if (visitor != null)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(visitor);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                    categoryList.RoleAssignments.Add(roleAssignment);
                }
                SPPrincipal member = web.SiteGroups["Guidewire Members"];
                if (member != null)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(member);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Reader);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                    categoryList.RoleAssignments.Add(roleAssignment);
                }
                SPPrincipal owner = web.SiteGroups["Guidewire Owners"];
                if (member != null)
                {
                    SPRoleAssignment roleAssignment = new SPRoleAssignment(owner);
                    SPRoleDefinition roleDefinition = web.RoleDefinitions.GetByType(SPRoleType.Administrator);
                    roleAssignment.RoleDefinitionBindings.Add(roleDefinition);
                    categoryList.RoleAssignments.Add(roleAssignment);
                }

                categoryList.Update();
            }
        }

        /// <summary>
        /// Creates special permission that allows users to add documents but not delete
        /// </summary>
        /// <param name="web"></param>
        private void CreateSpecialPermissions(SPWeb web)
        {
            if (!web.RoleDefinitions.Cast<SPRoleDefinition>().Any(item => item.Name == Resource.SpecialPermission))
            {
                SPRoleDefinition role = new SPRoleDefinition();
                role.Name = Resource.SpecialPermission;
                role.Description = Resource.SpecialPermissionDesc;
                role.BasePermissions = SPBasePermissions.AddListItems | SPBasePermissions.EditListItems | SPBasePermissions.ViewListItems | SPBasePermissions.OpenItems
                    | SPBasePermissions.ViewVersions | SPBasePermissions.CreateAlerts | SPBasePermissions.ViewFormPages | SPBasePermissions.BrowseDirectories | SPBasePermissions.ViewPages
                    | SPBasePermissions.BrowseUserInfo | SPBasePermissions.UseRemoteAPIs | SPBasePermissions.UseClientIntegration | SPBasePermissions.Open | SPBasePermissions.EditMyUserInfo
                    | SPBasePermissions.ManagePersonalViews;
                
                if (!web.IsRootWeb)
                    web.ParentWeb.RoleDefinitions.Add(role);
                else
                    web.RoleDefinitions.Add(role);
            }
        }

        private bool AddFieldLink(SPWeb web, SPContentType contentType, string fieldName, bool required)
        {
            bool fieldLinkAdded = false;
            SPField field = web.Fields[fieldName];
            if (field != null && !contentType.Fields.ContainsField(fieldName))
            {
                SPFieldLink link = new SPFieldLink(field);
                link.Required = required;
                contentType.FieldLinks.Add(link);
                fieldLinkAdded = true;
            }
            return fieldLinkAdded;
        }


        public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        {
            using (SPWeb web = GetWeb(properties))
            {
                web.AllowUnsafeUpdates = true;
                /* Delete any fields here */

                SPList workMatterDocumentsLibrary = web.Lists.TryGetList(Resource.WorkMatterDocumentsLibrary);
                if (workMatterDocumentsLibrary != null && workMatterDocumentsLibrary.ItemCount == 0)
                {
                    workMatterDocumentsLibrary.Delete();
                    SPContentType workMatterDocumentContentType = web.ContentTypes[Resource.WorkMatterDocumentContentType];
                    web.ContentTypes.Delete(workMatterDocumentContentType.Id);
                }

                SPList vendorDocumentsLibrary = web.Lists.TryGetList(Resource.VendorDocumentsLibrary);
                if (vendorDocumentsLibrary != null && vendorDocumentsLibrary.ItemCount == 0)
                {
                    vendorDocumentsLibrary.Delete();
                    SPContentType vendorDocumentContentType = web.ContentTypes[Resource.VendorDocumentContentType];
                    web.ContentTypes.Delete(vendorDocumentContentType.Id);
                }

                SPList configurationList = web.Lists.TryGetList(Resource.ConfigurationList);
                if (configurationList != null && configurationList.ItemCount == 0)
                {
                    configurationList.Delete();
                }

                SPList categoryList = web.Lists.TryGetList(Resource.CategoryList);
                if (categoryList != null && categoryList.ItemCount == 0)
                {
                    categoryList.Delete();
                }

                web.AllowUnsafeUpdates = false;
                web.Update();
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
