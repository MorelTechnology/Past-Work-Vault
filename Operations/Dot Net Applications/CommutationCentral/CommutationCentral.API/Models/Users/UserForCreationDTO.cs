using CommutationCentral.API.Models.UserRoles;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Models.Users
{
    public class UserForCreationDTO
    {
        [Required]
        public string IDPIdentifier { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "UserName is a required field")]
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        [Required]
        public List<ApplicationUserRoleDTO> UserRoles { get; set; }
    }
}
