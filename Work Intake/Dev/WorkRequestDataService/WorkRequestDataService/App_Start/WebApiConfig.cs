using System.Configuration;
using System.Web.Configuration;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WorkRequestDataService
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public static class WebApiConfig
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static void Register(HttpConfiguration config)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            // Web API configuration and services

            // Get Application settings from Web.Config
            KeyValueConfigurationCollection settings =
                WebConfigurationManager.OpenWebConfiguration("/").AppSettings.Settings;

            // Enable Cross Origin Resource Sharing (PM>Install-Package Microsoft.AspNet.WebApi.Cors)

            var corsSettings = new EnableCorsAttribute(
                origins: settings["AllowedOrigins"].Value,
                methods: settings["AllowedMethods"].Value,
                headers: settings["AllowedHeaders"].Value);

            corsSettings.SupportsCredentials = true; // Allow sending of credentials from the host.
            config.EnableCors(corsSettings);

            // Configure Web API to use only bearer token authentication.
            //   config.SuppressDefaultHostAuthentication();
            //   config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "data/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}