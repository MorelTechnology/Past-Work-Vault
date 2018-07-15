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

namespace CashFlow
{
    /// <summary>
    /// Interaction logic for SelectUser.xaml
    /// </summary>
    public partial class SelectUser : UserControl
    {
        MainWindow ourMainWindow = null;
        public string purpose = "";

        public SelectUser()
        {
            InitializeComponent();
        }

        public void setOurMainWindow(MainWindow o)
        {
            ourMainWindow = o;
        }

        public void fill()
        {
            dgFrom.ItemsSource = ourMainWindow.ourData.getUsersMatchingFilter(ebFilter.Text).DefaultView;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            processSelection();
        }

        private void processSelection()
        {
            // Fast fail on no associate selected
            if (dgFrom.SelectedIndex < 0)
            {
                MessageBox.Show("No associate selected");
                return;
            }

            if (purpose == "AddSubFor")
                ourMainWindow.ucSecurity.completeSubFor();

            if (purpose == "QA")
            {
                string sam = ((DataRowView)dgFrom.SelectedItem)["SamAccountName"].ToString();
                string adjusted = ((DataRowView)dgFrom.SelectedItem)["AdjustedName"].ToString();
                string id = ((DataRowView)dgFrom.SelectedItem)["ActiveDirectoryID"].ToString();

                ourMainWindow.completeSwitchUser(sam, adjusted, id);
            }
            Visibility = Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void ebFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            fill();
        }

        private void dgFrom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgFrom.SelectedIndex < 0)
                return;

            processSelection();
        }
    }
}
