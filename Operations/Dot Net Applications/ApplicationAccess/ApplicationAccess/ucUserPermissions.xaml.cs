using System;
using System.Collections.Generic;
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
using System.Data;
using System.Windows.Media.Animation;

namespace ApplicationAccess
{
    /// <summary>
    /// Interaction logic for ucUserPermissions.xaml
    /// </summary>
    public partial class ucUserPermissions : UserControl
    {
        DataTable dtAvailable = new DataTable();
        DataTable dtCurrent = new DataTable();
        DataTable dtOriginalCurrent = new DataTable();
        string applicationID = "";
        string applicationUserID = "";
        string applicationUserName = "";
        string applicationName = "";
        MainWindow ourParent = null;
        bool newUser = false;
        bool terminated = false;

        public ucUserPermissions()
        {
            InitializeComponent();
        }

        public void start(MainWindow parent, DataTable dtAvailable, DataTable dtCurrent, bool newuser, string application, string ApplicationID, string ApplicationUserID, string ApplicationUserName, string adjustedName, bool termd)
        {
            this.dtAvailable = dtAvailable;
            this.dtCurrent = dtCurrent;
            applicationID = ApplicationID;
            applicationUserID = ApplicationUserID;
            applicationUserName = ApplicationUserName;
            applicationName = application;
            this.newUser = newuser;
            this.terminated = termd;

            ourParent = parent;

            if (termd)
            {
                gridPermissions.Visibility =Visibility.Collapsed;
                gridNewUser.Visibility = System.Windows.Visibility.Collapsed;
                gridDates.Visibility = System.Windows.Visibility.Collapsed;
                dpEffective.Visibility = System.Windows.Visibility.Collapsed;
                dpExpiration.SelectedDate = dpExpiration.DisplayDate = DateTime.Today;
                dpExpiration.Visibility = System.Windows.Visibility.Collapsed;
                dpRemovedExpiration.SelectedDate = dpRemovedExpiration.DisplayDate = DateTime.Today;

                gridTicket.Visibility = Visibility.Visible;
                gridTicket.Opacity = 1d;
                btnSave.Content = "Next";
                lblTitle.Text = "Terminate permissions for " + adjustedName + " in all applications";

                lblEDTitle.Visibility = System.Windows.Visibility.Collapsed;
                lblToday.Visibility = System.Windows.Visibility.Collapsed;
                lblAEDTitle.Visibility = System.Windows.Visibility.Collapsed;
                lblNever.Visibility = System.Windows.Visibility.Collapsed;
                btn3Months.Visibility = System.Windows.Visibility.Collapsed;
                btn6Months.Visibility = System.Windows.Visibility.Collapsed;
                btn1Year.Visibility = System.Windows.Visibility.Collapsed;
                btnNever.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                dpExpiration.Visibility = System.Windows.Visibility.Visible;
                dgAvailable.ItemsSource = dtAvailable.DefaultView;
                dgCurrent.ItemsSource = dtCurrent.DefaultView;
                dpEffective.Visibility = System.Windows.Visibility.Visible;
                dpRemovedExpiration.SelectedDate = dpRemovedExpiration.DisplayDate = DateTime.Today;

                gridPermissions.Opacity = 1d;
                gridNewUser.Opacity = 1d;
                gridTicket.Opacity = 1d;

                gridTicket.Visibility = System.Windows.Visibility.Collapsed;

                gridPermissions.Visibility = (newuser) ? Visibility.Collapsed : Visibility.Visible;
                gridNewUser.Visibility = (newuser) ? Visibility.Visible : Visibility.Collapsed;
                lblTitle.Text = (newuser) ? "" : ("Set permissions for " + adjustedName + " in " + applicationName);
                gridDates.Visibility = System.Windows.Visibility.Collapsed;

                btnSave.Content = "Next";

                dtOriginalCurrent = dtCurrent.Copy();

                removeItemsInBfromA(dtAvailable, dtCurrent, "AccessDesc");
                ebPeopleFilter.Text = "";
                dgAssociates.ItemsSource = ourParent.dtUsers.DefaultView;
                ebPeopleFilter.Text = "";

                lblEDTitle.Visibility = System.Windows.Visibility.Visible;
                lblToday.Visibility = System.Windows.Visibility.Visible;
                lblAEDTitle.Visibility = System.Windows.Visibility.Visible;
                lblNever.Visibility = System.Windows.Visibility.Visible;
                btn3Months.Visibility = System.Windows.Visibility.Visible;
                btn6Months.Visibility = System.Windows.Visibility.Visible;
                btn1Year.Visibility = System.Windows.Visibility.Visible;
                btnNever.Visibility = System.Windows.Visibility.Visible;
            }

        }

