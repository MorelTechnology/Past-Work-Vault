using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.UI;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Admin
{
#pragma warning disable 1591

    [Authorize]
    public partial class Default : System.Web.UI.Page
    {
        //protected void Page_PreInit(object sender, EventArgs e)
        //{
        //    try
        //    { 
        //        {
        //            var msg = new HttpResponseMessage(HttpStatusCode.Unauthorized)
        //            { ReasonPhrase = "User '" + System.Web.HttpContext.Current.User.Identity.Name + "' is not an admin." };
        //            var ex = new HttpResponseException(msg);

        //            Models.Error.Log(ex, ex.Response.ReasonPhrase);
        //            Response.Write("Message: " + ex.Message + "<br>Reason: " + ex.Response.ReasonPhrase);
        //            Response.End();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        WorkRequestDataService.Models.Error.Log(ex);
        //        throw ex;
        //    }
        //}

        protected void Page_LoadComplete(object sender, EventArgs e)
        {
        }

        protected bool IsEnvironmentAdmin()
        {
            return new UserService().CurrentUserIsEnvironmentAdmin();
        }
    }
}