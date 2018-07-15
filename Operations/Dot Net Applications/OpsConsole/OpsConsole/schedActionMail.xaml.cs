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
    /// Interaction logic for schedActionMail.xaml
    /// </summary>
    public partial class schedActionMail : UserControl
    {
        DataTable dtAD = new DataTable();

        public void load()
        {
            if (dtAD.Rows.Count == 0)
            {
                // dtAD = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "GET_ALL_EMPLOYEES", "OPSCONSOLE").Tables["Users"];
                dtAD = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "NOTIFY_DIST_GROUPS", "OPSCONSOLE").Tables["WS"];
            }

            ebFilter.Text = "";
            fill();
        }

        private void fill()
        {
            string filter = ebFilter.Text;
            DataRow[] filtered = dtAD.Select("Name like '%" + filter + "%'");
            lbAddresses.ItemsSource = (filtered.Length == 0) ? null : filtered.CopyToDataTable().DefaultView;
        }

        public schedActionMail()
        {
            InitializeComponent();
        }

        private void ebFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            fill();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (lbAddresses.SelectedIndex >= 0)
                lbTo.Items.Add(((System.Data.DataRowView)lbAddresses.SelectedItem)["EmailAddress"]);
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if (lbTo.SelectedIndex >= 0)
                lbTo.Items.RemoveAt(lbTo.SelectedIndex);
        }

        private void lbAddresses_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (lbAddresses.SelectedIndex >= 0)
                lbTo.Items.Add(((System.Data.DataRowView)lbAddresses.SelectedItem)["EmailAddress"]);
        }


        public string getToAddresses()
        {
            string to="";

            foreach (string x in lbTo.Items)
            {
                to += x + "; ";
            }
            if (to.Length > 2)
                to = to.Substring(0,to.Length-2);
            return to;
        }

        public string getSubject()
        {
            return ebSubject.Text;
        }

        public string getBody()
        {
            return ebBody.Text;
        }

        public void setToAddresses(string value)
        {
            lbTo.Items.Clear();
            string[] addresses = value.Trim().Split(new char[] { ';' });
            foreach (string word in addresses)
            {
                if (word.Trim() != "")
                    lbTo.Items.Add(word.Trim());
            }
        }

        public void setSubject(string value)
        {
            ebSubject.Text = value;
        }

        public void setBody(string value)
        {
            ebBody.Text = value;
        }

        public void clear()
        {
            lbTo.Items.Clear();
            ebSubject.Text = ebBody.Text = "";
        }


    }
}
