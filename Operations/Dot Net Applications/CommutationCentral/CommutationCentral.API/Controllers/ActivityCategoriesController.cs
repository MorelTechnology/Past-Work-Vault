using AutoMapper;
using CommutationCentral.API.Entities.Lookups;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Lookups;
using CommutationCentral.API.Models.Lookups;
using CommutationCentral.API.Services;
using CommutationCentral.API.Services.Lookups;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Controllers
{
    [Route("api/ActivityCategories")]
    //[Authorize(Policy = "AppUsers")]
    public class ActivityCategoriesController : Controller
    {
        private IActivityCategoryRepository _activityCategoryRepository;
        private IUrlHelper _urlHelper;
        private IPropertyMappingService _propertyMappingService;
        private ITypeHelperService _typeHelperService;

        public ActivityCategoriesController(IActivityCategoryRepository activityCategoryRepository,
            IUrlHelper urlHelper,
            IPropertyMappingService propertyMappingService,
            ITypeHelperService typeHelperService)
        {
            _activityCategoryRepository = activityCategoryRepository;
            _urlHelper = urlHelper;
            _propertyMappingService = propertyMappingService;
            _typeHelperService = typeHelperService;
        }

        [HttpGet(Name = "GetActivityCategories")]
        public IActionResult GetActivityCategories(ActivityCategoryResourceParameters activityCategoryResourceParameters)
        {
            if (!_propertyMappingService.ValidMappingExistsFor<ActivityCategoryDTO, ActivityCategory>(activityCategoryResourceParameters.OrderBy))
            {
                return BadRequest();
            }

            if (!_typeHelperService.TypeHasProperties<ActivityCategoryDTO>(activityCategoryResourceParameters.Fields))
            {
                return BadRequest();
            }

            var activityCategoriesFromRepo = _activityCategoryRepository.GetActivityCategories(activityCategoryResourceParameters);

            var activityCategories = Mapper.Map<IEnumerable<ActivityCategoryDTO>>(activityCategoriesFromRepo);

            var previousPageLink = activityCategoriesFromRepo.HasPrevious ?
                CreateActivityCategoriesResourceUri(activityCategoryResourceParameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = activityCategoriesFromRepo.HasNext ?
                CreateActivityCategoriesResourceUri(activityCategoryResourceParameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink,
                totalCount = activityCategoriesFromRepo.TotalCount,
                pageSize = activityCategoriesFromRepo.PageSize,
                currentPage = activityCategoriesFromRepo.CurrentPage,
                totalPages = activityCategoriesFromRepo.TotalPages
            };

            Response.Headers.Add("X-Pagination", Newtonsoft.Json.JsonConvert.SerializeObject(paginationMetadata));

            var shapedActivityCategories = activityCategories.ShapeData(activityCategoryResourceParameters.Fields);

            return Ok(JsonConvert.SerializeObject(shapedActivityCategories));

            //return Ok(activityCategories.ShapeData(activityCategoryResourceParameters.Fields));
        }

        private string CreateActivityCategoriesResourceUri(ActivityCategoryResourceParameters activityCategoryResourceParameters,
            ResourceUriType type)
        {
            switch(type)
            {
                case ResourceUriType.PreviousPage:
                    return _urlHelper.Link("GetActivityCategories",
                        new
                        {
                            fields = activityCategoryResourceParameters.Fields,
                            orderBy = activityCategoryResourceParameters.OrderBy,
                            searchQuery = activityCategoryResourceParameters.SearchQuery,
                            pageNumber = activityCategoryResourceParameters.PageNumber - 1,
                            pageSize = activityCategoryResourceParameters.PageSize
                        });
                case ResourceUriType.NextPage:
                    return _urlHelper.Link("GetActivityCategories",
                        new
                        {
                            fields = activityCategoryResourceParameters.Fields,
                            orderBy = activityCategoryResourceParameters.OrderBy,
                            searchQuery = activityCategoryResourceParameters.SearchQuery,
                            pageNumber = activityCategoryResourceParameters.PageNumber + 1,
                            pageSize = activityCategoryResourceParameters.PageSize
                        });
                case ResourceUriType.Current:
                default:
                    return _urlHelper.Link("GetActivityCategories",
                        new
                        {
                            fields = activityCategoryResourceParameters.Fields,
                            orderBy = activityCategoryResourceParameters.OrderBy,
                            searchQuery = activityCategoryResourceParameters.SearchQuery,
                            pageNumber = activityCategoryResourceParameters.PageNumber,
                            pageSize = activityCategoryResourceParameters.PageSize
                        });
            }
        }

        [HttpGet("{id}", Name ="GetActivityCategory")]
        public IActionResult GetActivityCategory(int id, string fields)
        {
            if (!_typeHelperService.TypeHasProperties<ActivityCategoryDTO>(fields))
            {
                return BadRequest();
            }

            var activityCategoryFromRepo = _activityCategoryRepository.GetById(id);

            if (activityCategoryFromRepo == null)
            {
                return NotFound();
            }

            var activityCategory = Mapper.Map<ActivityCategoryDTO>(activityCategoryFromRepo);

            var shapedActivityCategory = activityCategory.ShapeData(fields) as IDictionary<string, object>;

            return Ok(JsonConvert.SerializeObject(shapedActivityCategory));
        }

        [HttpPost(Name = "CreateActivityCategory")]
        public IActionResult CreateActivityCategory([FromBody] ActivityCategoryForCreationDTO activityCategory)
        {
            if (activityCategory == null)
            {
                return BadRequest();
            }

            var activityCategoryEntity = Mapper.Map<ActivityCategory>(activityCategory);
            _activityCategoryRepository.AddEntity(activityCategoryEntity);

            if (!_activityCategoryRepository.Save())
            {
                throw new Exception("Creating an activity category failed on save");
            }

            var activityCategoryToReturn = Mapper.Map<ActivityCategoryDTO>(activityCategoryEntity);
            var shapedActivityCategory = activityCategoryToReturn.ShapeData(null) as IDictionary<string, object>;

            return CreatedAtRoute("GetActivityCategory", new { id = activityCategoryToReturn.Id },
                JsonConvert.SerializeObject(shapedActivityCategory));
        }

        [HttpPost("{id}")]
        public IActionResult BlockActivityCategoryCreation(int id)
        {
            if (_activityCategoryRepository.EntityExists(id))
            {
                return new StatusCodeResult(StatusCodes.Status409Conflict);
            }

            return NotFound();
        }

        [HttpDelete("{id}", Name = "DeleteActivityCategory")]
        public IActionResult DeleteActivityCategory(int id)
        {
            var activityCategoryFromRepo = _activityCategoryRepository.GetById(id);
            if (activityCategoryFromRepo == null)
            {
                return NotFound();
            }

            _activityCategoryRepository.DeleteEntity(activityCategoryFromRepo);

            if (!_activityCategoryRepository.Save())
            {
                throw new Exception($"Deleting activity category {id} failed on save.");
            }

            return NoContent();
        }

        [HttpOptions]
        public IActionResult GetActivityCategoriesOptions()
        {
            Response.Headers.Add("Allow", "GET,OPTIONS,POST");
            return Ok();
        }

        [HttpPut("{id}", Name = "UpdateActivityCategory")]
        public IActionResult UpdateActivityCategory(int id, [FromBody] ActivityCategoryForUpdateDTO activityCategory)
        {
            if (activityCategory == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return new UnprocessableEntityObjectResult(ModelState);
            }

            var activityCategoryFromRepo = _activityCategoryRepository.GetById(id);
            if (activityCategoryFromRepo == null)
            {
                return NotFound();
            }

            Mapper.Map(activityCategory, activityCategoryFromRepo);

            _activityCategoryRepository.UpdateEntity(activityCategoryFromRepo);

            if (!_activityCategoryRepository.Save())
            {
                throw new Exception($"Updating activity category {id} failed on save.");
            }

            return NoContent();
        }
    }
}
