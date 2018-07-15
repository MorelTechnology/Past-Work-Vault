using CommutationCentral.API.Entities;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Roles;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommutationCentral.API.Services.Roles
{
    public interface IRoleRepository
    {
        ApplicationRole GetById(string roleId);
        Task<bool> RoleExistsAsync(string roleName);
        PagedList<ApplicationRole> GetRolesPaged(RoleResourceParameters roleResourceParameters);
        List<ApplicationRole> GetRolesUnpaged(RoleResourceParameters roleResourceParameters);
        Task<IdentityResult> RemoveRoleClaimAsync(ApplicationRole role, Claim claim);
        Task<IdentityResult> AddRoleClaimAsync(ApplicationRole role, Claim claim);
        Task<IdentityResult> UpdateRoleAsync(ApplicationRole role);
        Task<IdentityResult> DeleteRoleAsync(ApplicationRole role);
    }
}
