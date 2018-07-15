using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
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
    #region Service Desk Data Contracts
    [DataContract]
    public class RootObject
    {
        [DataMember]
        public Operation operation { get; set; }
    }

    public class Result
    {
        public string status { get; set; }
        public string message { get; set; }
    }

    [DataContract]
    public class Operation
    {
        [DataMember]
        public string name { get; set; }

        [DataMember]
        public Result result { get; set; }

        [DataMember]
        public List<Detail> Details { get; set; }
    }

    [DataContract]
    public class Detail
    {

        [DataMember]
        public string NAME { get; set; }

        [DataMember]
        public string DESCRIPTION { get; set; }

        [DataMember]
        public List<CSUBCATEGORY> SUBCATEGORY { get; set; }

        [DataMember]
        public List<CITEM> ITEM { get; set; }
    }

    [DataContract]
    public class CSUBCATEGORY
    {
        [DataMember]
        public string NAME { get; set; }

        [DataMember]
        public string DESCRIPTION { get; set; }

        [DataMember]
        public List<CITEM> ITEM { get; set; }
    }

    [DataContract]
    public class CITEM
    {
        [DataMember]
        public string NAME { get; set; }

        [DataMember]
        public string DESCRIPTION { get; set; }
    }
    #endregion


    /// <summary>
    /// Interaction logic for ServiceCatalog.xaml
    /// </summary>
    public partial class ServiceCatalog : UserControl
    {
        public static string connectionString = "Data Source=SQLDEV2012R2;Initial Catalog=ServiceCatalog;Integrated Security=True";
        Dictionary<string, string> groupToID = new Dictionary<string, string>();
        List<spreadSheetData> ssd = new List<spreadSheetData>();
        bool loaded = false;
        DataTable dtGroups = new DataTable();
        DataTable dtItems = new DataTable();
        DataTable dtNotItems = new DataTable(); // Items not in the group
        DataTable dtTasks = new DataTable();
        DataTable dtAllIcons = new DataTable();     // All available icons
        DataTable dtAllServices = new DataTable();  // All Services
        DataTable dtAllTasks = new DataTable();     // All Tasks
        DataTable dtAllCatalogService = new DataTable();
        public DataTable dtSLA = new DataTable();
        RootObject sdCategories;

        public ServiceCatalog()
        {
            InitializeComponent();
        }

        public void Load()
        {
            if (loaded)
                return;

            loadSLA();
            loadCatalogData();
            loadAvailableIcons();
            dgGroups.ItemsSource = dtGroups.DefaultView;
            readCategorySubcategoryAndItem();

            loaded = true;
        }

        private void loadSLA()
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from [ServiceLevelAgreement]", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtSLA = dataSet.Tables[0];
            }
        }

        public void loadCatalogData()
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from [Catalog] order by Name", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtGroups=dataSet.Tables[0];

                dtGroups.Columns.Add("ImageFullPath");
                foreach (DataRow dr in dtGroups.Rows)
                    dr["ImageFullPath"] = "images/" + dr["ButtonIcon"].ToString();
            }
        }

        const string colGroup = "A";
        const string colItem = "B";
        const string colTask = "C";
        const string colForBusiness = "D";
        const string colForOps = "E";
        const string colForIT = "F";
        const string colSDCategory = "G";
        const string colSDSubCategory = "H";
        const string colSDItem = "I";
        const string colDescription = "J";
        const string colSLA = "N";

        public class spreadSheetData
        {
            public string group { get; set; }
            public string item { get; set; }
            public string task { get; set; }
            public string forBusiness { get; set; }
            public string forOps { get; set; }
            public string forIT { get; set; }
            public string SDCategory { get; set; }
            public string SDSubCategory { get; set; }
            public string SDItem { get; set; }
            public string description { get; set; }
            public string SLA { get; set; }
            public string buttonText { get; set; }
            public string itemID { get; set; }
        }

        private void btnImportFromSpreadsheet(object sender, RoutedEventArgs e)
        {
            int startRow = 4;
            int tabProposedST = 2;

            //lblStatus.Text = "";
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.DefaultExt = ".XLSX";
            dlg.Title = "Open Service Catalog Spreadsheet";
            dlg.Filter = "Spreadsheet Files (*.xlsx)|*.xlsx";
            Nullable<bool> result = dlg.ShowDialog();
            if (result == true)
            {
                //lblStatus2.Text = "";
                executeSQL("delete from [Task]");
                executeSQL("delete from [CatalogService]");
                executeSQL("delete from [Service]");
                executeSQL("delete from [Catalog]");

                Microsoft.Office.Interop.Excel.Application xlApp;
                Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
                Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
                object misValue = System.Reflection.Missing.Value;

                // READ WORKSHEET INTO STRUCTURE
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(dlg.FileName, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(tabProposedST);
                while (true)
                {
                    if ((xlWorkSheet.get_Range(colGroup + startRow.ToString()).Value2 == null) && (xlWorkSheet.get_Range(colItem + startRow.ToString()).Value2 == null) && (xlWorkSheet.get_Range(colTask + startRow.ToString()).Value2 == null))
                        break;

                    string rgroup = readCell(xlWorkSheet, startRow, colGroup);
                    string ritem = readCell(xlWorkSheet, startRow, colItem);
                    string rtask = readCell(xlWorkSheet, startRow, colTask);
                    string rdesc = readCell(xlWorkSheet, startRow, colDescription);
                    string rForBiz = readCell(xlWorkSheet, startRow, colForBusiness);
                    string rForOps = readCell(xlWorkSheet, startRow, colForOps);
                    string rForIT = readCell(xlWorkSheet, startRow, colForIT);
                    string rSDCat = readCell(xlWorkSheet, startRow, colSDCategory);
                    string rSDSubCat = readCell(xlWorkSheet, startRow, colSDSubCategory);
                    string rSDItem = readCell(xlWorkSheet, startRow, colSDItem);
                    string slatext = readCell(xlWorkSheet, startRow, colSLA);
                    //string rSLA = "1";

                    string rSLA = lookup(dtSLA, "Name", slatext, "ServiceLevelAgreementID");
                    if (rSLA == "")
                        rSLA = "1";

                    ssd.Add(new spreadSheetData() { group = rgroup, item = ritem, task = rtask, description = rdesc, forBusiness = rForBiz, forIT = rForIT, forOps = rForOps, SDCategory = rSDCat, SDSubCategory = rSDSubCat, SDItem = rSDItem, SLA = rSLA, buttonText = "", itemID = "" });
                    startRow++;
                }
                xlWorkBook.Close(true, misValue, misValue);
                xlApp.Quit();

                // WRITE OUT THE CATALOGS
                foreach (spreadSheetData s in ssd)
                {
                    if (s.group != "")
                    {
                        string icon = "";
                        if (s.group.StartsWith("Accounts")) icon = "key.png";
                        if (s.group.StartsWith("Applications")) icon = "blocks.png";
                        if (s.group.StartsWith("Backup")) icon = "cloud.png";
                        if (s.group.StartsWith("BI")) icon = "graph.png";
                        if (s.group.StartsWith("Building")) icon = "building.png";
                        if (s.group.StartsWith("Code")) icon = "migrate.png";
                        if (s.group.StartsWith("Comm")) icon = "phone.png";
                        if (s.group.StartsWith("Compliance")) icon = "gavel.png";
                        if (s.group.StartsWith("Hardware")) icon = "modem.png";
                        if (s.group.StartsWith("Security")) icon = "shield.png";
                        if (s.group.StartsWith("Servers")) icon = "disk.png";
                        if (s.group.StartsWith("Share")) icon = "sharepoint.png";
                        if (s.group.StartsWith("Supplies")) icon = "pencil.png";
                        if (s.group.StartsWith("Support")) icon = "question.png";
                        if (s.group.StartsWith("Innovation")) icon = "beaker.png";
                        if (s.group.StartsWith("All Other")) icon = "question.png";

                        string sql = "Insert into [Catalog] (Name, ButtonTitle, ButtonIcon, ButtonText, PageDescription, LargeAreaDescription) ";
                        sql += "VALUES('" + s.group + "','" + s.group + "','" + icon + "','" + s.description + "','" + s.description + "','" + s.description + "')";
                        executeSQL(sql);
                    }
                }

                // WRITE OUT THE SERVICES
                List<String> foundServices = new List<string>();
                foreach (spreadSheetData s in ssd)
                {
                    if (s.item != "")
                    {
                        if (foundServices.Contains(s.item) == false)
                        {
                            string sql = "Insert into [Service] (Name, Description, LargeAreaDescription) ";
                            sql += "VALUES('" + s.item + "','" + fixForSQL(s.description) + "','" + fixForSQL(s.description) + "')";
                            executeSQL(sql);
                            foundServices.Add(s.item);
                        }
                    }
                }

                // GET SERVICE ID's
                loadAllServices();

                // WRITE OUT THE TASKS
                string currentService = "";
                foreach (spreadSheetData sd in ssd)
                {
                    if (sd.item != "")
                        currentService = sd.item;

                    if ((sd.task != "") && (sd.task != "Duplicate"))
                    {
                        int slaid = 1;
                        string serviceid = lookup(dtAllServices, "Name", currentService, "ServiceID");

                        string sql = "Insert into [Task] (ServiceID, ServiceLevelAgreementID, Name, ForBusinessAssoc, ForOperationsAssoc, ForITAssoc, ServiceDeskCategory, ServiceDeskSubCategory, ServiceDeskItem, Description, Comment) ";
                        sql += "VALUES(" + serviceid + ",";
                        //sql += slaid.ToString() + ",";
                        sql += sd.SLA.ToString() + ",";
                        sql += "'" + sd.task + "',";
                        sql += ((sd.forBusiness.ToUpper().IndexOf("X") >= 0) ? "1" : "0") + ",";
                        sql += ((sd.forOps.ToUpper().IndexOf("X") >= 0) ? "1" : "0") + ",";
                        sql += ((sd.forIT.ToUpper().IndexOf("X") >= 0) ? "1" : "0") + ",";
                        sql += "'" + sd.SDCategory + "',";
                        sql += "'" + sd.SDSubCategory + "',";
                        sql += "'" + sd.SDItem + "',";
                        sql += "'" + fixForSQL(sd.description) + "','')";

                        executeSQL(sql);
                    }
                }

                // LOAD CATALOG ID's
                loadCatalogData();

                // PLACE SERVICES INTO CATALOGS
                string currentCatalog = "";
                foreach (spreadSheetData s in ssd)
                {
                    if (s.group != "")
                        currentCatalog = s.group;

                    if (s.item != "")
                    {
                        string serviceid = lookup(dtAllServices, "Name", s.item, "ServiceID");
                        string catalogid = lookup(dtGroups, "Name", currentCatalog, "CatalogID");

                        string sql = "Insert into [CatalogService] (CatalogID, ServiceID) ";
                        sql += "VALUES(" + catalogid + "," + serviceid + ")";
                        executeSQL(sql);
                    }
                }

                // SET TEXT FOR CATALOG
                loadAllCatalogService();
                loadAllServices();

                foreach (DataRow dr in dtGroups.Rows)
                {
                    string desc = "";

                    foreach (DataRow drc in dtAllCatalogService.Rows)
                    {
                        if (drc["CatalogID"].ToString() == dr["CatalogID"].ToString())
                        {
                            string serviceName = lookup(dtAllServices, "ServiceID", drc["ServiceID"].ToString(), "Name");
                            if (desc != "")
                                desc += ", ";
                            desc += serviceName;
                        }
                    }

                    int lastComma = desc.LastIndexOf(", ");
                    if (lastComma > 0)
                        desc = desc.Substring(0, lastComma) + " and " + desc.Substring(lastComma + 2);

                    string catalogName = dr["Name"].ToString();

                    string buttontext = "includes " + desc;
                    string pagedesc = "Your source for requests pertaining to " + desc;
                    string largedesc = "<b><font color=\"darkgreen\">" + catalogName + "</font></b><br>This category contains information regarding:<br>" + desc;

                    string sql = "update Catalog ";
                    sql += "set ButtonText='" + fixForSQL(buttontext) + "', ";
                    sql += " PageDescription='" + fixForSQL(pagedesc) + "', ";
                    sql += " LargeAreaDescription='" + fixForSQL(largedesc) + "' ";
                    sql += " where CatalogID=" + dr["CatalogID"].ToString();
                    executeSQL(sql);
                }

                MessageBox.Show("Complete");
                return;
            }
            //lblStatus.Text = "";


        }



        private bool itemIsUnique(string i)
        {
            bool found = false;
            foreach (spreadSheetData sd in ssd)
            {
                if (sd.item == i)
                {
                    if (found)
                        return false;
                    found = true;
                }
            }
            return true;
        }

        public string fixForSQL(string val)
        {
            return val.Replace("'", "''");
        }

        private string readCell(Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet, int row, string col)
        {
            string val = (xlWorkSheet.get_Range(col + row.ToString()).Value2 == null) ? "" : xlWorkSheet.get_Range(col + row.ToString()).Value2.ToString();
            return val;
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


        private void getGroupIDs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select distinct CatalogID,Name from [Catalog]", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    List<String> dbservers = new List<string>();

                    while (reader.Read())
                    {
                        string id =  reader["CatalogID"].ToString();
                        string name =  reader["Name"].ToString();
                        groupToID.Add(name, id);
                    }
                }
            }
        }

        [Obsolete]
        private void getItemIDs()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("select * from [Item]", conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    List<String> dbservers = new List<string>();

                    while (reader.Read())
                    {
                        string itemID = reader["ID"].ToString();
                        string groupID = reader["GroupID"].ToString();
                        string name = reader["Name"].ToString();

                        foreach (spreadSheetData sd in ssd)
                        {
                            if ((groupToID[sd.group] == groupID) && (sd.item == name))
                                sd.itemID = itemID;
                        }
                    }
                }
            }
        }

        private void executeQuery(string sql)
        {


            using (SqlConnection conn = new SqlConnection("Data Source=DEVSQL2;Initial Catalog=ServiceCatalog;Integrated Security=True"))
            {
                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    List<String> dbservers = new List<string>();

                    while (reader.Read())
                    {
                        dbservers.Add(reader["ServerName"].ToString());

                    }
                }

            }
        }


        private string nonull(string sin)
        {
            //sin = sin.Replace("'", "''");
            return (sin == null) ? "" : sin;
        }

        private void dgGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgGroups.SelectedIndex < 0)
                return;

            if (!(e.AddedItems[0] is System.Data.DataRowView))
                return;

            string group = ((System.Data.DataRowView)(e.AddedItems[0])).Row["CatalogID"].ToString();
            loadItemData(group);

            dgItems.ItemsSource = dtItems.DefaultView;
            dgItemsNotInGroup.ItemsSource = dtNotItems.DefaultView;
        }

        public void loadItemData()
        {
            string groupid = "";

            if (dgGroups.SelectedIndex >= 0)
                groupid = ((System.Data.DataRowView)(dgGroups.SelectedItem))["CatalogID"].ToString();
            else
            {
                dgItems.ItemsSource = null;
                dgItemsNotInGroup.ItemsSource = null;
                return;
            }

            loadItemData(groupid);

            dgItems.ItemsSource = dtItems.DefaultView;
            dgItemsNotInGroup.ItemsSource = dtNotItems.DefaultView;
        }

        private void loadItemData(string groupid)
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from [Service] i left join CatalogService gi on gi.ServiceID = i.ServiceID where CatalogID=" + groupid.ToString() + " order by i.Name", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtItems = dataSet.Tables[0];
            }

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("  select * from [Service] s where s.ServiceID not in (select cs.ServiceID from CatalogService cs where cs.CatalogID=" + groupid.ToString() + ") order by Name", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtNotItems = dataSet.Tables[0];
            }
        }

        private void dgItems_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dgItems.SelectedIndex < 0) || !(e.AddedItems[0] is System.Data.DataRowView))
                return;

            dgItemsNotInGroup.SelectedIndex = -1;
            loadTaskData();
        }

        private void dgItemsNotInGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dgItemsNotInGroup.SelectedIndex < 0) || !(e.AddedItems[0] is System.Data.DataRowView))
                return;

            dgItems.SelectedIndex = -1;
            loadTaskData();
        }


        public void loadTaskData()
        {
            string id = "";
            if (dgItems.SelectedIndex >= 0)
                id = ((System.Data.DataRowView)(dgItems.SelectedItem)).Row["ServiceID"].ToString();
            else if (dgItemsNotInGroup.SelectedIndex >= 0)
                id = ((System.Data.DataRowView)(dgItemsNotInGroup.SelectedItem)).Row["ServiceID"].ToString();
            else
                return;

            loadTaskData(id);
            dgTasks.ItemsSource = dtTasks.DefaultView;
        }

        public void loadTaskData(string id)
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from [Task] where ServiceID=" + id.ToString(), connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtTasks = dataSet.Tables[0];
            }
        }

        private void loadAvailableIcons()
        {
            dtAllIcons.Clear();
            dtAllIcons.Columns.Add("Name");
            dtAllIcons.Columns.Add("ImageFullPath");
            dtAllIcons.Rows.Add("beaker.png");
            dtAllIcons.Rows.Add("blocks.png");
            dtAllIcons.Rows.Add("building.png");
            dtAllIcons.Rows.Add("cloud.png");
            dtAllIcons.Rows.Add("disk.png");
            dtAllIcons.Rows.Add("document.png");
            dtAllIcons.Rows.Add("earth.png");
            dtAllIcons.Rows.Add("envelope.png");
            dtAllIcons.Rows.Add("gavel.png");
            dtAllIcons.Rows.Add("graph.png");
            dtAllIcons.Rows.Add("key.png");
            dtAllIcons.Rows.Add("migrate.png");
            dtAllIcons.Rows.Add("modem.png");
            dtAllIcons.Rows.Add("pencil.png");
            dtAllIcons.Rows.Add("people.png");
            dtAllIcons.Rows.Add("phone.png");
            dtAllIcons.Rows.Add("question.png");
            dtAllIcons.Rows.Add("sharepoint.png");
            dtAllIcons.Rows.Add("shield.png");
            dtAllIcons.Rows.Add("wifi.png");
            foreach (DataRow dr in dtAllIcons.Rows)
                dr["ImageFullPath"] = "images/" + dr["Name"].ToString();
        }

        #region Edit Add Remove Category
        private void btnEditGroup_Click(object sender, RoutedEventArgs e)
        {
            if (dgGroups.SelectedIndex < 0)
                return;

            string id = ((System.Data.DataRowView)(dgGroups.SelectedItem))["CatalogID"].ToString();

            screenEditGroup.Visibility = System.Windows.Visibility.Visible;
            screenEditGroup.Load(dtGroups, dtAllIcons, id, this);
        }

        private void btnAddCategory_Click(object sender, RoutedEventArgs e)
        {
            screenEditGroup.Visibility = System.Windows.Visibility.Visible;
            screenEditGroup.Load(dtGroups, dtAllIcons, "", this);
        }

        private void btnRemoveCategory_Click(object sender, RoutedEventArgs e)
        {
            if (dgGroups.SelectedIndex < 0)
                return;

            string id = ((System.Data.DataRowView)(dgGroups.SelectedItem))["CatalogID"].ToString();
            string name = ((System.Data.DataRowView)(dgGroups.SelectedItem))["Name"].ToString();

            if (MessageBox.Show("Are you sure you want to remove the Catalog \"" + name + "\"? The catalog will be permanently removed. The services in this catalog will continue to exist.", "Confirm Remove Category " + name, MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            removeCatalog(id);

            loadCatalogData();
            updateCatalog();
        }

        #endregion

        #region Insert Remove Service to Categroy
        private void btnRemoveItemFromGroup_Click(object sender, RoutedEventArgs e)
        {
            if ((dgGroups.SelectedIndex < 0) || (dgItems.SelectedIndex < 0))
                return;

            string groupid = ((System.Data.DataRowView)(dgGroups.SelectedItem))["CatalogID"].ToString();
            string itemid = ((System.Data.DataRowView)(dgItems.SelectedItem))["ServiceID"].ToString();

            string sql = "delete from CatalogService where CatalogID=" + groupid + " and ServiceID=" + itemid;
            executeSQL(sql);

            loadItemData(groupid);
            dgItems.ItemsSource = dtItems.DefaultView;
            dgItemsNotInGroup.ItemsSource = dtNotItems.DefaultView;
        }

        private void btnAddItemToGroup_Click(object sender, RoutedEventArgs e)
        {
            if ((dgGroups.SelectedIndex < 0) || (dgItemsNotInGroup.SelectedIndex < 0))
                return;

            string groupid = ((System.Data.DataRowView)(dgGroups.SelectedItem))["CatalogID"].ToString();
            string itemid = ((System.Data.DataRowView)(dgItemsNotInGroup.SelectedItem))["ServiceID"].ToString();

            string sql = "insert into CatalogService (CatalogID, ServiceID) VALUES(" + groupid + "," + itemid + ")";
            executeSQL(sql);

            loadItemData(groupid);
            dgItems.ItemsSource = dtItems.DefaultView;
            dgItemsNotInGroup.ItemsSource = dtNotItems.DefaultView;
        }
        #endregion

        #region Edit Add Remove Service
        private void btnEditItem_Click(object sender, RoutedEventArgs e)
        {
            string id = "";

            if (dgItems.SelectedIndex >= 0)
            {
                id = ((System.Data.DataRowView)(dgItems.SelectedItem))["ServiceID"].ToString();
                screenEditItem.Load(dtItems, id, this);
            }
            else if (dgItemsNotInGroup.SelectedIndex >= 0)
            {
                id = ((System.Data.DataRowView)(dgItemsNotInGroup.SelectedItem))["ServiceID"].ToString();
                screenEditItem.Load(dtNotItems, id, this);
            }
            else
                return;

            screenEditItem.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            screenEditItem.Visibility = System.Windows.Visibility.Visible;
            screenEditItem.Load(dtItems, "", this);
        }

        private void btnRemoveService_Click(object sender, RoutedEventArgs e)
        {
            string id = "", name="";
            if (dgItems.SelectedIndex >= 0)
            {
                id = ((System.Data.DataRowView)(dgItems.SelectedItem)).Row["ServiceID"].ToString();
                name = ((System.Data.DataRowView)(dgItems.SelectedItem))["Name"].ToString();
            }
            else if (dgItemsNotInGroup.SelectedIndex >= 0)
            {
                id = ((System.Data.DataRowView)(dgItemsNotInGroup.SelectedItem)).Row["ServiceID"].ToString();
                name = ((System.Data.DataRowView)(dgItemsNotInGroup.SelectedItem))["Name"].ToString();
            }
            else
                return;


            // Find tasks in this service
            loadAllTasks();
            string tasks = "";
            foreach (DataRow dr in dtAllTasks.Rows)
                if (dr["ServiceID"].ToString() == id)
                    tasks += "    " + dr["Name"].ToString() + Environment.NewLine;

            // Find the list of catalogs that contain this service
            loadAllCatalogService();
            string catalogs = "";
            foreach (DataRow dr in dtAllCatalogService.Rows)
                if (dr["ServiceID"].ToString() == id)
                    catalogs += "    " + lookup(dtGroups, "CatalogID", dr["CatalogID"].ToString(), "Name") + Environment.NewLine;

            if (MessageBox.Show("Are you sure you want to remove the Service \"" + name + "\"?" + Environment.NewLine + Environment.NewLine + "This service will be removed from the following catalogs:" + Environment.NewLine + catalogs + Environment.NewLine + "The following tasks contained in this service will also be removed:" + Environment.NewLine + tasks, "Confirm Remove Service " + name, MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            // Remove the tasks and service
            removeService(id);

            // UI bits
            loadItemData();
            dgTasks.ItemsSource = null;
        }
        #endregion

        #region Edit Add Remove Task
        private void btnEditTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedIndex < 0)
                return;

            string id = ((System.Data.DataRowView)(dgTasks.SelectedItem))["TaskID"].ToString();

            screenEditTask.Visibility = System.Windows.Visibility.Visible;
            screenEditTask.Load(dtTasks, sdCategories, id,this, "");
        }

        private void btnAddTask_Click(object sender, RoutedEventArgs e)
        {
            string id = "";
            if (dgItems.SelectedIndex >= 0)
                id = ((System.Data.DataRowView)(dgItems.SelectedItem)).Row["ServiceID"].ToString();
            else if (dgItemsNotInGroup.SelectedIndex >= 0)
                id = ((System.Data.DataRowView)(dgItemsNotInGroup.SelectedItem)).Row["ServiceID"].ToString();
            else
            {
                MessageBox.Show("You must select a service to own this task.");
                return;
            }

            screenEditTask.Visibility = System.Windows.Visibility.Visible;
            screenEditTask.Load(dtTasks, sdCategories, "",this, id);
        }

        private void btnRemoveTask_Click(object sender, RoutedEventArgs e)
        {
            if (dgTasks.SelectedIndex < 0)
                return;

            string id = ((System.Data.DataRowView)(dgTasks.SelectedItem))["TaskID"].ToString();
            string name = ((System.Data.DataRowView)(dgTasks.SelectedItem))["Name"].ToString();

            if (MessageBox.Show("Are you sure you want to remove the Task \"" + name + "\"? The task will be permanently removed.", "Confirm Remove Task " + name, MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes)
                return;

            removeTask(id);
            loadTaskData();
        }
        #endregion

        #region Get Category, Subcategory and Item from ServiceDesk
        // Get Categories, Subcategories and Items from ServiceDesk



        private void readCategorySubcategoryAndItem()
        {
            string json = GetData(@"https://sdpondemand.manageengine.com/api/json/admin/category?scope=sdpodapi&authtoken=ebd6098dbe251cc26eccf68617c8ed49&OPERATION_NAME=GET_CATEGORY", "");

            byte[] byteArray = Encoding.ASCII.GetBytes(json);
            MemoryStream stream = new MemoryStream(byteArray);
            stream.Position = 0;

            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(RootObject));
            sdCategories = (RootObject)serializer.ReadObject(stream);
        }
        // https://sdpondemand.manageengine.com/api/json/admin/category?scope=sdpodapi&authtoken=ebd6098dbe251cc26eccf68617c8ed49&OPERATION_NAME=GET_CATEGORY

        static string GetData(string url, string Parameters)
        {
            try
            {
                WebRequest request = WebRequest.Create(url + Parameters);
                request.Method = "POST";
                string postData = "";
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                WebResponse response = request.GetResponse();
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                return responseFromServer;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException.ToString());
                return "";
            }
        }

        #endregion

        #region ui helper
        public void updateCatalog()
        {
            dgGroups.ItemsSource = dtGroups.DefaultView;
        }
        #endregion

        #region data helper

        private string lookup(DataTable dt, string fromField, string fromVal, string lookupField)
        {
            foreach (DataRow dr in dt.Rows)
                if (dr[fromField].ToString() == fromVal)
                    return dr[lookupField].ToString();
            return "";
        }

        public void loadAllServices()
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from Service", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtAllServices = dataSet.Tables[0];
            }
        }

        public void loadAllTasks()
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from [Task]", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtAllTasks = dataSet.Tables[0];
            }
        }

        public void loadAllCatalogService()
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter("select * from [CatalogService]", connectionString))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtAllCatalogService = dataSet.Tables[0];
            }
        }

        private void removeCatalog(string id)
        {
            // Remove this service from all the catalogs it is in
            executeSQL("delete from [CatalogService] where CatalogID  =" + id);

            // Remove this service from all the catalogs it is in
            executeSQL("delete from [Catalog] where CatalogID  =" + id);

            loadAllCatalogService();
            loadCatalogData();
        }

        private void removeService(string id)
        {
            // Delete all the tasks
            loadAllTasks();
            foreach (DataRow dr in dtAllTasks.Rows)
                if (dr["ServiceID"].ToString() == id)
                    executeSQL("delete from [Task] where TaskId=" + dr["TaskID"].ToString());

            loadAllTasks();

            // Remove this service from all the catalogs it is in
            executeSQL("delete from [CatalogService] where ServiceID  =" + id);

            // Then remove the service
            executeSQL("delete from [Service] where ServiceID  =" + id);
            loadAllCatalogService();
            loadItemData();
        }

        private void removeTask(string id)
        {
            // IMPORTANT - Phase II - Remove TaskField and Field

            // Remove this service from all the catalogs it is in
            executeSQL("delete from [Task] where TaskID  =" + id);

            loadAllTasks();
            loadCatalogData();
        }




        #endregion


        #region Lanuch browser
        private void btnLaunchAsOPS_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"http://rssewebdev:81?USER=OPS");
        }

        private void btnLaunchAsIT_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"http://rssewebdev:81/?USER=IT");
        }

        private void btnLaunchAsBIZ_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(@"http://rssewebdev:81/?USER=BIZ");
        }
        #endregion

    }
}
