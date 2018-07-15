using System;
using System.Collections.Generic;
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
using System.Diagnostics;
using System.Data.SqlClient;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for WOCO.xaml
    /// </summary>
    public partial class WOCO : UserControl
    {
        // correct public static string connectionStringDEV = "Data Source=MANDEVBS01;Initial Catalog=MSDB;Integrated Security=True";
        public static string connectionStringDEV = "Data Source=MANTESTBS01;Initial Catalog=MSDB;Integrated Security=True";
        public static string connectionStringTEST = "Data Source=MANTESTBS01;Initial Catalog=MSDB;Integrated Security=True";
        public static string connectionStringPROD = "Data Source=MANPRODBS01;Initial Catalog=MSDB;Integrated Security=True";

        static string folderDEV = @"\\mansan02\batch$\REM\ExportClaimHistoryBST";
        static string folderTEST = @"\\mansan02\batch$\REM\ExportClaimHistoryBST";
        static string folderPROD = @"\\mansan02\batch$\REM\ExportClaimHistoryBST";

        public WOCO()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, RoutedEventArgs e)
        {
            string folder = "";
            string connectionString = "";

            if (ScriptEngine.envCurrent == ScriptEngine.environemnt.DEV)
            {
                folder = folderDEV;
                connectionString = connectionStringDEV;
            }
            if (ScriptEngine.envCurrent == ScriptEngine.environemnt.TEST)
            {
                folder = folderTEST;
                connectionString = connectionStringTEST;
            }
            if (ScriptEngine.envCurrent == ScriptEngine.environemnt.PROD)
            {
                folder = folderPROD;
                connectionString = connectionStringPROD;
            }

            string numbers = "(";
            foreach (string s in lbClaims.Items)
                numbers += "'" + s + "',";
            numbers = numbers.TrimEnd(new char[] { ',' });
            numbers = numbers + ")";

            using (StreamWriter outfile = new StreamWriter(System.IO.Path.Combine(folder,"claimno.txt")))
            {
                outfile.Write(numbers);
            }

            try
            {
                File.Delete(System.IO.Path.Combine(folder, "ClaimSummaryExport.txt"));
                File.Delete(System.IO.Path.Combine(folder, "ClaimTransExt.txt"));
                File.Delete(System.IO.Path.Combine(folder, "failure-notice.txt"));

                string sql = "EXEC msdb.dbo.sp_start_job N'WocoExtract'";

                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand command = conn.CreateCommand())
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

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtClaim.Text == "")
            {
                MessageBox.Show("Enter a claim number, then press Add");
                return;
            }
            lbClaims.Items.Add(txtClaim.Text);
            txtClaim.Text = "";
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbClaims.SelectedIndex < 0)
                return;

            lbClaims.Items.RemoveAt(lbClaims.SelectedIndex);
        }

        private void btnClearResults_Click(object sender, RoutedEventArgs e)
        {
            clearPreviousResults();
        }

        private void clearPreviousResults()
        {
            string folder = "";

            if (ScriptEngine.envCurrent == ScriptEngine.environemnt.DEV)
                folder = folderDEV;
            if (ScriptEngine.envCurrent == ScriptEngine.environemnt.TEST)
                folder = folderTEST;
            if (ScriptEngine.envCurrent == ScriptEngine.environemnt.PROD)
                folder = folderPROD;

            File.Delete(System.IO.Path.Combine(folder, "ClaimSummaryExport.txt"));
            File.Delete(System.IO.Path.Combine(folder, "ClaimTransExt.txt"));
            File.Delete(System.IO.Path.Combine(folder, "failure-notice.txt"));
            File.Delete(System.IO.Path.Combine(folder, "claimno.txt"));
        }


    }
}
