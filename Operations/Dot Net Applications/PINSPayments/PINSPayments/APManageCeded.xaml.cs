using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for APManageCeded.xaml
    /// </summary>
    public partial class APManageCeded : UserControl
    {
        #region Class data
        DataTable dtCedentHold = new DataTable();
        public DataTable dtPermissions = new DataTable();
        DataTable dtHoldReasons = new DataTable();
        #endregion

        public APManageCeded()
        {
            InitializeComponent();
        }

        public void AdjustPermissionsForUser()
        {
            string user = MainWindow.ourMainWindow.lblCurrentUser.Text.ToUpper().Replace("TRG\\", "");
            string CededPermissionAdd = "";
            string CededPermissionDelete = "";

            foreach (DataRow dr in dtPermissions.Rows)
            {
                if (dr["CedentPermissionSAM"].ToString().ToUpper() == user)
                {
                    CededPermissionAdd = dr["CedentPermissionAdd"].ToString();
                    CededPermissionDelete = dr["CedentPermissionDelete"].ToString();
                }
            }

            cbHoldReason.Visibility = (CededPermissionAdd == "True") ? Visibility.Visible : Visibility.Collapsed;
            lblHoldReason.Visibility = (CededPermissionAdd == "True") ? Visibility.Visible : Visibility.Collapsed;
            btnHold.Visibility = (CededPermissionAdd == "True") ? Visibility.Visible : Visibility.Collapsed;
            btnRelease.Visibility = (CededPermissionDelete == "True") ? Visibility.Visible : Visibility.Collapsed;
        }

        public void Load(string permissions)
        {
            UIHelpers.showRadioButtonStatus(btnAll, new Button[] { btnAll, btnHeld, btnReleased });
            loadCededHold();
            AdjustPermissionsForUser();
        }

        #region Load data
        private void loadCededHold()
        {
            dtCedentHold = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_GET_CEDENT_HOLD", "OPSCONSOLE").Tables["UM"];
            dtPermissions = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_GET_CEDENT_PERMISSIONS", "OPSCONSOLE").Tables["UM"];
            dtHoldReasons = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_GET_CEDENT_HOLD_REASONS", "OPSCONSOLE").Tables["UM"];
            //dgCedents.AutoGenerateColumns = true;
            cbHoldReason.ItemsSource = dtHoldReasons.Select("CedentCompanyHoldReason <> 'Initial Hold'").CopyToDataTable().DefaultView;

            applyFilters();
        }

        private void applyFilters()
        {
            string holdFilter = UIHelpers.getRadioButtonStatus(new Button[] { btnAll, btnHeld, btnReleased });
            string andFilter = "";
            if (holdFilter == "btnHeld")
                andFilter = " and (CedentHoldPayment=1)";
            if (holdFilter == "btnReleased")
                andFilter = " and (CedentHoldPayment=0)";
            if (rectInactive.Opacity < 0.3d)
                andFilter += " and (CedentActive=1)";

            DataRow[] dtIdeasFiltered = dtCedentHold.Select("(CedentCompany like '%" + txtFilter.Text + "%')" + andFilter);
            DataView dv = new DataView();
            if (dtIdeasFiltered.Count() == 0)
            {
                dgCedents.ItemsSource = null;
                return;
            }
            dv = dtIdeasFiltered.CopyToDataTable().DefaultView;
            dv.Sort = "CedentCompany";
            dgCedents.ItemsSource = dv;

            //dgCedents.ItemsSource = dtCedentHold.DefaultView;

        }



        #endregion

        private void btnHold_Click(object sender, RoutedEventArgs e)
        {
            if (cbHoldReason.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a hold reason");
                return;
            }

            if (dgCedents.SelectedIndex < 0)
            {
                MessageBox.Show("You must select one or more Cedents");
                return;
            }

            foreach (System.Data.DataRowView dgr in dgCedents.SelectedItems)
            {
                string cedentID = dgr["CedentCompanyID"].ToString();
                string cedentCompanyHoldID = dgr["CedentCompanyHoldID"].ToString();
                string cedentCompany = dgr["CedentCompany"].ToString();
                string cedentActive = dgr["CedentActive"].ToString();
                int holdReason = Convert.ToInt32(((System.Data.DataRowView)cbHoldReason.SelectedItem)["CedentCompanyHoldReasonID"].ToString());

                ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_UPDATE_CEDENT_HOLD", "OPSCONSOLE", false,
                    "@CedentCompanyID", cedentID,
                    "@CedentCompany", cedentCompany,
                    "@CedentHoldPayment", "True",
                    "@CedentCompanyHoldReasonID", holdReason.ToString(),
                    "@CedentActive", cedentActive,
                    "@ModifiedBy", MainWindow.currentUser);


                placeOnCedentHold(cedentID);
            }


            loadCededHold();

        }

        private void placeOnCedentHold(string cedentID)
        {

            DataTable dtPayments = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_CS_GETPAYMENTS_FOR_CEDENT_ID", "OPSCONSOLE",false, "@CedentCompanyID", cedentID).Tables["UM"];

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
            int? HoldType = MainWindow.ourMainWindow.screenHold.lookupHoldType("Cedent Company");
            int? HoldReason = MainWindow.ourMainWindow.screenHold.lookupHoldReason("Cedent Company Hold");
            string comment = "Cedent placed on hold";

            foreach (DataRow dr in dtPayments.Rows)
            {
                dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), HoldType, HoldReason, true, comment, "X");
                dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), HoldType, HoldReason, true, comment, operation);
            }

            ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, "AP_CRUD_PAYMENT_HOLD", "OPSCONSOLE");

            MainWindow.ourMainWindow.screenHold.Load("");
            //loadConformed();
            //calcTotals();
            //txtComment.Text = "";
        }

        private void removeFromCedentHold(string cedentID, string cedentCompanyHoldID)
        {
            DataTable dtPayments = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_CS_GETPAYMENTS_FOR_CEDENT_ID", "OPSCONSOLE", false, "@CedentCompanyID", cedentID).Tables["UM"];

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
            //int? HoldType = MainWindow.ourMainWindow.screenHold.lookupHoldType("Cedent Company");
            //int? HoldReason = MainWindow.ourMainWindow.screenHold.lookupHoldReason("Cedent Company Hold");
            //string comment = "Cedent hold releasted";

            foreach (DataRow dr in dtPayments.Rows)
            {
                ////// GET HISTORY FOR THIS PAYMENT //////
                DataRow drHistory = getHistory(dr["PaymentID"].ToString());
                string initialHoldReason = MainWindow.ourMainWindow.screenHold.lookupHoldReason("Initial Hold").ToString();
                string releasedReason = MainWindow.ourMainWindow.screenHold.lookupHoldReason("Released").ToString();


                if (drHistory != null)
                {
                    string strHoldReason = drHistory["PaymentHoldReasonID"].ToString();
                    string strHoldType = drHistory["PaymentHoldTypeID"].ToString();
                    string comment = drHistory["Comment"].ToString();

                    ////// IF THE PREVIOUS HOLD WAS AN INITIAL HOLD OR NO HOLD, RELEASE IT //////
                    if ((strHoldType == "") || (strHoldReason == initialHoldReason) || (strHoldReason == releasedReason))
                    {
                        int? HoldReasonReleased = MainWindow.ourMainWindow.screenHold.lookupHoldReason("Released");
                        int? HoldTypeReleased = MainWindow.ourMainWindow.screenHold.lookupHoldType("Claims");

                        dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), strHoldType, strHoldReason, true, comment, "X");
                        dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), HoldTypeReleased, HoldReasonReleased, false, comment, operation);
                    }

                    ////// OTHERWISE, REVERT TO PREVIOIUS HOLD //////
                    else
                    {
                        dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), strHoldType, strHoldReason, true, comment, "X");
                        dtUpdate.Rows.Add(0, dr["PaymentID"].ToString(), strHoldType, strHoldReason, true, comment, operation);
                    }


                }
            }

            
            ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, "AP_CRUD_PAYMENT_HOLD", "OPSCONSOLE");
            MainWindow.ourMainWindow.screenHold.Load("");
            //loadConformed();
            //calcTotals();
            //txtComment.Text = "";
        }


        private DataRow getHistory(string PaymentID)
        {
            int cedentHoldType = (int) MainWindow.ourMainWindow.screenHold.lookupHoldType("Cedent Company");
            DataTable dtHistory = MainWindow.ourMainWindow.screenHold.dtPINSSource.Select("PaymentID=" + PaymentID + " and PaymentHoldTypeID<>" + cedentHoldType.ToString() + " and EndDate is not null").CopyToDataTable();
            dtHistory.DefaultView.Sort = "StartDate desc";
            dtHistory = dtHistory.DefaultView.ToTable();

            if (dtHistory.Rows.Count > 0)
                return dtHistory.Rows[0];
            else
                return null;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            applyFilters();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtFilter.Text = "";
        }

        private void btnReleased_Click(object sender, RoutedEventArgs e)
        {
            UIHelpers.showRadioButtonStatus(btnReleased, new Button[] { btnAll, btnHeld, btnReleased });
            applyFilters();
        }

        private void btnHeld_Click(object sender, RoutedEventArgs e)
        {
            UIHelpers.showRadioButtonStatus(btnHeld, new Button[] { btnAll, btnHeld, btnReleased });
            applyFilters();
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            UIHelpers.showRadioButtonStatus(btnAll, new Button[] { btnAll, btnHeld, btnReleased });
            applyFilters();
        }

        private void btnRelease_Click(object sender, RoutedEventArgs e)
        {
            if (dgCedents.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a Cedent");
                return;
            }

            foreach (System.Data.DataRowView dgr in dgCedents.SelectedItems)
            {
                string cedentID = dgr["CedentCompanyID"].ToString();
                string cedentCompanyHoldID = dgr["CedentCompanyHoldID"].ToString();
                string cedentCompany = dgr["CedentCompany"].ToString();
                string cedentActive = dgr["CedentActive"].ToString();
                int holdReason = 0;

                // FIX REMEMBER TO UNCOMMENT
                ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_UPDATE_CEDENT_HOLD", "OPSCONSOLE", false,
                    "@CedentCompanyID", cedentID,
                    "@CedentCompany", cedentCompany,
                    "@CedentHoldPayment", "False",
                    "@CedentCompanyHoldReasonID", holdReason.ToString(),
                    "@CedentActive", cedentActive,
                    "@ModifiedBy", MainWindow.currentUser);

                removeFromCedentHold(cedentID, cedentCompanyHoldID);
            }


            loadCededHold();
        }

        private void dgCedents_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCedents.SelectedItems.Count != 1)
            {
                dgChangeHistory1.ItemsSource = null;
                return;
            }

            string cedentCompanyHoldID = ((System.Data.DataRowView) dgCedents.SelectedItems[0])["CedentCompanyHoldID"].ToString();

            DataTable dtCedentHoldHistory = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_GET_CEDENT_HISTORY", "OPSCONSOLE",false, "@CedentCompanyHoldID", cedentCompanyHoldID).Tables["UM"];
            dgChangeHistory1.ItemsSource = dtCedentHoldHistory.DefaultView;
        }

        private void btnShowInactive_Click(object sender, RoutedEventArgs e)
        {
            rectInactive.Opacity = (rectInactive.Opacity < 0.3d) ? 1d : 0.2d;
            applyFilters();

        }

        private void btnSetActive_Click(object sender, RoutedEventArgs e)
        {
            setSelectedToActive("True");
        }

        private void btnSetInactive_Click(object sender, RoutedEventArgs e)
        {
            setSelectedToActive("False");
        }

        private void setSelectedToActive(string cedentActive)
        {
            if (dgCedents.SelectedIndex < 0)
            {
                MessageBox.Show("You must select one or more Cedents");
                return;
            }

            foreach (System.Data.DataRowView dgr in dgCedents.SelectedItems)
            {
                string cedentID = dgr["CedentCompanyID"].ToString();
                string cedentCompanyHoldID = dgr["CedentCompanyHoldID"].ToString();
                string cedentCompany = dgr["CedentCompany"].ToString();
                string holdReason = dgr["CedentCompanyHoldReasonID"].ToString();
                string cedentHoldPayment = dgr["CedentHoldPayment"].ToString();

                ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_UPDATE_CEDENT_HOLD", "OPSCONSOLE", false,
                    "@CedentCompanyID", cedentID,
                    "@CedentCompany", cedentCompany,
                    "@CedentHoldPayment", cedentHoldPayment,
                    "@CedentCompanyHoldReasonID", (holdReason=="") ? "-1" : holdReason,
                    "@CedentActive", cedentActive,
                    "@ModifiedBy", MainWindow.currentUser);
            }

            loadCededHold();
        }

    }
}