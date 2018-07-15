using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
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
    public partial class MainWindow : Window
    {
        #region constants
        private const string DEMO = "DEMO";
        private const string NAV = "NAV";
        private const string NAVSERV = "NAVSERV";
        private const string ADMIN = "ADMIN";
        private const string AG = "AG";
        private const string AGREPORTS = "AGREPORTS";
        private const string AGNOTES = "AGNOTES";
        private const string AGDEPT = "AGDEPT";
        private const string RSSEMIG = "RSSEMIG";
        private const string ROCNOTIFY = "ROCNOTIFY";
        private const string SSIS = "SSIS";
        private const string TIMETRACKER = "TIMETRACKER";
        private const string STARS = "STARS";
        private const string WOCO = "WOCO";
        private const string SERVCAT = "SERVCAT";
        private const string SDTICKET = "SDTICKET";
        private const string APVENDORMAP = "APVENDORMAP";
        private const string APHOLD = "APHOLD";
        private const string RESOURCE = "RESOURCE";
        private const int FUNCTION_GROUP_WIDTH = 315;
        #endregion

        #region class variables
        public static MainWindow ourMainWindow = null;
        public static string currentUser = "";
        #endregion

        #region UI Objects
        public class presentationButton
        {
            public string Line1 { set; get; }
            public string Line2 { set; get; }
            public string function { set; get; }
        }

        public class presentationGroup
        {
            public string image { set; get; }
            public string description { set; get; }
            public string ID { set; get; }
            public string Order { set; get; }
            public List<presentationButton> buttons = new List<presentationButton>();
        }

        public List<presentationGroup> presentationGroups = new List<presentationGroup>();
        #endregion

        #region MainWindow
        public MainWindow()
        {
            // Initialize windows
            InitializeComponent();

            // Get logged in user
            currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            lblCurrentUser.Text = currentUser;

            // Determine if we are DEV, TEST or PROD
            setupDev();
            setupTest();
            setupProd();

            // Load the UI (groups and buttons) data
            loadData();

            // Keep track of the one main window
            ourMainWindow = this;

            // Create a function group and buttons on the screen
            createFunctionGroupsAndButtons();
        }
        #endregion

        #region DEV / TEST / PROD
        [Conditional("DEV")]
        private void setupDev()
        {
            lblEnvironment.Text = "DEV";
            lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 211, 211, 211));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.DEV;
        }

        [Conditional("TEST")]
        private void setupTest()
        {
            lblEnvironment.Text = "TEST";
            lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x99, 0x99, 0x99));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.TEST;
        }

        [Conditional("PROD")]
        private void setupProd()
        {
            lblEnvironment.Text = "Production";
            lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xD8, 0x85, 0x20));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.PROD;
        }
        #endregion

        #region UI setup
        // Temporary
        // This will end up replaced with a script engine call
        public void loadData()
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "CRUD_CONFIGURATION", "OPSCONSOLE");
            DataTable dtPresentationGroup = ds.Tables["PresentationGroups"];
            DataTable dtPresentationGroupButton = ds.Tables["PresentationGroupButtons"];

            if (lblEnvironment.Text == "Production")
            {
                dtPresentationGroupButton.Rows.Add("27", "4", "Automated Reminders", "", "SDTICKET", "8", "Y");
            }

            DataTable dtUserPrivs = ds.Tables["UserPermissions"];

            DataView dv = dtPresentationGroupButton.DefaultView;
            dv.Sort = "Order";
            dtPresentationGroupButton = dv.ToTable();

            if ((dtPresentationGroup == null) || (dtPresentationGroupButton == null))
            {
                MessageBox.Show("Cannot load the UI components via the script engine");
                return;
            }

            foreach (DataRow drPG in dtPresentationGroup.Rows)
            {
                bool showGroup = false;
                foreach (DataRow drPGB in dtPresentationGroupButton.Rows)
                    if (drPGB["PresentationGroupID"].ToString() == drPG["ID"].ToString())
                        if (getPermissions(currentUser.ToLower(), drPGB["Function"].ToString(), dtUserPrivs) != "")
                            showGroup = true;

                if (showGroup)
                {
                    presentationGroup pg = new presentationGroup();
                    pg.image = drPG["Image"].ToString();
                    pg.description = drPG["Description"].ToString();
                    pg.ID = drPG["ID"].ToString();
                    pg.Order = drPG["Order"].ToString();

                    foreach (DataRow drPGB in dtPresentationGroupButton.Rows)
                    {
                        if (drPGB["PresentationGroupID"].ToString() == drPG["ID"].ToString())
                        {
                            string perm = getPermissions(currentUser.ToLower(), drPGB["Function"].ToString(), dtUserPrivs);
                            if (perm != "")
                            {
                                pg.buttons.Add(new presentationButton()
                                {
                                    Line1 = drPGB["Line1Text"].ToString(),
                                    Line2 = drPGB["Line2Text"].ToString(),
                                    function = drPGB["Function"].ToString()
                                });
                            }
                        }
                    }

                    presentationGroups.Add(pg);
                }
            }

            //try
            //{
            //    string connectionString = "Integrated Security=SSPI;Initial Catalog=OpsConsole;Data Source=MANTESTBS01";

            //    using (SqlConnection connection = new SqlConnection(connectionString))
            //    using (SqlCommand command = new SqlCommand("select * from PresentationGroup order by [order]", connection))
            //    {
            //        connection.Open();
            //        using (SqlDataReader reader = command.ExecuteReader())
            //        {
            //            while (reader.Read())
            //            {
            //                presentationGroup pg = new presentationGroup();
            //                pg.image = readerString(reader, "Image");
            //                pg.description = readerString(reader, "Description");
            //                pg.ID = readerString(reader, "ID");
            //                pg.Order = readerString(reader, "Order");

            //                using (SqlConnection connection2 = new SqlConnection(connectionString))
            //                using (SqlCommand command2 = new SqlCommand("select * from PresentationGroupButton where PresentationGroupID=" + readerString(reader, "ID"), connection2))
            //                {
            //                    connection2.Open();
            //                    using (SqlDataReader reader2 = command2.ExecuteReader())
            //                    {
            //                        while (reader2.Read())
            //                        {
            //                            pg.buttons.Add(new presentationButton()
            //                            {
            //                                Line1 = readerString(reader2, "Line1Text"),
            //                                Line2 = readerString(reader2, "Line2Text"),
            //                                function = readerString(reader2, "Function")
            //                            });
            //                        }

            //                        presentationGroups.Add(pg);
            //                    }
            //                }



            //            }
            //        }
            //    }

            //}
            //catch (Exception)
            //{
            //    throw;
            //}
        }

        private void createFunctionGroupsAndButtons()
        {
            // Create a function group and buttons on the screen
            foreach (presentationGroup pg in presentationGroups)
            {
                FunctionGroup fg = new FunctionGroup();
                fg.setMainWindow(this);
                fg.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                fg.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                fg.Height = 100d;
                fg.Width = FUNCTION_GROUP_WIDTH;
                fg.Padding = new Thickness(6, 6, 6, 6);
                fg.setText(pg.description);
                fg.setImage(pg.image);
                wpWrapPanel.Children.Add(fg);
                foreach (presentationButton pb in pg.buttons)
                {
                    if (pb.Line1.StartsWith("-") == false)
                        fg.addButton(pb.Line1, pb.Line2.Trim(), pb.function);
                }
            }
        }
        #endregion

        #region UI processing
        public void processCommand(string command)
        {
            string perm = getPermissions(currentUser.ToLower(), command);
            if (perm == "")
            {
                MessageBox.Show("You do not have access to this feature. Please submit a ticket asking for permission.");
                return;
            }

            switch (command)
            {
                case ADMIN:
                    closeAllScreens();
                    screenAdmin.Load(perm);
                    screenAdmin.Visibility = System.Windows.Visibility.Visible;
                    lblScreenName.Text = "Administration";
                break;

                case AG:
                    closeAllScreens();
                    screenAG.Load(perm);
                    screenAG.Visibility = System.Windows.Visibility.Visible;
                    lblScreenName.Text = "Authority Guidelines";
                    break;

                case AGREPORTS:
                    closeAllScreens();
                    screenEditTable.setup("AGREPORTS");
                    screenEditTable.Visibility = System.Windows.Visibility.Visible;
                    lblScreenName.Text = "Authority Guidelines Report Configuration";
                    break;

                case ROCNOTIFY:
                    closeAllScreens();
                    screenEditTable.setup("ROCNOTIFY");
                    screenEditTable.Visibility = System.Windows.Visibility.Visible;
                    lblScreenName.Text = "ROC Job Notification";
                    break;

                case AGNOTES:
                    closeAllScreens();
                    screenEditTable.setup("AGNOTES");
                    screenEditTable.Visibility = System.Windows.Visibility.Visible;
                    lblScreenName.Text = "Authority Guidelines Notes Configuration";
                    break;

                case AGDEPT:
                    closeAllScreens();
                    screenEditTable.setup("AGDEPT");
                    screenEditTable.Visibility = System.Windows.Visibility.Visible;
                    lblScreenName.Text = "Authority Guidelines Edit Departments";
                    break;

                case RSSEMIG:
                    closeAllScreens();
                    screenRSSEMig.Visibility = System.Windows.Visibility.Visible;
                    screenRSSEMig.InitialLoad();
                    lblScreenName.Text = "RSSE Migration";
                    break;

                case NAV:
                    closeAllScreens();
                    screenNavision.Visibility = System.Windows.Visibility.Visible;
                    screenNavision.Load();
                    lblScreenName.Text = "Navision Environment Management";
                    break;

                case NAVSERV:
                    closeAllScreens();
                    screenNAVSERV.Visibility = System.Windows.Visibility.Visible;
                    screenNAVSERV.Load();
                    lblScreenName.Text = "Navision Service Control";

                    break;

                case SSIS:
                    closeAllScreens();
                    screenSSIS.Visibility = System.Windows.Visibility.Visible;
                    screenSSIS.InitialLoad();
                    lblScreenName.Text = "SSIS";
                    break;

                case TIMETRACKER:
                    closeAllScreens();
                    screenSSIS.Visibility = System.Windows.Visibility.Visible;
                    // screenSSIS.InitialLoad();
                    lblScreenName.Text = "Time Tracker";
                    break;

                case STARS:
                    closeAllScreens();
                    screenStars.Visibility = System.Windows.Visibility.Visible;
                    // screenSSIS.InitialLoad();
                    lblScreenName.Text = "STARS State Extract";
                    break;

                case WOCO:
                    closeAllScreens();
                    screenWOCO.Visibility = System.Windows.Visibility.Visible;
                    // screenSSIS.InitialLoad();
                    lblScreenName.Text = "WOCO Claim Extract";
                    break;

                case SERVCAT:
                    closeAllScreens();
                    screenServiceCatalog.Visibility = System.Windows.Visibility.Visible;
                    screenServiceCatalog.Load();
                    lblScreenName.Text = "Service Catalog Configuration";
                    break;

                case SDTICKET:
                    closeAllScreens();
                    //screenEditTable.setup("SDAUTOMATE");
                    screenReminder.Visibility = System.Windows.Visibility.Visible;
                    screenReminder.Load(perm);
                    lblScreenName.Text = "Automated Reminders";
                    break;

                case APHOLD:
                    closeAllScreens();
                    screenHold.Visibility = System.Windows.Visibility.Visible;
                    screenHold.Load(perm);
                    lblScreenName.Text = "Hold Payment";
                    break;

                case APVENDORMAP:
                    closeAllScreens();
                    screenVendorMap.Visibility = System.Windows.Visibility.Visible;
                    screenVendorMap.Load(perm);
                    lblScreenName.Text = "Vendor Mapping";
                    break;

                case RESOURCE:
                    closeAllScreens();
                    screenResourcePlanning.Visibility = System.Windows.Visibility.Visible;
                    screenResourcePlanning.Load(perm);
                    lblScreenName.Text = "Project Resource Management";
                    break;
            }
        }

        private void lblCurrentUser_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //scrollWrapPanel.Visibility = System.Windows.Visibility.Hidden;
            //screenSwitch.Visibility = System.Windows.Visibility.Visible;
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            showMainScreen();
        }

        public void switchUser(string newUser)
        {
            lblCurrentUser.Text = newUser;
        }

        public void showMainScreen()
        {
            closeAllScreens();
            scrollWrapPanel.Visibility = System.Windows.Visibility.Visible;
            lblScreenName.Text = "Home Screen";
        }

        public void closeAllScreens()
        {
            scrollWrapPanel.Visibility = System.Windows.Visibility.Hidden;

            foreach (Control c in gridInnerWorkArea.Children)
                if (c.Name != "")
                    c.Visibility = System.Windows.Visibility.Hidden;
        }
        #endregion

        #region Security
        private string getPermissions(string user, string function, DataTable dtUserPrivs = null)
        {
            if (dtUserPrivs == null)
                dtUserPrivs = ScriptEngine.script.runScript(null, "CRUD_CONFIGURATION", "OPSCONSOLE", "UserPermissions");
            if (dtUserPrivs == null)
            {
                MessageBox.Show("Error trying to get permissions with script CRUD_CONFIGURATION");
                return "";
            }

            foreach (DataRow drPriv in dtUserPrivs.Rows)
            {
                // was if ((drPriv["Username"].ToString() == user) && (drPriv["Function"].ToString() == function))
                // Oct 2 2016
                if ((drPriv["Username"].ToString() == user) && (drPriv["Function"].ToString() == function)  && (drPriv["EndDate"] == System.DBNull.Value))
                    return drPriv["Permission"].ToString();
            }
            return "";
        }
        #endregion

        #region utility
        public string lookup(DataTable dt, string fromField, string fromVal, string lookupField)
        {
            foreach (DataRow dr in dt.Rows)
                if (dr[fromField].ToString() == fromVal)
                    return dr[lookupField].ToString();
            return "";
        }

        public int maxval(DataTable dt, string fromField)
        {
            int max = -1;
            foreach (DataRow dr in dt.Rows)
            {
                int val = Convert.ToInt32(dr[fromField].ToString());
                if (val > max)
                    max = val;
            }
            return max;
        }

        private string readerString(SqlDataReader reader, string field)
        {
            if (reader[field] == null)
                return "";
            return reader[field].ToString();
        }
        #endregion

        #region ServiceDesk
        public bool showTicket(TextBlock tbNote, string ticketNumber)
        {
            SDTicket.RequestLongForm ticketInfo = SDTicket.getServiceTicketEntry(ticketNumber);
            if (ticketInfo != null)
            {
                tbNote.Inlines.Clear();
                tbNote.Inlines.Add(new Run("Category: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CATEGORY + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("By: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CREATEDBY + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Time: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CREATEDTIME + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Dept: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.DEPARTMENT + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Requester: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.REQUESTER + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("E-Mail: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.REQUESTEREMAIL + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Subject: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.SUBJECT + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Desc: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.SHORTDESCRIPTION.TrimStart().Replace("&nbsp;", " ") + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Technician: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.TECHNICIAN + Environment.NewLine) { Foreground = Brushes.MidnightBlue });
                return true;
            }
            else
            {
                tbNote.Inlines.Clear();
                tbNote.Inlines.Add(new Run("Unable to find ticket") { FontWeight = FontWeights.Bold });
                return false;
                // MarkTicketAsBad(ticketNumber);
            }
        }
        #endregion
    }
}
