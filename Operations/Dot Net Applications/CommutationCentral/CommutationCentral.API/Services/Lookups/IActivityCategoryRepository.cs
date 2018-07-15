using CommutationCentral.API.Entities.Lookups;
using CommutationCentral.API.Helpers;
using CommutationCentral.API.Helpers.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Services.Lookups
{
    public interface IActivityCategoryRepository : IRepository<ActivityCategory>
    {
        PagedList<ActivityCategory> GetActivityCategories(ActivityCategoryResourceParameters activityCategoryResourceParameters);
        IEnumerable<ActivityCategory> GetActivityCategories(IEnumerable<int> activityCategoryIds);
    }
}
