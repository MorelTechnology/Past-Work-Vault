using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RSSERuleCopier
{
    /// <summary>
    /// Interaction logic for Tables.xaml
    /// </summary>
    public partial class Tables : UserControl
    {
        public static string connectionString = "Data Source=bidevetl01;Initial Catalog=IAFRAMEWORK;Integrated Security=True";
        DataTable refTables = new DataTable();
        DataTable localTables = new DataTable();
        DataTable dtData = new DataTable();
        string schema = "";
        string process = "";

        public Tables()
        {
            InitializeComponent();
        }

        public void setTargetType(bool local, string process)
        {
            if (local)
            {
                connectionString = "Data Source=bidevetl01;Initial Catalog=IAFRAMEWORK;Integrated Security=True";
            }
            else
            {
                connectionString = "Data Source=bidevetl01;Initial Catalog=TPALOADING;Integrated Security=True";
            }
            this.schema = (local) ? "local" : "ref";
            this.process = process;

            lblTableTitle.Text = (local) ? "Local Tables" : "Reference Tables";
            getAllData();
            populateTableGrid();
            showSchemaOrData(showSchema: true);
            dgData.ItemsSource = null;
            dgSchema.ItemsSource = null;
        }


        private void getAllData()
        {
            if (schema == "local")
                localTables = getData("select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = '" + schema + "' and TABLE_NAME like 'T" + process + "[_]%' order by TABLE_NAME");
            else
                localTables = getData("select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = '" + schema + "' order by TABLE_NAME");
        }

        private DataTable getData(string sql)
        {
            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connectionString))
            {
                DataTable dataTable = new DataTable();
                dataAdapter.Fill(dataTable);
                return dataTable;
            }
        }

        private void populateTableGrid()    
        {
            DataRow[] selectedRefTables = localTables.Select("TABLE_NAME like '%" + match.Text + "%'");
            if (selectedRefTables.Length == 0)
            {
                dgTables.ItemsSource = null;
                return;
            }

            if (schema == "local")
            {
                foreach (DataRow dr in selectedRefTables)
                    dr["TABLE_NAME"] = dr["TABLE_NAME"].ToString().Replace("T" + process + "_", "");
            }

            dgTables.ItemsSource = selectedRefTables.CopyToDataTable().DefaultView;
        }

        private void showSchemaOrData(bool showSchema)
        {
            rectSchema.Opacity = (showSchema) ? 1d : .2d;
            rectData.Opacity = (!showSchema) ? 1d : .2d;
            dgSchema.Visibility = (showSchema) ? Visibility.Visible : Visibility.Collapsed;
            dgData.Visibility = (!showSchema) ? Visibility.Visible : Visibility.Collapsed;
        }

            
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string find = search.Text.ToUpper();

            DataTable dtCopy = dtData.Clone();
            foreach (DataRow dr in dtData.Rows)
            {
                foreach (DataColumn col in dr.Table.Columns)
                {
                    string data = dr[col.ColumnName].ToString();
                    if (data.ToUpper().IndexOf(find) >= 0)
                    {
                        dtCopy.ImportRow(dr);
                        break;
                    }
                }

            }

            dgData.ItemsSource = dtCopy.DefaultView;
        }

        private void btnClearDataSearch_Click(object sender, RoutedEventArgs e)
        {
            search.Text = "";
            fillData();
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            showSchemaOrData(showSchema: false);
        }

        private void btnSchema_Click(object sender, RoutedEventArgs e)
        {
            showSchemaOrData(showSchema: true);
        }

        private void btnClearSearch_Click(object sender, RoutedEventArgs e)
        {
            match.Text = "";
            populateTableGrid();
        }


        private void dgTables_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fillData();
        }

        private void fillData()
        {
            if (dgTables.SelectedIndex < 0)
                return;

            string tableName = ((System.Data.DataRowView)dgTables.SelectedItem)["TABLE_NAME"].ToString();
            string tableSchema = ((System.Data.DataRowView)dgTables.SelectedItem)["TABLE_SCHEMA"].ToString();

            if (schema == "local")
            {
                tableName = "T" + process + "_" + tableName;
            }

            DataTable dtSchema = getData("select* from INFORMATION_SCHEMA.COLUMNS where TABLE_SCHEMA = '" + schema + "' and TABLE_NAME = '" + tableName + "' order by ordinal_position");
            dtSchema.Columns.Add("TypeF");
            dgSchema.ItemsSource = dtSchema.DefaultView;

            dgData.ItemsSource = null;
            dgData.Columns.Clear();

            foreach (DataRow dr in dtSchema.Rows)
            {
                string dtype = dr["DATA_TYPE"].ToString().ToUpper();
                if ((dtype == "CHAR") || (dtype == "VARCHAR") || (dtype == "VARBINARY") || (dtype == "BINARY"))
                    dtype += "(" + dr["CHARACTER_MAXIMUM_LENGTH"].ToString().ToUpper() + ")";

                dr["TypeF"] = dtype;


                string columnName = dr["COLUMN_NAME"].ToString();

                DataGridTextColumn dgc = new DataGridTextColumn();
                dgc.Header = columnName;


                if ((dtype.StartsWith("CHAR")) || (dtype.StartsWith("VARCHAR")))
                {
                    int width = Convert.ToInt32(dr["CHARACTER_MAXIMUM_LENGTH"].ToString()) * 10;
                    if (width > 600)
                        width = 600;
                    if (width < 120)
                        width = 120;
                    dgc.Width = width;
                }
                else
                    dgc.Width = 120;

                dgc.Binding = new Binding(columnName);
                dgData.Columns.Add(dgc);

            }

            dtData = getData("select " + (((bool)cbLimit1000.IsChecked) ? "top 1000" : "") + " * from " + tableSchema + "." + tableName);
            dgData.ItemsSource = dtData.DefaultView;


        }

        private void match_TextChanged(object sender, TextChangedEventArgs e)
        {
            populateTableGrid();
        }
    }
}