        public void removeItemsInBfromA(DataTable A, DataTable B, string column)
        {
            foreach (DataRow drb in B.Rows)
            {
                foreach (DataRow dr in A.Rows)
                {
                    if (dr[column].ToString() == drb[column].ToString())
                    {
                        A.Rows.Remove(dr);
                        break;
                    }
                }
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = System.Windows.Visibility.Collapsed;
        }

        private void btnLookupTicket(object sender, RoutedEventArgs e)
        {
            SDTicket.showTicket(tbTicketInfo, txtTicket.Text);
        }

        private void txtTicket_KeyUp(object sender, KeyEventArgs e)
        {

        }

//            dtCurrentPermissions.Columns.Add("AccessID");
  //          dtCurrentPermissions.Columns.Add("AccessDesc");

        private void btnAddPermission_Click(object sender, RoutedEventArgs e)
        {
            List<DataRow> todel = new List<DataRow>();

            foreach (DataRowView drv in dgAvailable.SelectedItems)
            {
                dtCurrent.Rows.Add(drv.Row["AccessID"].ToString(), drv.Row["AccessDesc"].ToString());
                todel.Add(drv.Row);
            }

            foreach (DataRow dr in todel)
                dtAvailable.Rows.Remove(dr);

            dgCurrent.ItemsSource = dtCurrent.DefaultView;
            dgAvailable.ItemsSource = dtAvailable.DefaultView;
        }

        private void btnRemovePermission_Click(object sender, RoutedEventArgs e)
        {
            List<DataRow> todel = new List<DataRow>();

            foreach (DataRowView drv in dgCurrent.SelectedItems)
            {
                dtAvailable.Rows.Add(0,"","","",drv.Row["AccessDesc"].ToString(), Convert.ToInt32(drv.Row["AccessID"].ToString()));
                todel.Add(drv.Row);
            }

            foreach (DataRow dr in todel)
                dtCurrent.Rows.Remove(dr);

            dgCurrent.ItemsSource = dtCurrent.DefaultView;
            dgAvailable.ItemsSource = dtAvailable.DefaultView;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (terminated)
            {
                if (gridDates.Visibility == System.Windows.Visibility.Visible)
                {
                    string rid = "";
                    rid = GetOrCreateRequestID(txtTicket.Text);
                    terminateUserPermission(rid);
                    Visibility = System.Windows.Visibility.Collapsed;
                    MessageBox.Show("User termination completed");
                    return;
                }


                transitionFromTicketToDates();
                return;
            }
            
            if (gridPermissions.Visibility == System.Windows.Visibility.Visible)
            {
                txtTicket.Text = "";
                tbTicketInfo.Inlines.Clear();

                transitionFromPermissionsToTicket();
            }
            else if (gridTicket.Visibility == System.Windows.Visibility.Visible)
            {
                if (txtTicket.Text == "")
                {
                    MessageBox.Show("You must enter a ticket number");
                    return;
                }
                transitionFromTicketToDates();
            }

            else if (gridNewUser.Visibility == System.Windows.Visibility.Visible)
            {
                if (dgAssociates.SelectedIndex < 0)
                    MessageBox.Show("You must select a user");
                else
                {
                    applicationUserID = ((DataRowView)dgAssociates.SelectedValue)["ApplicationUserID"].ToString();
                    applicationUserName = ((DataRowView)dgAssociates.SelectedValue)["ActiveDirectoryUserName"].ToString();

                    transitionFromNewUserToPermissions();
                }
            }
            else if (gridDates.Visibility == System.Windows.Visibility.Visible)
            {
                string rid = "";
                rid = GetOrCreateRequestID(txtTicket.Text);

                saveChangesInUserPermission(rid);
                Visibility = System.Windows.Visibility.Collapsed;
            }
        }


        // ON THURSDAY MAY 4 WE WILL MOVE THIS OUT OF HERE AND INTO A DATA.CS 
        private void saveChangesInUserPermission(string requestid)
        {
            DataTable dtChanges = utApplicationUserAccess();

            ////// Run through dtOriginalCurrent and look for every row that isn't in the right grid, these get marked as Deleted //////
            foreach (DataRow dr in dtOriginalCurrent.Rows)
            {
                if (!userHasPermission(applicationID, applicationUserID, dr["AccessID"].ToString()))
                {
                    DataRow drd = dtChanges.NewRow();
                    drd["ApplicationID"] = Convert.ToInt32(applicationID);
                    drd["ApplicationUserID"] = Convert.ToInt32(applicationUserID);
                    drd["AccessID"] = Convert.ToInt32(dr["AccessID"]);
                    drd["RequestID"] = Convert.ToInt32(requestid);
                    drd["ApplicationUserName"] = applicationUserName;
                    drd["ModifiedDate"] = DateTime.Today.ToString();
                    drd["EffectiveDate"] = DateTime.Today.ToString();
                    drd["EffectiveDate"] = dpEffective.SelectedDate.ToString();
                    drd["CreatedDate"] = DateTime.Today.ToString();
                    // drd["ExpirationDate"] = DateTime.Today.ToString();
                    drd["ExpirationDate"] = dpRemovedExpiration.SelectedDate.ToString();
                    drd["CreatedBy"] = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    drd["CreatedDate"] = DateTime.Today.ToString();
                    drd["ModifiedBy"] = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                    drd["Operation"] = "D";

                    dtChanges.Rows.Add(drd);
                }
            }

            ////// Run through the right grid and add everything that isn't in dtOriginalCurrent as an Insert //////
            foreach (DataRowView drv in dgCurrent.Items)
            {
                    string accessid = drv.Row["AccessID"].ToString();

                    if (!inOriginalPermissions(applicationID, applicationUserID, accessid))
                    {
                        DataRow drd = dtChanges.NewRow();
                        drd["ApplicationID"] = Convert.ToInt32(applicationID);
                        drd["ApplicationUserID"] = Convert.ToInt32(applicationUserID);
                        drd["AccessID"] = Convert.ToInt32(accessid);
                        drd["RequestID"] = Convert.ToInt32(requestid);
                        drd["ApplicationUserName"] = applicationUserName;
                        drd["ModifiedDate"] = DateTime.Today.ToString();
                        drd["EffectiveDate"] = dpEffective.SelectedDate; // aaa DateTime.Today.ToString();
                        drd["ExpirationDate"] = dpExpiration.SelectedDate; //  new DateTime(2999, 12, 31).ToString();
                        drd["CreatedBy"] = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        drd["CreatedDate"] = DateTime.Today.ToString();
                        drd["ModifiedBy"] = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
                        drd["Operation"] = "I";
                        dtChanges.Rows.Add(drd);
                    }
            }

            int a = 4;
            DataTable dswhatever = ourParent.se.runScript(dtChanges, "AA_CRUD_APPLICATION_USER_ACCESS", "OPSCONSOLE");

            ourParent.reloadByApplication();            
        }


        private void terminateUserPermission(string requestid)
        {
            DataTable dtChanges = utApplicationUserAccess();
            DataSet dswhatever = ourParent.se.runScript(ScriptEngine.envCurrent, dtChanges, "AA_CRUD_APPLICATION_TERM_USER_ACCESS", "OPSCONSOLE", false, "@userid", applicationUserID, "@ExpirationDate", dpRemovedExpiration.SelectedDate.ToString(), "@Ticket", requestid);
            ourParent.reloadByApplication();
        }



        private string GetOrCreateRequestID(string ticket)
        {
            DataSet dsresult = ourParent.se.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "AA_CREATE_TICKET", "OPSCONSOLE", false, "@RequestTicketNo", ticket);
            return dsresult.Tables[0].Rows[0]["RequestID"].ToString();
        }

