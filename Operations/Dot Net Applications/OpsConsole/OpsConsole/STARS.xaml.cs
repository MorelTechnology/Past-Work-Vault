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
using Microsoft.Office.Interop.Excel;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for STARS.xaml
    /// </summary>
    public partial class STARS : UserControl
    {
        public class adjustment
        {
            public double amount { get; set; }
            public string state { get; set; }
            public string lob { get; set; }
            public string company { get; set; }
        }

        public List<adjustment> taxesLicensesFees = new List<adjustment>();
        public List<adjustment> commissions = new List<adjustment>();

        public STARS()
        {
            InitializeComponent();
        }

        private void btnOpenAdjustments_Click(object sender, RoutedEventArgs e)
        {
            lblStatus.Text = "Processing...";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".XLSX";
            dlg.Filter = "Spreadsheet Files (*.xlsx)|*.xlsx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                lblStatus2.Text = "";
                parseAdjustmentSpreadsheet(dlg.FileName, taxesLicensesFees, 3, "F", "B", "D", "A");
                parseAdjustmentSpreadsheet(dlg.FileName, commissions, 3, "M", "I", "K", "H");

                // Get unique list of companies from both sides of the spreadsheet
                foreach (string s in taxesLicensesFees.Select(x => x.company).Distinct())
                    if (!lbCompanies.Items.Contains(s))
                        lbCompanies.Items.Add(s);
                foreach (string s in commissions.Select(x => x.company).Distinct())
                    if (!lbCompanies.Items.Contains(s))
                        lbCompanies.Items.Add(s);
               

                lblStatus.Text = "Complete";
                lblStatus2.Text = taxesLicensesFees.Count().ToString() + " unique Taxes, Licenses & Fees, and " + commissions.Count().ToString() + " unique Commissions loaded";
                return;
            }
            lblStatus.Text = "";
        }

        private void parseAdjustmentSpreadsheet(string file, List<adjustment> adjustments, int startRow, string companyCol, string stateCol, string lobCol, string aountCol)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(file, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            while (true)
            {
                if (xlWorkSheet.get_Range(companyCol + startRow.ToString()).Value2 == null)
                    break;

                string company = xlWorkSheet.get_Range(companyCol + startRow.ToString()).Value2.ToString();
                string state = xlWorkSheet.get_Range(stateCol + startRow.ToString()).Value2.ToString();
                string lob = xlWorkSheet.get_Range(lobCol + startRow.ToString()).Value2.ToString();
                double amount = 0d;
                double.TryParse(xlWorkSheet.get_Range(aountCol + startRow.ToString()).Value2.ToString(), out amount);
                startRow++;
                addToBucket(adjustments, state, lob, company, amount);
            }

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            //releaseObject(xlWorkSheet);
            //releaseObject(xlWorkBook);
            //releaseObject(xlApp);
        }

        private void addToBucket(List<adjustment> adjustments, string state, string lob, string company, double amount)
        {
            foreach (adjustment a in adjustments)
                if ((a.state == state) && (a.lob == lob) && (a.company == company))
                {
                    a.amount += amount;
                    return;
                }
            adjustments.Add(new adjustment() { company = company, lob = lob, state = state, amount = amount });
        }

        private double getFromBucket(List<adjustment> adjustments, string state, string lob, string company)
        {
            foreach (adjustment a in adjustments)
                if ((a.state == state) && (a.lob == lob) && (a.company == company))
                    return a.amount;
            return 0d;
        }

        private void modifySpreadsheet(string file, List<adjustment> adjustments, int startRow, string company, string stateCol, string lobCol)
        {
            Microsoft.Office.Interop.Excel.Application xlApp;
            Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            object misValue = System.Reflection.Missing.Value;

            xlApp = new Microsoft.Office.Interop.Excel.Application();
            xlWorkBook = xlApp.Workbooks.Open(file, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

            while (true)
            {
                if (xlWorkSheet.get_Range(stateCol + startRow.ToString()).Value2 == null)
                    break;

                string state = xlWorkSheet.get_Range(stateCol + startRow.ToString()).Value2.ToString();
                string lob = xlWorkSheet.get_Range(lobCol + startRow.ToString()).Value2.ToString();

                double comm = getFromBucket(commissions, state, lob, company);
                double tlf = getFromBucket(taxesLicensesFees, state, lob, company);

                xlWorkSheet.get_Range("S" + startRow.ToString()).Value2 = comm;
                xlWorkSheet.get_Range("T" + startRow.ToString()).Value2 = tlf;

                startRow++;
            }

            xlWorkBook.Close(true, misValue, misValue);
            xlApp.Quit();

            //releaseObject(xlWorkSheet);
            //releaseObject(xlWorkBook);
            //releaseObject(xlApp);
        }

        private void btnModifyStarsSpreadsheet_Click(object sender, RoutedEventArgs e)
        {
            if (lbCompanies.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a company from the listbox");
                return;
            }

            string company = lbCompanies.SelectedItem.ToString();

            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".XLS";
            dlg.Filter = "Spreadsheet Files (*.xls)|*.xls";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                modifySpreadsheet(dlg.FileName, taxesLicensesFees, 3, company, "B", "D");
            }
        }
    }
}
