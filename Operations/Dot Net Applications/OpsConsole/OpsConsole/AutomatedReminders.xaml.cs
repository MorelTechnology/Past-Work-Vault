using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for AutomatedReminders.xaml
    /// </summary>
    public partial class AutomatedReminders : UserControl
    {
        #region Constants, Class members and Constructor
        DataTable dtNotifications = new DataTable();
        bool Loaded = false;

        public AutomatedReminders()
        {
            InitializeComponent();
        }
        #endregion

        #region LOAD DATA
        public void Load(string perm)
        {
            ////// LOAD DATA //////
            LoadData();

            ////// PLACEHOLDER //////
            if (!Loaded)
            {
            }
        }

        public void LoadData()
        {
            dtNotifications = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "NOTIFY_CRUD_NOTIFICATION", "OPSCONSOLE").Tables["WS"];

            DataRow[] filtered = dtNotifications.Select("EndDate is null");
            dgNotifications.ItemsSource = (filtered.Length == 0) ? null : filtered.CopyToDataTable().DefaultView;
        }
        #endregion

        #region DELETE DATA
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgNotifications.SelectedIndex < 0)
            {
                MessageBox.Show("You must select an item to delete");
            }

            string NotificationID = ((System.Data.DataRowView)dgNotifications.SelectedItem)["NotificationID"].ToString();
            deleteNotification(NotificationID);
            LoadData();
        }

        public void deleteNotification(string id)
        {
            DataTable dtUpdate = generateUpdateTable();
            DataRow drNewAR = dtUpdate.NewRow();
            drNewAR["NotificationID"] = id;
            drNewAR["NotificationFrequency"] = "";
            drNewAR["NotificationSpecificDate"] = "";
            drNewAR["NotificationDays"] = "";
            drNewAR["NotificationDayOfMonth"] = "";
            drNewAR["NotificationWeek"] = "";
            drNewAR["NotificationWeekDOW"] = "";
            drNewAR["NotificationMonths"] = "";
            drNewAR["NotificationExplanation"] = "";
            drNewAR["EmailTo"] = "";
            drNewAR["EmailSubject"] = "";
            drNewAR["EmailBody"] = "";
            drNewAR["TicketCategory"] = "";
            drNewAR["TicketSubcategory"] = "";
            drNewAR["TicketItem"] = "";
            drNewAR["TicketPriority"] = "";
            drNewAR["TicketSite"] = "";
            drNewAR["TicketGroup"] = "";
            drNewAR["TicketTechnician"] = "";
            drNewAR["TicketSubject"] = "";
            drNewAR["TicketDescription"] = "";
            drNewAR["Active"] = "";
            drNewAR["Operation"] = "E";
            dtUpdate.Rows.Add(drNewAR);


            ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, "NOTIFY_CRUD_NOTIFICATION", "OPSCONSOLE");
        }
        #endregion

        #region GENERATE UPDATE TABLE
        public DataTable generateUpdateTable()
        {
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
            return dtUpdate;
        }
        #endregion

        #region NEW / EDIT / EXIT
        private void btnNew_Click(object sender, RoutedEventArgs e)
        {
            // screenEdit.Load(dtDepartments, dtGroups, dtAssociates, dtClaimsCenterLimits, dtDataHavenLimits);
            screenEdit.setupNew(this);
            screenEdit.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgNotifications.SelectedIndex < 0)
            {
                MessageBox.Show("You must select an item to edit");
            }

            screenEdit.setupEdit(((System.Data.DataRowView)dgNotifications.SelectedItem), this);
            screenEdit.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ourMainWindow.showMainScreen();
        }
        #endregion
    }
}
