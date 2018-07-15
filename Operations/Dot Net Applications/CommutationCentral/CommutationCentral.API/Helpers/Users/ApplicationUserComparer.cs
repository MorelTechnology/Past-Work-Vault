using CommutationCentral.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Helpers.Users
{
    public class ApplicationUserComparer : IEqualityComparer<ApplicationUser>
    {
        public bool Equals(ApplicationUser x, ApplicationUser y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x == null || y == null)
                return false;
            return x.Id.Equals(y.Id, StringComparison.OrdinalIgnoreCase) && x.UserName.Equals(y.UserName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(ApplicationUser obj)
        {
            return (obj.UserName.ToLower() + obj.Id.ToLower()).GetHashCode();
        }
    }
}
