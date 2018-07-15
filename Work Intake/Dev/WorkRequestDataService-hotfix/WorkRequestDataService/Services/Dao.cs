using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.InteropServices;
using WorkRequestDataService.Models;
using static WorkRequestDataService.Services.Utility;

namespace WorkRequestDataService.Services
{
    /// <summary>
    /// Data Access Object
    /// Used to extract database interaction from public interface.
    /// </summary>
    internal class Dao
    {
        /// <summary>
        /// Fetches Work Request(s) from the database.
        /// </summary>
        /// <param name="matchOnValues">(Optional) A <see cref="WorkRequest"/> object used to specify
        /// desired results which should be returned.</param>
        /// <returns>An array of <see cref="WorkRequest"/> objects.</returns>
        /// <exception cref="KeyNotFoundException">No matching Work Request was returned from your search: " +
        ///                     JsonConvert.SerializeObject(matchOnValues)</exception>

        #region Internal Methods

        internal void AddComment(Comment comment)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO [data].[Comments] ";
                    cmd.CommandText += "(WorkRequestId, CommentDate, CommentUser, CommentText) ";
                    cmd.CommandText += "Values ('" + comment.WorkRequestId + "', '" +
                                                     comment.CommentDate + "', '" +
                                                     escape(comment.CommentUser) + "', '" +
                                                     escape(comment.CommentText) + "')";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally { if (conn != null) conn.Close(); }
        }

