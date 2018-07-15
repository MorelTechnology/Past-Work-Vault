namespace WorkRequestDataService.Models
{
    /// <summary>
    /// Work Request Variance Comparison
    /// </summary>
    public class Variance
    {
        #region Public Properties

        /// <summary>Property value prior to the change.</summary>
        public object OriginalValue { get; set; }

        /// <summary>The Referenced Property which a change is noted.</summary>
        public string Property { get; set; }

        /// <summary>Property value after the change.</summary>
        public object UpdatedValue { get; set; }

        #endregion Public Properties
    }
}