using System.Collections.Generic;
using System.Web.Http;
using WorkRequestDataService.Models;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Controllers
{
    /// <summary>
    /// This controller provides methods for interacting with application configuration.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class ConfigController : ApiController
    {
        #region Private Fields

        private ConfigurationService configurationService = new ConfigurationService();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Gets the specified configuration property value as an object of the type it was originally stored as.
        /// </summary>
        /// <param name="key">The key of the property to retrieve.</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("data/Config/GetConfigurationProperty/{key}")]
        public dynamic GetConfigurationProperty(string key)
        {
            return configurationService.GetConfigurationProperty(key);
        }

        /// <summary>
        /// Gets configuration value for Corporate Goals.
        /// </summary>
        /// <returns>Array of <see cref="string"/> representing Corporate goal values.</returns>
        [HttpGet()]
        [Route("data/Config/GetCorporateGoals/")]
        public dynamic GetCorporateGoals()
        {
            return configurationService.GetCorpGoals();
        }

        /// <summary>
        /// Gets configuration value for Product Owners (Formerly known as Product Managers).
        /// </summary>
        /// <returns><see cref="List{Dictionary}"/> of Product Owners.</returns>
        [HttpGet()]
        [Route("data/Config/GetProductManagers/")]
        public List<Dictionary<string, object>> GetProductManagers()
        {
            return configurationService.GetManagers();
        }

        /// <summary>
        /// Sets a configuration property.
        /// </summary>
        /// <param name="configurationProperty">A <see cref="ConfigurationProperty"/>object representing the property being set.</param>
        /// <returns><c>true</c> if update was performed; otherwise <c>false</c>. </returns>
        [HttpPost()]
        [Route("data/Config/SetConfigurationProperty")]
        public bool SetConfigurationProperty([FromBody]ConfigurationProperty configurationProperty)
        {
            return configurationService.SetConfig(configurationProperty);
        }

        #endregion Public Methods
    }
}