using System.Data;
using WorkRequestDataService.Models;

namespace WorkRequestDataService.Services
{
    /// <summary>
    /// Error Service
    /// Contains methods employed by Public API Controller <c>ErrorController</c>.
    /// </summary>
    internal class ErrorService
    {
        #region Internal Methods

        internal void AddError(Error error)
        {
            Dao dao = new Dao();
            dao.AddError(error);
        }

        internal DataTable GetErrors()
        {
            Dao dao = new Dao();
            return dao.GetErrors();
        }

        #endregion Internal Methods
    }
}