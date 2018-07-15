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

namespace PINSPayments
{
    /// <summary>
    /// Interaction logic for PaymentSearchFilter.xaml
    /// </summary>
    public partial class PaymentSearchFilter : UserControl
    {
        // EVENT
        // action = CLOSE, SEARCH, AND or SIZECHANGED
        public class FilterEventArgs : EventArgs
        {
            public string ID;
            public string action;
        }
        public event EventHandler<FilterEventArgs> FilterClicked;


        public PaymentSearchFilter()
        {
            InitializeComponent();
        }

        public void hideClose()
        {
            lblClose.Visibility = System.Windows.Visibility.Collapsed;
        }

        public void hideSearchButton(bool hide)
        {
            btnSearch.Visibility = (hide) ? System.Windows.Visibility.Collapsed : Visibility.Visible;
        }

        public string getSearchString()
        {
            // if ((txtValue1.Text == "") || (cbFilterOn1.SelectedIndex < 0) || (Visibility != System.Windows.Visibility.Visible))
            if ((cbFilterOn1.SelectedIndex < 0) || (Visibility != System.Windows.Visibility.Visible))
                    return " (1=1) ";

            string sql = "( " + buildPartialSearchString(1);
            if (txtValue2.Text != "")
                sql += " or " + buildPartialSearchString(2);
            if (txtValue3.Text != "")
                sql += " or " + buildPartialSearchString(3);
            sql += ")";
            return sql;
        }

        public string buildPartialSearchString(int line)
        {
            ComboBox cbFilterOn = null;
            ComboBox cbCondition = null;
            TextBox txtValue = null;

            if (line == 1)
            {
                cbFilterOn = cbFilterOn1;
                cbCondition = cbCondition1;
                txtValue = txtValue1;
            }

            if (line == 2)
            {
                cbFilterOn = cbFilterOn2;
                cbCondition = cbCondition2;
                txtValue = txtValue2;
            }

            if (line == 3)
            {
                cbFilterOn = cbFilterOn3;
                cbCondition = cbCondition3;
                txtValue = txtValue3;
            }

            if (cbFilterOn.SelectedIndex < 0)
                return "";

            string dbField="";
            string val = ((ComboBoxItem)cbFilterOn.SelectedItem).Content.ToString();

            if (val == "Vendor") dbField = "VendorName";
            if (val == "Claim No")      dbField = "ClaimNumber";
            if (val == "Payee")      dbField = "";                             // FIX!
            if (val == "Ceding Company")      dbField = "CedingCompany";
            if (val == "Insured")      dbField = "Insured";
            if (val == "Affiliate")      dbField = "Affiliate";
//            if (val == "Batch")      dbField = "SourceBatchNumber";
            if (val == "Currency")      dbField = "CurrencyCode";
            if (val == "Amount") dbField = "TotalPaymentAmount";
            if (val == "Hold Type") dbField = "PaymentHoldType";
            if (val == "Hold Reason") dbField = "PaymentHoldReason";
            if (val == "Cedent ID") dbField = "Insurer_ID";


            if (val == "Is on any hold")
            {
                dbField = "PaymentHoldFlg";
                cbCondition.SelectedIndex = 1;
                txtValue.Text = "True";
            }
            if (val == "Is on CLAIMS hold")
            {
                return "(PaymentHoldFlg='True' and PaymentHoldType='Claims') ";
            }
            if (val == "Is on REINSURANCE hold")
            {
                return "(PaymentHoldFlg='True' and PaymentHoldType='Reinsurance') ";
            }
            if (val == "Is on CEDENT hold")
            {
                return "(PaymentHoldFlg='True' and PaymentHoldType='Cedent Company') ";
            }
            if (val == "Is RELEASED")
            {
                dbField = "PaymentHoldFlg";
                cbCondition.SelectedIndex = 1;
                txtValue.Text = "False";
            }

            if (val == "Is UNABLE TO BE PROCESSED")
            {
                return "(PaymentHoldFlg='False' and StatusName='Fail') ";
            }


 
            string sql = "(" + dbField;

            val = ((ComboBoxItem)cbCondition.SelectedItem).Content.ToString();

            if (val == "Contains")
                sql += " like '%" + txtValue.Text + "%' ";
            if (val == "Equals")
                sql += " = '" + txtValue.Text + "' ";
            if (val == "Starts with")
                sql += " like '" + txtValue.Text + "%' ";
            if (val == "Ends with")
                sql += " like '" + txtValue.Text + "%' ";

            sql += ") ";
            return sql;
        }

