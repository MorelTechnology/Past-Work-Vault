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
    /// Interaction logic for ServiceCatalogGroupEdit.xaml
    /// </summary>
    public partial class ServiceCatalogGroupEdit : UserControl
    {
        string catalogID = "";
        ServiceCatalog parent = null;
        DataTable dtAllIcons;

        public ServiceCatalogGroupEdit()
        {
            InitializeComponent();
        }

        public void Load(DataTable dtGroups, DataTable dtIcons, string id, ServiceCatalog p)
        {
            //loadGroupData();
            catalogID = id;
            dgIcons.ItemsSource = dtIcons.DefaultView;
            parent = p;
            dtAllIcons = dtIcons;

            if (catalogID == "")
            {
                lblTitle.Text = "Add Catalog";
                txtName.Text = "";
                txtButtonText.Text = "";
                txtButtonTitle.Text = "";
                txtPageDescription.Text = "";
                txtLargeAreaDescription.Text = "";
                dgIcons.SelectedIndex = -1;
            }

            foreach (DataRow dr in dtGroups.Rows)
            {
                if (dr["CatalogID"].ToString() == id)
                {
                    txtName.Text = dr["Name"].ToString();
                    txtButtonText.Text = dr["ButtonText"].ToString();
                    txtButtonTitle.Text = dr["ButtonTitle"].ToString();
                    txtPageDescription.Text = dr["PageDescription"].ToString();
                    txtLargeAreaDescription.Text = dr["LargeAreaDescription"].ToString();

                    string icon = dr["ButtonIcon"].ToString();
                    int i=0;
                    dgIcons.SelectedIndex = -1;
                    foreach (DataRow dri in dtIcons.Rows)
                    {
                        if (dri["Name"].ToString() == icon)
                            dgIcons.SelectedIndex = i;
                        i++;
                    }
                }
            }

        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = parent.fixForSQL(txtName.Text);
            string buttonTitle = parent.fixForSQL(txtButtonTitle.Text);
            string buttonText = parent.fixForSQL(txtButtonText.Text);
            string pageDescription = parent.fixForSQL(txtPageDescription.Text);
            string largeAreaDescription = parent.fixForSQL(txtLargeAreaDescription.Text);
            string buttonIcon = "";
            if (dgIcons.SelectedIndex >= 0)
                buttonIcon = ((System.Data.DataRowView)(dgIcons.SelectedItem))["Name"].ToString();

            // Insert
            if (catalogID == "")
            {
                string sql = "Insert into [Catalog] (Name, ButtonTitle, ButtonIcon, ButtonText, PageDescription, LargeAreaDescription) ";
                sql += "VALUES('" + name + "','" + buttonTitle + "','" + buttonIcon + "','" + buttonText + "','" + pageDescription + "','" + largeAreaDescription + "')";

                parent.executeSQL(sql);
            }

            // Update
            else
            {
                string sql = "update [Catalog] set ";
                sql += "Name='" + name + "', ";
                sql += "ButtonTitle='" + buttonTitle + "', ";
                sql += "ButtonText='" + buttonText + "', ";
                sql += "PageDescription='" + pageDescription + "', ";
                sql += "LargeAreaDescription='" + largeAreaDescription + "', ";
                sql += "ButtonIcon ='" + buttonIcon + "' ";
                sql += "where CatalogID=" + catalogID;
                parent.executeSQL(sql);
            }

            parent.loadCatalogData();
            parent.updateCatalog();

            Visibility = System.Windows.Visibility.Hidden;
        }

        private void LoadIcons()
        {

        }
    }
}
