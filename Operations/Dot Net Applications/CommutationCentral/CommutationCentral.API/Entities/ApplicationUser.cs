using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        [Required]
        public string IDPIdentifier { get; set; }
        public virtual ICollection<ApplicationUserRole> UserRoles { get; } = new List<ApplicationUserRole>();
        public virtual ICollection<IdentityUserClaim<string>> UserClaims { get; } = new List<IdentityUserClaim<string>>();
    }
}
