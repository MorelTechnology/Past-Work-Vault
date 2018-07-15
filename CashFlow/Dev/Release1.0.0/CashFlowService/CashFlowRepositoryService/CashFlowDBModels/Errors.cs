using System;
using System.Collections.Generic;

namespace CashFlowRepositoryService.CashFlowDBModels
{
    public partial class Errors
    {
        public string ActiveDirectoryId { get; set; }
        public string AdjustedName { get; set; }
        public string Feature { get; set; }
        public string StackTrace { get; set; }
        public DateTime OccurenceTime { get; set; }
    }
}
