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
    /// Interaction logic for ucNotification.xaml
    /// </summary>
    public partial class ucNotification : UserControl
    {
        MainWindow ourParent = null;

        public ucNotification()
        {
            InitializeComponent();
        }

        public void setParent(MainWindow mw)
        {
            ourParent = mw;
        }

        private void dgNotifictions_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgNotifictions.SelectedIndex < 0)
                return;

            string wm = ((DataRowView)dgNotifictions.SelectedItem)["WorkMatter"].ToString().Trim();
            ourParent.filterOn(wm);
        }
    }
}
