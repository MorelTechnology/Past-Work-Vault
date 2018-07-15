using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for NavisionDemo.xaml
    /// </summary>
    public partial class NavisionDemo : UserControl
    {
        bool loaded = false;

        public void Load()
        {
            if (loaded)
                return;
            loadDatabaseNames();

            // Server_Backup_Directory


            loaded = true;
        }

        private void loadDatabaseNames()
        {
            using (SqlConnection conn = new SqlConnection("Data Source=MANTESTBS01;Initial Catalog=ITProductionSupport;Integrated Security=True"))
            {
                string sqlcomm = "select distinct ServerName from Server_Backup_Directory";

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    List<String> dbservers = new List<string>();

                    while (reader.Read())
                    {
                        dbservers.Add(reader["ServerName"].ToString());

                    }

                    cbFromServer.ItemsSource = dbservers;
                }

            }
        }

        public NavisionDemo()
        {
            InitializeComponent();
        }

        private async void btnProcessNav_Click(object sender, RoutedEventArgs e)
        {
            Int16.Parse("This is not an int");
            //DateTime.Parse("This is not a date");
        }


        private void cbFromServer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFromServer.SelectedIndex < 0)
                return;

            string server = cbFromServer.SelectedItem.ToString();

            using (SqlConnection conn = new SqlConnection("Data Source=MANTESTBS01;Initial Catalog=ITProductionSupport;Integrated Security=True"))
            {
                string sqlcomm = "select distinct DatabaseName from Server_Backup_Directory where ServerName='" + server + "'";

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    List<String> dbservers = new List<string>();

                    while (reader.Read())
                    {
                        dbservers.Add(reader["DatabaseName"].ToString());

                    }

                    cbFromDatabase.ItemsSource = dbservers;
                }

            }

        }
    }
}
