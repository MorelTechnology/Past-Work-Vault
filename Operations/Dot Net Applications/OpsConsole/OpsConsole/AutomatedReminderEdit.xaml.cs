using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for AutomatedReminderEdit.xaml
    /// </summary>
    public partial class AutomatedReminderEdit : UserControl
    {
        #region Constants, Class members and Constructor
        const string allmonths = "JAN~FEB~MAR~APR~MAY~JUN~JUL~AUG~SEP~OCT~NOV~DEC~";
        OpsConsole.schedSelectFrequency.freqType currentFT;
        AutomatedReminders parent = null;
        bool edit = false;
        string id = "";

        public AutomatedReminderEdit()
        {
            InitializeComponent();
        }
        #endregion

        #region SETUP NEW and SETUP EDIT
        public void setupNew(AutomatedReminders p)
        {
            ////// NO ACTIONS to start //////
            action.ShowEmail = action.ShowTicket = action.ShowRSSE = action.ShowReport = action.ShowSSIS = false;
            actionEmail.Visibility = actionTicket.Visibility = actionRSSE.Visibility = actionReport.Visibility = actionSSIS.Visibility = System.Windows.Visibility.Collapsed;

            /////// Default to weekly //////
            currentFT = schedSelectFrequency.freqType.WEEKLY;
            setupForFrequency(currentFT);
            filterDayOfMonth.setMode(true, 32, 0, 0);
            filterMonth.ShowMonths = allmonths;
            selectorFrequency.setVisualState(currentFT);
            filterTime.ShowTime = DateTime.Parse("1/1/2001 20:00 PM");
            parent = p;

            actionEmail.clear();

            ////// NEW / EDIT INFO //////
            edit = false;
            id = "";
        }

        public void setupEdit(System.Data.DataRowView dataRow, AutomatedReminders p)
        {
            setupNew(p);

            if (dataRow["NotificationFrequency"].ToString() == "ONCE")
            {
                setupForFrequency(schedSelectFrequency.freqType.ONCE);
                selectorFrequency.setVisualState(schedSelectFrequency.freqType.ONCE);

                string NotificationSpecificDate = dataRow["NotificationSpecificDate"].ToString(); // = dayno.ToString();
                DateTime dt = Convert.ToDateTime(NotificationSpecificDate);
                if (dt.Year == 1900)
                    filterDate.setDate(null);
                else
                    filterDate.setDate(dt);
            }
            if (dataRow["NotificationFrequency"].ToString() == "DAILY")
            {
                setupForFrequency(schedSelectFrequency.freqType.DAILY);
                selectorFrequency.setVisualState(schedSelectFrequency.freqType.DAILY);
            }

            if (dataRow["NotificationFrequency"].ToString() == "MONTHLY")
            {
                setupForFrequency(schedSelectFrequency.freqType.MONTHLY);
                selectorFrequency.setVisualState(schedSelectFrequency.freqType.MONTHLY);

                string NotificationDayOfMonth = dataRow["NotificationDayOfMonth"].ToString(); // = dayno.ToString();
                string NotificationWeek = dataRow["NotificationWeek"].ToString(); // = week.ToString();
                string NotificationWeekDOW = dataRow["NotificationWeekDOW"].ToString(); // = day.ToString();
                bool dayMode = (NotificationDayOfMonth == "0") ? false : true;

                filterDayOfMonth.setMode(dayMode, Convert.ToInt32(NotificationDayOfMonth), Convert.ToInt32(NotificationWeek), Convert.ToInt32(NotificationWeekDOW));

                ////// MONTH SELECTOR //////
                string NotificationMonths = dataRow["NotificationMonths"].ToString(); // = filterMonth.ShowMonths;
                filterMonth.ShowMonths = NotificationMonths;
                            }
            if (dataRow["NotificationFrequency"].ToString() == "WEEKLY")
            {
                setupForFrequency(schedSelectFrequency.freqType.WEEKLY);
                selectorFrequency.setVisualState(schedSelectFrequency.freqType.WEEKLY);
            }

            if ((dataRow["NotificationFrequency"].ToString() == "DAILY") || (dataRow["NotificationFrequency"].ToString() == "WEEKLY"))
            {
                ////// DAY OF WEEK SELECTOR //////
                filterDOW.ShowDays = dataRow["NotificationDays"].ToString();
            }

            ////// E-MAIL //////
            string EmailTo = dataRow["EmailTo"].ToString(); // = actionEmail.getToAddresses();
            string EmailSubject = dataRow["EmailSubject"].ToString(); // = actionEmail.getSubject();
            string EmailBody = dataRow["EmailBody"].ToString(); // = actionEmail.getBody();
            actionEmail.setBody(EmailBody);
            actionEmail.setSubject(EmailSubject);
            actionEmail.setToAddresses(EmailTo);
            if (EmailTo != "")
                action.ShowEmail = true;
            else
                action.ShowEmail = false;

            processSelectionChanged();

            ////// SD TICKET //////
            string cat = dataRow["TicketCategory"].ToString();
            string sub = dataRow["TicketSubcategory"].ToString();
            string itm = dataRow["TicketItem"].ToString();
            string priority = dataRow["TicketPriority"].ToString();
            string site = dataRow["TicketSite"].ToString();
            string group = dataRow["TicketGroup"].ToString();
            string subject = dataRow["TicketSubject"].ToString();
            string desc = dataRow["TicketDescription"].ToString();
            string tech = dataRow["TicketTechnician"].ToString();
            actionTicket.Load();
            if (cat != "")
            {
                action.ShowTicket = true;
                actionTicket.Visibility = System.Windows.Visibility.Visible;

                // Was
                // actionTicket.setItems(cat, sub, itm, priority, site, group, sub, desc, tech);

                // Oct 2 2016
                actionTicket.setItems(cat, sub, itm, priority, site, group, subject, desc, tech);
            }

            ////// NEW / EDIT INFO //////
            edit = true;
            id = dataRow["NotificationID"].ToString();
        }
        #endregion

        #region FREQUENCY and FREQUENCY changed events
        private void selectorFrequency_FrequencySelected(object sender, schedSelectFrequency.FreqTypeEventArgs e)
        {
            setupForFrequency(e.ft);
        }

        private void setupForFrequency(OpsConsole.schedSelectFrequency.freqType ft)
        {
            currentFT = ft;

            if (ft == schedSelectFrequency.freqType.ONCE)
            {
                filterDate.Visibility = System.Windows.Visibility.Visible;
                //filterTime.Visibility = System.Windows.Visibility.Visible;
                filterDOW.Visibility = System.Windows.Visibility.Collapsed;
                filterDayOfMonth.Visibility = System.Windows.Visibility.Collapsed;
                filterMonth.Visibility = System.Windows.Visibility.Collapsed;
            }

            if (ft == schedSelectFrequency.freqType.DAILY)
            {
                filterDate.Visibility = System.Windows.Visibility.Collapsed;
                //filterTime.Visibility = System.Windows.Visibility.Visible;
                filterDOW.Visibility = System.Windows.Visibility.Visible;
                filterDayOfMonth.Visibility = System.Windows.Visibility.Collapsed;
                filterMonth.Visibility = System.Windows.Visibility.Collapsed;
                filterDOW.ShowDays = "MTWRF";
            }

            if (ft == schedSelectFrequency.freqType.WEEKLY)
            {
                filterDate.Visibility = System.Windows.Visibility.Collapsed;
                //filterTime.Visibility = System.Windows.Visibility.Visible;
                filterDOW.Visibility = System.Windows.Visibility.Visible;
                filterDayOfMonth.Visibility = System.Windows.Visibility.Collapsed;
                filterMonth.Visibility = System.Windows.Visibility.Collapsed;
                filterDOW.ShowDays = "F";
            }

            if (ft == schedSelectFrequency.freqType.MONTHLY)
            {
                filterDate.Visibility = System.Windows.Visibility.Collapsed;
                //filterTime.Visibility = System.Windows.Visibility.Visible;
                filterDOW.Visibility = System.Windows.Visibility.Collapsed;
                filterDayOfMonth.Visibility = System.Windows.Visibility.Visible;
                filterMonth.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void schedActionSelector_FrequencySelected(object sender, EventArgs e)
        {
            processSelectionChanged();
        }

        private void processSelectionChanged()
        {
            actionEmail.Visibility = (action.ShowEmail) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            actionTicket.Visibility = (action.ShowTicket) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            actionRSSE.Visibility = (action.ShowRSSE) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            actionReport.Visibility = (action.ShowReport) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            actionSSIS.Visibility = (action.ShowSSIS) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            if (action.ShowRSSE)
                actionRSSE.load();
            if (action.ShowEmail)
                actionEmail.load();
            if (action.ShowTicket)
                actionTicket.Load();
        }
        #endregion

        #region SAVE and CANCEL
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string frequency = currentFT.ToString();

            //if (edit)
            //{
            //    DataTable dtDelete = new DataTable();
            //    dtDelete.Columns.Add("NotificationID");
            //    dtDelete.Columns.Add("NotificationFrequency");
            //    dtDelete.Columns.Add("NotificationSpecificDate");
            //    dtDelete.Columns.Add("NotificationDays");
            //    dtDelete.Columns.Add("NotificationDayOfMonth");
            //    dtDelete.Columns.Add("NotificationWeek");
            //    dtDelete.Columns.Add("NotificationWeekDOW");
            //    dtDelete.Columns.Add("NotificationMonths");
            //    dtDelete.Columns.Add("NotificationExplanation");
            //    dtDelete.Columns.Add("EmailTo");
            //    dtDelete.Columns.Add("EmailSubject");
            //    dtDelete.Columns.Add("EmailBody");
            //    dtDelete.Columns.Add("TicketCategory");
            //    dtDelete.Columns.Add("TicketSubcategory");
            //    dtDelete.Columns.Add("TicketItem");
            //    dtDelete.Columns.Add("TicketPriority");
            //    dtDelete.Columns.Add("TicketSite");
            //    dtDelete.Columns.Add("TicketGroup");
            //    dtDelete.Columns.Add("TicketTechnician");
            //    dtDelete.Columns.Add("TicketSubject");
            //    dtDelete.Columns.Add("TicketDescription");
            //    dtDelete.Columns.Add("Active");
            //    dtDelete.Columns.Add("Operation");
            //    dtDelete.TableName = "Notification";

            //    DataRow drDelAR = dtUpdate.NewRow();
            //    drDelAR["Operation"] = "D";
            //    drDelAR["NotificationID"] = id;
            //    ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtDelete, "NOTIFY_CRUD_NOTIFICATION", "OPSCONSOLE");
            //}


            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("NotificationID");
            dtUpdate.Columns.Add("NotificationFrequency");
            dtUpdate.Columns.Add("NotificationSpecificDate");
            dtUpdate.Columns.Add("NotificationDays");
            dtUpdate.Columns.Add("NotificationDayOfMonth");
            dtUpdate.Columns.Add("NotificationWeek");
            dtUpdate.Columns.Add("NotificationWeekDOW");
            dtUpdate.Columns.Add("NotificationMonths");
            dtUpdate.Columns.Add("NotificationExplanation");
            dtUpdate.Columns.Add("EmailTo");
            dtUpdate.Columns.Add("EmailSubject");
            dtUpdate.Columns.Add("EmailBody");
            dtUpdate.Columns.Add("TicketCategory");
            dtUpdate.Columns.Add("TicketSubcategory");
            dtUpdate.Columns.Add("TicketItem");
            dtUpdate.Columns.Add("TicketPriority");
            dtUpdate.Columns.Add("TicketSite");
            dtUpdate.Columns.Add("TicketGroup");
            dtUpdate.Columns.Add("TicketTechnician");
            dtUpdate.Columns.Add("TicketSubject");
            dtUpdate.Columns.Add("TicketDescription");
            dtUpdate.Columns.Add("Active");
            dtUpdate.Columns.Add("Operation");
            dtUpdate.TableName = "Notification";

            DataRow drNewAR = dtUpdate.NewRow();
            drNewAR["NotificationID"] = "0";
            drNewAR["NotificationFrequency"] = currentFT.ToString();

            if (currentFT == schedSelectFrequency.freqType.ONCE)
            {
                DateTime? dt = filterDate.getDate();
                if (dt == null)
                {
                    MessageBox.Show("You must select a date from the calendar");
                    return;
                }
                drNewAR["NotificationSpecificDate"] = ((DateTime) dt);
                drNewAR["NotificationDays"] = "";
            }
            else
                drNewAR["NotificationSpecificDate"] = "1/1/2000";

            if ((currentFT == schedSelectFrequency.freqType.DAILY) || (currentFT == schedSelectFrequency.freqType.WEEKLY))
            {
                drNewAR["NotificationDays"] = filterDOW.daystring();
            }

            if (currentFT == schedSelectFrequency.freqType.MONTHLY)
            {
                drNewAR["NotificationDays"] = "";

                bool dayNoMode=false;
                int dayno=0;
                int week=0;
                int day=0;

                filterDayOfMonth.getMode(ref dayNoMode, ref dayno, ref week, ref day);

                drNewAR["NotificationDayOfMonth"] = dayno.ToString();
                drNewAR["NotificationWeek"] = week.ToString();
                drNewAR["NotificationWeekDOW"] = day.ToString();
                // drNewAR["NotificationMonths"] = filterMonth.ShowMonths;
                drNewAR["NotificationMonths"] = filterMonth.monthstring();
            }
            else
            {
                drNewAR["NotificationDayOfMonth"] = "";
                drNewAR["NotificationWeek"] = "";
                drNewAR["NotificationWeekDOW"] = "";
                drNewAR["NotificationMonths"] = "";
            }

            updateNotificationExplanation(drNewAR);
            drNewAR["EmailTo"] = actionEmail.getToAddresses();
            drNewAR["EmailSubject"] = actionEmail.getSubject();
            drNewAR["EmailBody"] = actionEmail.getBody();
            drNewAR["TicketCategory"] = actionTicket.getCategory();
            drNewAR["TicketSubcategory"] = actionTicket.getSubcategory();
            drNewAR["TicketItem"] = actionTicket.getItem();
            drNewAR["TicketPriority"] = actionTicket.getPriority();
            drNewAR["TicketSite"] = actionTicket.getSite();
            drNewAR["TicketGroup"] = actionTicket.getGroup();
            drNewAR["TicketTechnician"] = actionTicket.getTechnician();
            drNewAR["TicketSubject"] = actionTicket.getSubject();
            drNewAR["TicketDescription"] = actionTicket.getDescription();
            drNewAR["Active"] = "True";
            drNewAR["Operation"] = (edit) ? "U" : "I";
            drNewAR["NotificationID"] = id;
            dtUpdate.Rows.Add(drNewAR);

            ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, "NOTIFY_CRUD_NOTIFICATION", "OPSCONSOLE");

            parent.LoadData();
            Visibility = System.Windows.Visibility.Collapsed;
        }

        public void updateNotificationExplanation(DataRow drNotify)
        {
            string explanation = "";

            if (drNotify["NotificationFrequency"].ToString() == "ONCE")
            {
                explanation = "Once on ";

                string NotificationSpecificDate = drNotify["NotificationSpecificDate"].ToString(); // = dayno.ToString();
                DateTime dt = Convert.ToDateTime(NotificationSpecificDate);
                if (dt.Year != 1900)
                    explanation += dt.ToShortDateString();
            }

            if ((drNotify["NotificationFrequency"].ToString() == "DAILY") || (drNotify["NotificationFrequency"].ToString() == "WEEKLY"))
            {
                if (drNotify["NotificationFrequency"].ToString() == "DAILY")
                    explanation = "Daily ";
                else
                    explanation = "Weekly ";

                if (drNotify["NotificationDays"].ToString() == "MTWRF")
                    explanation += "on Weekdays";

                else if (drNotify["NotificationDays"].ToString() == "MTWRFSS")
                    explanation += "on every day";

                else
                {
                    string days = "";
                    if (drNotify["NotificationDays"].ToString().IndexOf("M") >= 0)  days += "Mon, ";
                    if (drNotify["NotificationDays"].ToString().IndexOf("T") >= 0)  days += "Tue, ";
                    if (drNotify["NotificationDays"].ToString().IndexOf("W") >= 0)  days += "Wed, ";
                    if (drNotify["NotificationDays"].ToString().IndexOf("R") >= 0)  days += "Thr, ";
                    if (drNotify["NotificationDays"].ToString().IndexOf("F") >= 0)  days += "Fri, ";
                    if (drNotify["NotificationDays"].ToString().IndexOf("S") >= 0)  days += "Sat, ";
                    if (drNotify["NotificationDays"].ToString().IndexOf("U") >= 0)  days += "Sun, ";

                    if (days.Length > 2)
                        days = days.Substring(0,days.Length - 2);

                    int lastComma = days.LastIndexOf(", ");
                    if (lastComma > 0)
                        days = days.Substring(0,lastComma) + ", and " + days.Substring(lastComma+2);    // Include Harvard comma

                    explanation += "on " + days;
                }

            }
             
            if (drNotify["NotificationFrequency"].ToString() == "MONTHLY")
            {
                explanation = "Monthly on ";

                if (drNotify["NotificationDayOfMonth"].ToString() != "0")
                {
                    int dayno = Convert.ToInt32(drNotify["NotificationDayOfMonth"].ToString());

                    if (dayno == 32)
                    {
                        explanation += "the last day of the month";
                    }
                    else
                    {
                        explanation += "the " + numberOrdinalSuffix(dayno) + " day of the month";
                    }
                }

                else
                {
                    int weekno = Convert.ToInt32(drNotify["NotificationWeek"].ToString());
                    int weekdow = Convert.ToInt32(drNotify["NotificationWeekDOW"].ToString());
                    string[] daynames = { "", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };

                    string day = (weekdow == 32) ? "last" : numberOrdinalSuffix(weekdow);

                    if (weekno == 1) explanation += numberOrdinalSuffix(weekno) + " " + daynames[weekdow] + " of the month";
                    if (weekno == 2) explanation += numberOrdinalSuffix(weekno) + " " + daynames[weekdow] + " of the month";
                    if (weekno == 3) explanation += numberOrdinalSuffix(weekno) + " " + daynames[weekdow] + " of the month";
                    if (weekno == 4) explanation += numberOrdinalSuffix(weekno) + " " + daynames[weekdow] + " of the month";
                    if (weekno == 5) explanation += "last " + daynames[weekdow] + " of the last week";
                }

                string months = drNotify["NotificationMonths"].ToString();
                if (months != "JAN~FEB~MAR~APR~MAY~JUN~JUL~AUG~SEP~OCT~NOV~DEC~")
                {
                    explanation += " on these months: ";
                    explanation += months.TrimEnd(new char[] { '~' }).Replace("~", ", ");
                }
            }
            else
            {
            }

            drNotify["NotificationExplanation"] = explanation;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion

        #region Number to String helper
        private string numberOrdinalSuffix(int number)
        {
            if ((number == 1) || (number == 21) || (number == 31))
                return number.ToString() + "st";

            if ((number == 2) || (number == 22))
                return number.ToString() + "nd";

            if ((number == 3) || (number == 23))
                return number.ToString() + "rd";

            return number.ToString() + "th";
        }
        #endregion
    }
}
