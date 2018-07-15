using CommutationCentral.API.Services.Roles;
using CommutationCentral.API.Services.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Controllers
{
    [Route("api/users/{userId}/roles")]
    public class UserRolesController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UserRolesController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        [HttpGet(Name = "GetRolesForUser")]
        public IActionResult GetRolesForUser(string userId)
        {
            var userFromRepo = _userRepository.GetById(userId);

            if (userFromRepo == null)
            {
                return NotFound();
            }

            var userRolesFromRepo = _userRepository.GetRolesForUser(userFromRepo);
            return Ok(userRolesFromRepo);
        }

        [HttpGet("{role}", Name = "GetRoleForUser")]
        public IActionResult GetRoleForUser(string userId, string role)
        {
            var userFromRepo = _userRepository.GetById(userId);

            if (userFromRepo == null)
            {
                return NotFound();
            }

            var userRoleFromRepo = _userRepository.GetRolesForUser(userFromRepo).FirstOrDefault(r => r == role);
            if (string.IsNullOrEmpty(userRoleFromRepo))
            {
                return NotFound();
            }
            return Ok(userRoleFromRepo);
        }

        [HttpPost("{role}", Name = "AddUserToRole")]
        public IActionResult AddUserToRole(string userId, string role)
        {
            if (!_roleRepository.RoleExistsAsync(role).Result)
            {
                return NotFound();
            }

            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var operation = _userRepository.AddRoleForUser(user, role).Result;
            if (!operation.Succeeded)
            {
                throw new Exception($"Failed to add user to role. {operation.Errors.FirstOrDefault().Description}");
            }

            return CreatedAtRoute("GetRoleForUser", new { userId, role }, role);
        }

        [HttpDelete("{role}", Name = "RemoveUserFromRole")]
        public IActionResult RemoveUserFromRole(string userId, string role)
        {
            if (!_roleRepository.RoleExistsAsync(role).Result)
            {
                return NotFound();
            }

            var user = _userRepository.GetById(userId);
            if (user == null)
            {
                return NotFound();
            }

            var operation = _userRepository.RemoveRoleForUser(user, role).Result;
            if (!operation.Succeeded)
            {
                throw new Exception($"Failed to remove user from role. {operation.Errors.FirstOrDefault().Description}");
            }

            return NoContent();
        }
    }
}