        private DataTable utApplicationUserAccess()
        {
            DataTable dtAUA = new DataTable();
            dtAUA.Columns.Add("ApplicationID");
            dtAUA.Columns.Add("ApplicationUserID");
            dtAUA.Columns.Add("AccessID");
            dtAUA.Columns.Add("RequestID");
            dtAUA.Columns.Add("ApplicationUserName");
            dtAUA.Columns.Add("ModifiedDate");
            dtAUA.Columns.Add("EffectiveDate");
            dtAUA.Columns.Add("ExpirationDate");
            dtAUA.Columns.Add("CreatedBy");
            dtAUA.Columns.Add("CreatedDate");
            dtAUA.Columns.Add("ModifiedBy");
            dtAUA.Columns.Add("Operation");
            dtAUA.TableName = "ApplicationAccessInput";

            ////// Make a dataTable we are going to pass to the rules engine //////
            //DataTable dtAUA = new DataTable();
            //dtAUA.Columns.Add("ApplicationUserAccessID", typeof(Int32));
            //dtAUA.Columns.Add("ApplicationID", typeof(Int32));
            //dtAUA.Columns.Add("ApplicationUserID", typeof(Int32));
            //dtAUA.Columns.Add("AccessID", typeof(Int32));
            //dtAUA.Columns.Add("RequestID", typeof(Int32));
            //dtAUA.Columns.Add("RemovalRequestID", typeof(Int32));
            //dtAUA.Columns.Add("ApplicationUserName");
            //dtAUA.Columns.Add("ModifiedDate", typeof(DateTime));
            //dtAUA.Columns.Add("EffectiveDate", typeof(DateTime));
            //dtAUA.Columns.Add("ExpirationDate", typeof(DateTime));
            //dtAUA.Columns.Add("CreatedBy");
            //dtAUA.Columns.Add("CreatedDate", typeof(DateTime));
            //dtAUA.Columns.Add("ModifiedBy");
            //dtAUA.Columns.Add("Operation");
            //dtAUA.TableName = "ApplicationAccessInput";
            return dtAUA;
        }

