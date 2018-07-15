using System.Web.Http;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Controllers
{
    /// <summary>
    /// This controller provides convenience methods for a consumer application to first determine user
    /// authorization and access rights, in order to streamline user experience.
    /// <B>Note:</B> Generally, controllers throughout this web service inherently require a user to be pre-authorized with
    /// Windows(NTLM) credentials. A non-authorized user attempting to access methods of this service will generally be met with a 401 error.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class AuthController : ApiController
    {
        #region Private Fields

        private UserService userService = new UserService();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Determines whether the user account is granted administrator priviliges.
        /// </summary>
        /// <param name="samAccountName">SAMAccountName (user ID).</param>
        /// <returns>
        ///   <c>true</c> if the specified account has Configuration administrator rights; otherwise, <c>false</c>.
        /// </returns>
        [HttpGet()]
        [Route("data/Auth/isAdmin/{samAccountName}")]
        public bool IsAdmin(string samAccountName)
        {
            return userService.IsUserConfigurationAdmin(samAccountName);
        }

        /// <summary>
        /// Determines whether the supplied user meets the criteria established for
        /// general access to application methods.
        /// </summary>
        /// <param name="samAccountName">The SAMAccountName (user ID) of a user for which
        /// general access rights should be determined.</param>
        /// <returns>
        /// <c>true</c> if the specified user is permitted general access; otherwise, <c>false</c>.
        /// </returns>
        [HttpGet()]
        [Route("data/Auth/isAuthorized/{samAccountName}")]
        public bool IsAuthorized(string samAccountName)
        {
            return userService.IsUserAuthorized(samAccountName);
        }

        /// <summary>
        /// Determines whether the user account is granted administrator priviliges.
        /// </summary>
        /// <param name="samAccountName">SAMAccountName (user ID).</param>
        /// <returns>
        ///   <c>true</c> if the specified account has Environment administrator rights; otherwise, <c>false</c>.
        /// </returns>
        [HttpGet()]
        [Route("data/Auth/isEnvironmentAdmin/{samAccountName}")]
        public bool IsEnvironmentAdmin(string samAccountName)
        {
            return userService.IsUserEnviromnentAdmin(samAccountName);
        }

        #endregion Public Methods
    }
}