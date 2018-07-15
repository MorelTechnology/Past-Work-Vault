using System.Web.Mvc;

namespace WorkRequestDataService
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    public class FilterConfig
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}