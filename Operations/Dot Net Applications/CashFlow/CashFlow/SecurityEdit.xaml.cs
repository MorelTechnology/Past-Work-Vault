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

namespace CashFlow
{
    /// <summary>
    /// Interaction logic for SecurityEdit.xaml
    /// </summary>
    public partial class SecurityEdit : UserControl
    {
        MainWindow ourParent = null;

        public void setParent(MainWindow parent)
        {
            ourParent = parent;
            ucSelectUser.setOurMainWindow(parent);
            //loadUsers();
            //loadPermissions();
            ourParent.showRadioButtonStatus(btnClaims, new Button[] { btnClaims, btnOGC, btnOther });
            showUsers("Claims");
            updateSubstititutions();
        }

        //private void loadUsers()
        //{
        //    string sql = "";

        //    sql = "SELECT TOP 1000 u.[ActiveDirectoryID],u.[DisplayName],u.[AdjustedName],u.[SamAccountName],u.[EmailAddress],u.[Department],u.[Description],u.[SupervisorID],u.[UnitManagerID],u.[TeamManagerID],u.[ApprovalLimit],u.[StartUser],u.[EndUser],u.[StartDate],u.[EndDate],ISNULL(mgr.AdjustedName,'') as Manager,ISNULL(unit.AdjustedName,'') as UnitManager, case when u.Administrator=0 then '' else 'Yes' end as Administrator	 FROM [CashFlow].[data].[users] u left join [CashFlow].[data].[users] mgr on u.SupervisorID = mgr.ActiveDirectoryID left join [CashFlow].[data].[users] unit on u.UnitManagerID = unit.ActiveDirectoryID order by DisplayName";
        //    dtUsers = ourParent.getData(ourParent.dsn, sql);
        //}

        //private void loadPermissions()
        //{
        //    string sql = "SELECT u.[ActiveDirectoryID],u.[SubActiveDirectoryID], sub.AdjustedName, case when u.Edit=0 then 'No' else 'Yes' end as Edit FROM [CashFlow].[data].[permissions] u left join [CashFlow].[data].[users] sub on u.SubActiveDirectoryID  = sub.ActiveDirectoryID order by sub.DisplayName";
        //    dtPermissions = ourParent.getData(ourParent.dsn, sql);
        //}

        public void updateScreen()
        {
            updateSubstititutions();
            fillCurrentConfiguration();
        }

        private void updateSubstititutions()
        {
            DataTable dtSubs = ourParent.ourData.loadSubstititutions();
            dgSubstitutions.ItemsSource = dtSubs.DefaultView;
        }

        public SecurityEdit()
        {
            InitializeComponent();
        }

        private void btnClaims_Click(object sender, RoutedEventArgs e)
        {
            ourParent.showRadioButtonStatus(btnClaims, new Button[] { btnClaims, btnOGC, btnOther });
            showUsers("Claims");
        }

        private void btnOGC_Click(object sender, RoutedEventArgs e)
        {
            ourParent.showRadioButtonStatus(btnOGC, new Button[] { btnClaims, btnOGC, btnOther });
            showUsers("OGC");
        }

        private void btnOther_Click(object sender, RoutedEventArgs e)
        {
            ourParent.showRadioButtonStatus(btnOther, new Button[] { btnClaims, btnOGC, btnOther });
            showUsers("Operations");
        }


        private void showUsers(string dept)
        {
            dgAssociates.ItemsSource = ourParent.ourData.getUsersForDepartment(dept).DefaultView;
        }



        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
            ourParent.makeWBVisible();
        }

