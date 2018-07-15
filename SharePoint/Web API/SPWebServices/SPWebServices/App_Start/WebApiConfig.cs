using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SPWebServices
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Enable Cross Origin Resource Sharing (PM>Install-Package Microsoft.AspNet.WebApi.Cors)
            var allowedOrigins = System.Configuration.ConfigurationManager.AppSettings["Allowed_Origins"];
            var corsSettings = new EnableCorsAttribute(origins: allowedOrigins, headers: "*",  methods: "*");

            corsSettings.SupportsCredentials = true; // Allow sending of credentials from the host.
            config.EnableCors(corsSettings);

            // Web API routes
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "defaultController",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

        }
    }
}
