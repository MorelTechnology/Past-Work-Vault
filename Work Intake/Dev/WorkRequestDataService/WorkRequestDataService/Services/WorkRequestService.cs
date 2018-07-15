using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Runtime.InteropServices;
using WorkRequestDataService.Models;

namespace WorkRequestDataService.Services
{
    internal class WorkRequestService
    {
        #region Private Fields

        private Dao dao = new Dao();

        #endregion Private Fields

        #region Internal Methods

        internal int CreateWorkRequest(WorkRequest input)
        {
            try { return dao.AddWorkRequest(input); }
            catch (Exception ex)
            {
                Error.Log(ex, "Data sent to method was : " + Utility.objectToSql(input));
                throw;
            }
        }

        internal int DeleteWorkRequest(int workRequestId, bool isTest)
        {
            try
            {
                return dao.DeleteWorkRequest(workRequestId, isTest);
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw;
            }
        }

        internal WorkRequest[] GetUserAssignments()
        {

            UserService userService = new UserService();
            ConfigurationService configurationService = new ConfigurationService();
            try
            {
            var ctx = new PrincipalContext(ContextType.Domain);
            var user = UserPrincipal.FindByIdentity(ctx, IdentityType.SamAccountName, System.Web.HttpContext.Current.User.Identity.Name);
            var productOwnerGroups = configurationService.GetConfigurationProperty<List<string>>("User_Groups_Product_Owner").ToArray<string>();
            var digitalStrategyGroups = configurationService.GetConfigurationProperty<List<string>>("User_Groups_Digital_Strategy").ToArray<string>();
            var portfolioManagementGroups = configurationService.GetConfigurationProperty<List<string>>("User_Groups_Portfolio_Manager").ToArray<string>();
            var results = new List<WorkRequest>();

            // check user type upfront for efficiency
            bool userIsPortfolioManager = userService.IsUserMemberOf(ctx, user, portfolioManagementGroups);
            bool userIsDigitalStrategyUser = userService.IsUserMemberOf(ctx, user, digitalStrategyGroups);
            bool userIsProductOwner = userService.IsUserMemberOf(ctx, user, productOwnerGroups);

            // If the user is a member of any portfolio management groups, get and append requests that need review.
            if (userIsPortfolioManager)
                results.AddRange(GetWorkRequests(new WorkRequest { Status = WorkRequestStatus.SubmittedToPortfolioManager.Description() }));
            // If the user is a member of any digital strategy groups, get and append the requests that need review.
            if (userIsDigitalStrategyUser)
                results.AddRange(GetWorkRequests(new WorkRequest { Status = WorkRequestStatus.SubmittedToDigitalStrategy.Description() }));
            // If the user is a product owner, get items which have been submitted to them.
            if (userIsProductOwner)
                results.AddRange(GetWorkRequests(new WorkRequest
                {
                    Status = WorkRequestStatus.SubmittedToProductOwner.Description(),
                    Manager = user.Sid.ToString()
                }));
            // Get any Items the user created that have been returned to them.
            results.AddRange(GetWorkRequests(new WorkRequest
            {
                Status = WorkRequestStatus.ReturnedToAssociate.Description(),
                Requestor = user.Sid.ToString()
            }));
                return results.Count < 1 ? new WorkRequest[0] : results.ToArray<WorkRequest>();
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                return new WorkRequest[0];
            }
        }

        internal WorkRequest[] GetUserSubmissions(bool omitEndStageRequests)
        {
            try
            {
                var results = new List<WorkRequest>();
                string userSid = ((System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity).User.ToString();
                results.AddRange(dao.GetWorkRequest(new WorkRequest { Requestor = userSid }));
                if (omitEndStageRequests)
                {
                    results.RemoveAll(r => r.Status == WorkRequestStatus.ReadyForPrioritization.Description()
                                        || r.Status == WorkRequestStatus.ReadyForScheduling.Description());
                }
                return results.Count < 1 ? new WorkRequest[0] : results.ToArray<WorkRequest>();
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                return new WorkRequest[0];
            }

        }

        internal object GetWorkRequestComments(int workRequestId)
        {
            try
            {
                return dao.GetWorkRequestComments(workRequestId);
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw;
            }
        }

        internal WorkRequest[] GetWorkRequests([Optional]WorkRequest matchOnValues)
        {
            try
            {
                if (matchOnValues != null) return dao.GetWorkRequest(matchOnValues);
                else return dao.GetWorkRequest();
            }
            catch (Exception ex)
            {
                if (matchOnValues != null)
                    Error.Log(ex, "Data sent to method was : " + Utility.objectToSql(matchOnValues));
                else
                    Error.Log(ex);
                throw;
            }
        }

        internal bool RejectRequest(Rejection rejection)
        {
            bool result = false;
            try
            {
                WorkRequest workRequest = GetWorkRequests(new WorkRequest
                { RequestID = rejection.WorkRequestId }).First();

                if (rejection.IsDenial) workRequest.Status = "Denied";
                else workRequest.Status = "ReturnedToAssociate";
                workRequest.StatusDate = DateTime.Now;
                UpdateWorkRequest(workRequest, false);

                if (!String.IsNullOrWhiteSpace(rejection.Comment))
                    dao.AddComment(new Comment { WorkRequestId = rejection.WorkRequestId, CommentDate = workRequest.StatusDate.Value, CommentText = rejection.Comment });
                result = true;
            }
            catch (Exception ex)
            {
                Error.Log(ex, "Data sent to method was : " + Utility.objectToSql(rejection));
                throw;
            }

            return result;
        }

        internal int UpdateWorkRequest(WorkRequest input, bool isTest)
        {
            List<Variance> whatChanged = GetWorkRequests(
                                    new WorkRequest
                                    { RequestID = input.RequestID }).First().DetailedCompare(input);

            try
            {
                return dao.UpdateWorkRequest(input, isTest);
            }
            catch (Exception ex)
            {
                Error.Log(ex, "Data sent to method was : " + Utility.objectToSql(input));
                throw;
            }
        }

        #endregion Internal Methods
    }
}