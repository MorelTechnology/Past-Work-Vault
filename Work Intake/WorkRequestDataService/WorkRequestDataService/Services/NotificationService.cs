using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using WorkRequestDataService.Models;

namespace WorkRequestDataService.Services
{
    /// <summary>
    /// The Application Role type
    /// </summary>
    public enum Role
    {
        /// <summary> Application Administrators </summary>
        ApplicationAdmin = 0,

        /// <summary> Users Denied Access </summary>
        DeniedAccess = 1,

        /// <summary> Digital Strategy Leads </summary>
        DigitalStrategy = 2,

        /// <summary> Enviromnent Administrators </summary>
        EnvironmentAdmin = 3,

        /// <summary> Users with General Access </summary>
        GeneralAccess = 4,

        /// <summary> Portfolio Management Leads </summary>
        PortfolioManagement = 5,

        /// <summary> Product Owners </summary>
        ProductOwner = 6
    }

    /// <summary>
    /// Notification Service
    /// Contains methods employed by Public API Controller <c>NotificationController</c>.
    /// </summary>
    ///
    internal class NotificationService
    {
        internal bool Send(Notification notification)
        {
            ConfigurationService configurationService = new ConfigurationService();
            bool success = false;

            SmtpClient mailClient = new SmtpClient(configurationService.GetConfigurationProperty<string>("SMTP_Server"), configurationService.GetConfigurationProperty<int>("SMTP_Port"));
            try
            {
                mailClient.Send(CreateMessage(notification));
                success = true;
            }
            catch (Exception ex)
            {
                Error.Log(ex, "Data sent to method was : " + Utility.objectToSql(notification));
                success = false;
            }
            return success;
        }

        internal MailMessage CreateMessage(Notification notification)
        {
            WorkRequestService workRequestService = new WorkRequestService();
            UserService userService = new UserService();
            // Retrieve the referenced Work Item details
            WorkRequest item;
            try
            {
                item = workRequestService.GetWorkRequests(
                                    new WorkRequest
                                    { RequestID = notification.WorkRequestId }).First();
            }
            catch (Exception ex)
            {
                // Couldn't get the work request for some reason...
                Error.Log(ex, "Data sent to method was : " + Utility.objectToSql(notification));
                throw;
            }

            MailMessage message = null;
            string header = "<img src = '/Content/images/riverstone-logo.png' "
                + "height ='35%'><br><h2>Digital Strategy Work Request</h2>";
#if DEBUG
            // When running in Development and QA environments, redirect all email messages to the logged-in user.
            message =
                new MailMessage("Work Intake Tool <donotreply@trg.com>",
                userService.GetUserInfo(System.Web.HttpContext.Current.User.Identity.Name)["mail"] as string);
            //message.Body += header;
            message.Body += "<span style='background-color:yellow;'><b>NOTE: This application is in Test mode.</b><br>";
            message.Body += "In Production, this message would have been sent to: "
                + userService.GetUserInfo(notification.NotifyUser)["mail"] + "</span><br><br>";
#else
            message =
            new MailMessage("Work Intake Tool <donotreply@trg.com>",
            userService.GetUserInfo(notification.NotifyUser)["mail"] as string);

#endif
            GetTemplate(notification, item, message);
            return message;
        }

