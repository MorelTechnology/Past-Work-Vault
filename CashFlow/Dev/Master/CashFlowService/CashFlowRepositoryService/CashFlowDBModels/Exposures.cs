using System;
using System.Collections.Generic;

namespace CashFlowRepositoryService.CashFlowDBModels
{
    public partial class Exposures
    {
        public string WorkMatter { get; set; }
        public string ExpId { get; set; }
        public string Portfolio { get; set; }
        public string Coverage { get; set; }
        public string Type { get; set; }
        public decimal AttachPoint { get; set; }
        public DateTime EffectiveDate { get; set; }
        public string PolicyNumber { get; set; }
        public string PolicyType { get; set; }
        public string WithinLimits { get; set; }
        public bool ExpClosed { get; set; }
        public DateTime? ExpClosedDate { get; set; }
        public string WithinLimitsSource { get; set; }
    }
}
