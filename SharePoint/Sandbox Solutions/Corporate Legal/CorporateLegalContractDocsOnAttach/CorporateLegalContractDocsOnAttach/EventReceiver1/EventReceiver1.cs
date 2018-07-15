using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Security;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace CorporateLegalContractDocsOnAttach.EventReceiver1
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class EventReceiver1 : SPItemEventReceiver
    {
       /// <summary>
       /// An attachment was added to the item.
       /// </summary>
       public override void ItemAttachmentAdded(SPItemEventProperties properties)
       {
           base.ItemAttachmentAdded(properties);
           SPListItem item = properties.List.GetItemById(properties.ListItemId);
           SPAttachmentCollection attachments = item.Attachments;
           SPList destinationList = item.Web.Lists["Contract Documents"];
           for (int i = 0; i < attachments.Count; i++)
           {
               SPFile file = item.ParentList.ParentWeb.GetFile(item.Attachments.UrlPrefix + item.Attachments[i].ToString());
               string destinationLocation = destinationList.RootFolder.ServerRelativeUrl + "/" + file.Name;
               file.MoveTo(destinationLocation, true);
               SPFile newFile = destinationList.ParentWeb.GetFile(destinationLocation);
               SPListItem newItem = newFile.Item;
               newItem["Contract"] = item.ID;
               newItem["Title"] = newItem.File.Name;
               newItem.Update();
           }
       }


    }
}
