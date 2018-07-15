using CommutationCentral.API.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Helpers.Users
{
    public class UserSearchResultDTOComparer : IEqualityComparer<UserSearchResultDTO>
    {
        public bool Equals(UserSearchResultDTO x, UserSearchResultDTO y)
        {
            if (ReferenceEquals(x, y))
                return true;
            if (x == null || y == null)
                return false;
            return x.UserName.Equals(y.UserName, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(UserSearchResultDTO obj)
        {
            return (obj.UserName.ToLower()).GetHashCode();
        }
    }
}
