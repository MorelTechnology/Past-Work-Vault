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
    /// Interaction logic for schedFilterTime.xaml
    /// </summary>
    public partial class schedFilterTime : UserControl
    {
        public bool ignoreChanges = false;

        // PROPERTY - ShowTime
        public static readonly DependencyProperty ShowTimeProperty = DependencyProperty.Register("ShowTime", typeof(DateTime), typeof(schedFilterTime), new PropertyMetadata(DateTime.Parse("1/1/2001 13:00"), OnShowTimeChanged));
        public DateTime ShowTime { get { return (DateTime)this.GetValue(ShowTimeProperty); } set { this.SetValue(ShowTimeProperty, value); } }
        private static void OnShowTimeChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            schedFilterTime control = source as schedFilterTime;
            control.ignoreChanges = true;

            int hour = control.ShowTime.Hour;
            int min = control.ShowTime.Minute;
            string ampm="AM";
            if (hour > 12)
            {
                hour -= 12;
                ampm = "PM";
            }

            control.cbHour.SelectedIndex = hour-1;
            control.setDropdownFromValue(control.cbMinute, min.ToString("00"));
            control.setDropdownFromValue(control.cbAMPM, ampm);

            control.ignoreChanges = false;
        }

        public void setDropdownFromValue(ComboBox c, string value)
        {
            int index = 0;
            foreach (ComboBoxItem ci in c.Items)
            {
                if (ci.Content.ToString() == value)
                {
                    c.SelectedIndex = index;
                    return;
                }
                index++;
            }
        }

        public schedFilterTime()
        {
            InitializeComponent();
        }
    }
}