        internal void GetTemplate(Notification notification, WorkRequest item, MailMessage message)
        {
            ConfigurationService configurationService = new ConfigurationService();
            UserService userService = new UserService();
            string itemUrl = String.Format(configurationService.GetConfigurationProperty<string>("Single_Request_Url"), item.RequestID);
            message.IsBodyHtml = true;
            switch (notification.Template)
            {
                case Template.ReadyForScheduling:
                    {
                        string requestor = userService.GetUserInfo(item.Requestor)["displayName"] as string;
                        message.Subject = "Information: Request " + item.RequestID + " is ready for scheduling.";
                        message.Body += "Request Item " + item.RequestID + ", created by <b>" + requestor + "</b>, ";
                        message.Body += "has been marked as ready for scheduling.<br><br>";
                        if (!String.IsNullOrWhiteSpace(notification.Comments))
                            message.Body += "Comments: " + notification.Comments + "<br><br>";
                        message.Body += "<a href = '" + itemUrl + "'>[Click here to review the work request. ]</a>";
                        break;
                    }

                case Template.ManagerSubmission:
                    {
                        string requestor = userService.GetUserInfo(item.Requestor)["displayName"] as string;
                        message.Subject = "Your action is required: Review Request Item " + item.RequestID;
                        message.Body += "A new Work Request Item has been created by <b>" + requestor + "</b> ";
                        message.Body += "and assigned to you for review.<br><br>";
                        message.Body += "<a href = '" + itemUrl + "'>[Click here to review the work request. ]</a>";
                        break;
                    }

                case Template.PortfolioManagerSubmission:
                    {
                        string requestor = userService.GetUserInfo(item.Requestor)["displayName"] as string;
                        string approver = userService.GetUserInfo(item.Manager)["displayName"] as string;
                        message.Subject = "Your action is required: Review Request Item " + item.RequestID;
                        message.Body += "A Work Request Item, created by <b>" + requestor + "</b> ";
                        message.Body += "and approved by <b>" + approver + "</b> ";
                        message.Body += "has been assigned to you for review.<br><br>";
                        if (!String.IsNullOrWhiteSpace(notification.Comments))
                            message.Body += "Comments: " + notification.Comments + "<br><br>";
                        message.Body += "<a href = '" + itemUrl + "'>[Click here to review the work request. ]</a>";
                        break;
                    }

                case Template.DigitalStrategySubmission:
                    {
                        string requestor = userService.GetUserInfo(item.Requestor)["displayName"] as string;
                        string approver = userService.GetUserInfo(item.Manager)["displayName"] as string;
                        string submittingUser = userService.GetUserInfo(HttpContext.Current.User.Identity.Name)["displayName"] as string;
                        message.Subject = $"Your action is required: Review Request Item {item.RequestID}";
                        message.Body += $"A Work Request Item was created by <b>{requestor}</b> " +
                            $"and approved by <b>{approver}</b>. It has been sent to the Digital Strategy Leadership team " +
                            $"by <b>{submittingUser}</b>. Please review this request.<br><br>";
                        if (!String.IsNullOrWhiteSpace(notification.Comments))
                            message.Body += $"Comments: {notification.Comments}<br><br>";
                        message.Body += $"<a href='{itemUrl}'>[Click here to review the work request. ]</a>";
                        break;
                    }

                case Template.ReturnToAssociate:
                    {
                        string rejectionUser = userService.GetUserInfo(HttpContext.Current.User.Identity.Name)["displayName"] as string;
                        message.Subject = "Returned: Work Request Item " + notification.WorkRequestId;
                        message.Body += "Your request has been returned by <b>" + rejectionUser + "</b>";
                        message.Body += "<br><br>";
                        if (!String.IsNullOrWhiteSpace(notification.Comments))
                            message.Body += "Comments: " + notification.Comments + "<br><br>";
                        message.Body += "<a href = '" + itemUrl + "'>[Click here to review or modify the work request. ]</a>";
                        break;
                    }

                case Template.Approved:
                    {
                        message.Subject = "Ready for Prioritization: Work Request Item " + notification.WorkRequestId;
                        message.Body += "Work Request Item " + notification.WorkRequestId + " has been authorized as ready for prioritization.<br><br>";
                        if (!String.IsNullOrWhiteSpace(notification.Comments))
                            message.Body += "Comments: " + notification.Comments + "<br><br>";
                        message.Body += "<a href = '" + itemUrl + "'>[Click here to review the work request. ]</a>";
                        break;
                    }

                case Template.Cancelled:
                    {
                        string requestor = userService.GetUserInfo(item.Requestor)["displayName"] as string;
                        message.Subject = "Cancelled: Request Item " + notification.WorkRequestId;
                        message.Body += "Work Request Item " + notification.WorkRequestId + ", created by <b>" + requestor + "</b>, ";
                        message.Body += "has been cancelled.<br><br>";
                        message.Body += "<a href = '" + itemUrl + "'>[Click here to review the work request. ]</a>";
                        break;
                    }

                case Template.Denied:
                    {
                        string rejectionUser = userService.GetUserInfo(HttpContext.Current.User.Identity.Name)["displayName"] as string;
                        message.Subject = "Denied: Work Request Item " + notification.WorkRequestId;
                        message.Body += "Your request has been denied by <b>" + rejectionUser + "</b>";
                        message.Body += "<br><br>";
                        if (!String.IsNullOrWhiteSpace(notification.Comments))
                            message.Body += "Comments: " + notification.Comments + "<br><br>";
                        message.Body += "<a href = '" + itemUrl + "'>[Click here to review the work request. ]</a>";
                        break;
                    }
                case Template.Restored:
                    {
                        string requestor = userService.GetUserInfo(item.Requestor)["displayName"] as string;
                        message.Subject = "Restored: Request Item " + notification.WorkRequestId;
                        message.Body += "Work Request Item " + notification.WorkRequestId + ", created by <b>" + requestor + "</b>, ";
                        message.Body += "and previously marked as 'denied' has been restored. ";
                        message.Body += "The request will be re-submitted to the Digital Strategy Team.<br><br>";
                        message.Body += "<a href = '" + itemUrl + "'>[Click here to reiew The Work Request. ]</a>";
                        break;
                    }
                default:
                    {
                        throw new InvalidOperationException("No Corresponding Template exists for supplied value (" + notification.Template + ").");
                    }
            }
        }

