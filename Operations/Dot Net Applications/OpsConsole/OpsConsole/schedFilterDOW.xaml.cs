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
    /// Interaction logic for schedFilterDOW.xaml
    /// </summary>
    public partial class schedFilterDOW : UserControl
    {
        public bool ignoreChanges = false;

        #region properties
        // MTWRFSU
        // PROPERTY - Ticket
        public static readonly DependencyProperty ShowDaysProperty = DependencyProperty.Register("ShowDays", typeof(String), typeof(schedFilterDOW), new PropertyMetadata("", OnShowDaysChanged));
        public String ShowDays { get { return (String)this.GetValue(ShowDaysProperty); } set { this.SetValue(ShowDaysProperty, value); } }
        //public String ShowDays { get { return (String)daystring(); } set { this.SetValue(ShowDaysProperty, value); } }
        private static void OnShowDaysChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            schedFilterDOW control = source as schedFilterDOW;
            control.ignoreChanges = true;
            control.mon.IsChecked = control.ShowDays.IndexOf("M") >= 0;
            control.tue.IsChecked = control.ShowDays.IndexOf("T") >= 0;
            control.wed.IsChecked = control.ShowDays.IndexOf("W") >= 0;
            control.thr.IsChecked = control.ShowDays.IndexOf("R") >= 0;
            control.fri.IsChecked = control.ShowDays.IndexOf("F") >= 0;
            control.sat.IsChecked = control.ShowDays.IndexOf("S") >= 0;
            control.sun.IsChecked = control.ShowDays.IndexOf("U") >= 0;
            control.ignoreChanges = false;
        }
        #endregion

        public schedFilterDOW()
        {
            InitializeComponent();
        }

        private void DayCheckedOrUnchecked(object sender, RoutedEventArgs e)
        {
            return;
            if (ignoreChanges)
                return;

            string days = ((mon.IsChecked == true) ? "M" : "") + ((tue.IsChecked == true) ? "T" : "") + ((wed.IsChecked == true) ? "W" : "") + ((thr.IsChecked == true) ? "R" : "") + ((fri.IsChecked == true) ? "F" : "");
            days += ((sat.IsChecked == true) ? "S" : "") + ((sun.IsChecked == true) ? "U" : "");
            ShowDays = days;
        }

        public string daystring()
        {
            string days = ((mon.IsChecked == true) ? "M" : "") + ((tue.IsChecked == true) ? "T" : "") + ((wed.IsChecked == true) ? "W" : "") + ((thr.IsChecked == true) ? "R" : "") + ((fri.IsChecked == true) ? "F" : "");
            days += ((sat.IsChecked == true) ? "S" : "") + ((sun.IsChecked == true) ? "U" : "");
            return days;
        }

    }
}
