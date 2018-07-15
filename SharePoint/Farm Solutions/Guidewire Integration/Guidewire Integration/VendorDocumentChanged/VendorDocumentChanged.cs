using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;

namespace Guidewire_Integration.VendorDocumentChanged
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class VendorDocumentChanged : SPItemEventReceiver
    {
        private Util.GuidewireOperationType _operationType;

        /// <summary>
        /// An item is updating
        /// </summary>
        public override void ItemUpdating(SPItemEventProperties properties)
        {
            // TODO: Comment this
            try
            {
                properties.AfterProperties[Resource.FieldUpdateRequired] = "Yes";
            }
            catch(Exception ex)
            {
                Util.LogError(string.Format("ItemUpdating failed in Vendor Documents library for item {0}. Exception: {1}", properties.ListItemId, ex.Message));
                properties.ErrorMessage = ex.Message;
                properties.Status = SPEventReceiverStatus.CancelWithError;
            }
        }

        /// <summary>
        /// An item was deleted.
        /// </summary>
        public override void ItemDeleted(SPItemEventProperties properties)
        {
            base.ItemDeleted(properties);
            if (properties.ListItem != null && properties.List != null && properties.ListItem.File != null)
            {
                bool success = Util.CallGuideWire(properties.ListItem, Util.GuidewireOperationType.Delete);
            }
        }
    }
}