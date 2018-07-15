using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace CashFlowDataImport.Utility
{
    internal class Data
    {
        public static void importRow(DataRow row, DateTime importDateStamp = new DateTime())
        {
            var importSettings = Configuration.Import.Default;
            List<EntryModel> entrySet = new List<EntryModel>();
            NotificationModel comments = new NotificationModel();

            // Explode each datarow into separate entries they represent in the new structure.
            // Add each entry to entrySet.

            if (!String.IsNullOrWhiteSpace(row["Work Matter Number"].ToString()))
            {
                string analyst = Utility.Script.Actions.getAccountByLastName(row["Analyst"].ToString());

                #region Process 2017 Q1 Loss

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q1 Loss"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q1 Loss"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 1,
                        ValueName = "Loss",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q1 Loss

                #region Process 2017 Q2 Loss

                if (!string.IsNullOrWhiteSpace(row["Year 1 Q2 Loss"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q2 Loss"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 2,
                        ValueName = "Loss",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q2 Loss

                #region Process 2017 Q3 Loss

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q3 Loss"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q3 Loss"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 3,
                        ValueName = "Loss",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q3 Loss

                #region Process 2017 Q4 Loss

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q4 Loss"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q4 Loss"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 4,
                        ValueName = "Loss",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q4 Loss

                #region Process 2017 Q1 Cov/DJ Exp

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q1 Cov/DJ Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q1 Cov/DJ Exp"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 1,
                        ValueName = "CovDJ",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q1 Cov/DJ Exp

                #region Process 2017 Q2 Cov/DJ Exp

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q2 Cov/DJ Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q2 Cov/DJ Exp"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 2,
                        ValueName = "CovDJ",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q2 Cov/DJ Exp

                #region Process 2017 Q3 Cov/DJ Exp

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q3 Cov/DJ Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q3 Cov/DJ Exp"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 3,
                        ValueName = "CovDJ",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q3 Cov/DJ Exp

                #region Process 2017 Q4 Cov/DJ Exp

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q4 Cov/DJ Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q4 Cov/DJ Exp"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 4,
                        ValueName = "CovDJ",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q4 Cov/DJ Exp

                #region Process 2017 Q1 Def Exp

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q1 Def Exp "].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q1 Def Exp "] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 1,
                        ValueName = "DefExp",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q1 Def Exp

                #region Process 2017 Q2 Def Exp

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q 2 Def Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q 2 Def Exp"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 2,
                        ValueName = "DefExp",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q2 Def Exp

                #region Process 2017 Q3 Def Exp

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q3 Def Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q3 Def Exp"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 3,
                        ValueName = "DefExp",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q3 Def Exp

                #region Process 2017 Q4 Def Exp

                if (!String.IsNullOrWhiteSpace(row["Year 1 Q4 Def Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 1 Q4 Def Exp"] as string, out parsedAmount);
                    entrySet.Add(new EntryModel
                    {
                        WorkMatter = row["Work Matter Number"] as String,
                        Year = 2017,
                        Quarter = 4,
                        ValueName = "DefExp",
                        Amount = parsedAmount,
                        StartUser = analyst,
                        StartTime = importDateStamp
                    });
                }

                #endregion Process 2017 Q4 Def Exp

                // Year 2 and 3 are not broken into quarters.  It was decided to equally distribute any total by dividing by 4.

                #region Process 2018 Aggregate Loss

                if (!String.IsNullOrWhiteSpace(row["Year 2 Loss"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 2 Loss"] as string, out parsedAmount);
                    int q = 0;
                    do
                    {
                        q++;
                        entrySet.Add(new EntryModel
                        {
                            WorkMatter = row["Work Matter Number"] as String,
                            Year = 2018,
                            Quarter = q,
                            ValueName = "Loss",
                            Amount = (parsedAmount / 4),
                            StartUser = analyst,
                            StartTime = importDateStamp
                        });
                    } while (q < 4);
                }

                #endregion Process 2018 Aggregate Loss

                #region Process 2018 Aggregate Cov/DJ Exp

                if (!String.IsNullOrWhiteSpace(row["Year 2 Cov/DJ Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 2 Cov/DJ Exp"] as string, out parsedAmount);
                    int q = 0;
                    do
                    {
                        q++;
                        entrySet.Add(new EntryModel
                        {
                            WorkMatter = row["Work Matter Number"] as String,
                            Year = 2018,
                            Quarter = q,
                            ValueName = "CovDJ",
                            Amount = (parsedAmount / 4),
                            StartUser = analyst,
                            StartTime = importDateStamp
                        });
                    } while (q < 4);
                }

                #endregion Process 2018 Aggregate Cov/DJ Exp

                #region Process 2018 Aggregate Def Exp

                if (!String.IsNullOrWhiteSpace(row["Year 2 Def Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 2 Def Exp"] as string, out parsedAmount);
                    int q = 0;
                    do
                    {
                        q++;
                        entrySet.Add(new EntryModel
                        {
                            WorkMatter = row["Work Matter Number"] as String,
                            Year = 2018,
                            Quarter = q,
                            ValueName = "DefExp",
                            Amount = (parsedAmount / 4),
                            StartUser = analyst,
                            StartTime = importDateStamp
                        });
                    } while (q < 4);
                }

                #endregion Process 2018 Aggregate Def Exp

                #region Process 2019 Aggregate Loss

                if (!String.IsNullOrWhiteSpace(row["Year 3 Loss"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 3 Loss"] as string, out parsedAmount);
                    int q = 0;
                    do
                    {
                        q++;
                        entrySet.Add(new EntryModel
                        {
                            WorkMatter = row["Work Matter Number"] as String,
                            Year = 2019,
                            Quarter = q,
                            ValueName = "Loss",
                            Amount = (parsedAmount / 4),
                            StartUser = analyst,
                            StartTime = importDateStamp
                        });
                    } while (q < 4);
                }

                #endregion Process 2019 Aggregate Loss

                #region Process 2019 Aggregate Cov/DJ Exp

                if (!String.IsNullOrWhiteSpace(row["Year 3 Cov/DJ Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 3 Cov/DJ Exp"] as string, out parsedAmount);
                    int q = 0;
                    do
                    {
                        q++;
                        entrySet.Add(new EntryModel
                        {
                            WorkMatter = row["Work Matter Number"] as String,
                            Year = 2019,
                            Quarter = q,
                            ValueName = "CovDJ",
                            Amount = (parsedAmount / 4),
                            StartUser = analyst,
                            StartTime = importDateStamp
                        });
                    } while (q < 4);
                }

                #endregion Process 2019 Aggregate Cov/DJ Exp

                #region Process 2019 Aggregate Def Exp

                if (!String.IsNullOrWhiteSpace(row["Year 3 Def Exp"].ToString()))
                {
                    // Parse the amount, make it 0 for non-numerics.
                    decimal parsedAmount = 0; decimal.TryParse(row["Year 3 Def Exp"] as string, out parsedAmount);
                    int q = 0;
                    do
                    {
                        q++;
                        entrySet.Add(new EntryModel
                        {
                            WorkMatter = row["Work Matter Number"] as String,
                            Year = 2019,
                            Quarter = q,
                            ValueName = "DefExp",
                            Amount = (parsedAmount / 4),
                            StartUser = analyst,
                            StartTime = importDateStamp
                        });
                    } while (q < 4);
                }

                #endregion Process 2019 Aggregate Def Exp

                // Extract comments for the workmatter into a model for the entry Notification area
                comments.StartTime = importDateStamp;
                comments.WorkMatter = row["Work Matter Number"].ToString();
                comments.StartUser = analyst;
                comments.Analyst = Utility.Script.Actions.getSid(analyst.Replace("trg\\", ""));
                comments.Basis = row["Basis for Projections"].ToString();
                comments.Comments = row["Comments and Projection Rationale"].ToString();
            }

            if (entrySet.Count > 0)
            {
                int processed = 0;
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("______________________________________________________________________");
                sb.AppendLine("Import results for Work Matter #" + entrySet[0].WorkMatter + ": ");
                sb.AppendLine("Analyst: " + entrySet[0].StartUser);
                sb.AppendLine("Comments: " + comments.Comments);
                sb.AppendLine("Basis: " + comments.Basis);
                foreach (EntryModel entry in entrySet)
                {
                    sb.AppendLine(" =>  " + entry.Year + ", " + "Q" + entry.Quarter + " " + entry.ValueName + ": " + entry.Amount);
                }

                using (System.IO.StreamWriter file =
                new System.IO.StreamWriter(importSettings.logFileLocation, true))
                {
                    if (processed == 0)
                    {
                        file.WriteLine("Import Results for " + DateTime.Now.ToString("g"));
                    }

                    file.WriteLine(sb.ToString());
                    processed++;
                }

                if (!importSettings.whatIfMode)
                {
                    using (SqlConnection connection = new SqlConnection("Data Source=" + importSettings.sqlServerName + "; Initial Catalog=" + importSettings.sqlDatabaseName + "; Integrated Security=True"))
                    {
                        connection.Open();
                        foreach (EntryModel entry in entrySet)
                        {
                            string sql = "INSERT INTO " + importSettings.sqlTableName;
                            sql += " (WorkMatter, Exposure, PolicyNumber, Year, Quarter, ValueName, Amount, StartUser, StartTime, EndUser, EndTime) ";
                            sql += " VALUES (@WorkMatter, @Exposure, @PolicyNumber, @Year, @Quarter, @ValueName, @Amount, @StartUser, @StartTime, @EndUser, @EndTime)";
                            SqlCommand sqlCmd = new SqlCommand(sql, connection);
                            sqlCmd.CommandType = CommandType.Text;
                            sqlCmd.Parameters.AddWithValue("WorkMatter", entry.WorkMatter);
                            sqlCmd.Parameters.AddWithValue("Exposure", String.Empty);
                            sqlCmd.Parameters.AddWithValue("PolicyNumber", String.Empty);
                            sqlCmd.Parameters.AddWithValue("Year", entry.Year);
                            sqlCmd.Parameters.AddWithValue("Quarter", entry.Quarter);
                            sqlCmd.Parameters.AddWithValue("ValueName", entry.ValueName);
                            sqlCmd.Parameters.AddWithValue("Amount", entry.Amount);
                            sqlCmd.Parameters.AddWithValue("StartUser", entry.StartUser);
                            sqlCmd.Parameters.AddWithValue("StartTime", entry.StartTime);
                            sqlCmd.Parameters.AddWithValue("EndUser", "Former");
                            sqlCmd.Parameters.AddWithValue("EndTime", new DateTime(2017, 5, 1));
                            sqlCmd.ExecuteNonQuery();
                            sqlCmd.Dispose();
                        }

                        // Add Comments
                        string commentsSql = "INSERT INTO " + importSettings.sqlNotificationsTableName;
                        commentsSql += " (Analyst, WorkMatter, Status, Basis, Comments, ReasonableWorstCase, Confidence, Viewed, DeclinedReason, StartUser, StartTime, EndUser, EndTime, RWCLoss, RWCDefExp, RWCCovDJ)";
                        commentsSql += " VALUES (@Analyst, @WorkMatter, @Status, @Basis, @Comments, @ReasonableWorstCase, @Confidence, @Viewed, @DeclinedReason, @StartUser, @StartTime, @EndUser, @EndTime, @RWCLoss, @RWCDefExp, @RWCCovDJ)";
                        SqlCommand cmd = new SqlCommand(commentsSql, connection);
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.AddWithValue("Analyst", comments.Analyst);
                        cmd.Parameters.AddWithValue("WorkMatter", comments.WorkMatter);
                        cmd.Parameters.AddWithValue("Status", comments.Status);
                        cmd.Parameters.AddWithValue("Basis", comments.Basis);
                        cmd.Parameters.AddWithValue("Comments", comments.Comments);
                        cmd.Parameters.AddWithValue("ReasonableWorstCase", comments.ReasonableWorstCase);
                        cmd.Parameters.AddWithValue("Confidence", comments.Confidence);
                        cmd.Parameters.AddWithValue("Viewed", comments.Viewed);
                        cmd.Parameters.AddWithValue("DeclinedReason", comments.DeclinedReason);
                        cmd.Parameters.AddWithValue("StartUser", comments.StartUser);
                        cmd.Parameters.AddWithValue("StartTime", comments.StartTime);
                        cmd.Parameters.AddWithValue("EndUser", "Former");
                        cmd.Parameters.AddWithValue("EndTime", new DateTime(2017, 5, 1));
                        cmd.Parameters.AddWithValue("RWCLoss", comments.RWCLoss);
                        cmd.Parameters.AddWithValue("RWCDefExp", comments.RWCDefExp);
                        cmd.Parameters.AddWithValue("RWCCovDJ", comments.RWCCovDJ);
                        cmd.ExecuteNonQuery();
                        cmd.Dispose();
                    }
                }
            }
        }
    }
}