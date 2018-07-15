using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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

namespace ApplicationAccess
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataTable dtByPeople;
        private DataTable dtPermissionAccess;
        public ScriptEngine se = new ScriptEngine();
        public DataTable dtUsers;

        public MainWindow()
        {
            InitializeComponent();

            // Get logged in user
            string currentUser = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            lblCurrentUser.Text = currentUser;
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                se = new ScriptEngine();
                dtByPeople = se.runScript(new DataTable(), "AA_GETBYAPPLICATION", "OPSCONSOLE");
                showRadioButtonStatus(btnByApplication, new Button[] { btnByPeople, btnByApplication, btnByDate });

                LoadUsers();
                LoadPermissionAccess();
                // fillByPeople();
                fillByApplication();

                gridDate.Visibility = Visibility.Collapsed;
                gridApplication.Visibility = Visibility.Visible;
                gridPeople.Visibility = Visibility.Collapsed;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Oooops, got this error: " + Environment.NewLine + ex.ToString());
            }

        }

        private void LoadUsers()
        {
            dtUsers = se.runScript(new DataTable(), "AA_GETUSERS", "OPSCONSOLE");
        }

        private void LoadPermissionAccess()
        {
            dtPermissionAccess = se.runScript(new DataTable(), "AA_GETPERMISSIONACCESS", "OPSCONSOLE");
        }

        private void fillByPeople()
        {
//            string format = getRadioButtonStatus(new Button[] { btnLook1, btnLook2, btnLook3, btnLook4, btnLook5 });
            bool addBlankRows = false;
            bool nameBold = true;
            bool backgroundGray = true;
            bool foregroundGray = true;
            bool backgroundBlue = false;
            bool foregroundBlue = false;

            // Show columns as PERSON .... PERMISSIONS
            colAssociate1.Visibility = Visibility.Visible;
            colAssociate2.Visibility = Visibility.Collapsed;
            colAppName.Visibility = Visibility.Collapsed;
            colAssociate.Visibility = Visibility.Collapsed;

            DataTable formattedTable = dtByPeople.Clone();
            string find = ebPeopleFilter.Text.ToUpper();

            string currentAdjustedName = "";
            foreach (DataRow dr in dtByPeople.Rows)
            {
                if ((dr["IsActive"].ToString() == "1") && ((find == "") || (dr["AdjustedName"].ToString().ToUpper().IndexOf(find) >= 0)))
                {
                    if (dr["AdjustedName"].ToString() != currentAdjustedName)
                    {
                        if ((addBlankRows) && (currentAdjustedName != ""))
                            formattedTable.Rows.Add(formattedTable.NewRow());

                        currentAdjustedName = dr["AdjustedName"].ToString();
                    }

                    formattedTable.ImportRow(dr);
                }
            }


            formattedTable.Columns.Add("Background");
            formattedTable.Columns.Add("Foreground");
            formattedTable.Columns.Add("FW");
            formattedTable.Columns.Add("DisplayAccessDesc");

            currentAdjustedName = "";
            string currentApplicationName = "";
            bool first = true;
            foreach (DataRow dr in formattedTable.Rows)
            {
                dr["DisplayAccessDesc"] = dr["AccessDesc"].ToString();

                dr["Foreground"] = "Black";
                if (dr["AdjustedName"].ToString() == currentAdjustedName)
                {
                    dr["AdjustedName"] = "";

                    //if (format == "btnLook4")
                    //    dr["Background"] = "White";
                }
                else
                {
                    if (first)
                        first = false;
                    else
                    {
                    }

                    if (nameBold)
                        dr["FW"] = "Bold";

                    if (backgroundGray)
                        dr["Background"] = "LightGray";

                    if (foregroundGray)
                        dr["Foreground"] = "0xFFDDDDDD";

                    if (backgroundBlue)
                        dr["Background"] = "LightSteelBlue";
                    if (foregroundBlue)
                        dr["Foreground"] = "MidnightBlue";

                    currentAdjustedName = dr["AdjustedName"].ToString();
                    currentApplicationName = "";
                }


                //if ((format == "btnLook3") && (dr["AccessTypeName"].ToString() == ""))
                //{
                //    dr["Background"] = "White";
                //}

                //if ((format == "btnLook4") && (dr["AccessTypeName"].ToString() == ""))
                //{
                //    dr["Background"] = "WhiteSmoke";
                //}
                
                if (dr["ApplicationName"].ToString() == currentApplicationName)
                    dr["ApplicationName"] = "";
                else
                    currentApplicationName = dr["ApplicationName"].ToString();
            }


            gridResults.ItemsSource = formattedTable.DefaultView;
        }

        private void fillByApplication()
        {
            bool addBlankRows = false;
            bool nameBold = true;
            bool backgroundGray = true;
            bool foregroundGray = true;
            bool backgroundBlue = false;
            bool foregroundBlue = false;

            // Show columns as PERMISSIONS ... PERSON
             colAssociate1.Visibility = Visibility.Collapsed;
             colAssociate2.Visibility = Visibility.Collapsed;
             colAppName.Visibility = Visibility.Visible;
             colAssociate.Visibility = Visibility.Visible;

             DataTable formattedTable = dtByPeople.Clone();

            // THIS CODE IS FOR SEARCHING BY PERSON. 
            // IT COPIES FROM dtByPeople to formattedTable
            string find = ebAppFilter.Text.ToUpper();
            string currentAdjustedName = "";
            foreach (DataRow dr in dtByPeople.Rows)
            {
                if ((dr["IsActive"].ToString() == "1") && ((find == "") || (dr["ApplicationName"].ToString().ToUpper().IndexOf(find) >= 0)))
                {
                    if (dr["ApplicationName"].ToString() != currentAdjustedName)
                    {
                        if ((addBlankRows) && (currentAdjustedName != ""))
                            formattedTable.Rows.Add(formattedTable.NewRow());

                        currentAdjustedName = dr["ApplicationName"].ToString();
                    }

                    formattedTable.ImportRow(dr);
                }
            }
            // END OF SEARCHING CODE

            formattedTable.Columns.Add("Background");
            formattedTable.Columns.Add("Foreground");
            formattedTable.Columns.Add("FW");
            formattedTable.Columns.Add("ApplicationNameCopy");

            // BILL: Add column for what to display as
            formattedTable.Columns.Add("DisplayAccessDesc");

            currentAdjustedName = "";
            string currentApplicationName = "";

            // This will start off as ""
            // Then have the most recent value
            // On each row, if the current row data == currentAccessDesc then we now that DisplayAccessDesc should be blank
            // Otherwise, it is the first row with this data and needs highlighting
            string currentAccessDesc = "";

            bool first = true;
            foreach (DataRow dr in formattedTable.Rows)
            {
                // BILL: FOR NOW...... Always the same
                dr["DisplayAccessDesc"] = dr["AccessDesc"].ToString();
                dr["ApplicationNameCopy"] = dr["ApplicationName"].ToString();

               // Will change to "" when same as previous

                dr["Foreground"] = "Black";

                // CHANGE TO COMPARE currentAccessDescription instead of name
                if (dr["ApplicationName"].ToString() == currentAdjustedName)
                {
                    // BILL Set DisplayAccessDesc to "" if same as last value here
                    dr["ApplicationName"] = "";

                    //if (format == "btnLook4")
                    //    dr["Background"] = "White";
                }
                else
                {
                    if (first)
                        first = false;
                    else
                    {
                    }

                    if (nameBold)
                        dr["FW"] = "Bold";

                    if (backgroundGray)
                        dr["Background"] = "LightGray";

                    if (foregroundGray)
                        dr["Foreground"] = "0xFFDDDDDD";

                    if (backgroundBlue)
                        dr["Background"] = "LightSteelBlue";
                    if (foregroundBlue)
                        dr["Foreground"] = "MidnightBlue";


                    // set currentAccessDesc here

                    currentAdjustedName = dr["ApplicationName"].ToString();
                    currentApplicationName = "";
                }


                //if ((format == "btnLook3") && (dr["AccessTypeName"].ToString() == ""))
                //{
                //    dr["Background"] = "White";
                //}

                //if ((format == "btnLook4") && (dr["AccessTypeName"].ToString() == ""))
                //{
                //    dr["Background"] = "WhiteSmoke";
                //}
                
                if (dr["ApplicationName"].ToString() == currentApplicationName)
                    dr["ApplicationName"] = "";
                else
                    currentApplicationName = dr["ApplicationName"].ToString();
            }


            gridResults.ItemsSource = formattedTable.DefaultView;
        }


        private void btnByPeople_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnByPeople, new Button[] { btnByPeople, btnByApplication, btnByDate });
            dtByPeople = se.runScript(new DataTable(), "AA_GETBYPEOPLE", "OPSCONSOLE");
            fillByPeople();
            gridDate.Visibility = Visibility.Collapsed;
            gridApplication.Visibility = Visibility.Collapsed;
            gridPeople.Visibility = Visibility.Visible;
        }

        public void reloadByApplication()
        {
            dtByPeople = se.runScript(new DataTable(), "AA_GETBYAPPLICATION", "OPSCONSOLE");
            fillByApplication();
        }

        private void btnByApplication_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnByApplication, new Button[] { btnByPeople, btnByApplication, btnByDate });
            dtByPeople = se.runScript(new DataTable(), "AA_GETBYAPPLICATION", "OPSCONSOLE");
            fillByApplication();
            gridDate.Visibility = Visibility.Collapsed;
            gridApplication.Visibility = Visibility.Visible;
            gridPeople.Visibility = Visibility.Collapsed;
        }

        private void btnByDate_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnByDate, new Button[] { btnByPeople, btnByApplication, btnByDate });
            dtByPeople = se.runScript(new DataTable(), "AA_GETBYDATE", "OPSCONSOLE");
            fillByPeople();
            gridDate.Visibility = Visibility.Visible;
            gridApplication.Visibility = Visibility.Collapsed;
            gridPeople.Visibility = Visibility.Collapsed;
        }

        private void imgPeopleSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void ebPeopleFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            fillByPeople();
        }

        private void ebAppFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            fillByApplication();
        }

        public void showRadioButtonStatus(Button btnSet, Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                    if (c is Rectangle)
                        ((Rectangle)c).Opacity = (b == btnSet) ? 1d : 0.2d;
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

        private void btnNewApplicationAccess_Click(object sender, RoutedEventArgs e)
        {
            if (gridResults.SelectedValue == null)
            {
                MessageBox.Show("You must select a row first");
                return;
            }

            userPermissions.Visibility = System.Windows.Visibility.Visible;

            int applicationID = Convert.ToInt32(((DataRowView)gridResults.SelectedValue)["ApplicationID"].ToString());
            string applicationName = ((DataRowView)gridResults.SelectedValue)["ApplicationNameCopy"].ToString();
            DataTable dtAvailable = getAvailablePermissionsForAnApplication(applicationID);

            DataTable dtCurrentPermissions = new DataTable();
            dtCurrentPermissions.Columns.Add("AccessID");
            dtCurrentPermissions.Columns.Add("AccessDesc");
            userPermissions.start(this, dtAvailable, dtCurrentPermissions, true, applicationName, applicationID.ToString(), "0", "", "",false);
        }

        private void btnModifyApplicationAccess_Click(object sender, RoutedEventArgs e)
        {
            if (gridResults.SelectedValue == null)
            {
                MessageBox.Show("You must select a row first");
                return;
            }

            userPermissions.Visibility = System.Windows.Visibility.Visible;

          /*  if ( row is now selected )*/

            int applicationID = Convert.ToInt32(((DataRowView)gridResults.SelectedValue)["ApplicationID"].ToString());
            string userID = ((DataRowView)gridResults.SelectedValue)["ApplicationUserID"].ToString();
            string userName = ((DataRowView)gridResults.SelectedValue)["ApplicationUserName"].ToString();
            string adjustedName = ((DataRowView)gridResults.SelectedValue)["AdjustedName"].ToString();
            string applicationName = ((DataRowView)gridResults.SelectedValue)["ApplicationNameCopy"].ToString();

            // FOR NOW..... the application info is in dtByPeople, but we will move it on Thursday (that's tomorrow)
            DataTable dtCurrentPermissions = new DataTable();
            dtCurrentPermissions.Columns.Add("AccessID");
            dtCurrentPermissions.Columns.Add("AccessDesc");

            foreach (DataRow dr in dtByPeople.Rows)
            {
                if ((dr["ApplicationUserID"].ToString() == userID) && ((dr["ApplicationID"].ToString() == applicationID.ToString())))
                    dtCurrentPermissions.Rows.Add(dr["AccessID"].ToString(), dr["AccessDesc"].ToString());
            }

            DataTable dtAvailable = getAvailablePermissionsForAnApplication(applicationID);

            userPermissions.start(this, dtAvailable, dtCurrentPermissions, false, applicationName, applicationID.ToString(), userID, userName, adjustedName, false);
        }


        private DataTable getAvailablePermissionsForAnApplication(int applicationID)
        {
            /// PART 1 - Figure out how to make a clone table from dtPermissionAccess 
            DataTable formattedTable = dtPermissionAccess.Clone();

            foreach (DataRow dr in dtPermissionAccess.Rows)
            {
                if (dr["ApplicationID"].ToString() == applicationID.ToString())
                {
                    formattedTable.ImportRow(dr);
                }
            }
            return formattedTable;
        }

        public void askForTicket()
        {
            obtainTicket.Visibility = System.Windows.Visibility.Visible;
        }

        private void gridResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (gridResults.SelectedIndex < 0)
                return;


            string format = getRadioButtonStatus(new Button[] { btnByPeople, btnByApplication, btnByDate });
            if (format != "btnByApplication")
                return;


            string applicationName = ((DataRowView)gridResults.SelectedValue)["ApplicationNameCopy"].ToString();
            string userName = ((DataRowView)gridResults.SelectedValue)["AdjustedName"].ToString();

            lblModifyButtonApplicationName.Text = applicationName;
            lblModifyButtonUserName.Text = userName + " in";
            lblNewUserApplicationName.Text = applicationName;
            lblShitcanButtonUserName.Text = userName;
            btnModifyApplicationAccess.Visibility = System.Windows.Visibility.Visible;
            btnNewApplicationAccess.Visibility = System.Windows.Visibility.Visible;
            btnTerminteUser.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnTerminteUser_Click(object sender, RoutedEventArgs e)
        {
            if (gridResults.SelectedValue == null)
            {
                MessageBox.Show("You must select a row first");
                return;
            }

            userPermissions.Visibility = System.Windows.Visibility.Visible;

            /*  if ( row is now selected )*/

            int applicationID = Convert.ToInt32(((DataRowView)gridResults.SelectedValue)["ApplicationID"].ToString());
            string userID = ((DataRowView)gridResults.SelectedValue)["ApplicationUserID"].ToString();
            string userName = ((DataRowView)gridResults.SelectedValue)["ApplicationUserName"].ToString();
            string adjustedName = ((DataRowView)gridResults.SelectedValue)["AdjustedName"].ToString();
            string applicationName = ((DataRowView)gridResults.SelectedValue)["ApplicationNameCopy"].ToString();

            DataTable dtCurrentPermissions = new DataTable();
            dtCurrentPermissions.Columns.Add("AccessID");
            dtCurrentPermissions.Columns.Add("AccessDesc");

            foreach (DataRow dr in dtByPeople.Rows)
            {
                if ((dr["ApplicationUserID"].ToString() == userID) && ((dr["ApplicationID"].ToString() == applicationID.ToString())))
                    dtCurrentPermissions.Rows.Add(dr["AccessID"].ToString(), dr["AccessDesc"].ToString());
            }

            DataTable dtAvailable = getAvailablePermissionsForAnApplication(applicationID);

            userPermissions.start(this, dtAvailable, dtCurrentPermissions, false, applicationName, applicationID.ToString(), userID, userName, adjustedName, true);

        }


    }


    [ValueConversion(typeof(string), typeof(System.Windows.FontWeight))]
    public class StringToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType,
        object parameter, CultureInfo culture)
        {
            FontWeight fontWt;
            switch (value.ToString())
            {
                case "Bold":
                    fontWt = FontWeights.Bold;
                    break;
                case "ExtraBold":
                    fontWt = FontWeights.ExtraBold;
                    break;
                case "Normal":
                    fontWt = FontWeights.Normal;
                    break;
                case "Light":
                    fontWt = FontWeights.Light;
                    break;
                default:
                    fontWt = FontWeights.Normal;
                    break;
            }
            return fontWt;
        }

        public object ConvertBack(object value, Type targetType,
                                  object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
