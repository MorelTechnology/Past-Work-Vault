namespace WorkRequestDataService.Models
{
    /// <summary>
    /// Application Configuration Property Object
    /// </summary>
    public class ConfigurationProperty
    {
        #region Public Properties

        /// <summary> The key name of the property.</summary>
        public string Key { get; set; }

        /// <summary> The value to store, the type of which is determined at runtime </summary>
        public dynamic Value { get; set; }

        #endregion Public Properties
    }
}