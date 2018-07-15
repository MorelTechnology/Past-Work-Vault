using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.SharePoint;
using Microsoft.BusinessData.Runtime;
using Microsoft.BusinessData.MetadataModel;
using Microsoft.SharePoint.BusinessData.SharedService;
using Microsoft.SharePoint.Administration;
using Microsoft.SharePoint.BusinessData.Infrastructure;
using Microsoft.BusinessData.MetadataModel.Collections;
using System.Globalization;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using System.Xml.Linq;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace Link_GW_Docs
{
    class Util
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
                    var appLog = new EventLog { Source = Resource.ErrorSource };
                    appLog.WriteEntry(message, logErrorLevel, 42);
                }
                catch (Exception e)
                {
                    Microsoft.Office.Server.Diagnostics.PortalLog.LogString("Guidewire Integration errored: " + e.Message + " Original message: " + message);
                }
            });
        }

        public static String GetDocumentId(SPListItem listItem)
        {
            string documentId = string.Empty;
            if (listItem.Fields.ContainsField(Resource.FieldDocumentId) && listItem.Fields[Resource.FieldDocumentId].Type == SPFieldType.URL)
            {
                SPFieldUrlValue urlValue = new SPFieldUrlValue((string)listItem[Resource.FieldDocumentId]);
                documentId = urlValue.Description;
            }
            return documentId;
        }

        public static void SetBCSField(SPListItem item, SPBusinessDataField bcsField, string value, string finderMethodName)
        {
            try
            {
                IEntityInstance entityInst = GetEntityInstance(bcsField, value, item.ParentList.ParentWeb.Site, item, finderMethodName);
                if (entityInst != null)
                {
                    SetSecondaryFields(item, bcsField, entityInst, finderMethodName);
                    //item.UpdateOverwriteVersion();
                    item.SystemUpdate(false);
                }
                else throw new NullReferenceException("Null was returned from BCS FindSpecific method");
            }
            catch(NullReferenceException e)
            {
                LogError("SetBCSField errored with message " + e.Message, ErrorLevel.Warning);
                throw;
            }
            catch (Exception e)
            {
                LogError("SetBCSField errored with message " + e.Message, ErrorLevel.Warning);
                throw;
            }
        }

        private static IEntityInstance GetEntityInstance(SPBusinessDataField dataField, string entityId, SPSite site, SPListItem item, string finderMethodName)
        {
            IEntityInstance entInstance = null;
            try
            {
                IEntity entity = GetEntity(site, dataField);
                ILobSystemInstance lobSystemInstance = entity.GetLobSystem().GetLobSystemInstances()[0].Value;

                // Get methods collection
                foreach (KeyValuePair<string, IMethod> method in entity.GetMethods())
                {
                    // Get current method's instance
                    IMethodInstance methodInstance = method.Value.GetMethodInstances()[method.Key];
                    // Execute specific finder method
                    if (methodInstance.MethodInstanceType == MethodInstanceType.SpecificFinder && methodInstance.Name == finderMethodName)
                    {
                        Identity id = null;

                        if (EntityInstanceIdEncoder.IsEncodedIdentifier(entityId))
                        {
                            object[] oIDList = EntityInstanceIdEncoder.DecodeEntityInstanceId(entityId);
                            id = new Identity(oIDList[0]);

                            // Execute specific finder method and get the entity instance
                            entInstance = entity.FindSpecific(id, methodInstance.Name, entity.GetLobSystem().GetLobSystemInstances()[0].Value);
                            item[dataField.RelatedField] = entityId.ToString();
                        }
                        else
                        {
                            object oID = GetTypedIDValue(entityId, entity);
                            id = new Identity(oID);
                            string encodedIdentifier = EntityInstanceIdEncoder.EncodeEntityInstanceId(new object[] { oID });
                            // Execute specific finder method and get the entity instance
                            entInstance = entity.FindSpecific(id, methodInstance.Name, entity.GetLobSystem().GetLobSystemInstances()[0].Value);
                            item[dataField.RelatedField] = encodedIdentifier;
                        }
                    }
                }
            }
            catch (ObjectNotFoundException notFoundException)
            {
                LogError("GetEntityInstance errored with message " + notFoundException.Message + ". Adding item to Guidewire.");
                Console.WriteLine("GetEntityInstance errored with message " + notFoundException.Message + ". Adding item to Guidewire.");
                bool addDocumentToGuidewire = CallGuidewire(item, GuidewireOperationType.New);
                string outMessage = "";
                if (addDocumentToGuidewire)
                {
                    outMessage = string.Format("Item with ID {0} added to Guidewire", item.ID);
                } else
                {
                    outMessage = string.Format("Item with ID {0} could not be added to Guidewire", item.ID);
                    if (Settings.Default.DeleteFailures)
                    {
                        try
                        {
                            // Recycle the item if it can't be added to guidewire and it's older than 30 days
                            if (DateTime.Now.AddDays(-30) > (DateTime)item[SPBuiltInFieldId.Modified])
                            {
                                item.Recycle();
                            }
                        }
                        catch
                        {
                            // Swallow this error. The item doesn't exist in Guidewire and can't be deleted in SharePoint. It has problems
                        }
                    }
                    LogError(outMessage);
                }
                Console.WriteLine(outMessage);
            }
            catch (Exception ex)
            {
                // Swallow this error
                LogError("GetEntityInstance errored with message " + ex.Message);
                Console.WriteLine("GetEntityInstance errored with message " + ex.Message);
            }
            return entInstance;
        }

        private static IEntity GetEntity(SPSite site, SPBusinessDataField dataField)
        {
            IEntity result = null;
            try
            {
                SPServiceContext context = /*SPServiceContext.GetContext(site);*/SPServiceContext.Current;
                IMetadataCatalog catalog = null;
                BdcService bdcService = SPFarm.Local.Services.GetValue<BdcService>(String.Empty);
                if (bdcService != null && dataField != null)
                {
                    catalog = bdcService.GetDatabaseBackedMetadataCatalog(context);
                    if (catalog != null)
                        result = catalog.GetEntity(dataField.EntityNamespace, dataField.EntityName);
                }
            }
            catch (Exception e)
            {
                LogError("GetEntity failed with message " + e.Message);
            }
            return result;
        }

        private static object GetTypedIDValue(string sID, IEntity oEntity)
        {
            object oID = null;
            try
            {
                IIdentifierCollection type = oEntity.GetIdentifiers();
                String sIdentifierType = type[0].IdentifierType.FullName.ToLower().Replace("system.", String.Empty);

                // Find the instance value based on the given identifier type
                switch (sIdentifierType)
                {
                    case "string":
                        oID = sID;
                        break;
                    case "datetime":
                        oID = DateTime.Parse(sID, CultureInfo.CurrentCulture);
                        break;
                    case "boolean":
                        oID = Boolean.Parse(sID);
                        break;
                    case "int64":
                        oID = Int64.Parse(sID);
                        break;
                    case "int32":
                        oID = Int32.Parse(sID);
                        break;
                    case "int16":
                        oID = Int16.Parse(sID);
                        break;
                    case "double":
                        oID = Double.Parse(sID);
                        break;
                    case "char":
                        oID = Char.Parse(sID);
                        break;
                    case "guid":
                        oID = new Guid(sID);
                        break;
                    default:
                        oID = sID;
                        break;
                }
            }
            catch (Exception e)
            {
                LogError("GetTypedIDValue errored with message " + e.Message);
            }
            return oID;
        }

        private static void SetSecondaryFields(SPListItem listItem, SPBusinessDataField dataField, IEntityInstance entityInstance, string finderMethod)
        {
            try
            {
                // Convert the entity to a formatted datatable
                System.Data.DataTable dtBDCData = entityInstance.EntityAsFormattedDataTable;

                // Set the BCS field itself (Display Value)
                listItem[dataField.Id] = dtBDCData.Rows[0][dataField.BdcFieldName].ToString();

                // Get the specific finder method to get the columns that returns
                IMethodInstance method = entityInstance.Entity.GetMethodInstances(MethodInstanceType.SpecificFinder)[finderMethod];
                ITypeDescriptorCollection oDescriptors = method.GetReturnTypeDescriptor().GetChildTypeDescriptors()[0].GetChildTypeDescriptors();

                // Set the column names to the correct values
                foreach (ITypeDescriptor oType in oDescriptors)
                {
                    if (oType.ContainsLocalizedDisplayName())
                    {
                        if (dtBDCData.Columns.Contains(oType.Name))
                        {
                            dtBDCData.Columns[oType.Name].ColumnName = oType.GetLocalizedDisplayName();
                        }
                    }
                }

                // Get the secondary field display names - these should be set
                string[] sSecondaryFieldsDisplayNames = dataField.GetSecondaryFieldsNames();

                // Loop through the fields and set each column to its value
                foreach (string columnNameInt in sSecondaryFieldsDisplayNames)
                {
                    foreach (SPField field in listItem.Fields)
                    {
                        if (field.Title.StartsWith(dataField.Title) && field.GetProperty("BdcField") == columnNameInt)
                        {
                            Guid gFieldID = listItem.Fields[field.Title].Id;
                            listItem[gFieldID] = dtBDCData.Rows[0][columnNameInt].ToString();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                LogError("SetSecondaryFields errored with message " + e.Message, ErrorLevel.Warning);
                throw;
            }
        }

        public static void ClearFields(SPListItem item)
        {
            /*if (item.Fields.ContainsField(Resource.FieldCategory))
            {
                item[Resource.FieldCategory] = null;
            }
            if (item.Fields.ContainsField(Resource.FieldSubCategory))
            {
                item[Resource.FieldSubCategory] = null;
            }
            if (item.Fields.ContainsField(Resource.FieldDocumentStatus))
            {
                item[Resource.FieldDocumentStatus] = null;
            }*/
            if (item.Fields.ContainsField(Resource.FieldUpdateRequired))
            {
                item[Resource.FieldUpdateRequired] = "No";
            }
            item.SystemUpdate(false);
        }

        public static void LockItem(SPListItem item)
        {
            if (!Records.IsLocked(item))
            {
                Records.DeclareItemAsRecord(item);
            }
        }

        public static string GetViewQuery()
        {
            XDocument fileXml = XDocument.Load(@"ViewQuery.xml");
            return fileXml.ToString(SaveOptions.DisableFormatting);
        }

        public enum GuidewireOperationType
        {
            New = 1,
            Update = 2,
            Delete = 3
        }

        public static Boolean CallGuidewire(SPListItem item, GuidewireOperationType type)
        {
            Boolean success = false;
            EndpointAddress endpointAddress = new EndpointAddress(ConfigurationManager.AppSettings["GuidewireEndpoint"]);
            Binding binding = CreateBinding();
            GuidewireWS.trg_UpdateMetadataAPIPortTypeClient client = new GuidewireWS.trg_UpdateMetadataAPIPortTypeClient(binding, endpointAddress);
            GuidewireWS.authentication authentication = CreateAuthentication();
            GuidewireWS.locale locale = new GuidewireWS.locale();
            GuidewireWS.trg_DocumentInfo documentInfo = new GuidewireWS.trg_DocumentInfo();
            documentInfo.operation = (int)type;
            documentInfo.documentID = Util.GetDocumentId(item);
            List<GuidewireWS.trg_MetadataPair> metadata = CreateMetadata(item);
            if (metadata != null)
            {
                documentInfo.metadata = metadata.ToArray();
                try
                {
                    success = client.updateMetadata(authentication, locale, documentInfo);
                    if (!success)
                    {
                        Util.LogError("Call to Guidewire was unsuccessful. Operation type = " + type.ToString());
                        Console.WriteLine("Call to Guidewire was unsuccessful. Operation type = " + type.ToString());
                    }
                }
                catch (Exception e)
                {
                    success = false;
                    Util.LogError("CallGuidewire failed with exception:  " + e.Message);
                    Console.WriteLine("CallGuidewire failed with exception:  " + e.Message);
                }
            }
            else return false;

            return success;
        }

        private static Binding CreateBinding()
        {
            CustomBinding binding = new CustomBinding();
            TextMessageEncodingBindingElement encoding = new TextMessageEncodingBindingElement();
            encoding.MessageVersion = MessageVersion.Soap12;
            binding.Elements.Add(encoding);
            HttpTransportBindingElement transport = new HttpTransportBindingElement();
            binding.Elements.Add(transport);
            return binding;
        }

        private static GuidewireWS.authentication CreateAuthentication()
        {
            GuidewireWS.authentication authentication = new GuidewireWS.authentication()
            {
                username = "GWSharePointIntegration",
                password = "gw"
            };
            return authentication;
        }

        private static List<GuidewireWS.trg_MetadataPair> CreateMetadata(SPListItem item)
        {
            List<GuidewireWS.trg_MetadataPair> metadata = new List<GuidewireWS.trg_MetadataPair>();
            CreateMetadataPair(metadata, "name", "Title", item);
            if (item.Fields.ContainsField("Work Matter"))
                CreateMetadataPair(metadata, "workmatter", "Work Matter", item);
            else
                CreateMetadataPair(metadata, "trg_ContactID", "Vendor", item);

            CreateMetadataPair(metadata, "author", "Created By", item);

            if (item.Fields.ContainsField("Document Category") && item["Document Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_Category", "Document Category", item);
            }
            else if (item.Fields.ContainsField("Work Matter Document" + ": Category") && item["Work Matter Document" + ": Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_Category", "Work Matter Document" + ": Category", item);
            }
            else if (item.Fields.ContainsField("Vendor Document" + ": Category") && item["Vendor Document" + ": Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_Category", "Vendor Document" + ": Category", item);
            }
            if (item.Fields.ContainsField("Document Sub Category") && item["Document Sub Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_SubCategory", "Document Sub Category", item);
            }
            else if (item.Fields.ContainsField("Work Matter Document" + ": Sub Category") && item["Work Matter Document" + ": Sub Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_SubCategory", "Work Matter Document" + ": Sub Category", item);
            }
            else if (item.Fields.ContainsField("Vendor Document" + ": Sub Category") && item["Vendor Document" + ": Sub Category"] != null)
            {
                CreateMetadataPair(metadata, "trg_SubCategory", "Vendor Document" + ": Sub Category", item);
            }
            if (item.Fields.ContainsField("Document Status") && item["Document Status"] != null)
            {
                CreateMetadataPair(metadata, "status", "Document Status", item);
            }
            else if (item.Fields.ContainsField("Work Matter Document" + ": Status") && item["Work Matter Document" + ": Status"] != null)
            {
                CreateMetadataPair(metadata, "status", "Work Matter Document" + ": Status", item);
            }
            else if (item.Fields.ContainsField("Vendor Document" + ": Status") && item["Vendor Document" + ": Status"] != null)
            {
                CreateMetadataPair(metadata, "status", "Vendor Document" + ": Status", item);
            }
            if (item.Fields.ContainsField("Description") && item["Description"] != null)
            {
                CreateMetadataPair(metadata, "description", "Description", item);
            }
            else if (item.Fields.ContainsField("Work Matter Document" + ": Description") && item["Work Matter Document" + ": Description"] != null)
            {
                CreateMetadataPair(metadata, "description", "Work Matter Document" + ": Description", item);
            }
            else if (item.Fields.ContainsField("Vendor Document" + ": Description") && item["Vendor Document" + ": Description"] != null)
            {
                CreateMetadataPair(metadata, "description", "Vendor Document" + ": Description", item);
            }

            CreateMetadataPair(metadata, "mimetype", "File Type", item);
            CreateMetadataPair(metadata, "dateModified", "Modified", item);
            CreateMetadataPair(metadata, "dateCreated", "Created", item);

            return metadata;
        }

        private static void CreateMetadataPair(List<GuidewireWS.trg_MetadataPair> metadata, string key, string propertyName, SPListItem item)
        {
            try
            {
                GuidewireWS.trg_MetadataPair metadataPair = new GuidewireWS.trg_MetadataPair();
                //SPListItem listItem = properties.ListItem;
                SPListItem listItem = item;
                if (listItem.Fields.ContainsField(propertyName))
                {
                    metadataPair.key = key;
                    SPField propertyField = listItem.Fields[propertyName];
                    switch (propertyField.Type)
                    {
                        case SPFieldType.User:
                            {
                                SPFieldUser userField = propertyField as SPFieldUser;
                                SPFieldUserValue userFieldValue = (SPFieldUserValue)userField.GetFieldValue(listItem[propertyName].ToString());
                                if (userFieldValue != null && userFieldValue.User != null)
                                {
                                    metadataPair.value = userFieldValue.User.Name;
                                }
                                else
                                    metadataPair.value = (string)listItem[propertyName];
                            }
                            break;
                        case SPFieldType.Lookup:
                            {
                                SPFieldLookup lookupField = propertyField as SPFieldLookup;
                                SPFieldLookupValue lookupFieldValue = (SPFieldLookupValue)lookupField.GetFieldValue(listItem[propertyName].ToString());
                                if (lookupFieldValue != null && lookupFieldValue.LookupValue != null)
                                {
                                    metadataPair.value = lookupFieldValue.LookupValue;
                                }
                            }
                            break;
                        // Bamboo selectors have type of invalid but work like lookup
                        case SPFieldType.Invalid:
                            {
                                if (listItem[propertyName] != null)
                                {
                                    if (propertyField.TypeAsString == "BusinessData")
                                    {
                                        metadataPair.value = (string)listItem[propertyName];
                                    }
                                    else
                                    {
                                        SPFieldLookup lookupField = propertyField as SPFieldLookup;
                                        SPFieldLookupValue lookupFieldValue = (SPFieldLookupValue)lookupField.GetFieldValue(listItem[propertyName].ToString());
                                        if (lookupFieldValue != null && lookupFieldValue.LookupValue != null)
                                        {
                                            metadataPair.value = lookupFieldValue.LookupValue;
                                        }
                                    }
                                }
                            }
                            break;
                        case SPFieldType.DateTime:
                            {
                                metadataPair.value = listItem[propertyName].ToString();
                            }
                            break;
                        default:
                            if (listItem[propertyName] != null)
                            {
                                if (propertyName == "Name")
                                {
                                    String fileName = (string)listItem[propertyName];
                                    if (fileName.Length > 80)
                                    {
                                        String extension = fileName.Substring(fileName.LastIndexOf("."));
                                        String name = fileName.Substring(0, fileName.Length - extension.Length);
                                        fileName = name.Substring(0, 80 - extension.Length) + extension;
                                    }
                                    metadataPair.value = fileName;
                                }
                                else
                                    metadataPair.value = (string)listItem[propertyName];
                            }
                            break;
                    }
                    metadata.Add(metadataPair);
                }
            }
            catch (Exception e)
            {
                LogError("CreateMetadataPair failed with exception: " + e.Message);
            }
        }
    }
}
