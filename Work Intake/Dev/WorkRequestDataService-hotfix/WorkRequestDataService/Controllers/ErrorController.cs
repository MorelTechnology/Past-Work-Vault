using System.Data;
using System.Web.Http;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Controllers
{
    /// <summary>
    /// This controller provides methods for interacting with Work Request System Error data.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class ErrorController : ApiController
    {
        #region Private Fields

        private ErrorService errorService = new ErrorService();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Gets the error list.
        /// </summary>
        /// <returns><see cref="DataTable"/> of <see cref="WorkRequestDataService.Models.Error"/> objects.</returns>
        [HttpGet()]
        [Route("data/Error/GetErrorList")]
        public DataTable GetErrorList()
        {
            return errorService.GetErrors();
        }

        #endregion Public Methods
    }
}