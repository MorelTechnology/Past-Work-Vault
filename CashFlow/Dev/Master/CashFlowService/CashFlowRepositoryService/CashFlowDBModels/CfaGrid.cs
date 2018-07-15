using System;
using System.Collections.Generic;

namespace CashFlowRepositoryService.CashFlowDBModels
{
    public partial class CfaGrid
    {
        public int CfaGridId { get; set; }
        public string CfaGridKey { get; set; }
        public string CfaGridType { get; set; }
        public int? CfaGridYear { get; set; }
        public int? CfaGridQuarter { get; set; }
        public string CfaGridReadWrite { get; set; }
    }
}
