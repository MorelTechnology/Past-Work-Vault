using System.Collections.Generic;
using System.Web.Http;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Controllers
{
    /// <summary>
    /// This controller provides methods for interacting with Work Request System User data.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class UserController : ApiController
    {
        #region Private Fields

        private UserService userService = new UserService();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Gets the current user.
        /// </summary>
        /// <returns><see cref="Dictionary{TKey, TValue}"/> Current user Profile</returns>
        [HttpGet()]
        [Route("data/User/GetCurrentUser/")]
        public Dictionary<string, object> GetCurrentUser()
        {
            return userService.GetUserInfo(User.Identity.Name);
        }

        /// <summary>
        /// Gets a user.
        /// </summary>
        /// <param name="userIdentifier">A <see cref="string"/> of user identifier; Either SAMAccountName or User SID.</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("data/User/GetUser/{userIdentifier?}")]
        public Dictionary<string, object> GetUser(string userIdentifier = "")
        {
            return userService.GetUserInfo(userIdentifier);
        }

        #endregion Public Methods
    }
}