namespace WorkRequestDataService.Models
{
    /// <summary>Notification Template Types</summary>
    public enum Template
    {
#pragma warning disable 1591
        ManagerSubmission,
        PortfolioManagerSubmission,
        ReturnToAssociate,
        Approved,
        Cancelled,
        Denied,
        Restored,
        DigitalStrategySubmission,
        ReadyForScheduling
#pragma warning restore 1591
    }

    /// <summary>
    /// A Notification object.
    /// </summary>
    public class Notification
    {
        /// <summary>Work Request ID number</summary>
        public int WorkRequestId { get; set; }

        /// <summary>SAMAccountName or User Sid of User to notify.</summary>
        public string NotifyUser { get; set; }

        /// <summary>Notification Template to use.</summary>
        public Template Template { get; set; }

        /// <summary>Additional Comments to be included in the notification</summary>
        public string Comments { get; set; }
    }
}