        internal void AddError(Error error)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO [data].[Error] ";
                    cmd.CommandText += "(LocalizedDate, CurrentUser, IsAuthenticated, CurrentUrl, ExceptionMessage, StackTrace, AdditionalDetails) ";
                    cmd.CommandText += "Values ('" + error.LocalizedDate + "', '" +
                                                     escape(error.CurrentUser) + "', '" +
                                                     error.IsAuthenticated + "', '" +
                                                     escape(error.CurrentUrl) + "', '" +
                                                     escape(error.ExceptionMessage) + "', '" +
                                                     escape(error.StackTrace) + "', '" +
                                                     escape(error.AdditionalDetails) + "')";
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception) { /*  ¯\_(ツ)_/¯  If an error logger gets an error itself, does it make a sound? */ }
            finally { if (conn != null) conn.Close(); }
        }

        internal void AddHistory(List<Variance> workRequestUpdate)
        {
            UserService userService = new UserService();
            string statement = null;
            foreach (Variance change in workRequestUpdate)
            {
                var oldValue = change.OriginalValue is List<string>
                    ? string.Join(", ", ((List<String>)change.OriginalValue).ToArray())
                    : change.OriginalValue;

                var newValue = change.UpdatedValue is List<string>
                    ? string.Join(", ", ((List<String>)change.UpdatedValue).ToArray())
                    : change.UpdatedValue;

                // See if these are user sids and if so, get display names.
                try
                {
                    if (userService.UserIdentifierIsSid(oldValue.ToString()))
                        oldValue = userService.GetUserInfo(oldValue.ToString())["displayName"] as string;
                }
                catch { }
                try
                {
                    if (userService.UserIdentifierIsSid(newValue.ToString()))
                        newValue = userService.GetUserInfo(newValue.ToString())["displayName"] as string;
                }
                catch { }
                if (oldValue as string != newValue as string) // safeguard, since we do some string manipulation.
                    statement += string.Format("Property '{0}' updated from '{1}' to '{2}'.",
                        change.Property, oldValue, newValue);
                if (!change.Equals(workRequestUpdate.Last())) statement += "\n";
            }
        }

        internal int AddWorkRequest(WorkRequest workRequest)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            int requestId;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "INSERT INTO [data].[Requests] ";
                    cmd.CommandText += "(Requestor, Manager, Title, Goal, BusinessValueUnit, BusinessValueAmount, NonImplementImpact, RealizationOfImpact, RequestedCompletionDate, Status, StatusDate, LastModified, Created, ";
                    cmd.CommandText += " CorporateGoals, GoalSupport, SupportsDept, DeptGoalSupport, ConditionsOfSatisfaction)";
                    cmd.CommandText += "output INSERTED.RequestID ";
                    cmd.CommandText += "Values ('" + escape(workRequest.Requestor) + "', '" +
                                                     escape(workRequest.Manager) + "', '" +
                                                     escape(workRequest.Title) + "', '" +
                                                     escape(workRequest.Goal) + "', '" +
                                                     // Problem field deprecated, Iteration 9, January 2018
                                                     // escape(workRequest.Problem) + "', '" +
                                                     workRequest.BusinessValueUnit + "', '" +
                                                     workRequest.BusinessValueAmount + "', '" +
                                                     escape(workRequest.NonImplementImpact) + "', '" +
                                                     escape(workRequest.RealizationOfImpact) + "', '" +
                                                     workRequest.RequestedCompletionDate + "', '" +
                                                     // Quantification field deprecated, Iteration 6, December 2017, replaced by BusinessValueUnit/BusinessValueAmount
                                                     // escape(workRequest.Quantification) + "', '" +
                                                     // Benefit field deprecated, Iteration 9, January 2018
                                                     // escape(workRequest.Benefit) + "', '" +
                                                     workRequest.Status + "', '" +
                                                     DateTime.Now + "', '" +
                                                     DateTime.Now + "', '" +
                                                     DateTime.Now + "', '" +
                                                     Utility.objectToSql(workRequest.CorporateGoals) + "', '" +
                                                     escape(workRequest.GoalSupport) + "', '" +
                                                     workRequest.SupportsDept + "', '" +
                                                     escape(workRequest.DeptGoalSupport) + "', '" +
                                                     escape(workRequest.ConditionsOfSatisfaction) + "')";
                    requestId = (int)cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return requestId;
        }

        internal int DeleteWorkRequest(int workRequestId, bool isTest)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            int deleted = 0;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    if (isTest)
                    {
                        cmd.CommandText = "SELECT COUNT(*) FROM [data].[Requests]";
                    }
                    else
                    {
                        cmd.CommandText = "DELETE FROM [data].[Requests] ";
                    }
                    cmd.CommandText += "WHERE RequestID = " + workRequestId + " ";
                    if (isTest)
                        deleted = (int)cmd.ExecuteScalar();
                    else
                        deleted = (int)cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return deleted;
        }

        internal DataTable GetConfiguration()
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            DataTable table = new DataTable();
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select [Key],Value,StorageType from [data].[Config] WHERE Deprecated = 0";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(table);
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return table;
        }

        internal dynamic GetConfigurationProperty(string key)
        {
            //if (!UserCanAccessConfigurationProperty(key)) return new UnauthorizedAccessException();
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * from [data].[Config] Where [Key] = '" + key + "'";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable table = new DataTable();
                    da.Fill(table);
                    if (table.Rows.Count > 1) throw new DuplicateNameException("'" + key + "' did not return a unique value.");
                    return sqlToObject(table.Rows[0]["Value"], Type.GetType(table.Rows[0]["StorageType"].ToString()));
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        /// <summary>
        /// This method attempts to return the value found at for the given key
        /// as an object of the specified Type in T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        internal T GetConfigurationProperty<T>(string key)
        {
            return (T)GetConfigurationProperty(key);

            //var value = GetConfigurationProperty(key).ToString();
            //try
            //{
            //    return JsonConvert.DeserializeObject<T>(value);
            //}
            //catch // handles single strings or other non-deserializable blunders...
            //{
            //    return (T)Convert.ChangeType(value, typeof(T));
            //}
        }

        internal DataTable GetErrors()
        {
            {
                string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
                SqlConnection conn = null;
                DataTable table = new DataTable();
                try
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = "Select * from [data].[Error] ORDER BY LocalizedDate DESC";
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(table);
                    }
                }
                catch (Exception ex)
                {
                    Error.Log(ex);
                    throw ex;
                }
                finally
                {
                    if (conn != null)
                    {
                        conn.Close();
                    }
                }
                return table;
            }
        }

        internal WorkRequest[] GetWorkRequest([Optional]WorkRequest matchOnValues)
        {
            List<WorkRequest> results = new List<WorkRequest>();
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            try
            {
                string sql = "Select * from [data].[Requests] ";
                if (matchOnValues != null)
                {
                    var properties = matchOnValues.GetType().GetProperties();
                    sql += "Where ";

                    int numProps = properties.Length;
                    for (int i = 0; i < numProps; i++)
                    {
                        var key = properties[i].Name;
                        var value = properties[i].GetValue(matchOnValues);
                        if (value == null) continue;
                        sql += " " + key + " = '" + value + "' ";
                        if (numProps > 0 && i < numProps - 1) sql += " AND";
                    }
                    if (sql.EndsWith("AND")) sql = sql.Remove(sql.Length - 3);
                }
                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(sql, connString))
                    da.Fill(dt);

                foreach (var row in dt.AsEnumerable())
                {
                    try
                    {
                        results.Add(
                            new WorkRequest
                            {
                                RequestID = row.Field<dynamic>("RequestID"),
                                Requestor = row.Field<dynamic>("Requestor"),
                                Title = unescape(row.Field<dynamic>("Title")),
                                Manager = row.Field<dynamic>("Manager"),
                                Goal = unescape(row.Field<dynamic>("Goal")),
                                // Problem field deprecated, Iteration 9, January 2018
                                // Problem = unescape(row.Field<dynamic>("Problem")),
                                BusinessValueUnit = row.Field<dynamic>("BusinessValueUnit"),
                                BusinessValueAmount = row.Field<dynamic>("BusinessValueAmount"),
                                RequestedCompletionDate = row.Field<dynamic>("RequestedCompletionDate"),
                                // Quantification field deprecated, Iteration 6, December 2017, replaced by BusinessValueUnit/BusinessValueAmount
                                // Quantification = unescape(row.Field<dynamic>("Quantification")),
                                // Benefit field deprecated, Iteration 9, January 2018
                                // Benefit = unescape(row.Field<dynamic>("Benefit")),
                                Status = row.Field<dynamic>("Status"),
                                StatusDate = row.Field<dynamic>("StatusDate"),
                                LastModified = row.Field<dynamic>("LastModified"),
                                CorporateGoals = JsonConvert.DeserializeObject<List<string>>(row.Field<dynamic>("CorporateGoals")),
                                GoalSupport = unescape(row.Field<dynamic>("GoalSupport")),
                                SupportsDept = row.Field<dynamic>("SupportsDept"),
                                DeptGoalSupport = unescape(row.Field<dynamic>("DeptGoalSupport")),
                                ConditionsOfSatisfaction = unescape(row.Field<dynamic>("ConditionsOfSatisfaction")),
                                NonImplementImpact = unescape(row.Field<dynamic>("NonImplementImpact")),
                                RealizationOfImpact = unescape(row.Field<dynamic>("RealizationOfImpact"))
                            });
                    }
                    catch (Exception ex)
                    {
                        Error.Log(ex);
                    }
                }
                if (results.Count < 1)
                {
                    throw new KeyNotFoundException("No matching Work Request was returned from your search: " +
                    JsonConvert.SerializeObject(matchOnValues));
                }
                return results.ToArray<WorkRequest>();
            }
            catch (Exception ex)

            {
                Error.Log(ex);
                return new WorkRequest[0]; //empty
            }
        }

        internal object GetWorkRequestComments(int workRequestId)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            DataTable table = new DataTable();
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select * from [data].[Comments] WHERE WorkRequestId = '" + workRequestId + "' ORDER BY CommentDate";
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(table);
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return table;
        }

        internal bool SetConfig(Models.ConfigurationProperty cp)
        {
            bool result = false;
            if (!UserCanAccessConfigurationProperty(cp.Key)) throw new UnauthorizedAccessException();
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "UPDATE [data].[Config] SET Value = '" + objectToSql(cp.Value) + "', ModifiedBy = '" + System.Web.HttpContext.Current.User.Identity.Name + "' " +
                                      "WHERE [Key] = '" + cp.Key + "'";
                    cmd.ExecuteNonQuery();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return result;
        }

        internal int UpdateWorkRequest(WorkRequest workRequest, bool isTest)
        {
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            int updated = 0;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    if (isTest)
                    {
                        cmd.CommandText = "SELECT Count(*) FROM [data].[Requests]";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE [data].[Requests] ";
                        cmd.CommandText += "SET Requestor = '" + escape(workRequest.Requestor) + "', ";
                        cmd.CommandText += "Manager = '" + escape(workRequest.Manager) + "', ";
                        cmd.CommandText += "Title = '" + escape(workRequest.Title) + "', ";
                        cmd.CommandText += "Goal = '" + escape(workRequest.Goal) + "', ";
                        // Problem field deprecated, Iteration 9, January 2018
                        // cmd.CommandText += "Problem = '" + escape(workRequest.Problem) + "', ";
                        cmd.CommandText += "BusinessValueUnit = '" + workRequest.BusinessValueUnit + "', ";
                        cmd.CommandText += "BusinessValueAmount = '" + workRequest.BusinessValueAmount + "', ";
                        cmd.CommandText += "NonImplementImpact = '" + escape(workRequest.NonImplementImpact) + "', ";
                        cmd.CommandText += "RealizationOfImpact ='" + workRequest.RealizationOfImpact + "', ";
                        cmd.CommandText += "RequestedCompletionDate = '" + workRequest.RequestedCompletionDate + "', ";
                        // Quantification field deprecated, Iteration 6, December 2017, replaced by BusinessValueUnit/BusinessValueAmount
                        // cmd.CommandText += "Quantification = '" + escape(workRequest.Quantification) + "', ";
                        // Benefit field deprecated, Iteration 9, January 2018
                        // cmd.CommandText += "Benefit = '" + escape(workRequest.Benefit) + "', ";
                        cmd.CommandText += "LastModified = '" + DateTime.Now + "', ";
                        cmd.CommandText += "Status = '" + workRequest.Status + "', ";
                        cmd.CommandText += "CorporateGoals = '" + Utility.objectToSql(workRequest.CorporateGoals) + "', ";
                        cmd.CommandText += "GoalSupport = '" + escape(workRequest.GoalSupport) + "', ";
                        cmd.CommandText += "SupportsDept = '" + workRequest.SupportsDept + "', ";
                        cmd.CommandText += "DeptGoalSupport = '" + escape(workRequest.DeptGoalSupport) + "', ";
                        cmd.CommandText += "ConditionsOfSatisfaction = '" + escape(workRequest.ConditionsOfSatisfaction) + "'";
                    }
                    cmd.CommandText += " WHERE RequestID = " + workRequest.RequestID;
                    if (isTest)
                        updated = (int)cmd.ExecuteScalar();
                    else
                        updated = (int)cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
            return updated;
        }

        internal bool UserCanAccessConfigurationProperty(string key)
        {
            // is user at least a configuration admin?  Otherwise, they can't do squat.
            if (!new UserService().CurrentUserIsConfigurationAdmin()) return false;
            string connString = ConfigurationManager.ConnectionStrings["DBConnection"].ConnectionString;
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connString);
                conn.Open();

                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "Select Secure from [data].[Config] Where [Key] = '" + key + "'";
                    bool secured = (bool)cmd.ExecuteScalar();
                    if (!secured) return true; // Property is accessible to Configuration Admin.
                    else return (new UserService().CurrentUserIsEnvironmentAdmin()); // was secured, so check if user is environment admin.
                }
            }
            catch (Exception ex)
            {
                Error.Log(ex);
                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        #endregion Internal Methods
    }
}