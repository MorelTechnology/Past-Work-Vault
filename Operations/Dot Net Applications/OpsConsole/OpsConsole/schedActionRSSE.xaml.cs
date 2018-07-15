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
    /// Interaction logic for schedActionRSSE.xaml
    /// </summary>
    public partial class schedActionRSSE : UserControl
    {
        DataSet dsSI = new DataSet();

        public schedActionRSSE()
        {
            InitializeComponent();
        }

        public void load()
        {
            if (dsSI.Tables.Count == 0)
            {
                dsSI = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "GET_SCRIPTS_INFO", "GENERAL");
                lbApplication.ItemsSource = dsSI.Tables["Applications"].DefaultView;
            }
        }

        private void lbApplication_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbApplication.SelectedIndex < 0)
            {
                lbScript.ItemsSource = null;
                return;
            }

            string appid = ((System.Data.DataRowView)lbApplication.SelectedItem)["ApplicationID"].ToString();
            DataRow[] filtered = dsSI.Tables["Scripts"].Select("ApplicationID='"+appid+"'");
            lbScript.ItemsSource = (filtered.Length == 0) ? null : filtered.CopyToDataTable().DefaultView;
        }
    }
}
