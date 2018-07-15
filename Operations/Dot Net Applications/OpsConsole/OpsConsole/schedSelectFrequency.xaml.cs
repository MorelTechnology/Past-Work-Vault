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
    /// Interaction logic for schedSelectFrequency.xaml
    /// </summary>
    public partial class schedSelectFrequency : UserControl
    {
        #region events
        // EVENT - SELECTION
        public enum freqType { ONCE, DAILY, WEEKLY, MONTHLY };
        public class FreqTypeEventArgs : EventArgs
        {
            public freqType ft;
        }
        public event EventHandler<FreqTypeEventArgs> FrequencySelected;
        #endregion

        public schedSelectFrequency()
        {
            InitializeComponent();
        }

        public void setVisualState(freqType ft)
        {
            if (ft == freqType.DAILY)
                showRadioButtonStatus(btnDaily, new Button[] { btnOnce, btnDaily, btnWeekly, btnMonthly });
            if (ft == freqType.MONTHLY)
                showRadioButtonStatus(btnMonthly, new Button[] { btnOnce, btnDaily, btnWeekly, btnMonthly });
            if (ft == freqType.ONCE)
                showRadioButtonStatus(btnOnce, new Button[] { btnOnce, btnDaily, btnWeekly, btnMonthly });
            if (ft == freqType.WEEKLY)
                showRadioButtonStatus(btnWeekly, new Button[] { btnOnce, btnDaily, btnWeekly, btnMonthly });
        }

        private void btnOnce_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnOnce, new Button[] { btnOnce, btnDaily, btnWeekly, btnMonthly });
            if (FrequencySelected != null)
                FrequencySelected(this, new FreqTypeEventArgs() { ft=freqType.ONCE} );
        }

        private void btnDaily_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnDaily, new Button[] { btnOnce, btnDaily, btnWeekly, btnMonthly });
            if (FrequencySelected != null)
                FrequencySelected(this, new FreqTypeEventArgs() { ft = freqType.DAILY });
        }

        private void btnWeekly_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnWeekly, new Button[] { btnOnce, btnDaily, btnWeekly, btnMonthly });
            if (FrequencySelected != null)
                FrequencySelected(this, new FreqTypeEventArgs() { ft = freqType.WEEKLY });
        }

        private void btnMonthly_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnMonthly, new Button[] { btnOnce, btnDaily, btnWeekly, btnMonthly });
            if (FrequencySelected != null)
                FrequencySelected(this, new FreqTypeEventArgs() { ft = freqType.MONTHLY });
        }

        public void showRadioButtonStatus(Button btnSet, Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                    if (c is Rectangle)
                        ((Rectangle)c).Opacity = (b == btnSet) ? 1d : 0.2d;
        }

    }
}
