using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Entities
{
    public static class IdentityContextExtensions
    {
        public static void EnsureSeedDataForContext(this IdentityContext context, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (!context.Roles.Any())
            {
                var appRoles = new List<ApplicationRole>()
                {
                    new ApplicationRole(){Name = "CommutationCentral.dev.admin" },
                    new ApplicationRole(){Name = "CommutationCentral.dev.read" }
                };
                foreach (var appRole in appRoles)
                {
                    var createOperation = roleManager.CreateAsync(appRole).Result;
                    if (createOperation.Succeeded)
                    {
                        var resultingRole = roleManager.FindByNameAsync(appRole.Name).Result;
                        if (resultingRole != null)
                        {
                            var addClaimOperation = roleManager.AddClaimAsync(resultingRole, new System.Security.Claims.Claim("AccessLevel", "admin")).Result;
                        }
                    }
                }
            }
            if (!context.Users.Any())
            {
                var users = new List<ApplicationUser>()
                {
                    new ApplicationUser(){Email = "justin_moore@trg.com", IDPIdentifier = "cd6bbc74-2fd6-4e4c-a82e-8282305dcfa6", UserName = "trg\\jmoor", DisplayName = "Moore, Justin" }
                };
                foreach (var user in users)
                {
                    var createOperation = userManager.CreateAsync(user).Result;
                    if (createOperation.Succeeded)
                    {
                        var resultingUser = userManager.FindByNameAsync(user.UserName).Result;
                        if (resultingUser != null)
                        {
                            var addToRoleOperation = userManager.AddToRoleAsync(resultingUser, "CommutationCentral.dev.admin").Result;
                        }
                    }
                }
            }
        }
    }
}
