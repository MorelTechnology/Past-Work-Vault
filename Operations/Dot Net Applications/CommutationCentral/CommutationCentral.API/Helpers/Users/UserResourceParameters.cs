using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Helpers.Users
{
    public class UserResourceParameters : EntityResourceParameters
    {
        public override string OrderBy { get; set; } = "UserName";
    }
}
