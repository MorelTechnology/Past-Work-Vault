using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices;
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
    /// Interaction logic for ResourcePlanningResource.xaml
    /// </summary>
    public partial class ResourcePlanningResource : UserControl
    {
        #region Class Data
        ResourcePlanning parent = null;

        public enum mode { add, edit, remove, removeSecondRow };
        public mode eMode = mode.add;

        string ResourceID = "";
        #endregion

        #region Constructor
        public ResourcePlanningResource()
        {
            InitializeComponent();
        }
        #endregion

        #region Load Data
        public void Load(ResourcePlanning prnt)
        {
            parent = prnt;

            cbDepartment.ItemsSource = parent.dtDepartment.DefaultView;

            DataRow[] filtered = parent.dtResources.Select("IsManager = 1");
            cbManager.ItemsSource = (filtered.Length == 0) ? null : filtered.CopyToDataTable().DefaultView;

            cbManager.SelectedIndex = -1;
            cbDepartment.SelectedIndex = -1;

            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 300;
            myDoubleAnimation.To = 300;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(0d));
            gridOuter.BeginAnimation(HeightProperty, myDoubleAnimation);

            txtADSearchFor.Text = "";
        }
        #endregion

        #region Commands
        public void Add()
        {
            lblTitle.Text = "Add new resource";
            cbManager.SelectedIndex = -1;
            cbDepartment.SelectedIndex = -1;
            txtName.Text = "";
            txtTitle.Text = "";
            rectManager.Opacity = .2d;
            rectResource.Opacity = 1d;
            rectPM.Opacity = .2d;
            eMode = mode.add;
            ResourceID = "0";
            gridOuter.Height = 300d;
            btnAD.Visibility = System.Windows.Visibility.Visible;
        }

        public void Edit(string resid, string name)
        {
            lblTitle.Text = "Edit " + name;
            txtName.Text = name;

            string title = parent.getkey(parent.dtResources, "ResourceTitle", "ResourceID", resid);
            string mgrid = parent.getkey(parent.dtResources, "ResourceManagerID", "ResourceID", resid);
            string department = parent.getkey(parent.dtResources, "ResourceDepartment", "ResourceID", resid);
            string ismgr = parent.getkey(parent.dtResources, "IsManager", "ResourceID", resid);
            string isres = parent.getkey(parent.dtResources, "IsResource", "ResourceID", resid);
            string ispm = parent.getkey(parent.dtResources, "IsPM", "ResourceID", resid);

            if (mgrid != null)
            {
                string mgrname = parent.getkey(parent.dtResources, "ResourceName", "ResourceID", mgrid);
                parent.setDropdownFromValue(cbManager, "ResourceName", mgrname);
            }

            txtTitle.Text = title;

            parent.setDropdownFromValue(cbDepartment, "DepartmentName", department);

            rectManager.Opacity = (ismgr == "True") ? 1d : .2d;
            rectResource.Opacity = (isres == "True") ? 1d : .2d;
            rectPM.Opacity = (ispm == "True") ? 1d : .2d;
            eMode = mode.edit;
            ResourceID = resid;
            gridOuter.Height = 300d;
            btnAD.Visibility = System.Windows.Visibility.Visible;
        }

        private void Save()
        {
            DataTable dtResourceUpdate = new DataTable("Resource");
            dtResourceUpdate.Columns.Add("ResourceID");
            dtResourceUpdate.Columns.Add("ResourceName");
            dtResourceUpdate.Columns.Add("ResourceTitle");
            dtResourceUpdate.Columns.Add("ResourceManagerID");
            dtResourceUpdate.Columns.Add("ResourceDepartment");
            dtResourceUpdate.Columns.Add("IsManager");
            dtResourceUpdate.Columns.Add("IsResource");
            dtResourceUpdate.Columns.Add("IsPM");
            dtResourceUpdate.Columns.Add("EndDate");
            dtResourceUpdate.Columns.Add("Operation");

            string mgrid = "0";
            if (cbManager.SelectedIndex >= 0)
                mgrid = ((System.Data.DataRowView)cbManager.SelectedItem).Row["ResourceID"].ToString();

            string dept = "";
            if (cbDepartment.SelectedIndex >= 0)
                dept = ((System.Data.DataRowView)cbDepartment.SelectedItem).Row["DepartmentName"].ToString();

            DataRow drRes = dtResourceUpdate.NewRow();
            drRes["ResourceID"] = ResourceID;
            drRes["ResourceName"] = txtName.Text;
            drRes["ResourceTitle"] = txtTitle.Text;
            drRes["ResourceManagerID"] = mgrid;
            drRes["ResourceDepartment"] = dept;
            drRes["IsManager"] = (rectManager.Opacity > 0.5d) ? "1" : "0";
            drRes["IsResource"] = (rectResource.Opacity > 0.5d) ? "1" : "0";
            drRes["IsPM"] = (rectPM.Opacity > 0.5d) ? "1" : "0";
            drRes["EndDate"] = "";
            drRes["Operation"] = (eMode == mode.add) ? "I" : "U";
            dtResourceUpdate.Rows.Add(drRes);

            DataTable dt = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtResourceUpdate, "RP_CRUD_RESOURCE", "OPSCONSOLE").Tables["WS"];
            parent.LoadPeople();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ////// CHECK FOR UNIQUE NAME //////
            if (eMode == mode.add)
            {
                string presumtiveAddee = txtName.Text.Replace(" ","").ToUpper();

                foreach (DataRow dr in parent.dtResources.Rows)
                {
                    string existing = dr["ResourceName"].ToString().Replace(" ","").ToUpper();
                    if (presumtiveAddee == existing)
                    {
                        MessageBox.Show("The user " + txtName.Text + " already exists and cannot be added");
                        return;
                    }
                }
            }

            ////// PRE-REQUISITES //////
            if (txtTitle.Text == "")
            {
                MessageBox.Show("The title must be entered");
                return;
            }

            //if (cbManager.SelectedIndex == -1)
            //{
            //    MessageBox.Show("The manager must be selected");
            //    return;
            //}

            if (cbDepartment.SelectedIndex == -1)
            {
                MessageBox.Show("The department must be selected");
                return;
            }

            Save();
            MessageBox.Show(string.Format("User {0} has been added successfully!", txtName.Text));
            Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnPM_Click(object sender, RoutedEventArgs e)
        {
            rectPM.Opacity = (rectPM.Opacity < 0.5d) ? 1d : 0.2d;
        }

        private void btnManager_Click(object sender, RoutedEventArgs e)
        {
            rectManager.Opacity = (rectManager.Opacity < 0.5d) ? 1d : 0.2d;
        }

        private void btnResource_Click(object sender, RoutedEventArgs e)
        {
            rectResource.Opacity = (rectResource.Opacity < 0.5d) ? 1d : 0.2d;
        }

        private void btnAD_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation myDoubleAnimation = new DoubleAnimation();
            myDoubleAnimation.From = 300;
            myDoubleAnimation.To = 510;
            myDoubleAnimation.Duration = new Duration(TimeSpan.FromSeconds(1.5d));

            gridOuter.BeginAnimation(HeightProperty, myDoubleAnimation);
            btnAD.Visibility = System.Windows.Visibility.Collapsed;

            txtADSearchFor.Focus();
        }

        private void txtADSearchFor_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchFor = txtADSearchFor.Text.ToUpper();

            lbADResults.Items.Clear();

            if (txtADSearchFor.Text == "")
                return;

            foreach (DataRow dr in parent.dtEmployees.Rows)
            {
                if (dr["RealName"].ToString().ToUpper().IndexOf(searchFor) >= 0)
                    lbADResults.Items.Add(dr["RealName"].ToString());
            }
        }

        public void doneLoadingAD()
        {
            lblPleaseWait.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void lbADResults_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbADResults.SelectedIndex < 0)
                return;

            string name = lbADResults.SelectedItem.ToString();

            foreach (DataRow dr in parent.dtEmployees.Rows)
            {
                if (dr["RealName"].ToString() == name)
                {
                    txtTitle.Text = lookupTitle(dr["SamAccountName"].ToString());
                    txtName.Text = name;

                    string supervisor = dr["Supervisor"].ToString();
                    string mgrname = "";
                    int comma = supervisor.IndexOf(",");
                    if (comma > 0)
                        mgrname = supervisor.Substring(comma + 2) + " " + supervisor.Substring(0, comma);
                    else
                        mgrname = supervisor;
                    if (parent.setDropdownFromValue(cbManager, "ResourceName", mgrname) == false)
                    {
                        cbManager.SelectedIndex = -1;
                        // MessageBox.Show("Unfortunately, before this resource can be added, their manager " + mgrname + " must be added first. Sorry!");
                    }

                    string description = dr["Description"].ToString();
                    if (description.ToUpper().IndexOf("BIU") >= 0)
                        parent.setDropdownFromValue(cbDepartment, "DepartmentName", "BIU");
                    else if (description.ToUpper().IndexOf("PMO") >= 0)
                        parent.setDropdownFromValue(cbDepartment, "DepartmentName", "PMO");
                    else
                        parent.setDropdownFromValue(cbDepartment, "DepartmentName", "IT");


                    return;
                }
            }

        }


        private string lookupTitle(string UserName)
        {
            DataTable ReturnTable = new DataTable("UserGroups");
            ReturnTable.Columns.Add("GroupID");

            DirectoryEntry searchRoot = new DirectoryEntry();
            searchRoot.Path = "LDAP://DC=TRG,DC=com";
            searchRoot.AuthenticationType = AuthenticationTypes.Secure;

            DirectorySearcher adgSearcher = new DirectorySearcher();
            DirectorySearcher adSearcher = new DirectorySearcher();
            adSearcher.SearchRoot = searchRoot;
            adgSearcher.SearchRoot = searchRoot;
            adSearcher.Filter = "(samAccountName=" + UserName + ")";
            adSearcher.PropertiesToLoad.Add("memberOf");

            SearchResult samResult = adSearcher.FindOne();

            if (samResult != null)
            {
                DirectoryEntry adAccount = samResult.GetDirectoryEntry();
                foreach (String title in adAccount.Properties["title"])
                {
                    return title;
                }

            }

            return "";
        }
        #endregion
    }
}
