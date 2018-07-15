using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(WorkRequestDataService.Startup))]

namespace WorkRequestDataService
{
#pragma warning disable 1591

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}