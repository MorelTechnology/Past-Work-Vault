using System;

namespace WorkRequestDataService.Models
{
    /// <summary>
    /// Represents a Work Request Comment Entry
    /// </summary>
    public class Comment
    {
        #region Public Properties

        /// <value>The date of the comment. Set automatically for new comments.</value>
        public DateTime CommentDate { get; set; }

        /// <value>The text of the comment.</value>
        public string CommentText { get; set; }

        /// <value>The user sid of the person who made the comment.  Set automatically for new comments.</value>
        public string CommentUser { get; set; } = ((System.Security.Principal.WindowsIdentity)System.Web.HttpContext.Current.User.Identity).User.ToString();

        /// <value>The work request identifier.</value>
        public int WorkRequestId { get; set; }

        #endregion Public Properties
    }
}