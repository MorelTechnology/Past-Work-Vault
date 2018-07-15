using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interaction logic for NAVServiceControl.xaml
    /// </summary>
    public partial class NAVServiceControl : UserControl
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        int checkStatusCountdown = 0;

        public NAVServiceControl()
        {
            InitializeComponent();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dispatcherTimer.Start();
        }

        public void Load()
        {
            showCurrentServiceStatus();
        }

        private void btnStopNaveWeb_Click(object sender, RoutedEventArgs e)
        {
            string status = serviceControl(@"\\ccall-lap7", ServiceAction.stop, "AdobeARMservice");
            checkStatusCountdown = 20;
        }

        private void btnStartNavWeb_Click(object sender, RoutedEventArgs e)
        {
            string status = serviceControl(@"\\ccall-lap7", ServiceAction.start, "AdobeARMservice");
            checkStatusCountdown = 20;

        }

        private void btnStopMDNWS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnStartMDNWS_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            showCurrentServiceStatus();
        }

        private void showCurrentServiceStatus()
        {
            showParticularServiceStatus(ledNavWebProd, @"\\ccall-lap7", "AdobeARMservice");
            showParticularServiceStatus(ledMicrsoftynamicsNavWS, @"\\navwebtest", "MicrosoftDynamicsNavWS");
        }

        private void showParticularServiceStatus(Ellipse led, string server, string service)
        {
            string status = serviceControl(server, ServiceAction.query, service);
            if (status.IndexOf("RUNNING") > 0)
                led.Fill = new SolidColorBrush(Colors.Green);
            else if (status.IndexOf("STOPPED") > 0)
                led.Fill = new SolidColorBrush(Colors.Red);
            else if (status.IndexOf("START-PENDING") > 0)
                led.Fill = new SolidColorBrush(Colors.LightGreen);
            else if (status.IndexOf("STOP-PENDING") > 0)
                led.Fill = new SolidColorBrush(Colors.LightPink);
            else
                led.Fill = new SolidColorBrush(Colors.White);
        }

        private void btnProcessNav_Click(object sender, RoutedEventArgs e)
        {
            // servername should come from web.config
            string status = serviceControl(@"\\ccall-lap7", ServiceAction.query, "AdobeARMservice");
            MessageBox.Show(status);
        }

        public enum ServiceAction { start, stop, query };
        // public enum Service { AdobeARMservice, NAVWEBPRODSQL, MicrosoftDynamicsNavWS };            // <- List of three allowed services
        List<string> allowsServices = new List<string>() {"NAVWEBPRODSQL", "MicrosoftDynamicsNavWS" } ;

        private string serviceControl(string server, ServiceAction serviceAction, string service)
        {
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = "sc";
            p.StartInfo.Arguments = server + " " + serviceAction.ToString() + " " + service.ToString();
            p.Start();
            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();
            return output;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (checkStatusCountdown > 0)
            {
                checkStatusCountdown--;
                showCurrentServiceStatus();
            }

        }



    }
}
