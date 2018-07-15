using AutoMapper;
using CommutationCentral.API.Entities;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Roles;
using CommutationCentral.API.Helpers.Users;
using CommutationCentral.API.Models.Roles;
using CommutationCentral.API.Services;
using CommutationCentral.API.Services.Roles;
using CommutationCentral.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CommutationCentral.API.Controllers
{
    [Route("api/roles")]
    [Authorize]
    public class RolesController : Controller
    {
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IRoleRepository _roleRepository;
        private readonly IUrlHelper _urlHelper;
        private readonly IUserRepository _userRepository;

        public RolesController(IPropertyMappingService propertyMappingService, IRoleRepository roleRepository, IUrlHelper urlHelper, IUserRepository userRepository)
        {
            _propertyMappingService = propertyMappingService;
            _roleRepository = roleRepository;
            _urlHelper = urlHelper;
            _userRepository = userRepository;
        }

        [HttpGet("[action]", Name = "GetRolesPaged")]
        public IActionResult GetRolesPaged(RoleResourceParameters roleResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<ApplicationRoleDTO, ApplicationRole>(roleResourceParameters.Fields))
            {
                return BadRequest();
            }

            if (!_propertyMappingService.ValidMappingExistsFor<ApplicationRoleDTO, ApplicationRole>(roleResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            var rolesFromRepo = _roleRepository.GetRolesPaged(roleResourceParameters);

            var roles = Mapper.Map<IEnumerable<ApplicationRoleDTO>>(rolesFromRepo);

            var previousPageLink = rolesFromRepo.HasPrevious ?
                CreateRolesResourceUri(roleResourceParameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = rolesFromRepo.HasNext ?
                CreateRolesResourceUri(roleResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = rolesFromRepo.TotalCount,
                pageSize = rolesFromRepo.PageSize,
                currentPage = rolesFromRepo.CurrentPage,
                totalPages = rolesFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var shapedRoles = roles.ShapeData(roleResourceParameters.Fields);
            return Ok(shapedRoles);
        }

        [HttpGet("[action]", Name = "GetRolesUnpaged")]
        public IActionResult GetRolesUnpaged(RoleResourceParameters roleResourceParameters)
        {
            var i = User;
            if (!_propertyMappingService.ValidMappingExistsFor<ApplicationRoleDTO, ApplicationRole>(roleResourceParameters.Fields))
            {
                return BadRequest();
            }

            if (!_propertyMappingService.ValidMappingExistsFor<ApplicationRoleDTO, ApplicationRole>(roleResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            var rolesFromRepo = _roleRepository.GetRolesUnpaged(roleResourceParameters);

            var roles = Mapper.Map<IEnumerable<ApplicationRoleDTO>>(rolesFromRepo);
            var shapedRoles = roles.ShapeData(roleResourceParameters.Fields);
            return Ok(shapedRoles);
        }

        [HttpGet("{id}", Name = "GetRole")]
        public IActionResult GetRole(string id)
        {
            var roleFromRepo = _roleRepository.GetById(id);
            if (roleFromRepo == null)
            {
                return NotFound();
            }

            var role = Mapper.Map<ApplicationRoleDTO>(roleFromRepo);
            return Ok(role);
        }

        private string CreateRolesResourceUri(RoleResourceParameters roleResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetRoles",
                        new
                        {
                            fields = roleResourceParameters.Fields,
                            orderBy = roleResourceParameters.OrderBy,
                            searchQuery = roleResourceParameters.SearchQuery,
                            pageNumber = roleResourceParameters.PageNumber - 1,
                            pageSize = roleResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetRoles",
                        new
                        {
                            fields = roleResourceParameters.Fields,
                            orderBy = roleResourceParameters.OrderBy,
                            searchQuery = roleResourceParameters.SearchQuery,
                            pageNumber = roleResourceParameters.PageNumber + 1,
                            pageSize = roleResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetRoles",
                        new
                        {
                            fields = roleResourceParameters.Fields,
                            orderBy = roleResourceParameters.OrderBy,
                            searchQuery = roleResourceParameters.SearchQuery,
                            pageNumber = roleResourceParameters.PageNumber,
                            pageSize = roleResourceParameters.PageSize
                        });
            }
        }

        [HttpDelete("{id}", Name = "DeleteRole")]
        public IActionResult DeleteRole(string id)
        {
            var roleFromRepo = _roleRepository.GetById(id);
            if (roleFromRepo == null)
            {
                return NotFound();
            }

            var deleteOperation = _roleRepository.DeleteRoleAsync(roleFromRepo).Result;
            if (!deleteOperation.Succeeded)
            {
                throw new Exception($"Error deleting role. {deleteOperation.Errors.FirstOrDefault().Description}");
            }

            return NoContent();
        }

        [HttpPut("{id}", Name ="UpdateRole")]
        public async Task<IActionResult> UpdateRoleAsync(string id, [FromBody] ApplicationRoleForUpdateWithUsersDTO applicationRole)
        {
            if (applicationRole == null)
            {
                return BadRequest();
            }
            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var roleFromRepo = _roleRepository.GetById(id);
            if (roleFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(applicationRole, roleFromRepo);

            var dupeChecker = new ApplicationUserComparer();
            var currentRoleUsers = await _userRepository.GetUsersInRoleAsync(roleFromRepo.Name);
            var usersFromModel = _userRepository.GetUsers(applicationRole.Users.Select(u => u.Id));
            var usersToAdd = usersFromModel.Except(currentRoleUsers, dupeChecker);
            var usersToRemove = currentRoleUsers.Except(usersFromModel, dupeChecker);

            foreach(var user in usersToAdd)
            {
                var addOperation = await _userRepository.AddRoleForUser(user, roleFromRepo.Name);
                if (!addOperation.Succeeded)
                {
                    throw new Exception($"Error adding users to role. {addOperation.Errors.FirstOrDefault().Description}");
                }
            }

            foreach(var user in usersToRemove)
            {
                var removeOperation = await _userRepository.RemoveRoleForUser(user, roleFromRepo.Name);
                if (!removeOperation.Succeeded)
                {
                    throw new Exception($"Error removing users from role. {removeOperation.Errors.FirstOrDefault().Description}");
                }
            }

            var updateOperation = await _roleRepository.UpdateRoleAsync(roleFromRepo);
            if (!updateOperation.Succeeded)
            {
                throw new Exception($"Error updating role. {updateOperation.Errors.FirstOrDefault().Description}");
            }

            

            return NoContent();
        }
    }
}
