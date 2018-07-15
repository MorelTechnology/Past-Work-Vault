using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Administration;
using Microsoft.Exchange.WebServices.Data;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Reflection;

namespace TimeOffCalendarSync
{
    public class CalendarSyncJob : SPJobDefinition
    {
        public CalendarSyncJob() : base() { }

        public CalendarSyncJob(string jobName, SPWebApplication webApp)
            : base(jobName, webApp, null, SPJobLockType.Job)
        {
            this.Title = jobName;
        }

        protected override bool HasAdditionalUpdateAccess()
        {
            return true;
        }

        public override void Execute(Guid targetInstanceId)
        {
            if ((this.Parent as SPWebApplication).Name == Resources.DefaultWebApp)
            {
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013);
                service.Credentials = new WebCredentials(Resources.EWSUsername, Resources.EWSPassword);
                service.AutodiscoverUrl(Resources.EWSUsername, RedirectionUrlValidationCallback);
                GetCalendarGuids(service);
                GetCalendarEvents(service);
                SynchronizeUsers(Resources.SyncOU);
            }
        }

        private static bool RedirectionUrlValidationCallback(string redirectionUrl)
        {
            // The default for the validation callback is to reject the URL.
            bool result = false;

            Uri redirectionUri = new Uri(redirectionUrl);

            // Validate the contents of the redirection URL. In this simple validation
            // callback, the redirection URL is considered valid if it is using HTTPS
            // to encrypt the authentication credentials.
            if (redirectionUri.Scheme == "https")
            {
                result = true;
            }
            return result;
        }

