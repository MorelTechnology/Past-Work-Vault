using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using WorkRequestDataService.Models;

namespace WorkRequestDataService
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class ApplicationUserManager : UserManager<ApplicationUser>
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public ApplicationUserManager(IUserStore<ApplicationUser> store)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            : base(store)
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<ApplicationUser>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };
            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                manager.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            }
            return manager;
        }
    }
}