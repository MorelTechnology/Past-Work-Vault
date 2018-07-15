namespace WorkRequestDataService.Models
{
    /// <summary>
    /// WorkRequest Denial or Return object
    /// </summary>
    public class Rejection
    {
        #region Public Properties

        /// <summary>Comment to be associated to the WorkRequest being Rejected.</summary>
        public string Comment { get; set; }

        /// <summary><c>true</c> if the WorkRequest should be denied, otherwise
        /// <c>false</c> if it's simply being returned.</summary>
        public bool IsDenial { get; set; }

        /// <summary>The RequestId of the Work Request being rejected.</summary>
        public int WorkRequestId { get; set; }

        #endregion Public Properties
    }
}