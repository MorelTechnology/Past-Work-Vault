using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

//////////////////////////////////////////////////////////////////////
// NOTE: ALL OF THE CODE IN THIS FILE IS OBSOLETE !!
// 
// The current screen is Admin2
// This is only here for reference purposes.
//////////////////////////////////////////////////////////////////////

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for Admin.xaml
    /// </summary>
    public partial class Admin : UserControl
    {
        public bool loaded = false;
        DataTable dtUserPrivs = null;
        DataTable tablePresentationGroups = null;
        DataTable tablePresentationGroupButtons = null;

        public Admin()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////////////////////////
        // NOTE: ALL OF THE CODE IN THIS FILE IS OBSOLETE !!
        // 
        // The current screen is Admin2
        // This is only here for reference purposes.
        //////////////////////////////////////////////////////////////////////

        public void Load()
        {
            gridAddUser.Visibility = System.Windows.Visibility.Hidden;

            if (loaded)
                return;

            getTables();
            buildDataTable();
            loaded = true;
        }

        private void getTables()
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "CRUD_CONFIGURATION", "OPSCONSOLE");
            dtUserPrivs = ds.Tables["UserPermissions"];
            tablePresentationGroups = ds.Tables["PresentationGroups"];
            tablePresentationGroupButtons = ds.Tables["PresentationGroupButtons"];

            bool ignore = false;
            foreach (DataRow dr in tablePresentationGroupButtons.Rows)
            {
                if (dr["Line1Text"].ToString() == "Automated Reminders")
                    ignore = true;
            }
            if (!ignore)
                tablePresentationGroupButtons.Rows.Add("27", "4", "Automated Reminders", "", "SDTICKET", "8");

        }

        private void updatePermissions(string id, string username, string function, string permission, string operation)
        {
            DataTable myInputTable = new DataTable("UserPermissions");
            myInputTable.Columns.Add("ID", typeof(long));
            myInputTable.Columns.Add("Username");
            myInputTable.Columns.Add("Function");
            myInputTable.Columns.Add("Permission");
            myInputTable.Columns.Add("Operation");
            myInputTable.Columns.Add("TicketNumber");

            myInputTable.Rows.Add(id, username, function, permission, operation,"22222");

            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, myInputTable, "CRUD_CONFIGURATION", "OPSCONSOLE");
            dtUserPrivs = ds.Tables["UserPermissions"];
            tablePresentationGroups = ds.Tables["PresentationGroups"];
            tablePresentationGroupButtons = ds.Tables["PresentationGroupButtons"];
        }

        private void buildDataTable()
        {
            dgPermissions.AutoGenerateColumns = false;
            dgPermissions.HeadersVisibility = DataGridHeadersVisibility.Column;
            dgPermissions.Columns.Clear();

            DataTable myInputTable = new DataTable("InputTable");

            // Column for Group / Task
            myInputTable.Columns.Add("GroupTask");
            addColumnToListView("Group / Task", "GroupTask", 180, false);

            myInputTable.Columns.Add("Function");
            addColumnToListView("Function", "Function", 100, false);

            // Add a column for each person
            var x = (from r in dtUserPrivs.AsEnumerable() select r["Username"]).Distinct().ToList();
            foreach (string user in x)
            {
                myInputTable.Columns.Add(user);
                addDropDownColumnToListView(user, user, 100);
            }

            // Now add a row for each group and sub-function
            tablePresentationGroupButtons.DefaultView.Sort = "Order";
            tablePresentationGroupButtons = tablePresentationGroupButtons.DefaultView.ToTable();

            foreach (DataRow dr in tablePresentationGroups.Rows)
            {
                myInputTable.Rows.Add(dr["Description"]);

                foreach (DataRow drSub in tablePresentationGroupButtons.Rows)
                {
                    if (drSub["PresentationGroupID"].ToString() == dr["ID"].ToString())
                        myInputTable.Rows.Add("    " + drSub["Line1Text"], drSub["Function"].ToString());
                }
            }

            // Now add in the user permissions
            foreach (DataRow drPerm in dtUserPrivs.Rows)
            {
                // Find matching row
                foreach (DataRow drSub in tablePresentationGroupButtons.Rows)
                {
                    if (drSub["Function"].ToString() == drPerm["Function"].ToString())
                    {
                        // Find row in the grid
                        foreach (DataRow drGridRow in myInputTable.Rows)
                        {
                            if (drGridRow["Function"].ToString() == drPerm["Function"].ToString())
                            {
                                drGridRow[drPerm["Username"].ToString()] = drPerm["Permission"].ToString();
                            }
                        }
                    }
                }
            }

            dgPermissions.ItemsSource = myInputTable.DefaultView;

        }

        private void addColumnToListView(string columnName, string binding, int width, bool colorize)
        {
            DataGridTextColumn item = new DataGridTextColumn();
            item.Header = columnName;
            item.Width = width;
            item.Binding = new Binding(binding);
            dgPermissions.Columns.Add(item);
        }

        private void addDropDownColumnToListView(string columnName, string binding, int width)
        {
            DataGridComboBoxColumn dgCB = new DataGridComboBoxColumn();
            dgCB.Header = columnName;
            Binding b = new Binding(binding);
            b.Mode = BindingMode.TwoWay;
            dgCB.SelectedValueBinding = new Binding(binding);
            dgCB.Width = 100;
            List<String> options2 = new List<string>() { "", "Modify", "Read" };
            dgCB.ItemsSource = options2;
            dgPermissions.Columns.Add(dgCB);
        }

        private void dgPermissions_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (!(e.EditingElement is ComboBox))
                return;

            string permission = "";
            if (((ComboBox)e.EditingElement).SelectedIndex != -1)
                permission = ((ComboBox)e.EditingElement).SelectedItem.ToString();
            string user = e.Column.Header.ToString();
            string function = ((System.Data.DataRowView)(e.Row.Item)).Row["Function"].ToString();

            if (function == "")
                return;

            ////// FIND ID NUMBER OF EXISTING PRIV - IF THERE IS ONE //////
            foreach (DataRow drUser in dtUserPrivs.Rows)
            {
                if ( (drUser["Username"].ToString().ToLower() == user) && (drUser["Function"].ToString() == function))
                {
                    string id = drUser["ID"].ToString();
                    updatePermissions(id, user, function, permission, "D");
                }
            }

            updatePermissions("0", user, function, permission, "I");
        }

        private void dgPermissions_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ourMainWindow.showMainScreen();
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            txtNewUser.Text = "";
            gridAddUser.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnAddUserConfirm_Click(object sender, RoutedEventArgs e)
        {
            // Cananot be blank
            if (txtNewUser.Text == "")
            {
                MessageBox.Show("You must enter a username");
                return;
            }

            // Add the domain if it isn't there
            string newuser = txtNewUser.Text.ToLower();
            if (newuser.IndexOf(@"\") < 0)
                newuser = @"trg\" + newuser;

            // Cannot already exist
            foreach (DataRow drUser in dtUserPrivs.Rows)
                if (drUser["Username"].ToString().ToLower() == newuser)
                {
                    MessageBox.Show("This user already exists");
                    return;
                }

            updatePermissions("0", newuser, "", "", "I");
            buildDataTable();

            gridAddUser.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnCancelAdd_Click(object sender, RoutedEventArgs e)
        {
            gridAddUser.Visibility = System.Windows.Visibility.Hidden;
        }
    }


    public class MyBkColorConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //DataRowView drv = value as DataRowView;
            //if (drv != null)
            {
                //DateTime dt = DateTime.Parse(drv[2].ToString());
                //if (dt.Day % 2 == 0) //If it's a even number day.
                if (value == null)
                    return Brushes.Transparent;

                if (((String)value).StartsWith(" "))
                    return Brushes.Yellow;
                return Brushes.White;

            }
        }

        public static Brush ColorToBrush(string color) // color = "#E7E44D"
        {
            color = color.Replace("#", "");
            if (color.Length == 8)
            {
                return new SolidColorBrush(Color.FromArgb(255,
                    byte.Parse(color.Substring(2, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(4, 2), System.Globalization.NumberStyles.HexNumber),
                    byte.Parse(color.Substring(6, 2), System.Globalization.NumberStyles.HexNumber)));
            }
            else
            {
                return new SolidColorBrush(Colors.White);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

}
