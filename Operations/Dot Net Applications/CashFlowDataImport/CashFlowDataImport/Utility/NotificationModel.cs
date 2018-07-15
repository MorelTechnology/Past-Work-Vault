using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CashFlowDataImport.Utility
{
    class NotificationModel
    {
        public string Analyst { get; set; }
        public string WorkMatter { get; set; }
        public string Status { get; set; } = string.Empty;
        public string Basis { get; set; } 
        public string Comments { get; set; }
        public float ReasonableWorstCase { get; set; } = 0;
        public string Confidence { get; set; } = string.Empty;
        public bool Viewed { get; set; } = false;
        public string DeclinedReason { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public string StartUser { get; set; }
        public float RWCLoss { get; set; } = 0;
        public float RWCDefExp { get; set; } = 0;
        public float RWCCovDJ { get; set; } = 0;
    }
}
