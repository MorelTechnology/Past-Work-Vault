using CommutationCentral.API.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Models.Roles
{
    public class ApplicationRoleForUpdateWithUsersDTO : ApplicationRoleForManipulationDTO
    {
        public List<UserDTO> Users { get; set; }
    }
}
