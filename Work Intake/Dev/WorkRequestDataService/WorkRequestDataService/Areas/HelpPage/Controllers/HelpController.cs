using System;
using System.Web.Http;
using System.Web.Mvc;
using WorkRequestDataService.Areas.HelpPage.ModelDescriptions;
using WorkRequestDataService.Areas.HelpPage.Models;

namespace WorkRequestDataService.Areas.HelpPage.Controllers
{
    /// <summary>
    /// The controller that will handle requests for the help page.
    /// </summary>
    public class HelpController : Controller
    {
        private const string ErrorViewName = "Error";

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public HelpController()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
            : this(GlobalConfiguration.Configuration)
        {
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public HelpController(HttpConfiguration config)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            Configuration = config;
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public HttpConfiguration Configuration { get; private set; }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public ActionResult Index()
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            ViewBag.DocumentationProvider = Configuration.Services.GetDocumentationProvider();
            return View(Configuration.Services.GetApiExplorer().ApiDescriptions);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public ActionResult Api(string apiId)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (!String.IsNullOrEmpty(apiId))
            {
                HelpPageApiModel apiModel = Configuration.GetHelpPageApiModel(apiId);
                if (apiModel != null)
                {
                    return View(apiModel);
                }
            }

            return View(ErrorViewName);
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

        public ActionResult ResourceModel(string modelName)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (!String.IsNullOrEmpty(modelName))
            {
                ModelDescriptionGenerator modelDescriptionGenerator = Configuration.GetModelDescriptionGenerator();
                ModelDescription modelDescription;
                if (modelDescriptionGenerator.GeneratedModels.TryGetValue(modelName, out modelDescription))
                {
                    return View(modelDescription);
                }
            }

            return View(ErrorViewName);
        }
    }
}