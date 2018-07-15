using System;
using System.Collections.Generic;

namespace CashFlowRepositoryService.CashFlowDBModels
{
    public partial class Users
    {
        public string ActiveDirectoryId { get; set; }
        public string DisplayName { get; set; }
        public string AdjustedName { get; set; }
        public string SamAccountName { get; set; }
        public string EmailAddress { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public string SupervisorId { get; set; }
        public string UnitManagerId { get; set; }
        public string TeamManagerId { get; set; }
        public bool Administrator { get; set; }
        public int ApprovalLimit { get; set; }
        public string StartUser { get; set; }
        public string EndUser { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
