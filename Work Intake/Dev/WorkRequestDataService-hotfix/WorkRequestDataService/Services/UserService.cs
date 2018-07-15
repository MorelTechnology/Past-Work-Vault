using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Principal;
using WorkRequestDataService.Models;

namespace WorkRequestDataService.Services
{
    /// <summary>
    /// User Service
    /// Contains methods employed primarily by Public API Controller <c>AuthController</c>
    /// as well as other methods interacting with User data.
    /// </summary>

    internal class UserService
    {
        #region Internal Methods

        internal bool CurrentUserIsConfigurationAdmin()
        {
            return IsUserConfigurationAdmin(System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace("trg\\", ""));
        }

        internal bool CurrentUserIsEnvironmentAdmin()
        {
            return IsUserEnviromnentAdmin(System.Web.HttpContext.Current.User.Identity.Name.ToLower().Replace("trg\\", ""));
        }

        internal List<string> GetGroupMembers(List<string> groupNames)
        {
            List<string> members = new List<string>();
            var ctx = new PrincipalContext(ContextType.Domain);
            foreach (string groupName in groupNames)
            {
                GroupPrincipal group = GroupPrincipal.FindByIdentity(ctx, groupName);
                if (group != null)
                {
                    // iterate over members
                    foreach (Principal p in group.GetMembers())
                    {
                        members.Add(p.SamAccountName.Contains("\\") ?
                            p.SamAccountName.Split('\\')[1] :
                            p.SamAccountName);
                    }
                }
            }
            return members;
        }

        internal Dictionary<string, object> GetUserInfo(string userIdentifier)
        {
            if (String.IsNullOrEmpty(userIdentifier))
            {
                return new Dictionary<string, object> { ["displayName"] = "Unknown: (blank)" };
            }
            Dictionary<string, object> result = new Dictionary<string, object>();
            DirectoryEntry de = null;
            PrincipalContext ctx = new PrincipalContext(ContextType.Domain);
            try
            {
                if (UserIdentifierIsSid(userIdentifier))
                    de = UserPrincipal.FindByIdentity(ctx, IdentityType.Sid, userIdentifier).GetUnderlyingObject() as DirectoryEntry;
                else
                    //de = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, userIdentifier).GetUnderlyingObject() as DirectoryEntry;
                    de = Principal.FindByIdentity(ctx, IdentityType.SamAccountName, userIdentifier).GetUnderlyingObject() as DirectoryEntry;
            }
            catch
            {
                // didn't find a user, so return a bogus one.
                return new Dictionary<string, object> { ["displayName"] = "Unknown: '" + userIdentifier.ToString() + "'" };
            }
            foreach (string key in de.Properties.PropertyNames)
                try
                {
                    if (key.ToLower().Contains("sid"))
                    {
                        // Make sids great again.
                        result.Add(key, new SecurityIdentifier(de.Properties[key].Value as byte[], 0).ToString());
                    }
                    //else if (key.ToLower().Contains("guid"))
                    //{
                    //    result.Add(key, new Guid (de.Properties[key].Value as byte[], 0).ToString());
                    //}
                    else
                        result.Add(key, de.Properties[key].Value);
                }
                catch (Exception) { } //  ¯\_(ツ)_/¯  attempt to move on
            return result;
        }

        internal bool IsUserAuthorized(string samAccountName)
        {
            ConfigurationService configurationService = new ConfigurationService();
            bool authorized = false;
            try
            {
                var ctx = new PrincipalContext(ContextType.Domain);
                var user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, samAccountName);
                List<string> authorizedGroups = new List<string>();
                List<string> unAuthorizedGroups = new List<string>();

                authorizedGroups.AddRange(configurationService.GetConfigurationProperty<List<string>>("User_Groups_Application_Admin"));
                authorizedGroups.AddRange(configurationService.GetConfigurationProperty<List<string>>("User_Groups_Environment_Admin"));
                authorizedGroups.AddRange(configurationService.GetConfigurationProperty<List<string>>("User_Groups_General_Access"));
                authorizedGroups.AddRange(configurationService.GetConfigurationProperty<List<string>>("User_Groups_Digital_Strategy"));
                authorizedGroups.AddRange(configurationService.GetConfigurationProperty<List<string>>("User_Groups_Portfolio_Manager"));
                authorizedGroups.AddRange(configurationService.GetConfigurationProperty<List<string>>("User_Groups_Product_Owner"));

                unAuthorizedGroups.AddRange(configurationService.GetConfigurationProperty<List<string>>("User_Groups_Denied_Access"));

                // Fail anyone in a disallowed group

                if (IsUserMemberOf(ctx, user, unAuthorizedGroups.ToArray<string>()))
                {
                    authorized = false;
                    UnauthorizedAccessException ex = new UnauthorizedAccessException("RESTRICTED: You have not yet been granted access to this application. (" + samAccountName + ")");
                    Error.Log(ex);
                    throw (ex);
                }

                if (IsUserMemberOf(ctx, user, authorizedGroups.ToArray<string>()))
                    authorized = true;
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw;
            }
            return authorized;
        }

        internal bool IsUserConfigurationAdmin(string samAccountName)
        {
            ConfigurationService configurationService = new ConfigurationService();
            bool isAdmin = false;
            try
            {
                var ctx = new PrincipalContext(ContextType.Domain);
                var user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, samAccountName);
                List<string> adminGroups = new List<string>();
                adminGroups.AddRange(configurationService.GetConfigurationProperty<List<String>>("User_Groups_Application_Admin"));
                adminGroups.AddRange(configurationService.GetConfigurationProperty<List<String>>("User_Groups_Environment_Admin"));
                if (IsUserMemberOf(ctx, user, adminGroups.ToArray<string>()))
                    isAdmin = true;
            }
            catch
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        internal bool IsUserEnviromnentAdmin(string samAccountName)
        {
            ConfigurationService configurationService = new ConfigurationService();
            bool isAdmin = false;
            try
            {
                var ctx = new PrincipalContext(ContextType.Domain);
                var user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, samAccountName);
                List<string> adminGroups = new List<string>();
                adminGroups.AddRange(configurationService.GetConfigurationProperty<List<string>>("User_Groups_Environment_Admin"));
                if (IsUserMemberOf(ctx, user, adminGroups.ToArray<string>()))
                    isAdmin = true;
            }
            catch
            {
                isAdmin = false;
            }
            return isAdmin;
        }

        internal bool IsUserMemberOf(PrincipalContext ctx, UserPrincipal user, string groupName)
        {
            bool isMember = false;
            try { isMember = user.IsMemberOf(ctx, IdentityType.Name, groupName); }
            catch (Exception ex) { Error.Log(ex); }
            return isMember;
        }

        internal bool IsUserMemberOf(PrincipalContext ctx, UserPrincipal user, string[] groupNames)
        {
            // Extends method to support multiple groups and returns on first positive result

            foreach (string group in groupNames)
            {
                if (IsUserMemberOf(ctx, user, group)) return true;
            }
            return false;
        }

        internal bool UserIdentifierIsSid(string userIdentifier)
        {
            try
            {
                SecurityIdentifier sid = new SecurityIdentifier(userIdentifier);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
        }

        #endregion Internal Methods
    }
}