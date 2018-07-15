using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using System.Windows;
using Newtonsoft.Json;


namespace TestClient
{

    public partial class MainWindow : Window
    {
        #region Public Constructors

        public MainWindow()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods
        #region Button Click Events
        private void btnRESTCall_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                #region Get Rest URL Dynamically
                // This logic gets the rest version of the URL dynamically, 
                string baseUri;
                string restUri;
                using (var sc = new ServiceReference.ServiceClient())
                {
                    var uri = sc.Endpoint.Address.Uri;
                    baseUri = uri.ToString().Replace(uri.Segments[uri.Segments.Length - 1],"");
                };
                restUri = baseUri + "rest";
                #endregion

                WebRequest theRequest = WebRequest.Create(restUri + "/Documents");
                WebResponse theResponse = theRequest.GetResponse();
                using (var reader = new StreamReader(theResponse.GetResponseStream()))
                {
                    dgREST.ItemsSource = JsonConvert.DeserializeObject<ServiceReference.Document[]>(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSOAPCall_Click(object sender, RoutedEventArgs e)
        {
            ServiceReference.ServiceClient ProxySOAP = new ServiceReference.ServiceClient();
            dgSOAP.ItemsSource = ProxySOAP.GetDocuments();
        }
        #endregion Button Click Events
        #endregion Private Methods
    }
}