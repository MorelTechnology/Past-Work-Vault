using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
    /// Interaction logic for ResourcePlanning.xaml
    /// </summary>
    public partial class ResourcePlanning : UserControl
    {
        #region One Time Import - Temporary
        // FOR ONE TIME IMPORT ONLY
        public static string connectionString = "Data Source=DEVSQL2;Initial Catalog=ResourcePlanning;Integrated Security=True";
        #endregion

        #region Class Data
        ////// Data Tables //////
        public DataTable dtWorkstream = new DataTable();
        public DataTable dtProject = new DataTable();
        DataTable dtProjectResource = new DataTable();
        public DataTable dtProjectResourceInfo = new DataTable();
        public DataTable dtRole = new DataTable();
        public DataTable dtResources = new DataTable();
        public DataTable dtDepartment = new DataTable();
        DataTable dtProjectResourceUpdate = new DataTable("ProjectResource");
        public DataTable dtEmployees = new DataTable();

        Dictionary<string, DataGridTemplateColumn> columns = new Dictionary<string, DataGridTemplateColumn>();
        DateTime dtFirstMonth = DateTime.Today;
        DateTime dtLastMonth = DateTime.Today;
        bool ignoreResourceChanges = false;

        #region Background workers
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        bool Loaded = false;
        private readonly BackgroundWorker worker = new BackgroundWorker();
        private readonly BackgroundWorker worker2 = new BackgroundWorker();
        #endregion

        #endregion

        #region Constructor
        public ResourcePlanning()
        {
            InitializeComponent();
        }
        #endregion

        #region Load Data
        public void Load(string perm)
        {
            ////// CREATE A DICTIONARY OF NAMES TO GRID COLUMNS //////
            setupColumnDictionary();

            ////// DEFAULT FROM AND TO DROPDOWNS //////
            fillComboDates(cbFrom, DateTime.Today.AddMonths(-3), DateTime.Today.AddMonths(8));
            fillComboDates(cbTo, DateTime.Today.AddMonths(1), DateTime.Today.AddMonths(5));
            setDefaultDateRange();
            setupGridColumns();

            ////// START IN PROJECT VIEW //////
            setProjectsOrPeople(projects: true);

            ////// LOAD DATA //////
            LoadData();

            ////// FILL LIST OF PROJECTS //////
            fillProjects(0, "");
            dgResourceGrid.ItemsSource = null;

            ////// SETUP TIMER FOR PLEASE WAIT ANIMATION //////
            if (!Loaded)
            {
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);

                worker.DoWork += worker_DoWork;
                worker.RunWorkerCompleted += worker_RunWorkerCompleted;

                worker2.DoWork += worker_DoWork2;
                worker2.RunWorkerCompleted += worker_RunWorkerCompleted2;
                worker2.RunWorkerAsync();

                Loaded = true;
            }

            ////// MAKE SURE PLEASE WAIT IS OFF //////
            pleaseWait(wait: false);

            ////// INITIAL STATE //////
            rpProject.Visibility = System.Windows.Visibility.Collapsed;
            rpProjectResource.Visibility = System.Windows.Visibility.Collapsed;
            rpResource.Visibility = System.Windows.Visibility.Collapsed;
            gridDisable1.Visibility = System.Windows.Visibility.Collapsed;
            gridAddUser.Visibility = System.Windows.Visibility.Collapsed;

            ////// FILL PROJECT MANAGER DROPDOWN //////
            fillPMDropdown();

            ////// FILL WORKSTREAM DROPDOWN //////
            fillWorkstreamDropdown();
        }


        private void LoadData()
        {
            dtWorkstream = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETWORKSTREAMS", "OPSCONSOLE").Tables["WS"];
            //dtProject = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_CRUD_PROJECTS", "OPSCONSOLE").Tables["WS"];
            //dtResources = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETRESOURCE", "OPSCONSOLE").Tables["WS"];
            LoadProjects();
            LoadPeople();

            
            dtProjectResource = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETPROJECTRESOURCE", "OPSCONSOLE").Tables["WS"];
            dtRole = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETROLES", "OPSCONSOLE").Tables["WS"];
            dtDepartment = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETDEPARTMENTS", "OPSCONSOLE").Tables["WS"];
            dtProjectResourceInfo = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETPROJECTRESOURCEINFO", "OPSCONSOLE").Tables["WS"];

            DataRow rowAll = dtWorkstream.NewRow();
            rowAll["WorkstreamID"] = 0;
            rowAll["WorkstreamName"] = "All";
            dtWorkstream.Rows.InsertAt(rowAll, 0);
            dgWorkStream.ItemsSource = dtWorkstream.DefaultView;

            cbWorkstreamAddFrom.ItemsSource = dtWorkstream.DefaultView;
        }

        private void LoadProjects()
        {
            dtProject = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_CRUD_PROJECTS", "OPSCONSOLE").Tables["WS"];
        }

        public void LoadPeople()
        {
            dtResources = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETRESOURCE", "OPSCONSOLE").Tables["WS"];
            fillPeople("");
        }

        private void setupColumnDictionary()
        {
            columns = new Dictionary<string, DataGridTemplateColumn>();

            columns.Add("C1", C1);
            columns.Add("C2", C2);
            columns.Add("C3", C3);
            columns.Add("C4", C4);
            columns.Add("C5", C5);
            columns.Add("C6", C6);
            columns.Add("C7", C7);
            columns.Add("C8", C8);
            columns.Add("C9", C9);
            columns.Add("C10", C10);
            columns.Add("C11", C11);
            columns.Add("C12", C12);
            columns.Add("C13", C13);
            columns.Add("C14", C14);
            columns.Add("C15", C15);
            columns.Add("C16", C16);
            columns.Add("C17", C17);
            columns.Add("C18", C18);
            columns.Add("C19", C19);
            columns.Add("C20", C20);
            columns.Add("C21", C21);
            columns.Add("C22", C22);
            columns.Add("C23", C23);
            columns.Add("C24", C24);
            columns.Add("C25", C25);
            columns.Add("C26", C26);
        }
        #endregion

        #region Commands
        private void btnProjects_Click(object sender, RoutedEventArgs e)
        {
            setProjectsOrPeople(projects: true);
        }

        private void btnPeople_Click(object sender, RoutedEventArgs e)
        {
            setProjectsOrPeople(projects: false);
        }

        private void btnTotalAllocations_Click(object sender, RoutedEventArgs e)
        {
            if (rectTotalAllocations.Opacity < 0.5d)
                rectTotalAllocations.Opacity = 1d;
            else
                rectTotalAllocations.Opacity = 0.2d;
            fillProject();
        }

        private void btnAddResourcesToProject_Click(object sender, RoutedEventArgs e)
        {
            ////// FAST FAIL IF NO PROJECT IS SELECTED //////
            if (dgProject.SelectedIndex == -1)
            {
                MessageBox.Show("You must select a project first, then after that you may add resources to it.");
                return;
            }

            gridAddUser.Visibility = System.Windows.Visibility.Visible;
            gridProjectControls.Visibility = System.Windows.Visibility.Collapsed;
            cbManagerAddFrom.SelectedIndex = -1;
            cbWorkstreamAddFrom.SelectedIndex = -1;

            ////// FILL MANAGERS COMBO //////
            DataRow[] filtered = dtResources.Select("IsManager = 1");
            cbManagerAddFrom.ItemsSource = (filtered.Length == 0) ? null : filtered.CopyToDataTable().DefaultView;

            ////// DISABLE OTHER OPTIONS //////
            gridDisable1.Visibility = System.Windows.Visibility.Visible;
            //gridViewBy.IsEnabled = false;
            //gridProjects.IsEnabled = false;
            //gridRange.IsEnabled = false;
            //gridAllocate.IsEnabled = false;
            //dgResourceGrid.IsEnabled = false;

            fillProject();
        }

        private void btnCloseAddResources_Click(object sender, RoutedEventArgs e)
        {
            gridAddUser.Visibility = System.Windows.Visibility.Collapsed;
            gridProjectControls.Visibility = System.Windows.Visibility.Visible;
            cbManagerAddFrom.SelectedIndex = -1;
            cbWorkstreamAddFrom.SelectedIndex = -1;
            fillProject();

            ////// ENABLE OTHER OPTIONS //////
            gridDisable1.Visibility = System.Windows.Visibility.Collapsed;
            //gridViewBy.IsEnabled = true;
            //gridProjects.IsEnabled = true;
            //gridRange.IsEnabled = true;
            //gridAllocate.IsEnabled = true;
            //dgResourceGrid.IsEnabled = true;
        }

        private void btnAddSelectedResourcesToProject_Click(object sender, RoutedEventArgs e)
        {
            int projid = Convert.ToInt32(((System.Data.DataRowView)dgProject.SelectedItem)["ProjectID"].ToString());
            string name = "";

            if (dgResourceGrid.SelectedCells.Count > 1)
            {
                MessageBox.Show("Please select just one person");
                return;
            }

            if (dtFirstMonth.DayOfWeek != DayOfWeek.Monday)
            {
                MessageBox.Show("btnAddSelectedResourcesToProject_Click: First day of week not Monday");
                return;
            }


            foreach (var x in dgResourceGrid.SelectedCells)
            {
                var cell = ((System.Data.DataRowView)(x.Item));
                string resid = cell["ResourceID"].ToString();

                name = getkey(dtResources, "ResourceName", "ResourceID", resid.ToString());

                DataRow drNewPR = dtProjectResource.NewRow();
                drNewPR["ProjectID"] = projid.ToString();
                drNewPR["ResourceID"] = resid.ToString();
                drNewPR["ProjectPercentage"] = "0";

                //drNewPR["WeekStartDate"] = DateTime.Today;
                drNewPR["WeekStartDate"] = dtFirstMonth;
                dtProjectResource.Rows.Add(drNewPR);
                //                string person = ((System.Data.DataRowView)(x.Item)).Row[0].ToString();
            }

            ////// ENSURE THEY HAVE SELECTED A VALID RESOURCE FOR ADDING //////
            if (name == "")
            {
                MessageBox.Show("You must select a Resource to add to this project from below the green line");
                return;
            }

            gridAddUser.Visibility = System.Windows.Visibility.Collapsed;
            gridProjectControls.Visibility = System.Windows.Visibility.Visible;
            cbManagerAddFrom.SelectedIndex = -1;
            cbWorkstreamAddFrom.SelectedIndex = -1;
            fillProject();

            MessageBox.Show("You must now add percentages to " + name);

            ////// ENABLE OTHER OPTIONS //////
            gridDisable1.Visibility = System.Windows.Visibility.Collapsed;
            //gridViewBy.IsEnabled = true;
            //gridProjects.IsEnabled = true;
            //gridRange.IsEnabled = true;
            //gridAllocate.IsEnabled = true;
            //dgResourceGrid.IsEnabled = true;
        }

        private void btnPerctClick(object sender, RoutedEventArgs e)
        {
            ////// DETERMINE WHICH BUTTON THEY CLICKED //////
            string percent = ((Button)e.Source).Content.ToString().Replace("%", "");

            ////// CREATE AN UPDATE LIST //////
            //DataTable dtProjectResourceUpdate = dtProjectResource.Clone();
            //dtProjectResourceUpdate.Columns.Add("Operation");
            //dtProjectResourceUpdate.TableName = "ProjectResource";

            dtProjectResourceUpdate = new DataTable("ProjectResource");
            dtProjectResourceUpdate.Columns.Add("ProjectResourceID");
            dtProjectResourceUpdate.Columns.Add("ProjectID");
            dtProjectResourceUpdate.Columns.Add("ResourceID");
            dtProjectResourceUpdate.Columns.Add("WeekStartDate");
            dtProjectResourceUpdate.Columns.Add("ProjectPercentage");
            dtProjectResourceUpdate.Columns.Add("Operation");

            foreach (var x in dgResourceGrid.SelectedCells)
            {
                var cell = ((System.Data.DataRowView)(x.Item));
                string resid = cell["ResourceID"].ToString();
                string projid = cell["ProjectID"].ToString();
                //string header = x.Column.Header.ToString();
                int column = x.Column.DisplayIndex;

                if ((column <= 3) || (projid == "") || (resid == ""))
                {
                    pleaseWait(wait: false);
                    MessageBox.Show("You have selected cells outside the changeable area");
                    return;
                }

                DateTime d = DateForColumn(column - 6);

                if (d.DayOfWeek != DayOfWeek.Monday)
                {
                    MessageBox.Show("btnPerctClick: First day of week not Monday");
                    return;
                }

                DataRow drDelPR = dtProjectResourceUpdate.NewRow();
                drDelPR["ProjectResourceID"] = "0";
                drDelPR["ProjectID"] = projid.ToString();
                drDelPR["ResourceID"] = resid.ToString();
                drDelPR["WeekStartDate"] = d;
                drDelPR["ProjectPercentage"] = "0";
                drDelPR["Operation"] = "X";
                dtProjectResourceUpdate.Rows.Add(drDelPR);

                DataRow drNewPR = dtProjectResourceUpdate.NewRow();
                drNewPR["ProjectResourceID"] = "0";
                drNewPR["ProjectID"] = projid.ToString();
                drNewPR["ResourceID"] = resid.ToString();
                drNewPR["ProjectPercentage"] = percent;
                drNewPR["WeekStartDate"] = d;
                drNewPR["Operation"] = "I";
                dtProjectResourceUpdate.Rows.Add(drNewPR);
            }

            if (dtProjectResourceUpdate.Rows.Count == 0)
                return;

            pleaseWait(wait: true);

            if (worker.IsBusy != true)
                worker.RunWorkerAsync(percent);

        }

        private void btnNewProject_Click(object sender, RoutedEventArgs e)
        {
            rpProject.Visibility = System.Windows.Visibility.Visible;
            rpProject.Load(this);
            rpProject.Add();
        }

        private void btnEditProject_Click(object sender, RoutedEventArgs e)
        {
            if (dgProject.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a project to edit");
                return;
            }

            rpProject.Visibility = System.Windows.Visibility.Visible;
            rpProject.Load(this);

            int projidint = Convert.ToInt32(((System.Data.DataRowView)dgProject.SelectedItem)["ProjectID"].ToString());
            string projid = projidint.ToString();
            string resid = "";

            string pmanager = getkey(dtProject, "ProjectManager", "ProjectID", projid);
            string wsid = getkey(dtProject, "WorkstreamID", "ProjectID", projid);
            string pmid = getkey(dtResources, "ResourceID", "ResourceName", pmanager);
            string projname = getkey(dtProject, "ProjectName", "ProjectID", projid);
            string projdesc = getkey(dtProject, "ProjectDescription", "ProjectID", projid);


            rpProject.Edit(projid, pmid, wsid, projname, projdesc);
        }

        private void btnAddNewResource_Click(object sender, RoutedEventArgs e)
        {
            rpResource.Visibility = System.Windows.Visibility.Visible;
            rpResource.Add();
            rpResource.Load(this);
        }

        private void btnEditExistingResource_Click(object sender, RoutedEventArgs e)
        {
            ////// ONE AND ONLY ONE RESOURCE SELECTED //////
            if (dgResources.SelectedItems.Count != 1)
            {
                MessageBox.Show("You must select one (and only one) resource from the grid at the top of the screen, then press Edit");
                return;
            }

            foreach (var x in dgResources.SelectedItems)
            {
                if (x is System.Data.DataRowView)
                {
                    string name = ((System.Data.DataRowView)x)["ResourceName"].ToString();
                    string resid = ((System.Data.DataRowView)x)["ResourceID"].ToString();
                    rpResource.Visibility = System.Windows.Visibility.Visible;
                    rpResource.Load(this);
                    rpResource.Edit(resid, name);
                    return;
                }
            }
        }

        private void btnEditResourceProject_Click(object sender, RoutedEventArgs e)
        {
            string resid = "";
            string projid = "";
            string resname = "";
            string roleid = "";
            string comment = "";

            if (dgResourceGrid.SelectedCells.Count != 1)
            {
                MessageBox.Show("You must click on one name in the project grid, and only one name");
                return;
            }

            var cell = ((System.Data.DataRowView)(dgResourceGrid.SelectedCells[0].Item));
            resid = cell["ResourceID"].ToString();
            projid = cell["ProjectID"].ToString();
            resname = cell["Person"].ToString();
            string rolename = cell["RoleName"].ToString();
            comment = cell["Comments"].ToString();

            roleid = getkey(dtRole, "RoleID", "RoleName", rolename);

            rpProjectResource.Visibility = System.Windows.Visibility.Visible;
            rpProjectResource.Load(this, resid, resname, projid, roleid, comment);
        }

        private void btnRemoveResourcesFromProject_Copy_Click(object sender, RoutedEventArgs e)
        {
            if (dgProject.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a project to remove");
                return;
            }

            int projidint = Convert.ToInt32(((System.Data.DataRowView)dgProject.SelectedItem)["ProjectID"].ToString());
            string projid = projidint.ToString();
            string resid = "";

            string pmanager = getkey(dtProject, "ProjectManager", "ProjectID", projid);
            string wsid = getkey(dtProject, "WorkstreamID", "ProjectID", projid);
            string pmid = getkey(dtResources, "ResourceID", "ResourceName", pmanager);
            string projname = getkey(dtProject, "ProjectName", "ProjectID", projid);
            string projdesc = getkey(dtProject, "ProjectDescription", "ProjectID", projid);

            if (MessageBox.Show("Removing a project from RAPP is done via Service Desk Ticket. Would you like to generate an SD ticket to remove " + projname + "?", "Confirm Service Desk Ticket creation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                string desc = "Please remove RAPP project " + projname;
                string result = SDTicket.createTicket(MainWindow.ourMainWindow.lblCurrentUser.Text, "", desc, desc);

                if (result.StartsWith("ERROR"))
                    MessageBox.Show("An error occurred trying to create the ticket. The ticket was not created.");
                else
                    MessageBox.Show("Ticket number " + SDTicket.extractTicketNumberFromResults(result) + " has been created");
            }
        }


        private void btnRemoveResource_Click(object sender, RoutedEventArgs e)
        {
            ////// ONE AND ONLY ONE RESOURCE SELECTED //////
            if (dgResources.SelectedItems.Count != 1)
            {
                MessageBox.Show("You must select one (and only one) resource from the grid at the top of the screen, then press Remove");
                return;
            }

            foreach (var x in dgResources.SelectedItems)
            {
                if (x is System.Data.DataRowView)
                {
                    string name = ((System.Data.DataRowView)x)["ResourceName"].ToString();
                    string resid = ((System.Data.DataRowView)x)["ResourceID"].ToString();
                    if (MessageBox.Show("Removing a resource from RAPP is done via Service Desk Ticket. Would you like to generate an SD ticket to remove " + name + "?", "Confirm Service Desk Ticket creation", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        string desc = "Please remove RAPP user " + name;
                        string result = SDTicket.createTicket(MainWindow.ourMainWindow.lblCurrentUser.Text, "", desc, desc);

                        if (result.StartsWith("ERROR"))
                            MessageBox.Show("An error occurred trying to create the ticket. The ticket was not created.");
                        else
                            MessageBox.Show("Ticket number " + SDTicket.extractTicketNumberFromResults(result) + " has been created");
                    }
                    return;
                }
            }

            MessageBox.Show("To remove a resource please submit a service desk ticket. Resources should be automatically removed on termination.");

        }



        private void btnExit2_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ourMainWindow.showMainScreen();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ourMainWindow.showMainScreen();
        }

        private void btnShowRoleComments_Click(object sender, RoutedEventArgs e)
        {
            if (rectRoleComments.Opacity < 0.5d)
                rectRoleComments.Opacity = 1d;
            else
                rectRoleComments.Opacity = 0.2d;
            showhideRoleComments();
        }

        private void btnRemoveResourcesFromProject_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("To remove a resource from a project just set the allocation to 0% across all dates where the resource will no longer be used");
        }

        private void btnUndersubscribed_Click(object sender, RoutedEventArgs e)
        {
            cbFilterByManager.SelectedIndex = -1;
            cbFilterByWorkstream.SelectedIndex = -1;
            fillOverOrUnderSubscribed(bOver: false);
        }

        private void btnOversubscribed_Click(object sender, RoutedEventArgs e)
        {
            cbFilterByManager.SelectedIndex = -1;
            cbFilterByWorkstream.SelectedIndex = -1;
            fillOverOrUnderSubscribed(bOver: true);
        }

        private void btnRefresh1_Click(object sender, RoutedEventArgs e)
        {
            refreshScreen();
        }
        #endregion

        #region Selection and Change Events
        private void dgChangeHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
        
        private void dgWorkStream_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            reloadProjects();
        }

        private void cbFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFrom.SelectedIndex < 0)
            {
                if (dtFirstMonth.DayOfWeek != DayOfWeek.Monday)
                {
                    MessageBox.Show("Error cbFrom_SelectionChanged 1: The first day of the week must be Monday");
                }
                return;
            }

            string firstMonth = ((ComboBoxItem)cbFrom.SelectedValue).Content.ToString();
            dtFirstMonth = DateTime.Parse(firstMonth.Substring(0, 3) + " 1 " + firstMonth.Substring(4));
            int months = dtFirstMonth.Month - DateTime.Today.Month;

            setupGridColumns();
            refreshScreen();

            fillComboDates(cbTo, dtFirstMonth.AddMonths(1), dtFirstMonth.AddMonths(6));
            dgResourceGrid.ItemsSource = null;
        }

        private void cbTo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            setupGridColumns();
            refreshScreen();
        }

        private void dgResources_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!ignoreResourceChanges)
            {
                if (cbFilterByManager.SelectedIndex >= 0)
                    cbFilterByManager.SelectedIndex = -1;
                if (cbFilterByWorkstream.SelectedIndex >= 0)
                    cbFilterByWorkstream.SelectedIndex = -1;

                fillGridForPeople();
            }
        }

        private void dgProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillProject();
            return;
        }

        private void cbWorkstreamAddFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbWorkstreamAddFrom.SelectedIndex == -1)
                return;

            cbManagerAddFrom.SelectedIndex = -1;

            if (cbWorkstreamAddFrom.SelectedIndex >= 0)
                fillProject();
        }

        private void cbManagerAddFrom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbManagerAddFrom.SelectedIndex == -1)
                return;

            cbWorkstreamAddFrom.SelectedIndex = -1;

            if (cbManagerAddFrom.SelectedIndex >= 0)
                fillProject();
        }

        private void cbFilterByManager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilterByManager.SelectedIndex < 0)
                return;

            if (cbFilterByManager.SelectedIndex == 0)
            {
                dgResources.SelectedIndex = -1;
                dgResourceGrid.ItemsSource = null;
                return;
            }

            cbFilterByWorkstream.SelectedIndex = -1;

            ////// Get the name of the Project Manager //////
            string pmid = ((System.Data.DataRowView)cbFilterByManager.SelectedItem).Row["ResourceID"].ToString();
            string pmanager = getkey(dtResources, "ResourceName", "ResourceID", pmid);

            ////// Keep a list of found resources //////
            List<string> resources = new List<string>();
            List<string> resourcesconsidered = new List<string>();

            ////// Run through project resources //////
            foreach (DataRow drProjRes in dtProjectResource.Rows)
            {
                string resid = drProjRes["ResourceID"].ToString();

                // Future
                // Jan 16 2017 - Do not include Archived Resources
                // if (isResourceArchived(Convert.ToInt32(resid)) && !CommonUI.getCheckboxButtonStatus(btnShowArchived))
                //    continue;

                //if (!resourcesconsidered.Contains(resid))
                {
                    resourcesconsidered.Add(resid);
                    string projid = drProjRes["ProjectID"].ToString();
                    string pmforproject = getkey(dtProject, "ProjectManager", "ProjectID", projid);

                    if (pmanager == pmforproject)
                    {
                        if (!resources.Contains(resid))
                            resources.Add(resid);
                    }
                }
            }

            ignoreResourceChanges = true;
            dgResources.SelectedIndex = -1;
            foreach (var x in dgResources.ItemsSource)
            {
                if (x is System.Data.DataRowView)
                {
                    //string name = ((System.Data.DataRowView)x)["ResourceName"].ToString();
                    string resid = ((System.Data.DataRowView)x)["ResourceID"].ToString();
                    if (resources.Contains(resid))
                        dgResources.SelectedItems.Add(x);
                }
            }
            fillGridForPeople();
            ignoreResourceChanges = false;
        }

        private void cbFilterByProject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cbFilterByWorkstream_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbFilterByWorkstream.SelectedIndex < 0)
                return;

            if (cbFilterByWorkstream.SelectedIndex == 0)
            {
                dgResources.SelectedIndex = -1;
                dgResourceGrid.ItemsSource = null;
                return;
            }

            cbFilterByManager.SelectedIndex = -1;

            ////// Get the name of the Workstream //////
            string wsid = ((System.Data.DataRowView)cbFilterByWorkstream.SelectedItem).Row["WorkstreamID"].ToString();

            ////// Keep a list of found resources //////
            List<string> resources = new List<string>();
            List<string> resourcesconsidered = new List<string>();

            ////// Run through project resources //////
            foreach (DataRow drProjRes in dtProjectResource.Rows)
            {
                string resid = drProjRes["ResourceID"].ToString();

                //if (!resourcesconsidered.Contains(resid))
                {
                    resourcesconsidered.Add(resid);
                    string projid = drProjRes["ProjectID"].ToString();
                    string wsidforproject = getkey(dtProject, "WorkstreamID", "ProjectID", projid);

                    // Future
                    // Jan 16 2017 - Do not include Archived Resources
                    // if (isResourceArchived(Convert.ToInt32(resid)) && !CommonUI.getCheckboxButtonStatus(btnShowArchived))
                     //    continue;

                    if (wsid == wsidforproject)
                    {
                        if (!resources.Contains(resid))
                            resources.Add(resid);
                    }
                }
            }

            ignoreResourceChanges = true;
            dgResources.SelectedIndex = -1;
            foreach (var x in dgResources.ItemsSource)
            {
                if (x is System.Data.DataRowView)
                {
                    //string name = ((System.Data.DataRowView)x)["ResourceName"].ToString();
                    string resid = ((System.Data.DataRowView)x)["ResourceID"].ToString();
                    if (resources.Contains(resid))
                        dgResources.SelectedItems.Add(x);
                }
            }
            fillGridForPeople();
            ignoreResourceChanges = false;

        }

        #endregion

        #region Text Changed Events
        private void ebPeopleFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            fillPeople(ebPeopleFilter.Text);
        }
        #endregion

        #region Populate Controls with Data

        private void fillPMDropdown()
        {
            ////// POPULATE RESOURCES //////
            DataRow[] filtered = dtResources.Select((CommonUI.getCheckboxButtonStatus(btnShowArchived)) ? "IsPM = 1" : "IsPM = 1 and Archived=False");
            if (filtered.Length == 0)
                return;
            DataTable f = filtered.CopyToDataTable();
            DataRow dr = f.NewRow();
            f.Rows.InsertAt(dr, 0);
            cbFilterByManager.ItemsSource = f.DefaultView;
        }

        private void fillWorkstreamDropdown()
        {
            ////// POPULATE WORKSTREAMS //////
            DataRow[] filteredws = dtWorkstream.Select("WorkstreamName <> 'All'","WorkstreamName ASC");
            if (filteredws.Length == 0)
                return;
            DataTable f = filteredws.CopyToDataTable();
            DataRow dr = f.NewRow();
            f.Rows.InsertAt(dr, 0);
            
            cbFilterByWorkstream.ItemsSource = f.DefaultView;
            
        }


        #endregion

        #region Setup Grid
        private void setupGridColumns()
        {
            if ((cbFrom.SelectedValue == null) || (cbTo.SelectedValue == null))
            {
                dgResourceGrid.ItemsSource = null;
                return;
            }

            string firstMonth = ((ComboBoxItem)cbFrom.SelectedValue).Content.ToString();
            string lastMonth = ((ComboBoxItem)cbTo.SelectedValue).Content.ToString();

            dtFirstMonth = previousMonday(DateTime.Parse(firstMonth.Substring(0, 3) + " 1 " + firstMonth.Substring(4)));
            dtLastMonth = previousMonday(DateTime.Parse(lastMonth.Substring(0, 3) + " 1 " + lastMonth.Substring(4)));

            if (dtFirstMonth.DayOfWeek != DayOfWeek.Monday)
            {
                MessageBox.Show("Error setupGridColumns: The first day of the week must be Monday");
            }


            if (dtLastMonth <= dtFirstMonth)
                return;

            int weeks = (int)(((dtLastMonth - dtFirstMonth).TotalDays) / 7d) + 1;
            if (weeks > 26)
                weeks = 26;

            for (int i = 0; i < weeks; i++)
            {
                string col = "C" + (i + 1).ToString();
                columns[col].Header = dtFirstMonth.AddDays(i * 7).ToString("MMM d") + "\n" + dtFirstMonth.AddDays(i*7+4).ToString("MMM d");
                columns[col].Visibility = System.Windows.Visibility.Visible;

            }
            for (int i = weeks; i < 26; i++)
            {
                string col = "C" + (i + 1).ToString();
                columns[col].Visibility = System.Windows.Visibility.Collapsed;
            }
        }
        #endregion

        #region UI / Business Logic

        private void setProjectsOrPeople(bool projects)
        {
            gridProjectControls.Visibility = (projects) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;
            gridPersonControls.Visibility = (projects) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;

            rectProjects.Opacity = (projects) ? 1d : 0.2d;
            rectPeople.Opacity = (projects) ? 0.2d : 1d;

            gridPeopleOptions.Visibility = System.Windows.Visibility.Collapsed;
            //gridPeople.Visibility = gridPeopleContraints.Visibility = gridPeopleOptions.Visibility = (projects) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            gridPeople.Visibility = gridPeopleContraints.Visibility = (projects) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            gridProjects.Visibility = (projects) ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed;

            dgResourceGrid.ItemsSource = null;
            if (projects)
            {
                dgWorkStream.SelectedIndex = 0;
                dgProject.SelectedIndex = -1;
            }
            else
                dgResources.SelectedIndex = -1;
        }

        private void fillComboDates(ComboBox cb, DateTime dtStart, DateTime dtEnd)
        {
            cb.Items.Clear();
            while (dtStart <= dtEnd)
            {
                cb.Items.Add(new ComboBoxItem() { Content = dtStart.ToString("MMM yyyy") });
                dtStart = dtStart.AddMonths(1);
            }
        }

        private void setDefaultDateRange()
        {
            cbFrom.SelectedIndex = 3; // was 7
            cbTo.SelectedIndex = 2; // was 10
        }



        private void ebProjectFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            reloadProjects();
        }

        public void reloadProjects()
        {
            int ws = 0;
            if (dgWorkStream.SelectedIndex >= 0)
                ws = Convert.ToInt32(((System.Data.DataRowView)dgWorkStream.SelectedItem)["WorkstreamID"].ToString());
            
            fillProjects(ws, ebProjectFilter.Text);
        }
    
        private void fillPeople(string filter)
        {
            DataTable dtResourcesCopy = dtResources.Clone();

            foreach (DataRow dr in dtResources.Rows)
            {
                if ((filter == "") || dr["ResourceName"].ToString().ToUpper().IndexOf(filter.ToUpper()) >= 0)
                    if (dr["ResourceName"].ToString() != "")
                        dtResourcesCopy.ImportRow(dr);
            }

            //////// FILTER RESOURCES FOR ARCHIVED //////
            DataRow[] filtered = dtResourcesCopy.Select("Archived in (" + ((CommonUI.getCheckboxButtonStatus(btnShowArchived)) ? "false,true)" : "false)"));
            if (filtered.Length == 0)
                return;
            DataTable dtResourcesCopyCopy = filtered.CopyToDataTable();

            DataView dv = dtResourcesCopyCopy.DefaultView;
            dv.Sort = "ResourceName";
            DataTable sortedDT = dv.ToTable();

            dgResources.ItemsSource = sortedDT.DefaultView;
        }

        private void fillProjects(int wsid, string filter)
        {
            DataTable dtProjectCopy = dtProject.Clone();

            foreach (DataRow dr in dtProject.Rows)
            {
                if ((filter == "") || dr["ProjectName"].ToString().ToUpper().IndexOf(filter.ToUpper()) >= 0)
                    if (dr["ProjectName"].ToString() != "")
                        if ((wsid == 0) || (Convert.ToInt32(dr["WorkstreamID"].ToString()) == wsid))
                            dtProjectCopy.ImportRow(dr);
            }

            //////// FILTER PROJECTS FOR ARCHIVED //////
            DataRow[] filtered = dtProjectCopy.Select("Archived in (" + ((CommonUI.getCheckboxButtonStatus(btnShowArchived)) ? "false,true)" : "false)"));
            if (filtered.Length == 0)
            {
                dgProject.ItemsSource = null;
                return;
            }
            DataTable dtProjectCopyCopy = filtered.CopyToDataTable();

            DataView dv = dtProjectCopyCopy.DefaultView;
            dv.Sort = "ProjectName";
            DataTable sortedDT = dv.ToTable();

            dgProject.ItemsSource = sortedDT.DefaultView;
        }

        private bool isResourceInProject(string resourceID, string projectID)
        {
            foreach (DataRow drPR in dtProjectResource.Rows)
            {
                if ((drPR["ResourceID"].ToString() == resourceID) && (drPR["ProjectID"].ToString() == projectID))
                {
                    // Jan 16 2017 - Do not include Archived Projects
                    if (isProjectArchived(Convert.ToInt32(projectID)) && !CommonUI.getCheckboxButtonStatus(btnShowArchived))
                        return false;

                    return true;
                }
            }
            return false;
        }

        private void fillPeople()
        {
            fillPeople(ebPeopleFilter.Text);
        }

        private void fillProject()
        {
            if (dgProject.SelectedIndex < 0)
                return;

            string wsid = "";
            if (cbWorkstreamAddFrom.SelectedIndex >= 0)
                wsid = cbWorkstreamAddFrom.SelectedValue.ToString();
            //                wsid = ((ComboBoxItem)cbWorkstreamAddFrom.SelectedValue).Content.ToString();

            string mgrid = "";
            if (cbManagerAddFrom.SelectedIndex >= 0)
                mgrid = cbManagerAddFrom.SelectedValue.ToString();

            bool showTotals = (rectTotalAllocations.Opacity > 0.5d);
            int projid = Convert.ToInt32(((System.Data.DataRowView)dgProject.SelectedItem)["ProjectID"].ToString());

            fillGridForAProject(projid, showTotals, wsid, mgrid);
        }

        private void imgPeopleSearch_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void refreshScreen()
        {
            if (rectPeople.Opacity > 0.5D)
                fillGridForPeople();
            else
                fillProject();
        }

        private void showhideRoleComments()
        {
            colComments.Visibility = (rectRoleComments.Opacity < 0.5d) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
            colRole.Visibility = (rectRoleComments.Opacity < 0.5d) ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;
        }

        #endregion

        #region One Time Import
        // This code is temporary for a one-time data import
        // It's quality is irrelevet
        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("You do not have access to spreadsheet import.");
            return;

            string line;
            int row = 0;

            ////// DUMP ALL THE OLD DATA //////
            executeSQL("truncate Table data.Project");
            executeSQL("truncate Table data.Workstream");
            executeSQL("truncate Table data.Resource");
            executeSQL("truncate Table data.ProjectResource");
            executeSQL("truncate Table data.ProjectResourceInfo");
            executeSQL("truncate Table ref.Role");

            List<String[]> dataraw = new List<string[]>();

            System.IO.StreamReader file =new System.IO.StreamReader(@"c:\docs1\rapp3.txt");
            while ((line = file.ReadLine()) != null)
            {
                if ((line.Length > 20) && (!line.StartsWith("Resource Name")))
                {
                    string[] words = line.Split(new char[] { '\t' });
                    int count = words.Count();
                    dataraw.Add(words);
                    if (count != 345)
                    {
                        MessageBox.Show("OH NO!");
                    }
                    row++;
                }
            }
            file.Close();

            ////// KILL THE TOTAL LINES //////
            List<String[]> data = new List<string[]>();
            for (int i = 0; i < dataraw.Count; i++)
            {
                if ((i < dataraw.Count - 1) && (dataraw[i][0] == dataraw[i + 1][0]))
                    data.Add(dataraw[i]);
            }

            List<string> names = new List<string>();
            List<string> managers = new List<string>();
            List<string> workstreams = new List<string>();
            List<string> projects = new List<string>();


            ////// CREATE A DATATABLE FOR STAGING RESOURCES //////
            DataTable dtResourceStaging = new DataTable();
            dtResourceStaging.Columns.Add("ResourceName");
            dtResourceStaging.Columns.Add("ResourceTitle");
            dtResourceStaging.Columns.Add("ResourceManager");
            dtResourceStaging.Columns.Add("ResourceDepartment");
            dtResourceStaging.Columns.Add("IsManager");
            dtResourceStaging.Columns.Add("IsResource");
            dtResourceStaging.Columns.Add("IsPM");

            ////// CREATE ALL THE MANAGERS //////
            foreach (String[] words in data)
            {
                ////// RESOURCE //////
                string manager = words[columnToIndex("C")];
                manager = manager.Trim();
                manager = manager.Replace("Jervery", "Jervey");
                manager = manager.Replace("Steve Looney", "Stephen Looney");
                manager = manager.Replace("Kalupuru", "Kapuluru");
                manager = manager.Replace("Kaplesh", "Kalpesh");

                if ((manager != "") && !managers.Contains(manager))
                {
                    managers.Add(manager);
                    string sql = "Insert into data.Resource(ResourceName,ResourceTitle, ResourceManagerID, ResourceDepartment, IsManager, IsResource, IsPM) ";
                    sql += "VALUES('" + manager + "','',null,'',1,1,0)";
                    executeSQL(sql);
                }
            }

            ////// CREATE ALL THE PROJECT MANAGERS //////
            foreach (String[] words in data)
            {
                ////// RESOURCE //////
                string pm = words[columnToIndex("F")];
                pm = pm.Replace("(None Assigned)","");
                pm = pm.Trim();
                pm = pm.Replace("Jervery", "Jervey");
                pm = pm.Replace("Steve Looney", "Stephen Looney");
                pm = pm.Replace("Kalupuru", "Kapuluru");
                pm = pm.Replace("Kaplesh", "Kalpesh");

                if (managers.Contains(pm))
                {
                    string sql = "update data.Resource set IsPM=1 where ResourceName='" + pm + "'";
                    executeSQL(sql);
                }

                else if ((pm != "") && !managers.Contains(pm))
                {
                    managers.Add(pm);
                    string sql = "Insert into data.Resource(ResourceName,ResourceTitle, ResourceManagerID, ResourceDepartment, IsManager, IsResource, IsPM) ";
                    sql += "VALUES('" + pm + "','',null,'',1,1,1)";
                    executeSQL(sql);
                }
            }

            ////// READ RESOURCES BACK IN //////
            dtResources = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETRESOURCE", "OPSCONSOLE").Tables["WS"];

            ////// CREATE RESOURCES //////
            foreach (String[] words in data)
            {
                ////// RESOURCE //////
                string name = words[columnToIndex("A")];
                string title = words[columnToIndex("B")];
                string manager = words[columnToIndex("C")];
                string department = words[columnToIndex("D")];

                name = name.Replace(" (TL)","");
                name = name.Replace(" (PM)","");
                name = name.Replace("Jervery", "Jervey");
                name = name.Replace("Steve Looney", "Stephen Looney");
                name = name.Replace("Kalupuru", "Kapuluru");
                name = name.Replace("Kaplesh", "Kalpesh");

                name = name.Trim();

                string mgrid = getkey(dtResources, "ResourceID", "ResourceName", manager);
                if (mgrid == "")
                    mgrid = "null";


                ////// IF THIS PERSON IS A MANAGER OR PM, UPDATE WITH MORE INFO //////
                if (managers.Contains(name))
                {
                    names.Add(name);
                    string resid = getkey(dtResources, "ResourceID", "ResourceName", name);

                    if (resid != "")
                    {
                        if (resid == mgrid)
                            mgrid = "null";

                        string sql = "Update data.Resource set ResourceTitle='" + title + "', ResourceManagerID=" + mgrid + ", ResourceDepartment='" + department + "' where ResourceID=" + resid;
                        executeSQL(sql);
                    }
                }

                ////// OTHERWISE, ADD THEM IF WE DON'T HAVE THEM //////
                if (!names.Contains(name))
                {
                    names.Add(name);
                    string sql = "Insert into data.Resource(ResourceName,ResourceTitle, ResourceManagerID, ResourceDepartment, IsManager, IsResource, IsPM) ";
                    sql += "VALUES('" + name + "','" + title + "'," + mgrid + ",'" + department + "',0,1,0)";
                    executeSQL(sql);
                }

            }

            ////// CREATE ROLES //////
            List<string> roles = new List<string>();
            foreach (String[] words in data)
            {
                ////// WORKSTREAM //////
                string role = words[columnToIndex("H")];
                if ((role!= "") && !roles.Contains(role))
                {
                    roles.Add(role);
                    string sql = "Insert into ref.Role(RoleName) ";
                    sql += "VALUES('" + role + "')";
                    executeSQL(sql);
                }
            }

            ////// READ ROLES BACK IN //////
            dtRole = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETROLES", "OPSCONSOLE").Tables["WS"];

            ////// CREATE WORKSTREAMS //////
            foreach(String[] words in data)
            {
                ////// WORKSTREAM //////
                string workstream = words[columnToIndex("E")];
                if (!workstreams.Contains(workstream))
                {
                    workstreams.Add(workstream);
                    string sql = "Insert into data.Workstream(WorkstreamName) ";
                    sql += "VALUES('" + workstream + "')";
                    executeSQL(sql);
                }
            }

            ////// READ WORKSTREAMS BACK IN //////
            dtWorkstream = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "RP_GETWORKSTREAMS", "OPSCONSOLE").Tables["WS"];
            DataTable dtWorkStreams = new DataTable();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from data.Workstream", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtWorkStreams = dataSet.Tables[0];
            }

            //////// CREATE PROJECTS //////
            foreach (String[] words in data)
            {
                ////// PROJECT //////
                string project = words[columnToIndex("G")];
                project = project.Replace("\"", "");
                string PM = words[columnToIndex("F")];
                string workstream = words[columnToIndex("E")];
                string wsid = getkey(dtWorkStreams, "WorkstreamID", "WorkstreamName", workstream);
                if (!projects.Contains(project))
                {
                    if (project != "")
                    {
                        projects.Add(project);
                        string sql = "Insert into data.Project(ProjectName,ProjectManager,WorkstreamID,ProjectDescription,ProjectRank,ProjectPriority,ProjectApproach,ProjectTiming) ";
                        sql += "VALUES('" + project.Replace("'", "''") + "','" + PM + "'," + wsid + ",'','','','','')";
                        executeSQL(sql);
                    }
                }
            }

            ////// READ PROJECTS BACK IN //////
            DataTable dtProjects = new DataTable();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from data.Project", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtProjects = dataSet.Tables[0];
            }

            ////// READ PEOPLE BACK IN //////
            DataTable dtPeople= new DataTable();
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from data.Resource", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtPeople = dataSet.Tables[0];
            }

            //////// ALLOCATIONS //////
            foreach (String[] words in data)
            {
                ////// PROJECT-RESOURCE //////
                string project = words[columnToIndex("G")];
                string PM = words[columnToIndex("F")];
                string workstream = words[columnToIndex("E")];
                string wsid = getkey(dtWorkStreams, "WorkstreamID", "WorkstreamName", workstream);
                string name = words[columnToIndex("A")];
                name = name.Replace(" (TL)", "");
                name = name.Replace(" (PM)", "");
                name = name.Replace("Jervery", "Jervey");
                name = name.Replace("Steve Looney", "Stephen Looney");
                name = name.Replace("Kalupuru", "Kapuluru");
                name = name.Replace("Kaplesh", "Kalpesh");
                name = name.Trim();

                project = project.Replace("\"", "");
                string pid = getkey(dtProjects, "ProjectID", "ProjectName", project);
                string rid = getkey(dtPeople, "ResourceID", "ResourceName", name);

                if ((pid != "") && (rid != ""))
                {
                    ////// PUT IN THE PROJECT RESOURCE INFO //////
                    string role = words[columnToIndex("H")];
                    string comments = words[columnToIndex("I")];
                    string roleid = getkey(dtRole, "RoleID", "RoleName", role);
                    if (roleid == "")
                        roleid = "null";
                    string sqlpri = "Insert into data.ProjectResourceInfo(ProjectID, ResourceID, RoleID, Comments) ";
                    sqlpri += "VALUES(" + pid + "," + rid + "," + roleid + ",'" + comments.Replace("'","''") + "')";
                    executeSQL(sqlpri);

                    ////// RUN THROUGH THE ALLOCATIONS //////
                    for (int col=columnToIndex("L"); col<= columnToIndex("BF"); col++)
                    {
                        DateTime date = DateTime.Parse("2/15/16").AddDays((col - columnToIndex("L")) * 7);
                        string perct = words[col].Replace("%","");

                        if ((perct.Trim() != "") && (perct.Trim() != "0"))
                        {
                            string sql = "Insert into data.ProjectResource(ProjectID, ResourceID, WeekStartDate, ProjectPercentage) ";
                            sql += "VALUES(" + pid + "," + rid + ",'" + date.ToString("MM/dd/yyyy") + "'," + perct + ")";
                            executeSQL(sql);
                        }
                    }

                }
            }

            MessageBox.Show("All done");

        }
        #endregion

        #region Database Helper
        public string getkey(DataTable dt, string keyCol, string valCol, string val)
        {
            foreach (DataRow dr in dt.Rows)
            {
                if (dr[valCol].ToString() == val)
                    return dr[keyCol].ToString();
            }
            return "";
        }

        public void executeSQL(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error processing SQL" + Environment.NewLine + Environment.NewLine + ex.ToString() + Environment.NewLine + Environment.NewLine + "SQL: " + sql);
            }
        }
        #endregion

        #region FILL PEOPLE
        private void fillGridForPeople()
        {
            if (dtLastMonth <= dtFirstMonth)
                return;

            DataView dv = dtProject.DefaultView;
            dv.Sort = "WorkstreamID";
            DataTable dtProjectStored = dv.ToTable();

            colPerson1.Visibility = System.Windows.Visibility.Visible;
            colPerson2.Visibility = System.Windows.Visibility.Collapsed;
            colWorkstream.Visibility = System.Windows.Visibility.Visible;
            colProject.Visibility = System.Windows.Visibility.Visible;
            colComments.Visibility = System.Windows.Visibility.Collapsed;
            colRole.Visibility = System.Windows.Visibility.Collapsed;

            DataTable dtTest = new DataTable();
            dtTest.Columns.Add("Project");
            dtTest.Columns.Add("ProjectID");
            dtTest.Columns.Add("WorkstreamName");
            dtTest.Columns.Add("Person");
            dtTest.Columns.Add("ResourceID");
            dtTest.Columns.Add("vis");
            dtTest.Columns.Add("Background");
            for (int i = 0; i < 26; i++)
            {
                string col = "C" + (i + 1).ToString();
                if (columns[col].Visibility == System.Windows.Visibility.Visible)
                {
                    dtTest.Columns.Add(col);
                    dtTest.Columns.Add(col + "B");
                    dtTest.Columns.Add(col + "Color");
                }
            }

            foreach (var x in dgResources.SelectedItems)
            {
                if (x is System.Data.DataRowView)
                {
                    string name = ((System.Data.DataRowView)x)["ResourceName"].ToString();
                    string resid = ((System.Data.DataRowView)x)["ResourceID"].ToString();
                    Dictionary<string, int> totals = new Dictionary<string, int>();
                    int rowcount = 0;

                    string prevworkstream ="", prevdispname = "";
                    foreach (DataRow drProj in dtProjectStored.Rows)
                    {
                        string projid = drProj["ProjectID"].ToString();

                        if (isResourceInProject(resid, projid))
                        {
                            string project = drProj["Projectname"].ToString();
                            string workstream = getkey(dtWorkstream, "WorkstreamName", "WorkstreamID", drProj["WorkstreamID"].ToString());
                            string dispname = name;
                            string dispws = workstream;

                            if (dispname == prevdispname)
                            {
                                dispname = "";
                                if (workstream == prevworkstream)
                                    dispws = "";
                            }

                            dtTest.Rows.Add(project,projid, dispws,dispname,resid,"Collapsed");
                            rowcount++;

                            prevdispname = name;
                            prevworkstream = workstream;
                        }
                    }

                    foreach (DataRow drProjRes in dtProjectResource.Rows)
                    {
                        if (drProjRes["ResourceID"].ToString() == resid)
                        {
                            string projid = drProjRes["ProjectID"].ToString();
                            DateTime dtWeekStartDate = (DateTime) drProjRes["WeekStartDate"];
                            if ((dtWeekStartDate >= dtFirstMonth) && (dtWeekStartDate <= dtLastMonth))
                            {
                                int icol = (int) ((dtWeekStartDate - dtFirstMonth).TotalDays / 7d);
                                if (icol < 26)
                                {
                                    string col = "C" + (icol + 1).ToString();

                                    foreach (DataRow dr in dtTest.Rows)
                                    {
                                        if ((dr["ProjectID"].ToString() == projid) && (dr["ResourceID"].ToString() == resid))
                                        {
                                            dr[col] = drProjRes["ProjectPercentage"].ToString();
                                            // if (totals.ContainsKey(col))
                                            int rt = 0;
                                            totals.TryGetValue(col, out rt);
                                            totals[col] = rt + ((int)drProjRes["ProjectPercentage"]);

                                            ////// IF THEY ONLY HAVE ONE PROJECT - COLOR CODE IT //////
                                            if (rowcount == 1)
                                            {
                                                if (((int)drProjRes["ProjectPercentage"]) <= 60)
                                                    dr[col + "Color"] = "LightGreen";
                                            }

                                            break;
                                        }
                                    }
                                }

                            }
                        }
                    }

                    ////// TOTALS ROW //////
                    if (rowcount > 1)
                    {
                        DataRow drt = dtTest.NewRow();
                        drt["vis"] = "Collapsed";
                        drt["Person"] = name + " - Total";
                        foreach (KeyValuePair<string, int> kvp in totals)
                        {
                            drt[kvp.Key] = kvp.Value.ToString();
                            if (kvp.Value <= 60)
                                drt[kvp.Key + "Color"] = "LightGreen";
                            if (kvp.Value > 100)
                                drt[kvp.Key + "Color"] = "LightPink";
                        }
                        drt["Background"] = "LightGray";
                        dtTest.Rows.Add(drt);
                    }
                    else
                    {
                        if (dgResources.SelectedItems.Count > 1)
                            dtTest.Rows[dtTest.Rows.Count - 1]["Background"] = "LightGray";
                    }

                    ////// BLANK ROW //////
                    DataRow drblank = dtTest.NewRow();
                    dtTest.Rows.Add(drblank);

                }


            }


            dgResourceGrid.ItemsSource = dtTest.DefaultView;
        }
        #endregion

        #region FILL OVER / UNDER ALLOCATED
        private void fillOverOrUnderSubscribed(bool bOver)
        {
            if (dtLastMonth <= dtFirstMonth)
                return;

            DataView dv = dtProject.DefaultView;
            dv.Sort = "WorkstreamID";
            DataTable dtProjectStored = dv.ToTable();

            colPerson1.Visibility = System.Windows.Visibility.Visible;
            colPerson2.Visibility = System.Windows.Visibility.Collapsed;
            colWorkstream.Visibility = System.Windows.Visibility.Visible;
            colProject.Visibility = System.Windows.Visibility.Visible;
            colComments.Visibility = System.Windows.Visibility.Collapsed;
            colRole.Visibility = System.Windows.Visibility.Collapsed;

            DataTable dtTest = new DataTable();
            dtTest.Columns.Add("Project");
            dtTest.Columns.Add("ProjectID");
            dtTest.Columns.Add("WorkstreamName");
            dtTest.Columns.Add("Person");
            dtTest.Columns.Add("ResourceID");
            dtTest.Columns.Add("vis");
            dtTest.Columns.Add("Background");
            dtTest.Columns.Add("Total", typeof(int));
            for (int i = 0; i < 26; i++)
            {
                string col = "C" + (i + 1).ToString();
                if (columns[col].Visibility == System.Windows.Visibility.Visible)
                {
                    dtTest.Columns.Add(col);
                    dtTest.Columns.Add(col + "B");
                    dtTest.Columns.Add(col + "Color");
                }
            }

            
            ////// CALCULATE TOTALS //////
            foreach (DataRow drAll in dtResources.Rows)
            {
                string resid = drAll["ResourceID"].ToString();
                string name = getkey(dtResources, "ResourceName", "ResourceID", resid.ToString());

                if (isResourceArchived(Convert.ToInt32(resid)))
                    continue;

                Dictionary<string, int> atotals = new Dictionary<string, int>();

                foreach (DataRow drProjRes in dtProjectResource.Rows)
                {
                    if (drProjRes["ResourceID"].ToString() == resid)
                    {
                        DateTime dtWeekStartDate = (DateTime)drProjRes["WeekStartDate"];
                        if ((dtWeekStartDate >= dtFirstMonth) && (dtWeekStartDate <= dtLastMonth))
                        {
                            int icol = (int)((dtWeekStartDate - dtFirstMonth).TotalDays / 7d);
                            if (icol < 26)
                            {
                                string col = "C" + (icol + 1).ToString();

                                int rt = 0;
                                atotals.TryGetValue(col, out rt);
                                atotals[col] = rt + ((int)drProjRes["ProjectPercentage"]);
                            }
                        }
                    }
                }

                ////// TOTALS ROW //////
                DataRow drt = dtTest.NewRow();
                drt["vis"] = "Collapsed";
                drt["Person"] = name;
                drt["ResourceID"] = resid;
                int over = 0;
                int under = 0;
                int total = 0;
                foreach (KeyValuePair<string, int> kvp in atotals)
                {
                    total += kvp.Value;
                    drt[kvp.Key] = kvp.Value.ToString();
                    if (kvp.Value <= 60)
                    {
                        under++;
                        drt[kvp.Key + "Color"] = "LightGreen";
                    }
                    else if (kvp.Value <= 100)
                        drt[kvp.Key + "Color"] = "Gold";
                    else if (kvp.Value > 100)
                    {
                        over++;
                        drt[kvp.Key + "Color"] = "LightPink";
                    }
                }
                // drt["Background"] = "LightGray";
                drt["Total"] = total;
                if ((bOver) && (over > 3))
                    dtTest.Rows.Add(drt);

                // jan 10 2017 - Include people with no over
                else if ((!bOver) &&  (  ((under > 3) && (over < 3)) || ((under>= 1) && (over==0)) ))
                    dtTest.Rows.Add(drt);
            }

            DataView dvs = dtTest.DefaultView;
            dvs.Sort = (bOver) ? "Total desc" : "Total";
            DataTable dtSorted = dvs.ToTable();


            dgResourceGrid.ItemsSource = dtSorted.DefaultView;
        }
        #endregion

        #region FILL PROJECTS
        private void fillGridForAProject(int projectid, bool showTotals, string addfromwsid, string addfrommgrid)
        {
            if (dtLastMonth <= dtFirstMonth)
                return;

            colPerson1.Visibility = System.Windows.Visibility.Visible;
            colWorkstream.Visibility = System.Windows.Visibility.Collapsed;
            colProject.Visibility = System.Windows.Visibility.Collapsed;
            colPerson2.Visibility = System.Windows.Visibility.Collapsed;
            showhideRoleComments();

            ////// CREATE BACKING TABLE //////
            DataTable dtTest = new DataTable();
            dtTest.Columns.Add("Project");
            dtTest.Columns.Add("ProjectID");
            dtTest.Columns.Add("WorkstreamName");
            dtTest.Columns.Add("Person");
            dtTest.Columns.Add("ResourceID");
            dtTest.Columns.Add("vis");
            dtTest.Columns.Add("Background");
            dtTest.Columns.Add("RoleName");
            dtTest.Columns.Add("Comments");

            for (int i = 0; i < 26; i++)
            {
                string col = "C" + (i + 1).ToString();
                // Create all columns - 5/27/16
                //if (columns[col].Visibility == System.Windows.Visibility.Visible)
                {
                    dtTest.Columns.Add(col);
                    dtTest.Columns.Add(col + "B");
                    dtTest.Columns.Add(col + "Color");
                }
            }

            ////// FIND ALL RESOURCES WITHIN THIS DATE RANGE //////
            List<int> foundResources = new List<int>();
            string project = getkey(dtProject, "ProjectName", "ProjectID", projectid.ToString());
            string wsid = getkey(dtProject, "WorkstreamID", "ProjectID", projectid.ToString());
            string workstream = getkey(dtWorkstream, "WorkstreamName", "WorkstreamID", wsid);

            foreach (DataRow drProjRes in dtProjectResource.Rows)
            {
                int projid = (int)drProjRes["ProjectID"];
                int resid = (int)drProjRes["ResourceID"];
                DateTime dtWeekStartDate = (DateTime)drProjRes["WeekStartDate"];

                if ((projid == projectid) && ((dtWeekStartDate >= dtFirstMonth) && (dtWeekStartDate <= dtLastMonth)))
                {
                    if (!foundResources.Contains(resid))
                    {
                        string name = getkey(dtResources, "ResourceName", "ResourceID", resid.ToString());
                        string vis = (showTotals) ? "Visible" : "Collapsed";

                        ////// FIND PROJECT RESOURCE INFO //////
                        string rolename = "";
                        string comments = "";
                        foreach (DataRow drProjResInfo in dtProjectResourceInfo.Rows)
                        {
                            if ((drProjResInfo["ProjectID"].ToString() == projectid.ToString()) && (drProjResInfo["ResourceID"].ToString() == resid.ToString()))
                            {
                                comments = drProjResInfo["Comments"].ToString();
                                string roleid = drProjResInfo["RoleID"].ToString();
                                rolename = getkey(dtRole, "RoleName", "RoleID", roleid);
                                break;
                            }
                        }

                        dtTest.Rows.Add(project, projid, workstream, name, resid.ToString(), vis, "", rolename, comments);
                        foundResources.Add(resid);
                    }
                }
            }

            ////// GO THROUGH ALL PROJECT RESOURCE PAIRS AND ADD UP TOTALS //////
            Dictionary<string, int> totals = new Dictionary<string, int>();
            DataTable dtGraphTable = new DataTable();
            dtGraphTable.Columns.Add("Date",typeof(DateTime));
            dtGraphTable.Columns.Add("Days", typeof(double));

            int weeks = (int)(((dtLastMonth - dtFirstMonth).TotalDays) / 7d) + 1;
            if (weeks > 26)
                weeks = 26;
            for (int i = 0; i < weeks; i++)
            {
                string date = dtFirstMonth.AddDays(i * 7).ToString("MMM d");
                dtGraphTable.Rows.Add(date,0d);
            }

            foreach (DataRow drProjRes in dtProjectResource.Rows)
            {
                int projid = (int)drProjRes["ProjectID"];
                int resid = (int)drProjRes["ResourceID"];
                DateTime dtWeekStartDate = (DateTime)drProjRes["WeekStartDate"];
                if ((dtWeekStartDate >= dtFirstMonth) && (dtWeekStartDate <= dtLastMonth))
                {
                    int icol = (int)((dtWeekStartDate - dtFirstMonth).TotalDays / 7d);
                    if (icol < 26)
                    {
                        string col = "";

                        if (projid == projectid)
                        {
                            col = resid.ToString() + "-C" + (icol + 1).ToString();

                            int rt = 0;
                            totals.TryGetValue(col, out rt);
                            totals[col] = rt + ((int)drProjRes["ProjectPercentage"]);

                            dtGraphTable.Rows[icol]["Days"] = ((double)dtGraphTable.Rows[icol]["Days"]) + (((double)totals[col]) / 20d);
                        }

                        col = resid.ToString() + "-C" + (icol + 1).ToString() + "B";
                        int rtap = 0;
                        totals.TryGetValue(col, out rtap);
                        totals[col] = rtap + ((int)drProjRes["ProjectPercentage"]);
                    }
                }
            }

            graphPeopleDays.Visibility = System.Windows.Visibility.Collapsed;
            //graphPeopleDays.setDataDateSeries("Total person days per week", dtGraphTable, "Date", "Days", "");

            ////// FILL IN THE BACKING TABLE //////
            foreach (DataRow drPerson in dtTest.Rows)
            {
                string resid = drPerson["ResourceID"].ToString();

                foreach (KeyValuePair<string, int> kvp in totals)
                {
                    string key = kvp.Key;
                    int val = kvp.Value;

                    if (key.StartsWith(resid + "-"))
                    {
                        string colname = key.Replace(resid + "-", "");
                        drPerson[colname] = val.ToString();

                        if ((colname.IndexOf("B") > 0) && (kvp.Value > 100))
                            drPerson[colname.Replace("B","") + "Color"] = "LightPink";
                    }
                }
            }

            DataView dv = dtTest.DefaultView;
            dv.Sort = "Person";
            DataTable dtTestSorted = dv.ToTable();


            ////// IF WE ARE SHOWING RESOURCES TO ADD FROM A WORKSTREAM OR MANAGER //////
            List<int> availableResources = new List<int>();
            if ((addfromwsid != "") || (addfrommgrid != ""))
            {
                ////// BLANK ROW //////
                DataRow drblank = dtTestSorted.NewRow();
                dtTestSorted.Rows.Add(drblank);

                ////// INFO ROW //////
                DataRow drInfo = dtTestSorted.NewRow();
                drInfo["Person"] = "Available Resources";
                drInfo["Background"] = "LightGreen";
                drInfo["vis"] = "Collapsed";
                dtTestSorted.Rows.Add(drInfo);

                ////// FIND RESOURCES IN THIS WORKSTREAM WE AREN'T CURRENTLY USING //////
                if (addfromwsid != "")
                {
                    foreach (DataRow drProjRes in dtProjectResource.Rows)
                    {
                        int projid = (int)drProjRes["ProjectID"];
                        string wsidpr = getkey(dtProject, "WorkstreamID", "ProjectID", projid.ToString());
                        int resid = (int)drProjRes["ResourceID"];

                        if (wsidpr == addfromwsid)
                        {
                            if (!foundResources.Contains(resid))
                            {
                                if (!availableResources.Contains(resid))
                                    availableResources.Add(resid);
                            }
                        }
                    }
                }

                ////// FIND PEOPLE UNDER A MANAGER THAT AREN'T ON THIS PROJECT //////
                if (addfrommgrid != "")
                {
                    foreach (DataRow drRes in dtResources.Rows)
                    {
                        if (drRes["ResourceManagerID"].ToString() == addfrommgrid)
                        {
                            int resid = (int)drRes["ResourceID"];

                            if (!foundResources.Contains(resid))
                            {
                                if (!availableResources.Contains(resid))
                                {
                                    // Jan 10 2017 - Don't add archived resources
                                    if (!isResourceArchived(resid))
                                        availableResources.Add(resid);
                                }
                            }
                        }
                    }
                }


                ////// CALCULATE TOTALS //////
                foreach (int r in availableResources)
                {
                    string resid = r.ToString();
                    string name = getkey(dtResources, "ResourceName", "ResourceID", resid.ToString());

                    Dictionary<string, int> atotals = new Dictionary<string, int>();

                    foreach (DataRow drProjRes in dtProjectResource.Rows)
                    {
                        if (drProjRes["ResourceID"].ToString() == resid)
                        {
                            DateTime dtWeekStartDate = (DateTime)drProjRes["WeekStartDate"];
                            if ((dtWeekStartDate >= dtFirstMonth) && (dtWeekStartDate <= dtLastMonth))
                            {
                                int icol = (int)((dtWeekStartDate - dtFirstMonth).TotalDays / 7d);
                                if (icol < 26)
                                {
                                    string col = "C" + (icol + 1).ToString();

                                    int rt = 0;
                                    atotals.TryGetValue(col, out rt);
                                    atotals[col] = rt + ((int)drProjRes["ProjectPercentage"]);
                                }
                            }
                        }
                    }

                    ////// TOTALS ROW //////
                    DataRow drt = dtTestSorted.NewRow();
                    drt["vis"] = "Collapsed";
                    drt["Person"] = name;
                    drt["ResourceID"] = resid;
                    foreach (KeyValuePair<string, int> kvp in atotals)
                    {
                        drt[kvp.Key] = kvp.Value.ToString();
                        if (kvp.Value <= 60)
                            drt[kvp.Key + "Color"] = "LightGreen";
                        else if (kvp.Value <= 100)
                            drt[kvp.Key + "Color"] = "Gold";
                        else if (kvp.Value > 100)
                            drt[kvp.Key + "Color"] = "LightPink";
                    }
                    // drt["Background"] = "LightGray";
                    dtTestSorted.Rows.Add(drt);


                }

            }

            dgResourceGrid.ItemsSource = dtTestSorted.DefaultView;
            return;
        }

        // Jan 16, 2017
        private bool isResourceArchived(int resid)
        {
            foreach (DataRow drAll in dtResources.Rows)
                if (resid.ToString() == drAll["ResourceID"].ToString())
                    return (drAll["Archived"].ToString().ToUpper() == "TRUE") ? true : false;
            return false;
        }

        // Jan 16, 2017
        private bool isProjectArchived(int projid)
        {
            foreach (DataRow drAll in dtProject.Rows)
                if (projid.ToString() == drAll["ProjectID"].ToString())
                    return (drAll["Archived"].ToString().ToUpper() == "TRUE") ? true : false;
            return false;
        }

        #endregion

        #region Background Thread - Update data and Initial load from AD

        private void worker_DoWork(object sender, DoWorkEventArgs e)
        {
            ////// MODIFY PROJECTR ESOURCE AND GET MODIFIED RESULTS //////
            dtProjectResource = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtProjectResourceUpdate, "RP_CRUD_PROJECTRESOURCE", "OPSCONSOLE").Tables["WS"];
        }

        private void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ////// REFRESH THE SCREEN //////
            if (rectPeople.Opacity > 0.5D)
                fillGridForPeople();
            else
                fillProject();
            pleaseWait(wait: false);
        }

        private void worker_DoWork2(object sender, DoWorkEventArgs e)
        {
            ////// MODIFY PROJECTR ESOURCE AND GET MODIFIED RESULTS //////
            if (dtEmployees.Rows.Count == 0)
            {
                DataTable dtRawEmployees = new DataTable();
                dtRawEmployees = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "GET_ALL_EMPLOYEES", "OPSCONSOLE").Tables["Users"];
                dtEmployees = dtRawEmployees.Clone();

                foreach (DataRow drr in dtRawEmployees.Rows)
                {
                    if (drr["Department"].ToString() == "Operations")
                        dtEmployees.ImportRow(drr);
                }

                dtEmployees.Columns.Add("RealName");
                foreach (DataRow dr in dtEmployees.Rows)
                {
                    string displayName = dr["DisplayName"].ToString();
                    int comma = displayName.IndexOf(",");
                    if (comma > 0)
                        dr["RealName"] = displayName.Substring(comma + 2) + " " + displayName.Substring(0, comma);
                    else
                        dr["RealName"] = displayName;
                }
            }
        }

        private void worker_RunWorkerCompleted2(object sender, RunWorkerCompletedEventArgs e)
        {
            rpResource.doneLoadingAD();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            imgAngle.Angle += 5d;
            if (imgAngle.Angle >= 355d)
                imgAngle.Angle = 0d;
        }

        private void pleaseWait(bool wait)
        {
            if (wait)
            {
                dispatcherTimer.Start();
                imgWait.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                dispatcherTimer.Stop();
                imgWait.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #endregion

        #region Date Helper code
        private DateTime previousMonday(DateTime dt)
        {
            int sub = 0;
            while (true)
            {
                if (dt.AddDays(-sub).DayOfWeek == DayOfWeek.Monday)
                    return (dt.AddDays(-sub));
                sub++;
            }
        }
        #endregion

        #region UI Helper code
        private int columnToIndex(string col)
        {
            if (col.Length == 1)
                return col[0] - 'A';
            else
                return (col[0] - 'A' + 1) * 26 + col[1] - 'A';

        }

        public bool setDropdownFromValue(ComboBox c, string key, string value)
        {
            int index = 0;
            foreach (System.Data.DataRowView drv in c.Items)
            {
                if (drv[key].ToString() == value)
                {
                    c.SelectedIndex = index;
                    return true;
                }
                index++;
            }
            return false;
        }

        private DateTime DateForColumn(int col)
        {
            if (dtFirstMonth.DayOfWeek != DayOfWeek.Monday)
            {
                MessageBox.Show("Error DateForColumn: The first day of the week must be Monday");
            }
            return dtFirstMonth.AddDays(col * 7);
        }

        ////// TO DO - FIND A BETTER PLACE FOR THIS //////
        public void setDropdownFromValue(ComboBox c, string value)
        {
            int index = 0;
            foreach (ComboBoxItem ci in c.Items)
            {
                if (ci.Content.ToString() == value)
                {
                    c.SelectedIndex = index;
                    return;
                }
                index++;
            }
        }
        #endregion

        #region Code for giving presentations - DISABLED
        private void gridViewBy_MouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (((SolidColorBrush)gridViewBy.Background).Color.R == 104)
                gridViewBy.Background = new SolidColorBrush(Color.FromArgb(255, 212, 174, 44));
            else
                gridViewBy.Background = new SolidColorBrush(Color.FromArgb(255, 104, 122, 147));
        }

        private void gridPeople_MouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (((SolidColorBrush)gridPeople.Background).Color.R == 104)
                gridPeople.Background = new SolidColorBrush(Color.FromArgb(255, 212, 174, 44));
            else
                gridPeople.Background = new SolidColorBrush(Color.FromArgb(255, 104, 122, 147));
        }

        private void gridPeopleContraints_MouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (((SolidColorBrush)gridPeopleContraints.Background).Color.R == 104)
                gridPeopleContraints.Background = new SolidColorBrush(Color.FromArgb(255, 212, 174, 44));
            else
                gridPeopleContraints.Background = new SolidColorBrush(Color.FromArgb(255, 104, 122, 147));
        }

        private void gridRange_MouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (((SolidColorBrush)gridRange.Background).Color.R == 104)
                gridRange.Background = new SolidColorBrush(Color.FromArgb(255, 212, 174, 44));
            else
                gridRange.Background = new SolidColorBrush(Color.FromArgb(255, 104, 122, 147));
        }

        private void gridAllocate_MouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (((SolidColorBrush)gridAllocate.Background).Color.R == 104)
                gridAllocate.Background = new SolidColorBrush(Color.FromArgb(255, 212, 174, 44));
            else
                gridAllocate.Background = new SolidColorBrush(Color.FromArgb(255, 104, 122, 147));
        }

        private void gridProjectControls_MouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (((SolidColorBrush)gridProjectControls.Background).Color.R == 104)
                gridProjectControls.Background = new SolidColorBrush(Color.FromArgb(255, 212, 174, 44));
            else
                gridProjectControls.Background = new SolidColorBrush(Color.FromArgb(255, 104, 122, 147));
        }

        private void gridPersonControls_MouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (((SolidColorBrush)gridPersonControls.Background).Color.R == 104)
                gridPersonControls.Background = new SolidColorBrush(Color.FromArgb(255, 212, 174, 44));
            else
                gridPersonControls.Background = new SolidColorBrush(Color.FromArgb(255, 104, 122, 147));
        }

        private void gridProjects_MouseDown(object sender, MouseButtonEventArgs e)
        {
            return;
            if (((SolidColorBrush)gridProjects.Background).Color.R == 104)
                gridProjects.Background = new SolidColorBrush(Color.FromArgb(255, 212, 174, 44));
            else
                gridProjects.Background = new SolidColorBrush(Color.FromArgb(255, 104, 122, 147));
        }
        #endregion


        #region Future code
        private void btnShowArchived_Click(object sender, RoutedEventArgs e)
        {
            // Not a feature yet
            return;

            CommonUI.setCheckboxButtonStatus(btnShowArchived, !CommonUI.getCheckboxButtonStatus(btnShowArchived));

            fillPeople(ebPeopleFilter.Text);
            fillPMDropdown();
            reloadProjects();
        }
        #endregion

    }
}
