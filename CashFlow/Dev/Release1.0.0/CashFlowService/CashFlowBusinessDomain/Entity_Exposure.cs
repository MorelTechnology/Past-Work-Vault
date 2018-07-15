using System;
using System.Collections.Generic;

namespace CashFlowBusinessDomain
{
    public class Entity_Exposure
    {
        public string strExpID { get; set; }

        public string strPortfolio { get; set; }

        public string strCoverage { get; set; }

        public string strType { get; set; }

        public int iAttachPoint { get; set; }

        public DateTime dtiEffectiveDate { get; set; }

        public string strPolicyNumber { get; set; }

        public string strPolicyType { get; set; }

        public string strWithinLimits { get; set; }

        public string dtiExpClosed { get; set; }

        public DateTime dtiExpClosedDate { get; set; }

        public string strWithinLimitsSource { get; set; }
    }
}