        public void reset()
        {
            showHide(2, show: false);
            showHide(3, show: false);
            txtValue2.Text = "";
            txtValue3.Text = "";
            adjustHeight();
        }

        public void adjustHeight()
        {
            if (cbFilterOn2.Visibility == System.Windows.Visibility.Collapsed)
                Height = 62;
            else if (cbFilterOn3.Visibility == System.Windows.Visibility.Collapsed)
                Height = 92;
            else
                Height = 122;
            UpdateLayout();
        }

        public void showHide(int row, bool show)
        {
            TextBlock lblOr = (row == 2) ? lblOr2 : lblOr3;
            ComboBox cbFilterOn = (row == 2) ? cbFilterOn2 : cbFilterOn3;
            ComboBox cbCondition = (row == 2) ? cbCondition2 : cbCondition3;
            TextBox txtValue = (row == 2) ? txtValue2 : txtValue3;
            Button btnDel = (row == 2) ? btnDel2 : btnDel3;

            lblOr.Visibility = cbFilterOn.Visibility = cbCondition.Visibility = txtValue.Visibility = btnDel.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;

            adjustHeight();
            if (FilterClicked != null)
                FilterClicked(this, new FilterEventArgs() { ID = "", action = "SIZECHANGED" });
        }

        private void btnOR_Click(object sender, RoutedEventArgs e)
        {
            // Add row 2
            if (cbFilterOn2.Visibility == System.Windows.Visibility.Collapsed)
            {
                showHide(2, show: true);
                btnDel2.Visibility = System.Windows.Visibility.Visible;
            }

            else if (cbFilterOn3.Visibility == System.Windows.Visibility.Collapsed)
            {
                showHide(3, show: true);
                btnDel2.Visibility = System.Windows.Visibility.Collapsed;
                btnDel3.Visibility = System.Windows.Visibility.Visible;
            }

        }

        private void btnDel2_Click(object sender, RoutedEventArgs e)
        {
            showHide(2, show: false);
            txtValue2.Text = "";
        }

        private void btnDel3_Click(object sender, RoutedEventArgs e)
        {
            showHide(3, show: false);
            txtValue3.Text = "";
            btnDel2.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnAND_Click(object sender, RoutedEventArgs e)
        {
            if (FilterClicked != null)
                FilterClicked(this, new FilterEventArgs() { ID = "", action="AND" });
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            if (FilterClicked != null)
                FilterClicked(this, new FilterEventArgs() { ID = "", action = "SEARCH" });
        }

        private void lblClose_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (FilterClicked != null)
                FilterClicked(this, new FilterEventArgs() { ID = "", action = "CLOSE" });
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtValue1.Text = txtValue2.Text = txtValue3.Text = "";
            cbCondition1.SelectedIndex = cbCondition2.SelectedIndex = cbCondition3.SelectedIndex = -1;
            cbFilterOn1.SelectedIndex = cbFilterOn2.SelectedIndex = cbFilterOn3.SelectedIndex = -1;
            if (FilterClicked != null)
                FilterClicked(this, new FilterEventArgs() { ID = "", action = "CLEAR" });
        }

        public void hideClearButton()
        {
            btnClear.Visibility = System.Windows.Visibility.Hidden;
        }

    }
}
