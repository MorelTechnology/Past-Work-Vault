using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SampleREST
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
                routeTemplate: "api/{controller}/{id}/{property}/{type}",
                defaults: new
                {
                    id = RouteParameter.Optional,
                    property = RouteParameter.Optional,
                    type = RouteParameter.Optional,
                }
            );
        }
    }
}
