using CommutationCentral.API.Entities;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Users;
using CommutationCentral.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Services.Users
{
    public interface IUserRepository
    {
        PagedList<ApplicationUser> GetUsers(UserResourceParameters userResourceParameters);
        List<ApplicationUser> GetUsersUnpaged(UserResourceParameters userResourceParameters);
        IEnumerable<ApplicationUser> GetUsers(IEnumerable<string> userIds);
        List<UserSearchResultDTO> SearchUsers(string searchTerm);
        ApplicationUser GetById(string userId);
        ApplicationUser GetByIDPId(string idpId);
        ApplicationUser GetByUserName(string userName);
        Task<IdentityResult> DeleteUser(ApplicationUser applicationUser);
        Task<IdentityResult> UpdateUser(ApplicationUser applicationUser);
        bool UserExists(string userId);
        IEnumerable<string> GetRolesForUser(ApplicationUser applicationUser);
        Task<IdentityResult> AddRoleForUser(ApplicationUser applicationUser, string role);
        Task<IdentityResult> RemoveRoleForUser(ApplicationUser applicationUser, string role);
        Task<IdentityResult> AddRolesAsync(ApplicationUser applicationUser, List<string> roles);
        Task<IdentityResult> RemoveRolesAsync(ApplicationUser applicationUser, List<string> roles);
        Task<IEnumerable<ApplicationUser>> GetUsersInRoleAsync(string roleName);
        bool Save();
    }
}
