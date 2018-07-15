using System.Linq;
using System.Web.Http;
using WorkRequestDataService.Models;
using WorkRequestDataService.Services;

namespace WorkRequestDataService.Controllers
{
    /// <summary>
    /// This controller provides methods for interacting with Work Request System data.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    [Authorize]
    public class WorkRequestController : ApiController
    {
        #region Private Fields

        private WorkRequestService workRequestService = new WorkRequestService();

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Creates the specified input.
        /// Note: Some optional of the<c>WorkRequest</c>, such as RequestID and Created Date
        /// are not required input, and will be overridden at creation time by the system, if supplied.
        /// </summary>
        /// <param name="input">A <see cref="WorkRequest"/> object for creation. </param>
        /// <returns><see cref="int"/>The RequestID of the newly created request.</returns>
        [HttpPost()]
        [Route("data/WorkRequest/Create")]
        public int Create([FromBody]WorkRequest input)
        {
            //might need to be a string for input?
            return workRequestService.CreateWorkRequest(input);
        }

        /// <summary>
        /// Deletes the work request item.
        /// </summary>
        /// <param name="workRequestId">The ID of the work request to delete.</param>
        /// <returns><see cref="int"/>The ID of the work request that was deleted.</returns>
        [HttpPost()]
        [Route("data/WorkRequest/DeleteWorkRequestItem/")]
        public int DeleteWorkRequestItem([FromBody]int workRequestId)
        {
            return workRequestService.DeleteWorkRequest(workRequestId, false);
        }

        /// <summary>
        /// Gets all work requests.
        /// </summary>
        /// <returns>An array of <see cref="WorkRequest"/> objects./// </returns>
        [HttpGet()]
        [Route("data/WorkRequest/GetAllWorkRequests")]
        public object GetAllWorkRequests()
        {
            return workRequestService.GetWorkRequests();
        }

        /// <summary>
        /// Gets the comments for the given WorkRequest.
        /// </summary>
        /// <param name="workRequestId">An <see cref="int"/> representing the RequestID for the WorkRequest for which comments should be retrieved.</param>
        /// <returns>An array of <see cref="Comment"/> items associated with the given WorkRequest.</returns>
        [HttpGet()]
        [Route("data/WorkRequest/GetComments/{workRequestId}")]
        public object GetComments(int workRequestId)
        {
            return workRequestService.GetWorkRequestComments(workRequestId);
        }

        /// <summary>
        /// This method returns all work requests which match on all values provided.  This can be used, for
        /// instance, to return a single work request by id, or all by a given requestor.  When multiple values
        /// are passed in, only work requests which match all criteria will be returned.
        /// </summary>
        /// <param name="matchOnValues">a <see cref="WorkRequest" /> object used to define criteria for matches returned.</param>
        /// <returns>
        /// Array of <see cref="WorkRequest"/> objects whose values match the example provided or <c>null</c>.
        /// </returns>
        [HttpPost()]
        [Route("data/WorkRequest/GetFilteredWorkRequests/")]
        public object GetFilteredWorkRequests([FromBody]WorkRequest matchOnValues)
        {
            return workRequestService.GetWorkRequests(matchOnValues);
        }

        /// <summary>
        /// Gets Work Requests for which some action from the current user is expected.
        /// </summary>
        /// <returns>An array of <see cref="WorkRequest"/> objects.</returns>
        [HttpGet()]
        [Route("data/WorkRequest/GetMyAssignments")]
        public object GetMyAssignments()
        {
            return workRequestService.GetUserAssignments();
        }

        /// <summary>
        /// Gets Work Request Entries submitted by the current user, omitting those which
        /// have been reached the end stage. (i.e. Approved / Ready for prioritization).
        /// </summary>
        /// <returns>An array of <see cref="WorkRequest"/> where the current user is the <c>Requestor</c>.</returns>
        [HttpGet()]
        [Route("data/WorkRequest/GetMySubmissions")]
        public object GetMySubmissions()
        {
            //return workRequestService.GetUserSubmissions(true);
            return workRequestService.GetUserSubmissions(false);
        }

        /// <summary>
        /// A convenience method, extending <see cref="GetAllWorkRequests"/>, limiting
        /// the number of entries returned to the specified value, starting from the most
        /// recent submission.
        /// </summary>
        /// <param name="HowMany">An <see cref="int"/> representing the number of items to return.</param>
        /// <returns></returns>
        [HttpGet()]
        [Route("data/WorkRequest/GetTheLast/{HowMany}")]
        public object GetTheLast(int HowMany)
        {
            return workRequestService.GetWorkRequests().OrderByDescending(x => x.RequestID).Take(HowMany);
        }

        /// <summary>
        /// A conveneince method which returns a single WorkRequest with the specified ID number.
        /// </summary>
        /// <param name="workRequestId">The Work Request ID.</param>
        /// <returns>A <see cref="WorkRequest"/> in case of a match, or <c>null</c>.</returns>
        [HttpGet()]
        [Route("data/WorkRequest/GetWorkRequest/{workRequestId}")]
        public WorkRequest GetWorkRequest(int workRequestId)
        {
            return workRequestService.GetWorkRequests(new WorkRequest { RequestID = workRequestId }).FirstOrDefault();
        }

        /// <summary>
        /// Rejects the work Request per specified input.
        /// </summary>
        /// <param name="rejection">A <see cref="Rejection"/> object.</param>
        /// <returns><c>true</c> if a matching entry was found and a rejection was performed; otherwise <c>false</c>. </returns>
        [HttpPost()]
        [Route("data/WorkRequest/RejectWorkRequest/")]
        public object RejectWorkRequest([FromBody]Rejection rejection)
        {
            return workRequestService.RejectRequest(rejection);
        }

        /// <summary>
        /// Updates the work request item using the data given in the input.
        /// </summary>
        /// <param name="workRequest">A <see cref="WorkRequest"/> object indicating required updates.</param>
        /// <returns>An <see cref="int"/> value which indicates the RequestID of the impacted item.</returns>

        [HttpPost()]
        [Route("data/WorkRequest/UpdateWorkRequestItem/")]
        public int UpdateWorkRequestItem([FromBody] WorkRequest workRequest)
        {
            return workRequestService.UpdateWorkRequest(workRequest, false);
        }

        #endregion Public Methods
    }
}