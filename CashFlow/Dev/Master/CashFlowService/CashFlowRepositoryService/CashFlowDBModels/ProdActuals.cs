using System;
using System.Collections.Generic;

namespace CashFlowRepositoryService.CashFlowDBModels
{
    public partial class ProdActuals
    {
        public string ExpId { get; set; }
        public int ExposureId { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public decimal TotalPaidLosses { get; set; }
        public decimal PaidDjExp { get; set; }
        public decimal PaidNonDjExp { get; set; }
        public decimal? PaidNonDjExpWithinLimits { get; set; }
        public decimal? PaidNonDjExpOutsideLimits { get; set; }
    }
}