        private void dgAssociates_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgAssociates.SelectedIndex < 0)
                return;

            string adjname = ((DataRowView)dgAssociates.SelectedItem)["AdjustedName"].ToString();
            string adid = ((DataRowView)dgAssociates.SelectedItem)["ActiveDirectoryID"].ToString();
            lblSelectedUser1.Text = adjname + " is";

            //dgPermissions.ItemsSource = ourParent.ourData.getPermissionsForUser(adid).DefaultView;

            updateSubs();
        }

        private void updateSubs()
        {
            if (dgAssociates.SelectedIndex < 0)
                return;

            string adid = ((DataRowView)dgAssociates.SelectedItem)["ActiveDirectoryID"].ToString();

            dgSubstiteFor.ItemsSource = ourParent.ourData.getSubstitutionsForSub(adid).DefaultView;
            dgHandledBy.ItemsSource = ourParent.ourData.getSubstitutionsForUser(adid).DefaultView;
            //dgPermissions.ItemsSource = ourParent.ourData.getPermissionsForUser(adid).DefaultView;
        }

        private void btnModifyPermissions_Click(object sender, RoutedEventArgs e)
        {
            if (dgAssociates.SelectedIndex < 0)
            {
                MessageBox.Show("You must select an Associate to modfiy");
                return;
            }

            string adjname = ((DataRowView)dgAssociates.SelectedItem)["AdjustedName"].ToString();

            editUser.lblTitle.Text = "Modify " + adjname;
            editUser.Visibility = Visibility.Visible;
        }

        private void btnAddSubFor_Click(object sender, RoutedEventArgs e)
        {
            if (dgAssociates.SelectedIndex < 0)
            {
                MessageBox.Show("You must select an associate in the upper grid who will be substituting");
            }

            ucSelectUser.ebFilter.Text = "";
            ucSelectUser.fill();
            ucSelectUser.Visibility = Visibility.Visible;
            ucSelectUser.purpose = "AddSubFor";
        }

        public void completeSubFor()
        {
            string subadid = ((DataRowView)dgAssociates.SelectedItem)["ActiveDirectoryID"].ToString();
            string foradid = ((DataRowView)ucSelectUser.dgFrom.SelectedItem)["ActiveDirectoryID"].ToString();

            string SQL = "insert into [CashFlow].[data].[substitutions] VALUES('" + foradid + "','" + subadid + "')";
            ourParent.executeSQL(ourParent.dsn,SQL);

            updateSubs();
        }

        public void completeSubFor2()
        {
            string subadid = ((DataRowView)addSub.dgFrom.SelectedItem)["ActiveDirectoryID"].ToString();
            string foradid = ((DataRowView)addSub.dgFor.SelectedItem)["ActiveDirectoryID"].ToString();

            string SQL = "insert into [CashFlow].[data].[substitutions] VALUES('" + foradid + "','" + subadid + "')";
            ourParent.executeSQL(ourParent.dsn, SQL);

            updateSubstititutions();
            updateSubs();
        }

        private void btnRemoveSubFor_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubstiteFor.SelectedIndex < 0)
            {
                MessageBox.Show("You must select an associate you are subsitituing for to remove");
                return;
            }

            string subadid = ((DataRowView)dgSubstiteFor.SelectedItem)["SubActiveDirectoryID"].ToString();
            string foradid = ((DataRowView)dgSubstiteFor.SelectedItem)["ActiveDirectoryID"].ToString();

            string SQL = "delete from [CashFlow].[data].[substitutions] where ActiveDirectoryID='" + foradid + "' and SubActiveDirectoryID='" + subadid + "'";
            ourParent.executeSQL(ourParent.dsn, SQL);

            updateSubs();
        }

        private void btnImportActuals_Click(object sender, RoutedEventArgs e)
        {
            importActuals();            
        }

        private void btnImportAssociations_Click(object sender, RoutedEventArgs e)
        {
            importAssociations();
        }

        private void btnImportWMandExposures_Click(object sender, RoutedEventArgs e)
        {
            importWMandExposures();
        }

        private void btnImportAssociateHierarchy_Click(object sender, RoutedEventArgs e)
        {
            importUsers();
        }

        private void importUsers()
        {
            ////// STEP 1 - Get Kim's list //////
            string sql = "SELECT TOP 1000 [AssignedAdjuster],[AssignedManager],[AssignedGroup],[AssignedGroupParent],[AssignedGroupManager],[Department] FROM[OPS_BIU_Brewster].[dbo].[Manager]";
            DataTable dtKimsList = ourParent.getData("Data Source=opsdw;Initial Catalog=OPS_BIU_Brewster;Integrated Security=True", sql);

            ////// STEP 2 - Add new people //////
            foreach (DataRow dr in dtKimsList.Rows)
            {
                userInfo ui = ourParent.ourData.UIfromName(dr["AssignedAdjuster"].ToString());
                if (ui.adid == null)
                {
                    DataRow drUser = ADIDfromName(dr["AssignedAdjuster"].ToString());
                    if (drUser == null)
                        continue;

                    DataRow drUnit = ADIDfromName(dr["AssignedManager"].ToString());
                    DataRow drTeam = ADIDfromName(dr["AssignedGroupManager"].ToString());
                    if ((drUnit == null) || (drTeam == null))
                        continue;

                    string adid = drUser["ActiveDirectoryID"].ToString();
                    string sam = drUser["SAMAccountName"].ToString();

                    string adidUnit = drUnit["ActiveDirectoryID"].ToString();
                    string adidTeam = drTeam["ActiveDirectoryID"].ToString();

                    ////// If this SID (adid) is already in our database, don't try to insert it //////
                    userInfo uidup = ourParent.ourData.UIfromADID(adid);

                    if ((uidup.adid == null) && (adid != "") && (adidUnit != "") && (adidTeam != ""))
                    {
                        string sqlu = "insert into [CashFlow].[data].[Users] ";
                        sqlu += "([ActiveDirectoryID],[DisplayName],[AdjustedName],[SamAccountName],[EmailAddress],[Department],[Description],";
                        sqlu += "[SupervisorID],[UnitManagerID],[TeamManagerID],[Administrator],[ApprovalLimit]) values (";

                        sqlu += "'" + adid + "',";                                                      // ActiveDirectoryID
                        sqlu += "'" + drUser["DisplayName"].ToString().Replace("'", "''") + "',";        // DisplayName
                        sqlu += "'" + drUser["AdjustedName"].ToString().Replace("'", "''") + "',";      // AdjustedName
                        sqlu += "'" + sam.Replace("'", "''") + "',";                                    // SAM
                        sqlu += "'" + drUser["EmailAddress"].ToString().Replace("'", "''") + "',";      // EmailAddress
                        sqlu += "'" + drUser["Department"].ToString().Replace("'", "''") + "',";        // Department
                        sqlu += "'" + drUser["Description"].ToString().Replace("'", "''") + "',";       // Description
                        sqlu += "'" + adidUnit + "',";                                                  // SupervisorID
                        sqlu += "'" + adidUnit + "',";                                                  // UnitManagrID
                        sqlu += "'" + adidTeam + "',";                                                  // TeamManagrID
                        sqlu += "0,0)";
                        ourParent.executeSQL(ourParent.dsn, sqlu);
                    }
                }
            }

            ourParent.ourData.loadUsers();

            foreach (DataRow dr in dtKimsList.Rows)
            {
                userInfo ui = ourParent.ourData.UIfromName(dr["AssignedAdjuster"].ToString());
                if (ui.adid != null)
                {
                    userInfo umui = ourParent.ourData.UIfromName(dr["AssignedManager"].ToString());
                    userInfo umti = ourParent.ourData.UIfromName(dr["AssignedGroupManager"].ToString());

                    ////// STEP 3 - Set unit manager if we have a match //////
                    if (umui.adid != "")
                    {
                        string sqlu = "update [CashFlow].[data].[Users] set UnitManagerID='" + umui.adid + "' where ActiveDirectoryID='" + ui.adid + "'";
                        ourParent.executeSQL(ourParent.dsn, sqlu);
                    }

                    ////// STEP 4 - Set team manager if we have a match //////
                    if (umti.adid != "")
                    {
                        string sqlu = "update [CashFlow].[data].[Users] set TeamManagerID='" + umti.adid + "' where ActiveDirectoryID='" + ui.adid + "'";
                        ourParent.executeSQL(ourParent.dsn, sqlu);
                        int a = 4;
                    }

                }

            }
        }

        private DataRow ADIDfromName(string name)
        {
            try
            {
                string sql = "SELECT * FROM [ServiceDesk].[dbo].[Associate] where AdjustedName = '" + name.Replace("'", "''") + "' and EndDate is null";
                DataTable dtUser = ourParent.getData("Data Source=SQLDEV2012R2;Initial Catalog=ServiceDesk;Integrated Security=True", sql);
                if (dtUser.Rows.Count < 1)
                {
                    // MessageBox.Show("Cannot find AD info for " + name);
                    return null;
                }
                return dtUser.Rows[0];
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to access database SQLDEV2012R2.ServiceDesk");
                return null;
            }
        }

        private string SAMfromName(string name)
        {
            try
            {
                string sql = "SELECT * FROM [ServiceDesk].[dbo].[Associate] where AdjustedName = '" + name.Replace("'","''") + "' and EndDate is null";
                DataTable dtUser = ourParent.getData("Data Source=SQLDEV2012R2;Initial Catalog=ServiceDesk;Integrated Security=True", sql);
                if (dtUser.Rows.Count < 1)
                {
                    MessageBox.Show("Cannot find SAMAccountName for " + name);
                    return "";
                }
                return dtUser.Rows[0]["SamAccountName"].ToString().ToLower();
            }
            catch (Exception)
            {
                MessageBox.Show("Unable to access database SQLDEV2012R2.ServiceDesk");
                return "";
            }
        }


        private void importAssociations()
        {
            ////// Dump old Associations //////
            string SQL = "truncate table [CashFlow].[data].[Associations]";
            ourParent.executeSQL(ourParent.dsn, SQL);

            ////// Get new Associations //////
            string sql = "SELECT cia.ClaimAssociationID as AssociationID,ci.ClaimNumber as WorkMatter ";
            sql += "FROM dbo.cc_claiminassoc AS cia ";
            sql += "LEFT JOIN dbo.cc_claiminfo AS ci ON ci.ID = cia.ClaimInfoID ";
            sql += "ORDER BY cia.ClaimAssociationID, ci.ClaimNumber";
            DataTable dtAssociations = ourParent.getData("Data Source=opsdw;Initial Catalog=PROD_ClaimCenter;Integrated Security=True", sql);

            foreach (DataRow dr in dtAssociations.Rows)
            {
                sql = "Insert into [CashFlow].[data].[Associations] (AssociationID, WorkMatter) VALUES (";
                sql += dr["AssociationID"].ToString() + ",";
                sql += "'" + dr["WorkMatter"].ToString() + "')";
                ourParent.executeSQL(ourParent.dsn, sql);
            }

            MessageBox.Show("Import of Associations is complete");
        }

        private void importActuals()
        {
            if ((cbActualsQuarter.SelectedIndex < 0) || (cbActualsYear.SelectedIndex < 0))
            {
                MessageBox.Show("You must select a year and quarter");
                return;
            }

            int qtr = Convert.ToInt32(cbActualsQuarter.SelectedItem.ToString());
            int year = Convert.ToInt32(cbActualsYear.SelectedItem.ToString());

            DataTable dtMonth1 = getActualsForMonth(valuationDateForMonth((qtr - 1) * 3 + 1, year));
            DataTable dtMonth2 = getActualsForMonth(valuationDateForMonth((qtr - 1) * 3 + 2, year));
            DataTable dtMonth3 = getActualsForMonth(valuationDateForMonth((qtr - 1) * 3 + 3, year));

            foreach (DataRow dr in dtMonth1.Rows)
            {
                if (dr["ExpID"].ToString() == "CC000006858-0001")
                {
                    int a = 4;
                }
            }

            foreach (DataRow dr in dtMonth2.Rows)
            {
                if (dr["ExpID"].ToString() == "CC000006858-0001")
                {
                    int a = 4;
                }
            }

            foreach (DataRow dr in dtMonth3.Rows)
            {
                if (dr["ExpID"].ToString() == "CC000006858-0001")
                {
                    int a = 4;
                }
            }


            DataTable dtSum = dtMonth1.Copy();

            sumInto(dtSum, dtMonth2);
            sumInto(dtSum, dtMonth3);

            ////// Dump old actual data //////
            string SQL = "delete from [CashFlow].[data].[Actuals] where Year=" + year.ToString() + " and Quarter=" + qtr.ToString();
            ourParent.executeSQL(ourParent.dsn, SQL);


            // NOTE: Change this to sprocs when we have time
            foreach (DataRow dr in dtSum.Rows)
            {
                string sql = "Insert into [CashFlow].[data].[Actuals] (ExpID, Exposure_ID, Year, Quarter, Total_Paid_Losses, Paid_DJ_Exp, Paid_NonDJ_Exp_Within_Limits, Paid_NonDJ_Exp_Outside_Limits) VALUES (";
                sql += "'" + dr["ExpID"].ToString() + "',";
                sql += dr["Exposure_ID"].ToString() + ",";
                sql += year.ToString() + ",";
                sql += qtr.ToString() + ",";
                sql += dr["Total_Paid_Losses"].ToString() + ",";
                sql += dr["Paid_DJ_Exp"].ToString() + ",";
                sql += dr["Paid_NonDJ_Exp_Within_Limits"].ToString() + ",";
                sql += dr["Paid_NonDJ_Exp_Outside_Limits"].ToString() + ")";
                ourParent.executeSQL(ourParent.dsn, sql);
            }

            MessageBox.Show("Import of " + year.ToString() + "Q" + qtr.ToString() + " complete");
        }

        private void sumInto(DataTable dt1, DataTable dt2)
        {
            foreach (DataRow dr2 in dt2.Rows)
            {
                string exposure = dr2["ExpID"].ToString();
                bool found = false;

                foreach (DataRow dr1 in dt1.Rows)
                {
                    if (dr1["ExpID"].ToString() == exposure)
                    {
                        dr1["Total_Paid_Losses"] = Convert.ToDouble(dr1["Total_Paid_Losses"].ToString()) + Convert.ToDouble(dr2["Total_Paid_Losses"].ToString());
                        dr1["Paid_DJ_Exp"] = Convert.ToDouble(dr1["Paid_DJ_Exp"].ToString()) + Convert.ToDouble(dr2["Paid_DJ_Exp"].ToString());
                        // dr1["Paid_NonDJ_Exp"] = Convert.ToDouble(dr1["Paid_NonDJ_Exp"].ToString()) + Convert.ToDouble(dr2["Paid_NonDJ_Exp"].ToString());
                        dr1["Paid_NonDJ_Exp_Within_Limits"] = Convert.ToDouble(dr1["Paid_NonDJ_Exp_Within_Limits"].ToString()) + Convert.ToDouble(dr2["Paid_NonDJ_Exp_Within_Limits"].ToString());
                        dr1["Paid_NonDJ_Exp_Outside_Limits"] = Convert.ToDouble(dr1["Paid_NonDJ_Exp_Outside_Limits"].ToString()) + Convert.ToDouble(dr2["Paid_NonDJ_Exp_Outside_Limits"].ToString());
                        found = true;
                    }
                }

                if (!found)
                {
                    // dt1.Rows.Add(dr2["Exposure_ID"], dr2["Total_Paid_Losses"], dr2["Paid_DJ_Exp"], dr2["Paid_NonDJ_Exp"], dr2["ExpID"]);
                    dt1.Rows.Add(dr2["Exposure_ID"], dr2["Total_Paid_Losses"], dr2["Paid_DJ_Exp"], dr2["Paid_NonDJ_Exp_Within_Limits"], dr2["Paid_NonDJ_Exp_Outside_Limits"], dr2["ExpID"]);
                }

            }
        }

        private string valuationDateForMonth(int month, int year)
        {
            return new DateTime(year, month, DateTime.DaysInMonth(year, month)).ToString("MM/dd/yyyy");
        }

        private DataTable getActualsForMonth(string month)
        {
            string sql = "";

            sql += "SELECT ft.Exposure_ID ";
            sql += ", SUM(COALESCE(Paid_Loss, 0) + COALESCE(Recovery_Salavage, 0) + COALESCE(Recovery_Subrogation_Loss, 0) ) AS Total_Paid_Losses ";
            sql += ", SUM(COALESCE(Paid_Coverage_DJ_Expense, 0)) AS Paid_DJ_Exp ";
            sql += ", SUM(COALESCE(Paid_Adjusting_Expense_InLimits, 0)) AS Paid_NonDJ_Exp_Within_Limits ";
            sql += ", SUM(COALESCE(Paid_Adjusting_Expense, 0) + COALESCE(Recovery_Subrogation_Expense, 0)) AS Paid_NonDJ_Exp_Outside_Limits ";
            sql += "INTO #Paids  ";
            sql += "FROM OPSDW.Prod_RS_ODS.trn.Financial_Transaction AS ft ";
            sql += "WHERE Valuation_Dt = '" + month + "' ";
            sql += "GROUP BY ft.Exposure_ID ";

            sql += "SELECT    Exposure_Id ";
            sql += ", SUM(COALESCE(Total_Paid_Losses, 0)) Total_Paid_Losses ";
            sql += ", SUM(COALESCE(Paid_DJ_Exp, 0)) Paid_DJ_Exp ";
            sql += ", SUM(COALESCE(Paid_NonDJ_Exp_Within_Limits, 0)) Paid_NonDJ_Exp_Within_Limits ";
            sql += ", SUM(COALESCE(Paid_NonDJ_Exp_Outside_Limits, 0)) Paid_NonDJ_Exp_Outside_Limits ";
            sql += "INTO #Paids2  ";
            sql += "FROM #Paids AS p ";
            sql += "GROUP BY  Exposure_Id ";

            sql += "SELECT p.Exposure_ID, p.Total_Paid_Losses, p.Paid_DJ_Exp, p.Paid_NonDJ_Exp_Within_Limits, p.Paid_NonDJ_Exp_Outside_Limits, ";
            sql += "RTRIM(c.Claim_No) + '-' + e.Exposure_No as ExpID ";
            sql += "from #Paids2 p  ";
            sql += "left join clm.Exposure e on (p.Exposure_ID = e.Exposure_ID) and(e.sys_rowenddt = '12/31/9999') ";
            sql += "JOIN clm.Claim AS C ON e.Claim_Id = C.Claim_ID AND c.Sys_RowEndDt = '12/31/9999' ";
            sql += "where Total_Paid_Losses != 0 or Paid_DJ_Exp != 0 or Paid_NonDJ_Exp_Within_Limits != 0 or Paid_NonDJ_Exp_Outside_Limits != 0 ";


            //string sql = "SELECT ft.Exposure_ID ";
            //sql += ", SUM(COALESCE(Paid_Loss, 0) + COALESCE(Recovery_Salavage, 0) + COALESCE(Recovery_Subrogation_Loss, 0)) AS Total_Paid_Losses ";
            //sql += ", SUM(COALESCE(Paid_Coverage_DJ_Expense, 0)) AS Paid_DJ_Exp ";
            //sql += ", SUM(COALESCE(Paid_Adjusting_Expense, 0) + COALESCE(Recovery_Subrogation_Expense, 0)) AS Paid_NonDJ_Exp ";
            //sql += "INTO #Paids ";
            //sql += "FROM OPSDW.Prod_RS_ODS.trn.Financial_Transaction AS ft ";
            //sql += "WHERE Valuation_Dt = '" + month + "' ";
            //sql += "GROUP BY ft.Exposure_ID ";

            //sql += "SELECT    Exposure_Id ";
            //sql += ", SUM(COALESCE(Total_Paid_Losses, 0)) Total_Paid_Losses ";
            //sql += ", SUM(COALESCE(Paid_DJ_Exp, 0)) Paid_DJ_Exp ";
            //sql += ", SUM(COALESCE(Paid_NonDJ_Exp, 0)) Paid_NonDJ_Exp ";
            //sql += "INTO #Paids2 ";
            //sql += "FROM #Paids AS p ";
            //sql += "GROUP BY  Exposure_Id ";

            //sql += "SELECT p.Exposure_ID, p.Total_Paid_Losses, p.Paid_DJ_Exp, p.Paid_NonDJ_Exp, ";
            //sql += "RTRIM(c.Claim_No) + '-' + e.Exposure_No as ExpID ";
            //sql += "from #Paids2 p ";
            //sql += "left join clm.Exposure e on (p.Exposure_ID = e.Exposure_ID) and(e.sys_rowenddt = '12/31/9999') ";
            //sql += "JOIN clm.Claim AS C ON e.Claim_Id = C.Claim_ID AND c.Sys_RowEndDt = '12/31/9999' ";
            //sql += "where Total_Paid_Losses != 0 or Paid_DJ_Exp != 0 or Paid_NonDJ_Exp != 0";

            DataTable dtActuals = ourParent.getData("Data Source=opsdw;Initial Catalog=PROD_RS_ODS;Integrated Security=True", sql);
            return dtActuals;
        }

        private DataView getCurrentWMasDataView()
        {
            DataTable dtWM = ourParent.getData(MainWindow.ourMainWindow.dsn, "select distinct WorkMatter from [data].[WorkMatters] order by WorkMatter");
            DataView dvWM = new DataView(dtWM);
            dvWM.Sort = "WorkMatter";
            return dvWM;
        }

        private DataView getCurrentExposuresAsDataView()
        {
            DataTable dtExp = ourParent.getData(MainWindow.ourMainWindow.dsn, "select distinct ExpID from [data].[Exposures] order by ExpID");
            DataView dvExp = new DataView(dtExp);
            dvExp.Sort = "ExpID";
            return dvExp;
        }

        private bool existsInDataView(DataView dv, string value)
        {
            return (dv.Find(value) >= 0);
        }

        private void importWMandExposures()
        {
            if (MessageBox.Show("This takes a long time, are you sure?", "", MessageBoxButton.YesNo) == MessageBoxResult.No)
                return;

            ////// LOAD UP ALL THE CURRENTLY OPEN WM AND EXPOSURES //////
            DataTable dtExposures = getWMandExposures();

            // As of May 2018 we no longer delete Exposures and WorkMatters
            // This is because we may have old CF values that reference them
            // ourParent.executeSQL(ourParent.dsn, "delete from [CashFlow].[data].[Exposures]");
            // ourParent.executeSQL(ourParent.dsn, "delete from [CashFlow].[data].[WorkMatters]");

            ////// LOAD A LIST OF EXISTING WM AND EXPOSURES //////
            DataView dvWM = getCurrentWMasDataView();
            DataView dvExp = getCurrentExposuresAsDataView();

            ////// MARK ALL EXISTING EXPOSURES AND WORK MATTERS AS CLOSED //////
            ourParent.executeSQL(ourParent.dsn, "update [CashFlow].[data].[Exposures] set ExpClosed=1");
            ourParent.executeSQL(ourParent.dsn, "update [CashFlow].[data].[WorkMatters] set WMClosed=1");


            string currentWM = "";
            string portfolios = "";

            string WorkMatter = "";
            string Portfolio = "";
            string Coverage = "";
            string Type = "";
            string AttachPoint = "";
            string WorkMatterDescription = "";
            string InsuredName = "";
            string ExpID = "";
            string SpecialTrackingGroup = "";
            string AssignedAdjuster = "";
            string AssignedManager = "";
            string Department = "";
            string PolicyNumber = "";
            string EffectiveDate = "";
            string PolicyTypeDesc = "";
            string InsideLimits = "";

            foreach (DataRow dr in dtExposures.Rows)
            {
                ////// WRITE WORKMATTER HERE ON CHANGE //////
                if ((currentWM != "") && (dr["WorkMatter"].ToString() != currentWM))
                {
                    addWM(dvWM, WorkMatter, SpecialTrackingGroup, WorkMatterDescription, InsuredName, AssignedAdjuster, AssignedManager, Department, portfolios);
                    currentWM = dr["WorkMatter"].ToString();
                    portfolios = "";
                }
                else if (currentWM == "")
                    currentWM = dr["WorkMatter"].ToString();

                WorkMatter = dr["WorkMatter"].ToString();
                Portfolio = dr["Portfolio"].ToString();
                Coverage = dr["Coverage"].ToString();
                Type = dr["Type"].ToString();
                AttachPoint = dr["AttachPoint"].ToString();
                WorkMatterDescription = dr["WorkMatterDescription"].ToString();
                InsuredName = dr["InsuredName"].ToString();
                ExpID = dr["ExpID"].ToString();
                SpecialTrackingGroup = dr["SpecialTrackingGroup"].ToString();
                AssignedAdjuster = dr["AssignedAdjuster"].ToString();
                AssignedManager = dr["AssignedManager"].ToString();
                Department = dr["Department"].ToString();
                PolicyNumber = dr["PolicyNumber"].ToString();
                EffectiveDate = dr["EffectiveDate"].ToString();
                PolicyTypeDesc = dr["PolicyTypeDesc"].ToString();

                InsideLimits = "";
                string edt = dr["ExpTreatmentDesc"].ToString();
                if (edt != "")
                {
                    if (edt.IndexOf("Inside") > 0)
                        InsideLimits = "Y";
                    else if (edt.IndexOf("Outside") > 0)
                        InsideLimits = "N";
                }


                if (portfolios == "")
                    portfolios = Portfolio;
                else
                {
                    if (portfolios.IndexOf(Portfolio) < 0)
                        portfolios += ", " + Portfolio;
                }

                ////// Insert Exposure here //////
                addExposure(dvExp, WorkMatter, ExpID, Portfolio, Coverage, Type, AttachPoint, EffectiveDate, PolicyNumber, PolicyTypeDesc, InsideLimits);  // NEW INSIDE  CLA-589 

                // June 10 2018
                if ((InsideLimits == "Y") || (InsideLimits == "N"))
                    moveDefExpToInOrOut(InsideLimits, ExpID, null, "", true);
            }

            ////// WRITE LAST WORKMATTER HERE //////
            addWM(dvWM, WorkMatter, SpecialTrackingGroup, WorkMatterDescription, InsuredName, AssignedAdjuster, AssignedManager, Department, portfolios);

            ////// UPDATE ASSOCIATIONS //////
            updateWMAssociations();

            MessageBox.Show("WorkMatters and Exposures imported");
        }

        private void updateWMAssociations()
        {
            string sql = "select distinct WorkMatter from[CashFlow].[data].[Associations]";
            DataTable dtWM = ourParent.getData(ourParent.dsn, sql);

            foreach (DataRow dr in dtWM.Rows)
            {
                sql = "Update [CashFlow].[data].[WorkMatters] set HasAssociations=1 where WorkMatter='" + dr["WorkMatter"].ToString() + "'";
                ourParent.executeSQL(ourParent.dsn, sql);
            }

            int a = 4;
        }

        private void addWM(DataView dvWM, string WorkMatter, string SpecialTrackingGroup, string WorkMatterDescription, string InsuredName, string AssignedAdjuster, string AssignedManager, string Department, string portfolios)
        {
            if (existsInDataView(dvWM, WorkMatter))
            {
                string sql = "Update [CashFlow].[data].[WorkMatters] set ";
                sql += "SpecialTrackingGroup='" + SpecialTrackingGroup + "', ";
                sql += "WorkMatterDescription='" + WorkMatterDescription.Replace("'", "''") + "', ";
                sql += "InsuredName='" + InsuredName.Replace("'", "''") + "', ";
                sql += "AssignedAdjuster='" + AssignedAdjuster.Replace("'", "''") + "', ";
                sql += "AssignedManager='" + AssignedManager.Replace("'", "''") + "', ";
                sql += "Department='" + Department + "', ";
                sql += "Portfolio='" + portfolios + "', ";
                sql += "StartUser='InitialLoad', ";
                sql += "StartTime=GETDATE(), ";
                sql += "WMClosed=0 ";
                sql += "Where WorkMatter= '" + WorkMatter + "' ";

                ourParent.executeSQL(ourParent.dsn, sql);
            }

            else
            {
                string sql = "Insert into [CashFlow].[data].[WorkMatters] (WorkMatter, SpecialTrackingGroup, WorkMatterDescription, InsuredName, AssignedAdjuster, AssignedManager, Department, Portfolio, StartUser, StartTime, WMClosed) VALUES (";

                sql += "'" + WorkMatter + "',";
                sql += "'" + SpecialTrackingGroup + "',";
                sql += "'" + WorkMatterDescription.Replace("'", "''") + "',";
                sql += "'" + InsuredName.Replace("'", "''") + "',";
                sql += "'" + AssignedAdjuster.Replace("'", "''") + "',";
                sql += "'" + AssignedManager.Replace("'", "''") + "',";
                sql += "'" + Department + "',";
                sql += "'" + portfolios + "',";
                sql += "'InitialLoad',";
                sql += "GETDATE(),";
                sql += "0)";

                ourParent.executeSQL(ourParent.dsn, sql);
            }
        }

        private void addExposure(DataView dvExp, string WorkMatter, string ExpID, string Portfolio, string Coverage, string Type, string AttachPoint, string EffectiveDate, string PolicyNumber, string PolicyType, string WithinLimits)
        {
            if (existsInDataView(dvExp, ExpID))
            {
                string sql = "Update [CashFlow].[data].[Exposures] set ";

                sql += "WorkMatter='" + WorkMatter + "', ";
                sql += "ExpID='" + ExpID + "', ";
                sql += "Portfolio='" + Portfolio + "', ";
                sql += "Coverage='" + Coverage + "', ";
                sql += "Type='" + Type + "', ";
                sql += "AttachPoint=" + AttachPoint + ", ";
                sql += "EffectiveDate='" + EffectiveDate + "', ";
                sql += "PolicyNumber='" + PolicyNumber + "', ";
                sql += "PolicyType='" + PolicyType + "', ";
                sql += "WithinLimits='" + WithinLimits + "', ";  // NEW WITHIN LIMITS CLA-589
                sql += "ExpClosed=0 ";
                sql += "Where ExpID= '" + ExpID + "' ";

                ourParent.executeSQL(ourParent.dsn, sql);
            }

            else
            {
                string sql = "Insert into [CashFlow].[data].[Exposures] (WorkMatter, ExpID, Portfolio, Coverage, Type, AttachPoint, EffectiveDate, PolicyNumber, PolicyType, WithinLimits, ExpClosed) VALUES (";

                sql += "'" + WorkMatter + "',";
                sql += "'" + ExpID + "',";
                sql += "'" + Portfolio + "',";
                sql += "'" + Coverage + "',";
                sql += "'" + Type + "',";
                sql += "" + AttachPoint + ",";
                sql += "'" + EffectiveDate + "',";
                sql += "'" + PolicyNumber + "',";
                sql += "'" + PolicyType + "',";
                sql += "'" + WithinLimits + "',";  // NEW WITHIN LIMITS CLA-589
                sql += "0)";  

                ourParent.executeSQL(ourParent.dsn, sql);
            }
        }



        private DataTable getWMandExposures()
        {
            string sql = "SELECT p.TYPECODE AS Portfolio, ";
            sql += "w.ClaimNumber AS WorkMatter, ";
            sql += "ct.Name as Coverage, ";
            sql += "CASE RIGHT(ct.Name, 2) ";
            sql += "WHEN 'BI' THEN 'Bodily Injury' ";
            sql += "WHEN 'AI'   THEN 'Advertising Injury' ";
            sql += "WHEN 'LL'   THEN 'Fire Legal Liability' ";
            sql += "WHEN 'MP'   THEN 'Medical Payments' ";
            sql += "WHEN 'PD'   THEN 'Property Damage' ";
            sql += "WHEN 'PI'   THEN 'Personal Injury' ";
            sql += "ELSE '' ";
            sql += "END AS Type, ";

            sql += "ISNULL(cov.trg_AttachmentPointAmt, 0) as AttachPoint,                ";
            sql += "REPLACE(REPLACE(REPLACE(w.trg_WorkMatterDescription, CHAR(10), ''), CHAR(13), ''), '\"', '')  AS WorkMatterDescription, ";
            sql += "cp.InsuredName, ";
            sql += "RTRIM(c.CCClaimNumber) + '-' + e.trg_ExposureNumber AS ExpID, ";
            sql += "stg.DESCRIPTION AS SpecialTrackingGroup,         ";
            sql += "co.FirstName + ' ' + co.LastName AS AssignedAdjuster, ";
            sql += "ISNULL(m.AssignedManager, '') as AssignedManager,        ";
            sql += "ISNULL(u.Department, '') as Department, ";
            sql += "cp.PolicyNumber, ";
            sql += "cp.EffectiveDate, ";
            sql += "cp.Policy_Type_Desc as PolicyTypeDesc, ";
            sql += "et.DESCRIPTION AS ExpTreatmentDesc ";   // NEW INSIDE OUTSIDE
            sql += "FROM cc_exposure e ";
            sql += "JOIN ccx_trg_claim c ON e.trg_Claim = c.ID ";
            sql += "JOIN cc_claim w ON e.ClaimID = w.ID ";
            sql += "LEFT JOIN cc_user u ON u.ID = w.AssignedUserID ";
            sql += "LEFT JOIN dbo.cctl_trg_specialtrackinggroups stg ON stg.ID = w.trg_SpecialTrackingGroup ";
            sql += "LEFT JOIN dbo.cc_contact co ON co.ID = u.ContactID ";
            sql += "LEFT JOIN dbo.ccx_trg_ChildPolicy cp ON cp.ID = c.trg_ChildPolicy ";
            sql += "LEFT JOIN dbo.cctl_trg_portfolio p ON p.ID = cp.Portfolio ";
            sql += "LEFT JOIN OPS_BIU_Brewster.dbo.Manager m ON m.AssignedAdjuster = (co.FirstName + ' ' + co.LastName) ";
            sql += "LEFT JOIN dbo.cc_coverage cov on cov.ID = e.CoverageID ";
            sql += "LEFT JOIN dbo.cctl_trg_defenseexptreatment AS et ON et.ID = e.trg_ExpenseTreatmentDesc ";  // NEW INSIDE OUTSIDE
            sql += "LEFT JOIN dbo.cctl_coveragetype ct on ct.ID = cov.Type ";
            // sql += "WHERE e.CloseDate is null ";
            sql += "WHERE e.state IN (1,2) ";
            sql += "ORDER BY ";
            sql += "w.ClaimNumber, c.CCClaimNumber, e.trg_ExposureNumber ";

            DataTable dtExposures = ourParent.getData("Data Source=opsdw;Initial Catalog=PROD_ClaimCenter;Integrated Security=True", sql);
            return dtExposures;
        }

        private void btnData_Click(object sender, RoutedEventArgs e)
        {
            //return;
            gridFilterO.Visibility = Visibility.Visible;
            gridImportData.Visibility = Visibility.Visible;

            cbActualsYear.ItemsSource = new List<string> { (DateTime.Today.Year - 1).ToString(), DateTime.Today.Year.ToString() };
            cbActualsQuarter.ItemsSource = new List<string> { "1", "2", "3", "4" };

            cbActualsYear1.ItemsSource = new List<string> { (DateTime.Today.Year - 1).ToString(), DateTime.Today.Year.ToString() };
            cbActualsQuarter1.ItemsSource = new List<string> { "1", "2", "3", "4" };

        }

        private void btnCloseData_Click(object sender, RoutedEventArgs e)
        {
            gridFilterO.Visibility = Visibility.Collapsed;
            gridImportData.Visibility = Visibility.Collapsed;
        }

        private void btnMoveCurrentToHistory_Click(object sender, RoutedEventArgs e)
        {
            string sql = "delete from [CashFlow].[data].[CashFlowEntry] where EndUser = 'Former' and (year!=2017 or quarter> 2)";
            ourParent.executeSQL(ourParent.dsn, sql);

            sql = "delete from [CashFlow].[data].[CashFlowEntry] where EndUser='Former' and year=2019 and WorkMatter in ('WM000022287','WM000022606','WM000024742')";
            ourParent.executeSQL(ourParent.dsn, sql);

            sql = "insert into data.CashFlowEntry ([WorkMatter],[Exposure],[PolicyNumber],[Year],[Quarter],[ValueName],[Amount],[StartUser],[StartTime],[EndUser],[EndTime]) ";
            sql += "select[WorkMatter],'','',[Year],[Quarter],[ValueName],[Amount],[StartUser],GETDATE(),'Former',GETDATE() from data.CashFlowEntry ";
            sql += "where WorkMatter = Exposure and EndUser is null ";
            ourParent.executeSQL(ourParent.dsn, sql);

            MessageBox.Show("The current data (cash flows) has been copied to history" + Environment.NewLine + "Please re-start the application before using it");
        }

        private void btnResetStatus_Click(object sender, RoutedEventArgs e)
        {
            string sql = "update [CashFlow].[data].[Notifications] set status=''";
            ourParent.executeSQL(ourParent.dsn, sql);

            MessageBox.Show("All status has been reset." + Environment.NewLine + "Please re-start the application before using it");
        }

        private void btnAddSubstitution_Click(object sender, RoutedEventArgs e)
        {
            addSub.setOurMainWindow(MainWindow.ourMainWindow);
            addSub.fillLeft();
            addSub.fillRight();
            addSub.Visibility = Visibility.Visible;
        }

        private void btnRemoveSubstitution_Click(object sender, RoutedEventArgs e)
        {
            if (dgSubstitutions.SelectedIndex < 0)
            {
                MessageBox.Show("You must select a substitution you wish to remove");
                return;
            }

            string subadid = ((DataRowView)dgSubstitutions.SelectedItem)["SubActiveDirectoryID"].ToString();
            string foradid = ((DataRowView)dgSubstitutions.SelectedItem)["ActiveDirectoryID"].ToString();

            string SQL = "delete from [CashFlow].[data].[substitutions] where ActiveDirectoryID='" + foradid + "' and SubActiveDirectoryID='" + subadid + "'";
            ourParent.executeSQL(ourParent.dsn, SQL);

            updateSubstititutions();
            updateSubs();
        }

        private void btnActualsConfig_Click(object sender, RoutedEventArgs e)
        {
            if ((cbActualsQuarter1.SelectedIndex < 0) || (cbActualsYear1.SelectedIndex < 0))
            {
                MessageBox.Show("You must select a year and quarter");
                return;
            }
            else
            {
                int qtr = Convert.ToInt32(cbActualsQuarter1.SelectedItem.ToString());
                int year = Convert.ToInt32(cbActualsYear1.SelectedItem.ToString());

                DataSet dsNewActuals = new DataSet();
                dsNewActuals = MainWindow.getDataSproc(ourParent.dsn, "data.sp_UpdateCfaGrid", "@InputYear", year, "@InputQuarter", qtr);
                ourParent.ourData.loadCFAGrid();

                ////// Remove existing current //////
                string SQL = "delete from [CashFlow].[data].[CfaGrid] where CfaGridKey='CURRENT'";
                ourParent.executeSQL(ourParent.dsn, SQL);

                ////// Set current configuration //////
                SQL = "insert into [CashFlow].[data].[CfaGrid] VALUES('CURRENT', 'CURRENT'," + year.ToString() + "," + qtr.ToString() + ",'Y')";
                ourParent.executeSQL(ourParent.dsn, SQL);

                MessageBox.Show(string.Format("Selected Cashflow has been set to: Quarter {0}, {1}",qtr,year) + Environment.NewLine + "You must restart the application for changes to take effect" + Environment.NewLine + "The appliction will now exit");
                Environment.Exit(0);
            }
        }


        private void fillCurrentConfiguration()
        {
            DataTable current = ourParent.ourData.getCFAgrid().Select("CfaGridKey = 'CURRENT'").CopyToDataTable();
            if ((current == null) || (current.Rows.Count == 0))
                lblCurrentConfiguration.Text = "Current Quarter: NONE";
            else
                lblCurrentConfiguration.Text = "Current Quarter: " + current.Rows[0]["CfaGridYear"].ToString() + "Q" + current.Rows[0]["CfaGridQuarter"].ToString();
        }

        private void btnSelectConfiguration_Click(object sender, RoutedEventArgs e)
        {
            fillCurrentConfiguration();
        }

        private void btnExplainConfiguration_Click(object sender, RoutedEventArgs e)
        {

        }

        private void performCLA500()
        {
            List<string> exposuresMoved = new List<string>();

            ////// Get a list of Exposures that have CFE's with a DefExp //////
            DataTable dtAncientExposures = GetExposuresThatHaveDefExp();

            ////// Get the ODS Inside or Outside //////
            DataView dvInsideOutside = getODSInsideOutsideExpenseTreatmentValues();

            ////// Run though each Exposure that has a CFE entry with DefExp rather than DefExpIn or DefExpOut //////
            foreach (DataRow drBadBadExposure in dtAncientExposures.Rows)
            {
                string exposure = drBadBadExposure["Exposure"].ToString();

                if (drBadBadExposure["Exposure"].ToString() == "CC000000012-0001")
                {
                    int aaa = 44;
                }

                ////// If this exposure is already set to Y or N for WithinLimits just move the entry //////
                    string WithinLimits = drBadBadExposure["WithinLimits"].ToString();
                if ((WithinLimits == "Y") || (WithinLimits=="N"))
                    moveDefExpToInOrOut(WithinLimits, exposure, exposuresMoved, "CC", moveOnly: false);

                //////// Otherwise check ODS //////
                //else
                //{
                //    ////// Find in ODS //////
                //    int index = dvInsideOutside.Find(exposure);

                //    ////// If it isn't in the ODS, set it to Outside //////
                //    if (index < 0)
                //        moveDefExpToInOrOut("N", exposure, exposuresMoved, "Defaulted");

                //    else
                //    {
                //        string treatment = dvInsideOutside[index]["Expense_Limits_in_ODS"].ToString();
                //        if (treatment == "I")
                //            moveDefExpToInOrOut("Y", exposure, exposuresMoved, "ODS");
                //        else
                //            moveDefExpToInOrOut("N", exposure, exposuresMoved, "ODS");
                //    }
                //}
            }

            MessageBox.Show("Complete, " + dtAncientExposures.Rows.Count.ToString() + " exposures corrected");
        }

        private void moveDefExpToInOrOut(string WithinLimits, string ExpID, List<string> exposuresMoved, string byWhom, bool moveOnly)
        {
            string defExpenseType;
            if (WithinLimits == "Y")
                defExpenseType = "DefExpIn";
            else
                defExpenseType = "DefExpOut";
            string sql = "Update [CashFlow].[data].[CashFlowEntry] set ValueName='" + defExpenseType + "' where ValueName='DefExp' and Exposure='" + ExpID + "'";
            ourParent.executeSQL(ourParent.dsn, sql);

            // June 10 2018
            if (moveOnly)
                return;

            if (!exposuresMoved.Contains(ExpID))
            {
                sql = "Update [CashFlow].[data].[Exposures] set WithinLimits='" + WithinLimits + "', WithinLimitsSource='" + byWhom + "' where ExpID='" + ExpID + "'";
                ourParent.executeSQL(ourParent.dsn, sql);

                exposuresMoved.Add(ExpID);
            }
        }

        private DataTable GetExposuresThatHaveDefExp()
        {
            string sql = "";
            sql += "SELECT distinct Exposure, cfe.WorkMatter, exp.WithinLimits ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] cfe ";
            sql += "left join[CashFlow].[data].[Exposures] exp on exp.ExpID = cfe.Exposure ";
            sql += "where ValueName='DefExp' and Exposure != '' and exp.ExpID != exp.WorkMatter ";
            sql += "order by Exposure ";

            DataTable dtExp = ourParent.getData(ourParent.dsn, sql);
            return dtExp;
        }

        private DataView getODSInsideOutsideExpenseTreatmentValues()
        {
            string sql = "";
            sql += "IF OBJECT_ID('tempdb..#exposurenos') IS NOT NULL DROP TABLE #exposurenos ";
            sql += "IF OBJECT_ID('tempdb..#paids') IS NOT NULL DROP TABLE #paids ";
            sql += "IF OBJECT_ID('tempdb..#MAXDATE') IS NOT NULL DROP TABLE #MAXDATE ";
            sql += " ";
            sql += " ";
            sql += "/*Concatenates exposure number.  ";
            sql += "First half comes from the clm.Claim table, and the second half (e.g., “0001”) comes from clm.Exposure*/ ";
            sql += " ";
            sql += "USE Prod_RS_ODS ";
            sql += " ";
            sql += "SELECT e.Exposure_ID ";
            sql += "      ,RTRIM(c.Claim_No)+'-'+e.Exposure_No AS Exposure_No ";
            sql += "INTO #exposurenos              ";
            sql += "FROM clm.Exposure AS e ";
            sql += "left JOIN clm.Claim AS c ON c.Claim_ID = e.Claim_Id ";
            sql += "GROUP BY e.Exposure_ID, RTRIM(c.Claim_No)+'-'+e.Exposure_No ";
            sql += "ORDER BY Exposure_No ";
            sql += " ";
            sql += " ";
            sql += "/* Pulls all the defense expense payments in ODS for all the valuation dates   */ ";
            sql += " ";
            sql += "SELECT ft.Valuation_Dt ";
            sql += "            ,e.Exposure_No           ";
            sql += "            ,SUM(COALESCE(Paid_Adjusting_Expense_InLimits,0)) AS Paid_Adjusting_Expense_InLimits ";
            sql += "            ,SUM(COALESCE(Paid_Adjusting_Expense,0)) AS Paid_Adjusting_Expense ";
            sql += "      INTO #Paids          ";
            sql += "      FROM OPSDW.Prod_RS_ODS.trn.Financial_Transaction AS ft ";
            sql += "      LEFT JOIN #exposurenos AS e ON e.Exposure_ID = ft.Exposure_Id ";
            sql += "      GROUP BY e.Exposure_No,ft.Valuation_Dt ";
            sql += " ";
            sql += " ";
            sql += "/* Gets the Max Val date for each individual exposure*/ ";
            sql += " ";
            sql += "SELECT Max(Valuation_dt)AS maxdate, Exposure_No ";
            sql += "INTO #MAXDATE ";
            sql += "FROM #Paids ";
            sql += "GROUP BY Exposure_No ";
            sql += " ";
            sql += " ";
            sql += "/*Gets the most recent defense expense payments from ODS and determines the expense treatment. ";
            sql += "If the most recent payment is in the Paid_Adjusting_Expense_InLimits bucket, then it’s inside.  ";
            sql += "If the most recent payment is in the Paid_Adjusting_Expense bucket, then it’s outside*/ ";
            sql += " ";
            sql += "SELECT p.Valuation_Dt,p.Exposure_No, ";
            sql += "Expense_Limits_in_ODS = ";
            sql += "      CASE ";
            sql += "            WHEN Paid_Adjusting_Expense != 0.00 AND Paid_Adjusting_Expense_InLimits = 0.00  THEN 'O' ";
            sql += "            WHEN Paid_Adjusting_Expense_InLimits != 0.00 AND Paid_Adjusting_Expense =0.00 THEN 'I' ";
            sql += " ";
            sql += "            END ";
            sql += "FROM #Paids P ";
            sql += "JOIN #MAXDATE M on M.maxdate=P.Valuation_Dt and M.Exposure_No=P.Exposure_No ";
            sql += "where (Paid_Adjusting_Expense_InLimits != 0.00 AND Paid_Adjusting_Expense =0.00) or ";
            sql += "(Paid_Adjusting_Expense != 0.00 AND Paid_Adjusting_Expense_InLimits = 0.00) ";
            sql += "order by Exposure_No ";

            DataTable dtInsideOutside = ourParent.getData("Data Source=opsdw;Initial Catalog=PROD_RS_ODS;Integrated Security=True", sql);
            DataView dvInsideOutside = new DataView(dtInsideOutside);
            dvInsideOutside.Sort = "Exposure_No";
            return dvInsideOutside;
        }

        private void btnCLA500_Click(object sender, RoutedEventArgs e)
        {
            performCLA500();
        }
    }
}
