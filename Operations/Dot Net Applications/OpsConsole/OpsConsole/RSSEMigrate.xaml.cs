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
    /// Interaction logic for RSSEMigrate.xaml
    /// </summary>
    public partial class RSSEMigrate : UserControl
    {
        DataSet dsDev;
        DataSet dsTest;
        DataSet dsProd;

        DataSet dsFrom;
        DataSet dsTo;

        ScriptEngine.environemnt fromEnv = ScriptEngine.environemnt.DEV;
        ScriptEngine.environemnt toEnv = ScriptEngine.environemnt.TEST;

        #region Constructor and Initialization
        public RSSEMigrate()
        {
            InitializeComponent();
        }

        public void InitialLoad()
        {
            qaSmoke.Visibility = System.Windows.Visibility.Hidden;
            qa.Visibility = System.Windows.Visibility.Hidden;
            qaRemove.Visibility = System.Windows.Visibility.Hidden;

            Load();
        }
        #endregion

        private void Load()
        {
            LoadRSSEInfo(ScriptEngine.environemnt.DEV, out dsDev);
            LoadRSSEInfo(ScriptEngine.environemnt.TEST, out dsTest);
            LoadRSSEInfo(ScriptEngine.environemnt.PROD, out dsProd);
        }

        private void LoadRSSEInfo(ScriptEngine.environemnt env, out DataSet ds)
        {
            ds = ScriptEngine.script.runScript(env, new DataTable(), "GET_SCRIPTS_INFO", "GENERAL");
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Load();
        }

        private void cbDirection_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbDirection.SelectedIndex < 0)
                return;

            string selection = (string)((ComboBoxItem)cbDirection.SelectedValue).Content;
            setupScreen(selection);
        }

        private void setupScreen(string direction)
        {
            if (direction.StartsWith("Dev"))
            {
                rsseViewFrom.Title = "Dev";
                rsseViewFrom.dataFrom = dsDev;
                rsseViewFrom.dataTo = dsTest;

                rsseViewTo.Title = "Test";
                rsseViewTo.dataFrom = dsTest;
                rsseViewTo.dataTo = null;

                dsFrom = dsDev;
                dsTo = dsTest;

                fromEnv = ScriptEngine.environemnt.DEV;
                toEnv = ScriptEngine.environemnt.TEST;
            }

            else
            {
                rsseViewFrom.Title = "Test";
                rsseViewFrom.dataFrom = dsTest;
                rsseViewFrom.dataTo = dsProd;

                rsseViewTo.Title = "Prod";
                rsseViewTo.dataFrom = dsProd;
                rsseViewTo.dataTo = null;

                dsFrom = dsTest;
                dsTo = dsProd;

                fromEnv = ScriptEngine.environemnt.TEST;
                toEnv = ScriptEngine.environemnt.PROD;
            }
        }


        #region temporary QA stuff - to be removed eventually
        private void xxeeeeeeeee_Click(object sender, RoutedEventArgs e)
        {
            qaSmoke.Visibility = System.Windows.Visibility.Visible;
            qa.Visibility = System.Windows.Visibility.Visible;
        }

        private void createQAApplicationInDev(string appname, string appdesc)
        {
            DataTable dtInsert = dsDev.Tables["Applications"].Clone();
            dtInsert.TableName = "ApplicationInput";
            dtInsert.Columns.Add("Operation");

            dtInsert.ImportRow(dsDev.Tables["Applications"].Rows[0]);
            dtInsert.Rows[0]["ShortName"] = appname;
            dtInsert.Rows[0]["Description"] = appdesc;
            dtInsert.Rows[0]["ApplicationID"] = Guid.NewGuid().ToString();
            dtInsert.Rows[0]["Operation"] = "I";

            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.environemnt.DEV, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
            dsDev = ds;
        }

        private void CreateQAEnvironmentInDev(string appid, string shortname, string connname, string servername, string dbname)
        {
            DataTable dtInsert = dsDev.Tables["ApplicationEnvironments"].Clone();
            dtInsert.TableName = "ApplicationEnvironmentInput";
            dtInsert.Columns.Add("Operation");

            dtInsert.ImportRow(dsDev.Tables["ApplicationEnvironments"].Rows[0]);
            dtInsert.Rows[0]["ShortName"] = shortname;
            dtInsert.Rows[0]["ConnectionName"] = "No connection";
            dtInsert.Rows[0]["Server"] = servername;
            dtInsert.Rows[0]["DBName"] = dbname;
            dtInsert.Rows[0]["ApplicationID"] = appid;
            dtInsert.Rows[0]["EnvironmentID"] = Guid.NewGuid().ToString();
            dtInsert.Rows[0]["Operation"] = "I";

            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.environemnt.DEV, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
            dsDev = ds;
        }

        private void CreateQAScriptInDev(string appid, string shortname)
        {
            DataTable dtInsert = dsDev.Tables["Scripts"].Clone();
            dtInsert.TableName = "ScriptInput";
            dtInsert.Columns.Add("Operation");
            dtInsert.ImportRow(dsDev.Tables["Scripts"].Rows[0]);

            dtInsert.Rows[0]["ShortName"] = shortname;
            dtInsert.Rows[0]["ApplicationID"] = appid;
            dtInsert.Rows[0]["ScriptID"] = Guid.NewGuid().ToString();
            dtInsert.Rows[0]["Operation"] = "I";

            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.environemnt.DEV, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            qaSmoke.Visibility = System.Windows.Visibility.Hidden;
            qa.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            qaSmoke.Visibility = System.Windows.Visibility.Hidden;
            qa.Visibility = System.Windows.Visibility.Hidden;

            foreach (DataRow dr in dsDev.Tables["Applications"].Rows)
                if (dr["ShortName"].ToString() == txtNewAppName.Text)
                {
                    MessageBox.Show("Name already used!");
                    return;
                }

            createQAApplicationInDev(txtNewAppName.Text, txtNewAppDesc.Text);

            string appid = "";
            foreach (DataRow dr in dsDev.Tables["Applications"].Rows)
                if (dr["ShortName"].ToString() == txtNewAppName.Text)
                    appid = dr["ApplicationID"].ToString();

            if (appid != "")
            {
                CreateQAEnvironmentInDev(appid, txtNewEnvShortName.Text, txtNewEnvConnection.Text, txtNewEnvServer.Text, txtNewEnvDBName.Text);
                CreateQAScriptInDev(appid, txtNewScriptName.Text);
            }

            Load();

            if (cbDirection.SelectedIndex < 0)
                return;

            string selection = (string)((ComboBoxItem)cbDirection.SelectedValue).Content;
            setupScreen(selection);

        }

        private void btnCancelRemove_Click(object sender, RoutedEventArgs e)
        {
            qaSmoke.Visibility = System.Windows.Visibility.Hidden;
            qaRemove.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSaveRemove_Click(object sender, RoutedEventArgs e)
        {
            qaSmoke.Visibility = System.Windows.Visibility.Hidden;
            qaRemove.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnRemoveObjects_Click(object sender, RoutedEventArgs e)
        {
            qaSmoke.Visibility = System.Windows.Visibility.Visible;
            qaRemove.Visibility = System.Windows.Visibility.Visible;

            rectRemDev.Opacity = 1d;
            rectRemProd.Opacity = 0.2d;
            fillRemoveDropdown(dsDev);
        }
        #endregion

        #region Migration Plan Generation
        private void createAppMigrationPlan(string appID)
        {
            DataTable updateTable = createUpdateTable();

            compareObjects(updateTable, dsFrom.Tables["Applications"], dsTo.Tables["Applications"], "ApplicationID", appID, new List<string>() { "ApplicationID" }, new List<string>() { "ShortName", "Description" }, "ShortName", "");
            compareObjects(updateTable, dsFrom.Tables["ApplicationEnvironments"], dsTo.Tables["ApplicationEnvironments"], "ApplicationID", appID, new List<string>() { "ApplicationID", "EnvironmentID" }, new List<string>() { "ShortName", "ConnectionName", "Server", "DBName" }, "ShortName", "EnvironmentID");
            
            compareObjects(updateTable, dsFrom.Tables["ApplicationSchemas"], dsTo.Tables["ApplicationSchemas"], "ApplicationID", appID, new List<string>() { "ApplicationID", "SchemaID" }, new List<string>() { }, "SchemaID", "SchemaID");
            compareObjects(updateTable, dsFrom.Tables["ApplicationGroups"], dsTo.Tables["ApplicationGroups"], "ApplicationID", appID, new List<string>() { "ApplicationID", "GroupID" }, new List<string>() { }, "GroupID", "GroupID");
            //findUniqueObjects(updateTable, dsFrom.Tables["ApplicationSchemas"], dsTo.Tables["ApplicationSchemas"], "ApplicationID", appID, new List<string>() { "ApplicationID", "SchemaID" }, new List<string>() { }, "SchemaID", "SchemaID");
            //findUniqueObjects(updateTable, dsFrom.Tables["ApplicationGroups"], dsTo.Tables["ApplicationGroups"], "ApplicationID", appID, new List<string>() { "ApplicationID", "GroupID" }, new List<string>() { }, "GroupID", "GroupID");

            compareObjects(updateTable, dsFrom.Tables["EmailTemplates"], dsTo.Tables["EmailTemplates"], "ApplicationID", appID, new List<string>() { "ApplicationID", "TemplateID" }, new List<string>() { "Description", "TemplateCode", "TemplateBody", "TemplateSubject" }, "TemplateCode", "TemplateID");
            foreach (DataRow drScript in dsFrom.Tables["Scripts"].Rows)
            {
                if (drScript["ApplicationID"].ToString() == appID)  
                {
                    compareObjects(updateTable, dsFrom.Tables["ScriptGroups"], dsTo.Tables["ScriptGroups"], "ScriptID", drScript["ScriptID"].ToString(), new List<string>() { "ScriptID", "GroupID" }, new List<string>() { }, "GroupID", "");
                    //findUniqueObjects(updateTable, dsFrom.Tables["ScriptGroups"], dsTo.Tables["ScriptGroups"], "ScriptID", drScript["ScriptID"].ToString(), new List<string>() { "ScriptID", "GroupID" }, new List<string>() { }, "GroupID", "");
                    compareObjects(updateTable, dsFrom.Tables["Schemas"], dsTo.Tables["Schemas"], "SchemaID", drScript["SchemaID"].ToString(), new List<string>() { "SchemaID" }, new List<string>() { "ShortName", "Description", "Schema" }, "ShortName", "");
                }
            }

            compareObjects(updateTable, dsFrom.Tables["Scripts"], dsTo.Tables["Scripts"], "ApplicationID", appID, new List<string>() { "ApplicationID", "ScriptID" }, new List<string>() { "ShortName", "SchemaID", "XML" }, "ShortName", "ScriptID");

            // 1/27/16 - Show Script Names for Script Groups
            foreach (DataRow dr in updateTable.Rows)
            {
                if (dr["Table"].ToString() == "ScriptGroups")
                {
                    string scriptid = dr["SourceVal"].ToString();
                    foreach (DataRow drs in dsTest.Tables["Scripts"].Rows)
                    {
                        if (drs["ScriptID"].ToString() == scriptid)
                            dr["OtherID"] = drs["ShortName"];
                    }
                }
            }

            rsseViewTo.dataPlan = updateTable;
        }

        private DataTable createUpdateTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Table");
            dt.Columns.Add("Name");
            dt.Columns.Add("SourceCol");
            dt.Columns.Add("SourceVal");
            dt.Columns.Add("OtherIDName");
            dt.Columns.Add("OtherID");
            dt.Columns.Add("Action");
            return dt;
        }

        private void compareObjects(DataTable updateTable, DataTable source, DataTable destination, string sourceColumn, string sourceValue, List<string> destMatchColumns, List<string> comparisonColumns, string desc, string otherName)
        {
            // For each row in the source table
            foreach (DataRow drSource in source.Rows)
            {
                // See if we have a qualifying source record
                if (drSource[sourceColumn].ToString() == sourceValue)
                {
                    bool foundMatchingRow = false;
                    foreach (DataRow drDest in destination.Rows)
                    {
                        // See if each of the comparison fields match
                        bool thisRowMatches = true;
                        foreach (string s in destMatchColumns)
                        {
                            if (drSource[s].ToString() != drDest[s].ToString())
                                thisRowMatches = false;
                        }

                        if (thisRowMatches)
                        {
                            foundMatchingRow = true;
                            bool update = false;
                            // Compare each column if there differences we will update
                            foreach (string cc in comparisonColumns)
                            {
                                if (drSource[cc].ToString() != drDest[cc].ToString())
                                    update = true;
                            }

                            // Update if any of the comparison fields are different
                            if (update)
                                addToUpdateTableIfUnique(updateTable, source.TableName, drSource[desc].ToString(), sourceColumn, sourceValue, otherName, (otherName == "") ? "" : drSource[otherName].ToString(), "Update");
                                //updateTable.Rows.Add(source.TableName, drSource[desc].ToString(), sourceColumn, sourceValue, otherName, (otherName == "") ? "" : drSource[otherName].ToString(), "Update");
                        }
                    }


                    if (!foundMatchingRow)
                    {
                        // Insert
                        addToUpdateTableIfUnique(updateTable, source.TableName, drSource[desc].ToString(), sourceColumn, sourceValue, otherName, (otherName == "") ? "" : drSource[otherName].ToString(), "Insert");
                        //updateTable.Rows.Add(source.TableName, drSource[desc].ToString(), sourceColumn, sourceValue, otherName, (otherName == "") ? "" : drSource[otherName].ToString(), "Insert");
                    }

                }
            }
        }

        private void findUniqueObjects(DataTable updateTable, DataTable source, DataTable destination, string sourceColumn, string sourceValue, List<string> destMatchColumns, List<string> comparisonColumns, string desc, string otherName)
        {
            // For each row in the source table
            foreach (DataRow drSource in source.Rows)
            {
                // See if we have a qualifying source record
                if (drSource[sourceColumn].ToString() == sourceValue)
                {
                    bool foundMatchingRow = false;
                    foreach (DataRow drDest in destination.Rows)
                    {
                        // See if each of the comparison fields match
                        bool foundRow = true;
                        foreach (string s in destMatchColumns)
                        {
                            if (drSource[s].ToString() != drDest[s].ToString())
                                foundRow = false;
                        }
                        if (foundRow == true)
                            foundMatchingRow = true;
                    }

                    if (!foundMatchingRow)
                    {
                        addToUpdateTableIfUnique(updateTable, source.TableName, drSource[desc].ToString(), sourceColumn, sourceValue, otherName, (otherName == "") ? "" : drSource[otherName].ToString(), "Update");
                    }


                    if (!foundMatchingRow)
                    {
                        // Insert
                        addToUpdateTableIfUnique(updateTable, source.TableName, drSource[desc].ToString(), sourceColumn, sourceValue, otherName, (otherName == "") ? "" : drSource[otherName].ToString(), "Insert");
                        //updateTable.Rows.Add(source.TableName, drSource[desc].ToString(), sourceColumn, sourceValue, otherName, (otherName == "") ? "" : drSource[otherName].ToString(), "Insert");
                    }

                }
            }
        }


        private void addToUpdateTableIfUnique(DataTable updateTable,string table, string name, string sourcecol, string sourceval, string otheridname, string otherid, string action)
        {
            foreach (DataRow dr in updateTable.Rows)
                if ((dr["Table"].ToString() == table) &&
                    (dr["Name"].ToString() == name) &&
                    (dr["SourceCol"].ToString() == sourcecol) &&
                    (dr["SourceVal"].ToString() == sourceval) &&
                    (dr["OtherIDName"].ToString() == otheridname) &&
                    (dr["OtherID"].ToString() == otherid) &&
                    (dr["Action"].ToString() == action))
                    return;
            updateTable.Rows.Add(table, name, sourcecol, sourceval, otheridname, otherid, action);
        }
        #endregion

        private void rsseViewFrom_ScriptItemClicked(object sender, RSSEServerView.ScriptItemEventArgs e)
        {
            if (e.si == RSSEServerView.scriptItem.APPLICATION)
                createAppMigrationPlan(e.ID);
        }


        private void btnRemoveFromDev_Click(object sender, RoutedEventArgs e)
        {
            rectRemDev.Opacity = 1d;
            rectRemProd.Opacity = 0.2d;
            fillRemoveDropdown(dsDev);
        }

        private void btnRemoveFromProd_Click(object sender, RoutedEventArgs e)
        {
            rectRemDev.Opacity = 0.2d;
            rectRemProd.Opacity = 1d;
            fillRemoveDropdown(dsTest);
        }

        private void fillRemoveDropdown(DataSet ds)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("ShortName");
            dt.Columns.Add("ApplicationID");

            List<string> applications = new List<string>();


            foreach (DataRow dr in ds.Tables["Applications"].Rows)
                if (!isReservedApplication(dr["ShortName"].ToString()))
                {
                    dt.Rows.Add(dr["ShortName"].ToString(), dr["ApplicationID"].ToString());
                    applications.Add(dr["ShortName"].ToString());
                }

            cbRemoveAppID.ItemsSource = applications;
            //cbRemoveAppID.ItemsSource = dt.DefaultView;
        }

        private bool isReservedApplication(string s)
        {
            if (s == "EXCEPTIONHANDLING") return true;
            if (s == "GENERAL") return true;
            if (s == "IBOR") return true;
            if (s == "NAVISION") return true;
            if (s == "OPSCONSOLE") return true;
            if (s == "ROC_JOBS") return true;
            if (s == "VLP") return true;

            return false;
        }

        #region Migration
        private void btnMigrate_Click(object sender, RoutedEventArgs e)
        {
            if ((rsseViewTo.dataPlan == null) || (rsseViewTo.dataPlan.Rows.Count == 0))
            {
                MessageBox.Show("There is no plan");
                return;
            }

            ////// Before June 4, 2016 //////
            // if (rsseViewTo.dgPlan.SelectedItems.Count == 0)
            //     rsseViewTo.dgPlan.SelectAll();

            ////// June 4, 2016 - You must select each item to migrate //////
            if (rsseViewTo.dgPlan.SelectedItems.Count == 0)
            {
                MessageBox.Show("You must select which items you wish to migrate from the migration plan grid on the right side of the screen.");
                return;
            }

            foreach (DataRowView drv in rsseViewTo.dgPlan.SelectedItems)
            {
                string table = drv.Row["Table"].ToString();
                string name = drv.Row["Name"].ToString();
                string sourceCol = drv.Row["SourceCol"].ToString();
                string sourceVal = drv.Row["SourceVal"].ToString();
                string otherIDName = drv.Row["OtherIDName"].ToString();
                string otherIDVal = drv.Row["OtherID"].ToString();
                string action = drv.Row["Action"].ToString();

                if (table == "Applications")
                    AddApplication(toEnv, table, name, sourceCol, sourceVal, otherIDName, otherIDVal, action);

                if (table == "ApplicationEnvironments")
                    AddApplicationEnvironment(toEnv, table, name, sourceCol, sourceVal, otherIDName, otherIDVal, action);

                if (table == "Scripts")
                    AddScript(toEnv, table, name, sourceCol, sourceVal, otherIDName, otherIDVal, action);

                if (table == "EmailTemplates")
                    AddEmailTemplate(toEnv, table, name, sourceCol, sourceVal, otherIDName, otherIDVal, action);

                if (table == "Schemas")
                    AddSchema(toEnv, table, name, sourceCol, sourceVal, otherIDName, otherIDVal, action);

                if (table == "ScriptGroups")
                    AddScriptGroup(toEnv, table, name, sourceCol, sourceVal, otherIDName, otherIDVal, action);

                // Fixed 9/9/15
                if (table == "ApplicationGroups")
                    AddApplicationGroup(toEnv, table, name, sourceCol, sourceVal, otherIDName, otherIDVal, action);

                // Added 9/10/15
                if (table == "ApplicationSchemas")
                    AddApplicationSchemas(toEnv, table, name, sourceCol, sourceVal, otherIDName, otherIDVal, action);
            }


            Load();
            if (cbDirection.SelectedIndex < 0)
                return;

            string selection = (string)((ComboBoxItem)cbDirection.SelectedValue).Content;
            setupScreen(selection);
            MessageBox.Show("Migration is complete");
        }

        private void AddApplication (ScriptEngine.environemnt env, string table, string name, string sourceCol, string sourceVal, string otherIDName, string otherIDVal, string action)
        {
            try
            {
                DataTable dtInsert = dsDev.Tables["Applications"].Clone();
                dtInsert.TableName = "ApplicationInput";
                dtInsert.Columns.Add("Operation");
                dtInsert.ImportRow(dsDev.Tables["Applications"].Rows[0]);

                foreach (DataRow drApp in dsFrom.Tables["Applications"].Rows)
                {
                    if (drApp["ApplicationID"].ToString() == sourceVal)
                    {
                        dtInsert.Rows[0]["ApplicationID"] = drApp["ApplicationID"].ToString();
                        dtInsert.Rows[0]["ShortName"] = drApp["ShortName"].ToString();
                        dtInsert.Rows[0]["Description"] = drApp["Description"].ToString();
                        dtInsert.Rows[0]["Operation"] = "I";
                        DataSet ds = ScriptEngine.script.runScript(env, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error trying to AddApplication: " + Environment.NewLine + ex.ToString());
            }
        }

        private void AddApplicationEnvironment(ScriptEngine.environemnt env, string table, string name, string sourceCol, string sourceVal, string otherIDName, string otherIDVal, string action)
        {
            DataTable dtInsert = dsDev.Tables["ApplicationEnvironments"].Clone();
            dtInsert.TableName = "ApplicationEnvironmentInput";
            dtInsert.Columns.Add("Operation");
            dtInsert.ImportRow(dsDev.Tables["ApplicationEnvironments"].Rows[0]);

            foreach (DataRow drApp in dsFrom.Tables["ApplicationEnvironments"].Rows)
            {
                if ((drApp["ApplicationID"].ToString() == sourceVal) && (drApp[otherIDName].ToString() == otherIDVal))
                {
                    dtInsert.Rows[0]["ShortName"] = drApp["ShortName"].ToString();
                    dtInsert.Rows[0]["ConnectionName"] = drApp["ConnectionName"].ToString();
                    dtInsert.Rows[0]["Server"] = drApp["Server"].ToString();
                    dtInsert.Rows[0]["DBName"] = drApp["DBName"].ToString();
                    dtInsert.Rows[0]["ApplicationID"] = drApp["ApplicationID"].ToString();
                    dtInsert.Rows[0]["EnvironmentID"] = drApp["EnvironmentID"].ToString();
                    dtInsert.Rows[0]["Operation"] = action.Substring(0, 1);
                    DataSet ds = ScriptEngine.script.runScript(env, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
                }
            }
        }

        private void AddScript(ScriptEngine.environemnt env, string table, string name, string sourceCol, string sourceVal, string otherIDName, string otherIDVal, string action)
        {
            DataTable dtInsert = dsDev.Tables["Scripts"].Clone();
            dtInsert.TableName = "ScriptInput";
            dtInsert.Columns.Add("Operation");
            dtInsert.ImportRow(dsDev.Tables["Scripts"].Rows[0]);

            foreach (DataRow drApp in dsFrom.Tables["Scripts"].Rows)
            {
                if ((drApp["ApplicationID"].ToString() == sourceVal) && (drApp[otherIDName].ToString() == otherIDVal))
                {
                    dtInsert.Rows[0]["ScriptID"] = drApp["ScriptID"].ToString();
                    dtInsert.Rows[0]["ApplicationID"] = drApp["ApplicationID"].ToString();
                    dtInsert.Rows[0]["SchemaID"] = drApp["SchemaID"].ToString();
                    dtInsert.Rows[0]["ShortName"] = drApp["ShortName"].ToString();
                    dtInsert.Rows[0]["XML"] = drApp["XML"].ToString();
                    dtInsert.Rows[0]["Operation"] = action.Substring(0, 1);
                    DataSet ds = ScriptEngine.script.runScript(env, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
                }
            }
        }

        private void AddEmailTemplate(ScriptEngine.environemnt env, string table, string name, string sourceCol, string sourceVal, string otherIDName, string otherIDVal, string action)
        {
            DataTable dtInsert = dsDev.Tables["EmailTemplates"].Clone();
            dtInsert.TableName = "EmailTemplateInput";
            dtInsert.Columns.Add("Operation");
            dtInsert.ImportRow(dsDev.Tables["EmailTemplates"].Rows[0]);

            foreach (DataRow drApp in dsFrom.Tables["EmailTemplates"].Rows)
            {
                if ((drApp["ApplicationID"].ToString() == sourceVal) && (drApp[otherIDName].ToString() == otherIDVal))
                {
                    dtInsert.Rows[0]["ApplicationID"] = drApp["ApplicationID"].ToString();
                    dtInsert.Rows[0]["TemplateID"] = drApp["TemplateID"].ToString();
                    dtInsert.Rows[0]["Description"] = drApp["Description"].ToString();
                    dtInsert.Rows[0]["TemplateCode"] = drApp["TemplateCode"].ToString();
                    dtInsert.Rows[0]["TemplateBody"] = drApp["TemplateBody"].ToString();
                    dtInsert.Rows[0]["TemplateSubject"] = drApp["TemplateSubject"].ToString();
                    dtInsert.Rows[0]["Operation"] = action.Substring(0,1);
                    DataSet ds = ScriptEngine.script.runScript(env, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
                }
            }
        }

        private void AddSchema(ScriptEngine.environemnt env, string table, string name, string sourceCol, string sourceVal, string otherIDName, string otherIDVal, string action)
        {
            DataTable dtInsert = dsDev.Tables["Schemas"].Clone();
            dtInsert.TableName = "SchemaInput";
            dtInsert.Columns.Add("Operation");
            dtInsert.ImportRow(dsDev.Tables["Schemas"].Rows[0]);

            foreach (DataRow drApp in dsFrom.Tables["Schemas"].Rows)
            {
                if (drApp[sourceCol].ToString() == sourceVal)
                {
                    dtInsert.Rows[0]["SchemaID"] = drApp["SchemaID"].ToString();
                    dtInsert.Rows[0]["ShortName"] = drApp["ShortName"].ToString();
                    dtInsert.Rows[0]["Description"] = drApp["Description"].ToString();
                    dtInsert.Rows[0]["Schema"] = drApp["Schema"].ToString();
                    dtInsert.Rows[0]["Operation"] = action.Substring(0, 1);
                    DataSet ds = ScriptEngine.script.runScript(env, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
                }
            }
        }

        private void AddScriptGroup(ScriptEngine.environemnt env, string table, string name, string sourceCol, string sourceVal, string otherIDName, string otherIDVal, string action)
        {
            DataTable dtInsert = dsDev.Tables["ScriptGroups"].Clone();
            dtInsert.TableName = "ScriptGroupInput";
            dtInsert.Columns.Add("Operation");
            dtInsert.ImportRow(dsDev.Tables["ScriptGroups"].Rows[0]);

            foreach (DataRow drApp in dsFrom.Tables["ScriptGroups"].Rows)
            {
                if ((drApp[sourceCol].ToString() == sourceVal) && (drApp["GroupID"].ToString() == name))
                {
                    dtInsert.Rows[0]["ScriptID"] = drApp["ScriptID"].ToString();
                    dtInsert.Rows[0]["GroupID"] = drApp["GroupID"].ToString();
                    dtInsert.Rows[0]["Operation"] = action.Substring(0, 1);
                    DataSet ds = ScriptEngine.script.runScript(env, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
                }
            }
        }

        private void AddApplicationGroup(ScriptEngine.environemnt env, string table, string name, string sourceCol, string sourceVal, string otherIDName, string otherIDVal, string action)
        {
            DataTable dtInsert = dsDev.Tables["ApplicationGroups"].Clone();
            dtInsert.TableName = "ApplicationGroupInput";
            dtInsert.Columns.Add("Operation");
            dtInsert.ImportRow(dsDev.Tables["ApplicationEnvironments"].Rows[0]);

            foreach (DataRow drApp in dsFrom.Tables["ApplicationGroups"].Rows)
            {
                if ((drApp[sourceCol].ToString() == sourceVal) && (drApp["GroupID"].ToString() == name))
                {
                    dtInsert.Rows[0]["ApplicationID"] = drApp["ApplicationID"].ToString();
                    dtInsert.Rows[0]["GroupID"] = drApp["GroupID"].ToString();
                    dtInsert.Rows[0]["Operation"] = action.Substring(0, 1);
                    DataSet ds = ScriptEngine.script.runScript(env, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
                }
            }

        }

        private void AddApplicationSchemas(ScriptEngine.environemnt env, string table, string name, string sourceCol, string sourceVal, string otherIDName, string otherIDVal, string action)
        {
            DataTable dtInsert = dsDev.Tables["ApplicationSchemas"].Clone();
            dtInsert.TableName = "ApplicationSchemaInput";
            dtInsert.Columns.Add("Operation");
            dtInsert.ImportRow(dsDev.Tables["ApplicationSchemas"].Rows[0]);

            foreach (DataRow drApp in dsFrom.Tables["ApplicationSchemas"].Rows)
            {
                if ((drApp[sourceCol].ToString() == sourceVal) && (drApp["SchemaID"].ToString() == name))
                {
                    dtInsert.Rows[0]["ApplicationID"] = drApp["ApplicationID"].ToString();
                    dtInsert.Rows[0]["SchemaID"] = drApp["SchemaID"].ToString();
                    dtInsert.Rows[0]["Operation"] = action.Substring(0, 1);
                    DataSet ds = ScriptEngine.script.runScript(env, dtInsert, "GET_SCRIPTS_INFO", "GENERAL");
                }
            }
        }


        #endregion

    }
}
