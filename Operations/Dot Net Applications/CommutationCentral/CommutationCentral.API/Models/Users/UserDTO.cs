using CommutationCentral.API.Models.UserRoles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Models.Users
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public virtual ICollection<ApplicationUserRoleDTO> UserRoles { get; set; }
    }
}
