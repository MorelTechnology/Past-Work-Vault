using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Helpers.Lookups
{
    public class ActivityCategoryResourceParameters : EntityResourceParameters
    {
        public override string OrderBy { get; set; } = "Id";
    }
}
