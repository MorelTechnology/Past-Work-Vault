using System;
using System.Collections.Generic;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for APVendorMap.xaml
    /// </summary>
    public partial class APVendorMap : UserControl
    {
        #region Class Data
        DataTable dtUnmatchedVendor = new DataTable();
        DataTable dtDisctinctUnmatchedVendor = new DataTable();
        DataTable dtVendorMatch = new DataTable();
        public bool readOnly = false;
        #endregion

        #region Constructor and Initialization
        public APVendorMap()
        {
            InitializeComponent();
        }

        public void Load(string permissions)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1.0;
            da.To = 0.2d;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.8d));
            imgBackground.BeginAnimation(OpacityProperty, da);

            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 0.0;
            da2.To = 1.0;
            da2.Duration = new Duration(TimeSpan.FromSeconds(1.8d));
            gridBank.BeginAnimation(OpacityProperty, da2);
            gridCM.BeginAnimation(OpacityProperty, da2);
            gridDetails.BeginAnimation(OpacityProperty, da2);
            gridSite.BeginAnimation(OpacityProperty, da2);
            gridUnmatched.BeginAnimation(OpacityProperty, da2);

            readOnly = !(permissions == "Modify");
            
            LoadUnmappedVendors();
            resetMatch();
            showVendor();
            loadVendorMap();
            loadPINS();
            comboBankType.SelectedIndex = 0;
        }

        #endregion

        #region Load Data
        private void loadPINS()
        {
            DataTable dtPINS = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_GET_PINS_PAYMENT_DETAILS", "OPSCONSOLE").Tables["UM"];
            dgPINSInput.ItemsSource = dtPINS.DefaultView;
        }

        public void LoadUnmappedVendors()
        {
            dtUnmatchedVendor = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_UNMATCHED_VENDORS", "OPSCONSOLE").Tables["UM"];
            dgUnmatchedVendors.ItemsSource = dtUnmatchedVendor.DefaultView;
        }

        private void loadVendorMap()
        {
            dtVendorMatch = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_GET_VENDOR_MAP", "OPSCONSOLE").Tables["UM"];
            dgVendorMatch.ItemsSource = dtVendorMatch.DefaultView;
        }

        private void LoadSites(string primaryAddressID, string contactid)
        {
            ////// If there is no ID, there are no sites //////
            if (primaryAddressID == "")
            {
                dgSites.ItemsSource = null;
                return;
            }

            ////// Put the search clause into a table //////
            DataTable dtFind = new DataTable();
            dtFind.Columns.Add("passedtext");
            dtFind.TableName = "text";
            dtFind.Rows.Add(primaryAddressID);

            ////// Call our RSSE script //////
            DataSet dsPrim = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtFind, "AP_CONTMGR_PRIMADDRESS", "OPSCONSOLE");
            DataTable dtContacts = dsPrim.Tables[0];

            ////// Put the search clause into a table //////
            DataTable dtFind2 = new DataTable();
            dtFind2.Columns.Add("passedtext");
            dtFind2.TableName = "text";
            dtFind2.Rows.Add(contactid);

            ////// Call our RSSE script //////
            DataSet dsSec = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtFind2, "AP_CONTMGR_SECADDRESS", "OPSCONSOLE");
            DataTable dtContacts2 = dsSec.Tables[0];

            ////// Add the secondary addresses in after the primary //////
            foreach (DataRow dr in dtContacts2.Rows)
                dtContacts.ImportRow(dr);

            ////// Fill the grid and select if only one //////
            dgSites.ItemsSource = dtContacts.DefaultView;
            if (dtContacts.Rows.Count == 1)
                dgSites.SelectedIndex = 0;
        }

        private void LoadBanks(string contactid)
        {
            ////// If there is no ID, there are no banks //////
            if (contactid == "")
            {
                dgBank.ItemsSource = null;
                return;
            }

            ////// Put the search clause into a table //////
            DataTable dtFind = new DataTable();
            dtFind.Columns.Add("passedtext");
            dtFind.TableName = "text";
            dtFind.Rows.Add(contactid);

            ////// Call our RSSE script //////
            DataSet dsPrim = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtFind, "AP_CONTMGR_BANKS", "OPSCONSOLE");
            DataTable dtBanks = dsPrim.Tables[0];

            ////// Fill the grid and select if only one //////
            dgBank.ItemsSource = dtBanks.DefaultView;
            if (dtBanks.Rows.Count == 1)
                dgBank.SelectedIndex = 0;
        }

        private void ClearSites()
        {
            dgSites.ItemsSource = null;
        }

        private void ClearBanks()
        {
            dgBank.ItemsSource = null;
        }
        #endregion

        #region Commands
        private void btnExpandContacts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            btnExpandContacts.Visibility = System.Windows.Visibility.Collapsed;
            btnContractContacts.Visibility = System.Windows.Visibility.Visible;

            ThicknessAnimation da = new ThicknessAnimation();
            da.From = new Thickness(395, 0, 320, 10);
            da.To = new Thickness(395, 0, 80, 10);
            da.Duration = new Duration(TimeSpan.FromSeconds(0.8d));
            gridSite.BeginAnimation(MarginProperty, da);
        }

        private void btnContractContacts_MouseDown(object sender, MouseButtonEventArgs e)
        {
            btnExpandContacts.Visibility = System.Windows.Visibility.Visible;
            btnContractContacts.Visibility = System.Windows.Visibility.Collapsed;

            ThicknessAnimation da = new ThicknessAnimation();
            da.From = new Thickness(395, 0, 80, 10);
            da.To = new Thickness(395, 0, 320, 10);
            da.Duration = new Duration(TimeSpan.FromSeconds(0.8d));
            gridSite.BeginAnimation(MarginProperty, da);
        }

        private void circleExpandContacts_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void btnSearchCompanies_Click(object sender, RoutedEventArgs e)
        {
            ////// Make sure the user has entered some text //////
            if (txtCompany.Text.Trim() == "")
            {
                MessageBox.Show("You must enter some text in the first text box (Company / First Name)");
                return;
            }

            ////// Arrange the output columns for companies //////
            colCompany.Visibility = System.Windows.Visibility.Visible;
            colFirst.Visibility = System.Windows.Visibility.Collapsed;
            colLast.Visibility = System.Windows.Visibility.Collapsed;

            ////// Highly permissive search - Build the where clause from the words entered //////
            string sql = "";
            bool first = true;
            string[] words = txtCompany.Text.Trim().Split(new char[] { ' ' });
            foreach (string word in words)
            {
                if (first)
                {
                    sql += "  cont.[Name] like '%" + word.Replace("'","''") + "%'";
                    first = false;
                }
                else
                {
                    sql += "  and cont.[Name] like '%" + word.Replace("'", "''") + "%'";
                }
            }

            sql += " and trg_GlobalVendorID is not null";

            ////// Call our RSSE script //////
            DataSet dsTest = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_CONTMGR_FIND_COMPANY", "OPSCONSOLE", false, "@lookingfor", sql);
            dgContacts.ItemsSource = (dsTest == null) ? null : dsTest.Tables[0].DefaultView;
        }

        private void btnSearchPeople_Click(object sender, RoutedEventArgs e)
        {
            ////// Make sure the user has entered some text //////
            if ((txtCompany.Text.Trim() == "") && (txtLastName.Text.Trim() == ""))
            {
                MessageBox.Show("You must enter some text in the first text box (First Name) or the second text box (Last Name) or both.");
                return;
            }

            ////// Arrange the output columns for companies //////
            colCompany.Visibility = System.Windows.Visibility.Collapsed;
            colFirst.Visibility = System.Windows.Visibility.Visible;
            colLast.Visibility = System.Windows.Visibility.Visible;

            ////// Highly permissive search - Build the where clause from the words entered //////
            string sql = "";
            bool first = true;
            if (txtCompany.Text.Trim() != "")
            {
                string[] words = txtCompany.Text.Trim().Split(new char[] { ' ' });
                foreach (string word in words)
                {
                    if (first)
                    {
                        sql += "  cont.[FirstName] like '%" + word + "%'";
                        first = false;
                    }
                    else
                    {
                        sql += "  and cont.[FirstName] like '%" + word + "%'";
                    }
                }
            }
            if (txtLastName.Text.Trim() != "")
            {
                string[] words = txtLastName.Text.Trim().Split(new char[] { ' ' });
                foreach (string word in words)
                {
                    if (first)
                    {
                        sql += "  cont.[LastName] like '%" + word + "%'";
                        first = false;
                    }
                    else
                    {
                        sql += "  and cont.[LastName] like '%" + word + "%'";
                    }
                }
            }

            ////// Put the search clause into a table //////
            sql += " and trg_GlobalVendorID is not null";

            ////// Call our RSSE script //////
            DataSet dsTest = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_CONTMGR_FIND_COMPANY", "OPSCONSOLE", false, "@lookingfor", sql);
            dgContacts.ItemsSource = dsTest.Tables[0].DefaultView;
        }

        private void btnVendorDetails_Click(object sender, RoutedEventArgs e)
        {
            showVendor();
        }

        private void btnPINSDetails_Click(object sender, RoutedEventArgs e)
        {
            showPINS();
        }

        private void btnDeleteVendorMatch_Click(object sender, RoutedEventArgs e)
        {
            if (readOnly)
            {
                MessageBox.Show("You have read-only access to this screen. If you need modify access please submit a Service Desk ticket and request Modify access to the Vendor Map screen in OpsConsole");
                return;
            }
            // Note: Table type PINS.utVendorMapping has the columns:
            //       ID, PayeeID, VendorName, CMVendorID, SiteID, BankID, MethordOfPayment, Operation

            string vendor = ((System.Data.DataRowView)dgVendorMatch.SelectedItem)["VendorName"].ToString();
            string vendorID = ((System.Data.DataRowView)dgVendorMatch.SelectedItem)["ID"].ToString();

            DataTable dtUpdate = dtVendorMatch.Clone();
            foreach (DataRow dr in dtVendorMatch.Rows)
                if (dr["ID"].ToString() == vendorID)
                    dtUpdate.ImportRow(dr);

            dtUpdate.Columns.Add("Operation");
            dtUpdate.Columns.Remove("StartUser");
            dtUpdate.Columns.Remove("EndDate");
            dtUpdate.Columns.Remove("EndUser");

            dtUpdate.TableName = "VendorMapTable";

            foreach (DataRow dr in dtUpdate.Rows)
            {
                dr["Operation"] = "D";
            }

            dtVendorMatch = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, "AP_CRUD_VENDOR_MAP", "OPSCONSOLE").Tables["UM"];

            LoadUnmappedVendors();
            resetMatch();
            showVendor();
            loadVendorMap();
            loadPINS();
        }

        private void btnMatch_Click(object sender, RoutedEventArgs e)
        {
            if (readOnly)
            {
                MessageBox.Show("You have read-only access to this screen. If you need modify access please submit a Service Desk ticket and request Modify access to the Vendor Map screen in OpsConsole");
                return;
            }

            if (matchRing.Visibility != System.Windows.Visibility.Visible)
            {
                MessageBox.Show("When all three items are matched, Contact, Site and Bank, a green ring will appear around the match button showing that matching is now enabled\nNo match can be made at this time") ;
                return;
            }

            // Before Feb 23, 2016 we were using the PINS BrokerName
            // string vendor = ((System.Data.DataRowView)dgUnmatchedVendors.SelectedItem)["BrokerName"].ToString();
            string vendor = "";
            string vendorID = ((System.Data.DataRowView)dgUnmatchedVendors.SelectedItem)["payeeid"].ToString();
            string contactID = ((System.Data.DataRowView)dgContacts.SelectedItem)["trg_GlobalVendorID"].ToString();  // changed from ID on Dec 29
            string vendorStatus = ((System.Data.DataRowView)dgContacts.SelectedItem)["VendorStatus"].ToString();  // changed from ID on Dec 29
            string sites = ((System.Data.DataRowView)dgSites.SelectedItem)["trg_ABContactOrder"].ToString();  // changed from ID on Dec 29 2015

            // If the Vendor is a company use Name
            // If the Vendor is a person use Firstname + " " + LastName
            if (colCompany.Visibility == System.Windows.Visibility.Visible)
                vendor = ((System.Data.DataRowView)dgContacts.SelectedItem)["Name"].ToString();
            else
                vendor = ((System.Data.DataRowView)dgContacts.SelectedItem)["FirstName"].ToString() + " " + ((System.Data.DataRowView)dgContacts.SelectedItem)["LastName"].ToString();

            if (vendorStatus != "Active")
            {
                MessageBox.Show("This vendor cannot be mapped until it is made active. Please contact Treasury and ask them to make this vendor active.");
                return;
            }

            string bank = "0";
            if ((dgBank.Visibility == System.Windows.Visibility.Visible) && (dgBank.SelectedIndex >= 0))
                bank = ((System.Data.DataRowView)dgBank.SelectedItem)["trg_ABContactOrder"].ToString();  // changed from ID on Dec 29 2015

            // Note: Table type PINS.utVendorMapping has the columns:
            //       ID, PayeeID, VendorName, CMVendorID, SiteID, BankID, MethordOfPayment, Operation

            string methodofpayment = (comboBankType.SelectedIndex < 0) ? "Check" : ((ComboBoxItem) comboBankType.SelectedItem).Content.ToString();


            DataTable dtUpdate = dtVendorMatch.Clone();

            dtUpdate.Columns.Add("Operation");
            // dtUpdate.Columns.Add("Username");
            dtUpdate.Columns.Remove("StartUser");
            dtUpdate.Columns.Remove("EndDate");
            dtUpdate.Columns.Remove("EndUser");

            if (sites == "")
                sites = "1";
            if (contactID == "")
                contactID = "missing";

            dtUpdate.TableName = "VendorMapTable";

            dtUpdate.Rows.Add(0, vendorID, vendor, contactID, sites, bank, methodofpayment, "I");
            dtVendorMatch = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, "AP_CRUD_VENDOR_MAP", "OPSCONSOLE").Tables["VM"];

            LoadUnmappedVendors();
            resetMatch();
            showVendor();
            loadVendorMap();
            loadPINS();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ourMainWindow.showMainScreen();
        }
        #endregion

        #region Events
        private void dgUnmatchedVendors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowMatches();
        }

        private void dgContacts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dgContacts.SelectedIndex >= 0) && (dgContacts.SelectedItem is System.Data.DataRowView))
            {
                LoadSites(((System.Data.DataRowView)dgContacts.SelectedItem)["PrimaryAddressID"].ToString(), ((System.Data.DataRowView)dgContacts.SelectedItem)["ID"].ToString());
                LoadBanks(((System.Data.DataRowView)dgContacts.SelectedItem)["ID"].ToString());
            }
            else
            {
                ClearSites();
                ClearBanks();
            }
            ShowMatches();
        }

        private void dgSites_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowMatches();
        }

        private void dgBank_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowMatches();
        }

        private void comboBankType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboBankType.SelectedIndex == -1)
                return;

            if (comboBankType.SelectedIndex == 0)
                dgBank.Visibility = System.Windows.Visibility.Collapsed;
            else
                dgBank.Visibility = System.Windows.Visibility.Visible;
            ShowMatches();
        }
        #endregion

        #region Drawing Logic
        private void ShowMatches()
        {
            if (dgUnmatchedVendors.SelectedIndex == -1)
            {
                showCMLine(false);
                showSiteLine(false);
                showBankLine(false);
            }
            else
            {
                showCMLine((dgContacts.SelectedIndex >= 0) ? true : false);
                showBankLine(((dgBank.SelectedIndex >= 0) || (comboBankType.SelectedIndex == 0)) ? true : false);
                showSiteLine((dgSites.SelectedIndex >= 0) ? true : false);
            }

            if ((barCM1.Visibility == System.Windows.Visibility.Visible) && (barSite1.Visibility == System.Windows.Visibility.Visible) && (barBank1.Visibility == System.Windows.Visibility.Visible))
                matchRing.Visibility = System.Windows.Visibility.Visible;
            else
                matchRing.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void showCMLine(bool visible)
        {
            barCM1.Visibility = (visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void showSiteLine(bool visible)
        {
            barSite1.Visibility = (visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            barSite2.Visibility = (visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            barSite3.Visibility = (visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void showBankLine(bool visible)
        {
            barBank1.Visibility = (visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            barBank2.Visibility = (visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            barBank3.Visibility = (visible) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void showVendor()
        {
            rectVendor.Opacity = 1d;
            rectPins.Opacity = .2d;
            dgVendorMatch.Visibility = System.Windows.Visibility.Visible;
            dgPINSInput.Visibility = System.Windows.Visibility.Collapsed;
            btnDeleteVendorMatch.Visibility = System.Windows.Visibility.Visible;
        }

        private void showPINS()
        {
            rectVendor.Opacity = .2d;
            rectPins.Opacity = 1d;
            dgVendorMatch.Visibility = System.Windows.Visibility.Collapsed;
            dgPINSInput.Visibility = System.Windows.Visibility.Visible;
            btnDeleteVendorMatch.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion

        #region Logic
        public void resetMatch()
        {
            barCM1.Visibility = System.Windows.Visibility.Hidden;
            barBank1.Visibility = barBank2.Visibility = barBank3.Visibility = System.Windows.Visibility.Hidden;
            barSite1.Visibility = barSite2.Visibility = barSite3.Visibility = System.Windows.Visibility.Hidden;
            matchRing.Visibility = System.Windows.Visibility.Hidden;
            dgContacts.SelectedIndex = -1;
            dgSites.SelectedIndex = -1;
            dgBank.SelectedIndex = -1;
        }
        #endregion

    }
}
