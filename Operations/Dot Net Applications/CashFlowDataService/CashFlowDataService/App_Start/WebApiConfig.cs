using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace CashFlowDataService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "data/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            // Require (Custom) Authorization.
            config.Filters.Add(new Authentication.Authorization());
        }
    }
}