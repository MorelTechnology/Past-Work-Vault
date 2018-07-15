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
    /// Interaction logic for schedActionTicket.xaml
    /// </summary>
    public partial class schedActionTicket : UserControl
    {
        DataTable dtCategories = new DataTable();
        RootObject sdCategories = new RootObject();

        public schedActionTicket()
        {
            InitializeComponent();
        }


        public void Load()
        {
            DataTable dtCategories = new DataTable();
            dtCategories.Columns.Add("Name");

            sdCategories = SDTicket.readCategorySubcategoryAndItem();

            foreach (var cat in sdCategories.operation.Details)
                dtCategories.Rows.Add(cat.NAME);
            dgCat.ItemsSource = dtCategories.DefaultView;



            /*
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
                rectIT.Opacity = 1d;
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
             */

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


        public string getCategory()
        {
            return (dgCat.SelectedIndex < 0) ? "" : ((System.Data.DataRowView)dgCat.SelectedItem)["Name"].ToString();
        }

        public string getSubcategory()
        {
            return (dgSubCat.SelectedIndex < 0) ? "" : ((System.Data.DataRowView)dgSubCat.SelectedItem)["Name"].ToString();
        }

        public string getItem()
        {
            return (dgItem.SelectedIndex < 0) ? "" : ((System.Data.DataRowView)dgItem.SelectedItem)["Name"].ToString();
        }

        public string getPriority()
        {
            return (cbPriority.SelectedIndex < 0) ? "" : ((ComboBoxItem)cbPriority.SelectedItem).Content.ToString();
        }

        public string getSite()
        {
            return (cbSite.SelectedIndex < 0) ? "" : ((ComboBoxItem)cbSite.SelectedItem).Content.ToString();
        }

        public string getGroup()
        {
            return (cbGroup.SelectedIndex < 0) ? "" : ((ComboBoxItem)cbGroup.SelectedItem).Content.ToString();
        }

        public string getTechnician()
        {
            return "";
        }

        public string getSubject()
        {
            return ebSubject.Text;
        }

        public string getDescription()
        {
            return ebDescription.Text;
        }


        public void setItems(string cat, string sub, string itm, string priority,string site, string group, string subject, string desc, string tech)
        {
            ebSubject.Text = subject;
            ebDescription.Text = desc;

            CommonUI.setDropdownFromValue(cbPriority, priority);
            CommonUI.setDropdownFromValue(cbGroup, group);
            CommonUI.setDropdownFromValue(cbSite, site);

            if (cat != "")
            {
                if (CommonUI.setDataGridToValue(dgCat, cat) == false)
                {
                    // txtMessage.Text = "Category \"" + cat + "\" is not in ServiceDesk";
                }
                else
                {
                    fillSubcategories(cat);
                    if (sub != "")
                    {
                        if (CommonUI.setDataGridToValue(dgSubCat, sub) == false)
                        {
                            // txtMessage.Text = "Subcategory \"" + sub + "\" is not in ServiceDesk under category " + cat;
                        }
                        else
                        {
                            fillItmes(cat, sub);
                            if (itm != "")
                            {
                                if (CommonUI.setDataGridToValue(dgItem, itm) == false)
                                {
                                    // txtMessage.Text = "Item \"" + item + "\" is not in ServiceDesk under category " + cat + " and subcategory " + sub;
                                }
                            }
                        }
                    }
                }
            }



        }

    }
}
