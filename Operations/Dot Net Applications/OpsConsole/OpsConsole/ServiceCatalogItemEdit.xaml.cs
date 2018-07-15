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
    /// Interaction logic for ServiceCatalogItemEdit.xaml
    /// </summary>
    public partial class ServiceCatalogItemEdit : UserControl
    {
        ServiceCatalog parent = null;
        string serviceID = "";

        public ServiceCatalogItemEdit()
        {
            InitializeComponent();
        }

        public void Load(DataTable dtItems, string id, ServiceCatalog p)
        {
            parent = p;

            //loadGroupData();
            //dgIcons.ItemsSource = dtGroups.DefaultView;
            serviceID = id;

            foreach (DataRow dr in dtItems.Rows)
            {
                if (dr["ServiceID"].ToString() == id)
                {
                    txtName.Text = dr["Name"].ToString();
                    txtDescription.Text = dr["Description"].ToString();
                    txtLargeAreaDescription1.Text = dr["LargeAreaDescription"].ToString();
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string name = parent.fixForSQL(txtName.Text);
            string description = parent.fixForSQL(txtDescription.Text);
            string largeAreaDescription = parent.fixForSQL(txtLargeAreaDescription1.Text);
            string sql = "";

            if (serviceID == "")
            {
                sql = "Insert into [Service] (Name, Description, LargeAreaDescription) ";
                sql += "VALUES('" + name + "','" + description + "','" + largeAreaDescription + "')";
            }

            else
            {
                sql = "update [Service] set ";
                sql += "Name='" + name + "', ";
                sql += "Description='" + description + "', ";
                sql += "LargeAreaDescription='" + largeAreaDescription + "' ";
                sql += "where ServiceID=" + serviceID; ;
            }

            parent.executeSQL(sql);
            parent.loadItemData();
            Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Hidden;
        }
    }
}
