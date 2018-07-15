using CommutationCentral.API.Entities;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommutationCentral.API.Services
{
    public class ApplicationClaimsTransformer : IClaimsTransformation
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ApplicationClaimsTransformer(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            if (principal.Identity.IsAuthenticated)
            {
                Claim userId = principal.FindFirst("id");
                if (userId == null) userId = principal.FindFirst("sub");

                if (principal.FindFirst("role") == null && userId != null)
                {
                    var user = _userManager.Users.FirstOrDefault(u => u.IDPIdentifier == userId.Value);
                    var roles = await _userManager.GetRolesAsync(user);
                    foreach (var role in roles)
                    {
                        ((ClaimsIdentity)principal.Identity).AddClaim(new Claim(JwtClaimTypes.Role, role,
                            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"));
                    }
                }
            }
            return Task.FromResult(principal).Result;
        }
    }
}
