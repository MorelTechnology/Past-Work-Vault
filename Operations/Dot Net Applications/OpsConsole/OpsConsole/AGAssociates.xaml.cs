using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for AGAssociates.xaml
    /// </summary>
    public partial class AGAssociates : UserControl
    {
        public DataTable dtAssoc = null;

        public enum mode { add, edit, remove, removeSecondRow };
        public mode eMode = mode.add;

        string editID = "";
        string editAssociateID = "";

        public AGAssociates()
        {
            InitializeComponent();
        }

        public void Load(DataTable dtDepartments, DataTable dtGroups, DataTable dtAssociates, DataTable dtClaimsCenterLimits, DataTable dtDataHavenLimits)
        {
            cbDepartment.ItemsSource = dtDepartments.DefaultView;
            cbGroup.ItemsSource = dtGroups.DefaultView;
            txtFirst.Text = "";
            txtLast.Text = "";
            txtTicket.Text = "";
            cbDepartment.SelectedIndex = -1;
            cbGroup.SelectedIndex = -1;
            dtAssoc = dtAssociates;
            eMode = mode.add;
            lblTitle.Text = "Add Associate";
            btnSave.Content = "Save";
            txtFirst.IsReadOnly = txtLast.IsReadOnly = cbDepartment.IsReadOnly = cbGroup.IsReadOnly = false;
            cbDepartment.IsHitTestVisible = cbGroup.IsHitTestVisible = true;
            hideTicketInfo(hide: true);
            fillCCID(dtClaimsCenterLimits);
            fillDHID(dtDataHavenLimits);
        }

        private void fillCCID(DataTable dtClaimsCenterLimits)
        {
            DataTable dtCCNames = new DataTable();
            dtCCNames.Columns.Add("Name");
            dtCCNames.Columns.Add("ID");

            dtCCNames.Rows.Add("", "0");
            foreach (DataRow dr in dtClaimsCenterLimits.Rows)
            {
                if (!contains(dtCCNames, "ID", dr["ClaimCenterID"].ToString()))
                    dtCCNames.Rows.Add(dr["FirstName"].ToString() + " " + dr["Lastname"].ToString() + " - " + dr["ClaimCenterID"].ToString(), dr["ClaimCenterID"].ToString());
            }

            cbCCID.ItemsSource = dtCCNames.DefaultView;
        }

        private void fillDHID(DataTable dtDataHaven)
        {
            DataTable dtDHNames = new DataTable();
            dtDHNames.Columns.Add("Name");
            dtDHNames.Columns.Add("ID");

            // Sort associates by last name
            DataView dv = dtDataHaven.DefaultView;
            dv.Sort = "Name";
            DataTable sortedPeople = dv.ToTable();

            dtDHNames.Rows.Add("", "0");
            foreach (DataRow dr in sortedPeople.Rows)
            {
               if (!contains(dtDHNames, "ID", dr["Name"].ToString()))
                   dtDHNames.Rows.Add(dr["Name"].ToString(), dr["Name"].ToString());
            }

            cbDHID.ItemsSource = dtDHNames.DefaultView;
        }

        private bool contains(DataTable dt, string col, string val)
        {
            foreach (DataRow dr in dt.Rows)
                if (dr[col].ToString() == val)
                    return true;
            return false;
        }

        public void Edit(string associateID)
        {
            showUserDetails(associateID);
            eMode = mode.edit;
            lblTitle.Text = "Edit Associate";
        }

        public void Remove(string associateID)
        {
            showUserDetails(associateID);
            eMode = mode.remove;
            lblTitle.Text = "Confirm Remove";
            btnSave.Content = "Remove";
            txtFirst.IsReadOnly = txtLast.IsReadOnly = cbDepartment.IsReadOnly = cbGroup.IsReadOnly = true;
            cbDepartment.IsHitTestVisible = cbGroup.IsHitTestVisible = false;
        }

        public void RemoveSecond(string associateID)
        {
            showUserDetails(associateID);
            eMode = mode.removeSecondRow;
            lblTitle.Text = "Confirm Remove Second Row";
            btnSave.Content = "Remove";
            txtFirst.IsReadOnly = txtLast.IsReadOnly = cbDepartment.IsReadOnly = cbGroup.IsReadOnly = true;
            cbDepartment.IsHitTestVisible = cbGroup.IsHitTestVisible = false;
        }

        private void showUserDetails(string associateID)
        {
            foreach (DataRow drAssoc in dtAssoc.Rows)
            {
                if (drAssoc["AssociateID"].ToString() == associateID)
                {
                    txtFirst.Text = drAssoc["FirstName"].ToString();
                    txtLast.Text = drAssoc["LastName"].ToString();
                    
                    if (drAssoc["DepartmentID"] is System.DBNull)
                        cbDepartment.SelectedIndex = -1;
                    else
                        cbDepartment.SelectedValue = drAssoc["DepartmentID"].ToString();

                    if (drAssoc["GroupID"] is System.DBNull)
                        cbGroup.SelectedIndex = -1;
                    else
                        cbGroup.SelectedValue = drAssoc["GroupID"].ToString();
                    editAssociateID = associateID;
                    editID = drAssoc["ID"].ToString();

                    cbCCID.SelectedValue = drAssoc["ClaimCenterID"].ToString();
                    cbDHID.SelectedValue = drAssoc["DataHavenID"].ToString();

                    rectAssociate.Opacity = (drAssoc["AssociateFlg"].ToString() == "G") ? 1d : 0.2d;
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ourMainWindow.screenAG.reloadTables();
            MainWindow.ourMainWindow.screenAG.reParseData();
            Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string firstName = txtFirst.Text.Trim();
            string lastName = txtLast.Text.Trim();

            // Check for duplicates
            if (eMode == mode.add)
            {
                foreach (DataRow drAssoc in dtAssoc.Rows)
                {
                    if ((drAssoc["FirstName"].ToString().ToLower() == firstName.ToLower()) && (drAssoc["LastName"].ToString().ToLower() == lastName.ToLower()))
                    {
                        MessageBox.Show("An associate with this name already exists");
                        return;
                    }
                }
            }

            if (cbGroup.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a group");
                return;
            }

            if (cbDepartment.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a department");
                return;
            }

            //if (cbCCID.SelectedIndex < 0)
            //{
            //    MessageBox.Show("You must select a Claims Center ID, use the top (blank) row for none.");
            //    return;
            //}

            if (txtTicket.Text == "")
            {
                MessageBox.Show("You must enter a ticket number");
                return;
            }

            if (MainWindow.ourMainWindow.showTicket(tbNote, txtTicket.Text) == false)
            {
                MessageBox.Show("The ServiceDesk Ticket number is invalid. You must enter a valid ticket number.");
                return;
            }

            string ccid = "0";
            if (cbCCID.SelectedIndex >= 0)
                ccid = cbCCID.SelectedValue.ToString();

            string dhid = "";
            if (cbDHID.SelectedIndex >= 0)
                dhid = cbDHID.SelectedValue.ToString();

            // Build the input table
            DataTable dtSave = new DataTable("AssociateInput");
            dtSave.Columns.Add("ID");
            dtSave.Columns.Add("ClaimCenterID");
            dtSave.Columns.Add("AssociateID");
            dtSave.Columns.Add("GroupID");
            dtSave.Columns.Add("LastName");
            dtSave.Columns.Add("FirstName");
            dtSave.Columns.Add("DepartmentID");
            dtSave.Columns.Add("AssociateFlg");
            dtSave.Columns.Add("TicketNumber");
            dtSave.Columns.Add("Operation");
            dtSave.Columns.Add("DataHavenID");

            string assocFlag = (rectAssociate.Opacity == 1d) ? "G" : "";

            if (eMode == mode.edit)
                dtSave.Rows.Add(editID, ccid, editAssociateID, cbGroup.SelectedValue.ToString(), lastName, firstName, cbDepartment.SelectedValue.ToString(), assocFlag, txtTicket.Text, "U", dhid);
            else if (eMode == mode.remove)
                dtSave.Rows.Add(editID, 0, editAssociateID, "", lastName, firstName, "", "", txtTicket.Text, "D", dhid);
            else if (eMode == mode.add)
                dtSave.Rows.Add(0, ccid, (MainWindow.ourMainWindow.maxval(dtAssoc, "AssociateID") + 1).ToString(), cbGroup.SelectedValue.ToString(), lastName, firstName, cbDepartment.SelectedValue.ToString(), assocFlag, txtTicket.Text, "I", dhid);
            else if (eMode == mode.removeSecondRow)
                MainWindow.ourMainWindow.screenAG.removeSecondRow(editAssociateID, txtTicket.Text);

            if (ScriptEngine.script.runScript(dtSave, "AUTHORITYGUIDELINES_ASSOCIATE", "OPSCONSOLE") != null)
            {
                Visibility = System.Windows.Visibility.Hidden;
                // was this until 9/16 MainWindow.ourMainWindow.processCommand("AG");

                MainWindow.ourMainWindow.screenAG.reloadTables();
                MainWindow.ourMainWindow.screenAG.reParseData();
            }
        }

        private void btnLookupTicket_Click(object sender, RoutedEventArgs e)
        {
            if (txtTicket.Text == "")
            {
                MessageBox.Show("Enter a ticket number");
                return;
            }

            hideTicketInfo(hide:false);
            MainWindow.ourMainWindow.showTicket(tbNote, txtTicket.Text);
        }

        private void btnHideTicketInfo_Click(object sender, RoutedEventArgs e)
        {
            hideTicketInfo(hide:true);
        }

        private void hideTicketInfo(bool hide)
        {
            btnHideTicketInfo.Visibility = (hide) ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
            gridTicket.Visibility = (hide) ? System.Windows.Visibility.Hidden : System.Windows.Visibility.Visible;
            lblCC.Visibility = (hide) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Hidden;
        }

        private void btnAssocFlag_Click(object sender, RoutedEventArgs e)
        {
            rectAssociate.Opacity = (rectAssociate.Opacity < 1d) ? 1d : 0.2d;
        }
    }
}
