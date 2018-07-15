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

namespace ApplicationAccess
{
    /// <summary>
    /// Interaction logic for ObtainSDTicket.xaml
    /// </summary>
    public partial class ObtainSDTicket : UserControl
    {
        public ObtainSDTicket()
        {
            InitializeComponent();
        }

        private void btnLookupTicket(object sender, RoutedEventArgs e)
        {
            SDTicket.showTicket(tbTicketInfo, txtTicket.Text);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtTicket_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
