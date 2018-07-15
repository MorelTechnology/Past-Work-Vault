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
    /// Interaction logic for SelectUser.xaml
    /// </summary>
    public partial class SelectUser : UserControl
    {
        public SelectUser()
        {
            InitializeComponent();
        }

        private void ebFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            fill();
        }

        public void fill()
        {
            dgFrom.ItemsSource = MainWindow.ourMainWindow.screenHold.getUsersMatchingFilter(ebFilter.Text).DefaultView;
        }


        private void dgFrom_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            processSelect();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            processSelect();
        }

        private void processSelect()
        {
            // Fast fail on no associate selected
            if (dgFrom.SelectedIndex < 0)
            {
                MessageBox.Show("No associate selected");
                return;
            }

            Visibility = Visibility.Collapsed;

            string sam = ((DataRowView)dgFrom.SelectedItem)["SamAccountName"].ToString();
            string adjusted = ((DataRowView)dgFrom.SelectedItem)["AdjustedName"].ToString();
            string id = ((DataRowView)dgFrom.SelectedItem)["ActiveDirectoryID"].ToString();

            MainWindow.currentUser = sam;
            MainWindow.ourMainWindow.lblCurrentUser.Text = MainWindow.currentUser;

            MainWindow.ourMainWindow.screenHold.loadADGroups();
            MainWindow.ourMainWindow.screenHold.adjustForDepartment();
            MainWindow.ourMainWindow.AdjustForUser();
            MainWindow.ourMainWindow.screenManageCeded.AdjustPermissionsForUser();
        }

    }
}
