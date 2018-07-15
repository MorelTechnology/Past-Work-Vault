using System;
using System.Runtime.InteropServices;
using System.Web;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Models
{
    /// <summary>
    /// An Record representing an error encountered with a Data Service Operation.
    /// </summary>
    public class Error
    {
        #region Public Properties

        /// <value>Additional data, if any to record.</value>
        public string AdditionalDetails { get; set; }

        /// <value>The API or Web operation being performed when Error condition was hit.</value>
        public string CurrentUrl { get; set; } = HttpContext.Current.Request.Url.AbsoluteUri.ToString();

        /// <value>The name of the User who encounterd the error.</value>
        public string CurrentUser { get; set; } = HttpContext.Current.User.Identity.Name;

        /// <value>Runtime exception message that was thrown.</value>
        public string ExceptionMessage { get; set; }

        /// <value>Indicates whether the user was authenticaed when error occured.</value>
        public bool IsAuthenticated { get; set; } = HttpContext.Current.User.Identity.IsAuthenticated;

        /// <value>Localized date and time value.</value>
        public DateTime LocalizedDate { get; set; } = DateTime.Now.ToLocalTime();

        /// <value>StackTrace data</value>
        public string StackTrace { get; set; }

        #endregion Public Properties

        #region Internal Methods

        internal static void Log(Exception ex, [Optional]String additionalDetails)
        {
            ErrorService errorService = new ErrorService();
            Error error = new Error
            {
                ExceptionMessage = ex.Message,
                StackTrace = ex.StackTrace ?? Environment.StackTrace.ToString(),
                AdditionalDetails = additionalDetails ?? string.Empty
            };
            errorService.AddError(error);
        }

        #endregion Internal Methods
    }
}