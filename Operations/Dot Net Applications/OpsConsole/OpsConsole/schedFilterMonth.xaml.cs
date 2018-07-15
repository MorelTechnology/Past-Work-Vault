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
    /// Interaction logic for schedFilterMonth.xaml
    /// </summary>
    public partial class schedFilterMonth : UserControl
    {
        public bool ignoreChanges = false;

        // PROPERTY - MonthsSelected
        public static readonly DependencyProperty ShowMonthsProperty = DependencyProperty.Register("ShowMonths", typeof(String), typeof(schedFilterMonth), new PropertyMetadata("", OnShowMonthsChanged));
        public String ShowMonths { get { return (String)this.GetValue(ShowMonthsProperty); } set { this.SetValue(ShowMonthsProperty, value); } }
        private static void OnShowMonthsChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            schedFilterMonth control = source as schedFilterMonth;
            control.ignoreChanges = true;

            control.jan.IsChecked = control.ShowMonths.IndexOf("JAN~") >= 0;
            control.feb.IsChecked = control.ShowMonths.IndexOf("FEB~") >= 0;
            control.mar.IsChecked = control.ShowMonths.IndexOf("MAR~") >= 0;
            control.apr.IsChecked = control.ShowMonths.IndexOf("APR~") >= 0;
            control.may.IsChecked = control.ShowMonths.IndexOf("MAY~") >= 0;
            control.jun.IsChecked = control.ShowMonths.IndexOf("JUN~") >= 0;
            control.jul.IsChecked = control.ShowMonths.IndexOf("JUL~") >= 0;
            control.aug.IsChecked = control.ShowMonths.IndexOf("AUG~") >= 0;
            control.sep.IsChecked = control.ShowMonths.IndexOf("SEP~") >= 0;
            control.oct.IsChecked = control.ShowMonths.IndexOf("OCT~") >= 0;
            control.nov.IsChecked = control.ShowMonths.IndexOf("NOV~") >= 0;
            control.dec.IsChecked = control.ShowMonths.IndexOf("DEC~") >= 0;

            control.ignoreChanges = false;
        }


        public schedFilterMonth()
        {
            InitializeComponent();
        }

        private void MonthCheckedOrUnchecked(object sender, RoutedEventArgs e)
        {
            return;
            if (ignoreChanges)
                return;

            string months = ((jan.IsChecked == true) ? "JAN~" : "") +
                ((feb.IsChecked == true) ? "FEB~" : "") +
                ((mar.IsChecked == true) ? "MAR~" : "") +
                ((apr.IsChecked == true) ? "APR~" : "") +
                ((may.IsChecked == true) ? "MAY~" : "") +
                ((jun.IsChecked == true) ? "JUN~" : "") +
                ((jul.IsChecked == true) ? "JUL~" : "") +
                ((aug.IsChecked == true) ? "AUG~" : "") +
                ((sep.IsChecked == true) ? "SEP~" : "") +
                ((oct.IsChecked == true) ? "OCT~" : "") +
                ((nov.IsChecked == true) ? "NOV~" : "") +
                ((dec.IsChecked == true) ? "DEC~" : "");
                
            ShowMonths = months;
        }

        public string monthstring()
        {
            string months = ((jan.IsChecked == true) ? "JAN~" : "") +
                ((feb.IsChecked == true) ? "FEB~" : "") +
                ((mar.IsChecked == true) ? "MAR~" : "") +
                ((apr.IsChecked == true) ? "APR~" : "") +
                ((may.IsChecked == true) ? "MAY~" : "") +
                ((jun.IsChecked == true) ? "JUN~" : "") +
                ((jul.IsChecked == true) ? "JUL~" : "") +
                ((aug.IsChecked == true) ? "AUG~" : "") +
                ((sep.IsChecked == true) ? "SEP~" : "") +
                ((oct.IsChecked == true) ? "OCT~" : "") +
                ((nov.IsChecked == true) ? "NOV~" : "") +
                ((dec.IsChecked == true) ? "DEC~" : "");
            return months;
        }
    }
}
