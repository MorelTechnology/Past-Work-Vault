using CommutationCentral.API.Entities;
using CommutationCentral.API.Entities.Lookups;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Lookups;
using CommutationCentral.API.Models.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Services.Lookups
{
    public class ActivityCategoryRepository : Repository<ActivityCategory>, IActivityCategoryRepository
    {
        public ActivityCategoryRepository(CommutationCentralContext context,
            IPropertyMappingService propertyMappingService) : base(context, propertyMappingService)
        {
        }

        public PagedList<ActivityCategory> GetActivityCategories(ActivityCategoryResourceParameters activityCategoryResourceParameters)
        {
            var collectionBeforePaging = _context.ActivityCategories
                .ApplySort(activityCategoryResourceParameters.OrderBy, _propertyMappingService.GetPropertyMapping<ActivityCategoryDTO, ActivityCategory>());

            if (!string.IsNullOrEmpty(activityCategoryResourceParameters.SearchQuery))
            {
                // trim & ignore casing
                var searchQueryForWhereClause = activityCategoryResourceParameters.SearchQuery.Trim().ToLowerInvariant();

                collectionBeforePaging = collectionBeforePaging
                    .Where(ac => ac.Name.ToLowerInvariant().Contains(searchQueryForWhereClause));
            }

            return PagedList<ActivityCategory>.Create(collectionBeforePaging, activityCategoryResourceParameters.PageNumber,
                activityCategoryResourceParameters.PageSize);
        }

        public IEnumerable<ActivityCategory> GetActivityCategories(IEnumerable<int> activityCategoryIds)
        {
            return _context.ActivityCategories.Where(ac => activityCategoryIds.Contains(ac.Id))
                .OrderBy(ac => ac.Name)
                .ToList();
        }
    }
}
