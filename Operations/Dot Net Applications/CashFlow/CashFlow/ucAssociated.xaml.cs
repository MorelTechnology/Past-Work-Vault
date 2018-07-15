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
    /// Interaction logic for ucAssociated.xaml
    /// </summary>
    public partial class ucAssociated : UserControl
    {
        public ucAssociated()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void dgWM_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnView_Click(object sender, RoutedEventArgs e)
        {
            if (dgWM.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a WM to view");
                return;
            }

            string wm = ((DataRowView)dgWM.SelectedItem)["WorkMatter"].ToString();
            MainWindow.ourMainWindow.currentSpecialWorkMatter = wm;
            MainWindow.ourMainWindow.fillWM();
            MainWindow.ourMainWindow.selectWMinGrid(wm);
            Visibility = Visibility.Collapsed;
        }
    }
}
