using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
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
    /// Interaction logic for AuthorityGuidelines.xaml
    /// </summary>
    public partial class AuthorityGuidelines : UserControl
    {
        public bool loaded = false;
        public int currentGroupSelection = -1;
        public bool readOnly = false;

        #region DataTables
        DataTable dtAssociates = null;
        DataTable dtAssociateLimits = null;
        DataTable dtGroups = null;
        DataTable dtDepartments = null;
        DataTable dtLimits = null;
        DataTable dtGrid = null;
        DataTable dtNotes = null;
        DataTable dtEdits = null;
        DataTable dtDataHavenLimits = null;
        DataTable dtClaimsCenterLimits = null;
        #endregion

        #region Animation state
        public class optionAnimation
        {
            public enum oaStatus { open, opening, closed, closing };
            public oaStatus eStatus = oaStatus.closed;
            public double progress = 0;
            public double closedHeight = 23d;
            public double fullHeight = 212;
        }
        optionAnimation oa = new optionAnimation();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        #endregion

        #region Constants
        string ccwmlimithdr = "Claims Center WM Limit";     // Type 9
        string ccpaylimithdr = "Claims Center Pay Limit";   // Type 6
        string dhexplimithdr = "Data Haven Expense";
        string colorErrorRed = "#FFE4A7A7";
        string colorErrorRedChoice2 = "#FFC96666"; // 201 102 102
        string colorErrorRedChoice1 = "#FFE4A7A7"; // 228 167 167
        #endregion

        #region Constructor and Initialization
        public AuthorityGuidelines()
        {
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer.Start();
            oa.eStatus = optionAnimation.oaStatus.open;
            InitializeComponent();
        }

        public void Load(string permissions)
        {
            //if (loaded)
            //    return;

            dgAG.ItemsSource = null;
            getDataHaven();
            getClaimsCenter();
            getNotes();
            getTables();
            getAssociateLimits();

            if (dtAssociates == null)
            {
                MessageBox.Show("Cannot get tables from script engine.");
                return;
            }

            fillGroupsCombo();
            fillReportsCombo();
            loaded = true;

            bool enabled = true;
            if (permissions == "Modify")
            {
                readOnly = false;

                //btnSave.Visibility = System.Windows.Visibility.Visible;
                //btnAddSecondRow.Visibility = System.Windows.Visibility.Visible;
                //btnUndoEdit.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                readOnly = true;
                enabled = false;
                //btnSave.Visibility = System.Windows.Visibility.Hidden;
                //btnAddSecondRow.Visibility = System.Windows.Visibility.Hidden;
                //btnUndoEdit.Visibility = System.Windows.Visibility.Hidden;
            }

            btnSave.IsEnabled = enabled;
            btnAddAssociate.IsEnabled = enabled;
            btnRemoveAssociate.IsEnabled = enabled;
            btnEditAssociate.IsEnabled = enabled;
            btnAddSecondRow.IsEnabled = enabled;
            btnRemoveSecondRow.IsEnabled = enabled;
            btnUndoEdit.IsEnabled = enabled;

            screenAssociates.Visibility = System.Windows.Visibility.Hidden;
            screenAssociates.Margin = new Thickness(0);
        }
        #endregion

        #region Load Tables from Script Engine
        private void getDataHaven()
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "DATAHAVENFORNAV_LIMITS", "OPSCONSOLE");
            if (ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
            {
                MessageBox.Show("An error has occurred in the DATAHAVENFORNAV_LIMITS script: " + ds.Tables[0].Rows[0][0].ToString());
                return;
            }

            dtDataHavenLimits = ds.Tables["DHNLimits"];
        }

        private void getClaimsCenter()
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "CLAIM_CENTER_LIMITS", "OPSCONSOLE");
            if (ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
            {
                MessageBox.Show("An error has occurred in the CLAIM_CENTER_LIMITS script: " + ds.Tables[0].Rows[0][0].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["SE_CustomErrorCode"].ToString() + Environment.NewLine + ds.Tables[0].Rows[0]["SE_ScriptName"].ToString());
                return;
            }
            dtClaimsCenterLimits = ds.Tables["CCLimits"];
        }

        public void reloadTables()
        {
            getTables();
            getAssociateLimits();
        }

        private void getTables()
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AUTHORITYGUIDELINES_ASSOCIATE", "OPSCONSOLE");
            if (ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
            {
                MessageBox.Show("An error has occurred in the AUTHORITYGUIDELINES_ASSOCIATE script: " + ds.Tables[0].Rows[0][0].ToString());
                return;
            }

            dtAssociates = ds.Tables["Associates"];
            dtGroups = ds.Tables["Groups"];

            // Filter our data based on the selected group
            DataTable dtAllDepartments = ds.Tables["Departments"];
            dtDepartments = dtAllDepartments.Clone();
            var drDepartments = dtAllDepartments.Select("EndDate is null");
            foreach (DataRow dr in drDepartments)
                dtDepartments.ImportRow(dr);

            dtLimits = ds.Tables["Limits"];
        }

        private void getAssociateLimits()
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AUTHORITYGUIDELINES_ASSOCIATE", "OPSCONSOLE");
            if (ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
            {
                MessageBox.Show("An error has occurred in the AUTHORITYGUIDELINES_ASSOCIATE script: " + ds.Tables[0].Rows[0][0].ToString());
                return;
            }
            dtAssociateLimits = ds.Tables["AssociateLimits"];
        }

        private void getNotes()
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "AUTHORITYGUIDELINES_NOTE", "OPSCONSOLE");
            if (ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
            {
                MessageBox.Show("An error has occurred in the AUTHORITYGUIDELINES_NOTE script: " + ds.Tables[0].Rows[0][0].ToString());
                return;
            }
            dtNotes = ds.Tables["Notes"];
        }
        #endregion

        #region Create dynamic Data Tables
        private void createEditsDataTable()
        {
            // Create datatable to keep track of edits
            dtEdits = new DataTable("Edits");
            dtEdits.Columns.Add("Person");
            dtEdits.Columns.Add("Limit");
            dtEdits.Columns.Add("From");
            dtEdits.Columns.Add("To");
            dtEdits.Columns.Add("Ticket");
            dtEdits.Columns.Add("AssociateID");
            dtEdits.Columns.Add("LimitID");
            dtEdits.Columns.Add("Type");
            dtEdits.Columns.Add("TicketTextColor");
        }

        private void createGridDataTable()
        {
            dtGrid = new DataTable("Grid");
            dtGrid.Columns.Add("Name", typeof(string));
            dtGrid.Columns.Add("Department", typeof(string));
            dtGrid.Columns.Add("AssociateID", typeof(string));
            dtGrid.Columns.Add("DepartmentID", typeof(string));
            dtGrid.Columns.Add("Type", typeof(string));

            dtGrid.Columns.Add(ccwmlimithdr, typeof(string));
            dtGrid.Columns.Add(ccpaylimithdr, typeof(string));
            dtGrid.Columns.Add(dhexplimithdr, typeof(string));

            // The color of the Claims Center and DataHaven columns will be bound to following three columns in the Data Table.
            // The normal colors for claims center will be blue and data haven will be green, however, they will turn red if they don't match
            // the corresponding Authority Guideline color
            dtGrid.Columns.Add("DHCOLOR", typeof(string));
            dtGrid.Columns.Add("CC6COLOR", typeof(string));
            dtGrid.Columns.Add("CC9COLOR", typeof(string));

            addColumnToDataGrid("Name", "Name", 120, false, "");
            addColumnToDataGrid("Department", "Department", 155, false, "");
        }
        #endregion

        #region Group Selection
        private void fillGroupsCombo()
        {
            if (dtGroups != null)
                cbGroup.ItemsSource = dtGroups.DefaultView;
        }

        private void cbGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbGroup.SelectedIndex == currentGroupSelection)
                return;

            if (!(dtEdits == null) && (dtEdits.Rows.Count > 0))
            {
                if ((MessageBox.Show("You have switched groups with unsaved edits. Switching groups will lose your changes. Are you sure you want to proceed?", "Confirm group change", MessageBoxButton.YesNo) != MessageBoxResult.Yes))
                {
                    cbGroup.SelectedIndex = currentGroupSelection;
                    return;
                }
                else
                {
                    dtEdits.Rows.Clear();
                    dgEdits.ItemsSource = dtEdits.DefaultView;
                }
            }

            currentGroupSelection = cbGroup.SelectedIndex;
            parseData();
        }
        #endregion

        #region ParseData from source DataTables to grid DataTable
        public void reParseData()
        {
            parseData();
        }

        private void parseData()
        {
            // Fast fail on no group selected
            if (cbGroup.SelectedIndex < 0)
                return;
            string groupID = cbGroup.SelectedValue.ToString();

            // Clear out the current grid
            dgAG.Columns.Clear();

            // DataTable for the grid and to track edits 
            createEditsDataTable();
            createGridDataTable();

            // Filter our data based on the selected group
            var drAssociatesFiltered = dtAssociates.Select("GroupID = " + groupID);

            // Add columns to the Grid DataTable and the Grid control for each limit
            foreach (DataRow drLimit in dtLimits.Rows)
            {
                string colname = "Limit" + drLimit["LimitID"].ToString();
                dtGrid.Columns.Add(colname, typeof(string));
                string header = drLimit["Description"].ToString();
                int width = ((header.IndexOf("Transactional") >= 0) ? 92 : 89);
                addColumnToDataGrid(drLimit["Description"].ToString(), colname, width, true, "");

                // Add Claims Center and DataHaven columns in the correct place
                if (colname == "Limit1")
                    addColumnToDataGrid(ccwmlimithdr, ccwmlimithdr, 85, true, "CC9COLOR");
                if (colname == "Limit2")
                    addColumnToDataGrid(ccpaylimithdr, ccpaylimithdr, 85, true, "CC6COLOR");
                if (colname == "Limit11")
                    addColumnToDataGrid(dhexplimithdr, dhexplimithdr, 85, true, "DHCOLOR");
            }

            // Sort associates by last name
            DataView dv = dtAssociates.DefaultView;
            dv.Sort = "LastName";
            dv.RowFilter = "GroupID=" + groupID;
            DataTable sortedAssociates = dv.ToTable();

            // Add the rows for each associate
            foreach (DataRow drAssociate in sortedAssociates.Rows)
            {
                if (drAssociate["EndDate"] is System.DBNull)
                {
                    dtGrid.Rows.Add(drAssociate["FirstName"].ToString() + " " + drAssociate["LastName"].ToString(), departmentName(drAssociate["DepartmentID"].ToString()), drAssociate["AssociateID"].ToString(),drAssociate["DepartmentID"].ToString(),"R");
                    if (associateHasCLimits(drAssociate["AssociateID"].ToString()))
                        dtGrid.Rows.Add(drAssociate["FirstName"].ToString() + " " + drAssociate["LastName"].ToString() + "/C", "", drAssociate["AssociateID"].ToString(),drAssociate["DepartmentID"].ToString(),"C");
                }
            }

            // Put in the limits for each associate
            foreach (DataRow drAL in dtAssociateLimits.Rows)
            {
                if (drAL["EndDate"] is DBNull)
                {
                    foreach (DataRow drG in dtGrid.Rows)
                    {
                        if (drG["Name"].ToString() == associateName(drAL["AssociateID"].ToString()) + ((drAL["Type"].ToString() == "C") ? "/C" : ""))
                        {
                            string limitID = drAL["LimitID"].ToString();
                            drG["Limit" + limitID] = formatLimit(drAL["Limit"] is DBNull ? 0.0 : Convert.ToDouble(drAL["Limit"]));
                        }

                    }
                }
            }

            // Add in limmits from Claims Center
            foreach (DataRow drG in dtGrid.Rows)
            {
                drG["CC6COLOR"] = "LightSkyBlue";
                drG["CC9COLOR"] = "LightSkyBlue";
            }
            foreach (DataRow drCC in dtClaimsCenterLimits.Rows)
            {
                foreach (DataRow drG in dtGrid.Rows)
                {
                    if (doNamesMatchCC(drG["Name"].ToString(), drCC["FirstName"].ToString() + " " + drCC["Lastname"].ToString(), drCC["UserName"].ToString()))
                    {
                        string formattedCCLimit = formatLimit(drCC["LimitAmount"] is DBNull ? 0.0 : Convert.ToDouble(drCC["LimitAmount"]));
                        if (drCC["LimitType"].ToString() == "9")
                        {
                            drG[ccwmlimithdr] = formattedCCLimit;
                            if (formattedCCLimit != drG["Limit1"].ToString())
                                drG["CC9COLOR"] = colorErrorRed;
                        }
                        if (drCC["LimitType"].ToString() == "6")
                        {
                            drG[ccpaylimithdr] = formattedCCLimit;
                            if (formattedCCLimit != drG["Limit2"].ToString())
                                drG["CC6COLOR"] = colorErrorRed;
                        }
                    }
                }
            }

            // Add in limmits from Date Haven
            foreach (DataRow drG in dtGrid.Rows)
            {
                drG["DHCOLOR"] = "DarkSeaGreen";
            }
            foreach (DataRow drDH in dtDataHavenLimits.Rows)
            {
                foreach (DataRow drG in dtGrid.Rows)
                {
                    if (doNamesMatchDH(drG["Name"].ToString(), drDH["Name"].ToString(), ""))
                    {
                        string formattedDHLimit = formatLimit(drDH["ExpenseLimit"] is DBNull ? 0.0 : Convert.ToDouble(drDH["ExpenseLimit"]), zeroShowsAs0: true);
                        drG[dhexplimithdr] = formattedDHLimit;

                        if (formattedDHLimit != drG["Limit11"].ToString())
                            drG["DHCOLOR"] = colorErrorRed;
                    }
                }
            }

            // Update the grid
            dgAG.ItemsSource = dtGrid.DefaultView;
        }
        #endregion

        #region Name Matching (between AG, DH and CC user names)
        private bool doNamesMatchCC(string name1, string name2, string userName)
        {
            if (name1.IndexOf("/C") > 0)
                return false;

            if (name1 == name2)
                return true;

            if ((userName != "") && (usernameFromName(name1).ToUpper() == userName.ToUpper()))
                return true;

            return false;
        }

        private bool doNamesMatchDH(string name1, string name2, string userName)
        {
            if (name1 == name2)
                return true;

            if (nicknameToName(name1) == nicknameToName(name2))
                return true;

            return false;
        }

        private string nicknameToName(string name)
        {
            name = name.Replace("Bill ", "William ");
            name = name.Replace("Mike ", "Michael ");
            name = name.Replace("Bob ", "Robert ");
            name = name.Replace("Tim ", "Timothy ");
            return name;
        }
        #endregion

        #region DataTable Helper Routines
        private string usernameFromName(string name)
        {
            try
            {
                if ((name.IndexOf(" ") + 1) > name.Length - 4)
                    return "";

                return @"TRG\" + name.Substring(0, 1) + name.Substring(name.IndexOf(" ") + 1, 4);
            }
            catch (Exception)
            {
                return "";
            }
        }

        private string formatLimit(double number, bool zeroShowsAs0=false)
        {
            if (number == -1)
                return "Unlimited";

            if (number == -2)
                return "999,999,999";

            if (number == -3)
                return "0";

            if ((zeroShowsAs0 == false) && (number == 0))
                return "";

            return number.ToString("#,##0");
        }

        private double limitToDouble(string limit)
        {
            limit = limit.Replace(",", "");

            if (limit.ToUpper().IndexOf("UNL") >= 0)
                return -1;
            if (limit == "")
                return 0d;
            if (limit == "999999999")
                return -2d;
            return Convert.ToDouble(limit);
        }

        private string departmentName(string departmentID)
        {
            foreach (DataRow drDEPT in dtDepartments.Rows)
            {
                if ((drDEPT["DepartmentID"].ToString() == departmentID))
                    return drDEPT["Description"].ToString();
            }
            return "";
        }

        private string associateName(string associateID)
        {
            foreach (DataRow drAssociate in dtAssociates.Rows)
            {
                if ((drAssociate["AssociateID"].ToString() == associateID))
                    return drAssociate["FirstName"].ToString() + " " + drAssociate["LastName"].ToString();
            }
            return "";
        }

        private bool associateHasCLimits(string associateID)
        {
            foreach (DataRow drAL in dtAssociateLimits.Rows)
            {
                if ((drAL["AssociateID"].ToString() == associateID) && (drAL["Type"].ToString() == "C") && (drAL["EndDate"] is System.DBNull))
                    return true;
            }
            return false;
        }

        private string limitIDFromDescription(string desc)
        {
            foreach (DataRow drLimit in dtLimits.Rows)
                if (drLimit["Description"].ToString() == desc)
                    return drLimit["LimitID"].ToString();
            return "";
        }

        private string lookup(DataTable dt, string fromField, string fromVal, string lookupField)
        {
            foreach (DataRow dr in dt.Rows)
                if (dr[fromField].ToString() == fromVal)
                    return dr[lookupField].ToString();
            return "";
        }
        #endregion

        #region DataGrid Helper Routines
        private void addColumnToDataGrid(string columnName, string binding, int width, bool rightjustify, string colorcolumn)
        {
            DataGridTextColumn item = new DataGridTextColumn();

            if (rightjustify)
            {
                Style styleReading = new Style(typeof(TextBlock));
                Setter s = new Setter();
                s.Property = DataGridCell.HorizontalAlignmentProperty;
                s.Value = HorizontalAlignment.Right;
                styleReading.Setters.Add(s);
                item.ElementStyle = styleReading;
            }
      
            item.Header = columnName;
            item.Width = width;
            item.Binding = new Binding(binding);

            if (colorcolumn != "")
            {
                item.CellStyle = new Style(typeof(DataGridCell));
                Setter s = new Setter();
                s.Property = DataGridCell.BackgroundProperty;
                s.Value = new Binding(colorcolumn);
                item.CellStyle.Setters.Add(s);
                item.IsReadOnly = true;
            }

            if (readOnly)
                item.IsReadOnly = true;

            dgAG.Columns.Add(item);
        }
        #endregion

        #region Editing
        private void dgAG_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string to = ((TextBox)e.EditingElement).Text;
            string col = e.Column.Header.ToString();
            string person = ((System.Data.DataRowView)(e.Row.Item)).Row[0].ToString();

            string limitID = limitIDFromDescription(col);
            if (limitID == "")
                return;

            string associateID = ((System.Data.DataRowView)(e.Row.Item)).Row["AssociateID"].ToString();
            string old = ((System.Data.DataRowView)(e.Row.Item)).Row["Limit"+limitID].ToString();
            string type = ((System.Data.DataRowView)(e.Row.Item)).Row["Type"].ToString();

            double val;

            if (to == "")
                to = "0";
            to = to.ToUpper();
            to = to.Replace("K", "000");
            to = to.Replace("M", "000000");
            if (to.StartsWith("Z"))
                to = "-3";
            if (to.IndexOf("U") >= 0)
                to = "-1";
            if (Double.TryParse(to, out val) == false)
            {
                MessageBox.Show("\"" + to + "\" is not a valid number. You may enter numbers like 10000 or 10,000 or u (for unlimited)");
                (e.EditingElement as TextBox).Text = old;
                return;
            }

            to = formatLimit(val);

            if ((val!=-3) && (to == old))
                return;

            (e.EditingElement as TextBox).Text = to;
            
            // Update existing
            bool reupdated = false;
            foreach (DataRow drExisting in dtEdits.Rows)
            {
                if ((drExisting["AssociateID"].ToString() == associateID) &&
                    (drExisting["LimitID"].ToString() == limitID) &&
                    (drExisting["Type"].ToString() == type))
                {
                    drExisting["To"] = (val == -3) ? "-3" : to;
                    reupdated = true;
                }
            }
            if (!reupdated)
                dtEdits.Rows.Add(person, col, old, (val == -3) ? "-3" : to, "", associateID, limitID, type, "Black");
            dgEdits.ItemsSource = dtEdits.DefaultView;

            if ("Limit" + limitID == "Limit11")
            {
                string formattedDHLimit = ((System.Data.DataRowView)(e.Row.Item)).Row[dhexplimithdr].ToString();
                if (formattedDHLimit != to)
                    ((System.Data.DataRowView)(e.Row.Item)).Row["DHCOLOR"] = colorErrorRed;
                else
                    ((System.Data.DataRowView)(e.Row.Item)).Row["DHCOLOR"] = "DarkSeaGreen";
            }

            if ("Limit" + limitID == "Limit1")
            {
                string formattedDHLimit = ((System.Data.DataRowView)(e.Row.Item)).Row[ccwmlimithdr].ToString();
                if (formattedDHLimit != to)
                    ((System.Data.DataRowView)(e.Row.Item)).Row["CC9COLOR"] = colorErrorRed;
                else
                    ((System.Data.DataRowView)(e.Row.Item)).Row["CC9COLOR"] = "LightSkyBlue";
            }

            if ("Limit" + limitID == "Limit2")
            {
                string formattedDHLimit = ((System.Data.DataRowView)(e.Row.Item)).Row[ccpaylimithdr].ToString();
                if (formattedDHLimit != to)
                    ((System.Data.DataRowView)(e.Row.Item)).Row["CC6COLOR"] = colorErrorRed;
                else
                    ((System.Data.DataRowView)(e.Row.Item)).Row["CC6COLOR"] = "LightSkyBlue";
            }

            dgAG.ItemsSource = dtGrid.DefaultView;
        }

        private void dgEdits_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string col = e.Column.Header.ToString();

            if (col != "Ticket")
                return;

            string ticket = ((TextBox)e.EditingElement).Text;
            ((System.Data.DataRowView)(e.Row.Item)).Row["Ticket"] = ticket;

            showTicket(ticket);
        }
        #endregion

        #region Selection
        private void dgAG_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count < 1)
                return;

            if (!(e.AddedCells[0].Item is System.Data.DataRowView))
                return;

            string header = e.AddedCells[0].Column.Header.ToString();
            string row = ((System.Data.DataRowView)(e.AddedCells[0].Item)).Row.ItemArray[0].ToString();
            string dept = ((System.Data.DataRowView)(e.AddedCells[0].Item)).Row.ItemArray[1].ToString();

            // Update the notes
            string limitID = limitIDFromDescription(header);
            updateNoteForLimitID(limitID);

            if (header == "Department")
            {
                lblChangeHistoryDescription.Text = "for " + departmentName(((System.Data.DataRowView)(e.AddedCells[0].Item)).Row["DepartmentID"].ToString());
                showHistory("", ((System.Data.DataRowView)(e.AddedCells[0].Item)).Row["DepartmentID"].ToString(), "");
            }

            else if (header == "Name")
            {
                string associateID = ((System.Data.DataRowView)(e.AddedCells[0].Item)).Row["AssociateID"].ToString();
                string name = lookup(dtAssociates, "AssociateID", associateID, "FirstName") + " " + lookup(dtAssociates, "AssociateID", associateID, "LastName");
                lblChangeHistoryDescription.Text = "for " + name;
                showHistory(associateID, "", "");
            }

            else
            {
                lblChangeHistoryDescription.Text = "for " + header;
                showHistory("", "", limitID);
            }

            
        }
        #endregion

        #region History
        private void showHistory(string associateID, string departmentID, string limitID)
        {
            DataTable dtHistory = new DataTable("History");
            dtHistory.Columns.Add("Person");
            dtHistory.Columns.Add("Limit");
            dtHistory.Columns.Add("To");
            dtHistory.Columns.Add("by");
            dtHistory.Columns.Add("Date", typeof(DateTime));
            dtHistory.Columns.Add("Ticket");

            foreach (DataRow drAL in dtAssociateLimits.Rows)
            {
                bool ok = false;

                if ((associateID != "") && (drAL["AssociateID"].ToString() == associateID))
                    ok = true;

                if ((departmentID != "") && (lookup(dtAssociates, "AssociateID", drAL["AssociateID"].ToString(), "DepartmentID") == departmentID) && (limitToDouble(drAL["Limit"].ToString()) != 0d))
                    ok = true;

                if ((limitID != "") && (drAL["LimitID"].ToString() == limitID))
                    ok = true;


                if (ok)
                {
                    string person = associateName(drAL["AssociateID"].ToString());
                    string limit = lookup(dtLimits, "LimitID", drAL["LimitID"].ToString(), "Description");
                    //string newlimit = formatLimit(Convert.ToDouble(drAL["Limit"].ToString()));
                    string newlimit = formatLimit(limitToDouble(drAL["Limit"].ToString()));
                    string by = drAL["StartUser"].ToString();
                    string date = DateTime.Parse(drAL["StartDate"].ToString()).ToShortDateString();
                    string ticket = drAL["TicketNumber"].ToString();

                    dtHistory.Rows.Add(person, limit, newlimit, by, date, ticket);
                }
            }


            // Sort associates by last name
            DataView dv = dtHistory.DefaultView;
            dv.Sort = "date desc, person";
            DataTable sortedHistory = dv.ToTable();

            dgChangeHistory.ItemsSource = sortedHistory.DefaultView;
        }
        #endregion

        #region Column Notes
        private void updateNoteForLimitID(string limitID)
        {
            string notes = "";

            // TEMPORARY !!!!!
            if (limitID == "5")
                notes = lookup(dtNotes, "NoteID", "2", "Note");
            if (limitID == "6")
                notes = lookup(dtNotes, "NoteID", "3", "Note");
            if (limitID == "7")
                notes = lookup(dtNotes, "NoteID", "4", "Note");
            if (limitID == "8")
                notes = lookup(dtNotes, "NoteID", "5", "Note");
            if (limitID == "10")
                notes = lookup(dtNotes, "NoteID", "6", "Note");
            if (limitID == "11")
                notes = lookup(dtNotes, "NoteID", "7", "Note");
            if (limitID == "12")
                notes = lookup(dtNotes, "NoteID", "8", "Note") + Environment.NewLine + Environment.NewLine + lookup(dtNotes, "NoteID", "9", "Note");
            if (limitID == "14")
                notes = lookup(dtNotes, "NoteID", "10", "Note");
            // END TEMPORARY


            // TEMPORARY
            //foreach (DataRow drNote in dtNotes.Rows)
            //{
            //    if (drNote["LimitID"].ToString() == limitID)
            //        notes = drNote["Note"].ToString();
            //}
            // END TEMPORARY

            //tbNote.
            tbNote.Text = notes;
        }
        #endregion

        #region Animation
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (oa.eStatus == optionAnimation.oaStatus.opening)
            {
                oa.progress++;
                gridLowerControls.Height = 203d * (oa.progress / 10d);
                dgAG.Margin = new Thickness(dgAG.Margin.Left, dgAG.Margin.Top, dgAG.Margin.Right, 45 + (oa.progress / 10d) * 208d);
                if (oa.progress >= 10d)
                    oa.eStatus = optionAnimation.oaStatus.open;
            }

            if (oa.eStatus == optionAnimation.oaStatus.closing)
            {
                oa.progress--;
                gridLowerControls.Height = 203d * (oa.progress / 10d);
                dgAG.Margin = new Thickness(dgAG.Margin.Left, dgAG.Margin.Top, dgAG.Margin.Right, 45 + (oa.progress / 10d) * 208d);
                if (oa.progress <= 0d)
                {
                    oa.eStatus = optionAnimation.oaStatus.closed;
                    gridLowerControls.Visibility = System.Windows.Visibility.Hidden;
                    return;
                }
            }

        }

        private void btnExpandCollapse_Click(object sender, RoutedEventArgs e)
        {
            if (oa.eStatus == optionAnimation.oaStatus.open)
            {
                oa.progress = 10;
                oa.eStatus = optionAnimation.oaStatus.closing;
                btnExpandCollapse.Content = "Show Info";
            }
            if (oa.eStatus == optionAnimation.oaStatus.closed)
            {
                oa.progress = 0;
                oa.eStatus = optionAnimation.oaStatus.opening;
                btnExpandCollapse.Content = "Hide Info";
                gridLowerControls.Visibility = System.Windows.Visibility.Visible;
            }
        }
        #endregion

        #region Save changes
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            saveData();
        }

        private void saveData()
        {
            // Make sure all the ticket numbers are entered
            foreach (DataRow drEdit in dtEdits.Rows)
            {
                if (drEdit["Ticket"].ToString() == "")
                {
                    MessageBox.Show("Each change requires a ticket number before the changes may be saved");
                    return;
                }
            }

            // Make sure all the ticket numbers are good
            foreach (DataRow drEdit in dtEdits.Rows)
            {
                if (drEdit["TicketTextColor"].ToString() == Colors.Red.ToString())
                {
                    MessageBox.Show("Ticket " + drEdit["Ticket"].ToString() + " is not valid. Ticket numbers must be valid before saving.");
                    return;
                }
            }

            // Create the table
            DataTable dtUpdaes = createSaveTable();

            // Fill the table
            fillSaveTable(dtUpdaes);

            // Run the script
            if (runSaveScript(dtUpdaes) == true)
            {
                // Clear the edits history and update the grid
                dtEdits.Rows.Clear();
                dgEdits.ItemsSource = dtEdits.DefaultView;
            }

            getAssociateLimits();
            parseData();
        }

        private DataTable createSaveTable()
        {
            // Create datatable to keep track of edits
            DataTable dtSave = new DataTable("AssociateLimitInput");
            dtSave.Columns.Add("ID");
            dtSave.Columns.Add("AssociateID");
            dtSave.Columns.Add("LimitID");
            dtSave.Columns.Add("Type");
            dtSave.Columns.Add("Limit");
            dtSave.Columns.Add("TicketNumber");
            dtSave.Columns.Add("Operation");
            //myInputTable.Rows.Add(17, 7, 2, "R", 11000, "2575848", "U");

            return dtSave;
        }

        private void fillSaveTable(DataTable myInputTable)
        {
            foreach (DataRow drEdit in dtEdits.Rows)
            {
                string to = drEdit["To"].ToString();
                string ticket = drEdit["Ticket"].ToString();
                string associateID = drEdit["AssociateID"].ToString();
                string limitID = drEdit["LimitID"].ToString();
                string type = drEdit["Type"].ToString();
                int id = getID(associateID, limitID, type);

                myInputTable.Rows.Add(id, Convert.ToInt32(associateID), Convert.ToInt32(limitID), type, limitToDouble(to), ticket, (id >= 0) ? "U" : "I");
            }
        }

        private int getID(string associateID, string limitID, string type)
        {
            foreach (DataRow drAL in dtAssociateLimits.Rows)
            {
                if ((drAL["AssociateID"].ToString() == associateID) &&
                    (drAL["Type"].ToString() == type) &&
                    (drAL["LimitID"].ToString() == limitID) &&
                    (drAL["EndDate"] is DBNull))
                    return Convert.ToInt32(drAL["ID"].ToString());
            }
            return -1;
        }

        private bool runSaveScript(DataTable myInputTable)
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, myInputTable, "AUTHORITYGUIDELINES_ASSOCIATELIMIT", "OPSCONSOLE");
            if (ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
            {
                MessageBox.Show("An error has occurred in the AUTHORITYGUIDELINES_ASSOCIATELIMIT script: " + ds.Tables[0].Rows[0][0].ToString());
                return false;
            }

            return true;
        }
        #endregion

        #region ServiceDesk
        private void showTicket(string ticketNumber)
        {
            SDTicket.RequestLongForm ticketInfo = SDTicket.getServiceTicketEntry(ticketNumber);
            if (ticketInfo != null)
            {
                tbNote.Inlines.Clear();
                tbNote.Inlines.Add(new Run("Category: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CATEGORY + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("By: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CREATEDBY + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Time: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CREATEDTIME + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Dept: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.DEPARTMENT + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Requester: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.REQUESTER + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("E-Mail: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.REQUESTEREMAIL + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Subject: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.SUBJECT + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Desc: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.SHORTDESCRIPTION.TrimStart().Replace("&nbsp;"," ") + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Technician: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.TECHNICIAN + Environment.NewLine) { Foreground = Brushes.MidnightBlue });
                MarkTicketAsGoodOrBad(ticketNumber, Colors.Black);
            }
            else
            {
                tbNote.Inlines.Clear();
                tbNote.Inlines.Add(new Run("Unable to find ticket") { FontWeight = FontWeights.Bold });
                MarkTicketAsGoodOrBad(ticketNumber, Colors.Red);
            }
        }
        #endregion

        #region History and Edits Selection

        private void MarkTicketAsGoodOrBad(string ticketNumber, Color color)
        {
            foreach (DataRow dtRow in dtEdits.Rows)
            {
                if (dtRow["Ticket"].ToString() == ticketNumber)
                {
                    dtRow["Ticket"] = ticketNumber;
                    dtRow["TicketTextColor"] = color;
                    dgEdits.ItemsSource = dtEdits.DefaultView;
                }
            }
        }

        private void dgEdits_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count < 1)
                return;

            if (!(e.AddedCells[0].Item is System.Data.DataRowView))
                return;

            string header = e.AddedCells[0].Column.Header.ToString();
            string ticket = ((System.Data.DataRowView)(e.AddedCells[0].Item)).Row.ItemArray[4].ToString();
            if (ticket == "")
                return;
            showTicket(ticket);
        }

        private void dgChangeHistory_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (e.AddedCells.Count < 1)
                return;

            if (!(e.AddedCells[0].Item is System.Data.DataRowView))
                return;

            string header = e.AddedCells[0].Column.Header.ToString();
            string ticket = ((System.Data.DataRowView)(e.AddedCells[0].Item)).Row.ItemArray[5].ToString();
            if (ticket == "")
                return;
            showTicket(ticket);
        }

        private void btnUndoEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgEdits.SelectedCells.Count <= 0)
            {
                MessageBox.Show("You must select an edit row to undo");
                return;
            }

            string associateID = ((System.Data.DataRowView)(dgEdits.SelectedCells[0].Item)).Row["AssociateID"].ToString();
            string from = ((System.Data.DataRowView)(dgEdits.SelectedCells[0].Item)).Row["From"].ToString();
            string to = ((System.Data.DataRowView)(dgEdits.SelectedCells[0].Item)).Row["To"].ToString();
            string type = ((System.Data.DataRowView)(dgEdits.SelectedCells[0].Item)).Row["Type"].ToString();
            string limitID = ((System.Data.DataRowView)(dgEdits.SelectedCells[0].Item)).Row["LimitID"].ToString();

            // Revert change
            foreach (DataRow dtRow in dtGrid.Rows)
            {
                if ((dtRow["AssociateID"].ToString() == associateID) &&
                    (dtRow["Type"].ToString() == type))
                {
                    dtRow["Limit" + limitID] = from;
                }
            }

            ((System.Data.DataRowView)(dgEdits.SelectedCells[0].Item)).Row.Delete();
            dgEdits.ItemsSource = dtEdits.DefaultView;
        }

        private void dgEdits_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
        }

        private void dgEdits_SourceUpdated(object sender, DataTransferEventArgs e)
        {
        }
        #endregion

        #region Second Row
        private void btnAddSecondRow_Click(object sender, RoutedEventArgs e)
        {
           if (!(dtEdits == null) && (dtEdits.Rows.Count > 0))
            {
                MessageBox.Show("You must save your changes first before the second reow can be added");
                return;
            }

            if (dgAG.SelectedCells.Count <= 0)
            {
                MessageBox.Show("You must select a person");
                return;
            }

            string person = ((System.Data.DataRowView)(dgAG.SelectedCells[0].Item)).Row[0].ToString();
            string associateID = ((System.Data.DataRowView)(dgAG.SelectedCells[0].Item)).Row["AssociateID"].ToString();

            // Create the table
            DataTable dtUpdaes = createSaveTable();
            dtUpdaes.Rows.Add(0, Convert.ToInt32(associateID), 1, "C", 0d, "ADDCROW", "I");

            // Run the script
            if (runSaveScript(dtUpdaes) == true)
            {
            }

            getAssociateLimits();
            parseData();
        }

        private void btnRemoveSecondRow_Click(object sender, RoutedEventArgs e)
        {
            if (dgAG.SelectedCells.Count <= 0)
            {
                MessageBox.Show("You must select an associate for second row removal by selecting any cell on the grid");
                return;
            }

            string associateID = ((System.Data.DataRowView)(dgAG.SelectedCells[0].Item)).Row["AssociateID"].ToString();

            screenAssociates.Load(dtDepartments, dtGroups, dtAssociates, dtClaimsCenterLimits, dtDataHavenLimits);
            screenAssociates.RemoveSecond(associateID);
            screenAssociates.Visibility = System.Windows.Visibility.Visible;
        }

        public void removeSecondRow(string associateID, string ticketNumber)
        {
            if (!(dtEdits == null) && (dtEdits.Rows.Count > 0))
            {
                MessageBox.Show("You must save your changes first before the second row can be removed");
                return;
            }

            if (dgAG.SelectedCells.Count <= 0)
            {
                MessageBox.Show("You must select a person");
                return;
            }

            string person = ((System.Data.DataRowView)(dgAG.SelectedCells[0].Item)).Row[0].ToString();

            // Create the table
            DataTable dtUpdaes = createSaveTable();

            foreach (DataRow dtSMD in dtAssociateLimits.Rows)
            {
                if ((dtSMD["AssociateID"].ToString() == associateID) && (dtSMD["Type"].ToString() == "C"))
                    dtUpdaes.Rows.Add(dtSMD["ID"].ToString(), Convert.ToInt32(associateID), dtSMD["LimitID"].ToString(), "C", 0d, ticketNumber, "D");
            }

            // Run the script
            if (runSaveScript(dtUpdaes) == true)
            {
            }

            getAssociateLimits();
            parseData();
        }
        #endregion

        #region Reports
        private void fillReportsCombo()
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "CRUD_CONFIGURATION", "OPSCONSOLE");
            if (ds.Tables[0].TableName.ToUpper() == "ERRORTABLE")
            {
                MessageBox.Show("An error has occurred in the CRUD_CONFIGURATION script: " + ds.Tables[0].Rows[0][0].ToString());
                return;
            }
            cbReports.ItemsSource = ds.Tables["Report"].DefaultView;
        }

        private void cbReports_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbReports.SelectedIndex <= 0)
                return;
            System.Diagnostics.Process.Start(cbReports.SelectedValue.ToString());
            cbReports.SelectedIndex = -1;
            //System.Diagnostics.Process.Start(@"http://mantestbs01/Reports/Pages/Report.aspx?ItemPath=%2fReportViewerRoot%2fReports+I+Can+Run%2fIT+Production+Support%2fAuthority+Guidelines%2fAuthority+Limits+Comparison");
        }
        #endregion

        #region Close screen
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!(dtEdits == null) && (dtEdits.Rows.Count > 0))
            {
                if ((MessageBox.Show("You have unsaved edits. Closing this screen will lose these changes. Are you sure you want to proceed?", "Confirm exit with unsaved changes", MessageBoxButton.YesNo) != MessageBoxResult.Yes))
                    return;
            }
            MainWindow.ourMainWindow.showMainScreen();
        }
        #endregion

        #region Color Scheme
        private void btnColor2_Click(object sender, RoutedEventArgs e)
        {
            colorErrorRed = colorErrorRedChoice2;
            rectColor1.Opacity = .3d;
            rectColor2.Opacity = 1d;
            recolor(colorErrorRedChoice1);
        }

        private void btnColor1_Click(object sender, RoutedEventArgs e)
        {
            colorErrorRed = colorErrorRedChoice1;
            rectColor1.Opacity = 1d;
            rectColor2.Opacity = .3d;
            recolor(colorErrorRedChoice2);
        }

        private void recolor(string from)
        {
            foreach (DataRow dtRow in dtGrid.Rows)
            {
                if (dtRow["CC9COLOR"].ToString() == from)
                    dtRow["CC9COLOR"] = colorErrorRed;
                if (dtRow["CC6COLOR"].ToString() == from)
                    dtRow["CC6COLOR"] = colorErrorRed;
                if (dtRow["DHCOLOR"].ToString() == from)
                    dtRow["DHCOLOR"] = colorErrorRed;
            }
        }
        #endregion

        #region Edit Associates
        private void btnAddAssociate_Click(object sender, RoutedEventArgs e)
        {
            if (!(dtEdits == null) && (dtEdits.Rows.Count > 0))
            {
                if ((MessageBox.Show("Adding an associate requires that edits be saved first. You may continue if you like, but unsaved edits will be lost. Are you sure you want to proceed?", "Confirm lose edits", MessageBoxButton.YesNo) != MessageBoxResult.Yes))
                {
                    cbGroup.SelectedIndex = currentGroupSelection;
                    return;
                }
                else
                {
                    dtEdits.Rows.Clear();
                    dgEdits.ItemsSource = dtEdits.DefaultView;
                }
            }


            screenAssociates.Load(dtDepartments, dtGroups, dtAssociates, dtClaimsCenterLimits, dtDataHavenLimits);
            screenAssociates.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnRemoveAssociate_Click(object sender, RoutedEventArgs e)
        {
            if (dgAG.SelectedCells.Count <= 0)
            {
                MessageBox.Show("You must select an associate to edit by selecting any cell on the grid");
                return;
            }

            if (!(dtEdits == null) && (dtEdits.Rows.Count > 0))
            {
                if ((MessageBox.Show("Removing an associate requires that edits be saved first. You may continue if you like, but unsaved edits will be lost. Are you sure you want to proceed?", "Confirm lose edits", MessageBoxButton.YesNo) != MessageBoxResult.Yes))
                {
                    cbGroup.SelectedIndex = currentGroupSelection;
                    return;
                }
                else
                {
                    dtEdits.Rows.Clear();
                    dgEdits.ItemsSource = dtEdits.DefaultView;
                }
            }

            string associateID = ((System.Data.DataRowView)(dgAG.SelectedCells[0].Item)).Row["AssociateID"].ToString();

            screenAssociates.Load(dtDepartments, dtGroups, dtAssociates, dtClaimsCenterLimits, dtDataHavenLimits);
            screenAssociates.Remove(associateID);
            screenAssociates.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnEditAssociate_Click(object sender, RoutedEventArgs e)
        {
            if (dgAG.SelectedCells.Count <= 0)
            {
                MessageBox.Show("You must select an associate to edit by selecting any cell on the grid");
                return;
            }

            if (!(dtEdits == null) && (dtEdits.Rows.Count > 0))
            {
                if ((MessageBox.Show("Editing an associate requires that edits be saved first. You may continue if you like, but unsaved edits will be lost. Are you sure you want to proceed?", "Confirm lose edits", MessageBoxButton.YesNo) != MessageBoxResult.Yes))
                {
                    cbGroup.SelectedIndex = currentGroupSelection;
                    return;
                }
                else
                {
                    dtEdits.Rows.Clear();
                    dgEdits.ItemsSource = dtEdits.DefaultView;
                }
            }

            string associateID = ((System.Data.DataRowView)(dgAG.SelectedCells[0].Item)).Row["AssociateID"].ToString();

            screenAssociates.Load(dtDepartments, dtGroups, dtAssociates, dtClaimsCenterLimits, dtDataHavenLimits);
            screenAssociates.Edit(associateID);
            screenAssociates.Visibility = System.Windows.Visibility.Visible;
        }
        #endregion

        #region E-mail notification
        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you have made all changes and want to send notification e-mail to: AG Update Notification?", "Confirm e-mail", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;

            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "SEND_NOTIFY_AG_EMAIL", "OPSCONSOLE");
        }
        #endregion

    }
}
