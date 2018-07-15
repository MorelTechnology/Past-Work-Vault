using System;
using System.Data;
using System.Data.SqlClient;
using static AppsToOranges.Utility;

namespace LitMatterSyncService
{
    internal class Database
    {
        private static EventLogger log = new EventLogger("Litigation Matter Synchronization Service", "Application");

        internal class Read
        {
            public DataTable queryForMattersToProcess()
            {
                DataTable table = new DataTable();
                try
                {
                    //Set up SQL connection to staging table, load query results into memory
                    using (SqlConnection connection = new SqlConnection(Settings.Default.ClaimCenterConnectionString))
                    {
                        SqlCommand command = new SqlCommand("SELECT * FROM " + Settings.Default.LitMattersSqlView + " WHERE IsMatterProcessed = 0", connection);
                        connection.Open();
                        SqlDataReader result = command.ExecuteReader();
                        table.Load(result);
                    }
                }
                catch (Exception ex)
                {
                    log.addError("Error while attempting to query the Claim Center Database: " + ex.ToString());
                }

                return table;
            }

            public bool isMatterUnique(string lmNumber)
            {
                //Past migrations have erroneously created more than one LM with the same number.  In a case such as this, we run the risk of updating a completely unrelated
                //site. This method helps us to avoid this by ensuring the LM Number only returns a single case.
                using (SqlConnection connection = new SqlConnection(Settings.Default.ClaimCenterConnectionString))
                {
                    SqlCommand command = new SqlCommand("select count(*) AS UniqueCount from ClaimCenter.dbo.cc_matter where CloseDate Is Null and trg_MatterNumber = '" + lmNumber + "'", connection);
                    connection.Open();
                    switch ((Int32)command.ExecuteScalar())
                    {
                        case 0:
                            log.addWarning("Unexpected: A litigation matter with the number " + lmNumber +
                                " is not found in ClaimCenter.  This may result in an orphaned SharePoint site.");
                            return true;

                        case 1: // Just one Matter found.
                            return true;

                        default:
                            return false;
                    }
                }
            }
        }

        internal class Write
        {
            public void markAsProcessed(string[] processedMatters)
            {
                string processedMattersString = "'" + string.Join("', '", processedMatters) + "'";
                try
                {
                    //Set up SQL connection to staging table, load query results into memory
                    using (SqlConnection connection = new SqlConnection(Settings.Default.ClaimCenterConnectionString))
                    {
                        SqlCommand command = new SqlCommand("UPDATE " + Settings.Default.LitMattersSqlView +
                            " SET IsMatterProcessed = 1 WHERE trg_MatterNumber IN(" + processedMattersString + ")", connection);
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    log.addError("Error while attempting to query the Claim Center Database: " + ex.ToString());
                }
            }
        }
    }
}