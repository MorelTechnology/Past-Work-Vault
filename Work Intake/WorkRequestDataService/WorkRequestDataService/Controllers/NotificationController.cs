using System.Web.Http;
using WorkRequestDataService.Models;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Controllers
{
    /// <summary>
    /// This controller provides methods for interacting with Work Request System Notifications.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class NotificationController : ApiController
    {
        #region Private Fields

        private NotificationService notificationService = new NotificationService();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Performs a notification action based on input specification.
        /// </summary>
        /// <param name="notification">A <see cref="Notification"/> object.</param>
        /// <returns></returns>
        [HttpPost()]
        [Route("data/Notification/")]
        public bool Notify([FromBody] Notification notification)
        {
            return notificationService.Send(notification);
        }
        /// <summary>
        /// Attempts to get DISTRIBUTION GROUP email address(es) for AD Groups associated with a given application role.
        /// If distribution group addresses could not be obtained, the method will attempt to retrieve values for the individual
        /// users within.
        /// </summary>
        /// <param name="role">A <see cref="Role"/> value as it relates to this application.</param>
        /// <returns>E-mail addresses associated with the specified role.</returns>
        [HttpGet()]
        [Route("data/Notificiation/GetEmailsForRole/{role}")]
        public string[] GetEmailsForRole(Role role)
        {
            return notificationService.GetEmailsForRole(role);
        }
        /// <summary>
        /// Extension method of <see cref="GetEmailsForRole(Role)"/> which accepts an array role values as POST body.
        /// </summary>
        /// <param name="roles">An array of <see cref="Role"/> values</param>
        /// <returns>E-mail addresses associated with the specified role(s).</returns>
        [HttpPost()]
        [Route("data/Notification/GetEmailsForRoles")]
        public string[] GetEmailsForRoles([FromBody] Role[] roles)
        {
            return notificationService.GetEmailsForRoles(roles);
        }

        #endregion Public Methods
    }
}