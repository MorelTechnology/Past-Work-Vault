using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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

namespace PINSPayments
{
    /// <summary>
    /// Interaction logic for APHold.xaml
    /// </summary>
    public partial class APHold : UserControl
    {
        #region Class data
        public DataTable dtPINS = new DataTable();
        public DataTable dtPINSSource = new DataTable();
        public static DataTable dtEmployees = new DataTable();
        DataTable dtHoldTypes = new DataTable();
        DataTable dtHoldReasons = new DataTable();
        DataTable dtTotals = new DataTable();

        const int maxCurrencyColumns = 5;
        const int rowGridTotal = 1;
        const int rowOnHoldTotal = 2;
        const int rowReleasedTotal = 3;
        const int rowSelectedTotal = 4;
        const int rowCount = 5;

        const string claimsColor = "LightPink";
        const string reinsColor = "LightBlue";

        string department = "";
        public bool readOnly = false;
        #endregion
        //PaymentSearchFilter filter1 = new PaymentSearchFilter();
        //PaymentSearchFilter filter2 = new PaymentSearchFilter();


        #region Constructors and Initialization
        public APHold()
        {
            InitializeComponent();
        }

        public void Load(string permissions)
        {
            filter1.hideClose();
            filter2.hideClearButton();
            adjustElements();
            loadConformed();
            loadHoldReasons();
            loadHoldTypes();
            loadADGroups();
            adjustForDepartment();
            calcTotals();
            readOnly = !(permissions == "Modify");
        }
        #endregion

        #region Load data
        private void loadConformed()
        {
            // See if we are sorted on a particular column
            string colname = "";
            System.ComponentModel.ListSortDirection? direction = null;
            string propertyName = "";

            getSortColumn(ref colname, ref direction, ref propertyName);

            dtPINSSource = new DataTable();
            dtPINSSource = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_CS_GETPAYMENTS", "OPSCONSOLE").Tables["UM"];

            dtPINS = new DataTable();
            dtPINS = dtPINSSource.Clone();
            string f1 = filter1.getSearchString() + " and " + filter2.getSearchString();
            DataRow[] drs = dtPINSSource.Select(f1);
            foreach (DataRow dr in drs)
                if (dr["EndDate"].ToString() == "")
                    dtPINS.ImportRow(dr);

            dtPINS.Columns.Add("Background");
            dtPINS.Columns.Add("PADouble", typeof(double));
            dtPINS.Columns.Add("ProcessingImage");
            dtPINS.Columns.Add("StatusImage");
            dtPINS.Columns.Add("StatusColor");
            dtPINS.Columns.Add("StatusSymbolText");
            dtPINS.Columns.Add("StatusColorProcess");
            dtPINS.Columns.Add("StatusSymbolTextProcess");

            foreach (DataRow dr in dtPINS.Rows)
            {
                dr["PADouble"] = Convert.ToDouble(dr["TotalPaymentAmount"].ToString());
                if (dr["TRAN_CODE"].ToString().ToUpper().StartsWith("X"))
                {
                    dr["Background"] = Colors.LightSteelBlue;
                }

                if (dr["PaymentHoldFlg"].ToString() == "True")
                {
                    //if (dr["PaymentHoldType"].ToString() == "Reinsurance")
                    //    dr["Background"] = reinsColor;
                    //if (dr["PaymentHoldType"].ToString() == "Claims")
                    //    dr["Background"] = claimsColor;


                    if (dr["PaymentHoldType"].ToString() == "Reinsurance")
                    {
                        dr["StatusImage"] = "images/hold-reinsurance.png";
                        dr["StatusColor"] = "RoyalBlue";
                        dr["StatusSymbolText"] = "l";
                    }
                    else if (dr["PaymentHoldType"].ToString() == "Claims")
                    {
                        dr["StatusImage"] = "images/hold-claims.png";
                        dr["StatusColor"] = "LightPink";
                        dr["StatusSymbolText"] = "l";

                    }
                    else if (dr["PaymentHoldType"].ToString() == "Cedent Company")
                    {
                        dr["StatusImage"] = "images/hold-claims.png";
                        dr["StatusColor"] = "Gold";
                        dr["StatusSymbolText"] = "l";

                    }
                    else
                    {
                        dr["StatusImage"] = "";
                        dr["StatusColor"] = "Transparent";
                        dr["StatusSymbolText"] = "";
                    }

                }
                else
                {
                    dr["StatusImage"] = "";
                    dr["StatusColor"] = "Transparent";
                    dr["StatusSymbolText"] = "";
                }
                if ((dr["StatusName"].ToString() == "Fail") && (dr["PaymentHoldFlg"].ToString() == "False"))
                {
                    dr["ProcessingImage"] = "images/error.png";
                    dr["StatusColor"] = "FireBrick";
                    dr["StatusSymbolText"] = "n";

                }

            }

            dgPINSInput.ItemsSource = dtPINS.DefaultView;
            calcTotals();

            setSortColumn(colname, direction, propertyName);
            if (colname != "")
                dgPINSInput.Items.Refresh();
        }

