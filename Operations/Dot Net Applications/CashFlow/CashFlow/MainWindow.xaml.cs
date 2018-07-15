using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Media;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WindowsInput;

namespace CashFlow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string dsn = "";
        public static MainWindow ourMainWindow = null;
        public bool readOnly = false;
        public bool isLauren = false;
        public bool ignoreSTGSelectionChanges = false;
        public SqlConnection staticConn = new SqlConnection();
        public SqlCommand staticCmd = new SqlCommand();

        // June 6 2018
        public enum cfeGrid { MainCFEG, Allocation };
        // End June 6

        #region Background workers

        private System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        //bool Loaded = false;
        //private readonly BackgroundWorker worker = new BackgroundWorker();
        //private readonly BackgroundWorker worker2 = new BackgroundWorker();

        #endregion Background workers

        #region consts

        public const string APPROVED = "Approved";
        public const string DENIED = "Denied";
        public const string UMDENIED = "UM Denied";
        public const string SUBMITTED = "Submitted";
        public const string UMAPPROVED = "UM Submitted";
        public const string PENDING = "Pending";

        #endregion consts

        public Data ourData = new Data();

        // DataTable dtCrit = new DataTable();
        private DataTable dtData = new DataTable();

        private bool blownOpen = false;
        private DataTable dtNumbers = new DataTable();
        private DataTable dtNumbersBeforeEdits = new DataTable();
        private DataTable dtEmptyNumbers = null;
        //DataTable dtNotifications = new DataTable();

        public static userInfo uiLoggedInUser;      // User who logged in. Note: Subbing does not change this, admin switch user does
        public static userInfo uiCurrentUser;      // User who logged in, or substitute user if subbing

        public static string currentFunction = "";

        public string currentExposure = "";
        private string currentWorkMatter = "";
        private bool currentWMisEditable = false;
        private bool currentWMisApprovable = false;
        public string currentSpecialWorkMatter = "";   // This is an additional WM
        private bool hasSeenNotificationMessage = false;

        public DataTable dtCurrentAssociates = null;
        public DataTable dtCurrentExposure = null;

        private bool originalUserWasAdmin = false;

        public class period
        {
            public int year;
            public int quarter;
            public bool readOnly = false;
            public bool total = false;
            public string header = "";
        }

        private List<period> periods = new List<period>();
        //private List<period> PCFAPeriods = new List<period>();

        private void initializePeriods()
        {
            periods = new List<period>();
            DataTable dtCFA = ourData.getCurrentCFAgrid();

            foreach (DataRow dr in dtCFA.Rows)
            {
                string header = dr["CfaGridYear"].ToString() + ((dr["CfaGridQuarter"].ToString() == "0") ? "+" : ("Q" + dr["CfaGridQuarter"].ToString()));
                int year = Convert.ToInt32(dr["CfaGridYear"].ToString());
                int qtr = Convert.ToInt32(dr["CfaGridQuarter"].ToString());
                bool ro = (dr["CfaGridReadWrite"].ToString() == "Y") ? true : false;

                periods.Add(new period() { header = header, year = year, quarter = qtr, readOnly = ro, total = false });
            }
            periods.Add(new period() { header = "Total", year = 0, quarter = 0, readOnly = true, total = true });

        }

        //private void initializePCFAPeriods()
        //{
        //    PCFAPeriods = new List<period>();
        //    DataTable dtCFA = ourData.getCurrentPCFAgrid();

        //    foreach (DataRow dr in dtCFA.Rows)
        //    {
        //        string header = dr["CfaGridYear"].ToString() + ((dr["CfaGridQuarter"].ToString() == "0") ? "+" : ("Q" + dr["CfaGridQuarter"].ToString()));
        //        int year = Convert.ToInt32(dr["CfaGridYear"].ToString());
        //        int qtr = Convert.ToInt32(dr["CfaGridQuarter"].ToString());
        //        bool ro = (dr["CfaGridReadWrite"].ToString() == "Y") ? true : false;

        //        PCFAPeriods.Add(new period() { header = header, year = year, quarter = qtr, readOnly = ro, total = false });
        //    }
        //    PCFAPeriods.Add(new period() { header = "Total", year = 0, quarter = 0, readOnly = true, total = true });
        //}

        private void setupPFIcolumnNames()
        {
            ////// Every year that is in 
            colY1.Header = ourData.PFI[0].year.ToString() + " *";
            colY2.Header = ourData.PFI[1].year.ToString() + " *";

            if (ourData.PFI.Count == 3)
            {
                colY3.Header = ourData.PFI[2].year.ToString() + "+ *";
                colY4.Visibility = Visibility.Collapsed;
            }
            else
            {
                colY3.Header = ourData.PFI[2].year.ToString() + " *";
                colY4.Header = ourData.PFI[3].year.ToString() + "+ *";
            }
        }

        public MainWindow()
        {
            ourMainWindow = this;
            currentFunction = "startup";
            InitializeComponent();

            ////// MAKE SURE PLEASE WAIT IS OFF //////
            pleaseWait(wait: false);

            // Determine if we are DEV, TEST or PROD
            setupDev();
            setupTest();
            setupProd();
            setupLauren();

            ShowDeltas(false);

            // TO DO: Put this somewhere else
            //ourData.firstQuarter = 2;
            //ourData.firstYear = 2017;
            ourData.InitialLoad();



            //loadUsers();
            //loadPermissions();
            //loadSubstitutions();
            ucSecurity.setParent(this);
            notification.setParent(this);
            lblNotifications.Visibility = Visibility.Collapsed;
            rectNotifications.Visibility = Visibility.Collapsed;
            ovalNotifications.Visibility = Visibility.Collapsed;

            showRadioButtonStatus(btnNotifications, new Button[] { btnNotifications, btnPreviousCFA, btnExposure, btnCriticalDates });
            collapseViews();
            notification.Visibility = Visibility.Visible;

            exposure.setParent(this);
            exposure.setupActualLabels(ourData.actualAYear, ourData.actualAQuarter, ourData.actualBYear, ourData.actualBQuarter);

            initializePeriods();
            setupPFIcolumnNames();
            //initializePCFAPeriods();
            buildNumberGrid("True", "True");
            soClean();
            enableCashFlowEntry(false);
            lblSelectExposure1.Visibility = lblSelectExposure2.Visibility = Visibility.Collapsed;

            lblSaved.Visibility = Visibility.Collapsed;
            setupForUser(UserPrincipal.Current, switchingUser: true, subbingUser: false);
        }

        #region DEV / TEST / PROD

        [Conditional("DEV")]
        private void setupDev()
        {
            dsn = "Data Source = sqldev2012r2; Initial Catalog = CashFlow; User Id=cashflow; Password=cashflow";
            lblEnvironment.Text = "DEV";
            //lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 211, 211, 211));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.DEV;
        }

        [Conditional("TEST")]
        private void setupTest()
        {
            dsn = "Data Source = sqltest2012r2; Initial Catalog = CashFlow; User Id=cashflow; Password=C*s4F1O#Trg17";
            // dsn = "Data Source = sqltest2012r2; Initial Catalog = CashFlow; Integrated Security = True";
            lblEnvironment.Text = "TEST";

            // APRIL 8 2018
            // APRIL 10 2018
            //staticConn.ConnectionString = dsn;
            //staticConn.Open();
            //staticCmd = staticConn.CreateCommand();



                // lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 211, 211, 211));
                //lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x99, 0x99, 0x99));
                ScriptEngine.envCurrent = ScriptEngine.environemnt.TEST;
        }

        [Conditional("PROD")]
        private void setupProd()
        {
            dsn = "Data Source = sqlprod2012r2; Initial Catalog = CashFlow; User Id=cashflow; Password=C*s4F1O#Trg17";
            lblEnvironment.Text = "Production";
            //lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xD8, 0x85, 0x20));
            //lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 100, 0, 15));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.PROD;
        }

        [Conditional("LAUREN")]
        private void setupLauren()
        {
            dsn = "Data Source = sqlprod2012r2; Initial Catalog = CashFlow; User Id=cashflow; Password=C*s4F1O#Trg17";
            lblEnvironment.Text = "Read Only";
            // lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 211, 211, 211));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.PROD;
            readOnly = true;
            isLauren = true;
            //goPink();
            //goGold();
        }
        #endregion


        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            gridMain.RowDefinitions[2].MinHeight = 428;
            getCritDates();
        }

        private void gridSplitter_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private string lastName(string lcf)
        {
            int comma = lcf.IndexOf(",");
            return (comma < 0) ? lcf : lcf.Substring(0, comma);
        }

        public string getListOfSelectedUsers()
        {
            string associates = "";

            if (ourData.isUserSuperuser(uiCurrentUser.adid))
            {
                string dept = ourData.getSuperuserDept(uiCurrentUser.adid);
                associates = ourData.getAllUsersInDepartment(dept);
            }
            else if (   ((!isLauren) && cbAssociate.SelectedIndex <= 0) || (isLauren && (cbAssociate.SelectedIndex == 1)))
            {
                if (dtCurrentAssociates == null)
                    return "";

                foreach (DataRow dr in dtCurrentAssociates.Rows)
                    associates += "'" + (dr["AdjustedName"].ToString().Replace("'", "''")) + "',";

                associates = associates.TrimEnd(new char[] { ',' });
            }
            else if (cbAssociate.SelectedIndex > 0)
            {
                string val = cbAssociate.SelectedValue.ToString();
                string name = ((DataRowView)cbAssociate.SelectedItem)["AdjustedName"].ToString();
                // associates += "'" + lastName(name) + "'";
                associates += "'" + name.Replace("'", "''") + "'";
            }

            return associates;
        }

        public void fillWM()
        {
            currentFunction = "fillWM";
            string associates = getListOfSelectedUsers();
            string searchfor = ebFilter.Text.ToUpper();
            string statusFilter = getRadioButtonStatus(new Button[] { btnShowAll, btnShowIncomplete, btnShowApproved, btnShowDenied }).Replace("btnShow", "");
            string stg = "";
            if (cbSpecialTrackingGroup.SelectedIndex >= 0)
                stg = (cbSpecialTrackingGroup.SelectedIndex == 0) ? "" : cbSpecialTrackingGroup.SelectedValue.ToString();

            // dumbourData.getAllWorkMatters();
            loadNotificationsForUser();

            // NEW AUG 10 2017
            ourData.getAllWorkMatters();

            // MIDDLE OF CHANGING
            //if (associates.Length > 260)
            //    dtData = ourData.getWorkMatters("", searchfor, statusFilter, currentSpecialWorkMatter, stg);
            //else
                dtData = ourData.getWorkMatters(associates, searchfor, statusFilter, currentSpecialWorkMatter, stg);

            dgWM.ItemsSource = dtData.DefaultView;
            btnRecall.Visibility = Visibility.Collapsed;
        }


        private void getCritDates()
        {
            // dtCrit = getData("Data Source=manproddata02;Initial Catalog=Critical_Dates;Integrated Security=True", "EXEC[dbo].[rpt_ClaimsDepartmentMonthly]");
        }

        public DataTable getData(string connectionString, string sql)
        {
            // APRIL 8 2018
            // APRIL 10 2018
            //return getData2B(connectionString, sql);

            try
            {
                using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connectionString))
                {
                    dataAdapter.SelectCommand.CommandTimeout = 420;
                    DataTable dataTable = new DataTable();
                    dataAdapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (Exception ex)
            {

                throw;
            }

        }

        //public DataTable getData2(string connectionString, string sql)
        //{
        //    using (var conn = new SqlConnection())
        //    {
        //        using (var cmd = conn.CreateCommand())
        //        {
        //            cmd.CommandText = sql;
        //            cmd.Connection.ConnectionString = connectionString;
        //            cmd.Connection.Open();
        //            var table = new DataTable();
        //            table.Load(cmd.ExecuteReader());

        //            ////// Make every column not read only //////
        //            ////// https://stackoverflow.com/questions/5434833/readonlyexception-datatable-datarow-column-x-is-read-only //////
        //            foreach (DataColumn dc in table.Columns)
        //                dc.ReadOnly = false;

        //            return table;
        //        }
        //    }
        //}

        //public DataTable getData2A(string connectionString, string sql)
        //{

        //    using (var cmd = staticConn.CreateCommand())
        //    {
        //        cmd.CommandText = sql;
        //        var table = new DataTable();
        //        table.Load(cmd.ExecuteReader());

        //        ////// Make every column not read only //////
        //        ////// https://stackoverflow.com/questions/5434833/readonlyexception-datatable-datarow-column-x-is-read-only //////
        //        foreach (DataColumn dc in table.Columns)
        //            dc.ReadOnly = false;

        //        return table;
        //    }
        //}


        public DataTable getData2B(string connectionString, string sql)
        {
            // APRIL 10 2018
            return getData(connectionString, sql);

            //staticCmd.CommandText = sql;
            //staticCmd.Parameters.Clear();
            //staticCmd.CommandType = CommandType.Text;

            //var table = new DataTable();
            //table.Load(staticCmd.ExecuteReader());

            //////// Make every column not read only //////
            //////// https://stackoverflow.com/questions/5434833/readonlyexception-datatable-datarow-column-x-is-read-only //////
            //foreach (DataColumn dc in table.Columns)
            //{
            //    dc.ReadOnly = false;
            //    dc.AllowDBNull = true;
            //}

            //return table;
        }



        public DataTable getData3(string connectionString, string sql)
        {
            using (var conn = new SqlConnection())
            {
                using (var da = new SqlDataAdapter())
                {
                    using (da.SelectCommand = conn.CreateCommand())
                    {
                        da.SelectCommand.CommandText = sql;
                        da.SelectCommand.Connection.ConnectionString = connectionString;
                        DataSet ds = new DataSet(); //conn is opened by dataadapter
                        da.Fill(ds);
                        return ds.Tables[0];
                    }
                }
            }
        }

        //CLA-288 Rotational quarters and years
        public static DataSet getDataSproc(string connectionString, string sProc, string param1, object val1, string param2, object val2)
        {
            DataSet dsSqlProc = new DataSet();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sProc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(param1, val1);
                    cmd.Parameters.AddWithValue(param2, val2);
                    conn.Open();
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();
                    dataAdapter.SelectCommand = cmd;
                    dataAdapter.Fill(dsSqlProc, "aDataSet");
                }
            }
            return dsSqlProc;
        }

        public void executeSQL(string connectionString, string sql)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = con.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void clearExposureInfo()
        {
            exposure.clear();
            lblExpentry.Text = "";
            if (dtCurrentExposure != null)
            {
                dtCurrentExposure.Rows.Clear();
                // populateNumberGrid();
            }
            currentExposure = "";
            // buildNumberGrid();
            dgNumbers.ItemsSource = dtEmptyNumbers.Copy().DefaultView;

            history.dgNumbersHistory.ItemsSource = dtEmptyNumbers.DefaultView;
            // dgNumbers.ItemsSource = dtNumbers.DefaultView;
        }

        private void clearWMInfo()
        {
            clearExposureInfo();
            lblWMentry.Text = "";
            cbBasis.SelectedIndex = -1;
            cbConfidence.SelectedIndex = -1;
            txtRWCCovDJ.Text = "";
            txtRWCDefExp.Text = "";
            txtRWCLoss.Text = "";
            txtComments.Text = "";
            lblCommentCounter.Text = "";
            currentWorkMatter = "";
        }

        private void dgWM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            currentFunction = "WM selection changed";
            processWMSelection();
        }


        public static Dictionary<string, int> timing = new Dictionary<string, int>();
        public static int lastTickCount = 0;

        public static void countIt(string name)
        {
            if (name != "")
            {
                if (timing.ContainsKey(name) == false)
                    timing.Add(name, 0);
                timing[name] += Environment.TickCount - lastTickCount;
            }
            lastTickCount = Environment.TickCount;
        }

        public void processWMSelection()
        {
            int start = Environment.TickCount;

            countIt("");
            clearExposureInfo();
            countIt("clearExposureInfo");

            clearWMInfo();
            countIt("clearWMInfo");

            // tbDescription.Inlines.Clear();
            gridFilterBottom.Visibility = (dgWM.SelectedItems.Count > 1) ? Visibility.Visible : Visibility.Collapsed;
            borderShowAssociations.Visibility = (dgWM.SelectedItems.Count > 1) ? Visibility.Collapsed : Visibility.Visible;

            soClean();

            if (dgWM.SelectedIndex < 0)
            {
                soClean();
                enableWMApprove(false);
                enableWMEdit(false);
                btnRecall.Visibility = Visibility.Collapsed;
                return;
            }

            enableCashFlowEntry(false);

            string wm = ((DataRowView)dgWM.SelectedItem)["WorkMatter"].ToString();
            string aa = ((DataRowView)dgWM.SelectedItem)["AssignedAdjuster"].ToString();
            string status = ((DataRowView)dgWM.SelectedItem)["Status"].ToString();

            currentWorkMatter = wm;

            ////// Enable WorkMatter edit if this WM belongs to the user or they are subbing for the user //////
            countIt("Point A");


            bool isTeamLead = ourData.isUserTeamLead(uiCurrentUser.adid);
            countIt("ourData.isUserTeamLead");

            bool isUnitLead = ourData.isUserUnitLead(uiCurrentUser.adid);
            countIt("ourData.isUserUnitLead");

            // JUNE 12 2018 - NO MULTI SELECT AND APPROVE
            if (dgWM.SelectedItems.Count > 1)
                currentWMisEditable = false;
            else
                currentWMisEditable = ourData.canUserEditWorkmatter(isUnitLead, isTeamLead, uiCurrentUser.name, uiCurrentUser.dept, aa, wm, status);
            countIt("ourData.canUserEditWorkmatter");

            enableWMEdit(currentWMisEditable);

            ////// Enable Manager Actions if this WM belongs to us, or a person we are subbing for //////
            if (dgWM.SelectedItems.Count > 1)
                currentWMisApprovable = false;
            else
                currentWMisApprovable = ourData.canUserApproveWorkmatter(uiCurrentUser.adid, aa, wm, status);
            countIt("ourData.canUserApproveWorkmatter");

            enableWMApprove(currentWMisApprovable);

            txtComments.Text = ((DataRowView)dgWM.SelectedItem)["Comments"].ToString();
            lblCommentCounter.Text = (txtComments.Text.Length == 0) ? "" : txtComments.Text.Length.ToString() + " / 1000";

            txtRWCLoss.Text = ((DataRowView)dgWM.SelectedItem)["RWCLoss"].ToString();
            txtRWCDefExp.Text = ((DataRowView)dgWM.SelectedItem)["RWCDefExp"].ToString();
            txtRWCCovDJ.Text = ((DataRowView)dgWM.SelectedItem)["RWCCovDJ"].ToString();

            foreach (ComboBoxItem cbi in cbBasis.Items)
            {
                if (cbi.Content.ToString() == ((DataRowView)dgWM.SelectedItem)["Basis"].ToString())
                    cbi.IsSelected = true;
            }

            foreach (ComboBoxItem cbi in cbConfidence.Items)
            {
                if (cbi.Content.ToString() == ((DataRowView)dgWM.SelectedItem)["Confidence"].ToString())
                    cbi.IsSelected = true;
            }

            // lblWMentry.Text = "WM ";

            // fixwb1.NavigateToString("   ");
            // fixtbCoverage.Text = "";
            // fix dgCDValues.ItemsSource = null;

            if (dgWM.SelectedItems.Count > 1)
            {
                ////// Scott - 1/5/218 - If they select a second item it can't be dirty because there is no way to save as there is no 1 current item and the edit boxes are erased //////
                soClean();
                btnRecall.Visibility = Visibility.Collapsed;
                return;
            }

            clearExposureInfo();

            countIt("Point B");
            exposure.fillExposures(wm);
            countIt("exposure.fillExposures");

            // April 9 2018 - This screen no longer exists
            DataSet ds = ourData.getWorkMatterHistory(wm);
            //history.dgWorkMatterHistory.ItemsSource = ds.Tables[0].DefaultView;
            //previousCFA.dgNumbers.ItemsSource = ds.Tables[1].DefaultView;
            previousCFA.dgNumbers.ItemsSource = ds.Tables[1].DefaultView;

            //DataTable dtWMHistory = ourData.getWorkMatterHistory(wm);
            //history.dgWorkMatterHistory.ItemsSource = dtWMHistory.DefaultView;

            /// MOVE THIS ////
            ///
            previousCFA.txtBasis.Text = "";
            previousCFA.txtComments.Text = "";

            countIt("Point C");

            foreach (DataRow drWMH in ds.Tables[0].Rows)
            {
                if (drWMH["EndUser"].ToString() == "Former")
                {
                    previousCFA.txtBasis.Text = drWMH["Basis"].ToString();
                    previousCFA.txtComments.Text = drWMH["Comments"].ToString();
                    break;
                }
            }
            countIt("Point D");

            DataTable dtExpHistory = ourData.getExposureHistory(wm);
            countIt("ourData.getExposureHistory");

            btnRecall.Visibility = (ourData.isWMRecallable(isUnitLead, isTeamLead, uiCurrentUser.name, uiCurrentUser.dept, aa, wm, status)) ? Visibility.Visible : Visibility.Collapsed;
            countIt("Point E");

            lblWMentry.Text = wm;

            // JUNE 6 2018 - On selection change make sure DEF EXP UNKNOWN is closed
            // popOpenDefExpUnknownCoumn(unknownOpen: false);

            // JUNE 11 2018
            popOpenDefExpUnknownCoumn(anyExposureHasUnknownDefExp());


            soClean();
        }

        private void addToDataTable(DataTable dt, DataRow dr, string key)
        {
            if (dr[key].ToString() != "")
                dt.Rows.Add(key, dr[key].ToString());
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Reject";

            if (dgWM.SelectedItems.Count == 0)
            {
                MessageBox.Show("You do not have a WorkMatter selected");
                return;
            }

            currentFunction = "Reject";
            ucReason.setApproveOrDeny(RejectionReason.reasonType.rejection);
            ucReason.ebReason.Text = "";
            ucReason.setParent(this);
            ucReason.Visibility = Visibility.Visible;
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Approve";

            if (dgWM.SelectedItems.Count == 0)
            {
                MessageBox.Show("You do not have a WorkMatter selected");
                return;
            }

            ucReason.setApproveOrDeny(RejectionReason.reasonType.approval);
            ucReason.ebReason.Text = "";
            ucReason.setParent(this);
            ucReason.Visibility = Visibility.Visible;
        }

        public void completeRejection()
        {
            ////// REJECTION //////
            if (ucReason.currentPurpose() == RejectionReason.reasonType.rejection)
            {
                currentFunction = "Reject";

                ////// IF WE ARE IN OGC always set to DENIED //////
                if (uiCurrentUser.dept == "OGC")
                    adjustStatusForSelectedItems(DENIED, ucReason.ebReason.Text);
                else
                {
                    ////// IF WE ARE A UM - GO TO UMDENIED //////
                    if (ourData.isUserUnitLead(uiCurrentUser.adid))
                        adjustStatusForSelectedItems(UMDENIED, ucReason.ebReason.Text);
                    else
                        adjustStatusForSelectedItems(DENIED, ucReason.ebReason.Text);
                }
            }

            ////// APPROVAL //////
            else
            {
                ////// If the Manger is from OGC - Or the Manager is a Team lead - Set to APPROVED /////
                if ((uiCurrentUser.dept == "OGC") || (ourData.isUserTeamLead(uiCurrentUser.adid)))
                    adjustStatusForSelectedItems(APPROVED, ucReason.ebReason.Text);

                ////// Otherwise, move the selected item to STAGE 1 approval //////
                else
                    adjustStatusForSelectedItems(UMAPPROVED, ucReason.ebReason.Text);
            }
        }

        // btnSubmit
        private void btnMakeAvailable_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Submit";

            // JUNE 6 2018
            // You cannot submit if there are any CFE entries for any exposures in this WM that are DefExp
            if (workMatterHasOrphanedDefExp(currentWorkMatter))
            {
                MessageBox.Show("This WorkMatter may not be submitted because it has open exposures which have Defense Expense values that do not have their Expense Treatment set to Within or Outside policy limits");
                return;
            }

            string theseThings = "";
            if (cbBasis.SelectedIndex < 0)
                theseThings += "- Basis" + Environment.NewLine;
            if (cbConfidence.SelectedIndex < 0)
                theseThings += "- Confidence Rating" + Environment.NewLine;
            if (txtRWCLoss.Text == "")
                theseThings += "- RWC Loss" + Environment.NewLine;
            if (txtRWCDefExp.Text == "")
                theseThings += "- RWC Def Exp" + Environment.NewLine;
            if (txtRWCCovDJ.Text == "")
                theseThings += "- RWC Cov / DJ Expense" + Environment.NewLine;
            if (txtComments.Text == "")
                theseThings += "- Some comments" + Environment.NewLine;

            if (theseThings != "")
            {
                if (MessageBox.Show("It is recommended to have:" + Environment.NewLine + theseThings + Environment.NewLine + Environment.NewLine + "Do you want to submit anyway?", "Confirm submission", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
            }

            if (isDirty())
            {
                saveAction();
            }

            //if (uiCurrentUser.dept.ToUpper() == "CLAIMS")
            //{
            //    if (ourData.isUserTeamLead(uiCurrentUser.adid))
            //        adjustStatusForSelectedItems(SUBMITTED);

            //}
            if (uiCurrentUser.dept.ToUpper() == "CLAIMS")
            {
                ////// if the WM is assigned to us and we save it, make it pending //////
                if (ourData.getadidFromWM(currentWorkMatter) == uiCurrentUser.adid)
                    adjustStatusForSelectedItems(SUBMITTED);
                else if (ourData.isUserTeamLead(uiCurrentUser.adid))
                    adjustStatusForSelectedItems(APPROVED);
                else if (ourData.isUserUnitLead(uiCurrentUser.adid))
                    adjustStatusForSelectedItems(UMAPPROVED);
                else
                    adjustStatusForSelectedItems(SUBMITTED);
            }
            else
            {
                ////// if the WM is assigned to us and we submit it, make it SUBMITTED //////
                if (ourData.getadidFromWM(currentWorkMatter) == uiCurrentUser.adid)
                    adjustStatusForSelectedItems(SUBMITTED);
                else if (ourData.isUserTeamLead(uiCurrentUser.adid))
                    adjustStatusForSelectedItems(APPROVED);
                else if (ourData.isUserUnitLead(uiCurrentUser.adid))
                    adjustStatusForSelectedItems(APPROVED);
                else
                    adjustStatusForSelectedItems(SUBMITTED);
            }
        }

        // JUNE 6 2018
        private bool workMatterHasOrphanedDefExp(string wm)
        {
            string sql = "select * from [CashFlow].[data].[CashFlowEntry] cfe ";
            sql += "join[CashFlow].[data].[Exposures] e on cfe.Exposure = e.ExpID ";
            sql += "where cfe.WorkMatter = '" + wm + "' and cfe.WorkMatter != Exposure and EndUser is null and ValueName = 'DefExp' and ExpClosed = 0";

            DataTable dt = getData(dsn, sql);
            return dt.Rows.Count >= 1;
        }

        private void adjustStatusForSelectedItems(string status, string declinedReason = "", bool forcestatus = false)
        {
            string dateTime = DateTime.Now.ToString();

            if (dgWM.SelectedItems.Count <= 0)
                return;

            // SCOTT 1/4/18
            bool multiple = false;
            if (dgWM.SelectedItems.Count > 1)
                multiple = true;

            // NEW
            List<string> selectedItems = new List<string>();
            foreach (DataRowView drv in dgWM.SelectedItems)
                if (forcestatus || (status != PENDING) ||
                    ((status == PENDING) && (drv["Status"].ToString() == "")))

                    selectedItems.Add(drv["WorkMatter"].ToString());

            foreach (string wm in selectedItems)
            {
                foreach (DataRow dr in dtData.Rows)
                    if (dr["WorkMatter"].ToString() == wm)
                    {
                        // SCOTT 1/4/18
                        if (multiple)
                            adjustOnlyStatusAndReason(dateTime, dr["WorkMatter"].ToString(), status, declinedReason);
                        else
                            adjustStatus(dateTime, dr["WorkMatter"].ToString(), status, declinedReason);
                    }
            }

            dgWM.ItemsSource = dtData.DefaultView;
            loadNotificationsForUser();
        }

        //private DataTable getStatus(string WorkMatter)
        //{
        //    return getData(dsn, "select * from[CashFlow].[data].[Notifications] where WorkMatter = '" + WorkMatter + "' and EndTime is null");
        //}

        private void adjustStatus(string datetime, string WorkMatter, string status, string declinedReason)
        {
            DataTable dtNotifyBacking = new DataTable();
            dtNotifyBacking.Columns.Add("Analyst");
            dtNotifyBacking.Columns.Add("WorkMatter");
            dtNotifyBacking.Columns.Add("Status");
            dtNotifyBacking.Columns.Add("Basis");
            dtNotifyBacking.Columns.Add("Comments");
            dtNotifyBacking.Columns.Add("ReasonableWorstCase");
            dtNotifyBacking.Columns.Add("Confidence");
            dtNotifyBacking.Columns.Add("Viewed");
            dtNotifyBacking.Columns.Add("DeclinedReason");
            dtNotifyBacking.Columns.Add("RWCLoss");
            dtNotifyBacking.Columns.Add("RWCDefExp");
            dtNotifyBacking.Columns.Add("RWCCovDJ");
            dtNotifyBacking.Columns.Add("Operation");
            dtNotifyBacking.TableName = "Notification";

            string currentBasis = (cbBasis.SelectedIndex < 0) ? "" : ((ComboBoxItem)cbBasis.SelectedItem).Content.ToString().Replace("'", "''");
            string currentConf = (cbConfidence.SelectedIndex < 0) ? "" : ((ComboBoxItem)cbConfidence.SelectedItem).Content.ToString().Replace("'", "''");
            int iRWCLoss = money(txtRWCLoss.Text);
            int iRWCDefExp = money(txtRWCDefExp.Text);
            int iRWCCovDJ = money(txtRWCCovDJ.Text);

            string adid = ourData.getadidFromWM(WorkMatter);
            dtNotifyBacking.Rows.Add(adid, WorkMatter, status, currentBasis, txtComments.Text, 0, currentConf, 0, declinedReason, iRWCLoss, iRWCDefExp, iRWCCovDJ, "X");

            // ourData.dtNotifications = ourData.se.runScript(dtNotifyBacking, "CASH_CRUD_NOTIFICATION", "OPSCONSOLE");
            ourData.dtNotifications = ourData.runStoredProcedure(dsn, "data.sp_CRUD_Notifications", "@NotInput", dtNotifyBacking);

            fillWM();
        }

        // SCOTT 1/4/18 - Adjust only status and nothing else
        private void adjustOnlyStatusAndReason(string datetime, string WorkMatter, string status, string declinedReason)
        {
            string adid = ourData.getadidFromWM(WorkMatter);

            ourData.dtNotifications = ourData.runStoredProcedure(dsn, "data.sp_ModifyNotificationStatus", "@WorkMatter", WorkMatter, "@NewStatus", status, "@Reason", declinedReason);

            fillWM();
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            gridHelp.Visibility = Visibility.Collapsed;
            gridOpaque.Visibility = Visibility.Collapsed;
            // fix wb1.Visibility = Visibility.Visible;
        }

        private void image1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            gridHelp.Visibility = Visibility.Visible;
            gridOpaque.Visibility = Visibility.Visible;
            // fix wb1.Visibility = Visibility.Collapsed;
        }

        private int money(string m)
        {
            int o = 0;
            Int32.TryParse(moneyToNormal(m), out o);
            return o;
        }

        private string moneyToNormal(string m)
        {
            return m.ToUpper().Replace("$", "").Replace(",", "").Replace("K", "000").Replace("M", "000000");
        }

        private void dgNumbers_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }

        private void dgNumbers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
        }

        private void tbPeriod_LostFocus(object sender, RoutedEventArgs e)
        {
        }

        public void calc()
        {
            lblError.Text = "";

            foreach (DataRow dr in dtNumbers.Rows)
            {
                string loss = dr["Loss"].ToString();
                string defexpin = dr["DefExpIn"].ToString();
                string defexpout = dr["DefExpOut"].ToString();
                string covdj = dr["CovDJ"].ToString();

                // June 6 2018
                string defexp = dr["DefExp"].ToString();

                int iloss = 0;
                if (moneyToNormal(loss) != "")
                    if (Int32.TryParse(moneyToNormal(loss), out iloss))
                    {
                        if (loss != iloss.ToString("C0"))
                            soDirty();
                        dr["Loss"] = iloss.ToString("C0");
                    }
                    else
                    {
                        lblError.Text = loss + " is not a valid number";
                    }

                int idefexpin = 0;
                if (moneyToNormal(defexpin) != "")
                    if (Int32.TryParse(moneyToNormal(defexpin), out idefexpin))
                    {
                        if (defexpin != idefexpin.ToString("C0"))
                            soDirty();
                        dr["DefExpIn"] = idefexpin.ToString("C0");
                    }
                    else
                    {
                        lblError.Text = defexpin + " is not a valid number";
                    }

                int idefexpout = 0;
                if (moneyToNormal(defexpout) != "")
                    if (Int32.TryParse(moneyToNormal(defexpout), out idefexpout))
                    {
                        if (defexpout != idefexpout.ToString("C0"))
                            soDirty();
                        dr["DefExpOut"] = idefexpout.ToString("C0");
                    }
                    else
                    {
                        lblError.Text = defexpout + " is not a valid number";
                    }

                // JUNE 6 2018
                int idefexp = 0;
                if (moneyToNormal(defexp) != "")
                    if (Int32.TryParse(moneyToNormal(defexp), out idefexp))
                    {
                        if (defexp != idefexp.ToString("C0"))
                            soDirty();
                        dr["DefExp"] = idefexp.ToString("C0");
                    }
                    else
                    {
                        lblError.Text = defexp + " is not a valid number";
                    }



                int icovdj = 0;
                if (moneyToNormal(covdj) != "")
                    if (Int32.TryParse(moneyToNormal(covdj), out icovdj))
                    {
                        if (covdj != icovdj.ToString("C0"))
                            soDirty();
                        dr["CovDJ"] = icovdj.ToString("C0");
                    }
                    else
                    {
                        lblError.Text = covdj + " is not a valid number";
                    }

                // CHANGE JUNE 6 2018
                int itotal = iloss + idefexpin + idefexpout + +idefexp + icovdj;
                if (itotal > 0)
                    dr["Total"] = itotal.ToString("C0");
            }

            //PutValueInRowCol(ref dtNumbers, 8, 1, GetValueFromRowCol(dtNumbers, 0, 1) + GetValueFromRowCol(dtNumbers, 1, 1) + GetValueFromRowCol(dtNumbers, 2, 1) + GetValueFromRowCol(dtNumbers, 3, 1) + GetValueFromRowCol(dtNumbers, 4, 1) + GetValueFromRowCol(dtNumbers, 5, 1) + GetValueFromRowCol(dtNumbers, 6, 1) + GetValueFromRowCol(dtNumbers, 7, 1));
            //PutValueInRowCol(ref dtNumbers, 8, 2, GetValueFromRowCol(dtNumbers, 0, 2) + GetValueFromRowCol(dtNumbers, 1, 2) + GetValueFromRowCol(dtNumbers, 2, 2) + GetValueFromRowCol(dtNumbers, 3, 2) + GetValueFromRowCol(dtNumbers, 4, 2) + GetValueFromRowCol(dtNumbers, 5, 2) + GetValueFromRowCol(dtNumbers, 6, 2) + GetValueFromRowCol(dtNumbers, 7, 2));
            //PutValueInRowCol(ref dtNumbers, 8, 3, GetValueFromRowCol(dtNumbers, 0, 3) + GetValueFromRowCol(dtNumbers, 1, 3) + GetValueFromRowCol(dtNumbers, 2, 3) + GetValueFromRowCol(dtNumbers, 3, 3) + GetValueFromRowCol(dtNumbers, 4, 3) + GetValueFromRowCol(dtNumbers, 5, 3) + GetValueFromRowCol(dtNumbers, 6, 3) + GetValueFromRowCol(dtNumbers, 7, 3));
            //PutValueInRowCol(ref dtNumbers, 8, 4, GetValueFromRowCol(dtNumbers, 8, 1) + GetValueFromRowCol(dtNumbers, 8, 2) + GetValueFromRowCol(dtNumbers, 8, 3));
            //            dtNumbers.Columns.Count

            int lastRowsInGrid = dtNumbers.Rows.Count -1;

            // WAS
            // for (int iCol = 1; iCol <= 5; iCol++)

            // JUN 6 2018
            for (int iCol = 1; iCol <= 6; iCol ++)
            {
                int total = 0;
                for (int iRow = 0; iRow <= lastRowsInGrid-1; iRow++)
                    total += GetValueFromRowCol(dtNumbers, iRow, iCol);
                PutValueInRowCol(ref dtNumbers, lastRowsInGrid, iCol, total);
            }

//            PutValueInRowCol(ref dtNumbers, lastRowsInGrid, 1, GetValueFromRowCol(dtNumbers, 0, 1) + GetValueFromRowCol(dtNumbers, 1, 1) + GetValueFromRowCol(dtNumbers, 2, 1) + GetValueFromRowCol(dtNumbers, 3, 1) + GetValueFromRowCol(dtNumbers, 4, 1) + GetValueFromRowCol(dtNumbers, 5, 1) + GetValueFromRowCol(dtNumbers, 6, 1));
//            PutValueInRowCol(ref dtNumbers, lastRowsInGrid, 2, GetValueFromRowCol(dtNumbers, 0, 2) + GetValueFromRowCol(dtNumbers, 1, 2) + GetValueFromRowCol(dtNumbers, 2, 2) + GetValueFromRowCol(dtNumbers, 3, 2) + GetValueFromRowCol(dtNumbers, 4, 2) + GetValueFromRowCol(dtNumbers, 5, 2) + GetValueFromRowCol(dtNumbers, 6, 2));
//            PutValueInRowCol(ref dtNumbers, lastRowsInGrid, 3, GetValueFromRowCol(dtNumbers, 0, 3) + GetValueFromRowCol(dtNumbers, 1, 3) + GetValueFromRowCol(dtNumbers, 2, 3) + GetValueFromRowCol(dtNumbers, 3, 3) + GetValueFromRowCol(dtNumbers, 4, 3) + GetValueFromRowCol(dtNumbers, 5, 3) + GetValueFromRowCol(dtNumbers, 6, 3));
//            PutValueInRowCol(ref dtNumbers, lastRowsInGrid, 4, GetValueFromRowCol(dtNumbers, lastRowsInGrid, 1) + GetValueFromRowCol(dtNumbers, lastRowsInGrid, 2) + GetValueFromRowCol(dtNumbers, lastRowsInGrid, 3));


            dgNumbers.ItemsSource = dtNumbers.DefaultView;
        }

        public void PutValueInRowCol(ref DataTable dt, int row, int col, int val)
        {
            dt.Rows[row][col] = (val == 0) ? "" : val.ToString("C0");
        }

        public int GetValueFromRowCol(DataTable dt, int row, int col)
        {
            string val = dt.Rows[row][col].ToString();
            return money(val);
        }

        private void tbPeriod_PreviewLostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            calc();
            soDirty();
        }

        private void tbPeriod_SourceUpdated(object sender, DataTransferEventArgs e)
        {
            calc();
            soDirty();
        }

        private void tbPeriod_LostMouseCapture(object sender, MouseEventArgs e)
        {
            calc();
            soDirty();
        }

        private void dgNumbers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calc();
        }

        private void dgNumbers_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            calc();
        }

        private void btnSecurity_Click(object sender, RoutedEventArgs e)
        {
            if (isLauren == true)
                return;

            currentFunction = "Security";
            ucSecurity.updateScreen();
            ucSecurity.Visibility = Visibility.Visible;
            // fix wb1.Visibility = Visibility.Collapsed;
        }

        public void makeWBVisible()
        {
            // fix wb1.Visibility = Visibility.Visible;
        }

        public void showRadioButtonStatus(Button btnSet, Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                {
                    if (c is Rectangle)
                        ((Rectangle)c).Opacity = (b == btnSet) ? 1d : 0.2d;
                    if (c is Ellipse)
                        ((Ellipse)c).Opacity = (b == btnSet) ? 1d : 0.2d;
                }
        }

        public string getRadioButtonStatus(Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                    if (c is Rectangle)
                        if (((Rectangle)c).Opacity == 1d)
                            return b.Name;
            return "";
        }

        public bool isCheckboxChecked(Button button)
        {
            foreach (Object c in ((StackPanel)button.Content).Children)
            {
                if (c is Rectangle)
                    if (((Rectangle)c).Opacity == 1d)
                        return true;

                if (c is Ellipse)
                    if (((Ellipse)c).Opacity == 1d)
                        return true;
            }
            return false;
        }

        public void setCheckbox(Button button, bool chkd)
        {
            foreach (Object c in ((StackPanel)button.Content).Children)
            {
                if (c is Rectangle)
                    ((Rectangle)c).Opacity = (chkd) ? 1d : 0.2d;
                if (c is Ellipse)
                    ((Ellipse)c).Opacity = (chkd) ? 1d : 0.2d;
            }

        }

        private void lblWelcome_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            whoAmI();
        }

        private void lblCurrentUser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DataTable dt = new DataTable();
            whoAmI();
        }

        private void whoAmI()
        {
            string info = "You are " + uiCurrentUser.name + " in the " + uiCurrentUser.dept + " department" + Environment.NewLine + Environment.NewLine;
            if (uiCurrentUser.dept == "Claims")
            {
                info += "Your workmatters will be approved by your unit manger: " + uiCurrentUser.unit_name + Environment.NewLine;
                info += "Then by your team manager:" + uiCurrentUser.team_name + Environment.NewLine;
            }
            else
            {
                info += "Your workmatters will be approved by your unit manger: " + uiCurrentUser.unit_name + Environment.NewLine;
                info += "Or by your team manager:" + uiCurrentUser.team_name + Environment.NewLine;
            }

            info += Environment.NewLine;

            info += "You are " + ((ourData.isUserUnitLead(uiCurrentUser.adid)) ? "" : "not") + " a unit manager" + Environment.NewLine;
            info += "You are " + ((ourData.isUserTeamLead(uiCurrentUser.adid)) ? "" : "not") + " a team manager" + Environment.NewLine;
            info += "You are " + ((ourData.isUserAdmin(uiCurrentUser.adid)) ? "" : "not") + " a CashFlow admin" + Environment.NewLine;
            info += "You are " + ((ourData.isUserAdmin(uiCurrentUser.adid)) ? "" : "not") + " a superuser (above team lead)" + Environment.NewLine;
            info += Environment.NewLine;

            info += "Your login credentials are: " + "trg\\" + uiCurrentUser.sam + Environment.NewLine;
            info += "Your Active Directory ID is: " + uiCurrentUser.adid + Environment.NewLine;

            string sub = ourData.whoISubbingFor(uiCurrentUser.adid);
            if (sub == "")
                sub = "Nobody ";
            else
                sub = ourData.UIfromADID(sub).name;

            info += sub + " is substituting for you";

            MessageBox.Show(info, "Information for " + uiCurrentUser.name);
        }

        private void lblVersion_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            whoAmI();
        }

        private void switchUser()
        {
            //if ((((SolidColorBrush)gridBlueBar.Background).Color.R != 0xC2) && (((SolidColorBrush)gridBlueBar.Background).Color.R != 209))
            //    return;

            hasSeenNotificationMessage = false;

            soClean();
            ucQAUser.setOurMainWindow(this);
            ucQAUser.fill();
            ucQAUser.lblTitle.Text = "Switch to";
            ucQAUser.purpose = "QA";
            ucQAUser.Visibility = Visibility.Visible;
        }

        public void completeSwitchUser(string samAccountName, string adjustedName, string ID)
        {
            setupForUser(UserPrincipal.FindByIdentity(UserPrincipal.Current.Context, IdentityType.Sid, ID), switchingUser: true, subbingUser: false);
        }

        private void cbSubstitutingFor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string adid = (cbSubstitutingFor.SelectedValue == null) ? uiLoggedInUser.adid : cbSubstitutingFor.SelectedValue.ToString();
            setupForUser(UserPrincipal.FindByIdentity(UserPrincipal.Current.Context, IdentityType.Sid, adid), switchingUser: false, subbingUser: true);
        }


        private void setupForUser(UserPrincipal userPrincipal, bool switchingUser, bool subbingUser)
        {
            if (userPrincipal == null)
            {
                MessageBox.Show("You do not have access to the CashFlow tool. Please submit a Service Desk ticket. Our appologies for the inconvenience.");
                Environment.Exit(0);
            }

            currentFunction = "Setup for user " + userPrincipal.SamAccountName;
            uiCurrentUser = ourData.UIfromADID(userPrincipal.Sid.ToString());

            // Jun 10 2018
            if (uiLoggedInUser == null)
                uiLoggedInUser = uiCurrentUser;

            if (uiCurrentUser.adid == null)
            {
                MessageBox.Show("You do not have access to the CashFlow tool. Please submit a Service Desk ticket. Our appologies for the inconvenience.");
                Environment.Exit(0);
            }


            ignoreSTGSelectionChanges = true;
            cbSpecialTrackingGroup.ItemsSource = null;
            cbSpecialTrackingGroup.SelectedIndex = -1;
            ignoreSTGSelectionChanges = false;

            // User was found but SamAccountNames Mismatch. Update, to avoid issues retrieving data.
            if (string.IsNullOrWhiteSpace(uiCurrentUser.adid))
                if (userPrincipal.SamAccountName.ToUpper().Replace("TRG\\", "") != uiCurrentUser.sam.ToUpper().Replace("TRG\\", ""))
                    resynchronizeUser(userPrincipal, uiCurrentUser);

            if (switchingUser)
            {
                uiLoggedInUser = uiCurrentUser;
                lblCurrentUser.Text = uiCurrentUser.name;
            }

            adjustForCurrentUser();
            loadNotificationsForUser();

            ////// If this is the first time the application is being launched and the user is an Admin make note //////
            if (ourData.isUserAdmin(uiCurrentUser.sam))
                originalUserWasAdmin = true;

            // Populate substitutions dropdown
            //string adid = ((DataRowView)dgAssociates.SelectedItem)["ActiveDirectoryID"].ToString();
            if (switchingUser)
            {
                DataTable dtSubs = ourData.getSubstitutionsForSub(uiCurrentUser.adid, true);
                if (dtSubs.Rows.Count <= 1)
                    showSubstitutionsSelection(false);
                else
                {
                    showSubstitutionsSelection(true);
                    cbSubstitutingFor.ItemsSource = dtSubs.DefaultView;
                }
            }

            if (ourData.isUserAdmin(userPrincipal.SamAccountName))
            {
                btnSecurity.Visibility = Visibility.Visible;
                lblAdditional.Visibility = Visibility.Visible;
            }
            else
            {
                btnSecurity.Visibility = Visibility.Collapsed;
                lblAdditional.Visibility = Visibility.Collapsed;
            }
            clearWMInfo();
        }

        private void showSubstitutionsSelection(bool show)
        {
            if (show)
            {
                lblSubstitutingFor.Visibility = Visibility.Visible;
                cbSubstitutingFor.Visibility = Visibility.Visible;
            }
            else
            {
                cbSubstitutingFor.Visibility = Visibility.Collapsed;
                lblSubstitutingFor.Visibility = Visibility.Collapsed;
            }
        }
        private void resynchronizeUser(UserPrincipal userPrincipal, userInfo oldUserInfo)
        {
            DataTable dtUserUpdate = new DataTable();
            dtUserUpdate.Columns.Add("old_sam");
            dtUserUpdate.Columns.Add("current_sam");
            dtUserUpdate.Columns.Add("old_FirstName");
            dtUserUpdate.Columns.Add("current_FirstName");
            dtUserUpdate.Columns.Add("old_LastName");
            dtUserUpdate.Columns.Add("current_LastName");
            dtUserUpdate.Columns.Add("current_Email");

            dtUserUpdate.Rows.Add(oldUserInfo.sam, userPrincipal.SamAccountName, oldUserInfo.name.Split(' ')[0], userPrincipal.GivenName, oldUserInfo.name.Split(' ')[1], userPrincipal.Surname, userPrincipal.EmailAddress);
            ourData.runStoredProcedure(dsn, "data.sp_UpdateUser", "@UserUpdateInput", dtUserUpdate);
        }

        private void loadNotificationsForUser()
        {
            // LAUREN TEST
            //if (isLauren)
            //    return;

            string associates = "";

            if ((dtCurrentAssociates == null) || (dtCurrentAssociates.Rows.Count == 0))
                return;

            foreach (DataRow dr in dtCurrentAssociates.Rows)
                associates += "'" + dr["ActiveDirectoryID"].ToString() + "',";

            associates = associates.TrimEnd(new char[] { ',' });

            bool isTeamLead = ourData.isUserTeamLead(uiCurrentUser.adid);
            bool isUnitLead = ourData.isUserUnitLead(uiCurrentUser.adid);
            // ourData.dtNotifications = ourData.getNotificationsForAnalysts(isTeamLead, uiCurrentUser.adid, associates);
            DataTable dtNot = ourData.getNotificationsForAnalysts(isUnitLead, isTeamLead, uiCurrentUser.dept.ToUpper(), uiCurrentUser.adid, associates);

            if (dtNot == null)
            {
                notification.dgNotifictions.ItemsSource = null;
                switchToExposure();
            }
            else
            {
                notification.dgNotifictions.ItemsSource = dtNot.DefaultView;
                if (dtNot.Rows.Count > 0)
                {
                    showNotificationsMessageIfNecessary();
                    switchToNotifications();
                }
                else
                    switchToExposure();
            }
        }

        public void enableCashFlowEntry(bool enable, bool showSelectMessage=true)
        {
            if (!enable)
                lblError.Text = "";
            dgNumbers.IsEnabled = (enable && currentWMisEditable);

            if (dgNumbers.IsEnabled)
            {
                dgNumbers.Background = new SolidColorBrush(Color.FromArgb(0xFF, 240, 240, 240));
                
            }
            else
            {
                dgNumbers.Background = new SolidColorBrush(Color.FromArgb(0xFF, 224, 224, 224));
            }

            // Consider this..... it seems good
            dgNumbers.IsReadOnly = !dgNumbers.IsEnabled;

            if (showSelectMessage)
                lblSelectExposure1.Visibility = (enable) ? Visibility.Collapsed : Visibility.Visible;
            else
                lblSelectExposure1.Visibility = Visibility.Collapsed;

            lblSelectExposure2.Visibility = Visibility.Collapsed;
        }

        public void enableWMEdit(bool enable)
        {
            Visibility vis = (enable) ? Visibility.Visible : Visibility.Collapsed;

            cbBasis.IsEnabled = cbConfidence.IsEnabled = txtRWCLoss.IsEnabled = txtRWCDefExp.IsEnabled = txtRWCCovDJ.IsEnabled = txtComments.IsEnabled = enable;
            btnSpread.IsEnabled = enable;
            btnMakeAvailable.Visibility = vis;
            borderSave.Visibility = vis;
            lblAnalystActions.Visibility = vis;
        }

        public void enableWMApprove(bool enable)
        {
            Visibility vis = (enable) ? Visibility.Visible : Visibility.Collapsed;

            btnApprove.Visibility = vis;
            btnReject.Visibility = vis;
            lblManagerActions.Visibility = vis;
        }

        private void showNotificationsMessageIfNecessary()
        {
            animateNotificationsMessage();
            //foreach (DataRow dr in ourData.dtNotifications.Rows)
            //    if (dr["Viewed"].ToString() == "False")
            //    {
            //        animateNotificationsMessage();
            //    }
        }

        private void animateNotificationsMessage()
        {
            // LAUREN TEST
            //if (isLauren)
            //    return;

            if (hasSeenNotificationMessage)
                return;

            hasSeenNotificationMessage = true;

            lblNotifications.Visibility = Visibility.Visible;
            rectNotifications.Visibility = Visibility.Visible;
            ovalNotifications.Visibility = Visibility.Visible;
            lblNotifications.Opacity = 1d;
            rectNotifications.Opacity = 1d;
            ovalNotifications.Opacity = 1d;

            ////// FADE OUT NEW USER //////
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1d;
            da.To = 0d;
            da.Duration = new Duration(TimeSpan.FromSeconds(5.5d));

            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 1d;
            da2.To = 0d;
            da2.Duration = new Duration(TimeSpan.FromSeconds(5.5d));

            DoubleAnimation da3 = new DoubleAnimation();
            da3.From = 1d;
            da3.To = 0d;
            da3.Duration = new Duration(TimeSpan.FromSeconds(5.5d));

            ////// IF YOU DON'T KILL THE ANIMATION, YOU CAN'T SET THE OPACITY PROPERTY //////
            // da.Completed += (ss, ee) => { gridNewUser.BeginAnimation(OpacityProperty, null); };
            da.Completed += (ss, ee) =>
            {
                lblNotifications.Visibility = Visibility.Collapsed;
                lblNotifications.BeginAnimation(OpacityProperty, null);
            };

            da2.Completed += (ss, ee) =>
            {
                rectNotifications.Visibility = Visibility.Collapsed;
                rectNotifications.BeginAnimation(OpacityProperty, null);
            };

            da3.Completed += (ss, ee) =>
            {
                ovalNotifications.Visibility = Visibility.Collapsed;
                ovalNotifications.BeginAnimation(OpacityProperty, null);
            };

            ////// START ANIMATION //////
            lblNotifications.BeginAnimation(OpacityProperty, da);
            rectNotifications.BeginAnimation(OpacityProperty, da2);
            ovalNotifications.BeginAnimation(OpacityProperty, da3);
        }

        private void adjustForCurrentUser()
        {
            dtCurrentAssociates = ourData.getAllAssociatesForMe(uiCurrentUser.adid);

            bool Manager = (dtCurrentAssociates.Rows.Count > 1);

            DataTable dtCopy = dtCurrentAssociates.Copy();
            DataRow blank = dtCopy.NewRow();
            dtCopy.Rows.InsertAt(blank, 0);

            if (isLauren)
            {
                DataRow star = dtCopy.NewRow();
                star["AdjustedName"] = "All";
                dtCopy.Rows.InsertAt(star, 1);
            }

            // TEMP TEMP MAY 2
            //dtCopy.Columns.Add("Color");
            //dtCopy.Columns.Add("DisplayAs");
            //foreach (DataRow dr in dtCopy.Rows)
            //{
            //    if (dr["Department"].ToString() == "Claims")
            //        dr["Color"] = "Blue";
            //    if (dr["Department"].ToString() == "OGC")
            //        dr["Color"] = "Red";

            //    if (lblEnvironment.Text.StartsWith("Read"))
            //    {
            //        if ((dr["AdjustedName"].ToString() == "" || dr["AdjustedName"].ToString() == "All"))
            //            dr["DisplayAs"] = dr["AdjustedName"].ToString();
            //        else
            //            dr["DisplayAs"] = dr["AdjustedName"].ToString() + "  (" + dr["Department"].ToString() + ")";
            //    }
            //    else
            //        dr["DisplayAs"] = dr["AdjustedName"].ToString();
            //}

            //if (isLauren)
             //   cbAssociate.Items.Clear();
            //cbAssociate.ItemsSource = dtCopy.DefaultView;
            cbAssociate.ItemsSource = dtCopy.DefaultView;
            fillWM();

            enableWMApprove(false);
            enableWMEdit(false);
        }

        private void btnNotifications_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Notifications";
            switchToNotifications();
        }

        private void switchToNotifications()
        {
            showRadioButtonStatus(btnNotifications, new Button[] { btnNotifications, btnPreviousCFA, btnExposure, btnCriticalDates });
            collapseViews();
            notification.Visibility = Visibility.Visible;
        }

        private void btnPreviousCFA_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Previous CFA";
            showRadioButtonStatus(btnPreviousCFA, new Button[] { btnNotifications, btnPreviousCFA, btnExposure, btnCriticalDates });
            collapseViews();
            // claims.Visibility = Visibility.Visible;
            previousCFA.Visibility = Visibility.Visible;
        }

        private void btnExposure_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Exposure";
            switchToExposure();
        }

        private void switchToExposure()
        {
            showRadioButtonStatus(btnExposure, new Button[] { btnNotifications, btnPreviousCFA, btnExposure, btnCriticalDates });
            collapseViews();
            exposure.Visibility = Visibility.Visible;
            borderSpread.Visibility = Visibility.Visible;
            // borderSplit.Visibility = Visibility.Visible;
        }

        private void btnCriticalDates_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnCriticalDates, new Button[] { btnNotifications, btnPreviousCFA, btnExposure, btnCriticalDates });
            collapseViews();
            criticalDates.Visibility = Visibility.Visible;
        }

        //private void btnHistory_Click(object sender, RoutedEventArgs e)
        //{
        //    MessageBox.Show("This feature not currently available");
        //    return;
        //    currentFunction = "History";
        //    showRadioButtonStatus(btnHistory, new Button[] { btnNotifications, btnPreviousCFA, btnExposure, btnCriticalDates });
        //    collapseViews();
        //    history.Visibility = Visibility.Visible;
        //}

        private void collapseViews()
        {
            notification.Visibility = Visibility.Collapsed;
            exposure.Visibility = Visibility.Collapsed;
            history.Visibility = Visibility.Collapsed;
            claims.Visibility = Visibility.Collapsed;
            criticalDates.Visibility = Visibility.Collapsed;
            borderSpread.Visibility = Visibility.Collapsed;
            // borderSplit.Visibility = Visibility.Collapsed;
            borderSaveExposureSplit.Visibility = Visibility.Collapsed;
            previousCFA.Visibility = Visibility.Collapsed;
        }

        private void ebFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void ebFilter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                // your event handler here
                e.Handled = true;
                fillWM();
                //MessageBox.Show("Enter Key is pressed! Searching for" + ebFilter.Text);
            }
        }

        public void filterOn(string wm)
        {
            ebFilter.Text = wm;
            fillWM();
        }

        private void cbAssociate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // LAUREN TEST
            //if (isLauren)
            //    return;

            currentFunction = "Select Associate";
            fillWM();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            cbAssociate.SelectedIndex = -1;
            fillWM();
        }

        private void btnClearFilter_Click_1(object sender, RoutedEventArgs e)
        {
            ebFilter.Text = "";
            fillWM();
        }

        /////// EXPOSURE ///////

        #region exposure

        public void loadNumbersForExposure(string exposure)
        {
            currentFunction = "Load numbers for exposure";
            dtCurrentExposure = ourData.loadCashFlowForExposure(exposure);
            currentExposure = exposure;
            lblExpentry.Text = exposure;
        }

        public void buildNumberGrid(string defexpinro, string defexpoutro)
        {
            colPeriod.Width = 74d;

            dtNumbers = new DataTable();
            dtNumbers.Columns.Add("Period");
            dtNumbers.Columns.Add("Loss");
            //dtNumbers.Columns.Add("DefExp");
            dtNumbers.Columns.Add("DefExpIn");
            dtNumbers.Columns.Add("DefExpOut");

            // ADDED BACK IN JUNE 6 2018
            dtNumbers.Columns.Add("DefExp");

            dtNumbers.Columns.Add("CovDJ");
            dtNumbers.Columns.Add("Total");
            dtNumbers.Columns.Add("LDCColor");
            dtNumbers.Columns.Add("LossRO");
            dtNumbers.Columns.Add("DefExpInColor");
            dtNumbers.Columns.Add("DefExpInRO");
            dtNumbers.Columns.Add("DefExpOutColor");
            dtNumbers.Columns.Add("DefExpOutRO");


            // APRIL 10 2018
            //dtNumbers.Columns.Add("DefExpIn");
            //dtNumbers.Columns.Add("DefExpOut");

            foreach (period p in periods)
            {
                string color = "Transparent";
                string ro = "False";

                //if (p.total)
                //    rowheader = (p.year == 0) ? "Total" : p.year.ToString();
                //else
                //    rowheader = (p.quarter == 0) ? p.year.ToString() : p.year.ToString() + "Q" + p.quarter.ToString();

                string incolor = (defexpinro == "True") ? "LightGray" : "Transparent";
                string outcolor = (defexpoutro == "True") ? "LightGray" : "Transparent";

                if (p.readOnly || p.total)
                {
                    incolor = outcolor = color = "LightGray";
                    ro = "True";
                    defexpinro = defexpoutro = "True";
                }

                // WAS
                //dtNumbers.Rows.Add(p.header, "", "", "", "", "", color, ro, incolor, defexpinro, outcolor, defexpoutro); // MAY 6 remove DefEx, add InOut // APRIL 10 2018 ADDED "" "" to the end

                // JUNE 6 2018
                dtNumbers.Rows.Add(p.header, "", "", "", "", "", "", color, ro, incolor, defexpinro, outcolor, defexpoutro); // MAY 6 remove DefEx, add InOut // APRIL 10 2018 ADDED "" "" to the end
            }

            dgNumbers.ItemsSource = dtNumbers.DefaultView;

            // DataTable dtNumbersCopy = dtNumbers.Copy();

            if (dtEmptyNumbers == null)
                dtEmptyNumbers = dtNumbers.Copy();

            exposure.dtEmptyNumbersWMforSplit = dtEmptyNumbers.Copy();
            exposure.dgNumbers.ItemsSource = exposure.dtEmptyNumbersWMforSplit.DefaultView;
        }

        public void clearExposuresWMSplitGrid()
        {
            exposure.dtEmptyNumbersWMforSplit = dtEmptyNumbers.Copy();
            exposure.dgNumbers.ItemsSource = exposure.dtEmptyNumbersWMforSplit.DefaultView;
        }

        public void populateNumberGrid()
        {
            // May 15, 2018
            // string inOrOut = "";
            // if (dtCurrentExposure.Rows.Count == 1)
            //    inOrOut = (dtCurrentExposure.Rows[0]["WithinLimits"] == DBNull.Value) ? "" : dtCurrentExposure.Rows[0]["WithinLimits"].ToString();

            foreach (DataRow dr in dtCurrentExposure.Rows)
            {
                string year = dr["Year"].ToString();
                string qtr = dr["Quarter"].ToString();

                DataRow drng = findDataRowForYearQuarter(year, qtr);

                // Nov 13 2017 - test for null
                if (drng != null)
                {
                    string valuename = dr["ValueName"].ToString();

                    // Don't process orhpan values
                    // JUNE 6 2018 - DO PROCESS
//                    if (valuename != "DefExp")
                    {
                        double current = formattedTextToDouble(drng[valuename].ToString());
                        double amount = Convert.ToDouble(dr["Amount"].ToString());

                        drng[valuename] = (current + amount).ToString("C0");
                    }
                }
            }

            dtNumbersBeforeEdits = dtNumbers.Copy();
            // Aug 7 2017 soClean();
        }


        // JUNE 6 2018 
        // Does the current sprint have values in the CashFlowEntry table that are DefExp not DefExpIn or DefExpOut
        public bool currentExposureHasUnknownDefExp()
        {
            foreach (DataRow dr in dtCurrentExposure.Rows)
                if (dr["ValueName"].ToString() == "DefExp")
                    return true;

            return false;
        }

        // JUNE 11 2018 
        // Does the current sprint have values in the CashFlowEntry table that are DefExp not DefExpIn or DefExpOut
        public bool anyExposureHasUnknownDefExp()
        {
            string sql = "SELECT top 1 w.[WorkMatter],[Exposure],w.[PolicyNumber],[Year],[Quarter],[ValueName],[Amount],[StartUser],[StartTime],[EndUser],[EndTime] ";
            sql += "FROM [CashFlow].[data].[CashFlowEntry] w ";
            sql += "LEFT JOIN [CashFlow].[data].[Exposures] e on (e.WorkMatter=w.WorkMatter) and (e.ExpID=w.Exposure) ";
            sql += "WHERE (w.[WorkMatter] = '" + currentWorkMatter + "') and (w.WorkMatter != w.Exposure) and (EndTime is null) and (Amount != 0) and e.ExpClosed=0 and ValueName='DefExp'";
            DataTable dtCFTest = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            return (dtCFTest.Rows.Count > 0);
        }


        // JUNE 6 2018
        public void popOpenDefExpUnknownCoumn(bool unknownOpen)
        {
            if (unknownOpen)
            {
                gridWMEntry.Width = 642;
                lblNotifications.Margin = new Thickness(515, 625, 0, 0);
                rectNotifications.Margin = new Thickness(709, 670, 0, 0);
                ovalNotifications.Margin = new Thickness(637, 733, 0, 0);
                columnUnknown.Visibility = Visibility.Visible;
                rect1.Margin = new Thickness(642, 0, 0, 0);
                gridClaimInfo.Margin = new Thickness(647, 0, 0, 0);

                txtRWCTotal.Width = 227;
                cbConfidence.Width = 229;
                txtComments.Width = 620;
                lblCommentCounter.Margin = new Thickness(532, 120, 0, 0);
                gridFilterWMExp.Width = 647;
            }
            else
            {
                gridWMEntry.Width = 542;
                lblNotifications.Margin = new Thickness(415, 625, 0, 0);
                rectNotifications.Margin = new Thickness(609, 670, 0, 0);
                ovalNotifications.Margin = new Thickness(537, 733, 0, 0);
                columnUnknown.Visibility = Visibility.Collapsed;
                rect1.Margin = new Thickness(542, 0, 0, 0);
                gridClaimInfo.Margin = new Thickness(547, 0, 0, 0);

                txtRWCTotal.Width = 127;
                cbConfidence.Width = 129;
                txtComments.Width = 520;
                lblCommentCounter.Margin = new Thickness(432, 120, 0, 0);
                gridFilterWMExp.Width = 547;
            }
        }

        public void populateNumberGridForSingleExposure()
        {
            foreach (DataRow dr in dtCurrentExposure.Rows)
            {
                //                if (dr[""]
                string year = dr["Year"].ToString();
                string qtr = dr["Quarter"].ToString();

                DataRow drng = findDataRowForYearQuarter(year, qtr);

                string valuename = dr["ValueName"].ToString();
                double current = formattedTextToDouble(drng[valuename].ToString());
                double amount = Convert.ToDouble(dr["Amount"].ToString());

                drng[valuename] = (current + amount).ToString("C0");
            }

            dtNumbersBeforeEdits = dtNumbers.Copy();
            soClean();
        }

        public DataRow findDataRowForYearQuarter(string year, string qtr)
        {
            foreach (DataRow dr in dtNumbers.Rows)
                if (dr["Period"].ToString() == year + "Q" + qtr)
                    return dr;
            foreach (DataRow dr in dtNumbers.Rows)
                if (dr["Period"].ToString().Replace("+", "") == year)
                    return dr;
            return null;
        }

        public double formattedTextToDouble(string text)
        {
            text = moneyToNormal(text);

            if (text == "")
                return 0d;

            double val = 0d;
            Double.TryParse(text, out val);
            return val;
        }

        public void saveNumberChanges()
        {
            currentFunction = "Save number changes";

            ////// STEP 1 - Generate backing table //////
            DataTable dtBackingTable = ourData.generateCFBackingTable();

            string dateTime = DateTime.Now.ToString();

            // Compare old and new grid
            foreach (DataRow drold in dtNumbersBeforeEdits.Rows)
            {
                if (drold["LossRO"].ToString() == "False")
                {
                    foreach (DataRow drnew in dtNumbers.Rows)
                    {
                        handleDifference(ref dtBackingTable, dateTime, "Loss", drold, drnew);
                        //handleDifference(ref dtBackingTable, dateTime, "DefExp", drold, drnew);
                        handleDifference(ref dtBackingTable, dateTime, "DefExpIn", drold, drnew);
                        handleDifference(ref dtBackingTable, dateTime, "DefExpOut", drold, drnew);
                        handleDifference(ref dtBackingTable, dateTime, "CovDJ", drold, drnew);
                    }
                }
            }

            ourData.updateCFValues(dtBackingTable);
        }

        private void saveStatus()
        {
            currentFunction = "Save status";

            if (dgWM.SelectedItems.Count == 0)
            {
                MessageBox.Show("You do not have a WorkMatter selected");
                return;
            }

            string status = ((DataRowView)dgWM.SelectedItem)["Status"].ToString();
            string wm = ((DataRowView)dgWM.SelectedItem)["WorkMatter"].ToString();

            adjustStatus(DateTime.Now.ToString(), wm, status, "");
            return;
        }

        private void handleDifference(ref DataTable dtBackingTable, string dateTime, string valuename, DataRow drold, DataRow drnew)
        {
            if (drold["Period"].ToString() == drnew["Period"].ToString())
            {
                string oldLoss = drold[valuename].ToString();
                string newLoss = drnew[valuename].ToString();

                if (oldLoss != newLoss)
                {
                    string period = drold["Period"].ToString();
                    string year = period.Substring(0, 4);

                    ////// YEARLY //////
                    if (period.IndexOf("Q") < 0)
                    {
                        double yearly = formattedTextToDouble(newLoss);

                        dtBackingTable.Rows.Add(currentWorkMatter, currentExposure, "POL1", year, "1", valuename, yearly / 4d, "X");
                        dtBackingTable.Rows.Add(currentWorkMatter, currentExposure, "POL1", year, "2", valuename, yearly / 4d, "X");
                        dtBackingTable.Rows.Add(currentWorkMatter, currentExposure, "POL1", year, "3", valuename, yearly / 4d, "X");
                        dtBackingTable.Rows.Add(currentWorkMatter, currentExposure, "POL1", year, "4", valuename, yearly / 4d, "X");
                    }

                    ////// QUARTERLY //////
                    else
                    {
                        string quarter = period.Substring(5, 1);
                        dtBackingTable.Rows.Add(currentWorkMatter, currentExposure, "POL1", year, quarter, valuename, formattedTextToDouble(newLoss), "X");
                    }
                }
            }
        }

        public void soClean()
        {
            //borderSave.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x68, 0x7A, 0x93));
            borderSave.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 147, 141, 137));
        }

        public void soDirty()
        {
            borderSave.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x2A, 0xDA, 0x7A));
        }

        public bool isDirty()
        {
            if (((SolidColorBrush)borderSave.BorderBrush).Color == Color.FromArgb(0xff, 0x2A, 0xDA, 0x7A))
                return true;
            return false;
        }

        #endregion exposure

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Save";

            if (dgWM.SelectedItems.Count == 0)
            {
                MessageBox.Show("You do not have a WorkMatter selected");
                return;
            }

            if (lblError.Text != "")
            {
                MessageBox.Show("You cannot save until the invalid number is fixed");
                return;
            }

            if (currentWorkMatter == currentSpecialWorkMatter)
            {
                MessageBox.Show("You cannot make changes to this Work Matter");
                return;
            }

            saveAction();

            // SO HERE I THINK WE NEED TO RE-SELECT THE EXPOSURE?
        }

        public void saveAction()
        {
            if (dgWM.SelectedItems.Count == 0)
                return;

            if (lblError.Text != "")
                return;

            string cwm = currentWorkMatter;
            string cexp = currentExposure;

            if (currentWorkMatter == currentSpecialWorkMatter)
                return;

            ////// CLAIMS //////
            if (uiCurrentUser.dept.ToUpper() == "CLAIMS")
            {
                if (ourData.getadidFromWM(cwm) == uiCurrentUser.adid)
                    adjustStatusForSelectedItems(PENDING, "", forcestatus: true);

                else if ((ourData.isUserUnitLead(uiCurrentUser.adid) == false) && (ourData.isUserTeamLead(uiCurrentUser.adid) == false))
                    adjustStatusForSelectedItems(PENDING, "", forcestatus: true);
            }

            ////// OGC //////
            else
            {
                ////// if the WM is assigned to us and we save it, make it pending //////
                if (ourData.getadidFromWM(cwm) == uiCurrentUser.adid)
                    adjustStatusForSelectedItems(PENDING, "", forcestatus: true);

                ////// The logic is the same as for claims but sepearted it for when it isn't //////
                else if ((ourData.isUserUnitLead(uiCurrentUser.adid) == false) && (ourData.isUserTeamLead(uiCurrentUser.adid) == false))
                    adjustStatusForSelectedItems(PENDING, "", forcestatus: true);
            }

            currentWorkMatter = cwm;
            currentExposure = cexp;
            selectWMinGrid(currentWorkMatter);

            currentWorkMatter = cwm;
            currentExposure = cexp;

            soClean();

            if (currentExposure == "")
            {
                // MessageBox.Show("Error saving exposure - Current exposure is set to blank");
            }
            else
                saveNumberChanges();

            saveStatus();

            currentWorkMatter = cwm;
            currentExposure = cexp;
            ourData.rollupExposuresCFIntoWorkMatter(currentWorkMatter);
            fillWM();

            selectWMinGrid(currentWorkMatter);

            flashSaved();

            SoundPlayer player = new SoundPlayer(Properties.Resources.SaveSound);
            player.Load();
            player.Play();
        }

        private void cbBasis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            soDirty();
        }

        private void txtComments_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCommentCounter.Text = (txtComments.Text.Length == 0) ? "" : txtComments.Text.Length.ToString() + " / 1000";
            soDirty();
        }

        private void calcRWCTotal()
        {
            double total = 0d;

            double rwcLoss = 0d;
            double rwcDefExp = 0d;
            double rwcCovDJ = 0d;

            double.TryParse(moneyToNormal(txtRWCLoss.Text), out rwcLoss);
            double.TryParse(moneyToNormal(txtRWCDefExp.Text), out rwcDefExp);
            double.TryParse(moneyToNormal(txtRWCCovDJ.Text), out rwcCovDJ);

            txtRWCTotal.Text = (rwcLoss + rwcDefExp + rwcCovDJ).ToString("C0");

            soDirty();
        }

        private void cbConfidence_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            soDirty();
        }

        private void btnSpread_Click(object sender, RoutedEventArgs e)
        {
            if (currentWorkMatter == "")
            {
                MessageBox.Show("There is no Work Matter selected");
                return;
            }
            if (currentWorkMatter == currentSpecialWorkMatter)
            {
                MessageBox.Show("You cannot allocate on this Work Matter");
                return;
            }

            processSpreadButton(false);
        }

        private void processSpreadButton(bool thenSave)
        {
            if (btnSpreadText.Text == "Allocate")
            {
                btnSpreadText.Text = "Cancel";
                borderSaveExposureSplit.Visibility = Visibility.Visible;
                clearExposuresWMSplitGrid();
                // borderSplit.Visibility = Visibility.Visible;
            }
            else
            {
                btnSpreadText.Text = "Allocate";
                borderSaveExposureSplit.Visibility = Visibility.Collapsed;
                // borderSplit.Visibility = Visibility.Collapsed;
            }

            exposure.blowOpen(thenSave);
        }

        public void showOrObscureForAllocations()
        {
            if (btnSpreadText.Text == "Allocate")
            {
                gridFilterTop.Visibility = Visibility.Collapsed;
                gridFilterWMExp.Visibility = Visibility.Collapsed;
            }
            else
            {
                gridFilterTop.Visibility = Visibility.Visible;
                gridFilterWMExp.Visibility = Visibility.Visible;
            }
        }

        private void btnSplit_Click(object sender, RoutedEventArgs e)
        {
            btnSplitText.Text = (btnSplitText.Text == "Split") ? "Combine" : "Split";
            exposure.showSplit();
        }

        private void btnSaveExposureSplit_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Save Exposure Split";

            // Fast fail on all sliders set to 0
            if (exposure.getTotalSliderValues() == 0d)
            {
                MessageBox.Show("You cannot save if all the sliders are set to 0%");
                return;
            }

            ////// STEP 1 - CREATE A BACKING TABLE TO SEND TO RSSE //////
            DataTable dtCashFlowEntry = new DataTable();
            dtCashFlowEntry.Columns.Add("WorkMatter");
            dtCashFlowEntry.Columns.Add("Exposure");
            dtCashFlowEntry.Columns.Add("PolicyNumber");
            dtCashFlowEntry.Columns.Add("Year", typeof(int));
            dtCashFlowEntry.Columns.Add("Quarter", typeof(int));
            dtCashFlowEntry.Columns.Add("ValueName");
            dtCashFlowEntry.Columns.Add("Amount", typeof(decimal));
            dtCashFlowEntry.Columns.Add("Operation");
            dtCashFlowEntry.TableName = "CashFlowEntry";

            //dtCashFlowEntry.Rows.Add("WM1", "EXP1", "POL1", 2017, 1, "Loss", 1000, "X");
            //DataTable dtCrap = ourData.se.runzScript(dtCashFlowEntry, "CASH_CRUD_CASHFLOWENTRY", "OPSCONSOLE" );

            ////// STEP 2 - GO THROUGH EACH EXPOSURE //////
            ////// CLA-498 - ONLY SELECTED EXPOSURES !!!!!
            // foreach (System.Data.DataRowView drv in exposure.dgExposures.ItemsSource)
            foreach (System.Data.DataRowView drv in exposure.dgExposures.SelectedItems)
            {
                    string exp = drv["ExpID"].ToString();
                string portfolio = drv["Portfolio"].ToString();
                string expenseTreatment = drv["WithinLimits"].ToString();

                ////// STEP 3 - FIND EACH NUMBER IN THE WORKMATTER GRID //////
                foreach (System.Data.DataRowView drvg in exposure.dgNumbers.ItemsSource)
                {
                    if (drvg["LossRO"].ToString() == "False")
                    {
                        ProcessValue(portfolio, exp, dtCashFlowEntry, "Loss", "Loss", drvg);
                        //ProcessValue(portfolio, exp, dtCashFlowEntry, "DefExp", drvg);
                        if (expenseTreatment == "Y")
                            ProcessValue(portfolio, exp, dtCashFlowEntry, "DefExpIn", "DefExpIn", drvg);
                        else
                        {
                            ProcessValue(portfolio, exp, dtCashFlowEntry, "DefExpIn", "DefExpOut", drvg);
                        }
                        ProcessValue(portfolio, exp, dtCashFlowEntry, "CovDJ", "CovDJ", drvg);
                    }
                }
            }

            if (dtCashFlowEntry.Rows.Count == 0)
            {
                MessageBox.Show("You must enter at least one number to allocate over");
                return;
            }

            ////// STEP 3 - WRITE IT OUT //////
            ourData.updateCFValues(dtCashFlowEntry);

            ////// STEP 4 - CLOSE THE WINDOW //////
            processSpreadButton(true);

            //flashSaved();
        }

        private void ProcessValue(string portfolio, string exp, DataTable dtCashFlowEntry, string value, string valueSaveAs, System.Data.DataRowView drvg)
        {
            string nval = drvg[value].ToString();

            ////// Fast fail on blanks //////
            if (nval == "")
                return;

            ////// Fast fail on can't parse number //////
            double val = 0;

            if (double.TryParse(moneyToNormal(nval), out val) == false)
                return;

            ////// Determine count //////
            double count = exposure.dicFound[portfolio];

            ////// Determine percentage //////
            double perct = exposure.getSliderValeByPortfolio(portfolio);

            string year = "";
            string quarter = "";
            ParsePeriodName(drvg["Period"].ToString(), ref year, ref quarter);

            if (quarter == "")
            {
                dtCashFlowEntry.Rows.Add(currentWorkMatter, exp, "POL1", year, 1, valueSaveAs, ((val * perct) / count) / 4d, "X");
                dtCashFlowEntry.Rows.Add(currentWorkMatter, exp, "POL1", year, 2, valueSaveAs, ((val * perct) / count) / 4d, "X");
                dtCashFlowEntry.Rows.Add(currentWorkMatter, exp, "POL1", year, 3, valueSaveAs, ((val * perct) / count) / 4d, "X");
                dtCashFlowEntry.Rows.Add(currentWorkMatter, exp, "POL1", year, 4, valueSaveAs, ((val * perct) / count) / 4d, "X");
            }
            else
                dtCashFlowEntry.Rows.Add(currentWorkMatter, exp, "POL1", year, quarter, valueSaveAs, (val * perct) / count, "X");
        }

        private void ParsePeriodName(string periodName, ref string year, ref string quarter)
        {
            string period = periodName;
            year = period.Substring(0, 4);

            if (period.IndexOf("Q") >= 0)
                quarter = period.Substring(5, 1);
            else
                quarter = "";
        }

        private void btnAllocateFromPortfolios_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnShowAll_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnShowAll, new Button[] { btnShowAll, btnShowIncomplete, btnShowApproved, btnShowDenied });
            fillWM();
        }

        private void btnShowIncomplete_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnShowIncomplete, new Button[] { btnShowAll, btnShowIncomplete, btnShowApproved, btnShowDenied });
            fillWM();
        }

        private void btnShowApproved_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnShowApproved, new Button[] { btnShowAll, btnShowIncomplete, btnShowApproved, btnShowDenied });
            fillWM();
        }

        private void btnShowDenied_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnShowDenied, new Button[] { btnShowAll, btnShowIncomplete, btnShowApproved, btnShowDenied });
            fillWM();
        }

        private void goPink()
        {
            //imgBanner.Source = new BitmapImage(new Uri(@"Images/header_bg-pink.gif", UriKind.RelativeOrAbsolute));
            gridBlueBar.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xC2, 0x2F, 0xD1));
            rectShowAll.Fill = rectApproved.Fill = rectPending.Fill = rectDenied.Fill = new SolidColorBrush(Color.FromArgb(0xff, 0xC2, 0x2F, 0xD1));
            ovalPreviousCFA.Fill = ovalNotifications2.Fill = ovalExposure.Fill = new SolidColorBrush(Color.FromArgb(0xff, 0xC2, 0x2F, 0xD1));
            previousCFA.gridMain.Background =
            notification.gridMain.Background = exposure.gridMain.Background = history.gridMain.Background =
            gridFilter.Background = gridAnalystActions.Background = gridManagerActions.Background = gridAdditional.Background = gridMain.Background = gridWMEntry.Background = gridClaimInfo.Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x93, 0x68, 0x8B));
            rect1.Fill = rect2.Fill = gridSplitter.Background = gridTL.Background = new SolidColorBrush(Color.FromArgb(0xff, 179, 136, 172));

            // #FF93688B
        }

        private void goGold()
        {
            //imgBanner.Source = new BitmapImage(new Uri(@"Images/header_gold.gif", UriKind.RelativeOrAbsolute));
            gridBlueBar.Background = new SolidColorBrush(Color.FromArgb(0xff, 209, 140, 46));
            rectShowAll.Fill = rectApproved.Fill = rectPending.Fill = rectDenied.Fill = new SolidColorBrush(Color.FromArgb(0xff, 209,140,46));
            rectShowAll.Stroke = rectApproved.Stroke = rectPending.Stroke = rectDenied.Stroke = new SolidColorBrush(Color.FromArgb(0xff, 209, 140, 46));
            ovalPreviousCFA.Fill = ovalNotifications2.Fill = ovalExposure.Fill = new SolidColorBrush(Color.FromArgb(0xff, 209,140,46));
            previousCFA.gridMain.Background =
            notification.gridMain.Background = exposure.gridMain.Background = history.gridMain.Background =
            gridFilter.Background = gridAnalystActions.Background = gridManagerActions.Background = gridAdditional.Background = gridMain.Background = gridWMEntry.Background = gridClaimInfo.Background = new SolidColorBrush(Color.FromArgb(0xFF, 145,126,106));
            rect1.Fill = rect2.Fill = gridSplitter.Background = gridTL.Background = new SolidColorBrush(Color.FromArgb(0xff, 182, 170, 133));

            // #FF93688B
        }


        private void btnShowAssociations_Click(object sender, RoutedEventArgs e)
        {
            currentFunction = "Show associations";

            if (dgWM.SelectedIndex < 0)
                return;

            string wm = ((DataRowView)dgWM.SelectedItem)["WorkMatter"].ToString();
            string wmDesc = ((DataRowView)dgWM.SelectedItem)["WorkMatterDescription"].ToString();

            DataTable dtAssoc = ourData.getAssociatedWorkMatters(wm);

            if (dtAssoc.Rows.Count == 0)
            {
                MessageBox.Show("The row you have selected has no associated Work Matters");
                return;
            }

            string wmlist = ourData.DataTableToCommaSeparatedList(dtAssoc, "WorkMatter", "'");
            DataTable dtAssocWM = ourData.getWorkMattersFromWorkMatterList(wmlist);
            associated.lblTitle.Text = "Associated Work Matters to " + wm + " - " + wmDesc;
            associated.dgWM.ItemsSource = dtAssocWM.DefaultView;
            associated.btnView.Visibility = (uiCurrentUser.dept == "OGC") ? Visibility.Visible : Visibility.Collapsed;

            associated.Visibility = Visibility.Visible;
        }

        private void Image_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //if ((uiCurrentUser.name.ToLower().IndexOf("scott") >= 0) || (uiCurrentUser.name.ToLower().IndexOf("kim") >= 0))
            //    goPink();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            imgAngle.Angle += 5d;
            if (imgAngle.Angle >= 355d)
                imgAngle.Angle = 0d;
        }

        private void pleaseWait(bool wait)
        {
            if (wait)
            {
                dispatcherTimer.Start();
                imgWait.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                dispatcherTimer.Stop();
                imgWait.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        public bool selectWMinGrid(string workmatter)
        {
            int index = 0;
            foreach (DataRowView drv in dgWM.Items)
                if (drv["WorkMatter"].ToString() == workmatter)
                {
                    dgWM.SelectedIndex = index;
                }
                else
                    index++;
            return false;
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            handleGridKeysLikeExcel(cfeGrid.MainCFEG, ref e);
        }

        // This routine disallows symbols that are not 0 to 9, period, minus, k and m
        // And it simulates the UP, DOWN, LEFT and RIGHT of Excel
        public void handleGridKeysLikeExcel(cfeGrid cg, ref KeyEventArgs e)
        {
            if (e.Key == Key.Down)
            {
                int count;

                // Jun 6 2018 Extra
                if (cg == cfeGrid.MainCFEG)
                    count = (columnUnknown.Visibility == Visibility.Visible) ? 5 : 4;
                else
                    count = (exposure.dgtcDefExp.Visibility == Visibility.Visible) ? 3 : 2;

                for (int i=0; i<count; i++)
                {
                    var e0 = new KeyEventArgs(Keyboard.PrimaryDevice, Keyboard.PrimaryDevice.ActiveSource, 0, Key.Tab) { RoutedEvent = Keyboard.KeyDownEvent };
                    InputManager.Current.ProcessInput(e0);
                }
                return;
            }

            if (e.Key == Key.Up)
            {
                InputSimulator a = new InputSimulator();
                a.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);

                // Jun 6 2018 Extra
                int count;
                if (cg == cfeGrid.MainCFEG)
                    count = (columnUnknown.Visibility == Visibility.Visible) ? 5 : 4;
                else
                    count = (exposure.dgtcDefExp.Visibility == Visibility.Visible) ? 3 : 2;

                for (int i = 0; i < count; i++)
                    a.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
                // End June 6 2018

                a.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
            }

            if (e.Key == Key.Left)
            {
                InputSimulator a = new InputSimulator();
                a.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.SHIFT);
                a.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
                a.Keyboard.KeyUp(WindowsInput.Native.VirtualKeyCode.SHIFT);
            }

            if (e.Key == Key.Right)
            {
                InputSimulator a = new InputSimulator();
                a.Keyboard.KeyPress(WindowsInput.Native.VirtualKeyCode.TAB);
            }

            if ((e.Key == Key.LeftShift) || (e.Key == Key.RightShift))
                return;

            if ((Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift)) &&
                    ((e.Key == Key.D0) || (e.Key == Key.D1) || (e.Key == Key.D2) || (e.Key == Key.D3) || (e.Key == Key.D4) || (e.Key == Key.D5) || (e.Key == Key.D6) || (e.Key == Key.D7) || (e.Key == Key.D8) || (e.Key == Key.D9)))
            {
                e.Handled = true;
                return;
            }

            switch (e.Key)
            {
                case Key.OemPlus:
                case Key.OemMinus:
                case Key.Subtract:
                case Key.Oem1:
                case Key.Oem2:
                case Key.Oem3:
                case Key.Oem4:
                case Key.Oem5:
                case Key.Oem6:
                case Key.Oem7:
                case Key.A:
                case Key.B:
                case Key.C:
                case Key.D:
                case Key.E:
                case Key.F:
                case Key.G:
                case Key.H:
                case Key.I:
                case Key.J:
                case Key.L:
                case Key.N:
                case Key.O:
                case Key.P:
                case Key.Q:
                case Key.R:
                case Key.S:
                case Key.T:
                case Key.U:
                case Key.V:
                case Key.W:
                case Key.X:
                case Key.Y:
                case Key.Z:
                    e.Handled = true;
                    return;
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //            var h = ((Panel)Application.Current.MainWindow.Content).ActualHeight;
            //          var w = ((Panel)Application.Current.MainWindow.Content).ActualWidth;

            if ((ActualHeight < 817) || (ActualWidth < 1320))
            {
                gridTL.Height = 817; // - SystemParameters.WindowCaptionHeight - (SystemParameters.WindowResizeBorderThickness.Top + SystemParameters.WindowResizeBorderThickness.Bottom);
                gridTL.Width = 1320; // - (SystemParameters.WindowResizeBorderThickness.Left + SystemParameters.WindowResizeBorderThickness.Right);
                                     // BIG FIX 
                viewbox.Stretch = Stretch.Uniform;
            }
            else
            {
                // BIG FIX 
                viewbox.Stretch = Stretch.None;

                gridTL.Height = ActualHeight - SystemParameters.WindowCaptionHeight - (SystemParameters.WindowResizeBorderThickness.Top + SystemParameters.WindowResizeBorderThickness.Bottom) - 10d;
                gridTL.Width = ActualWidth - (SystemParameters.WindowResizeBorderThickness.Left + SystemParameters.WindowResizeBorderThickness.Right) - 10d;
            }

            return;

            if (Height < 750d)
                goSLRmode();
            else
                goNormal();
        }

        private void goSLRmode()
        {
            toolbar.Visibility = Visibility.Collapsed;
            gridBlueBar.Visibility = Visibility.Collapsed;
            gridFilter.Margin = new Thickness(5, 0, 0, 0);
            gridMain.Margin = new Thickness(5, 75, 5, 4);
            gridAnalystActions.Margin = new Thickness(807, 0, 0, 0);
            gridManagerActions.Margin = new Thickness(959, 0, 0, 0);
            gridAdditional.Margin = new Thickness(1125, 0, 5, 0);
            dgWM.FontSize = 11d;
            dgWM.Margin = new Thickness(9, 5, 56, 5);
            lblKey1.Visibility = lblKey2.Visibility = lblKey3.Visibility = lblKey4.Visibility = lblKey5.Visibility = lblKey6.Visibility = lblKey7.Visibility = lblKey8.Visibility = lblKey9.Visibility = imgKey1.Visibility = Visibility.Collapsed;
            colSTG.Visibility = Visibility.Collapsed;
        }

        private void goNormal()
        {
            toolbar.Visibility = Visibility.Visible;
            gridBlueBar.Visibility = Visibility.Visible;
            gridFilter.Margin = new Thickness(5, 60, 0, 0);
            gridMain.Margin = new Thickness(5, 135, 5, 4);
            gridAnalystActions.Margin = new Thickness(807, 60, 0, 0);
            gridManagerActions.Margin = new Thickness(959, 60, 0, 0);
            gridAdditional.Margin = new Thickness(1125, 60, 5, 0);
            dgWM.FontSize = 12d;
            dgWM.Margin = new Thickness(9, 11, 7, 32);
            lblKey1.Visibility = lblKey2.Visibility = lblKey3.Visibility = lblKey4.Visibility = lblKey5.Visibility = lblKey6.Visibility = lblKey7.Visibility = lblKey8.Visibility = lblKey9.Visibility = imgKey1.Visibility = Visibility.Visible;
            colSTG.Visibility = Visibility.Visible;
        }

        private void gridAdditional_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void flashSaved()
        {
            lblSaved.Opacity = 0d;
            lblSaved.Visibility = Visibility.Visible;

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0d;
            da.To = 1d;
            da.Duration = new Duration(TimeSpan.FromSeconds(.5d));

            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 1d;
            da2.To = 0d;
            da2.Duration = new Duration(TimeSpan.FromSeconds(.5d));

            ////// IF YOU DON'T KILL THE ANIMATION, YOU CAN'T SET THE OPACITY PROPERTY //////
            // da.Completed += (ss, ee) => { gridNewUser.BeginAnimation(OpacityProperty, null); };
            da.Completed += (ss, ee) =>
            {
                lblSaved.BeginAnimation(OpacityProperty, null);
                lblSaved.BeginAnimation(OpacityProperty, da2);
            };

            da2.Completed += (ss, ee) =>
            {
                lblSaved.Visibility = Visibility.Collapsed;
                lblSaved.BeginAnimation(OpacityProperty, null);
            };

            ////// START ANIMATION //////
            lblSaved.BeginAnimation(OpacityProperty, da);
        }

        private void gridAdditional_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            // original if ((lblEnvironment.Text != "Production") || (originalUserWasAdmin))
            if ((lblEnvironment.Text != "Production"))
                switchUser();
        }

        private void txtRWCLoss_PreviewKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void txtRWCDefExp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
        }

        private void txtRWCLoss_TextChanged(object sender, TextChangedEventArgs e)
        {
            calcRWCTotal();
        }

        private void txtRWCDefExp_TextChanged(object sender, TextChangedEventArgs e)
        {
            calcRWCTotal();
        }

        private void txtRWCCovDJ_TextChanged(object sender, TextChangedEventArgs e)
        {
            calcRWCTotal();
        }

        private string convertNumber(string input, out bool ok)
        {
            if (input == "")
            {
                ok = true;
                return "";
            }

            int ival = 0;
            if (Int32.TryParse(moneyToNormal(input), out ival))
            {
                ok = true;
                return ival.ToString("C0");
            }
            else
            {
                ok = false;
                return input;
            }
        }

        private void txtRWCLoss_LostFocus(object sender, RoutedEventArgs e)
        {
            bool worked = false;
            txtRWCLoss.Text = convertNumber(txtRWCLoss.Text, out worked);
        }

        private void txtRWCDefExp_LostFocus(object sender, RoutedEventArgs e)
        {
            bool worked = false;
            txtRWCDefExp.Text = convertNumber(txtRWCDefExp.Text, out worked);
        }

        private void txtRWCCovDJ_LostFocus(object sender, RoutedEventArgs e)
        {
            bool worked = false;
            txtRWCCovDJ.Text = convertNumber(txtRWCCovDJ.Text, out worked);
        }

        private void btnUserGuide_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"\\mansan02\fdrive$\BUILD\Production\CashFlow\" + "Cash Flow Application User Guide.docx");
        }

        private void btnVideo_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"\\mansan02\fdrive$\BUILD\Production\CashFlow\" + "CashFlow.mp4");
        }

        private void btnReportBug_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This feature is coming in the next version. For now please send e-mail to ServiceDesk. Thank you.");
        }

        private void cbSpecialTrackingGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ignoreSTGSelectionChanges == false)
                fillWM();
        }

        private void btnDashboard_Click(object sender, RoutedEventArgs e)
        {
            dashboard.Visibility = Visibility.Visible;
        }

        private void btnShowDelta_Click(object sender, RoutedEventArgs e)
        {
            if (isCheckboxChecked(btnShowDelta))
                ShowDeltas(false);
            else
                ShowDeltas(true);
        }

        private void ShowDeltas(bool show)
        {
            setCheckbox(btnShowDelta, show);

            exposure.colQ1PL.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;
            exposure.colQ1DE.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;
            exposure.colQ1DJ.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;
            exposure.colQ1TOT.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;
            exposure.colQ2PL.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;
            exposure.colQ2DE.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;
            exposure.colQ2DJ.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;
            exposure.colQ2TOT.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;

            exposure.colQ1PLDelta.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            exposure.colQ1DEDelta.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            exposure.colQ1DJDelta.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            exposure.colQ1TOTDelta.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            exposure.colQ2PLDelta.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            exposure.colQ2DEDelta.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            exposure.colQ2DJDelta.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            exposure.colQ2TOTDelta.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;

        }

        private void btnShowTotals_Click(object sender, RoutedEventArgs e)
        {
            if (isCheckboxChecked(btnShowTotals))
                ShowTotals(false);
            else
                ShowTotals(true);

        }

        private void ShowTotals(bool show)
        {
            setCheckbox(btnShowTotals, show);

            if (show)
                exposure.dgExposures.SelectAll();
            else
            {
                exposure.dgExposures.UnselectAll();

                // HELP DON'T KNOW TEMP HELP
                buildNumberGrid("True","True");
                soClean();
                lblExpentry.Text = "";
                enableCashFlowEntry(false);
            }
        }

        private void btnRecall_Click(object sender, RoutedEventArgs e)
        {
            ////// If the Analyst is from OGC set to PENDING /////
            if (uiCurrentUser.dept == "OGC")
                adjustStatusForSelectedItems(PENDING, "Recalled by OGC Analyst", forcestatus: true);

            ////// Otherwise, move the selected item to STAGE 1 approval //////
            else
            {
                if (ourData.isUserUnitLead(uiCurrentUser.adid))
                    adjustStatusForSelectedItems(SUBMITTED, "Recalled by Claims UM", forcestatus: true);
                else
                    adjustStatusForSelectedItems(PENDING, "Recalled by Claims Analyst", forcestatus: true);
            }

        }

        private void btnCalendar_Click(object sender, RoutedEventArgs e)
        {
            calendar.Visibility = Visibility.Visible;
            calendar.initialize();
        }
    }
}