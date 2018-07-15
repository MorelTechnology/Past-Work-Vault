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
    /// Interaction logic for schedActionSelector.xaml
    /// </summary>
    public partial class schedActionSelector : UserControl
    {
        public schedActionSelector()
        {
            InitializeComponent();
        }

        #region events definitions
        // EVENT - SELECTION
        public event EventHandler FrequencySelected;
        #endregion

        #region properties
        // PROPERTY - Email
        public static readonly DependencyProperty ShowEmailProperty = DependencyProperty.Register("ShowEmail", typeof(Boolean), typeof(schedActionSelector), new PropertyMetadata(false, OnShowEmailChanged));
        public Boolean ShowEmail  {get { return (Boolean)this.GetValue(ShowEmailProperty); }  set { this.SetValue(ShowEmailProperty, value); } }
        private static void OnShowEmailChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)  {schedActionSelector control = source as schedActionSelector;  control.rectEmail.Opacity = ((Boolean)e.NewValue) ? 1d : 0.2d;  }

        // PROPERTY - Ticket
        public static readonly DependencyProperty ShowTicketProperty = DependencyProperty.Register("ShowTicket", typeof(Boolean), typeof(schedActionSelector), new PropertyMetadata(false, OnShowTicketChanged));
        public Boolean ShowTicket {get { return (Boolean)this.GetValue(ShowTicketProperty); } set { this.SetValue(ShowTicketProperty, value); } }
        private static void OnShowTicketChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        { schedActionSelector control = source as schedActionSelector;  control.rectTicket.Opacity = ((Boolean)e.NewValue) ? 1d : 0.2d;  }

        // PROPERTY - RSSE
        public static readonly DependencyProperty ShowRSSEProperty = DependencyProperty.Register("ShowRSSE", typeof(Boolean), typeof(schedActionSelector), new PropertyMetadata(false, OnShowRSSEChanged));
        public Boolean ShowRSSE { get { return (Boolean)this.GetValue(ShowRSSEProperty); } set { this.SetValue(ShowRSSEProperty, value); } }
        private static void OnShowRSSEChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        { schedActionSelector control = source as schedActionSelector; control.rectRSSE.Opacity = ((Boolean)e.NewValue) ? 1d : 0.2d; }

        // PROPERTY - SSIS
        public static readonly DependencyProperty ShowSSISProperty = DependencyProperty.Register("ShowSSIS", typeof(Boolean), typeof(schedActionSelector), new PropertyMetadata(false, OnShowSSISChanged));
        public Boolean ShowSSIS { get { return (Boolean)this.GetValue(ShowSSISProperty); } set { this.SetValue(ShowSSISProperty, value); } }
        private static void OnShowSSISChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        { schedActionSelector control = source as schedActionSelector; control.rectSSIS.Opacity = ((Boolean)e.NewValue) ? 1d : 0.2d; }

        // PROPERTY - Report
        public static readonly DependencyProperty ShowReportProperty = DependencyProperty.Register("ShowReport", typeof(Boolean), typeof(schedActionSelector), new PropertyMetadata(false, OnShowReportChanged));
        public Boolean ShowReport { get { return (Boolean)this.GetValue(ShowReportProperty); } set { this.SetValue(ShowReportProperty, value); } }
        private static void OnShowReportChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        { schedActionSelector control = source as schedActionSelector; control.rectReport.Opacity = ((Boolean)e.NewValue) ? 1d : 0.2d; }
        #endregion

        #region events
        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            ShowEmail = !ShowEmail;
            if (FrequencySelected != null)
                FrequencySelected(this, null);
        }

        private void btnTicket_Click(object sender, RoutedEventArgs e)
        {
            ShowTicket = !ShowTicket;
            if (FrequencySelected != null)
                FrequencySelected(this, null);
        }

        private void btnRSSE_Click(object sender, RoutedEventArgs e)
        {
            ShowRSSE = !ShowRSSE;
            if (FrequencySelected != null)
                FrequencySelected(this, null);
        }

        private void btnSSIS_Click(object sender, RoutedEventArgs e)
        {
            ShowSSIS = !ShowSSIS;
            if (FrequencySelected != null)
                FrequencySelected(this, null);
        }

        private void btnReport_Click(object sender, RoutedEventArgs e)
        {
            ShowReport = !ShowReport;
            if (FrequencySelected != null)
                FrequencySelected(this, null);
        }
        #endregion

    }
}
