using System;
using System.Collections.Generic;

namespace CashFlowRepositoryService.CashFlowDBModels
{
    public partial class ProdWorkMatters
    {
        public string WorkMatter { get; set; }
        public string SpecialTrackingGroup { get; set; }
        public string WorkMatterDescription { get; set; }
        public string InsuredName { get; set; }
        public string AssignedAdjuster { get; set; }
        public string AssignedManager { get; set; }
        public string StartUser { get; set; }
        public string Department { get; set; }
        public string Portfolio { get; set; }
        public bool HasAssociations { get; set; }
        public DateTime StartTime { get; set; }
        public string EndUser { get; set; }
        public DateTime? EndTime { get; set; }
        public bool Wmclosed { get; set; }
        public DateTime? WmclosedDate { get; set; }
    }
}
