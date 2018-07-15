using System;
using System.Collections.Generic;
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
using System.DirectoryServices.AccountManagement;
using System.Data;
using System.Diagnostics;

namespace PINSPayments
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public static string currentUser = "";
        public static MainWindow ourMainWindow = null;
        public DataTable dtUserPrivs = null;

        public MainWindow()
        {
            InitializeComponent();

            // Keep track of the one main window
            ourMainWindow = this;

            currentUser = UserPrincipal.Current.SamAccountName;
            lblCurrentUser.Text = currentUser;

            // Determine if we are DEV, TEST or PROD
            setupDev();
            setupTest();
            setupProd();

            string perm = getPermissions(currentUser.ToLower(), "APHOLD");
            screenHold.Load(perm);  // fix add permissions
            screenManageCeded.Load(perm);
            lblAccessLevel.Text = (perm != "Modify") ? "Read only" : "";

            switchToHoldScreen();
        }

        #region DEV / TEST / PROD
        [Conditional("DEV")]
        private void setupDev()
        {
            lblEnvironment.Text = "DEV";
            //lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 211, 211, 211));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.DEV;
        }

        [Conditional("TEST")]
        private void setupTest()
        {
            lblEnvironment.Text = "TEST";
            // lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0x99, 0x99, 0x99));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.TEST;
        }

        [Conditional("PROD")]
        private void setupProd()
        {
            lblEnvironment.Text = "Production";
            // lblEnvironment.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xD8, 0x85, 0x20));
            ScriptEngine.envCurrent = ScriptEngine.environemnt.PROD;
        }
        #endregion


        private string getPermissions(string user, string function)
        {
            if (!user.StartsWith("trg"))
                user = "trg\\" + user;

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
                if ((drPriv["Username"].ToString().ToLower() == user.ToLower()) && (drPriv["Function"].ToString() == function) && (drPriv["EndDate"] == System.DBNull.Value))
                    return drPriv["Permission"].ToString();
            }
            return "";
        }


        public void AdjustForUser()
        {
            string perm = getPermissions(currentUser.ToLower(), "APHOLD");
            screenHold.Load(perm);  // fix add permissions
            screenManageCeded.Load(perm);
            lblAccessLevel.Text = (perm != "Modify") ? "Read only" : "";
        }

        public void showMainScreen()
        {
            //closeAllScreens();
            //scrollWrapPanel.Visibility = System.Windows.Visibility.Visible;
            //lblScreenName.Text = "Home Screen";
        }

        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            showMainScreen();
        }

        private void btnHoldScreen_Click(object sender, RoutedEventArgs e)
        {
            switchToHoldScreen();
        }

        private void switchToHoldScreen()
        {
            lblScreenName.Text = "Assumed Payments";
            UIHelpers.showRadioButtonStatus(btnHoldScreen, new Button[] { btnHoldScreen, btnManageCededCompanyHold, btnAssumedPayments });
            closeScreens();
            screenHold.Visibility = Visibility.Visible;
            btnActivateCheckRun.Visibility = Visibility.Visible;
        }

        private void btnManageCededCompanyHold_Click(object sender, RoutedEventArgs e)
        {
            lblScreenName.Text = "Manage Ceding Company Hold";
            UIHelpers.showRadioButtonStatus(btnManageCededCompanyHold, new Button[] { btnHoldScreen, btnManageCededCompanyHold, btnAssumedPayments });
            closeScreens();
            screenManageCeded.Visibility = Visibility.Visible;
        }

        private void btnAssumedPayments_Click(object sender, RoutedEventArgs e)
        {
            lblScreenName.Text = "Payment Status";
            screenPayments.Load();
            UIHelpers.showRadioButtonStatus(btnAssumedPayments, new Button[] { btnHoldScreen, btnManageCededCompanyHold, btnAssumedPayments });
            closeScreens();
            screenPayments.Visibility = Visibility.Visible;
        }

        private void closeScreens()
        {
            screenHold.Visibility = Visibility.Collapsed;
            screenManageCeded.Visibility = Visibility.Collapsed;
            screenPayments.Visibility = Visibility.Collapsed;
            btnActivateCheckRun.Visibility = Visibility.Collapsed;
        }

        private void toolbar_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if ((lblEnvironment.Text != "Production") || (UserPrincipal.Current.SamAccountName.ToUpper().Replace("TRG\\","").StartsWith("SMARC")))
            {
                DataRow[] rowsFiltered = APHold.dtEmployees.Select("Department='Claims' or Department='Reinsurance'");
                DataView dv = rowsFiltered.CopyToDataTable().DefaultView;
                dv.Sort = "AdjustedName";
                ucQAUser.dgFrom.ItemsSource = dv.ToTable().DefaultView;

                ucQAUser.Visibility = Visibility.Visible;
            }
        }

        

        private void btnActivateCheckRun_Click(object sender, RoutedEventArgs e)
        {
            DataSet dsResults = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_ACTIVATE_CHECK_RUN_MAIL", "OPSCONSOLE");
        }
    }


}