        private void getSortColumn(ref string colname, ref System.ComponentModel.ListSortDirection? direction, ref string propertyName)
        {
            colname = "";
            direction = null;
            propertyName = "";

            foreach (var col in dgPINSInput.Columns)
                if (col.SortDirection != null)
                {
                    colname = col.Header.ToString(); ;
                    direction = col.SortDirection;
                    propertyName = (dgPINSInput.Items.SortDescriptions.Count == 1) ? dgPINSInput.Items.SortDescriptions[0].PropertyName : "";
                    return;
                }
        }

        private void setSortColumn(string colname, System.ComponentModel.ListSortDirection? direction, string propertyName)
        {
            dgPINSInput.Items.SortDescriptions.Clear();
            if (direction != null)
                dgPINSInput.Items.SortDescriptions.Add(new SortDescription(propertyName, (ListSortDirection)direction));

            foreach (var col in dgPINSInput.Columns)
                if (col.Header != null)
                    col.SortDirection = (col.Header.ToString() == colname) ? direction : null;

        }

        private void loadHoldReasons()
        {
            dtHoldReasons = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_CS_GETHOLDREASONS", "OPSCONSOLE").Tables["UM"];
            cbHoldReason.ItemsSource = dtHoldReasons.DefaultView;
        }

        private void loadHoldTypes()
        {
            dtHoldTypes = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_CS_GETHOLDTYPES", "OPSCONSOLE").Tables["UM"];
        }

        public int? lookupHoldType(string value)
        {
            foreach (DataRow dr in dtHoldTypes.Rows)
                if (dr["PaymentHoldType"].ToString().ToUpper() == value.ToUpper())
                    return Convert.ToInt32(dr["PaymentHoldTypeID"]);
            return null;
        }

        public int? lookupHoldReason(string value)
        {
            foreach (DataRow dr in dtHoldReasons.Rows)
                if (dr["PaymentHoldReason"].ToString().ToUpper() == value.ToUpper())
                    return Convert.ToInt32(dr["PaymentHoldReasonID"]);
            return null;
        }

        public void loadADGroups()
        {
            if (dtEmployees.Rows.Count == 0)
            {
                DataTable dtPermissions = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_GET_CEDENT_PERMISSIONS", "OPSCONSOLE").Tables["UM"];
                dtEmployees = ScriptEngine.script.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "SD_GET_ASSOCIATES", "OPSCONSOLE").Tables["WS"];
                foreach (DataRow dr in dtEmployees.Rows)
                {
                    foreach (DataRow drp in dtPermissions.Rows)
                    {
                        if (drp["CedentPermissionSAM"].ToString().ToUpper() == dr["SamAccountName"].ToString().ToUpper())
                            dr["Department"] = drp["SpoofDepartment"].ToString();
                    }
                }
            }

            foreach (DataRow dr in dtEmployees.Rows)
            {
                if (dr["SamAccountName"].ToString().ToUpper() == MainWindow.currentUser.ToUpper().Replace("TRG\\", ""))
                {
                    department = dr["Department"].ToString();
                    lblDepartment.Text = department;
                }
            }
        }

