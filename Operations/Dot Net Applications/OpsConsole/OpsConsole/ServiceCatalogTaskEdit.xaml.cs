using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for ServiceCatalogTaskEdit.xaml
    /// </summary>
    public partial class ServiceCatalogTaskEdit : UserControl
    {
        RootObject sdCategories = null;
        string taskID = "";
        string serviceID = "";
        ServiceCatalog parent = null;

        public ServiceCatalogTaskEdit()
        {
            InitializeComponent();
        }

        public void Load(DataTable dtTasks, RootObject sdCat, string id, ServiceCatalog p, string ServiceID)
        {
            txtMessage.Text = "";

            sdCategories = sdCat;
            taskID = id;
            parent = p;
            serviceID = ServiceID;

            DataTable dtCategories = new DataTable();
            dtCategories.Columns.Add("Name");

            foreach (var cat in sdCategories.operation.Details)
                dtCategories.Rows.Add(cat.NAME);
            dgCat.ItemsSource = dtCategories.DefaultView;

            dgSubCat.ItemsSource = null;
            dgItem.ItemsSource = null;
            dgSLA.ItemsSource = parent.dtSLA.DefaultView;

            if (id != "")
            {
                lblTitle.Text = "Edit Task";
                foreach (DataRow dr in dtTasks.Rows)
                {
                    if (dr["TaskID"].ToString() == id)
                    {
                        txtName.Text = dr["Name"].ToString();
                        rectBiz.Opacity = (dr["ForBusinessAssoc"].ToString() == "True") ? 1d : 0.2d;
                        rectOps.Opacity = (dr["ForOperationsAssoc"].ToString() == "True") ? 1d : 0.2d;
                        rectIT.Opacity = (dr["ForITAssoc"].ToString() == "True") ? 1d : 0.2d;

                        string cat = dr["ServiceDeskCategory"].ToString();
                        string sub = dr["ServiceDeskSubCategory"].ToString();
                        string item = dr["ServiceDeskItem"].ToString();

                        txtDescription.Text = dr["Description"].ToString();

                        setDataGridByColumn(dgSLA, "ServiceLevelAgreementID", dr["ServiceLevelAgreementID"].ToString());

                        if (cat != "")
                        {
                            if (setDataGridToValue(dgCat, cat) == false)
                            {
                                txtMessage.Text = "Category \"" + cat + "\" is not in ServiceDesk";
                            }
                            else
                            {
                                fillSubcategories(cat);
                                if (sub != "")
                                {
                                    if (setDataGridToValue(dgSubCat, sub) == false)
                                    {
                                        txtMessage.Text = "Subcategory \"" + sub + "\" is not in ServiceDesk under category " + cat;
                                    }
                                    else
                                    {
                                        fillItmes(cat, sub);
                                        if (item != "")
                                        {
                                            if (setDataGridToValue(dgItem, item) == false)
                                                txtMessage.Text = "Item \"" + item + "\" is not in ServiceDesk under category " + cat + " and subcategory " + sub;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            else
            {
                lblTitle.Text = "Add Task";
                txtName.Text = "";
                txtDescription.Text = "";
                rectBiz.Opacity = 1d;
                rectOps.Opacity = 1d;
                rectIT.Opacity =  1d;
                dgSLA.SelectedIndex = -1;
                dgCat.SelectedIndex = -1;
                dgSubCat.SelectedIndex = -1;
                dgItem.SelectedIndex = -1;
            }


            //loadGroupData();
            //dgIcons.ItemsSource = dtGroups.DefaultView;

            //foreach (DataRow dr in dtGroups.Rows)
            //{
            //    if (dr["ID"].ToString() == id)
            //    {
            //        txtName.Text = dr["Name"].ToString();
            //        txtButtonText.Text = dr["ButtonText"].ToString();
            //        txtButtonTitle.Text = dr["ButtonTitle"].ToString();
            //    }
            //}

        }

        private bool setDataGridToValue(DataGrid dg, string s)
        {
            int i = 0;
            foreach (System.Data.DataRowView dr in dg.ItemsSource)
            {
                if (dr["Name"].ToString() == s)
                {
                    dg.SelectedIndex = i;
                    dg.ScrollIntoView(dg.Items[i]);
                    return true;
                }
                i++;
            }

            return false;
        }

        private bool setDataGridByColumn(DataGrid dg, string col, string val)
        {
            int i = 0;
            foreach (System.Data.DataRowView dr in dg.ItemsSource)
            {
                if (dr[col].ToString() == val)
                {
                    dg.SelectedIndex = i;
                    return true;
                }
                i++;
            }
            return false;
        }

        private void btnIT_Click(object sender, RoutedEventArgs e)
        {
            rectIT.Opacity = (rectIT.Opacity < 1) ? 1 : 0.2;
        }

        private void btnOps_Click(object sender, RoutedEventArgs e)
        {
            rectOps.Opacity = (rectOps.Opacity < 1) ? 1 : 0.2;
        }

        private void btnBiz_Click(object sender, RoutedEventArgs e)
        {
            rectBiz.Opacity = (rectBiz.Opacity < 1) ? 1 : 0.2;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            string cat = "", subcat = "", item = "", sla = "", sql="";

            if (dgCat.SelectedIndex >= 0)
                cat = ((System.Data.DataRowView)(dgCat.SelectedItem))["Name"].ToString();
            if (dgSubCat.SelectedIndex >= 0)
                subcat = ((System.Data.DataRowView)(dgSubCat.SelectedItem))["Name"].ToString();
            if (dgItem.SelectedIndex >= 0)
                item = ((System.Data.DataRowView)(dgItem.SelectedItem))["Name"].ToString();
            if (dgSLA.SelectedIndex >= 0)
                sla = ((System.Data.DataRowView)(dgSLA.SelectedItem))["ServiceLevelAgreementID"].ToString();

            string name = parent.fixForSQL(txtName.Text);
            string forBiz = (rectBiz.Opacity < 1) ? "0" : "1";
            string forOps = (rectOps.Opacity < 1) ? "0" : "1";
            string forIT = (rectIT.Opacity < 1) ? "0" : "1";
            string desc = parent.fixForSQL(txtDescription.Text);

            if (taskID == "")
            {
                sql = "Insert into [Task] (ServiceID, ServiceLevelAgreementID, Name, ForBusinessAssoc, ForOperationsAssoc, ForITAssoc, ServiceDeskCategory, ServiceDeskSubCategory, ServiceDeskItem, Description, Comment) ";
                sql += "VALUES(" + serviceID + "," + sla + ",'" + name + "'," + forBiz + "," + forOps + ","  + forIT + ",'" + cat + "','" + subcat +  "','" + item + "','"  + desc  +  "','')";
            }
            else
            {
                sql = "update [Task] set ";
                sql += "Name='" + name + "', ";
                sql += "Description='" + desc + "', ";
                sql += "ServiceDeskCategory='" + parent.fixForSQL(cat) + "', ";
                sql += "ServiceDeskSubCategory='" + parent.fixForSQL(subcat) + "', ";
                sql += "ServiceDeskItem='" + parent.fixForSQL(item) + "', ";
                if (sla != "")
                    sql += "ServiceLevelAgreementID=" + sla + ", ";
                sql += "[ForBusinessAssoc] = " + forBiz + ", ";
                sql += "[ForOperationsAssoc] = " + forOps + ", ";
                sql += "[ForITAssoc] = " + forIT + " ";
                sql += "where TaskID=" + taskID;
            }

            parent.executeSQL(sql);
            parent.loadTaskData();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Save Task field: " + Environment.NewLine + ex.ToString());
            }

            Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Hidden;
        }

        private void dgCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCat.SelectedIndex < 0)
                return;

            string category = ((System.Data.DataRowView)(dgCat.SelectedItem))["Name"].ToString();

            fillSubcategories(category);
            dgItem.ItemsSource = null;
        }

        private void fillSubcategories(string category)
        {
            DataTable dtSubCategories = new DataTable();
            dtSubCategories.Columns.Add("Name");

            dtSubCategories.Rows.Add("[app name]");
            dtSubCategories.Rows.Add("[item]");
            foreach (var cat in sdCategories.operation.Details)
            {
                if ((cat.NAME == category) && (cat.SUBCATEGORY != null))
                    foreach (var sub in cat.SUBCATEGORY)
                        dtSubCategories.Rows.Add(sub.NAME);
            }

            dgSubCat.ItemsSource = dtSubCategories.DefaultView;
        }

        private void dgSubCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((dgCat.SelectedIndex < 0) || (dgSubCat.SelectedIndex < 0))
                return;

            string category = ((System.Data.DataRowView)(dgCat.SelectedItem))["Name"].ToString();
            string subcategory = ((System.Data.DataRowView)(dgSubCat.SelectedItem))["Name"].ToString();

            fillItmes(category, subcategory);
        }

        private void fillItmes(string category, string subcategory)
        {
            DataTable dtItems = new DataTable();
            dtItems.Columns.Add("Name");

            if (subcategory == "[app name]")
            {
                dtItems.Rows.Add("Admin");
                dtItems.Rows.Add("Break/Bug");
                dtItems.Rows.Add("Enhancement");
                dtItems.Rows.Add("Install/Configure");
                dtItems.Rows.Add("Issue/Problem");
                dtItems.Rows.Add("Process Support");
                dtItems.Rows.Add("Project");
                dtItems.Rows.Add("System Upgrade/Release");
            }

            else if (subcategory == "[item]")
            {
                dtItems.Rows.Add("Availability");
                dtItems.Rows.Add("Blue Screen");
                dtItems.Rows.Add("Configuration");
                dtItems.Rows.Add("CPU");
                dtItems.Rows.Add("Create Server");
                dtItems.Rows.Add("Decommission");
                dtItems.Rows.Add("Disk Space");
                dtItems.Rows.Add("Equipment Move");
                dtItems.Rows.Add("Hard Drive");
                dtItems.Rows.Add("Install");
                dtItems.Rows.Add("Issue");
                dtItems.Rows.Add("Issue/Problem");
                dtItems.Rows.Add("Load Image & Configure Profile on New or Replacement");
                dtItems.Rows.Add("Loaner");
                dtItems.Rows.Add("Maintenance/Patching");
                dtItems.Rows.Add("Memory");
                dtItems.Rows.Add("Not Printing");
                dtItems.Rows.Add("Other");
                dtItems.Rows.Add("Paper Jam");
                dtItems.Rows.Add("Performance");
                dtItems.Rows.Add("Peripherals (Mouse, Keyboard, Monitor)");
                dtItems.Rows.Add("reboot");
                dtItems.Rows.Add("Settings");
                dtItems.Rows.Add("Supplies");
                dtItems.Rows.Add("Will Not Boot");
            }

            else
            {
                foreach (var cat in sdCategories.operation.Details)
                {
                    if (cat.NAME == category)
                        foreach (var sub in cat.SUBCATEGORY)
                            if ((sub.NAME == subcategory) && (sub.ITEM != null))
                                foreach (var item in sub.ITEM)
                                    dtItems.Rows.Add(item.NAME);
                }

            }
            dgItem.ItemsSource = dtItems.DefaultView;
        }


        private void btnShowHideFields_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Feature not implemented yet - coming in phase II");
            return;

            if (gridControl.Width <= 700d)
            {
                DoubleAnimation da = new DoubleAnimation();
                da.From = 657;
                da.To = 1146;
                da.Duration = new Duration(TimeSpan.FromSeconds(0.75d));
                gridControl.BeginAnimation(Grid.WidthProperty, da);
                lblButtonShowFields.Content = "Hide";
                //                gridControl.Width = 1146d;
            }
            else
            {
                DoubleAnimation da = new DoubleAnimation();
                da.From = 1146;
                da.To = 657;
                da.Duration = new Duration(TimeSpan.FromSeconds(0.75d));
                gridControl.BeginAnimation(Grid.WidthProperty, da);
                lblButtonShowFields.Content = "Show";
                //                gridControl.Width = 657d;
            }
        }

        private void btnAddField_Click(object sender, RoutedEventArgs e)
        {
            gridAddField.Height = 0d;
            gridAddField.Visibility = System.Windows.Visibility.Visible;

            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = .1;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.75d));
            gridInnerControl.BeginAnimation(Grid.OpacityProperty, da);

            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 1;
            da2.To = .1;
            da2.Duration = new Duration(TimeSpan.FromSeconds(0.75d));
            gridBottomToolbar.BeginAnimation(Grid.OpacityProperty, da2);

            DoubleAnimation da3 = new DoubleAnimation();
            da3.From = 0;
            da3.To = 438;
            da3.Duration = new Duration(TimeSpan.FromSeconds(0.75d));
            gridAddField.BeginAnimation(Grid.HeightProperty, da3);

            DoubleAnimation da4 = new DoubleAnimation();
            da4.From = .5;
            da4.To = 1;
            da4.Duration = new Duration(TimeSpan.FromSeconds(0.5d));
            gridAddField.BeginAnimation(Grid.OpacityProperty, da4);

            gridInnerControl.IsEnabled = false;
            gridBottomToolbar.IsEnabled = false;
            gridAddField.IsEnabled = true;
        }

        private void btnCancelAddField_Click(object sender, RoutedEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = .1;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.75d));
            gridInnerControl.BeginAnimation(Grid.OpacityProperty, da);

            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = .1;
            da2.To = 1;
            da2.Duration = new Duration(TimeSpan.FromSeconds(0.75d));
            gridBottomToolbar.BeginAnimation(Grid.OpacityProperty, da2);

            DoubleAnimation da3 = new DoubleAnimation();
            da3.From = 438;
            da3.To = 0;
            da3.Duration = new Duration(TimeSpan.FromSeconds(0.75d));
            gridAddField.BeginAnimation(Grid.HeightProperty, da3);

            DoubleAnimation da4 = new DoubleAnimation();
            da4.From = 1;
            da4.To = 0;
            da4.Duration = new Duration(TimeSpan.FromSeconds(1d));
            gridAddField.BeginAnimation(Grid.OpacityProperty, da4);

            gridInnerControl.IsEnabled = true;
            gridBottomToolbar.IsEnabled = true;
            gridAddField.IsEnabled = false;
            //gridAddField.Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSaveAddField_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEditPicklists_Click(object sender, RoutedEventArgs e)
        {

        }

        private void cbBehaviors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }





    }
}
