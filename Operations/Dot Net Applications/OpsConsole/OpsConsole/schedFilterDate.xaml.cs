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

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for schedFilterDate.xaml
    /// </summary>
    public partial class schedFilterDate : UserControl
    {
        public schedFilterDate()
        {
            InitializeComponent();
        }

        public void setDate(DateTime? dt)
        {
            cal.SelectedDate = dt;
        }

        public DateTime? getDate()
        {
            return cal.SelectedDate;
        }
    }
}
