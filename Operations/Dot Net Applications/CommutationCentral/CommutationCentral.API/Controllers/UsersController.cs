using AutoMapper;
using CommutationCentral.API.Entities;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Users;
using CommutationCentral.API.Models.Users;
using CommutationCentral.API.Services;
using CommutationCentral.API.Services.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Controllers
{
    [Route("api/users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;
        private readonly IPropertyMappingService _propertyMappingService;
        private readonly IUrlHelper _urlHelper;
        private readonly ITypeHelperService _typeHelperService;

        public UsersController(UserManager<ApplicationUser> userManager, IUserRepository userRepository,
            IPropertyMappingService propertyMappingService, IUrlHelper urlHelper, ITypeHelperService typeHelperService)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _propertyMappingService = propertyMappingService;
            _urlHelper = urlHelper;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetUsers")]
        public IActionResult GetUsers(UserResourceParameters userResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<UserDTO, ApplicationUser>(userResourceParameters.Fields))
            {
                return BadRequest();
            }

            if (!_propertyMappingService.ValidMappingExistsFor<UserDTO, ApplicationUser>(userResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            var usersFromRepo = _userRepository.GetUsers(userResourceParameters);

            var users = Mapper.Map<IEnumerable<UserDTO>>(usersFromRepo);

            var previousPageLink = usersFromRepo.HasPrevious ?
                CreateUsersResourceUri(userResourceParameters, ResourceUriType.PreviousPage) : null;
            var nextPageLink = usersFromRepo.HasNext ?
                CreateUsersResourceUri(userResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = usersFromRepo.TotalCount,
                pageSize = usersFromRepo.PageSize,
                currentPage = usersFromRepo.CurrentPage,
                totalPages = usersFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var shapedUsers = users.ShapeData(userResourceParameters.Fields);
            return Ok(shapedUsers);
        }

        [HttpGet("[action]", Name = "GetUsersUnpaged")]
        public IActionResult GetUsersUnpaged(UserResourceParameters userResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<UserDTO, ApplicationUser>(userResourceParameters.Fields))
            {
                return BadRequest();
            }

            if (!_propertyMappingService.ValidMappingExistsFor<UserDTO, ApplicationUser>(userResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            var usersFromRepo = _userRepository.GetUsersUnpaged(userResourceParameters);
            var users = Mapper.Map<IEnumerable<UserDTO>>(usersFromRepo);
            var shapedData = users.ShapeData(userResourceParameters.Fields);

            return Ok(shapedData);
        }

        [HttpGet("[action]", Name = "GetCurrentUserRoles")]
        public IActionResult GetCurrentUserRoles()
        {
            var userFromRepo = _userRepository.GetByIDPId(User.FindFirst("id").Value);
            if (userFromRepo == null)
            {
                throw new Exception($"Currently authenticated user was not found in the application database.");
            }
            var userRoles = _userRepository.GetRolesForUser(userFromRepo);
            return Ok(userRoles);
        }

        [HttpGet("[action]({roleName})", Name = "GetUsersInRole")]
        public IActionResult GetUsersInRole(string roleName)
        {
            if (string.IsNullOrEmpty(roleName))
            {
                return BadRequest();
            }

            var usersFromRepo = _userRepository.GetUsersInRoleAsync(roleName).Result;
            var users = Mapper.Map<IEnumerable<UserDTO>>(usersFromRepo);
            return Ok(users);
        }

        [HttpGet("[action]('{searchTerm}')", Name = "SearchUsers")]
        public IActionResult SearchUsers(string searchTerm)
        {
            var usersToReturn = _userRepository.SearchUsers(searchTerm);
            return Ok(usersToReturn);
        }

        private string CreateUsersResourceUri(UserResourceParameters userResourceParameters,
            ResourceUriType type)
        {
            switch (type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetUsers",
                        new
                        {
                            fields = userResourceParameters.Fields,
                            orderBy = userResourceParameters.OrderBy,
                            searchQuery = userResourceParameters.SearchQuery,
                            pageNumber = userResourceParameters.PageNumber - 1,
                            pageSize = userResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetUsers",
                        new
                        {
                            fields = userResourceParameters.Fields,
                            orderBy = userResourceParameters.OrderBy,
                            searchQuery = userResourceParameters.SearchQuery,
                            pageNumber = userResourceParameters.PageNumber + 1,
                            pageSize = userResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetUsers",
                        new
                        {
                            fields = userResourceParameters.Fields,
                            orderBy = userResourceParameters.OrderBy,
                            searchQuery = userResourceParameters.SearchQuery,
                            pageNumber = userResourceParameters.PageNumber,
                            pageSize = userResourceParameters.PageSize
                        });
            }
        }

        [HttpGet("{id}", Name = "GetUser")]
        public IActionResult GetUser(string id, string fields)
        {
            if (!_typeHelperService.TypeHasProperties<UserDTO>(fields))
            {
                return BadRequest();
            }

            var userFromRepo = _userRepository.GetById(id);

            if (userFromRepo == null)
            {
                return NotFound();
            }
            var userRoles = _userRepository.GetRolesForUser(userFromRepo).OrderBy(r => r).ToList();

            var user = Mapper.Map<UserDTO>(userFromRepo);

            var shapedUser = user.ShapeData(fields) as IDictionary<string, object>;

            return Ok(shapedUser);
        }

        [HttpPost(Name = "CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserForCreationDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var userEntity = Mapper.Map<ApplicationUser>(user);
            var existingUser = _userRepository.GetByUserName(userEntity.UserName);
            if (existingUser != null)
            {
                throw new Exception($"Creating a user failed. User already exists.");
            }

            var createOperation = await _userManager.CreateAsync(userEntity);
            if (!createOperation.Succeeded)
            {
                throw new Exception($"Creating a user failed. {createOperation.Errors.FirstOrDefault().Description}");
            }

            if (user.UserRoles.Any())
            {
                var addRolesOperation = await _userRepository.AddRolesAsync(userEntity, user.UserRoles.Select(ur => ur.Name).ToList());
                if (!addRolesOperation.Succeeded)
                {
                    throw new Exception($"Adding user roles failed. {addRolesOperation.Errors.FirstOrDefault().Description}");
                }
            }

            var userFromRepo = _userRepository.GetByUserName(userEntity.UserName);

            var userToReturn = Mapper.Map<UserDTO>(userFromRepo);
            var shapedUser = userToReturn.ShapeData(null) as IDictionary<string, object>;

            return CreatedAtRoute("GetUser", new { id = userToReturn.Id }, shapedUser);
        }

        [HttpPost("{id}")]
        public IActionResult BlockUserCreation(string id)
        {
            if (_userRepository.UserExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userFromRepo = _userRepository.GetById(id);
            if (userFromRepo == null)
            {
                return NotFound();
            }

            var operation = await _userRepository.DeleteUser(userFromRepo);

            if (!operation.Succeeded)
            {
                throw new Exception($"Deleting a user failed. {operation.Errors.FirstOrDefault().Description}");
            }

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetUsersOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpPut("{id}", Name = "UpdateUser")]
        public async Task<IActionResult> UpdateUser(string id, [FromBody] UserForUpdateDTO user)
        {
            if (user == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var userFromRepo = _userRepository.GetById(id);
            if (userFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(user, userFromRepo);

            var operation = await _userRepository.UpdateUser(userFromRepo);
            if (!operation.Succeeded)
            {
                throw new Exception($"Updating user failed. {operation.Errors.FirstOrDefault().Description}");
            }

            var currentUserRoles = _userRepository.GetRolesForUser(userFromRepo);
            var rolesToRemove = currentUserRoles.Except(user.UserRoles.Select(ur => ur.Name));
            var rolesToAdd = user.UserRoles.Select(ur => ur.Name).Except(currentUserRoles);
            var addOperation = await _userRepository.AddRolesAsync(userFromRepo, rolesToAdd.ToList());
            var removeOperation = await _userRepository.RemoveRolesAsync(userFromRepo, rolesToRemove.ToList());
            if (!addOperation.Succeeded || !removeOperation.Succeeded)
            {
                throw new Exception($"Updating user failed. {addOperation.Errors.FirstOrDefault().Description} {removeOperation.Errors.FirstOrDefault().Description}");
            }

            return NoContent();
        }
    }
}
