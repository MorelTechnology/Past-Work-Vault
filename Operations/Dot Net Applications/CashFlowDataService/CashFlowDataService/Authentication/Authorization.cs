using CashFlowDataService.Models;
using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Web.Http;

namespace CashFlowDataService.Authentication
{
    public class Authorization : AuthorizeAttribute
    {
        private UserPrincipal currentUser = UserPrincipal.Current;

        public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            if (AuthorizeRequest(actionContext))
            {
                return;
            }
            HandleUnauthorizedRequest(actionContext);
        }

        protected override void HandleUnauthorizedRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            var response = actionContext.ControllerContext.Request
                .CreateResponse(HttpStatusCode.Unauthorized, currentUser.DisplayName + " is not authorized to use this application.");
            HttpResponseException ex = new HttpResponseException(response);

            //log error.
            DataAccess.addError(currentUser.Sid.ToString(), currentUser.GivenName + " "
                + currentUser.Surname, "Authentication", response.StatusCode + ": "
                + response.RequestMessage);
            throw ex;
        }

        private bool AuthorizeRequest(System.Web.Http.Controllers.HttpActionContext actionContext)
        {
            //Determine if requesting user is a valid application user.  (In this case we check the App's User Table).
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            parameters.Add("ActiveDirectoryID", currentUser.Sid.ToString());
            var result = DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_IsUserActive", parameters).Rows[0].ItemArray[0];
            return Convert.ToBoolean(result);
        }
    }
}