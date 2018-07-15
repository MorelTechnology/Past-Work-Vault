using System;
using System.Collections.Generic;
using System.ComponentModel;
using static WorkRequestDataService.Services.Utility;

namespace WorkRequestDataService.Models
{
    ///<summary>Unit of Measurement for Declared Business Value</summary>
    public enum BusinessValueUnit
    {
#pragma warning disable 1591

        [Description("Update required to convert legacy Quantification statement.")]
        Unknown = 0,

        [Description("Dollars per year")]
        Dollars = 1,

        [Description("Hours per year")]
        Hours = 2

#pragma warning restore 1591
    }

    /// <summary>
    /// The Status of the Work Request
    /// </summary>
    public enum WorkRequestStatus
    {
#pragma warning disable 1591

        [Description("In Progress")]
        InProgress = 0,

        [Description("Submitted To Product Owner")]
        SubmittedToProductOwner = 1,

        [Description("Submitted To Portfolio Manager")]
        SubmittedToPortfolioManager = 2,

        [Description("Returned To Associate")]
        ReturnedToAssociate = 3,

        [Description("Ready For Prioritization")]
        ReadyForPrioritization = 4,

        [Description("Cancelled")]
        Cancelled = 5,

        [Description("Archived")]
        Archived = 6,

        [Description("Denied")]
        Denied = 7,

        [Description("Submitted To Digital Strategy")]
        SubmittedToDigitalStrategy = 8,

        [Description("Ready For Scheduling")]
        ReadyForScheduling = 9

#pragma warning restore 1591
    }

    /// <summary>Impact Realization Likelihood</summary>
    public enum RealizationOfImpact
    {
#pragma warning disable 1591

        [Description("Unspecified")]
        Unspecified = 0,

        [Description("Very High")]
        VeryHigh = 1,

        [Description("High")]
        High = 2,

        [Description("Low")]
        Low = 3,

        [Description("Very Low")]
        VeryLow = 4

#pragma warning restore 1591
    }

    /// <summary>
    /// Object Representing a request for Work to be performed.
    /// </summary>
    public class WorkRequest
    {
        private BusinessValueUnit? _businessValueUnit;
        private WorkRequestStatus? _status;
        private RealizationOfImpact? _realizationOfImpact;

        // Benefit field deprecated, Iteration 9, January 2018
        // public string Benefit { get; set; }

        /// <summary>The Business Value Amount</summary>
        public decimal? BusinessValueAmount { get; set; }

        /// <summary>Business Value Unit of Measurement</summary>
        public string BusinessValueUnit
        {
            get { if (_businessValueUnit.HasValue) return _businessValueUnit.Description(); else return null; }
            set
            {
                if (value != null)
                    try
                    {
                        _businessValueUnit = (BusinessValueUnit)Enum.Parse(typeof(BusinessValueUnit), value, true);
                    }
                    catch
                    {
                        _businessValueUnit = (BusinessValueUnit)GetEnumFromDescription(value, typeof(BusinessValueUnit));
                    }
            }
        }

        /// <summary>Comments indicating satisfaction requiremnets.</summary>
        public string ConditionsOfSatisfaction { get; set; }

        /// <summary>Array of Corporate Goals associated with the Work Request</summary>
        public List<string> CorporateGoals { get; set; }

        /// <summary>Statement regarding Department Goal</summary>
        public string DeptGoalSupport { get; set; }

        /// <summary>General Purpose Statement</summary>
        public string Goal { get; set; }

        /// <summary>Goal Supporting Statement</summary>
        public string GoalSupport { get; set; }

        /// <summary>
        /// Last Modified Date
        /// NOTE: This value is controlled by the system, and is considered 'query only.'
        /// </summary>
        public DateTime? LastModified { get; set; }

        /// <summary>A user sid representing the assigned Manager (Product Owner) for the request.</summary>
        public string Manager { get; set; }

        // Problem field deprecated, Iteration 9, January 2018
        // public string Problem { get; set; }

        // Quantification field deprecated, Iteration 6, December 2017, replaced by BusinessValueUnit/BusinessValueAmount
        // public string Quantification { get; set; }
        /// <summary>Statement regarding the impact of not performing the work described in the request.</summary>
        public string NonImplementImpact { get; set; }

        /// <summary>The ID of the Work Request.
        /// Note: This value is controlled by the system, and is considered 'query only.'
        ///</summary>
        public int? RequestID { get; set; }

        /// <summary>The User Sid of the User requesting the work in this request.</summary>
        public string Requestor { get; set; }

        /// <summary>Requested Completion Date of Work in the request.</summary>
        public DateTime? RequestedCompletionDate { get; set; }

        /// <summary>Likelihood that impact will be realized.</summary>
        public string RealizationOfImpact
        {
            get { if (_realizationOfImpact.HasValue) return _realizationOfImpact.Description(); else return null; }
            set
            {
                if (value != null)
                    try
                    {
                        _realizationOfImpact = (RealizationOfImpact)Enum.Parse(typeof(RealizationOfImpact), value, true);
                    }
                    catch
                    {
                        _realizationOfImpact = (RealizationOfImpact)GetEnumFromDescription(value, typeof(RealizationOfImpact));
                    }
            }
        }

        /// <summary>The Status of the Work Request.
        /// Generally this is used to Query.  Setting this value should be limited to the UI or Client Application.
        /// </summary>
        public string Status
        {
            get { if (_status.HasValue) return _status.Description(); else return null; }
            set
            {
                if (value != null)
                    try
                    {
                        _status = (WorkRequestStatus)Enum.Parse(typeof(WorkRequestStatus), value, true);
                    }
                    catch
                    {
                        _status = (WorkRequestStatus)GetEnumFromDescription(value, typeof(WorkRequestStatus));
                    }
            }
        }

        /// <summary>The date the current status was set.
        /// This value is controlled by the system, and is considered 'query only.'
        /// </summary>
        public DateTime? StatusDate { get; set; }

        /// <summary>Indicates whether the request Supports a department goal.</summary>
        public bool? SupportsDept { get; set; }

        /// <summary>Title (headline) of the Work Request Entry.</summary>
        public string Title { get; set; }
    }
}