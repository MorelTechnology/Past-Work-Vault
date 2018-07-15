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
    /// Interaction logic for ucAddSubstitution.xaml
    /// </summary>
    public partial class ucAddSubstitution : UserControl
    {
        MainWindow ourMainWindow = null;

        public ucAddSubstitution()
        {
            InitializeComponent();
        }

        public void setOurMainWindow(MainWindow o)
        {
            ourMainWindow = o;
        }

        public void fillLeft()
        {
            dgFrom.ItemsSource = ourMainWindow.ourData.getUsersMatchingFilter(ebFilter.Text).DefaultView;
        }

        public void fillRight()
        {
            dgFor.ItemsSource = ourMainWindow.ourData.getUsersMatchingFilter(ebFilterFor.Text).DefaultView;
        }

        private void ebFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            fillLeft();
        }

        private void ebFilterFor_TextChanged(object sender, TextChangedEventArgs e)
        {
            fillRight();
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if (dgFrom.SelectedIndex < 0)
            {
                MessageBox.Show("You must select an associate you are substituting from");
                return;
            }
            if (dgFor.SelectedIndex < 0)
            {
                MessageBox.Show("You must select an associate you are substituting for");
                return;
            }

            string subadid = ((DataRowView)dgFrom.SelectedItem)["ActiveDirectoryID"].ToString();
            string foradid = ((DataRowView)dgFor.SelectedItem)["ActiveDirectoryID"].ToString();

            if (ourMainWindow.ourData.isUserSuperuser(subadid) || ourMainWindow.ourData.isUserSuperuser(foradid))
            {
                MessageBox.Show("Superusers cannot be a substitute or be substituted");
                return;
            }

            if ( (ourMainWindow.ourData.isUserTeamLead(subadid) == false) && (ourMainWindow.ourData.isUserUnitLead(subadid) == false) )
            {
                if (ourMainWindow.ourData.isUserTeamLead(foradid) || ourMainWindow.ourData.isUserUnitLead(foradid))
                {
                    MessageBox.Show("An Analyst may only substitute for another Analyst. Not a Unit Manager or VP");
                    return;
                }
            }

            if (ourMainWindow.ourData.isUserUnitLead(subadid)  && (ourMainWindow.ourData.isUserTeamLead(subadid) == false) )
            {
                if (ourMainWindow.ourData.isUserTeamLead(foradid))
                {
                    MessageBox.Show("A Unit Manager may only substitute for another Unit Manger or an Analyst, not a VP");
                    return;
                }
            }


            ourMainWindow.ucSecurity.completeSubFor2();
            Visibility = Visibility.Hidden;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Hidden;
        }
    }
}
