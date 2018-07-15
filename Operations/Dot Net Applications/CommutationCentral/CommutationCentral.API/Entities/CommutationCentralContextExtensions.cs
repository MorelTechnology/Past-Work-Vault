using CommutationCentral.API.Entities.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Entities
{
    public static class CommutationCentralContextExtensions
    {
        public static void EnsureSeedDataForContext(this CommutationCentralContext context)
        {
            // init seed data
            if (!context.ActivityCategories.Any())
            {
                var activityCategories = new List<ActivityCategory>()
                {
                    new ActivityCategory(){Name="Project Scope" },
                    new ActivityCategory(){Name="Coding Issue" },
                    new ActivityCategory(){Name="Data Request" },
                    new ActivityCategory(){Name="Reconciliation Issue" },
                    new ActivityCategory(){Name="Collateral Issue" },
                    new ActivityCategory(){Name="Claims Issue" },
                    new ActivityCategory(){Name="Dispute Issue" },
                    new ActivityCategory(){Name="Finance Issue" },
                    new ActivityCategory(){Name="RI Issue" },
                    new ActivityCategory(){Name="OGC Issue" },
                    new ActivityCategory(){Name="Documentation Request" },
                    new ActivityCategory(){Name="Release Agreement" },
                    new ActivityCategory(){Name="Update Request" },
                    new ActivityCategory(){Name="Housekeeping Issue" },
                    new ActivityCategory(){Name="SOX Issue" },
                    new ActivityCategory(){Name="Other" },
                    new ActivityCategory(){Name="Arrange Meeting" },
                    new ActivityCategory(){Name="Offer" },
                    new ActivityCategory(){Name="Follow Up" },
                    new ActivityCategory(){Name="Question" }
                };
                context.ActivityCategories.AddRange(activityCategories);
                context.SaveChanges();
            }
        }
    }
}