        public DataTable getUsersMatchingFilter(string filter)
        {
            DataRow[] rows = dtEmployees.Select("AdjustedName like'%" + filter + "%'");
            return (rows.Count() == 0) ? dtEmployees.Clone() : rows.CopyToDataTable();
        }


        private void fillHistory()
        {
            if (dgPINSInput.SelectedIndex == -1)
            {
                dgChangeHistory1.ItemsSource = null;
                return;
            }

            DataTable dtHistory = new DataTable();
            dtHistory.Columns.Add("PaymentID", typeof(Int32));
            dtHistory.Columns.Add("PaymentHoldType");
            dtHistory.Columns.Add("PaymentHoldFlg");
            dtHistory.Columns.Add("PaymentHoldReason");
            dtHistory.Columns.Add("ClaimNumber");
            dtHistory.Columns.Add("Comment");
            dtHistory.Columns.Add("StartUser");
            dtHistory.Columns.Add("EndUser");
            dtHistory.Columns.Add("StartDate", typeof(DateTime));
            dtHistory.Columns.Add("EndDate", typeof(DateTime));

            foreach (System.Data.DataRowView dgr in dgPINSInput.SelectedItems)
            {
                foreach (DataRow dr in dtPINSSource.Rows)
                    if (dr["PaymentID"].ToString() == dgr["PaymentID"].ToString())
                    {
                        dtHistory.Rows.Add(dr["PaymentID"].ToString(), dr["PaymentHoldType"].ToString(), dr["PaymentHoldFlg"].ToString(), dr["PaymentHoldReason"].ToString(), dr["ClaimNumber"].ToString(), dr["Comment"].ToString(), dr["StartUser"].ToString(), dr["EndUser"].ToString(), dr["StartDate"], dr["EndDate"]);
                    }
            }


            DataView dv = new DataView();
            if (dtHistory.Rows.Count == 0)
            {
                dgChangeHistory1.ItemsSource = null;
                return;
            }
            dv = dtHistory.DefaultView;
            dv.Sort = "StartDate";

            dgChangeHistory1.ItemsSource = dv;

        }
        #endregion

        #region Screen Layout
        private void adjustElements()
        {
            spFilters.UpdateLayout();
            double extraHeight = (spFilters.ActualHeight <= 0) ? 62d : spFilters.ActualHeight;
            dgPINSInput.Margin = new Thickness(10, extraHeight + 10, 10, 246); // was 216
        }

        public void adjustForDepartment()
        {
            btnReleaseCedentHold.Visibility = Visibility.Collapsed;
            lblWarning.Visibility = Visibility.Collapsed;
            lblComment.Visibility = Visibility.Visible;

            ////// For Scott Marcus and Stephen Boudreau only //////
            if ((department != "Reinsurance") && (department != "Claims"))
            {
                btnHold.Visibility = System.Windows.Visibility.Collapsed;
                btnRelease.Visibility = System.Windows.Visibility.Collapsed;
                cbHoldReason.Visibility = System.Windows.Visibility.Collapsed;
                lblHoldReason.Visibility = System.Windows.Visibility.Collapsed;
                txtComment.Visibility = Visibility.Collapsed;
                lblWarning.Visibility = Visibility.Visible;
                lblComment.Visibility = Visibility.Collapsed;
                return;
            }
            else
            {
                btnHold.Visibility = System.Windows.Visibility.Visible;
                btnRelease.Visibility = System.Windows.Visibility.Visible;
                txtComment.Visibility = Visibility.Visible;
            }

            cbHoldReason.Visibility = (department == "Reinsurance") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            lblHoldReason.Visibility = (department == "Reinsurance") ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
            btnRelease.Content = (department == "Reinsurance") ? "Remove" : "Release";
        }
        #endregion

