using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommutationCentral.API.Entities;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Users;
using CommutationCentral.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using Microsoft.Extensions.Logging;
using System.DirectoryServices.ActiveDirectory;

namespace CommutationCentral.API.Services.Users
{
    public class UserRepository : IUserRepository
    {
        private UserManager<ApplicationUser> _userManager;
        private IPropertyMappingService _propertyMappingService;
        private RoleManager<ApplicationRole> _roleManager;
        private ILogger<UserRepository> _logger;

        public UserRepository(UserManager<ApplicationUser> userManager, IPropertyMappingService propertyMappingService,
            RoleManager<ApplicationRole> roleManager, ILogger<UserRepository> logger)
        {
            _userManager = userManager;
            _propertyMappingService = propertyMappingService;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IdentityResult> DeleteUser(ApplicationUser applicationUser)
        {
            return await _userManager.DeleteAsync(applicationUser);
        }

        public ApplicationUser GetById(string userId)
        {
            return _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefault(u => u.Id == userId);
        }

        public ApplicationUser GetByIDPId(string idpId)
        {
            return _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefault(u => u.IDPIdentifier == idpId);
        }

        public ApplicationUser GetByUserName(string userName)
        {
            return _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).FirstOrDefault(u => u.UserName == userName);
        }

        public PagedList<ApplicationUser> GetUsers(UserResourceParameters userResourceParameters)
        {
            var appUsers = _userManager.Users.Include(u => u.UserRoles).ThenInclude(ur => ur.Role).AsQueryable();

            var collectionBeforePaging = appUsers.OrderBy($"{userResourceParameters.OrderBy}").AsQueryable();

            if (!string.IsNullOrEmpty(userResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = userResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(u => u.Email.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || u.UserName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<ApplicationUser>.Create(collectionBeforePaging, userResourceParameters.PageNumber, userResourceParameters.PageSize);
        }

        public List<ApplicationUser> GetUsersUnpaged(UserResourceParameters userResourceParameters)
        {
            var appUsers = _userManager.Users
                .Include(u => u.UserRoles).ThenInclude(ur => ur.Role).AsQueryable();
            var collection = appUsers.OrderBy($"{userResourceParameters.OrderBy}").AsQueryable();

            if (!string.IsNullOrEmpty(userResourceParameters.SearchQuery))
            {
                var searchQueryForWhereClause = userResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collection = collection
                    .Where(u => u.Email.ToLowerInvariant().Contains(searchQueryForWhereClause)
                    || u.UserName.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return collection.ToList();
        }

        public List<UserSearchResultDTO> SearchUsers(string searchTerm)
        {

            var usersToReturn = Enumerable.Empty<UserSearchResultDTO>().AsQueryable();
            var dupeChecker = new UserSearchResultDTOComparer();

            // Search the domain directory
            try
            {
                var domainFriendlyName = "trg";
                DirectoryEntry entry = new DirectoryEntry($"LDAP://DC={domainFriendlyName},DC=com");
                using (DirectorySearcher ds = new DirectorySearcher(entry))
                {
                    var usersFromDirectory = new List<UserSearchResultDTO>();
                    ds.PropertiesToLoad.Add("name");
                    ds.PropertiesToLoad.Add("userPrincipalName");
                    ds.PropertiesToLoad.Add("sAMAccountName");
                    ds.PropertiesToLoad.Add("mail");
                    ds.PropertiesToLoad.Add("objectGUID");
                    ds.Filter = $"(&(&(objectClass=user)(objectCategory=user))(|(email=*{searchTerm}*)(name=*{searchTerm}*)(userPrincipalName=*{searchTerm}*)(sAMAccountName=*{searchTerm}*)))";

                    SearchResultCollection results = ds.FindAll();
                    foreach (SearchResult result in results)
                    {
                        var objectGuid = new Guid((byte[])result.Properties["objectguid"][0]);
                        var user = new UserSearchResultDTO()
                        {
                            DisplayName = result.Properties["name"][0].ToString(),
                            UserName = $"{domainFriendlyName}\\{result.Properties["sAMAccountName"][0].ToString()}",
                            Email = result.Properties.Contains("mail") ? result.Properties["mail"][0].ToString() : null,
                            ObjectGUID = objectGuid.ToString()
                        };
                        usersFromDirectory.Add(user);
                    }
                    usersToReturn = usersToReturn.Union(usersFromDirectory, dupeChecker);
                }
            }
            // Domain searching isn't available, fall back to searching the app's user table
            catch (System.Runtime.InteropServices.COMException ex)
            {
                _logger.LogError($"SearchUsers was unable to connect to the specified domain.", ex);

                var usersFromRepo = _userManager.Users.Where(u => u.UserName.Contains(searchTerm)
                || u.Email.Contains(searchTerm)
                || u.UserClaims.Any(uc => (uc.ClaimType == "name" || uc.ClaimType == "email") && uc.ClaimValue.Contains(searchTerm)))
                .Select(u => new UserSearchResultDTO()
                {
                    DisplayName = u.UserClaims.FirstOrDefault(c => c.ClaimType == "name").ClaimValue,
                    UserName = u.UserName,
                    Email = string.IsNullOrEmpty(u.Email) ? u.UserClaims.FirstOrDefault(c => c.ClaimType == "email").ClaimValue : u.Email
                })
                .AsQueryable();

                usersToReturn = usersToReturn.Union(usersFromRepo, dupeChecker);
            }

            return usersToReturn.ToList();
        }

        public IEnumerable<ApplicationUser> GetUsers(IEnumerable<string> userIds)
        {
            return _userManager.Users
                .Where(u => u.UserClaims.Any(uc => uc.ClaimType == "ActiveAppUser" && uc.ClaimValue == "CommutationCentral.dev") && userIds.Contains(u.Id));
        }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public async Task<IdentityResult> UpdateUser(ApplicationUser applicationUser)
        {
            return await _userManager.UpdateAsync(applicationUser);
        }

        public async Task<IdentityResult> AddRolesAsync(ApplicationUser applicationUser, List<string> roles)
        {
            return await _userManager.AddToRolesAsync(applicationUser, roles);
        }

        public async Task<IdentityResult> RemoveRolesAsync(ApplicationUser applicationUser, List<string> roles)
        {
            return await _userManager.RemoveFromRolesAsync(applicationUser, roles);
        }

        public bool UserExists(string userId)
        {
            return _userManager.Users.Any(u => u.Id == userId);
        }

        public IEnumerable<string> GetRolesForUser(ApplicationUser applicationUser)
        {
            var userRoles = _userManager.GetRolesAsync(applicationUser).Result.Where(ur => ur.StartsWith("CommutationCentral.dev"));
            return userRoles;
        }

        public async Task<IdentityResult> AddRoleForUser(ApplicationUser applicationUser, string role)
        {
            return await _userManager.AddToRoleAsync(applicationUser, role);
        }

        public async Task<IdentityResult> RemoveRoleForUser(ApplicationUser applicationUser, string role)
        {
            return await _userManager.RemoveFromRoleAsync(applicationUser, role);
        }

        public async Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName)
        {
            return await _userManager.GetUsersInRoleAsync(roleName);
        }
    }
}
