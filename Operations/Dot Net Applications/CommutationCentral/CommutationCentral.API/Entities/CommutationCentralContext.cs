using CommutationCentral.API.Entities.Lookups;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Entities
{
    public class CommutationCentralContext : DbContext
    {
        public CommutationCentralContext(DbContextOptions<CommutationCentralContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        public DbSet<ActivityCategory> ActivityCategories { get; set; }
    }
}
