using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using Microsoft.SharePoint;
using System.Linq;

namespace TFSPermissionsProvisioning.Features.ProvisionPermissions
{
    /// <summary>
    /// This class handles events raised during feature activation, deactivation, installation, uninstallation, and upgrade.
    /// </summary>
    /// <remarks>
    /// The GUID attached to this class may be used during packaging and should not be modified.
    /// </remarks>

    [Guid("739720ca-396a-4e5c-bba8-8fe10ce18735")]
    public class ProvisionPermissionsEventReceiver : SPFeatureReceiver
    {

        public override void FeatureActivated(SPFeatureReceiverProperties properties)
        {
            SPWeb web = (SPWeb)properties.Feature.Parent;
            SPMember owner = web.SiteAdministrators[0];
            SPUser user = (SPUser)owner;
            if (!GroupExistsInSite(web, "Riverstone TFS Default Members"))
            {
                web.SiteGroups.Add("Riverstone TFS Default Members", owner, user, "This group contains the Riverstone Active Directory groups that have access to TFS portal sites by default.");
            }
            SPGroup defaultGroup = web.SiteGroups["Riverstone TFS Default Members"];
            defaultGroup.OnlyAllowMembersViewMembership = false;
            defaultGroup.Update();
            SPRoleAssignment roleAssignment = new SPRoleAssignment(defaultGroup);
            SPRoleDefinition contributeDefinition = web.RoleDefinitions["Contribute"];
            roleAssignment.RoleDefinitionBindings.Add(contributeDefinition);
            if (!GroupExistsInWeb(web, "Riverstone TFS Default Members"))
            {
                web.RoleAssignments.Add(roleAssignment);
            }
            AddUserToGroup(defaultGroup, "trg\\biudatasmes");
            AddUserToGroup(defaultGroup, "trg\\biuoperations");
            AddUserToGroup(defaultGroup, "trg\\biureporting");
            AddUserToGroup(defaultGroup, "trg\\dataarchitect");
            AddUserToGroup(defaultGroup, "trg\\developer");
            AddUserToGroup(defaultGroup, "trg\\enterprisearchitect");
            AddUserToGroup(defaultGroup, "trg\\pmuba");
            AddUserToGroup(defaultGroup, "trg\\pmupm");
            AddUserToGroup(defaultGroup, "trg\\tfsadministrator");
            AddUserToGroup(defaultGroup, "trg\\OpsManagement");
        }

        private void AddUserToGroup(SPGroup group, string loginName)
        {
            SPUser user = group.ParentWeb.EnsureUser(loginName);
            group.AddUser(user);
            group.Update();
        }

        private bool GroupExistsInWeb(SPWeb web, string name)
        {
            return web.Groups.OfType<SPGroup>().Count(g => g.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase)) > 0;
        }

        private bool GroupExistsInSite(SPWeb web, string name)
        {
            return web.SiteGroups.OfType<SPGroup>().Count(g => g.Name.Equals(name, StringComparison.InvariantCulture)) > 0;
        }


        // Uncomment the method below to handle the event raised before a feature is deactivated.

        //public override void FeatureDeactivating(SPFeatureReceiverProperties properties)
        //{
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