        #region Commands
        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            adjustElements();
        }

        private void btnHold_Click(object sender, RoutedEventArgs e)
        {
            if (readOnly)
            {
                MessageBox.Show("You have read-only access to this screen. If you need modify access please submit a Service Desk ticket and request Modify access to the Assumed Payments Hold screen in OpsConsole");
                return;
            }

            if (dgPINSInput.SelectedIndex == -1)
            {
                MessageBox.Show("You must select one or more payments to hold");
                return;
            }

            if ((department == "Reinsurance") && (cbHoldReason.SelectedIndex < 0))
            {
                MessageBox.Show("You must select a Hold Reason");
                return;
            }

            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("PaymentHoldID", typeof(Int32));
            dtUpdate.Columns.Add("PaymentID", typeof(Int32));
            dtUpdate.Columns.Add("PaymentHoldTypeID", typeof(Int32));
            dtUpdate.Columns.Add("PaymentHoldReasonID", typeof(Int32));
            dtUpdate.Columns.Add("PaymentHoldFlg", typeof(bool));
            dtUpdate.Columns.Add("Comment");
            dtUpdate.Columns.Add("Operation");
            dtUpdate.TableName = "PaymentHoldTable";

            string operation="U";
            int? HoldType = lookupHoldType(department);
            int? HoldReason = (cbHoldReason.SelectedIndex < 0) ? lookupHoldReason("Other") : Convert.ToInt32(((System.Data.DataRowView)cbHoldReason.SelectedItem)["PaymentHoldReasonID"].ToString());
            string comment = txtComment.Text;

            foreach (System.Data.DataRowView dgr in dgPINSInput.SelectedItems)
            {
                foreach (DataRow dr in dtPINS.Rows)
                    if (dr["PaymentID"].ToString() == dgr["PaymentID"].ToString())
                    {
                        //dtUpdate.Rows.Add(Convert.ToInt32(dr["PaymentHoldID"].ToString()), dr["PaymentID"].ToString(), HoldType, HoldReason, true, comment, "D");
                        dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), HoldType, HoldReason, true, comment, "X");
                        dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), HoldType, HoldReason, true, comment, operation);
                    }
            }


            ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, "AP_CRUD_PAYMENT_HOLD", "OPSCONSOLE");

            dgPINSInput.SelectedIndex = -1;
            loadConformed();
            //calcTotalOnHold();
            calcTotals();
            txtComment.Text = "";
        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            if (readOnly)
            {
                MessageBox.Show("You have read-only access to this screen. If you need modify access please submit a Service Desk ticket and request Modify access to the Assumed Payments Hold screen in OpsConsole");
                return;
            }
            if (dgPINSInput.SelectedIndex == -1)
            {
                MessageBox.Show("You must select one or more payments");
                return;
            }

            txtComment.Text = "";
            bool bWarnedThemOnce = false;

            DataTable dtUpdate = new DataTable();
            dtUpdate.Columns.Add("PaymentHoldID", typeof(Int32));
            dtUpdate.Columns.Add("PaymentID", typeof(Int32));
            dtUpdate.Columns.Add("PaymentHoldTypeID", typeof(Int32));
            dtUpdate.Columns.Add("PaymentHoldReasonID", typeof(Int32));
            dtUpdate.Columns.Add("PaymentHoldFlg", typeof(bool));
            dtUpdate.Columns.Add("Comment");
            dtUpdate.Columns.Add("Operation");
            dtUpdate.TableName = "PaymentHoldTable";

            string operation = "U";
            int? HoldType = lookupHoldType(department);
            int? HoldReason = lookupHoldReason("Released");
            string comment = "";


            ////// DON'T ALLOW RELEASE OF LARGE TRANSACTION AMMOUNT IF WE DON'T HAVE SPECIAL PERMISSION //////
            bool foundLTA = false;
            foreach (System.Data.DataRowView dgr in dgPINSInput.SelectedItems)
                if (dgr["PaymentHoldReason"].ToString().StartsWith("Large"))
                    foundLTA = true;
            if ((foundLTA) && (!canUserReleaseLarge()))
            {
                MessageBox.Show("Only one of the following users can release a hold of Large Transaction Amount:" + Environment.NewLine + Environment.NewLine + usersWhoCanReleaseLTA(), "These holds cannot be released because there is a Large Transaction Amount Hold");
                return;
            }


            foreach (System.Data.DataRowView dgr in dgPINSInput.SelectedItems)
            {
                foreach (DataRow dr in dtPINS.Rows)
                    if (dr["PaymentID"].ToString() == dgr["PaymentID"].ToString())
                    {
                        ////// Reinsurance cannot release a claims hold //////
                        if ((dr["PaymentHoldType"].ToString() == "Claims") && (department == "Reinsurance"))
                        {
                            if (!bWarnedThemOnce)
                                MessageBox.Show("Reinsurance cannot release the Claims holds");
                            bWarnedThemOnce = true;
                        }

                        // New business rules change 2/4/16 - When releasing a reinsurance hold, create a claims hold
                        else if (department == "Reinsurance")
                        {
                            dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), HoldType, HoldReason, true, comment, "X");
                            // Monday July 25 2016 - Changed HoldReason from "Released" to "Other"
                            dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), lookupHoldType("Claims"), lookupHoldReason("Other"), true, comment, operation);
                        }

                        // Monday July 25 2016 - Look for Claims explicitly so that people who aren't in Reinsurance or Claims get nothing
                        else if (department == "Claims")
                        {
                            //dtUpdate.Rows.Add(Convert.ToInt32(dr["PaymentHoldID"].ToString()), dr["PaymentID"].ToString(), HoldType, HoldReason, true, comment, "D");
                            dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), HoldType, HoldReason, true, comment, "X");
                            dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), HoldType, HoldReason, false, comment, operation);
                        }

                        // Monday July 25 2016
                        else
                        {
                            MessageBox.Show("Your department is: " + department + " and it must be \"Claims\" or \"Reinsurance\"");
                        }
                    }
            }


            ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, "AP_CRUD_PAYMENT_HOLD", "OPSCONSOLE");

            dgPINSInput.SelectedIndex = -1;
            loadConformed();
            calcTotals();
        }


        private bool canUserReleaseLarge()
        {
            string user = MainWindow.ourMainWindow.lblCurrentUser.Text.ToUpper().Replace("TRG\\", "");

            foreach (DataRow dr in MainWindow.ourMainWindow.screenManageCeded.dtPermissions.Rows)
                if (dr["CedentPermissionSAM"].ToString().ToUpper() == user)
                    return (dr["CedentPermissionReleaseLTA"].ToString() == "True") ? true : false;
            return false;
        }

        private string usersWhoCanReleaseLTA()
        {
            string result = "";

            foreach (DataRow dr in MainWindow.ourMainWindow.screenManageCeded.dtPermissions.Rows)
                if (dr["CedentPermissionReleaseLTA"].ToString() == "True")
                    result += getFullName(dr["CedentPermissionSAM"].ToString()) + Environment.NewLine;

            return result;
        }

        private string getFullName(string SAM)
        {
            foreach (DataRow dr in dtEmployees.Rows)
                if (dr["SamAccountName"].ToString().ToUpper() == SAM.ToUpper().Replace("TRG\\", ""))
                    return dr["AdjustedName"].ToString();
            return "";
        }


        private void filter1_FilterClicked(object sender, PaymentSearchFilter.FilterEventArgs e)
        {
            if (e.action == "AND")
            {
                filter2.Visibility = System.Windows.Visibility.Visible;
                filter1.hideSearchButton(hide: true);
                adjustElements();
            }

            if (e.action == "SEARCH")
            {
                loadConformed();
            }

            if (e.action == "CLEAR")
            {
                filter2.Visibility = System.Windows.Visibility.Collapsed;
                filter1.hideSearchButton(hide: false);
                filter2.reset();
                adjustElements();
                loadConformed();
            }

            if (e.action == "SIZECHANGED")
            {
                adjustElements();
            }
        }

        private void filter2_FilterClicked(object sender, PaymentSearchFilter.FilterEventArgs e)
        {
            if (e.action == "CLOSE")
            {
                filter2.Visibility = System.Windows.Visibility.Collapsed;
                filter1.hideSearchButton(hide: false);
                filter2.reset();
                adjustElements();
            }

            if (e.action == "SIZECHANGED")
            {
                adjustElements();
            }

            if (e.action == "SEARCH")
            {
                loadConformed();
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ourMainWindow.showMainScreen();
        }

        private void btnExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            ////// Get file name //////
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "holds.csv"; // Default file name
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "Excel spreadsheet (.csv)|*.csv"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

                //<DataGridTextColumn Header="Claim No" Width="82" Binding="{Binding ClaimNumber}"/>
                //<DataGridTextColumn Header="Hold Type" Width="80" Binding="{Binding HoldType}"/>
                //<DataGridTextColumn Header="Hold Reason" Width="80" Binding="{Binding HoldReason}"/>
                //<DataGridTextColumn Header="Vendor ID" Width="70" Binding="{Binding CMVendorID}"/>
                //<DataGridTextColumn Header="Broker" Width="200" Binding="{Binding MappedVendorName}"/>
                //<DataGridTextColumn Header="Ceding Company" Width="190" Binding="{Binding CedingCompany}"/>
                //<DataGridTextColumn Header="Currency" Width="59" Binding="{Binding ReportingCurrency}"/>
                //<DataGridTextColumn Header="Amount" Width="80" Binding="{Binding PADouble, StringFormat=\{0:N2\}}">
                //</DataGridTextColumn>
                //<DataGridTextColumn Header="Date of Loss" Width="90" Binding="{Binding DateOfLoss, StringFormat=\{0:MM/dd/yyyy\}}"/>
                //<DataGridTextColumn Header="Agency Code" Width="80" Binding="{Binding PALAffiliateName}"/>

            // Process save file dialog box results
            if (result == true)
            {
                using (StreamWriter sw = new StreamWriter(dlg.FileName)) 
                {
                    string line = "";
    
                    line = "\"" + "CMVendorID" + "\",";
                    line += "\"" + "VendorName" + "\",";
                    line += "\"" + "Broker" + "\",";
                    line += "\"" + "CedingCompany" + "\",";
                    line += "\"" + "ClaimNumber" + "\",";
                    line += "\"" + "Insured" + "\",";
                    line += "\"" + "RiskMovementID" + "\",";
                    line += "\"" + "ReferenceNumber" + "\",";
                    line += "\"" + "DateOfLoss" + "\",";
                    line += "\"" + "Currency" + "\",";
                    line += "\"" + "Payment Amount" + "\",";
                    line += "\"" + "AffiliateName" + "\",";
                    line += "\"" + "PaymentHoldType" + "\",";
                    line += "\"" + "PaymentHoldReason" + "\",";
                    line += "\"" + "CedentID" + "\",";
                    line += "\"" + "Comment" + "\",";

                    sw.WriteLine(line);


                    foreach (DataRow dr in dtPINS.Rows)
                    {
                        line = "\"" + dr["CMVendorID"].ToString() + "\",";
                        line += "\"" + dr["VendorName"].ToString() + "\",";
                        line += "\"" + dr["BrokerName"].ToString() + "\",";
                        line += "\"" + dr["CedingCompany"].ToString() + "\",";
                        line += "\"" + dr["ClaimNumber"].ToString() + "\",";
                        line += "\"" + dr["Insured"].ToString() + "\",";
                        line += "\"" + dr["RiskMovementID"].ToString() + "\",";
                        line += "\"" + dr["ReferenceNumber"].ToString() + "\",";
                        line += "\"" + dr["DateOfLoss"].ToString() + "\",";
                        line += "\"" + dr["ReportingCurrency"].ToString() + "\",";
                        line += dr["PADouble"].ToString() + ",";
                        line += "\"" + dr["PALAffiliateName"].ToString() + "\",";
                        line += "\"" + dr["PaymentHoldType"].ToString() + "\",";
                        line += "\"" + dr["PaymentHoldReason"].ToString() + "\",";
                        line += "\"" + dr["Insurer_ID"].ToString() + "\",";
                        line += "\"" + dr["Comment"].ToString().Replace("\"","\"\"") + "\",";

                        sw.WriteLine(line);
                    }
                }
            }
        }
        #endregion

        #region Calucations
        private void calcTotalForPage()
        {
            double total = 0d;
            foreach (DataRow dr in dtPINS.Rows)
                total += Convert.ToDouble(dr["TotalPaymentAmount"].ToString());

            lblTotalGrid.Text = "$" + total.ToString("#,##0.00");
        }

        private void calcTotalOnHold()
        {
            double total = 0d;

            foreach (System.Data.DataRowView dgr in dgPINSInput.Items)
            {
                if (dgr["Background"].ToString() == "LightPink")
                    total += Convert.ToDouble(dgr["TotalPaymentAmount"].ToString());
            }

            lblTotalOnHold.Text = "$" + total.ToString("#,##0.00");
        }

        private void calcTotalSelected()
        {
            double total = 0d;

            foreach (System.Data.DataRowView dgr in dgPINSInput.SelectedItems)
            {
                total += Convert.ToDouble(dgr["TotalPaymentAmount"].ToString());
            }

            lblTotalSelected.Text = "$" + total.ToString("#,##0.00");
        }

        private void calcTotals()
        {
            ////// Create a data table //////
            dtTotals = new DataTable();
            dtTotals.Columns.Add("description");
            dtTotals.Columns.Add("value1");
            dtTotals.Columns.Add("value2");
            dtTotals.Columns.Add("value3");
            dtTotals.Columns.Add("value4");
            dtTotals.Columns.Add("value5");
            dtTotals.Rows.Add("Totals", "", "", "", "", "");
            dtTotals.Rows.Add("Entire grid");
            dtTotals.Rows.Add("-On Hold");
            dtTotals.Rows.Add("=Released");
            dtTotals.Rows.Add("Selected");
            dtTotals.Rows.Add("Total selected");

            ////// Make sure every currency is represented //////
            foreach (DataRow dr in dtPINS.Rows)
                getCurrencyColumn(dr["CurrencyCode"].ToString());

            ////// Grid totals //////
            foreach (DataRow dr in dtPINS.Rows)
            {
                string cur = dr["CurrencyCode"].ToString();
                int column = getCurrencyColumn(dr["CurrencyCode"].ToString());
                if (column >= 0)
                    dtTotals.Rows[rowGridTotal][column] = currencySymbol(cur) + (Convert.ToDouble(dtTotals.Rows[rowGridTotal][column].ToString().Replace("$", "").Replace(",", "").Replace("£", "")) + Convert.ToDouble(dr["TotalPaymentAmount"].ToString())).ToString("#,##0.00");
            }

            ////// On hold totals //////
            foreach (System.Data.DataRowView dgr in dgPINSInput.Items)
            {
                if (dgr["Background"].ToString() == claimsColor || dgr["Background"].ToString() == reinsColor)
                {
                    int column = getCurrencyColumn(dgr["CurrencyCode"].ToString());
                    string cur = dgr["CurrencyCode"].ToString();
                    if (column >= 0)
                        dtTotals.Rows[rowOnHoldTotal][column] = currencySymbol(cur) + (Convert.ToDouble(dtTotals.Rows[rowOnHoldTotal][column].ToString().Replace("$", "").Replace(",", "").Replace("£", "")) + Convert.ToDouble(dgr["TotalPaymentAmount"].ToString())).ToString("#,##0.00");
                }
            }

            ////// Selected totals //////
            foreach (System.Data.DataRowView dgr in dgPINSInput.SelectedItems)
            {
                int column = getCurrencyColumn(dgr["CurrencyCode"].ToString());
                string cur = dgr["CurrencyCode"].ToString();
                if (column >= 0)
                {
                    dtTotals.Rows[rowSelectedTotal][column] = currencySymbol(cur) + (Convert.ToDouble(dtTotals.Rows[rowSelectedTotal][column].ToString().Replace("$", "").Replace(",", "").Replace("£", "")) + Convert.ToDouble(dgr["TotalPaymentAmount"].ToString())).ToString("#,##0.00");
                    dtTotals.Rows[rowCount][column] = (Convert.ToDouble(dtTotals.Rows[rowCount][column].ToString()) + 1).ToString();
                }
            }

            ////// Calculate released //////
            for (int i = 1; i <= maxCurrencyColumns; i++)
                if (dtTotals.Rows[0][i].ToString() != "")
                {
                    string cur = dtTotals.Rows[0][i].ToString();
                    dtTotals.Rows[rowReleasedTotal][i] = currencySymbol(cur) + (Convert.ToDouble(dtTotals.Rows[rowGridTotal][i].ToString().Replace("$", "").Replace(",", "").Replace("£", "")) - (Convert.ToDouble(dtTotals.Rows[rowOnHoldTotal][i].ToString().Replace("$", "").Replace(",", "").Replace("£", "")))).ToString("#,##0.00");
                }

            ////// Calculate grand totals //////
            //if (dtTotals.Rows[0][2].ToString() != "")
            //{
            //    int totalcolumn = getCurrencyColumn(dgr[""].ToString());
            //}

            dgTotals.ItemsSource = dtTotals.DefaultView;
        }

        private int getCurrencyColumn(string currencyCode)
        {
            ////// Return the colummn if we have it //////
            for (int i = 1; i <= maxCurrencyColumns; i++)
                if (dtTotals.Rows[0][i].ToString() == currencyCode)
                    return i;

            ////// Create the column if we don't //////
            for (int i = 1; i <= maxCurrencyColumns; i++)
                if (dtTotals.Rows[0][i].ToString() == "")
                {
                    dtTotals.Rows[0][i] = currencyCode;
                    dtTotals.Rows[1][i] = currencySymbol(currencyCode) + "0.00";
                    dtTotals.Rows[2][i] = currencySymbol(currencyCode) + "0.00";
                    dtTotals.Rows[3][i] = currencySymbol(currencyCode) + "0.00";
                    dtTotals.Rows[4][i] = currencySymbol(currencyCode) + "0.00";
                    dtTotals.Rows[5][i] = "0";
                    return i;
                }

            ////// No room at the inn //////
            return -1;
        }

        private string currencySymbol(string currencyCode)
        {
            if (currencyCode.ToUpper() == "USD")
                return "$";
            if (currencyCode.ToUpper() == "CAD")
                return "$";
            if (currencyCode.ToUpper() == "GBP")
                return "£";
            return "";
        }
        #endregion

        #region Events
        private void dgPINSInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            calcTotalSelected();
            calcTotals();
            fillHistory();
        }

        private void lblDepartment_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
        }

        private void lbForceDepartmentChangeForQA_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lbForceDepartmentChangeForQA.Visibility = System.Windows.Visibility.Collapsed;
            if (lbForceDepartmentChangeForQA.SelectedIndex < 0)
                return;
            department = ((ListBoxItem)lbForceDepartmentChangeForQA.SelectedItem).Content.ToString();
            lblDepartment.Text = department;
            adjustForDepartment();
        }

        private void lbForceDepartmentChangeForQA_LostFocus(object sender, RoutedEventArgs e)
        {
            lbForceDepartmentChangeForQA.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void dgTotals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dgTotals.UnselectAll();
        }

        private void lblDepartment_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            //string currentUser = MainWindow.currentUser.ToUpper().Replace("TRG\\", "");
            //if ((currentUser == "SBOUD") || (currentUser == "SMARC") || (currentUser == "KMOON") || (currentUser == "RMEIE"))
            //{
            //    lbForceDepartmentChangeForQA.SelectedIndex = -1;
            //    lbForceDepartmentChangeForQA.Visibility = System.Windows.Visibility.Visible;
            //}
        }

        private void filter1_Loaded(object sender, RoutedEventArgs e)
        {
        }
        #endregion

    }
}
