using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CommutationCentral.API.Entities;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CommutationCentral.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var db = services.GetRequiredService<CommutationCentralContext>();
                    db.EnsureSeedDataForContext();
                    var identityDb = services.GetRequiredService<IdentityContext>();
                    var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    identityDb.EnsureSeedDataForContext(roleManager, userManager);
                }
                catch (Exception ex)
                {
                    var dLogger = services.GetRequiredService<ILogger<Program>>();
                    dLogger.LogError(ex, "An error occurred while migrating the database.");
                }
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .Build();
    }
}
