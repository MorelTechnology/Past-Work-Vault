using System.Web.Mvc;
using System.Web.Routing;

namespace WorkRequestDataService
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class RouteConfig
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static void RegisterRoutes(RouteCollection routes)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}