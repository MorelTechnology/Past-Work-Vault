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
    /// Interaction logic for ResourcePlanningProject.xaml
    /// </summary>
    public partial class ResourcePlanningProject : UserControl
    {
        #region Class Data
        ResourcePlanning parent = null;

        public enum mode { add, edit, remove, removeSecondRow };
        public mode eMode = mode.add;
        string s_projid = "";
        string ProjectID = "";
        #endregion

        #region Constructor
        public ResourcePlanningProject()
        {
            InitializeComponent();
        }
        #endregion

        #region Load Data
        public void Load(ResourcePlanning prnt)
        {
            parent = prnt;

            ////// POPULATE WORKSTREAMS //////
            DataRow[] filteredws = parent.dtWorkstream.Select("WorkstreamName <> 'All'");
            cbWorkstream.ItemsSource = (filteredws.Length == 0) ? null : filteredws.CopyToDataTable().DefaultView;
            
            ////// POPULATE RESOURCES //////
            DataRow[] filtered = parent.dtResources.Select("IsPM = 1 and Archived=false");
            cbProjectManager.ItemsSource = (filtered.Length == 0) ? null : filtered.CopyToDataTable().DefaultView;

            cbWorkstream.SelectedIndex = -1;
            cbProjectManager.SelectedIndex = -1;
        }
        #endregion

        #region Commands
        public void Add()
        {
            lblTitle.Text = "Add Project";
            txtDescription.Text = "";
            txtName.Text = "";
            cbProjectManager.SelectedIndex = -1;
            cbWorkstream.SelectedIndex = -1;
            eMode = mode.add;
        }

        public void Edit(string projid, string pmid, string wsid, string projname, string projdesc)
        {
            txtDescription.Text = projdesc;
            txtName.Text = projname;
            lblTitle.Text = "Edit Project";

            cbProjectManager.SelectedIndex = -1;
            cbWorkstream.SelectedIndex = -1;

            parent.setDropdownFromValue(cbProjectManager, "ResourceID", pmid);
            parent.setDropdownFromValue(cbWorkstream, "WorkstreamID", wsid);

            s_projid = projid;
            eMode = mode.edit;
        }

        private void Save()
        {
            ////// Make sure the name is unique //////
            DataTable dtResourceUpdate = new DataTable("Project");
            dtResourceUpdate.Columns.Add("ProjectID");
            dtResourceUpdate.Columns.Add("ProjectName");
            dtResourceUpdate.Columns.Add("ProjectDescription");
            dtResourceUpdate.Columns.Add("ProjectRank");
            dtResourceUpdate.Columns.Add("ProjectPriority");
            dtResourceUpdate.Columns.Add("ProjectApproach");
            dtResourceUpdate.Columns.Add("ProjectTiming");
            dtResourceUpdate.Columns.Add("WorkstreamID");
            dtResourceUpdate.Columns.Add("ProjectManager");
            dtResourceUpdate.Columns.Add("Operation");

            string workstreamid = ((System.Data.DataRowView)cbWorkstream.SelectedItem).Row["WorkstreamID"].ToString();

            string pmid = "0";
            if (cbProjectManager.SelectedIndex >= 0)
                pmid = ((System.Data.DataRowView)cbProjectManager.SelectedItem).Row["ResourceID"].ToString();
            string pmanager = parent.getkey(parent.dtResources, "ResourceName", "ResourceID", pmid);



            DataRow drRPI = dtResourceUpdate.NewRow();

            drRPI["ProjectID"] = (eMode == mode.add) ? "" : s_projid;
            drRPI["ProjectName"] = txtName.Text;
            drRPI["ProjectDescription"] = txtDescription.Text;
            drRPI["ProjectRank"] = "";
            drRPI["ProjectPriority"] = "";
            drRPI["ProjectApproach"] = "";
            drRPI["ProjectTiming"] = "";
            drRPI["WorkstreamID"] = workstreamid;
            drRPI["ProjectManager"] = pmanager;
            drRPI["Operation"] = (eMode == mode.add) ? "I" : "U";
            dtResourceUpdate.Rows.Add(drRPI);

            parent.dtProject = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtResourceUpdate, "RP_CRUD_PROJECTS", "OPSCONSOLE").Tables["WS"];
            // parent.refreshScreen();
            parent.reloadProjects();
            Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ////// MAKE SURE WORKSTREAM IS SELECTED //////
            if (cbWorkstream.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a Workstream for this project to be in");
                return;
            }

            if (txtName.Text == "")
            {
                MessageBox.Show("The project must have a name");
                return;
            }

            ////// MAKE SURE PM IS SELECTED //////
            if (cbProjectManager.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a Project Manager for this project");
                return;
            }


            Save();
            Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion
    }
}
