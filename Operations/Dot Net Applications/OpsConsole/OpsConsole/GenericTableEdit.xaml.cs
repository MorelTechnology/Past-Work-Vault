using System;
using System.Collections.Generic;
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
    /// Interaction logic for GenericTableEdit.xaml
    /// </summary>
    public partial class GenericTableEdit : UserControl
    {
        DataTable dtTableInfo;
        DataTable dtColumnInfo;
        DataTable dtData;
        DataTable dtUpdate;

        public GenericTableEdit()
        {
            InitializeComponent();
        }

        public void setup(string name)
        {
            // Get the info for editing this table
            if (getTableInfo(name) == false)
            {
                MessageBox.Show("Unable to obtain information for table edit: " + name);
                return;
            }

            lblCaption.Text = tableInfo("ScreenLabel");

            if (tableInfo("KeyLabel") != "")
            {
                cbKeySelector.Visibility = System.Windows.Visibility.Visible;
                lblKeyLegend.Text = tableInfo("KeyLabel");
            }

            else
            {
                cbKeySelector.Visibility = System.Windows.Visibility.Hidden;
                lblKeyLegend.Text = "";
            }

            lblRowLegend.Text = tableInfo("RowLegend");

            // clear out all the columns
            dgData.Columns.Clear();

            // create new columns
            foreach (DataRow drCol in dtColumnInfo.Rows)
            {
                addColumnToDataGrid(dgData, false, drCol["ColumnLabel"].ToString(), drCol["ColumnDBColumn"].ToString(), Convert.ToInt32(drCol["ColumnWidth"].ToString()), false, drCol["ColumnDBColumn"].ToString() + "Color", drCol["ColumnDBColumn"].ToString() + "Foreground");
            }


            getData(null);
            dgData.ItemsSource = dtData.DefaultView;
        }

        #region DataGrid Helper Routines
        private void addColumnToDataGrid(DataGrid dg, bool readOnly, string columnName, string binding, int width, bool rightjustify, string colorcolumn, string foreground)
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

            if (foreground != "")
            {
                item.CellStyle = new Style(typeof(DataGridCell));
                Setter s = new Setter();
                s.Property = DataGridCell.ForegroundProperty;
                s.Value = new Binding(foreground);
                item.CellStyle.Setters.Add(s);
            }

            if (colorcolumn != "")
            {
                //item.CellStyle = new Style(typeof(DataGridCell));
                Setter s = new Setter();
                s.Property = DataGridCell.BackgroundProperty;
                s.Value = new Binding(colorcolumn);
                item.CellStyle.Setters.Add(s);
            }


            if (readOnly)
                item.IsReadOnly = true;

            dg.Columns.Add(item);
        }
        #endregion


        private void getData(DataTable dtUpdate)
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "CRUD_CONFIGURATION", "OPSCONSOLE");
            DataTable dtPresentationGroup = ds.Tables["Report"];

            if (tableInfo("SCRIPT") == "")
                return;

            DataSet dsOriginalData = ScriptEngine.script.runScript(ScriptEngine.envCurrent, dtUpdate, tableInfo("SCRIPT"), tableInfo("APP"));
            if ((dsOriginalData != null) && (dsOriginalData.Tables[0].TableName.ToUpper() == "ERRORTABLE"))
            {
                MessageBox.Show("An error has occurred in the script: " + dsOriginalData.Tables[0].Rows[0][0].ToString());
                return;
            }
            
            DataTable dtOriginalData = dsOriginalData.Tables[tableInfo("PRIMARYRESULTTABLENAME")];

            CreateGridBackingTableFromTable(dtOriginalData);
        }

        private bool isSCDColumn(string colname)
        {
            if ((colname == "StartDate") ||
                (colname == "EndDate") ||
                (colname == "StartUser") ||
                (colname == "EndUser"))
                return true;
            return false;
        }

        private void CreateGridBackingTableFromTable(DataTable dtOriginal)
        {
            dtUpdate = new DataTable();

            dtData = new DataTable();
            dtData = dtOriginal.Copy();
            foreach (DataColumn col in dtOriginal.Columns)
            {
                if (!isSCDColumn(col.ColumnName))
                {
                    if (dataType(col.ColumnName) == "smalldatetime")
                        dtUpdate.Columns.Add(col.ColumnName);
                    else
                        dtUpdate.Columns.Add(col.ColumnName, col.DataType);
                }
                dtData.Columns.Add(col.ColumnName + "Color");
                dtData.Columns.Add(col.ColumnName + "Foreground");
            }
            dtData.Columns.Add("Operation");
            dtUpdate.Columns.Add("Operation");

            foreach (DataRow dr in dtData.Rows)
            {
                foreach (DataColumn col in dtData.Columns)
                {
                    if (col.ColumnName.EndsWith("Color"))
                        dr[col.ColumnName] = "#FFFFFFFF";
                    if (col.ColumnName.EndsWith("Foreground"))
                        dr[col.ColumnName] = "#FF000000";
                }
            }

            foreach (DataRow dr in dtData.Rows)
            {
                if (hasColumn(dtData, "EndDate"))
                    if (dr["EndDate"].ToString() != "")
                        dr.Delete();
            }

        }

        private bool hasColumn(DataTable dt, string col)
        {
            foreach (DataColumn dc in dt.Columns)
                if (dc.ColumnName == col)
                    return true;
            return false;

        }

        private string tableInfo(string col)
        {
            return dtTableInfo.Rows[0][col].ToString().Trim();
        }

        private bool getTableInfo(string tableEditName)
        {
            DataSet ds = ScriptEngine.script.runScript(ScriptEngine.envCurrent, new DataTable(), "CRUD_CONFIGURATION", "OPSCONSOLE");
            DataTable dtAll = ds.Tables["EditTable"];
            DataTable dtEditTableColumn = ds.Tables["EditTableColumn"];

            dtTableInfo = dtAll.Clone();
            foreach (DataRow drTI in dtAll.Rows)
            {
                if (drTI["Name"].ToString() == tableEditName)
                    dtTableInfo.ImportRow(drTI);
            }

            dtColumnInfo = dtEditTableColumn.Clone();
            foreach (DataRow drETC in dtEditTableColumn.Rows)
            {
                if (drETC["EditTableID"].ToString() == tableInfo("ID"))
                    dtColumnInfo.ImportRow(drETC);
            }

            return true;
        }

        private string dataType(string column)
        {
            foreach (DataRow dr in dtColumnInfo.Rows)
            {
                if (dr["ColumnDBColumn"].ToString() == column)
                    return dr["Type"].ToString();
            }
            return "";
        }

        private void dgData_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            string to = ((TextBox)e.EditingElement).Text;
            string colHeading = e.Column.Header.ToString();

            // Find the column from heading
            string dbCol = "";
            foreach (DataRow dr in dtColumnInfo.Rows)
            {
                if (dr["ColumnLabel"].ToString() == colHeading)
                    dbCol = dr["ColumnDBColumn"].ToString();
            }

            ((System.Data.DataRowView)(e.Row.Item)).Row[dbCol + "Color"] = "LightGreen";
                
            if (((System.Data.DataRowView)(e.Row.Item)).Row["Operation"].ToString() != "I")
                ((System.Data.DataRowView)(e.Row.Item)).Row["Operation"] = "U";

            dgData.ItemsSource = dtData.DefaultView;
        }

        private void dgData_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

        }

        private void btnInsertRow_Click(object sender, RoutedEventArgs e)
        {
            dtData.Rows.Add();

            foreach (DataColumn col in dtData.Columns)
                if (col.ColumnName.EndsWith("Color"))
                    dtData.Rows[dtData.Rows.Count - 1][col.ColumnName] = "LightGreen";

            dtData.Rows[dtData.Rows.Count - 1]["Operation"] = "I";
            dgData.ItemsSource = dtData.DefaultView;
        }

        private void btnRemoveRow_Click(object sender, RoutedEventArgs e)
        {
            if (dgData.SelectedCells.Count <= 0)
            {
                MessageBox.Show("You must select a row by clicking on any cell");
                return;
            }

            foreach (DataColumn col in dtData.Columns)
                if (col.ColumnName.EndsWith("Color"))
                    ((System.Data.DataRowView)(dgData.SelectedCells[0].Item)).Row[col.ColumnName] = "#FFFFCCCC";

            ((System.Data.DataRowView)(dgData.SelectedCells[0].Item)).Row["Operation"] = "D";
            dgData.ItemsSource = dtData.DefaultView;
        }

        private void btnCancelEdit_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.ourMainWindow.showMainScreen();
        }

        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            dtUpdate.Rows.Clear();

            foreach (DataRow dr in dtData.Rows)
            {
                if (dr.RowState != DataRowState.Deleted)
                {
                    if (dr["Operation"].ToString() != "")
                    {
                        dtUpdate.Rows.Add();
                        foreach (DataColumn col in dtUpdate.Columns)
                        {
                            if (col.ColumnName.IndexOf("Date") >= 0)
                                dtUpdate.Rows[dtUpdate.Rows.Count - 1][col.ColumnName] = Convert.ToDateTime(dr[col.ColumnName]);
                            else
                                dtUpdate.Rows[dtUpdate.Rows.Count - 1][col.ColumnName] = dr[col.ColumnName];

                            ////// NEW as of 2/19/2016 //////
                            ////// Set ID columns to 0 because the XMLParser used in the RSSEDLL drops empty fields //////
                            if ((col.ColumnName == "ID") && (dtUpdate.Rows[dtUpdate.Rows.Count - 1][col.ColumnName] == System.DBNull.Value))
                            {
                                dtUpdate.Rows[dtUpdate.Rows.Count - 1][col.ColumnName] = "0";
                            }

                        }

                        if (dtUpdate.Columns.Contains("TicketNumber"))
                            dtUpdate.Rows[dtUpdate.Rows.Count - 1]["TicketNumber"] = "000000";
                    }
                }
            }

            //dtOriginalData = MainWindow.ourMainWindow.runScript(null, tableInfo("SCRIPT"), tableInfo("APP"));
            dtUpdate.TableName = tableInfo("InputTableName");

            getData(dtUpdate);
            dgData.ItemsSource = dtData.DefaultView;
            
        }
    }
}