        internal string[] GetEmailsForRole(Role role)
        {
            List<string> emails = new List<string>();

            switch (role)
            {
                case Role.ApplicationAdmin:
                    foreach (string group in new ConfigurationService()
                          .GetConfigurationProperty<List<string>>("User_Groups_Application_Admin"))
                    {
                        try { emails.Add(new UserService().GetUserInfo(group)["mail"].ToString()); }
                        catch (KeyNotFoundException)
                        {
                            var members = new UserService().GetGroupMembers(new List<string> { group });
                            foreach (string member in members) emails.Add(new UserService().GetUserInfo(member)["mail"].ToString());
                        }
                    }
                    break;

                case Role.DeniedAccess:
                    foreach (string group in new ConfigurationService()
                          .GetConfigurationProperty<List<string>>("User_Groups_Denied_Access"))
                    {
                        try { emails.Add(new UserService().GetUserInfo(group)["mail"].ToString()); }
                        catch (KeyNotFoundException)
                        {
                            var members = new UserService().GetGroupMembers(new List<string> { group });
                            foreach (string member in members) emails.Add(new UserService().GetUserInfo(member)["mail"].ToString());
                        }
                    }
                    break;

                case Role.DigitalStrategy:
                    foreach (string group in new ConfigurationService()
                        .GetConfigurationProperty<List<string>>("User_Groups_Digital_Strategy"))
                    {
                        try { emails.Add(new UserService().GetUserInfo(group)["mail"].ToString()); }
                        catch (KeyNotFoundException)
                        {
                            var members = new UserService().GetGroupMembers(new List<string> { group });
                            foreach (string member in members) emails.Add(new UserService().GetUserInfo(member)["mail"].ToString());
                        }
                    }
                    break;

                case Role.EnvironmentAdmin:
                    foreach (string group in new ConfigurationService()
                          .GetConfigurationProperty<List<string>>("User_Groups_Environment_Admin"))
                    {
                        try { emails.Add(new UserService().GetUserInfo(group)["mail"].ToString()); }
                        catch (KeyNotFoundException)
                        {
                            var members = new UserService().GetGroupMembers(new List<string> { group });
                            foreach (string member in members) emails.Add(new UserService().GetUserInfo(member)["mail"].ToString());
                        }
                    }
                    break;

                case Role.GeneralAccess:
                    foreach (string group in new ConfigurationService()
                          .GetConfigurationProperty<List<string>>("User_Groups_General_Access"))
                    {
                        try { emails.Add(new UserService().GetUserInfo(group)["mail"].ToString()); }
                        catch (KeyNotFoundException)
                        {
                            var members = new UserService().GetGroupMembers(new List<string> { group });
                            foreach (string member in members) emails.Add(new UserService().GetUserInfo(member)["mail"].ToString());
                        }
                    }
                    break;

                case Role.PortfolioManagement:
                    foreach (string group in new ConfigurationService()
                        .GetConfigurationProperty<List<string>>("User_Groups_Portfolio_Manager"))
                    {
                        try { emails.Add(new UserService().GetUserInfo(group)["mail"].ToString()); }
                        catch (KeyNotFoundException)
                        {
                            var members = new UserService().GetGroupMembers(new List<string> { group });
                            foreach (string member in members) emails.Add(new UserService().GetUserInfo(member)["mail"].ToString());
                        }
                    }
                    break;

                case Role.ProductOwner:
                    foreach (string group in new ConfigurationService()
                          .GetConfigurationProperty<List<string>>("User_Groups_Product_Owner"))
                    {
                        try { emails.Add(new UserService().GetUserInfo(group)["mail"].ToString()); }
                        catch (KeyNotFoundException)
                        {
                            var members = new UserService().GetGroupMembers(new List<string> { group });
                            foreach (string member in members) emails.Add(new UserService().GetUserInfo(member)["mail"].ToString());
                        }
                    }
                    break;
            }
            return emails.ToArray<string>();
        }
        internal string[] GetEmailsForRoles (Role[] roles)
        {
            List<string> allEmails = new List<string>();
            foreach (Role role in roles)
                allEmails.AddRange(GetEmailsForRole(role));
            return allEmails.ToArray<string>();

        }
    }
}