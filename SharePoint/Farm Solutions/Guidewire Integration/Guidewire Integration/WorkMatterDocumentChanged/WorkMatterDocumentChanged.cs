using System;
using System.Security.Permissions;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;
using Microsoft.SharePoint.Workflow;
using System.ServiceModel;
using System.IO;
using System.Collections;

namespace Guidewire_Integration.WorkMatterDocumentChanged
{
    /// <summary>
    /// List Item Events
    /// </summary>
    public class WorkMatterDocumentChanged : SPItemEventReceiver
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
            catch (Exception ex)
            {
                Util.LogError(string.Format("ItemUpdating failed in Work Matter Documents library for item {0}. Exception: {1}", properties.ListItemId, ex.Message));
                properties.ErrorMessage = ex.Message;
                properties.Status = SPEventReceiverStatus.CancelWithError;
            }
        }

        /// <summary>
        /// An item is being deleted.
        /// </summary>
        /// <param name="properties"></param>
        public override void ItemDeleting(SPItemEventProperties properties)
        {
            base.ItemDeleting(properties);
            if (properties.ListItem != null && properties.List != null && properties.ListItem.File != null)
            {
                Boolean success = Util.CallGuideWire(properties.ListItem, Util.GuidewireOperationType.Delete);
            }
        }
    }
}