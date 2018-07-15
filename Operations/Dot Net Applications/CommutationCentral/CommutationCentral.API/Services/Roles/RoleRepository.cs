using CommutationCentral.API.Entities;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Roles;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using System.Security.Claims;

namespace CommutationCentral.API.Services.Roles
{
    public class RoleRepository : IRoleRepository
    {
        private RoleManager<ApplicationRole> _roleManager;

        public RoleRepository(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public PagedList<ApplicationRole> GetRolesPaged(RoleResourceParameters roleResourceParameters)
        {
            var appRoles = _roleManager.Roles.AsQueryable();

            var collectionBeforePaging = appRoles.OrderBy($"{roleResourceParameters.OrderBy}").AsQueryable();

            if (!string.IsNullOrEmpty(roleResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = roleResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(r => r.Name.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || r.NormalizedName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<ApplicationRole>.Create(collectionBeforePaging, roleResourceParameters.PageNumber, roleResourceParameters.PageSize);
        }

        public List<ApplicationRole> GetRolesUnpaged(RoleResourceParameters roleResourceParameters)
        {
            var appRoles = _roleManager.Roles.AsQueryable();

            if (!string.IsNullOrEmpty(roleResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = roleResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                appRoles = appRoles
                    .Where(r => r.Name.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || r.NormalizedName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return appRoles.ToList();
        }

        public ApplicationRole GetById(string roleId)
        {
            return _roleManager.Roles.FirstOrDefault(r => r.Id == roleId);
        }

        public async Task<IdentityResult> RemoveRoleClaimAsync(ApplicationRole role, Claim claim)
        {
            return await _roleManager.RemoveClaimAsync(role, claim);
        }

        public async Task<IdentityResult> AddRoleClaimAsync(ApplicationRole role, Claim claim)
        {
            return await _roleManager.AddClaimAsync(role, claim);
        }

        public async Task<IdentityResult> UpdateRoleAsync(ApplicationRole role)
        {
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> DeleteRoleAsync(ApplicationRole role)
        {
            return await _roleManager.DeleteAsync(role);
        }
    }
}
