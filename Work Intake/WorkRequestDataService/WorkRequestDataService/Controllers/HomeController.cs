using System.Web.Mvc;

namespace WorkRequestDataService.Controllers
{
    /// <summary>
    /// Default Controller
    /// Used as a vehicle to load documentation UI
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class HomeController : Controller
    {
        #region Public Methods

        /// <summary>
        /// Calls Index which redirects to UI.
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        #endregion Public Methods
    }
}