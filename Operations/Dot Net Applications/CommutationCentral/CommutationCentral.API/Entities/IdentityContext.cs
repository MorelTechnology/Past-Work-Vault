using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommutationCentral.API.Entities
{
    public class IdentityContext : IdentityDbContext<ApplicationUser, ApplicationRole, string, IdentityUserClaim<string>,
        ApplicationUserRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public IdentityContext(DbContextOptions<IdentityContext> options)
            : base(options)
        {
            Database.Migrate();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>(b =>
            {
                b.HasMany(x => x.UserRoles).WithOne().HasForeignKey(ur => ur.UserId).IsRequired();
                b.HasMany(u => u.UserClaims).WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            });
            builder.Entity<ApplicationUserRole>(b =>
            {
                b.HasOne(aur => aur.User).WithMany(u => u.UserRoles).HasForeignKey(aur => aur.UserId);
                b.HasOne(aur => aur.Role).WithMany(r => r.UserRoles).HasForeignKey(aur => aur.RoleId);
            });
            builder.Entity<ApplicationRole>(b =>
            {
                b.HasMany(r => r.RoleClaims).WithOne().HasForeignKey(uc => uc.RoleId).IsRequired();
            });
        }
    }
}
