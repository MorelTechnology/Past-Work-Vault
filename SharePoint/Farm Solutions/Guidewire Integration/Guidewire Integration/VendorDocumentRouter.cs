using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.RecordsManagement.RecordsRepository;
using System.IO;
using Microsoft.SharePoint;
using System.Collections;
using Microsoft.SharePoint.Utilities;

namespace Guidewire_Integration
{
    public class VendorDocumentRouter : ICustomRouter
    {
        CustomRouterResult ICustomRouter.OnSubmitFile(EcmDocumentRoutingWeb web, string recordSeries, string userName, Stream fileContent, RecordsRepositoryProperty[] properties, SPFolder finalFolder, ref string resultDetails)
        {
            CustomRouterResult result = new CustomRouterResult();
            try
            {
                if (web == null)
                    throw new ArgumentNullException("web");
                if (!web.IsRoutingEnabled)
                    throw new ArgumentException("Invalid content organizer.");

                string submitterLoginName = Resource.DefaultAdminAccount;

                string submittingUserName = userName;
                if (string.IsNullOrEmpty(userName))
                {
                    submittingUserName = submitterLoginName;
                }

                using (SPSite site = new SPSite(web.DropOffZoneUrl))
                {
                    using (SPWeb rootWeb = site.OpenWeb())
                    {
                        SPUser submittingUser = rootWeb.SiteUsers[submittingUserName];
                        Hashtable fileProperties = EcmDocumentRouter.GetHashtableForRecordsRepositoryProperties(properties, recordSeries);
                        fileProperties["Title"] = fileProperties["Name"];
                        string modifiedFileName = fileProperties["BaseName"] + " - " + Util.RemoveMetaCharacters(fileProperties["Vendor"].ToString()) + "." + fileProperties["File Type"];

                        SPFile existingFile = rootWeb.GetFile(finalFolder.ServerRelativeUrl + "/" + modifiedFileName);
                        if (existingFile.Exists)
                        {
                            if ((existingFile.Item.Fields.ContainsField("Vendor_x0020_Document_x003a__x0020_Status") && existingFile.Item["Vendor_x0020_Document_x003a__x0020_Status"].ToString() == "Final") || Records.IsRecord(existingFile.Item))
                            {
                                result = CustomRouterResult.SuccessCancelFurtherProcessing;
                                resultDetails = "There was an error processing this document. It has already been added to the specified contact and has been marked as final. The file cannot be overwritten.";
                                SPUtility.TransferToErrorPage(resultDetails);
                                return result;
                            }
                            fileProperties[Resource.FieldDocumentIDString] = existingFile.Item[Resource.FieldDocumentIDString];
                        }

                        SPFile newFile = EcmDocumentRouter.SaveFileToFinalLocation(web, finalFolder, fileContent, modifiedFileName, "", fileProperties, submittingUser, false, "");
                        SPListItem newItem = newFile.Item;
                        if (newFile.Name != modifiedFileName)
                        {
                            result = CustomRouterResult.SuccessCancelFurtherProcessing;
                            resultDetails = "There was an error processing this document. It has already been added to the specified contact and has been marked as final. The file cannot be overwritten.";
                            // We shouldn't delete these documents. We can troubleshoot stuck documents, but let's not delete them
                            //using (DisabledEventsScope scope = new DisabledEventsScope())
                            //{
                            //    newItem.Delete();
                            //}
                            SPUtility.TransferToErrorPage(resultDetails);
                            return result;
                        }

                        newItem["Modified"] = DateTime.Now;
                        newItem.UpdateOverwriteVersion();
                        result = CustomRouterResult.SuccessCancelFurtherProcessing;
                    }
                }
            }
            catch (Exception ex)
            {
                Util.LogError("OnSubmitFile failed with message: " + ex.Message);
            }
            return result;
        }
    }
}
