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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PINSPayments
{
    /// <summary>
    /// Interaction logic for APPayments.xaml
    /// </summary>
    public partial class APPayments : UserControl
    {
        #region Class data
        DataTable dtNavDataUSA = new DataTable();
        DataTable dtNavDataCAD = new DataTable();
        DataTable dtNavData = new DataTable();
        DataSet dsNavData = new DataSet();
        DataTable dtPINSCopy = new DataTable();
        #endregion


        public APPayments()
        {
            InitializeComponent();
        }


        public void Load()
        {
            UIHelpers.showRadioButtonStatus(btnAll, new Button[] { btnAll, btnCleared, btnIssued, btnVoided });

            ////// If we got it once, we'll assume it's up to date enough, add a refresh button in V2 ///////
            if (dtNavDataUSA.Rows.Count > 0)
                return;

            ////// Get the US and Canadian payments from NAV //////
            dsNavData = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_GET_NAV", "OPSCONSOLE");
            dtNavDataUSA = dsNavData.Tables["USD"];
            dtNavDataCAD = dsNavData.Tables["CAD"];

            dtNavData = dtNavDataUSA.Copy();
            dtNavData.Merge(dtNavDataCAD);

            dtNavData.Columns.Add("ClaimNumber");
            dtNavData.Columns.Add("VendorName");
            dtNavData.Columns.Add("CedingCompany");
            dtNavData.Columns.Add("Insured");
            dtNavData.Columns.Add("ReferenceNumber");
            dtNavData.Columns.Add("BrokerName");
            dtNavData.Columns.Add("DateOfLoss", typeof(DateTime));

            dtPINSCopy = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AP_CS_GETALLPAYMENTS", "OPSCONSOLE").Tables["UM"];
            DataTable dtPinsPresentation = dtPINSCopy.Clone();

            DataView dvLookup = new DataView(dtPINSCopy);
            dvLookup.Sort = "PaymentID";

            foreach (DataRow dr in dtNavData.Rows)
            {
                string conformedID = dr["ConformedPaymentID"].ToString();

                int index = dvLookup.Find(conformedID);
                if (index >= 0)
                {
                    dr["ClaimNumber"] = dvLookup[index]["ClaimNumber"].ToString();
                    dr["VendorName"] = dvLookup[index]["VendorName"].ToString();
                    dr["CedingCompany"] = dvLookup[index]["CedingCompany"].ToString();
                    dr["Insured"] = dvLookup[index]["Insured"].ToString();
                    dr["DateOfLoss"] = dvLookup[index]["DateOfLoss"].ToString();
                    dr["BrokerName"] = dvLookup[index]["BrokerName"].ToString();
                    dr["ReferenceNumber"] = dvLookup[index]["ReferenceNumber"].ToString();
                }
            }

            filterData();
        }

        private void filterData()
        {
            DataTable dtNavFiltered = dtNavData.Clone();

            foreach (DataRow dr in dtNavData.Rows)
            {
                if (dr["ClaimNumber"].ToString() != "")
                {
                    string action = dr["Action"].ToString();
                    string actionFilter = UIHelpers.getRadioButtonStatus(new Button[] { btnAll, btnCleared, btnIssued, btnVoided });
                    string textFilter = txtFilter.Text.ToUpper();

                    if ((actionFilter == "btnCleared") && (action != "Cleared"))
                        continue;

                    if ((actionFilter == "btnIssued") && (action != "Issued"))
                        continue;

                    if ((actionFilter == "btnVoided") && (action != "Voided"))
                        continue;

                    if (textFilter != "")
                    {
                        string claim = dr["ClaimNumber"].ToString().ToUpper();
                        string vendor = dr["VendorName"].ToString().ToUpper();
                        string ceding = dr["CedingCompany"].ToString().ToUpper();
                        string insured = dr["Insured"].ToString().ToUpper();
                        string refno = dr["Reference Number"].ToString().ToUpper();

                        if ((claim.IndexOf(textFilter) < 0) &&
                            (vendor.IndexOf(textFilter) < 0) &&
                            (ceding.IndexOf(textFilter) < 0) &&
                            (insured.IndexOf(textFilter) < 0) &&
                            (refno.IndexOf(textFilter) < 0))
                            continue;

                    }

                    dtNavFiltered.ImportRow(dr);
                }
            }

            dgPayments.ItemsSource = dtNavFiltered.DefaultView;

        }


        private DataRow findPayment(string riskMovementID)
        {
            foreach (DataRow dr in MainWindow.ourMainWindow.screenHold.dtPINSSource.Rows)
            {
                if (dr["RiskMovementID"].ToString() == riskMovementID)
                    return dr;
            }

            return null;
        }

        private void txtFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterData();
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            txtFilter.Text = "";
        }

        private void btnVoided_Click(object sender, RoutedEventArgs e)
        {
            UIHelpers.showRadioButtonStatus(btnVoided, new Button[] { btnAll, btnCleared, btnIssued, btnVoided });
            filterData();
        }

        private void btnAll_Click(object sender, RoutedEventArgs e)
        {
            UIHelpers.showRadioButtonStatus(btnAll, new Button[] { btnAll, btnCleared, btnIssued, btnVoided });
            filterData();
        }

        private void btnCleared_Click(object sender, RoutedEventArgs e)
        {
            UIHelpers.showRadioButtonStatus(btnCleared, new Button[] { btnAll, btnCleared, btnIssued, btnVoided });
            filterData();
        }

        private void btnIssued_Click(object sender, RoutedEventArgs e)
        {
            UIHelpers.showRadioButtonStatus(btnIssued, new Button[] { btnAll, btnCleared, btnIssued, btnVoided });
            filterData();
        }
    }
}
