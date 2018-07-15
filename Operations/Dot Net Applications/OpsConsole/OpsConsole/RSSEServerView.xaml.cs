using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for RSSEServerView.xaml
    /// </summary>
    public partial class RSSEServerView : UserControl
    {
        public enum scriptItem { APPLICATION, APP_ENVIRONMENT, APPS_CHEMA, APP_GROUP, GROUP, GROUP_USER, SCHEMA, SCRIPT, SCRIPT_GROUP };

        #region events
        // public event EventHandler DetailsClicked;
        public class ScriptItemEventArgs : EventArgs
        {
            public scriptItem si;
            public string ID;
        }
        public event EventHandler<ScriptItemEventArgs> ScriptItemClicked;

        #endregion

        #region properties

        // PROPERTY - TITLE
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register("Title", typeof(String), typeof(RSSEServerView), new PropertyMetadata("", OnTitleChanged));
        public string Title
        {
            get { return (String)this.GetValue(TitleProperty); }
            set { this.SetValue(TitleProperty, value); }
        }
        private static void OnTitleChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            RSSEServerView control = source as RSSEServerView;
            control.lblTitle.Text = (String)e.NewValue;
        }

        // PROPERTY - SHOWPLAN
        public static readonly DependencyProperty ShowPlanProperty = DependencyProperty.Register("ShowPlan", typeof(Boolean), typeof(RSSEServerView), new PropertyMetadata(false, OnShowPlanChanged));
        public Boolean ShowPlan
        {
            get { return (Boolean)this.GetValue(ShowPlanProperty); }
            set { this.SetValue(ShowPlanProperty, value); }
        }
        private static void OnShowPlanChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            RSSEServerView control = source as RSSEServerView;
            if ((Boolean)e.NewValue)
            {
                control.lblPlan.Visibility = Visibility.Visible;
                control.dgPlan.Visibility = Visibility.Visible;

                control.showSection(control.lblApplication, control.dgApplications, false);
                control.showSection(control.lblEnvironments, control.dgEnvironments, false);
                control.showSection(control.lblGroups, control.lbGroups, false);
                control.showSection(control.lblSchemas, control.dgSchemas, false);
                control.showSection(control.lblScripts, control.dgScripts, false);
            }
            else
            {
                control.lblPlan.Visibility = Visibility.Collapsed;
                control.dgPlan.Visibility = Visibility.Collapsed;

                control.showSection(control.lblApplication, control.dgApplications, true);
                control.showSection(control.lblEnvironments, control.dgEnvironments, true);
                control.showSection(control.lblGroups, control.lbGroups, true);
                control.showSection(control.lblSchemas, control.dgSchemas, true);
                control.showSection(control.lblScripts, control.dgScripts, true);
            }
        }

        // PROPERTY - FROM DATA
        public static readonly DependencyProperty dataFromProperty = DependencyProperty.Register("dataFrom", typeof(DataSet), typeof(RSSEServerView), new PropertyMetadata(new DataSet(), OndataFromChanged));
        public DataSet dataFrom
        {
            get { return (DataSet)this.GetValue(dataFromProperty); }
            set { this.SetValue(dataFromProperty, value); }
        }
        private static void OndataFromChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            RSSEServerView control = source as RSSEServerView;

            DataSet dsFrom = (DataSet) e.NewValue;

            control.dgApplications.ItemsSource = dsFrom.Tables["Applications"].DefaultView;
        }

        // PROPERTY - TO DATA
        public static readonly DependencyProperty dataToProperty = DependencyProperty.Register("dataTo", typeof(DataSet), typeof(RSSEServerView), new PropertyMetadata(new DataSet(), OndataToChanged));
        public DataSet dataTo
        {
            get { return (DataSet)this.GetValue(dataToProperty); }
            set { this.SetValue(dataToProperty, value); }
        }
        private static void OndataToChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            RSSEServerView control = source as RSSEServerView;

            //var uriSource = new Uri((String)e.NewValue, UriKind.Relative);
            //control.imgLogo.Source = new BitmapImage(uriSource);
        }

        // PROPERTY - PLAN DATA
        public static readonly DependencyProperty dataPlanProperty = DependencyProperty.Register("dataPlan", typeof(DataTable), typeof(RSSEServerView), new PropertyMetadata(new DataTable(), OndataPlanChanged));
        public DataTable dataPlan
        {
            get { return (DataTable)this.GetValue(dataPlanProperty); }
            set { this.SetValue(dataPlanProperty, value); }
        }
        private static void OndataPlanChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            RSSEServerView control = source as RSSEServerView;
            control.dgPlan.ItemsSource = ((DataTable)e.NewValue).DefaultView;
            //var uriSource = new Uri((String)e.NewValue, UriKind.Relative);
            //control.imgLogo.Source = new BitmapImage(uriSource);
        }



        #endregion

        public RSSEServerView()
        {
            InitializeComponent();
        }

        private void lblApplication_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            showSection(lblApplication, dgApplications, lblApplication.Text.StartsWith("►"));
        }

        private void lblGroups_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            showSection(lblGroups, lbGroups, lblGroups.Text.StartsWith("►"));
        }

        private void lblSchemas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            showSection(lblSchemas, dgSchemas, lblSchemas.Text.StartsWith("►"));
        }

        private void lblEnvironments_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            showSection(lblEnvironments, dgEnvironments, lblEnvironments.Text.StartsWith("►"));
        }

        private void lblScripts_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            showSection(lblScripts, dgScripts, lblScripts.Text.StartsWith("►"));
        }

        private void StackPanel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void showSection(TextBlock tb, DataGrid dg, bool show)
        {
            if (show)
            {
                dg.Visibility = System.Windows.Visibility.Visible;
                tb.Text = "▼" + tb.Text.Substring(1);
            }
            else
            {
                dg.Visibility = System.Windows.Visibility.Collapsed;
                tb.Text = "►" + tb.Text.Substring(1);
            }
            adjustScriptBlockSize();
        }

        private void showSection(TextBlock tb, ListBox dg, bool show)
        {
            if (show)
            {
                dg.Visibility = System.Windows.Visibility.Visible;
                tb.Text = "▼" + tb.Text.Substring(1);
            }
            else
            {
                dg.Visibility = System.Windows.Visibility.Collapsed;
                tb.Text = "►" + tb.Text.Substring(1);
            }
            adjustScriptBlockSize();
        }

        private void dgApplications_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgApplications.SelectedIndex < 0)
                return;

            if (!(e.AddedItems[0] is System.Data.DataRowView))
                return;

            string appID = ((System.Data.DataRowView)(e.AddedItems[0])).Row["ApplicationID"].ToString();

            // Filter our data based on the selected application id
            DataTable dtFilteredEnvironments = dataFrom.Tables["ApplicationEnvironments"].Clone();
            var drFE = dataFrom.Tables["ApplicationEnvironments"].Select("ApplicationID = '" + appID + "'");
            foreach (DataRow dr in drFE)
                dtFilteredEnvironments.ImportRow(dr);
            dgEnvironments.ItemsSource = dtFilteredEnvironments.DefaultView;

            // Filter our data based on the selected application id
            DataTable dtFilteredScripts = dataFrom.Tables["Scripts"].Clone();
            dtFilteredScripts.Columns.Add("Schema");
            dtFilteredScripts.Columns.Add("Matches");
            var drFS = dataFrom.Tables["Scripts"].Select("ApplicationID = '" + appID + "'");
            foreach (DataRow dr in drFS)
            {
                dtFilteredScripts.ImportRow(dr);
                dtFilteredScripts.Rows[dtFilteredScripts.Rows.Count - 1]["Schema"] = MainWindow.ourMainWindow.lookup(dataFrom.Tables["Schemas"], "SchemaID", dtFilteredScripts.Rows[dtFilteredScripts.Rows.Count - 1]["SchemaID"].ToString(), "ShortName");
            }
            dgScripts.ItemsSource = dtFilteredScripts.DefaultView;

            // Filter our data based on the selected application id
            DataTable dtFilteredSchemas = dataFrom.Tables["Schemas"].Clone();
            var drFSCH = dataFrom.Tables["ApplicationSchemas"].Select("ApplicationID = '" + appID + "'");
            foreach (DataRow dr in drFSCH)
            {
                foreach (DataRow drs in dataFrom.Tables["Schemas"].Rows)
                {
                    if (dr["SchemaID"].ToString() == drs["SchemaID"].ToString())
                        dtFilteredSchemas.ImportRow(drs);
                }
            }
            dgSchemas.ItemsSource = dtFilteredSchemas.DefaultView;


            // Fire off script item clicked event
            if (ScriptItemClicked != null)
                ScriptItemClicked(this, new ScriptItemEventArgs() { si = scriptItem.APPLICATION, ID = appID });

        }

        private void adjustScriptBlockSize()
        {
            double height = spInfo.ActualHeight - (lblApplication.ActualHeight + dgApplications.ActualHeight + lblGroups.ActualHeight + lbGroups.ActualHeight + lblSchemas.ActualHeight + dgSchemas.ActualHeight + lblEnvironments.ActualHeight + dgEnvironments.ActualHeight + lblSchemas.ActualHeight + 30);
            if (height > 0)
                dgScripts.Height = height;
        }

        private void Grid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
        }

        private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            adjustScriptBlockSize();
        }

        private void dgScripts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fast fail if nothing selected, or nothing added
            if ((dgScripts.SelectedIndex < 0) || (e.AddedItems.Count < 1) || !(e.AddedItems[0] is System.Data.DataRowView))
                return;

            // Get ID for row clicked
            string scriptID = ((System.Data.DataRowView)(e.AddedItems[0])).Row["ScriptID"].ToString();

            // Fire off script item clicked event
            if (ScriptItemClicked != null)
                ScriptItemClicked(this, new ScriptItemEventArgs() { si = scriptItem.SCRIPT, ID = scriptID });
        }

        private void dgEnvironments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fast fail if nothing selected, or nothing added
            if ((dgEnvironments.SelectedIndex < 0) || (e.AddedItems.Count < 1) || !(e.AddedItems[0] is System.Data.DataRowView))
                return;

            // Get ID for row clicked
            string environmentID = ((System.Data.DataRowView)(e.AddedItems[0])).Row["EnvironmentID"].ToString();

            // Fire off script item clicked event
            if (ScriptItemClicked != null)
                ScriptItemClicked(this, new ScriptItemEventArgs() { si = scriptItem.APP_ENVIRONMENT, ID = environmentID });
        }

        private void dgSchemas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fast fail if nothing selected, or nothing added
            if ((dgSchemas.SelectedIndex < 0) || (e.AddedItems.Count < 1) || !(e.AddedItems[0] is System.Data.DataRowView))
                return;

            // Get ID for row clicked
            string schemaID = ((System.Data.DataRowView)(e.AddedItems[0])).Row["SchemaID"].ToString();

            // Fire off script item clicked event
            if (ScriptItemClicked != null)
                ScriptItemClicked(this, new ScriptItemEventArgs() { si = scriptItem.SCHEMA, ID = schemaID });
        }

        private void lbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Fire off script item clicked event
            //if (ScriptItemClicked != null)
              //  ScriptItemClicked(this, new ScriptItemEventArgs() { si = scriptItem.GROUP, ID = appID });
        }

        private void lblPlan_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            showSection(lblPlan, dgPlan, lblPlan.Text.StartsWith("►"));
        }



    }
}
