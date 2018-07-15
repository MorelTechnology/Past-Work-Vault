using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for Admin2.xaml
    /// </summary>
    public partial class Admin2 : UserControl
    {
        #region class variables and constructor
        public bool loaded = false;                         // True after initial load
        DataTable dtAllUserPrivs = null;                    // Privs including history
        DataTable dtFilteredUserPrivs = null;               // Current privs, no history
        DataTable tablePresentationGroups = null;           // Function groups (boxes on the main screen)
        DataTable tablePresentationGroupButtons = null;     // Functions (buttons on the main screen)
        DataSet dsCrudConfig = null;                        // DataSet all tables returned by CRUD_CONFIGURATION
        public DataTable dtAssociates = new DataTable();    // List of associates from Active Directory

        public Admin2()
        {
            InitializeComponent();
        }
        #endregion

        #region data loading
        public void Load(string permissions)
        {
            screenResourceOrTicket.setParent(this);
            loadAssociates();
            loadCrudConfig();
            getTables();

            if (loaded)
                return;

            getGroups();
            fillFunctions();
            loaded = true;

            // Next 4 lines Oct 2 2016
            btnAddAssociateToFunction.IsEnabled = permissions == "Modify";
            btnChangeAssociateToReadOnly.IsEnabled = permissions == "Modify";
            btnRemoveAssociateFromFunction.IsEnabled = permissions == "Modify";
            btnChangeAssociateToModify.IsEnabled = permissions == "Modify";
        }

        private void loadAssociates()
        {
            // Aug 1 2016
            dtAssociates = ScriptEngine.script.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "SD_GET_ASSOCIATES", "OPSCONSOLE").Tables["WS"];
            // dtAssociates = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "SD_GET_ASSOCIATES", "OPSCONSOLE").Tables["WS"];
        }

        private void loadCrudConfig()
        {
            dsCrudConfig = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "CRUD_CONFIGURATION", "OPSCONSOLE");
        }

        private void getTables()
        {
            dtAllUserPrivs = dsCrudConfig.Tables["UserPermissions"];
            dtFilteredUserPrivs = dtAllUserPrivs.Clone();
            DataRow[] dr = dtAllUserPrivs.Select("EndDate is null", "Permission");
            if (dr.Length > 0)
                dtFilteredUserPrivs = dr.CopyToDataTable();
        }

        private void getGroups()
        {
            tablePresentationGroups = dsCrudConfig.Tables["PresentationGroups"];
            tablePresentationGroupButtons = dsCrudConfig.Tables["PresentationGroupButtons"];

            bool ignore = false;
            foreach (DataRow dr in tablePresentationGroupButtons.Rows)
            {
                if (dr["Line1Text"].ToString() == "Automated Reminders")
                    ignore = true;
            }
            if (!ignore)
                tablePresentationGroupButtons.Rows.Add("27", "4", "Automated Reminders", "", "SDTICKET", "8", "Y");
        }
        #endregion

        #region datatable lookup helpers
        private string SamAccountNameToName(string sam)
        {
            sam = sam.ToLower().Replace("trg\\","");
            foreach (DataRow dr in dtAssociates.Rows)
                if (dr["SamAccountName"].ToString().ToLower() == sam)
                    return dr["AdjustedName"].ToString();
            return sam;
        }

        private string NameToSamAccountName(string name)
        {
            name = name.ToLower();
            foreach (DataRow dr in dtAssociates.Rows)
                if (dr["AdjustedName"].ToString().ToLower() == name)
                    return dr["SamAccountName"].ToString();

            // prior to Dec 29. 2016: return "";
            return name;
        }

        private string FunctionToLine1(string function)
        {
            foreach (DataRow dr in tablePresentationGroupButtons.Rows)
                if (dr["Function"].ToString().ToUpper() == function.ToUpper())
                    return dr["Line1Text"].ToString();
            return "";
        }
        #endregion

        #region event handlers
        private void dgPINSInput_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void dgFeatures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            processFeatureSelection();
        }

        private void dgFunctionMembers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            processAssociateSelection();
        }

        private void btnAddAssociateToFunction_Click(object sender, RoutedEventArgs e)
        {
            if (!arePrerequisitesOK(false, "to add an associate"))
                return;

            DataRow dr = ((System.Data.DataRowView)(dgFeatures.SelectedItem)).Row;

            bool ticketRequired = (dr["RequiresTicket"].ToString() == "Y");

            openResourceOrTicket();
            screenResourceOrTicket.AddUserMode(ticketRequired, dr["Line1Text"].ToString(), dr["Function"].ToString());
        }

        private void btnChangeAssociateToReadOnly_Click(object sender, RoutedEventArgs e)
        {
            getPermissionChangeTicketNumber("Read");
        }

        private void btnChangeAssociateToModify_Click(object sender, RoutedEventArgs e)
        {
            getPermissionChangeTicketNumber("Modify");
        }

        private void btnRemoveAssociateFromFunction_Click(object sender, RoutedEventArgs e)
        {
            getPermissionChangeTicketNumber("Remove");
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            ((Image)sender).Source = null;
        }
        #endregion

        #region create backing tables
        private void fillFunctions()
        {
            tablePresentationGroupButtons.DefaultView.Sort = "Order";
            tablePresentationGroupButtons = tablePresentationGroupButtons.DefaultView.ToTable();

            DataTable myInputTable = new DataTable("InputTable");
            myInputTable.Columns.Add("Icon");
            myInputTable.Columns.Add("HasIcon");
            myInputTable.Columns.Add("GroupTask");
            myInputTable.Columns.Add("Function");
            myInputTable.Columns.Add("Line1Text");
            myInputTable.Columns.Add("RequiresTicket");

            foreach (DataRow dr in tablePresentationGroups.Rows)
            {
                myInputTable.Rows.Add("images/" + dr["Image"], "Visible", dr["Description"], "", "");

                foreach (DataRow drSub in tablePresentationGroupButtons.Rows)
                {
                    if (drSub["PresentationGroupID"].ToString() == dr["ID"].ToString())
                        myInputTable.Rows.Add("", "Collapsed", "    " + drSub["Line1Text"], drSub["Function"].ToString(), drSub["Line1Text"], drSub["RequiresTicket"].ToString());
                }
            }

            dgFeatures.ItemsSource = myInputTable.DefaultView;
        }

        private void FillFunctionMembers(string function)
        {
            DataTable myInputTable = new DataTable("InputTable");
            myInputTable.Columns.Add("Associate");
            myInputTable.Columns.Add("Access");
            myInputTable.Columns.Add("AssociateID");

            foreach (DataRow dr in dtFilteredUserPrivs.Rows)
            {
                if (dr["Function"].ToString() == function)
                {
                    string username = SamAccountNameToName(dr["Username"].ToString());

                    if (dr["Permission"].ToString() != "")
                    {
                        myInputTable.Rows.Add(username, dr["Permission"]);
                    }
                }
            }

            dgFunctionMembers.ItemsSource = myInputTable.DefaultView;
        }

        private void showHistoryForAssociate(string associate)
        {
            lblHistorySubLabel.Text = "for " + associate;

            string sam = "trg\\" + NameToSamAccountName(associate);
            if (sam == "trg\\")
                return;


            colAssociate.Visibility = System.Windows.Visibility.Collapsed;
            colFunction.Visibility = System.Windows.Visibility.Visible;


            DataTable dtHistory = new DataTable();
            dtHistory.Columns.Add("Function");
            dtHistory.Columns.Add("Associate");
            dtHistory.Columns.Add("Access");
            dtHistory.Columns.Add("From");
            dtHistory.Columns.Add("To");
            dtHistory.Columns.Add("Ticket");
            dtHistory.Columns.Add("by");

            foreach (DataRow drp in dtAllUserPrivs.Rows)
            {
                if (drp["UserName"].ToString().ToUpper() == sam.ToUpper())
                {
                    string function = drp["Function"].ToString();
                    if (function != "")
                    {
                        string functiondesc = FunctionToLine1(function);
                        string permission = (drp["Permission"].ToString() == "") ? "None" : drp["Permission"].ToString();
                        DateTime dtFrom = Convert.ToDateTime(drp["StartDate"].ToString());
                        string from = (dtFrom.Year == 1900) ? "Initial" : dtFrom.ToString("dd-MMM-yyyy");
                        string to = "";
                        if (!(drp["EndDate"] is System.DBNull))
                            to = (Convert.ToDateTime(drp["EndDate"].ToString())).ToString("dd-MMM-yyyy");
                        string ticket = drp["TicketNumber"].ToString();
                        string by = SamAccountNameToName(drp["StartUser"].ToString());
                        if (functiondesc != "")
                            dtHistory.Rows.Add(functiondesc, "", permission, from, to, ticket, by);
                    }
                }
            }

            dtHistory.DefaultView.Sort = "Function, To";
            dgHistory.ItemsSource = dtHistory.DefaultView;



        }

        private void showHistoryForFunction(string function)
        {
            if (dgFeatures.SelectedIndex < 0)
                return;

            DataRow dr = ((System.Data.DataRowView)(dgFeatures.SelectedItem)).Row;
            lblHistorySubLabel.Text = "for " + dr["Line1Text"].ToString();

            colAssociate.Visibility = System.Windows.Visibility.Visible;
            colFunction.Visibility = System.Windows.Visibility.Collapsed;

            DataTable dtHistory = new DataTable();
            dtHistory.Columns.Add("Function");
            dtHistory.Columns.Add("Associate");
            dtHistory.Columns.Add("Access");
            dtHistory.Columns.Add("From");
            dtHistory.Columns.Add("To");
            dtHistory.Columns.Add("Ticket");
            dtHistory.Columns.Add("by");

            if (function == "")
            {
                dgHistory.ItemsSource = dtHistory.DefaultView;
                return;
            }

            foreach (DataRow drp in dtAllUserPrivs.Rows)
            {
                if (drp["Function"].ToString().ToUpper() == function.ToUpper())
                {
                    string name = SamAccountNameToName(drp["Username"].ToString());
                    string permission = (drp["Permission"].ToString() == "") ? "None" : drp["Permission"].ToString();
                    DateTime dtFrom = Convert.ToDateTime(drp["StartDate"].ToString());
                    string from = (dtFrom.Year == 1900) ? "Initial" : dtFrom.ToString("dd-MMM-yyyy");
                    string to = "";
                    if (!(drp["EndDate"] is System.DBNull))
                        to = (Convert.ToDateTime(drp["EndDate"].ToString())).ToString("dd-MMM-yyyy");
                    string ticket = drp["TicketNumber"].ToString();
                    string by = SamAccountNameToName(drp["StartUser"].ToString());

                    dtHistory.Rows.Add("", name, permission, from, to, ticket, by);
                }
            }

            dtHistory.DefaultView.Sort = "Associate, To";
            dgHistory.ItemsSource = dtHistory.DefaultView;
        }
        #endregion

        #region screen logic
        private void processFeatureSelection()
        {
            if (dgFeatures.SelectedIndex < 0)
                return;

            string function = ((System.Data.DataRowView)(dgFeatures.SelectedItem)).Row["Function"].ToString();
            FillFunctionMembers(function);
            showHistoryForFunction(function);
        }

        private void processAssociateSelection()
        {
            if (dgFunctionMembers.SelectedIndex < 0)
                return;

            if (!(dgFunctionMembers.SelectedItem is System.Data.DataRowView))
                return;

            string associate = ((System.Data.DataRowView)(dgFunctionMembers.SelectedItem)).Row["Associate"].ToString();
            showHistoryForAssociate(associate);
        }

        private bool arePrerequisitesOK(bool needsAssociate, string message)
        {
            if (dgFeatures.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a function " + message);
                return false;
            }

            string function = ((System.Data.DataRowView)(dgFeatures.SelectedItem)).Row["Function"].ToString();
            if (function == "")
            {
                MessageBox.Show("You must select a valid function row " + message);
                return false;
            }

            if (needsAssociate)
            {
                if (dgFunctionMembers.SelectedIndex < 0)
                {
                    MessageBox.Show("You must select an associate " + message);
                    return false;
                }
            }

            return true;
        }

        private void getPermissionChangeTicketNumber(string change)
        {
            if (!arePrerequisitesOK(true, ""))
                return;

            DataRow dr = ((System.Data.DataRowView)(dgFeatures.SelectedItem)).Row;
            DataRow drFM = ((System.Data.DataRowView)(dgFunctionMembers.SelectedItem)).Row;

            ////// IF A TICKET IS REQUIRED //////
            if (dr["RequiresTicket"].ToString() == "Y")
            {
                openResourceOrTicket();
                screenResourceOrTicket.EnterTicketMode(change, dr["Line1Text"].ToString(), dr["Function"].ToString(), drFM["Associate"].ToString());
            }

            ////// TICKET NOT REQUIRED //////
            else
            {
                ModifyPermissions(change, "", drFM["Associate"].ToString(), dr["Function"].ToString());
            }
        }

        public void AddAssociateToFunction(string perm, string ticket, string associate, string function)
        {
            string sam = NameToSamAccountName(associate);

            sam = "trg\\" + sam;

            ////// If they are already there just do an update //////
            foreach (DataRow dr in dtFilteredUserPrivs.Rows)
            {
                if ((dr["Username"].ToString().ToLower() == sam.ToLower()) && (dr["Function"].ToString().ToUpper() == function.ToUpper()))
                {
                    updatePermissions(dr["ID"].ToString(), sam, function, perm, "U", ticket);
                    return;
                }
            }
            updatePermissions("0", sam, function, perm, "I", ticket);
        }
        #endregion

        #region modify tables with RSSE
        public void ModifyPermissions(string mode, string ticket, string associate, string function)
        {
            string sam = "trg\\" + NameToSamAccountName(associate);

            foreach (DataRow dr in dtFilteredUserPrivs.Rows)
                if ((dr["Username"].ToString().ToLower() == sam.ToLower()) && (dr["Function"].ToString().ToUpper() == function.ToUpper()))
                {
                    if (mode == "Remove")
                        updatePermissions(dr["ID"].ToString(), sam, function, "", "U", ticket);
                    else
                        updatePermissions(dr["ID"].ToString(), sam, function, mode, "U", ticket);
                }
        }

        private void updatePermissions(string id, string username, string function, string permission, string operation, string ticket)
        {
            DataTable myInputTable = new DataTable("UserPermissions");
            myInputTable.Columns.Add("ID", typeof(long));
            myInputTable.Columns.Add("Username");
            myInputTable.Columns.Add("Function");
            myInputTable.Columns.Add("Permission");
            myInputTable.Columns.Add("Operation");
            myInputTable.Columns.Add("TicketNumber");

            myInputTable.Rows.Add(id, username, function, permission, operation, ticket);

            dsCrudConfig = ScriptEngine.script.runScript(ScriptEngine.envCurrent, myInputTable, "CRUD_CONFIGURATION", "OPSCONSOLE");
            //dtUserPrivs = ds.Tables["UserPermissions"];
            getTables();
            processFeatureSelection();
        }
        #endregion

        #region animation
        public void openResourceOrTicket()
        {
            screenResourceOrTicket.Visibility = System.Windows.Visibility.Visible;
            screenResourceOrTicket.Opacity = 0d;

            DoubleAnimation da = new DoubleAnimation();
            da.From = 0d;
            da.To = 1d;
            da.Duration = new Duration(TimeSpan.FromSeconds(1d));
            screenResourceOrTicket.BeginAnimation(OpacityProperty, da);
        }


        public void closeResourceOrTicket()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1d;
            da.To = 0d;
            da.Duration = new Duration(TimeSpan.FromSeconds(1d));
            da.Completed += hideResourceOrTicketWindow;
            screenResourceOrTicket.BeginAnimation(OpacityProperty, da);
        }

        public void hideResourceOrTicketWindow(object sender, EventArgs e)
        {
            screenResourceOrTicket.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion

    }
}