        private void GetCalendarGuids(ExchangeService service)
        {
            // Use a view to retrieve calendar events within 180 days
            DateTime startDate = DateTime.Now.AddDays(-180);
            DateTime endDate = DateTime.Now.AddDays(180);
            Mailbox userMailbox = new Mailbox(Resources.EWSUserMailbox);
            FolderId calendar = new FolderId(WellKnownFolderName.Calendar, userMailbox);
            CalendarView cView = new CalendarView(startDate, endDate);
            cView.PropertySet = new PropertySet(AppointmentSchema.ICalUid, AppointmentSchema.Organizer, AppointmentSchema.Subject, AppointmentSchema.Location,
                AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.LegacyFreeBusyStatus, AppointmentSchema.AppointmentState, AppointmentSchema.IsCancelled);
            FindItemsResults<Item> items = null;

            items = service.FindItems(calendar, cView);

            // Set up a datatable to store appointments
            DataTable appointmentsData = new DataTable("Appointments");

            // ICalUid Column
            DataColumn iCalUidColumn = new DataColumn();
            iCalUidColumn.DataType = Type.GetType("System.String");
            iCalUidColumn.ColumnName = "ICalUid";
            appointmentsData.Columns.Add(iCalUidColumn);

            // ChangeKey Column
            DataColumn changeKeyColumn = new DataColumn();
            changeKeyColumn.DataType = Type.GetType("System.String");
            changeKeyColumn.ColumnName = "ChangeKey";
            appointmentsData.Columns.Add(changeKeyColumn);

            // UniqueId Column
            DataColumn uniqueIdColumn = new DataColumn();
            uniqueIdColumn.DataType = Type.GetType("System.String");
            uniqueIdColumn.ColumnName = "UniqueID";
            appointmentsData.Columns.Add(uniqueIdColumn);

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Resources.SqlConnectionString, SqlBulkCopyOptions.KeepNulls & SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = items.Count();
                bulkCopy.DestinationTableName = "dbo.AppointmentIds";
                bulkCopy.ColumnMappings.Clear();
                bulkCopy.ColumnMappings.Add("ICalUid", "ICalUid");
                bulkCopy.ColumnMappings.Add("ChangeKey", "ChangeKey");
                bulkCopy.ColumnMappings.Add("UniqueID", "UniqueID");

                foreach (Item item in items)
                {
                    try
                    {
                        DataRow appointmentRow = appointmentsData.NewRow();
                        ItemId itemId = null;
                        string iCalUid = null;

                        item.TryGetProperty(AppointmentSchema.ICalUid, out iCalUid);
                        if (iCalUid != null)
                        {
                            appointmentRow["ICalUid"] = iCalUid;
                        }

                        itemId = item.Id;
                        appointmentRow["ChangeKey"] = itemId.ChangeKey;
                        appointmentRow["UniqueID"] = itemId.UniqueId;

                        appointmentsData.Rows.Add(appointmentRow);
                    }
                    catch (Exception ex)
                    {
                        Util.LogError("GetCalendarGuids failed with message: " + ex.Message);
                    }
                }

                using (SqlConnection conn = new SqlConnection(Resources.SqlConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(Resources.TruncateStoredProcedureName, conn) { CommandType = CommandType.StoredProcedure })
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }

                bulkCopy.WriteToServer(appointmentsData);
            }
        }

        private void GetCalendarEvents(ExchangeService service)
        {
            // Use a view to retrieve calendar events within 60 days from today
            DateTime startDate = DateTime.Now.AddDays(-60);
            DateTime endDate = DateTime.Now.AddDays(60);
            Mailbox userMailbox = new Mailbox(Resources.EWSUserMailbox);
            FolderId calendar = new FolderId(WellKnownFolderName.Calendar, userMailbox);
            CalendarView cView = new CalendarView(startDate, endDate);
            cView.PropertySet = new PropertySet(AppointmentSchema.ICalUid, AppointmentSchema.Organizer, AppointmentSchema.Subject, AppointmentSchema.Location,
                AppointmentSchema.Start, AppointmentSchema.End, AppointmentSchema.LegacyFreeBusyStatus, AppointmentSchema.AppointmentState, AppointmentSchema.IsCancelled, AppointmentSchema.IsAllDayEvent);
            FindItemsResults<Item> items = null;

            items = service.FindItems(calendar, cView);

            // Set up a datatable to store appointments
            DataTable appointmentsData = new DataTable("Appointments");

            // ICalUid Column
            DataColumn iCalUidColumn = new DataColumn();
            iCalUidColumn.DataType = Type.GetType("System.String");
            iCalUidColumn.ColumnName = "ICalUid";
            appointmentsData.Columns.Add(iCalUidColumn);

            // Account name column
            DataColumn accountNameColumn = new DataColumn();
            accountNameColumn.DataType = Type.GetType("System.String");
            accountNameColumn.ColumnName = "AccountName";
            appointmentsData.Columns.Add(accountNameColumn);

            // Subject column
            DataColumn subjectColumn = new DataColumn();
            subjectColumn.DataType = Type.GetType("System.String");
            subjectColumn.ColumnName = "Subject";
            appointmentsData.Columns.Add(subjectColumn);

            // Location column
            DataColumn locationColumn = new DataColumn();
            locationColumn.DataType = Type.GetType("System.String");
            locationColumn.ColumnName = "Location";
            appointmentsData.Columns.Add(locationColumn);

            // Start time column
            DataColumn startTimeColumn = new DataColumn();
            startTimeColumn.DataType = Type.GetType("System.DateTime");
            startTimeColumn.ColumnName = "StartTime";
            appointmentsData.Columns.Add(startTimeColumn);

            // End time column
            DataColumn endTimeColumn = new DataColumn();
            endTimeColumn.DataType = Type.GetType("System.DateTime");
            endTimeColumn.ColumnName = "EndTime";
            appointmentsData.Columns.Add(endTimeColumn);

            // Status column
            DataColumn statusColumn = new DataColumn();
            statusColumn.DataType = Type.GetType("System.String");
            statusColumn.ColumnName = "Status";
            appointmentsData.Columns.Add(statusColumn);

            // Appointment State column
            DataColumn appointmentStateColumn = new DataColumn();
            appointmentStateColumn.DataType = Type.GetType("System.Int32");
            appointmentStateColumn.ColumnName = "AppointmentState";
            appointmentsData.Columns.Add(appointmentStateColumn);

            // Is Cancelled Column
            DataColumn isCancelledColumn = new DataColumn();
            isCancelledColumn.DataType = Type.GetType("System.Boolean");
            isCancelledColumn.ColumnName = "IsCancelled";
            appointmentsData.Columns.Add(isCancelledColumn);

            // Is All Day Event Column
            DataColumn isAllDayEventColumn = new DataColumn();
            isAllDayEventColumn.DataType = Type.GetType("System.Boolean");
            isAllDayEventColumn.ColumnName = "IsAllDayEvent";
            appointmentsData.Columns.Add(isAllDayEventColumn);

            // ChangeKey Column
            DataColumn changeKeyColumn = new DataColumn();
            changeKeyColumn.DataType = Type.GetType("System.String");
            changeKeyColumn.ColumnName = "ChangeKey";
            appointmentsData.Columns.Add(changeKeyColumn);

            // UniqueId Column
            DataColumn uniqueIdColumn = new DataColumn();
            uniqueIdColumn.DataType = Type.GetType("System.String");
            uniqueIdColumn.ColumnName = "UniqueID";
            appointmentsData.Columns.Add(uniqueIdColumn);

            List<ResolvedEmailAddress> resolvedEmailAddresses = new List<ResolvedEmailAddress>();
            int i = 0;

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Resources.SqlConnectionString, SqlBulkCopyOptions.KeepNulls & SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = items.Count();
                bulkCopy.DestinationTableName = "dbo.TimeOffCalendarTemp";
                bulkCopy.ColumnMappings.Clear();
                bulkCopy.ColumnMappings.Add("ICalUid", "ICalUid");
                bulkCopy.ColumnMappings.Add("AccountName", "AccountName");
                bulkCopy.ColumnMappings.Add("Subject", "Subject");
                bulkCopy.ColumnMappings.Add("Location", "Location");
                bulkCopy.ColumnMappings.Add("StartTime", "StartTime");
                bulkCopy.ColumnMappings.Add("EndTime", "EndTime");
                bulkCopy.ColumnMappings.Add("Status", "Status");
                bulkCopy.ColumnMappings.Add("AppointmentState", "AppointmentState");
                bulkCopy.ColumnMappings.Add("IsCancelled", "IsCancelled");
                bulkCopy.ColumnMappings.Add("IsAllDayEvent", "IsAllDayEvent");
                bulkCopy.ColumnMappings.Add("ChangeKey", "ChangeKey");
                bulkCopy.ColumnMappings.Add("UniqueID", "UniqueID");

                foreach (Item item in items)
                {
                    try
                    {
                        i++;
                        DataRow appointmentRow = appointmentsData.NewRow();
                        EmailAddress accountName = null;
                        ItemId itemId = null;
                        string iCalUid, subject, location = null;
                        DateTime startTime, endTime = new DateTime();
                        LegacyFreeBusyStatus status = new LegacyFreeBusyStatus();
                        int appointmentState;
                        bool isCancelled, isAllDayEvent;

                        item.TryGetProperty(AppointmentSchema.ICalUid, out iCalUid);
                        if (iCalUid != null)
                        {
                            appointmentRow["ICalUid"] = iCalUid;
                        }

                        item.TryGetProperty(AppointmentSchema.Organizer, out accountName);
                        if (accountName != null)
                        {
                            if (resolvedEmailAddresses.Where(re => re.EWSAddress == accountName.Address).Count() < 1)
                            {
                                NameResolutionCollection nd = service.ResolveName(accountName.Address);
                                if (nd.Count > 0)
                                {
                                    ResolvedEmailAddress resAddress = new ResolvedEmailAddress { EWSAddress = accountName.Address, SMTPAddress = nd[0].Mailbox.Address };
                                    resolvedEmailAddresses.Add(resAddress);
                                    appointmentRow["AccountName"] = resAddress.SMTPAddress;
                                }
                            }
                            else
                            {
                                ResolvedEmailAddress resAddress = resolvedEmailAddresses.First(re => re.EWSAddress == accountName.Address);
                                appointmentRow["AccountName"] = resAddress.SMTPAddress;
                            }
                        }

                        item.TryGetProperty(AppointmentSchema.Subject, out subject);
                        if (subject != null)
                        {
                            appointmentRow["Subject"] = subject;
                        }

                        item.TryGetProperty(AppointmentSchema.Location, out location);
                        if (location != null)
                        {
                            appointmentRow["Location"] = location;
                        }

                        item.TryGetProperty(AppointmentSchema.Start, out startTime);
                        if (startTime != null)
                        {
                            appointmentRow["StartTime"] = startTime;
                        }

                        item.TryGetProperty(AppointmentSchema.End, out endTime);
                        if (endTime != null)
                        {
                            appointmentRow["EndTime"] = endTime;
                        }

                        item.TryGetProperty(AppointmentSchema.LegacyFreeBusyStatus, out status);
                        if (status != null)
                        {
                            appointmentRow["Status"] = status;
                        }

                        item.TryGetProperty(AppointmentSchema.AppointmentState, out appointmentState);
                        appointmentRow["AppointmentState"] = appointmentState;

                        item.TryGetProperty(AppointmentSchema.IsCancelled, out isCancelled);
                        appointmentRow["IsCancelled"] = isCancelled;

                        item.TryGetProperty(AppointmentSchema.IsAllDayEvent, out isAllDayEvent);
                        appointmentRow["IsAllDayEvent"] = isAllDayEvent;

                        itemId = item.Id;
                        appointmentRow["ChangeKey"] = itemId.ChangeKey;
                        appointmentRow["UniqueID"] = itemId.UniqueId;

                        appointmentsData.Rows.Add(appointmentRow);

                        int percentComplete = 0;
                        if (items.Count() > 0)
                        {
                            percentComplete = ((i + 1) * 100 / items.Count());
                        }
                        else
                        {
                            percentComplete = 100;
                        }
                        UpdateProgress(percentComplete);
                    }
                    catch (Exception ex)
                    {
                        Util.LogError("GetCalendarEvents failed with message: " + ex.Message);
                    }
                }

                bulkCopy.WriteToServer(appointmentsData);

                using (SqlConnection conn = new SqlConnection(Resources.SqlConnectionString))
                {
                    using (SqlCommand command = new SqlCommand(Resources.MergeStoredProcedureName, conn) { CommandType = CommandType.StoredProcedure })
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        private void SynchronizeUsers(string ou)
        {
            List<ADUser> users = new List<ADUser>();
            using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, "TRG", ou))
            {
                using (UserPrincipal qbeUser = new UserPrincipal(ctx))
                {
                    using (PrincipalSearcher srch = new PrincipalSearcher(qbeUser))
                    {
                        foreach (Principal found in srch.FindAll())
                        {
                            try
                            {
                                DirectoryEntry obj = found.GetUnderlyingObject() as DirectoryEntry;
                                ADUser user = new ADUser()
                                {
                                    department = obj.Properties.Contains("department") ? obj.Properties["department"].Value.ToString() : "",
                                    description = obj.Properties.Contains("description") ? obj.Properties["description"].Value.ToString() : "",
                                    displayName = !string.IsNullOrEmpty(found.DisplayName) ? found.DisplayName : "",
                                    manager = obj.Properties.Contains("manager") ? obj.Properties["manager"].Value.ToString() : "",
                                    primaryMail = obj.Properties.Contains("mail") ? obj.Properties["mail"].Value.ToString() : "",
                                    distinguishedName = !string.IsNullOrEmpty(found.DistinguishedName) ? found.DistinguishedName : ""
                                };
                                users.Add(user);
                            }
                            catch (Exception ex)
                            {
                                Util.LogError("SynchronizeUsers failed with message: " + ex.Message);
                            }
                        }
                    }
                }
            }

            DataTable usersData = new DataTable(typeof(ADUser).Name);

            PropertyInfo[] props = typeof(ADUser).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in props)
            {
                usersData.Columns.Add(prop.Name);
            }
            foreach (ADUser user in users)
            {
                var values = new object[props.Length];
                for (int i = 0; i < props.Length; i++)
                {
                    values[i] = props[i].GetValue(user, null);
                }
                usersData.Rows.Add(values);
            }

            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(Resources.SqlConnectionString, SqlBulkCopyOptions.KeepNulls & SqlBulkCopyOptions.KeepIdentity))
            {
                bulkCopy.BatchSize = usersData.Rows.Count;
                bulkCopy.DestinationTableName = "dbo.Users";
                bulkCopy.ColumnMappings.Clear();
                bulkCopy.ColumnMappings.Add("displayName", "DisplayName");
                bulkCopy.ColumnMappings.Add("primaryMail", "PrimaryEmail");
                bulkCopy.ColumnMappings.Add("department", "Department");
                bulkCopy.ColumnMappings.Add("description", "Description");
                bulkCopy.ColumnMappings.Add("manager", "Manager");
                bulkCopy.ColumnMappings.Add("distinguishedName", "DistinguishedName");

                using (SqlConnection conn = new SqlConnection(Resources.SqlConnectionString))
                {
                    using (SqlCommand command = new SqlCommand("truncate table " + bulkCopy.DestinationTableName, conn))
                    {
                        conn.Open();
                        command.ExecuteNonQuery();
                    }
                }

                bulkCopy.WriteToServer(usersData);
            }
        }
    }
}
