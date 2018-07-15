using System;
using System.Collections.Generic;
using System.Data;
using WorkRequestDataService.Models;

namespace WorkRequestDataService.Services
{
    /// <summary>
    /// Configuration Service
    /// Contains methods employed by Public API Controller <c>ConfigController</c>.
    /// </summary>
    internal class ConfigurationService
    {
        #region Internal Methods

        internal DataTable GetConfiguration()
        {
            Dao dao = new Dao();
            return dao.GetConfiguration();
        }

        internal T GetConfigurationProperty<T>(string key)
        {
            Dao dao = new Dao();
            return dao.GetConfigurationProperty<T>(key);
        }

        internal Object GetConfigurationProperty(string key)
        {
            Dao dao = new Dao();
            return dao.GetConfigurationProperty(key);
        }

        internal dynamic GetCorpGoals()
        {
            Dao dao = new Dao();
            return GetConfigurationProperty<List<string>>("Corporate_Goals");
        }

        internal List<Dictionary<string, object>> GetManagers()
        {
            Dao dao = new Dao();
            UserService userService = new UserService();
            var managerIds = userService.GetGroupMembers(GetConfigurationProperty<List<string>>("User_Groups_Product_Owner"));

            List<Dictionary<string, object>> managerUsers = new List<Dictionary<string, object>>();

            foreach (var id in managerIds)
            {
                try
                {
                    managerUsers.Add(userService.GetUserInfo(id));
                }
                catch (Exception ex)
                {
                    InvalidOperationException outerEx = new InvalidOperationException("Error while trying to retrieve Product Manager list. " +
                        "Could not locate user with id '" + id + "'", ex);
                    Error.Log(outerEx);
                    throw (outerEx);
                }
            }
            return managerUsers;
        }

        internal bool SetConfig(ConfigurationProperty cp)
        {
            Dao dao = new Dao();
            return dao.SetConfig(cp);
        }

        #endregion Internal Methods
    }
}