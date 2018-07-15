using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.Runtime.InteropServices;

namespace CashFlowDataService.Models
{
    internal class DataAccess
    {
        // Use the following connection string when the servuce us able to run as the TRG\CashFlow_svc user.
        // internal const string connectionString = "Data Source = sqltest2012r2; Initial Catalog = CashFlow; Integrated Security=SSPI";

        internal const string connectionString = "Data Source = sqltest2012r2; Initial Catalog = CashFlow; User Id=cashflow; Password=C*s4F1O#Trg17";
        internal static UserPrincipal currentUser = UserPrincipal.Current;
        internal static string userName = currentUser.SamAccountName.ToUpper();

        internal static DataTable executeStoredProc(string connectionString, string storedProcName, Object parameters)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = storedProcName;
                        command.CommandType = CommandType.StoredProcedure;
                        if (parameters != null)
                        {
                            if (parameters is Dictionary<string, object>)
                            {
                                foreach (KeyValuePair<string, object> parameter in parameters as Dictionary<string, object>)
                                {
                                    command.Parameters.Add(new SqlParameter("@" + parameter.Key, parameter.Value));
                                }
                            }
                            else if (parameters is DataTable)
                            {
                                command.Parameters.Add(parameters);
                            }
                            else
                            {
                                ArgumentException ex = new ArgumentException("Invalid Object Submitted", "parameters");
                                addError(currentUser.Sid.ToString(), currentUser.GivenName + " " +
                                    currentUser.Surname, ex.Source, ex.StackTrace);
                                throw ex;
                            }
                        }
                        using (SqlDataReader dr = command.ExecuteReader())
                        {
                            var tb = new DataTable();
                            tb.Load(dr);
                            return tb;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                addError(currentUser.Sid.ToString(), currentUser.GivenName + " " + currentUser.Surname, "CashFlowDataService", ex.StackTrace);
                throw;
            }
        }

        //internal static DataTable executeStoredProc(string connectionString, string storedProcName, DataTable parameters)
        //{
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(connectionString))
        //        {
        //            connection.Open();
        //            using (SqlCommand command = connection.CreateCommand())
        //            {
        //                command.CommandText = storedProcName;
        //                command.CommandType = CommandType.StoredProcedure;
        //                if (parameters != null && parameters.Rows.Count > 0)
        //                    command.Parameters.Add(parameters);
        //                using (SqlDataReader dr = command.ExecuteReader())
        //                {
        //                    var tb = new DataTable();
        //                    tb.Load(dr);
        //                    return tb;
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        addError(currentUser.Sid.ToString(), currentUser.GivenName + " " + currentUser.Surname, "CashFlowDataService", ex.StackTrace);
        //        throw;
        //    }
        //}

        internal static void addError(string adid, string adjname, string feature, string stackTrace)
        {
            Dictionary<string, object> paramaters = new Dictionary<string, object>();
            paramaters.Add("ADID", adid);
            paramaters.Add("AdjName", adjname);
            paramaters.Add("Feature", feature);
            paramaters.Add("StackTrace", stackTrace);
            paramaters.Add("UserName", userName);
            executeStoredProc(connectionString, "data.sp_AddError", paramaters);
        }

        internal static void CashFlowEntry()
        {
            //to be implemented
        }
    }

    internal class Exposure
    {
        internal static DataTable GetCashFlow(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("UserName")) parameters.Add("UserName", DataAccess.userName);
            return DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_GetCashFlowForExposure", parameters);
            //return DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_GetCashFlowForExp", parameters);
        }

        internal static DataTable GetHistory(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("UserName")) parameters.Add("UserName", DataAccess.userName);
            return DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_GetExposureHistory", parameters);
        }

        internal static DataTable GetExposuresForWM(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("UserName")) parameters.Add("UserName", DataAccess.userName);
            return DataAccess.executeStoredProc(DataAccess.connectionString, "sp_GetExposuresForWM", parameters);
        }
    }

    internal class WorkMatter
    {
        internal static DataTable GetAssociatedWorkMatters(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("UserName")) parameters.Add("UserName", DataAccess.userName);
            return DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_GetAssociatedWorkMatters", parameters);
        }

        internal static DataTable GetCashFlow(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("UserName")) parameters.Add("UserName", DataAccess.userName);
            return DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_GetCashFlowForWM", parameters);
        }

        internal static DataTable GetHistory(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("UserName")) parameters.Add("UserName", DataAccess.userName);
            return DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_GetWorkMatterHistory", parameters);
        }

        internal static DataTable GetPreviousCashFlow(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("UserName")) parameters.Add("UserName", DataAccess.userName);
            return DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_GetPreviousCashFlow", parameters);
        }
    }

    internal class Notification
    {
        internal static DataTable GetNotificationsForAnalysts(Dictionary<string, object> parameters)
        {
            if (!parameters.ContainsKey("UserName")) parameters.Add("UserName", DataAccess.userName);
            return DataAccess.executeStoredProc(DataAccess.connectionString, "data.sp_GetNotificationsForAnalysts", parameters);
        }

        internal static void Modify(string input)
        {
            //to be implemented
        }
    }
}