        private bool userHasPermission(string appid, string userid, string accessid)
        {
            foreach (DataRowView drv in dgCurrent.Items)
            {
                if ( // (drv.Row["ApplicationID"].ToString() == appid) &&
                    // (drv.Row["ApplicationUserID"].ToString() == userid) &&
                    (drv.Row["AccessID"].ToString() == accessid))
                    return true;
            }

            return false;
        }

        private bool inOriginalPermissions(string appid, string userid, string accessid)
        {
            foreach (DataRow dr in dtOriginalCurrent.Rows)
            {
                if (// (dr["ApplicationID"].ToString() == appid) &&
                    // (dr["ApplicationUserID"].ToString() == userid) &&
                    (dr["AccessID"].ToString() == accessid))
                    return true;
            }

            return false;
        }


        private void transitionFromNewUserToPermissions()
        {
            gridPermissions.Visibility = System.Windows.Visibility.Visible;

            ////// FADE OUT NEW USER //////
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1d;
            da.To = 0d;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.8d));

            ////// FADE IN PERMISSIONS //////
            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 0;
            da2.To = 1d;
            da2.Duration = new Duration(TimeSpan.FromSeconds(1.8d));

            ////// IF YOU DON'T KILL THE ANIMATION, YOU CAN'T SET THE OPACITY PROPERTY //////
            da.Completed += (ss, ee) => { gridNewUser.BeginAnimation(OpacityProperty, null); };
            da2.Completed += (ss, ee) =>
            {
                gridNewUser.Visibility = System.Windows.Visibility.Collapsed;
                gridTicket.Visibility = System.Windows.Visibility.Collapsed;
                gridPermissions.Visibility = System.Windows.Visibility.Visible;
                gridPermissions.BeginAnimation(OpacityProperty, null);
            };

