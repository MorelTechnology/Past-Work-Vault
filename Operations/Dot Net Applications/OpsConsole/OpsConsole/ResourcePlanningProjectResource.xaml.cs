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
    /// Interaction logic for ResourcePlanningProjectResource.xaml
    /// </summary>
    public partial class ResourcePlanningProjectResource : UserControl
    {
        #region Class Data
        ResourcePlanning parent = null;

        public enum mode { add, edit, remove, removeSecondRow };
        public mode eMode = mode.add;
        string resid = "";
        string projid = "";
        string ProjectResourceID = "";
        #endregion

        #region Constructor
        public ResourcePlanningProjectResource()
        {
            InitializeComponent();
        }
        #endregion

        #region Load Data
        public void Load(ResourcePlanning prnt, string rid, string resname, string pid, string roleid, string comment)
        {
            parent = prnt;
            resid = rid;
            projid = pid;
            cbRole.SelectedIndex = -1;

            ////// POPULATE TITLE //////
            lblTitle.Text = "Edit Project Resource Info for " + resname;

            ////// POPULATE ROLES //////
            cbRole.ItemsSource = parent.dtRole.DefaultView;

            ////// SELECT ROLE IF THERE IS ONE //////
            if ((roleid != null) && (roleid != "") && (roleid != "0"))
            {
                string rolename = parent.getkey(parent.dtRole, "RoleName", "RoleID", roleid);
                parent.setDropdownFromValue(cbRole, "RoleName", rolename);
            }

            ////// PUT IN TEXT //////
            txtComments.Text = comment;
        }
        #endregion

        #region Commands
        private void Save()
        {
            DataTable dtResourceUpdate = new DataTable("ProjectResourceInfo");
            dtResourceUpdate.Columns.Add("ProjectResourceInfoID");
            dtResourceUpdate.Columns.Add("ProjectID");
            dtResourceUpdate.Columns.Add("ResourceID");
            dtResourceUpdate.Columns.Add("RoleID");
            dtResourceUpdate.Columns.Add("Comments");
            dtResourceUpdate.Columns.Add("Operation");

            string roleid = "0";
            if (cbRole.SelectedIndex >= 0)
                roleid = ((System.Data.DataRowView)cbRole.SelectedItem).Row["RoleID"].ToString();

            DataRow drRPI = dtResourceUpdate.NewRow();
            drRPI["ProjectResourceInfoID"] = "";
            drRPI["ProjectID"] = projid;
            drRPI["ResourceID"] = resid;
            drRPI["RoleID"] = roleid;
            drRPI["Comments"] = txtComments.Text;
            drRPI["Operation"] = "X";
            dtResourceUpdate.Rows.Add(drRPI);

            DataRow drRPIA = dtResourceUpdate.NewRow();
            drRPIA["ProjectResourceInfoID"] = "";
            drRPIA["ProjectID"] = projid;
            drRPIA["ResourceID"] = resid;
            drRPIA["RoleID"] = roleid;
            drRPIA["Comments"] = txtComments.Text;
            drRPIA["Operation"] = "I";
            dtResourceUpdate.Rows.Add(drRPIA);

            parent.dtProjectResourceInfo = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtResourceUpdate, "RP_CRUD_PROJECTRESOURCEINFO", "OPSCONSOLE").Tables["WS"];
            parent.refreshScreen();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
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
