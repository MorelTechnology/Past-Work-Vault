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
    /// Interaction logic for schedFilterDayOfMonth.xaml
    /// </summary>
    public partial class schedFilterDayOfMonth : UserControl
    {
        public schedFilterDayOfMonth()
        {
            InitializeComponent();
        }

        private void rbDayNo_Checked(object sender, RoutedEventArgs e)
        {
            setMode(true,1,0,0);
        }

        private void rbWeek_Checked(object sender, RoutedEventArgs e)
        {
            setMode(false,0,1,1);
        }

        public void setMode(bool dayNoMode,int dayno, int week, int day)
        {
            cbDay.IsEnabled = dayNoMode;
            cbWeekNo.IsEnabled = cbDayName.IsEnabled = !dayNoMode;
            if (dayNoMode)
            {
                rbDayNo.IsChecked = true;
                rbWeek.IsChecked = false;
                cbWeekNo.SelectedIndex = cbDayName.SelectedIndex = -1;
                cbDay.SelectedIndex = dayno - 1;
            }
            else
            {
                rbDayNo.IsChecked = false;
                rbWeek.IsChecked = true;
                cbDay.SelectedIndex = -1;
                cbWeekNo.SelectedIndex = week - 1;
                cbDayName.SelectedIndex = day - 1;
            }
        }

        public void getMode(ref bool dayNoMode, ref int dayno, ref int week, ref int day)
        {
            dayNoMode = (bool) rbDayNo.IsChecked;
            dayno = cbDay.SelectedIndex + 1;
            week = cbWeekNo.SelectedIndex + 1;
            day = cbDayName.SelectedIndex + 1;
        }

    }
}