            ////// START ANIMATION //////
            gridNewUser.BeginAnimation(OpacityProperty, da);
            gridPermissions.BeginAnimation(OpacityProperty, da2);
        }


        private void transitionFromPermissionsToTicket()
        {
            gridTicket.Visibility = System.Windows.Visibility.Visible;

            ////// FADE OUT PERMISSIONS //////
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1d;
            da.To = 0d;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.8d));

            ////// FADE IN GET TICKET //////
            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 0;
            da2.To = 1d;
            da2.Duration = new Duration(TimeSpan.FromSeconds(1.8d));

            ////// IF YOU DON'T KILL THE ANIMATION, YOU CAN'T SET THE OPACITY PROPERTY //////
            da.Completed += (ss, ee) =>
            {
                gridPermissions.BeginAnimation(OpacityProperty, null);
            };

            da2.Completed += (ss, ee) =>
            {
                gridNewUser.Visibility = System.Windows.Visibility.Collapsed;
                gridPermissions.Visibility = System.Windows.Visibility.Collapsed;
                gridDates.Visibility = System.Windows.Visibility.Collapsed;

                gridTicket.BeginAnimation(OpacityProperty, null);
            };

            ////// START ANIMATION //////
            gridPermissions.BeginAnimation(OpacityProperty, da);
            gridTicket.BeginAnimation(OpacityProperty, da2);
            btnSave.Content = "Next";
        }

        private void transitionFromTicketToDates()
        {
            gridDates.Visibility = System.Windows.Visibility.Visible;

            ////// FADE OUT TICKET //////
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1d;
            da.To = 0d;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.8d));

            ////// FADE IN DATES //////
            DoubleAnimation da2 = new DoubleAnimation();
            da2.From = 0;
            da2.To = 1d;
            da2.Duration = new Duration(TimeSpan.FromSeconds(1.8d));

            lblToday.Visibility = System.Windows.Visibility.Visible;
            lblNever.Visibility = System.Windows.Visibility.Visible;
            dpEffective.SelectedDate = dpEffective.DisplayDate = DateTime.Today;

            if (terminated)
                dpExpiration.SelectedDate = dpExpiration.DisplayDate = DateTime.Today;
            else
                dpExpiration.SelectedDate = dpExpiration.DisplayDate = new DateTime(2999, 12, 31);


            ////// IF YOU DON'T KILL THE ANIMATION, YOU CAN'T SET THE OPACITY PROPERTY //////
            da.Completed += (ss, ee) =>
            {
                gridTicket.BeginAnimation(OpacityProperty, null);
            };

            da2.Completed += (ss, ee) =>
            {
                gridNewUser.Visibility = System.Windows.Visibility.Collapsed;
                gridPermissions.Visibility = System.Windows.Visibility.Collapsed;
                gridTicket.Visibility = System.Windows.Visibility.Collapsed;

                gridDates.BeginAnimation(OpacityProperty, null);
            };

            ////// START ANIMATION //////
            gridTicket.BeginAnimation(OpacityProperty, da);
            gridDates.BeginAnimation(OpacityProperty, da2);
            btnSave.Content = "Finish";
        }

        private void ebPeopleFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataTable dtFilteredUsers = ourParent.dtUsers.Clone();

            foreach (DataRow dr in ourParent.dtUsers.Rows)
            {
                if (dr["AdjustedName"].ToString().ToUpper().IndexOf(ebPeopleFilter.Text.ToUpper()) >= 0)
                    dtFilteredUsers.ImportRow(dr);
            }
            dgAssociates.ItemsSource = dtFilteredUsers.DefaultView;

        }

        private void dgAssociates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAssociates.SelectedIndex > 0)
            {
                string adjustedName = ((DataRowView)dgAssociates.SelectedValue)["AdjustedName"].ToString();
                lblTitle.Text = "Set permissions for " + adjustedName + " in " + applicationName;

            }
        }

        private void dpEffective_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            lblToday.Visibility = (dpEffective.SelectedDate == DateTime.Today) ? Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            lblNever.Visibility = (dpExpiration.SelectedDate == new DateTime(2999, 12, 31)) ? Visibility.Visible : System.Windows.Visibility.Collapsed;
        }

        private void btnNever_Click(object sender, RoutedEventArgs e)
        {
            dpExpiration.SelectedDate = dpExpiration.DisplayDate = new DateTime(2999, 12, 31);
        }

        private void btn1Year_Click(object sender, RoutedEventArgs e)
        {
            dpExpiration.SelectedDate = dpExpiration.DisplayDate = DateTime.Today.AddYears(1);
        }

        private void btn6Months_Click(object sender, RoutedEventArgs e)
        {
            dpExpiration.SelectedDate = dpExpiration.DisplayDate = DateTime.Today.AddMonths(6);
        }

        private void btn3Months_Click(object sender, RoutedEventArgs e)
        {
            dpExpiration.SelectedDate = dpExpiration.DisplayDate = DateTime.Today.AddMonths(3);
        }


    }
}
