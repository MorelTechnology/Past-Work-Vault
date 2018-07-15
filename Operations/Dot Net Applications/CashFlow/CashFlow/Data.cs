using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CashFlow
{
    public class userInfo
    {
        public string adid;
        public string name;
        public string sam;
        public string dept;

        public string unit_adid;
        public string unit_name;
        public string unit_sam;

        public string team_adid;
        public string team_name;
        public string team_sam;
    }

    public class Data
    {
        #region DataTables

        public ScriptEngine se = new ScriptEngine();
        private DataTable dtWorkMatters = new DataTable();
        public DataTable dtNotifications = new DataTable();
        private DataTable dtSuperusers = new DataTable();
        private DataTable dtUsers = new DataTable();
        private DataTable dtPermissions = new DataTable();
        private DataTable dtSubstitutions = new DataTable();
        private DataTable dtExposures = new DataTable();
        private DataTable dtSTG = new DataTable();
        private DataTable dtCFAGrid = new DataTable();
        private string selectedPeriodKey = "";

        #endregion DataTables

        #region consts
        public const string CURRENTCFAKEY = "CURRENT";
        #endregion

        #region Data

        public int firstYear;
        public int firstPCFAYear;
        public int firstPCFAQuarter;
        public int actualAYear;
        public int actualAQuarter;
        public int actualBYear;
        public int actualBQuarter;
        // public int firstQuarter;

        public class PFIInfo
        {
            public int year;
            public int firstQuarter;
            public int lastQuarter;
        }
        public List<PFIInfo> PFI = new List<PFIInfo>();

        #endregion Data

        #region constructor

        public Data()
        {
        }

        #endregion constructor

        #region Load Data

        public void InitialLoad()
        {
            getAllWorkMatters();
            loadInitialTables();

            ////// Determine selected period //////
            foreach (DataRow dr in dtCFAGrid.Rows)
                if (dr["CfaGridKey"].ToString().ToUpper() == CURRENTCFAKEY)
                    selectedPeriodKey = dr["CfaGridYear"].ToString() + "Q" + dr["CfaGridQuarter"].ToString();
            if (selectedPeriodKey == "")
            {
                MessageBox.Show("There is no CURRENT period selected in the table [CashFlow].[data].[CfaGrid]" + Environment.NewLine + "CashFlow cannot continue and will exit now");
                Environment.Exit(0);
            }

            ////// Determine first year to use in CFA grid //////
            DataTable dtCFA = getCurrentCFAgrid();
            foreach (DataRow dr in dtCFA.Rows)
                if (dr["CfaGridKey"].ToString().ToUpper() == selectedPeriodKey)
                {
                    firstYear = Convert.ToInt32(dr["CfaGridYear"].ToString());  
                    break;
                }

            ////// Determine first year and quarter in Previous CFA grid //////
            DataTable dtPCFA = getCurrentPCFAgrid();
            foreach (DataRow dr in dtPCFA.Rows)
                if (dr["CfaGridKey"].ToString().ToUpper() == selectedPeriodKey)
                {
                    firstPCFAYear = Convert.ToInt32(dr["CfaGridYear"].ToString());
                    firstPCFAQuarter = Convert.ToInt32(dr["CfaGridQuarter"].ToString());
                    break;
                }

            ////// Determine the two columns of Actuals grid //////
            DataTable dtACT = getActualsGrid();
            if (dtACT.Rows.Count == 2)
            {
                actualAYear = Convert.ToInt32(dtACT.Rows[0]["CfaGridYear"].ToString());
                actualAQuarter = Convert.ToInt32(dtACT.Rows[0]["CfaGridQuarter"].ToString());
                actualBYear = Convert.ToInt32(dtACT.Rows[1]["CfaGridYear"].ToString());
                actualBQuarter = Convert.ToInt32(dtACT.Rows[1]["CfaGridQuarter"].ToString());
            }
            else
            {
                MessageBox.Show("The Actuals row count is not 2 in data.InitialLoad");
            }

            ////// Determine PFI column information for top grid of main screen //////
            determinePFIInfoFromCFAGrid();
        }

        private void determinePFIInfoFromCFAGrid()
        {
            int currentYear = 0;
            PFIInfo pi = null;

            DataTable dtCFA = getCurrentCFAgrid();
            foreach (DataRow dr in dtCFA.Rows)
                if (dr["CfaGridKey"].ToString().ToUpper() == selectedPeriodKey)
                {
                    int year = Convert.ToInt32(dr["CfaGridYear"].ToString());
                    int qtr = Convert.ToInt32(dr["CfaGridQuarter"].ToString());

                    if (year != currentYear)
                    {
                        if (pi != null)
                            PFI.Add(pi);
                        pi = new PFIInfo() { year = year, firstQuarter = (qtr==0) ? 1 : qtr, lastQuarter = (qtr==0) ? 4 : qtr };
                        currentYear = year;
                    }
                    else
                        pi.lastQuarter = qtr;
                }

            PFI.Add(pi);
        }

        public string currentPeriod()
        {
            return selectedPeriodKey;
        }

        public DataTable getCFAgrid()
        {
            return dtCFAGrid;
        }

        public DataTable getCurrentCFAgrid()
        {
            DataRow[] rows = dtCFAGrid.Select("CfaGridKey='" + selectedPeriodKey + "' and CfaGridType='CFA'","CfaGridYear, CfaGridQuarter");
            return (rows.Count() == 0) ? dtCFAGrid.Clone() : rows.CopyToDataTable();
        }

        public DataTable getCurrentPCFAgrid()
        {
            DataRow[] rows = dtCFAGrid.Select("CfaGridKey='" + selectedPeriodKey + "' and CfaGridType='PCFA'", "CfaGridYear, CfaGridQuarter");
            return (rows.Count() == 0) ? dtCFAGrid.Clone() : rows.CopyToDataTable();
        }

        public DataTable getActualsGrid()
        {
            DataRow[] rows = dtCFAGrid.Select("CfaGridKey='" + selectedPeriodKey + "' and CfaGridType='ACT'", "CfaGridYear, CfaGridQuarter");
            return (rows.Count() == 0) ? dtCFAGrid.Clone() : rows.CopyToDataTable();
        }

        public void getAllWorkMatters()
        {
            // June 8 2018 - Added "where wmClosed=0"
            string sql = "SELECT * FROM[CashFlow].[data].[WorkMatters] where wmClosed=0";

            // TEMP TEMP TEMP
            //sql += "where SpecialTrackingGroup != 'Asbestos'";


            //sql += "where SpecialTrackingGroup in ('None','CD Not DryStone','Pollution','Other Health Hazard','Chemical','Silica Sand','CD - Drystn','Lead','Pharmaceutical','Radiation','Dust','DES','Chinese Drywall','Implant')";
            dtWorkMatters = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            // dtWorkMatters = se.runScript(new DataTable(), "CASH_GETWM", "OPSCONSOLE");
            addVisualColumnsToWorkMatters();

            //sql = "select distinct SpecialTrackingGroup FROM[CashFlow].[data].[WorkMatters] where SpecialTrackingGroup != 'Asbestos' order by SpecialTrackingGroup";
            //dtSTG = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);
            //DataRow blank = dtSTG.NewRow();
            //dtSTG.Rows.InsertAt(blank, 0);

            //if (MainWindow.ourMainWindow.cbSpecialTrackingGroup.ItemsSource == null)    
            //    MainWindow.ourMainWindow.cbSpecialTrackingGroup.ItemsSource = dtSTG.DefaultView;
        }

        private void loadInitialTables()
        {
            //string sql = "SELECT u.[ActiveDirectoryID],u.[DisplayName],u.[AdjustedName],u.[SamAccountName],u.[EmailAddress],u.[Department],u.[Description],u.[SupervisorID],u.[UnitManagerID],u.[TeamManagerID],u.[ApprovalLimit],u.[StartUser],u.[EndUser],u.[StartDate],u.[EndDate],ISNULL(mgr.AdjustedName,'') as Manager,ISNULL(unit.AdjustedName,'') as UnitManager, case when u.Administrator=0 then '' else 'Yes' end as Administrator  FROM [CashFlow].[data].[users] u left join [CashFlow].[data].[users] mgr on u.SupervisorID = mgr.ActiveDirectoryID left join [CashFlow].[data].[users] unit on u.UnitManagerID = unit.ActiveDirectoryID order by DisplayName";
            loadUsers();

            string sql = "select u.[ActiveDirectoryID],u.[SubActiveDirectoryID], sub.AdjustedName, case when u.Edit=0 then 'No' else 'Yes' end as Edit FROM [CashFlow].[data].[permissions] u left join [CashFlow].[data].[users] sub on u.SubActiveDirectoryID  = sub.ActiveDirectoryID order by sub.DisplayName";
            dtPermissions = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            sql = "select * from [CashFlow].[data].[Notifications] where EndTime is null";
            dtNotifications = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            loadSubstititutions();

            sql = "select * from [CashFlow].[data].[SuperUsers] ";
            dtSuperusers = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            loadCFAGrid();
            return;

            //DataSet dsTables = se.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "CASH_GETINITIALTABLES", "OPSCONSOLE");
            // dtUsers = dsTables.Tables["USERS"];
            // dtPermissions = dsTables.Tables["PERMISSIONS"];
            // dtSubstitutions = dsTables.Tables["SUBSTITUTIONS"];
            // dtNotifications = dsTables.Tables["NOTIFICATIONS"];
        }

        public void loadUsers()
        {
            string sql = "select u.[ActiveDirectoryID],u.[DisplayName],u.[AdjustedName],u.[SamAccountName],u.[EmailAddress],u.[Department],u.[Description],u.[SupervisorID], ";
            sql += "u.[UnitManagerID],u.[TeamManagerID],u.[ApprovalLimit],u.[StartUser],u.[EndUser],u.[StartDate],u.[EndDate], ";
            sql += "ISNULL(unit.AdjustedName,'') as UnitManager,ISNULL(team.AdjustedName,'') as TeamManager, case when u.Administrator=0 then '' else 'Yes' end as Administrator ";
            sql += "from [CashFlow].[data].[users] u ";
            sql += "left join[CashFlow].[data].[users] unit on u.UnitManagerID = unit.ActiveDirectoryID ";
            sql += "left join [CashFlow].[data].[users] team on u.TeamManagerID = team.ActiveDirectoryID ";
            sql += "order by DisplayName";
            dtUsers = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);
        }

        public void loadCFAGrid()
        {
            string sql = "select * from [CashFlow].[data].[CfaGrid] order by CfaGridKey, CfaGridYear, CfaGridQuarter";
            dtCFAGrid = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);
        }

        public DataTable loadSubstititutions()
        {
            string sql = "SELECT u.[ActiveDirectoryID],u.[SubActiveDirectoryID], sub.AdjustedName as SubAdjustedName, [for].AdjustedName as ForAdjustedName FROM [CashFlow].[data].[substitutions] u left join [CashFlow].[data].[users] sub on u.SubActiveDirectoryID  = sub.ActiveDirectoryID left join [CashFlow].[data].[users] [for] on u.ActiveDirectoryID  = [for].ActiveDirectoryID order by sub.DisplayName";
            dtSubstitutions = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);
            return dtSubstitutions;
        }

        #endregion Load Data

        #region Users, Permissions and Substitutions

        public userInfo UIfromName(string name)
        {
            userInfo ui = new userInfo();

            foreach (DataRow dr in dtUsers.Rows)
                if (dr["AdjustedName"].ToString().ToUpper() == name.ToUpper())
                    getUI(dr, ref ui);
            return ui;
        }

        public userInfo UIfromADID(string adid)
        {
            userInfo ui = new userInfo();

            foreach (DataRow dr in dtUsers.Rows)
                if (dr["ActiveDirectoryID"].ToString().ToUpper() == adid.ToUpper())
                    getUI(dr, ref ui);
            return ui;
        }

        public userInfo UIfromSam(string sam)
        {
            userInfo ui = new userInfo  ();

            foreach (DataRow dr in dtUsers.Rows)
                if (dr["SamAccountName"].ToString().ToUpper() == sam.ToUpper().Replace("TRG\\", ""))
                    getUI(dr, ref ui);

            return ui;
        }

        private void getUI(DataRow dr, ref userInfo ui)
        {
            ui.adid = dr["ActiveDirectoryID"].ToString();
            ui.name = dr["AdjustedName"].ToString();
            ui.sam = dr["SamAccountName"].ToString().ToLower();
            ui.dept = dr["Department"].ToString();
            ui.unit_adid = dr["UnitManagerID"].ToString();
            ui.team_adid = dr["TeamManagerID"].ToString();

            foreach (DataRow dru in dtUsers.Rows)
                if (dru["ActiveDirectoryID"].ToString().ToUpper() == ui.unit_adid)
                {
                    ui.unit_name = dru["AdjustedName"].ToString();
                    ui.unit_sam = dru["SamAccountName"].ToString();
                }

            foreach (DataRow drt in dtUsers.Rows)
                if (drt["ActiveDirectoryID"].ToString().ToUpper() == ui.team_adid)
                {
                    ui.team_name = drt["AdjustedName"].ToString();
                    ui.team_sam = drt["SamAccountName"].ToString();
                }
        }

        public DataTable getUsersForDepartment(string dept)
        {
            DataRow[] rows = dtUsers.Select("Department='" + dept + "'");
            return rows.CopyToDataTable();
        }

        public DataTable getUsersMatchingFilter(string filter)
        {
            DataRow[] rows = dtUsers.Select("AdjustedName like'%" + filter + "%'");
            return (rows.Count() == 0) ? dtUsers.Clone() : rows.CopyToDataTable();
        }

        public DataTable getPermissionsForUser(string adid)
        {
            DataRow[] rows = dtPermissions.Select("ActiveDirectoryID='" + adid + "'");
            return (rows.Count() == 0) ? dtPermissions.Clone() : rows.CopyToDataTable();
        }

        public DataTable getSubstitutionsForUser(string adid)
        {
            DataRow[] rows = dtSubstitutions.Select("ActiveDirectoryID='" + adid + "'");
            return (rows.Count() == 0) ? dtPermissions.Clone() : rows.CopyToDataTable();
        }

        public DataTable getSubstitutionsForSub(string adid, bool blankUpFront=false)
        {
            DataRow[] rows = dtSubstitutions.Select("SubActiveDirectoryID='" + adid + "'");
            DataTable dtSubs = (rows.Count() == 0) ? dtSubstitutions.Clone() : rows.CopyToDataTable();
            if (blankUpFront)
            {
                DataRow blankRow = dtSubs.NewRow();
                dtSubs.Rows.InsertAt(blankRow, 0);
            }
            return dtSubs;
        }

        public DataTable getAllAssociatesForMe(string currentUserID)
        {
            DataTable dtce = dtUsers.Clone();
            string sudept = (isUserSuperuser(currentUserID)) ? getSuperuserDept(currentUserID) : "----";
            ////// Handle subs here //////

            foreach (DataRow dr in dtUsers.Rows)
            {
                if (dr["ActiveDirectoryID"].ToString() == currentUserID)
                    dtce.ImportRow(dr);
                else if (dr["UnitManagerID"].ToString() == currentUserID)
                    dtce.ImportRow(dr);
                else if (dr["TeamManagerID"].ToString() == currentUserID)
                    dtce.ImportRow(dr);
                else if (dr["Department"].ToString() == sudept)
                    dtce.ImportRow(dr);

                else if (MainWindow.ourMainWindow.isLauren)
                    dtce.ImportRow(dr);
            }

            return dtce;
        }

        public bool isUserAdmin(string sAMAccountName)
        {
            DataRow[] rows = dtUsers.Select("SamAccountName like '" + sAMAccountName.ToUpper().Replace("TRG\\", "") + "'");
            return ((rows.Count() == 0) || (rows[0]["Administrator"].ToString() != "Yes")) ? false : true;
        }

        public bool isUserTeamLead(string adid)
        {
            DataRow[] rows = dtUsers.Select("TeamManagerID='" + adid + "'");
            return (rows.Count() == 0) ? false : true;
        }

        public bool isUserUnitLead(string adid)
        {
            DataRow[] rows = dtUsers.Select("UnitManagerID='" + adid + "'");
            return (rows.Count() == 0) ? false : true;
        }

        public bool isUserSuperuser(string adid)
        {
            DataRow[] rows = dtSuperusers.Select("Associate='" + adid + "'");
            return (rows.Count() == 0) ? false : true;
        }

        public bool amISubbingFor(string adidMe, string adidFor)
        {
            foreach (DataRow dr in dtSubstitutions.Rows)
                if ((dr["ActiveDirectoryID"].ToString() == adidMe) && (dr["SubActiveDirectoryID"].ToString() == adidFor))
                    return true;
            return false;
        }

        public string whoISubbingFor(string adid)
        {
            foreach (DataRow dr in dtSubstitutions.Rows)
                if (dr["ActiveDirectoryID"].ToString() == adid)
                    return dr["SubActiveDirectoryID"].ToString();
            return "";
        }

        public bool isWMRecallable(bool isUnitLead, bool isTeamLead, string name, string dept, string assignedAdjuster, string workmatter, string status)
        {
            ////// CLAIMS //////
            if (dept == "Claims")
            {
                ////// If you are th Assigned Adjuster you can recall if it has been submitted but not yet APPROVED or DENIED //////
                if ((name == assignedAdjuster) && (status == MainWindow.SUBMITTED))
                    return true;

                ////// If you are the Unit Manager you can recall if you approved it to TL but it has not been APPROVED or DENIED by TL //////
                if (isUnitLead)
                {
                    if (status == MainWindow.UMAPPROVED)
                        return true;
                }

                return false;
            }

            /// OGC ///
            else
            {
                ////// If you are th Assigned Adjuster you can recall if it has been submitted but not yet APPROVED or DENIED //////
                if ((name == assignedAdjuster) && (status == MainWindow.SUBMITTED))
                    return true;
            }

            return false;
        }

        public bool canUserEditWorkmatter(bool isUnitLead, bool isTeamLead, string name, string dept, string assignedAdjuster, string workmatter, string status)
        {
            ////// CLAIMS //////
            if (dept == "Claims")
            {
                ////// OLD
                //////// If you are th Assigned Adjuster you can edit if the status is empty, PENDING or UM DENIED //////
                //if (name == assignedAdjuster)
                //{
                //    if ((status == "") || (status == MainWindow.PENDING) || (status == MainWindow.UMDENIED))
                //        return true;
                //

                ////// If you are th Assigned Adjuster you can edit if the status is empty, PENDING, DENIED or UM DENIED //////
                if (name == assignedAdjuster)
                {
                    if ((status == "") || (status == MainWindow.PENDING) || (status == MainWindow.UMDENIED) || (status == MainWindow.DENIED))
                        return true;
                }

                ////// If you are the Unit Manager you can edit if SUBMITTED (by adjuster) or DENIED (by team lead) //////
                if (isUnitLead)
                {
                    if ((status == MainWindow.SUBMITTED) || (status == MainWindow.DENIED))
                        return true;
                }

                if (isTeamLead)
                {
                    if ((status == MainWindow.UMAPPROVED))
                        return true;
                }
                return false;
            }

            /// OGC ///
            else
            {
                ////// If you are th Assigned Adjuster you can edit if the status is empty, PENDING or UM DENIED //////
                if (name == assignedAdjuster)
                {
                    if ((status == "") || (status == MainWindow.PENDING) || (status == MainWindow.UMDENIED) || (status == MainWindow.DENIED))
                        return true;
                }

                if (isUnitLead || isTeamLead)
                {
                    if ((status == MainWindow.SUBMITTED))
                        return true;
                }
            }

            if ((status == MainWindow.SUBMITTED) || (status == MainWindow.APPROVED) || (status == MainWindow.UMAPPROVED))
                return false;

            //string wmOwner = getadidFromWM(workmatter);

            ////// A user can edit a WorkMatter if it is their WM //////
            if (name == assignedAdjuster)
                return true;
            //if (adid == getadidFromWM(wmOwner))
            //    return true;

            ////// Or if they are substituting for the assigned analyst //////
            string adid = UIfromName(name).adid;
            string subid = UIfromName(assignedAdjuster).adid;

            if (amISubbingFor(adid, subid))
                return true;

            return false;
        }

        public bool canUserApproveWorkmatter(string adid, string assignedAdjuster, string workmatter, string status)
        {
            if ((status == MainWindow.PENDING) || (status == ""))
                return false;

            userInfo ui = UIfromName(assignedAdjuster);
            if ((ui.unit_adid == adid) || (ui.team_adid == adid))
                return true;

            // Removed Scott 2/5/2018
            //string subadid = whoISubbingFor(adid);
            //if ((ui.unit_adid == subadid) || (ui.team_adid == subadid))
            //    return true;

            return false;
        }

        public string getSuperuserDept(string adid)
        {
            DataRow[] rows = dtSuperusers.Select("Associate='" + adid + "'");
            if (rows.Count() >= 1)
                return rows[0]["Department"].ToString();
            return "";
        }

        public string getAllUsersInDepartment(string dept)
        {
            string associates = "";

            foreach (DataRow dr in dtUsers.Rows)
                if (dr["Department"].ToString() == dept)
                    associates += "'" + (dr["AdjustedName"].ToString().Replace("'", "''")) + "',";

            associates = associates.TrimEnd(new char[] { ',' });
            return associates;
        }

        #endregion Users, Permissions and Substitutions

        #region WorkMatters

        private void addVisualColumnsToWorkMatters()
        {
            dtWorkMatters.Columns.Add("Basis");
            dtWorkMatters.Columns.Add("Comments");
            dtWorkMatters.Columns.Add("Status");
            dtWorkMatters.Columns.Add("Confidence");
            dtWorkMatters.Columns.Add("ReasonableWorstCase", typeof(double));
            dtWorkMatters.Columns.Add("Background");
            dtWorkMatters.Columns.Add("HasAssociationsImage");
            dtWorkMatters.Columns.Add("StatusImage");
            dtWorkMatters.Columns.Add("StatusSymbolText");
            dtWorkMatters.Columns.Add("StatusColor");
            dtWorkMatters.Columns.Add("Year1Total", typeof(double));
            dtWorkMatters.Columns.Add("Year2Total", typeof(double));
            dtWorkMatters.Columns.Add("Year3Total", typeof(double));
            dtWorkMatters.Columns.Add("Year4Total", typeof(double));
            dtWorkMatters.Columns.Add("RWCLoss");
            dtWorkMatters.Columns.Add("RWCDefExp");
            dtWorkMatters.Columns.Add("RWCCovDJ");
        }

        public DataTable getWorkMatters(string associates, string searchstring, string statusFilter, string specialWM, string stg)
        {
            //if (associates == "")
            //    return dtWorkMatters.Clone();

            string filter = "";
            if (associates == "")
                associates = "''";

            //if (associates == "")
            //    filter = "(1=1) ";
            //else
                filter = "((AssignedAdjuster in (" + associates + ")) or (WorkMatter='" + specialWM + "')) ";


            if (stg != "")
                filter += "and SpecialTrackingGroup = '" + stg + "' ";

            if (searchstring.StartsWith("WM"))
            {
                string claim = searchstring.Substring(2);
                claim = claim.TrimStart(new char[] { '0' });
                filter += "and WorkMatter like '%00" + claim + "'";
            }
            else
                filter += (searchstring == "") ? "" : "and (WorkMatterDescription like '%" + searchstring + "%' or InsuredName like '%" + searchstring + "%') ";

            try
            {
                DataRow[] filtered = dtWorkMatters.Select(filter, "AssignedAdjuster, WorkMatter");
                if (filtered.Count() == 0)
                    return dtWorkMatters.Clone();
                else
                {
                    DataTable dtFiltered = filtered.CopyToDataTable();
                    applyNotificationsToGivenWorkMatters(ref dtFiltered, statusFilter);
                    applyTotalsToWorkMatters(ref dtFiltered, "");


                    // dtSTG = dtFiltered.Select("SpecialTrackingGroup", "SpecialTrackingGroup").CopyToDataTable();

                    DataView view = new DataView(dtFiltered);
                    view.Sort = "SpecialTrackingGroup";
                    dtSTG = view.ToTable(true, "SpecialTrackingGroup");

                    DataRow blank = dtSTG.NewRow();
                    dtSTG.Rows.InsertAt(blank, 0);

                    // if (MainWindow.ourMainWindow.cbSpecialTrackingGroup.ItemsSource == null)

                    //string stg2 = "";
                    //if (MainWindow.ourMainWindow.cbSpecialTrackingGroup.SelectedIndex >= 0)
                    //    stg2 = (MainWindow.ourMainWindow.cbSpecialTrackingGroup.SelectedIndex == 0) ? "" : MainWindow.ourMainWindow.cbSpecialTrackingGroup.SelectedValue.ToString();

                    MainWindow.ourMainWindow.ignoreSTGSelectionChanges = true;
                    if (MainWindow.ourMainWindow.cbSpecialTrackingGroup.ItemsSource == null)
                        MainWindow.ourMainWindow.cbSpecialTrackingGroup.ItemsSource = dtSTG.DefaultView;
                    MainWindow.ourMainWindow.ignoreSTGSelectionChanges = false;



                    //if (stg2 != "")
                    //    setComboValueOrEditbox(MainWindow.ourMainWindow.cbSpecialTrackingGroup, "SpecialTrackingGroup", stg);




                    return dtFiltered;
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().IndexOf("Error in Like") >= 0)
                {
                    MessageBox.Show("The text you have entered for searching is not valid.");
                    MainWindow.ourMainWindow.ebFilter.Text = "";
                    return dtWorkMatters.Clone();
                }
                throw ex;
            }
        }


        private void setComboValueOrEditbox(ComboBox cb, string colname, string value)
        {
            cb.SelectedIndex = -1;

            if (cb.IsEditable)
                cb.Text = value;

            else
            {
                foreach (DataRowView dr in cb.Items)
                    if (dr[colname].ToString() == value)
                    {
                        cb.SelectedItem = dr;
                        return;
                    }
            }
        }

        public string getadidFromWM(string workmatter)
        {
            string sql = "select AssignedAdjuster FROM[CashFlow].[data].[WorkMatters] where WorkMatter = '" + workmatter + "'";
            DataTable dtOneWM = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            if (dtOneWM.Rows.Count < 1)
                return "";

            userInfo ui = UIfromName(dtOneWM.Rows[0]["AssignedAdjuster"].ToString());
            return ui.adid;
        }

        public DataTable getWorkMattersFromWorkMatterList(string WorkMatters)
        {
            string filter = "WorkMatter in (" + WorkMatters + ") ";

            DataTable dtFiltered;
            DataRow[] filtered = dtWorkMatters.Select(filter, "AssignedAdjuster, WorkMatter");
            if (filtered.Count() == 0)
                dtFiltered = dtWorkMatters.Clone();
            else
                dtFiltered = filtered.CopyToDataTable();

            //    return dtWorkMatters.Clone();
            //else
            {
                applyNotificationsToGivenWorkMatters(ref dtFiltered, "");
                foreach (string f in WorkMatters.Split(new char[] { ',' }))
                {
                    bool found = false;
                    foreach (DataRow dr in dtFiltered.Rows)
                    {
                        if (dr["WorkMatter"].ToString() == f.Replace("'", ""))
                            found = true;
                    }

                    if (found == false)
                    {
                        DataRow dtNewRow = dtFiltered.NewRow();
                        dtNewRow["WorkMatter"] = f.Replace("'", "");
                        dtNewRow["WorkMatterDescription"] = "Closed";
                        //dtFiltered.ImportRow(dtNewRow);
                        dtFiltered.Rows.Add(dtNewRow);
                    }
                }
                return dtFiltered;
            }
        }

        public void applyNotificationsToGivenWorkMatters(ref DataTable dtData, string statusFilter)
        {
            // LAUREN TEST
            //if (MainWindow.ourMainWindow.isLauren)
            //    return;

            foreach (DataRow dr in dtData.Rows)
            {
                if (dr["HasAssociations"].ToString().ToUpper() == "TRUE")
                    dr["HasAssociationsImage"] = "images/link16.png";
                else
                    dr["HasAssociationsImage"] = "";

                dr["Background"] = "Transparent";
                dr["Status"] = "";

                string wm = dr["WorkMatter"].ToString();
                foreach (DataRow dr2 in dtNotifications.Rows)
                {
                    string wm2 = dr2["WorkMatter"].ToString();
                    if (wm == wm2)
                    {
                        string status = dr2["Status"].ToString();
                        dr["Basis"] = dr2["Basis"].ToString();
                        dr["Comments"] = dr2["Comments"].ToString();
                        dr["Confidence"] = dr2["Confidence"].ToString();
                        dr["ReasonableWorstCase"] = Convert.ToDouble(dr2["ReasonableWorstCase"].ToString());
                        dr["RWCLoss"] = Convert.ToDouble(dr2["RWCLoss"].ToString());
                        dr["RWCDefExp"] = Convert.ToDouble(dr2["RWCDefExp"].ToString());
                        dr["RWCCovDJ"] = Convert.ToDouble(dr2["RWCCovDJ"].ToString());

                        dr["Status"] = status;

                        if ((status == MainWindow.SUBMITTED) || (status == MainWindow.UMAPPROVED))
                        {
                            //dr["Background"] = "PeachPuff";
                            dr["StatusImage"] = "images/submitted.png";
                            dr["StatusSymbolText"] = "l";
                            dr["StatusColor"] = "SandyBrown";
                        }

                        if ((status == MainWindow.DENIED) || (status == MainWindow.UMDENIED))
                        {
                            //dr["Background"] = "LightCoral";
                            dr["StatusImage"] = "images/rejected.png";
                            dr["StatusSymbolText"] = "n";
                            dr["StatusColor"] = "FireBrick";
                        }

                        if (status == MainWindow.APPROVED)
                        {
                            //dr["Background"] = "DarkSeaGreen";
                            dr["StatusImage"] = "images/approved.png";
                            dr["StatusSymbolText"] = "l";
                            dr["StatusColor"] = "MediumSeaGreen";
                        }
                    }
                }
            }

            string filter = "";
            if (statusFilter == "Approved")
                filter = "Status='Approved'";

            if (statusFilter == "Denied")
                filter = "(Status='" + MainWindow.DENIED + "' or Status='" + MainWindow.UMDENIED + "')";

            if (statusFilter == "Incomplete")
                filter = "(Status='' or Status='Pending')";

            if (filter != "")
            {
                DataRow[] filtered = dtData.Select(filter, "AssignedAdjuster, WorkMatter");
                if (filtered.Count() == 0)
                    dtData = dtWorkMatters.Clone();
                else
                {
                    dtData = filtered.CopyToDataTable();
                }
            }
        }

        public void applyTotalsToWorkMatters(ref DataTable dtData, string statusFilter)
        {
            string wmlist = DataTableToCommaSeparatedList(dtData, "WorkMatter", "'");
            DataTable dtCFWM = loadCashFlowForExposure(wmlist);

            foreach (DataRow dr in dtData.Rows)
            {
                string wm = dr["WorkMatter"].ToString();

                double year1 = 0d;
                double year2 = 0d;
                double year3 = 0d;
                double year4 = 0d;

                foreach (DataRow drcf in dtCFWM.Rows)
                {
                    if (drcf["WorkMatter"].ToString() == wm)
                    {
                        int year = Convert.ToInt32(drcf["Year"].ToString());
                        int quarter = Convert.ToInt32(drcf["Quarter"].ToString());

                        for (int iy = 0;iy < PFI.Count; iy++)
                        {
                            if ((year == PFI[iy].year)  &&
                                (quarter >= PFI[iy].firstQuarter) && (quarter <= PFI[iy].lastQuarter))
                            {
                                if (iy == 0)
                                    year1 += Convert.ToDouble(drcf["Amount"].ToString());
                                if (iy == 1)
                                    year2 += Convert.ToDouble(drcf["Amount"].ToString());
                                if (iy == 2)
                                    year3 += Convert.ToDouble(drcf["Amount"].ToString());
                                if (iy == 3)
                                    year4 += Convert.ToDouble(drcf["Amount"].ToString());
                            }
                        }

                    }
                }

                dr["Year1Total"] = year1;
                dr["Year2Total"] = year2;
                dr["Year3Total"] = year3;
                dr["Year4Total"] = year4;
            }
        }

        public DataTable getAssociatedWorkMatters(string workmatter)
        {
            string sql = "select WorkMatter from[CashFlow].[data].[Associations] ";
            sql += "where[AssociationID] in ";
            sql += "(SELECT[AssociationID] ";
            sql += "FROM[CashFlow].[data].[Associations] ";
            sql += "where WorkMatter = '" + workmatter + "') ";
            sql += "and WorkMatter != '" + workmatter + "'";
            DataTable dtAssoc = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            //DataSet dsTables = se.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "CASH_GETASSOCIATEDWM", "OPSCONSOLE", false, "@WorkMatter", workmatter);
            //DataTable dtAssoc = dsTables.Tables[0];
            return dtAssoc;
        }

        public DataSet getWorkMatterHistory(string workmatter)
        {
            string PrevCfaSql = "";
            string sql = "select u.AdjustedName,n.* from[CashFlow].[data].[Notifications] n ";
            sql += "left join[CashFlow].[data].[Users] ";
            sql += "u on n.StartUser = upper(u.SamAccountName) ";
            sql += "where WorkMatter='" + workmatter + "' ";
            sql += "order by StartTime desc ";

            // APRIL 8 2018
            DataTable dtWMHistory = MainWindow.ourMainWindow.getData2B(MainWindow.ourMainWindow.dsn, sql);

            DataTable dtPCFAPrev = getCurrentPCFAgrid();
            int currentIndex = 0;
            foreach (DataRow dr in dtPCFAPrev.Rows)
            {
                int PcfaYear = Convert.ToInt32(dr["CfaGridYear"].ToString());
                int PcfaQuarter = Convert.ToInt32(dr["CfaGridQuarter"].ToString());
                if (currentIndex < dtPCFAPrev.Rows.Count -1)
                {
                    PrevCfaSql += genGetPrevCFSQL(PcfaYear, PcfaQuarter, workmatter, true);
                }
                else
                {
                    PrevCfaSql += genGetPrevCFSQL(PcfaYear, PcfaQuarter, workmatter, false);
                }
                currentIndex++;
            }

            // APRIL 8 2018
            DataTable dtPrev = MainWindow.ourMainWindow.getData2B(MainWindow.ourMainWindow.dsn, PrevCfaSql);
            //DataTable dtPrev2 = 
            dtPrev.Columns.Add("Total", typeof(double));

            double totLoss = 0d;
            double totDefExp = 0d;
            double totCovDJ = 0d;

            foreach (DataRow dr in dtPrev.Rows)
            {
                double loss = MainWindow.ourMainWindow.formattedTextToDouble(dr["Loss"].ToString());
                double defexp = MainWindow.ourMainWindow.formattedTextToDouble(dr["DefExp"].ToString());
                double defexpin = MainWindow.ourMainWindow.formattedTextToDouble(dr["DefExpIn"].ToString());
                double defexpout = MainWindow.ourMainWindow.formattedTextToDouble(dr["DefExpOut"].ToString());
                double covdj = MainWindow.ourMainWindow.formattedTextToDouble(dr["CovDJ"].ToString());

                //dr["Total"] = loss + defexp + covdj;
                dr["Total"] = loss + defexpin + defexpout + defexp + covdj;
                totLoss += loss;
                //totDefExp += defexp;
                totDefExp += defexpin + defexpout + defexp;
                totCovDJ += covdj;
            }

            // June 11 2018 - Use correct columns
            dtPrev.Rows.Add("Total", totCovDJ, totDefExp, 0,0,totLoss, totCovDJ + totDefExp + totLoss);

            DataSet ds = new DataSet();
            ds.Tables.Add(dtWMHistory);
            ds.Tables.Add(dtPrev);
            return ds;

            // [data].[sp_GetPreviousCashFlow]"

            //DataSet dsTables = se.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "CASH_GETALLHISTORY", "OPSCONSOLE", false, "@WorkMatter", workmatter, "@StartingYear", DateTime.Now.Year.ToString());
            //DataTable dtWMHistory = dsTables.Tables[0];
            //return dsTables;
        }

        private string genGetPrevCFSQLnotright(int StartingYear, string WorkMatter)
        {
            string sql = "Select convert(varchar(10), " + StartingYear.ToString() + ")+'Q1' as Period, ";
            sql += "ISNULL((Select Sum(Amount) as CovDJ ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 1 ";
            sql += "and ValueName = 'CovDJ' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as CovDJ, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExp ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 1 ";
            sql += "and ValueName = 'DefExp' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExp, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as Loss ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 1 ";
            sql += "and ValueName = 'Loss' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as Loss ";
            sql += " ";
            sql += "union all ";
            sql += " ";
            sql += "Select convert(varchar(10), " + StartingYear.ToString() + ")+'Q2' as Period, ";
            sql += "ISNULL((Select Sum(Amount) as CovDJ ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 2 ";
            sql += "and ValueName = 'CovDJ' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as CovDJ, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExp ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 2 ";
            sql += "and ValueName = 'DefExp' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExp, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as Loss ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 2 ";
            sql += "and ValueName = 'Loss' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as Loss ";
            sql += " ";
            sql += "union all ";
            sql += " ";
            sql += "Select convert(varchar(10), " + StartingYear.ToString() + ")+'Q3' as Period, ";
            sql += "ISNULL((Select Sum(Amount) as CovDJ ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 3 ";
            sql += "and ValueName = 'CovDJ' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as CovDJ, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExp ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 3 ";
            sql += "and ValueName = 'DefExp' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExp, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as Loss ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 3 ";
            sql += "and ValueName = 'Loss' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as Loss ";
            sql += " ";
            sql += "union all ";
            sql += " ";
            sql += "Select convert(varchar(10), " + StartingYear.ToString() + ")+'Q4' as Period, ";
            sql += "ISNULL((Select Sum(Amount) as CovDJ ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 4 ";
            sql += "and ValueName = 'CovDJ' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as CovDJ, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExp ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 4 ";
            sql += "and ValueName = 'DefExp' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExp, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as Loss ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + StartingYear.ToString() + " ";
            sql += "and Quarter = 4 ";
            sql += "and ValueName = 'Loss' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as Loss ";
            sql += " ";
            sql += "union all ";
            sql += " ";
            sql += "Select convert(varchar(10), " + (StartingYear + 1).ToString() + " ) as Period, ";
            sql += "ISNULL((Select Sum(Amount) as CovDJ ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + (StartingYear + 1).ToString() + " ";
            sql += "and ValueName = 'CovDJ' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as CovDJ, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExp ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + (StartingYear + 1).ToString() + "  ";
            sql += "and ValueName = 'DefExp' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExp, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as Loss ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + (StartingYear + 1).ToString() + " ";
            sql += "and ValueName = 'Loss' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as Loss ";
            sql += " ";
            sql += "union all ";
            sql += " ";
            sql += "Select convert(varchar(10), " + (StartingYear + 2).ToString() + " ) as Period, ";
            sql += "ISNULL((Select Sum(Amount) as CovDJ ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + (StartingYear + 2).ToString() + " ";
            sql += "and ValueName = 'CovDJ' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as CovDJ, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExp ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + (StartingYear + 2).ToString() + " ";
            sql += "and ValueName = 'DefExp' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExp, ";
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as Loss ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser is null and Exposure=WorkMatter ";
            sql += "and Year = " + (StartingYear + 2).ToString() + " ";
            sql += "and ValueName = 'Loss' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as Loss ";
            sql += " ";
            return sql;
        }

        private string genertePrevCFSQLForQuarter(int year, int qtr, string WorkMatter, bool union)
        {
            string sql = "";

            if (qtr == 0)
                sql = "Select convert(varchar(10), " + year.ToString() + ") as Period, ";
            else
                sql = "Select convert(varchar(10), " + year.ToString() + ")+'Q" + qtr.ToString() + "' as Period, ";

            sql += "ISNULL((Select Sum(Amount) as CovDJ ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";

            // JUNE 11 2018
            sql += "where EndUser = 'Former' ";
            sql += "and Year = " + year.ToString() + " ";
            if (qtr != 0)
                sql += "and Quarter = " + qtr.ToString() + " ";
            sql += "and ValueName = 'CovDJ' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as CovDJ, ";

            // June 10 2018 uncommented this
            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExp ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser = 'Former' ";
            sql += "and Year = " + year.ToString() + " ";
            if (qtr != 0)
                sql += "and Quarter = " + qtr.ToString() + " ";
            // CLA-588
            sql += "and ValueName = 'DefExp' ";
            //sql += "and ValueName in ('DefExp','DefExpIn','DefExpOut') ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExp, ";


            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExpIn ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser = 'Former' ";
            sql += "and Year = " + year.ToString() + " ";
            if (qtr != 0)
                sql += "and Quarter = " + qtr.ToString() + " ";
            sql += "and ValueName in ('DefExpIn') ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExpIn, ";

            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as DefExpOut ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser = 'Former' ";
            sql += "and Year = " + year.ToString() + " ";
            if (qtr != 0)
                sql += "and Quarter = " + qtr.ToString() + " ";
            sql += "and ValueName = 'DefExpOut' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as DefExpOut, ";








            sql += " ";
            sql += "ISNULL((Select Sum(Amount) as Loss ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "where EndUser = 'Former' ";
            sql += "and Year = " + year.ToString() + " ";
            if (qtr != 0)
                sql += "and Quarter = " + qtr.ToString() + " ";
            sql += "and ValueName = 'Loss' ";
            sql += "and WorkMatter = '" + WorkMatter.ToString() + "'),0) as Loss ";
            sql += " ";
            if (union)
                sql += "union all ";

            return sql;
        }

        private string genGetPrevCFSQL(int PcfaYear, int PcfaQuarter, string WorkMatter, bool IsUnion)
        {
            string sql = "";
            
                sql += genertePrevCFSQLForQuarter(PcfaYear, PcfaQuarter, WorkMatter, IsUnion);
            
            
            return sql;
        }



        #endregion WorkMatters

        #region Exposures

        public DataTable getExposureHistory(string exposure)
        {
            string sql = "SELECT [StartTime],u.AdjustedName ";
            sql += "FROM [CashFlow].[data].[CashFlowEntry] cfe ";
            sql += "left join[CashFlow].[data].[Users] u ";
            sql += "on cfe.StartUser = upper(u.SamAccountName) ";
            sql += "where Exposure='" + exposure + "' ";
            sql += "group by StartTime, u.AdjustedName ";
            sql += "order by StartTime desc";
            //DataTable dtExpHistory = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            // APRIL 8 2018
            DataTable dtExpHistory = MainWindow.ourMainWindow.getData2B(MainWindow.ourMainWindow.dsn, sql);

            //DataSet dsTables = se.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "CASH_GETEXPOSUREHISTORY", "OPSCONSOLE", false, "@Exposure", exposure);
            //DataTable dtExpHistory = dsTables.Tables[0];
            return dtExpHistory;
        }

        public DataTable getExposuresForWorkMatter(string workmatter)
        {
            string y1 = actualAYear.ToString();
            string q1 = actualAQuarter.ToString();
            string y2 = actualBYear.ToString();
            string q2 = actualBQuarter.ToString();

            MainWindow.countIt("EFWM");
            DataTable dtCFA = getCashFlowAmountsForWM(workmatter);
            MainWindow.countIt("EFWM getCashFlowAmountsForWM");

            //string sql = "SELECT TOP 1000 [WorkMatter],e.[ExpID],[Portfolio],[Coverage],[Type],[AttachPoint],[EffectiveDate],[PolicyNumber],[PolicyType] ";
            //sql += ",actq1.Paid_DJ_Exp as Q1PaidDJ ";
            //sql += ",actq1.Paid_NonDJ_Exp as Q1PaidNONDJ,actq1.Total_Paid_Losses as Q1PaidLosses,actq1.Year as Q1Y,actq1.Quarter as Q1Q ";
            //sql += ",actq2.Paid_DJ_Exp as Q2PaidDJ,actq2.Paid_NonDJ_Exp as Q2PaidNONDJ,actq2.Total_Paid_Losses as Q2PaidLosses,actq2.Year as Q2Y,actq2.Quarter as Q2Q ";
            //sql += "FROM[CashFlow].[data].[Exposures] e ";
            //sql += "left join data.Actuals actq1 on (actq1.ExpID = e.ExpID) and(actq1.Year= 2017) and(actq1.Quarter= 1) ";
            //sql += "left join data.Actuals actq2 on(actq2.ExpID = e.ExpID) and(actq2.Year= 2017) and(actq2.Quarter= 2) ";
            //sql += "WHERE[WorkMatter] ='" + workmatter + "'";

            string sql = "SELECT TOP 1000[WorkMatter],e.[ExpID],[Portfolio],[Coverage],[Type],[AttachPoint],[EffectiveDate],[PolicyNumber],[PolicyType] ";
            //sql += ", actq1.Paid_DJ_Exp as Q1PaidDJ ";
            //sql += ",actq1.Paid_NonDJ_Exp as Q1PaidNONDJ,actq1.Total_Paid_Losses as Q1PaidLosses ";
            //sql += ",(actq1.Paid_DJ_Exp + actq1.Paid_NonDJ_Exp + actq1.Total_Paid_Losses) as Q1Total, ";
            //sql += "actq1.Year as Q1Y,actq1.Quarter as Q1Q  ";

            // May 28 
            sql += ",actq2.Paid_DJ_Exp as Q2PaidDJ,actq2.Paid_NonDJ_Exp + actq2.Paid_NonDJ_Exp_Within_Limits + actq2.Paid_NonDJ_Exp_Outside_Limits as Q2PaidNONDJ,actq2.Total_Paid_Losses as Q2PaidLosses ";
            sql += ",(actq2.Paid_DJ_Exp + actq2.Paid_NonDJ_Exp + actq2.Total_Paid_Losses + actq2.Paid_NonDJ_Exp_Within_Limits + actq2.Paid_NonDJ_Exp_Outside_Limits) as Q2Total, ";
            sql += "actq2.Year as Q2Y,actq2.Quarter as Q2Q ";

            sql += ",actq3.Paid_DJ_Exp as Q3PaidDJ,actq3.Paid_NonDJ_Exp + actq3.Paid_NonDJ_Exp_Within_Limits + actq3.Paid_NonDJ_Exp_Outside_Limits as Q3PaidNONDJ,actq3.Total_Paid_Losses as Q3PaidLosses ";
            sql += ",(actq3.Paid_DJ_Exp + actq3.Paid_NonDJ_Exp + actq3.Total_Paid_Losses + actq3.Paid_NonDJ_Exp_Within_Limits + actq3.Paid_NonDJ_Exp_Outside_Limits) as Q3Total, ";
            sql += "actq3.Year as Q3Y,actq3.Quarter as Q3Q ";

            //sql += ",actq2.Paid_DJ_Exp as Q2PaidDJ,actq2.Paid_NonDJ_Exp as Q2PaidNONDJ,actq2.Total_Paid_Losses as Q2PaidLosses ";
            //sql += ",(actq2.Paid_DJ_Exp + actq2.Paid_NonDJ_Exp + actq2.Total_Paid_Losses) as Q2Total, ";
            //sql += "actq2.Year as Q2Y,actq2.Quarter as Q2Q ";

            //sql += ",actq3.Paid_DJ_Exp as Q3PaidDJ,actq3.Paid_NonDJ_Exp as Q3PaidNONDJ,actq3.Total_Paid_Losses as Q3PaidLosses ";
            //sql += ",(actq3.Paid_DJ_Exp + actq3.Paid_NonDJ_Exp + actq3.Total_Paid_Losses) as Q3Total, ";
            //sql += "actq3.Year as Q3Y,actq3.Quarter as Q3Q ";





            sql += ",'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' as Q2PaidLossesDelta ";
            sql += ",'DarkGreenxxxxxxxxxxxxxx' as Q2PaidLossesDeltaColor ";

            sql += ",'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' as Q2PaidDJDelta ";
            sql += ",'Blackxxxxxxxxxxxxxxxx' as Q2PaidDJDeltaColor ";

            sql += ",'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' as Q2PaidNONDJDelta ";
            sql += ",'DarkRedxxxxxxxxxxxxxx' as Q2PaidNONDJDeltaColor ";

            sql += ",'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx' as Q2TotalDelta ";
            sql += ",'Blackxxxxxxxxxxxxxxxxxxxx' as Q2TotalDeltaColor ";

            sql += ",'$12,000xxxxxxxx" + Environment.NewLine + "-$11,000" + Environment.NewLine + "=$11,000' as Q3PaidLossesDelta ";
            sql += ",'Chocolatexxxxxxxxxxxxxxxxxxxxxxxx' as Q3PaidLossesDeltaColor ";

            sql += ",'$13,000xxxxxxxxx" + Environment.NewLine + "-$12,000" + Environment.NewLine + "=$11,000' as Q3PaidDJDelta ";
            sql += ",'Bluexxxxxxxxxxxxxxxxxxx' as Q3PaidDJDeltaColor ";

            sql += ",'$14,000xxxxxxx" + Environment.NewLine + "-$13,000" + Environment.NewLine + "=$11,000' as Q3PaidNONDJDelta ";
            sql += ",'DeepPinkxxxxxxxxxxxxxxxxxx' as Q3PaidNONDJDeltaColor ";

            sql += ",'$15,000xxxxxxxx" + Environment.NewLine + "-$14,000" + Environment.NewLine + "=$11,000' as Q3TotalDelta ";
            sql += ",'MediumVioletRedxxxxxxxxxxxxxxxxx' as Q3TotalDeltaColor ";

            sql += ",'DarkGreenxxxxxxxxxxxxxxxxxxx' as Q2PaidLossesDeltaColor ";
            sql += ",'Blackxxxxxxxxxxxxxxxxxxxx' as Q2PaidDJDeltaColor ";
            sql += ",'DarkRedxxxxxxxxxxxxxxxxx' as Q2PaidNONDJDeltaColor ";
            sql += ",'CarkGreenxxxxxxxxxxxxxxx' as Q2QDeltaColor ";

            sql += ",'DarkGreenxxxxxxxxxxxxxx' as Q3PaidLossesDeltaColor ";
            sql += ",'Blackxxxxxxxxxxxxxxxxxxx' as Q3PaidDJDeltaColor ";
            sql += ",'DarkRedxxxxxxxxxxxxxx' as Q3PaidNONDJDeltaColor ";
            sql += ",'CarkGreenxxxxxxxxxxxxx' as Q3QDeltaColor ";

            sql += ",WithinLimits ";
            sql += ",'' as WithinLimitsDisplay ";


            sql += "FROM[CashFlow].[data].[Exposures] e ";

            //sql += "left join data.Actuals actq1 on (actq1.ExpID = e.ExpID) and(actq1.Year= 2017) and(actq1.Quarter= 1) ";
            sql += "left join data.Actuals actq2 on(actq2.ExpID = e.ExpID) and(actq2.Year= " + y1 + ") and(actq2.Quarter= " + q1 + ") ";
            sql += "left join data.Actuals actq3 on (actq3.ExpID = e.ExpID) and(actq3.Year= " + y2 + ") and(actq3.Quarter= " + q2 + ") ";

            // was
            // sql += "WHERE[WorkMatter] ='" + workmatter + "'";

            // June 8 2018
            sql += "WHERE[WorkMatter] ='" + workmatter + "' and ExpClosed = 0";
            MainWindow.countIt("EFWM gensql");


            // APRIL 8 2018
            //dtExposures = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);
            //MainWindow.countIt("EFWM getData");

            // APRIL 8 2018
            dtExposures = MainWindow.ourMainWindow.getData2B(MainWindow.ourMainWindow.dsn, sql);
            MainWindow.countIt("EFWM getData2B");

            //dtExposures = MainWindow.ourMainWindow.getData3(MainWindow.ourMainWindow.dsn, sql);
            //MainWindow.countIt("EFWM getData3");



            //dtExposures.Columns.Add("Q2PaidLossesDelta", typeof(string));

            //dtExposures.Columns.Add("Q2PaidLossesDeltaColor", typeof(string));
            //dtExposures.Columns.Add("Q2PaidDJDelta", typeof(string));
            //dtExposures.Columns.Add("Q2PaidDJDeltaColor", typeof(string));
            //dtExposures.Columns.Add("Q2PaidNONDJDelta", typeof(string));
            //dtExposures.Columns.Add("Q2PaidNONDJDeltaColor", typeof(string));
            //dtExposures.Columns.Add("Q2TotalDelta", typeof(string));
            //dtExposures.Columns.Add("Q2TotalDeltaColor", typeof(string));
            //dtExposures.Columns.Add("Q3PaidLossesDelta", typeof(string));
            //dtExposures.Columns.Add("Q3PaidLossesDeltaColor", typeof(string));
            //dtExposures.Columns.Add("Q3PaidDJDelta", typeof(string));
            //dtExposures.Columns.Add("Q3PaidDJDeltaColor", typeof(string));
            //dtExposures.Columns.Add("Q3PaidNONDJDelta", typeof(string));
            //dtExposures.Columns.Add("Q3PaidNONDJDeltaColor", typeof(string));
            //dtExposures.Columns.Add("Q3TotalDelta", typeof(string));
            //dtExposures.Columns.Add("Q3TotalDeltaColor", typeof(string));
            //dtExposures.Columns.Add("", typeof(string));
            //dtExposures.Columns.Add("", typeof(string));
            //dtExposures.Columns.Add("", typeof(string));
            //dtExposures.Columns.Add("", typeof(string));
            //dtExposures.Columns.Add("", typeof(string));
            //dtExposures.Columns.Add("", typeof(string));


            //dtExposures.Columns.Add(" Q2PaidLossesDeltaColor ";
            //dtExposures.Columns.Add(" Q2PaidDJDeltaColor ";
            //dtExposures.Columns.Add(" Q2PaidNONDJDeltaColor ";
            //dtExposures.Columns.Add(" Q2QDeltaColor ";
            //dtExposures.Columns.Add("
            //dtExposures.Columns.Add(" Q3PaidLossesDeltaColor ";
            //dtExposures.Columns.Add(" Q3PaidDJDeltaColor ";
            //dtExposures.Columns.Add(" Q3PaidNONDJDeltaColor ";
            //dtExposures.Columns.Add(" Q3QDeltaColor ";
            //dtExposures.Columns.Add("
            //dtExposures.Columns.Add("
            //dtExposures.Columns.Add("



            foreach (DataRow drExp in dtExposures.Rows)
            {
                if (drExp["WithinLimits"].ToString() == "N")
                    drExp["WithinLimitsDisplay"] = "Outside";
                else if (drExp["WithinLimits"].ToString() == "Y")
                    drExp["WithinLimitsDisplay"] = "Within";
            }
            MainWindow.countIt("EFWM for loop 1");

            foreach (DataRow drExp in dtExposures.Rows)
            {
                string exposure = drExp["ExpID"].ToString();

                // June 12 2018
                double cfdef = getCashFlowAmount(dtCFA, workmatter, exposure, y1, q1, "DefExp");

                double cfdefin = getCashFlowAmount(dtCFA, workmatter, exposure, y1, q1, "DefExpIn");
                double cfdefout = getCashFlowAmount(dtCFA, workmatter, exposure, y1, q1, "DefExpOut");
                double cfdj = getCashFlowAmount(dtCFA, workmatter, exposure, y1, q1, "CovDJ");
                double cfloss = getCashFlowAmount(dtCFA, workmatter, exposure, y1, q1, "Loss");

                double adef = safeConvert(drExp["Q2PaidNONDJ"].ToString());
                double adj = safeConvert(drExp["Q2PaidDJ"].ToString());
                double aloss = safeConvert(drExp["Q2PaidLosses"].ToString());

                drExp["Q2PaidDJDelta"] = generateText(cfdj, adj);
                drExp["Q2PaidDJDeltaColor"] = generateColor(cfdj, adj);

                //drExp["Q2PaidNONDJDelta"] = generateText(cfdef, adef);
                //drExp["Q2PaidNONDJDeltaColor"] = generateColor(cfdef, adef);

                // June 12 2018 add orphans
                drExp["Q2PaidNONDJDelta"] = generateText(cfdefin + cfdefout + cfdef, adef);
                drExp["Q2PaidNONDJDeltaColor"] = generateColor(cfdefin + cfdefout +cfdef, adef);

                drExp["Q2PaidLossesDelta"] = generateText(cfloss, aloss);
                drExp["Q2PaidLossesDeltaColor"] = generateColor(cfloss, aloss);

                //drExp["Q2TotalDelta"] = generateText(cfdj + cfloss + cfdef, adj + aloss + adef);
                //drExp["Q2TotalDeltaColor"] = generateColor(cfdj + cfloss + cfdef, adj + aloss + adef);

                // June 12 2018
                drExp["Q2TotalDelta"] = generateText(cfdj + cfloss + cfdefin + cfdefout + cfdef, adj + aloss + adef);
                drExp["Q2TotalDeltaColor"] = generateColor(cfdj + cfloss + cfdefin + cfdefout + cfdef, adj + aloss + adef);
            }
            MainWindow.countIt("EFWM for loop 2");


            foreach (DataRow drExp in dtExposures.Rows)
            {
                string exposure = drExp["ExpID"].ToString();
                double cfdef = getCashFlowAmount(dtCFA, workmatter, exposure, y2, q2, "DefExp");
                double cfdefin = getCashFlowAmount(dtCFA, workmatter, exposure, y2, q2, "DefExpIn");
                double cfdefout = getCashFlowAmount(dtCFA, workmatter, exposure, y2, q2, "DefExpOut");
                double cfdj = getCashFlowAmount(dtCFA, workmatter, exposure, y2, q2, "CovDJ");
                double cfloss = getCashFlowAmount(dtCFA, workmatter, exposure, y2, q2, "Loss");

                double adef = safeConvert(drExp["Q3PaidNONDJ"].ToString());
                double adj = safeConvert(drExp["Q3PaidDJ"].ToString());
                double aloss = safeConvert(drExp["Q3PaidLosses"].ToString());

                drExp["Q3PaidDJDelta"] = generateText(cfdj, adj);
                drExp["Q3PaidDJDeltaColor"] = generateColor(cfdj, adj);

                //drExp["Q3PaidNONDJDelta"] = generateText(cfdef, adef);
                //drExp["Q3PaidNONDJDeltaColor"] = generateColor(cfdef, adef);
                drExp["Q3PaidNONDJDelta"] = generateText(cfdefin + cfdefout + cfdef, adef);
                drExp["Q3PaidNONDJDeltaColor"] = generateColor(cfdefin + cfdefout + cfdef, adef);

                drExp["Q3PaidLossesDelta"] = generateText(cfloss, aloss);
                drExp["Q3PaidLossesDeltaColor"] = generateColor(cfloss, aloss);

                //drExp["Q3TotalDelta"] = generateText(cfdj+cfloss+cfdef, adj+aloss+adef);
                //drExp["Q3TotalDeltaColor"] = generateColor(cfdj + cfloss + cfdef, adj + aloss + adef);

                drExp["Q3TotalDelta"] = generateText(cfdj + cfloss + cfdefin + cfdefout + cfdef, adj + aloss + adef);
                drExp["Q3TotalDeltaColor"] = generateColor(cfdj + cfloss + cfdefin + cfdefout + cfdef, adj + aloss + adef);
            }
            MainWindow.countIt("EFWM for loop 3");


            return dtExposures;

            //DataSet dsTables = se.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "CASH_GETEXPOSURES", "OPSCONSOLE", false, "@WorkMatter", workmatter);
            // dtExposures = dsTables.Tables[0];
            // return dtExposures;
        }

        private string generateText(double cf, double act)
        {
            if ((cf == 0) && (act == 0))
                return "";

            string r = "C $" + cf.ToString("n0") + Environment.NewLine + "-A $" + act.ToString("n0") + Environment.NewLine + "= $" + (cf - act).ToString("n0");
            return r;
        }

        private string generateColor(double cf, double act)
        {
            if (Math.Abs(cf - act) <= 1)
                return "Black";
            else if (act > cf)
                return "DarkRed";
            else
                return "DarkGreen";
        }

        private double safeConvert(string s)
        {
            double d;
            if (Double.TryParse(s, out d))
                return d;
            return 0;
        }

        private double getCashFlowAmount(DataTable dtCFA, string wm, string exposure, string year, string quarter, string valuename)
        {
            foreach (DataRow dr in dtCFA.Rows)
            {
                if ((dr["Exposure"].ToString() == exposure) &&
                    (dr["Year"].ToString() == year) &&
                    (dr["Quarter"].ToString() == quarter) &&
                    (dr["ValueName"].ToString() == valuename) &&
                    (dr["WorkMatter"].ToString() == wm))
                    return (Convert.ToDouble(dr["Amount"].ToString()));
            }
            return 0d;
        }

        private DataTable getCashFlowAmountsForWM(string wm)
        {
            string sql = "SELECT * ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            //sql += "where Year = 2018 and Quarter in (1,2) ";
            // sql += "and WorkMatter = '" + wm + "' ";
            sql += "Where WorkMatter = '" + wm + "' ";
            sql += "and EndTime is null ";
            sql += "and Exposure != WorkMatter ";
            sql += "and Exposure != '' ";
            sql += "and Amount != 0 ";
            sql += "order by WorkMatter, Exposure, Year, Quarter, ValueName ";

            // APRIL 8 2018
            DataTable dtCFA = MainWindow.ourMainWindow.getData2B(MainWindow.ourMainWindow.dsn, sql);
            return dtCFA;
        }

        #endregion Exposures

        #region Notifications

        public DataTable getNotifications()
        {
            return dtNotifications;
        }

        public DataTable getNotificationsForAnalysts(bool isUnitLead, bool isTeamLead, string dept, string adidMe, string Analysts)
        {
            DataTable dtList = new DataTable();
            dtList.Columns.Add("value");
            dtList.TableName = "List";

            string[] users = Analysts.Split(',');
            foreach (string e in users)
                dtList.Rows.Add(e);

            /////// WORK AROUND //////
            string sql = "select u.DisplayName, u.AdjustedName, n.* ";
            sql += "from[CashFlow].[data].[Notifications] n ";
            sql += "left join[CashFlow].[data].Users u on u.ActiveDirectoryID = n.Analyst ";

            // Scott Feb 6 2018 changed 700 to 2000
            if (Analysts.Length > 2000)
                sql += "where (EndTime is null) ";
            else
                sql += "where (Analyst in (" + Analysts + ")) and (EndTime is null) ";

            sql += "order by DisplayName ";
            DataTable dtCF = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            // DataSet dsTables = se.runScript(ScriptEngine.environemnt.DEV, dtList, "CASH_GETNOTIFICATIONSFORANALYSTS", "OPSCONSOLE", false);
            // DataTable dtCF = dsTables.Tables[0];

            DataTable dtCopy = dtCF.Clone();
            foreach (DataRow drNot in dtCF.Rows)
            {
                if ((dept == "CLAIMS"))
                {
                    ////// If analyst or unit manager //////
                    if (!isTeamLead)
                    {
                        ////// If we are a Unit Manager and it is DENIED by the team lead we will show it as a notification //////
                        if (isUnitLead && (drNot["Status"].ToString() == MainWindow.DENIED))
                            dtCopy.ImportRow(drNot);

                        /// OLD
                        //////// If it is from us, and we are not a Unit Manager and it is UM DENIED then we will show it  //////
                        //if ((!isUnitLead) && (drNot["Analyst"].ToString() == adidMe) && (drNot["Status"].ToString() == MainWindow.UMDENIED))
                        //    dtCopy.ImportRow(drNot);

                        ////// If it is from us, and we are not a Unit Manager and it is UM DENIED then we will show it  //////
                        if ((!isUnitLead) && (drNot["Analyst"].ToString() == adidMe) && ((drNot["Status"].ToString() == MainWindow.UMDENIED) || (drNot["Status"].ToString() == MainWindow.DENIED)))
                            dtCopy.ImportRow(drNot);

                        ////// Or if it is NOT OURS and SUBMITTED we will show it as a notification //////
                        else if ((drNot["Analyst"].ToString() != adidMe) && (drNot["Status"].ToString() == MainWindow.SUBMITTED))
                            dtCopy.ImportRow(drNot);
                    }

                    ////// If we are a TEAM lead, only show UM Approved from below us //////
                    else
                    {
                        ////// Or if it is NOT OURS and SUBMITTED we will show it as a notification //////
                        if ((drNot["Analyst"].ToString() != adidMe) && (drNot["Status"].ToString() == MainWindow.UMAPPROVED))
                            dtCopy.ImportRow(drNot);
                    }
                }

                if (dept == "OGC")
                {
                    ////// If analyst //////
                    if ((!isTeamLead) && (!isUnitLead))
                    {
                        ////// If it is from us and denied by anyone  //////
                        if ((drNot["Analyst"].ToString() == adidMe) && ((drNot["Status"].ToString() == MainWindow.DENIED) || (drNot["Status"].ToString() == MainWindow.UMDENIED)))
                            dtCopy.ImportRow(drNot);
                    }

                    ////// If we are a UNIT OR TEAM lead, only show ours and submitted from below us //////
                    else
                    {
                        ////// Or if it is NOT OURS and SUBMITTED we will show it as a notification //////
                        if ((drNot["Status"].ToString() == MainWindow.SUBMITTED))
                            dtCopy.ImportRow(drNot);
                    }
                }
            }

            return dtCopy;
        }

        #endregion Notifications

        #region CashFlow

        //public void updateCFValue(string workMatter, string Exposure, string datetime, string year, string quarter, string valuename, double value)
        //{
        //    ////// STEP 2 - CREATE A BACKING TABLE TO SEND TO RSSE //////
        //    DataTable dtCashFlowEntry = new DataTable();
        //    dtCashFlowEntry.Columns.Add("WorkMatter");
        //    dtCashFlowEntry.Columns.Add("Exposure");
        //    dtCashFlowEntry.Columns.Add("PolicyNumber");
        //    dtCashFlowEntry.Columns.Add("Year", typeof(int));
        //    dtCashFlowEntry.Columns.Add("Quarter", typeof(int));
        //    dtCashFlowEntry.Columns.Add("ValueName");
        //    dtCashFlowEntry.Columns.Add("Amount", typeof(decimal));
        //    dtCashFlowEntry.Columns.Add("Operation");
        //    dtCashFlowEntry.TableName = "CashFlowEntry";

        //    dtCashFlowEntry.Rows.Add(workMatter, Exposure, "POL1", year, quarter, valuename, value, "X");
        //    DataTable dtCrap = se.runScript(dtCashFlowEntry, "CASH_CRUD_CASHFLOWENTRY", "OPSCONSOLE");
        //}

        public DataTable generateCFBackingTable()
        {
            DataTable dtCashFlowEntry = new DataTable();
            dtCashFlowEntry.Columns.Add("WorkMatter");
            dtCashFlowEntry.Columns.Add("Exposure");
            dtCashFlowEntry.Columns.Add("PolicyNumber");
            dtCashFlowEntry.Columns.Add("Year", typeof(int));
            dtCashFlowEntry.Columns.Add("Quarter", typeof(int));
            dtCashFlowEntry.Columns.Add("ValueName");
            dtCashFlowEntry.Columns.Add("Amount", typeof(decimal));
            dtCashFlowEntry.Columns.Add("Operation");
            dtCashFlowEntry.TableName = "CashFlowEntry";
            return dtCashFlowEntry;
        }

        public void updateCFValues(DataTable dtCashFlowEntry)
        {
            runStoredProcedure(MainWindow.ourMainWindow.dsn, "data.sp_CRUD_CashFlowEntry", "@CFInput", dtCashFlowEntry);

            // DataTable dtCrap = se.runScript(dtCashFlowEntry, "CASH_CRUD_CASHFLOWENTRY", "OPSCONSOLE");
        }

        public void setExposureWithinOrOutOfLimits(string exposure, string withinLimits)
        {
            runStoredProcedure(MainWindow.ourMainWindow.dsn, "data.sp_SetExposureWithinOrOutsideLimites", "@Exposure", exposure, "@NewStatus", withinLimits);
        }



        public DataTable runStoredProcedure(string connString, string sproc, string dtParmName, DataTable dtBacking)
        {
            // APRIL 8 2018
            // APRIL 10 
            // return runStoredProcedure2B(connString, sproc, dtParmName, dtBacking);

            if (MainWindow.ourMainWindow.readOnly == true)
                return new DataTable();

                using (var conn = new SqlConnection(connString))
            {
                using (var cmd = new SqlCommand(sproc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue(dtParmName, dtBacking).SqlDbType = SqlDbType.Structured;
                    //cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25).Value = "TRG\\" + MainWindow.uiCurrentUser.sam;

                    // June 10 2018
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25).Value = "TRG\\" + MainWindow.uiLoggedInUser.sam;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        public DataTable runStoredProcedure2B(string connString, string sproc, string dtParmName, DataTable dtBacking)
        {
            if (MainWindow.ourMainWindow.readOnly == true)
                return new DataTable();


            MainWindow.ourMainWindow.staticCmd.CommandType = CommandType.StoredProcedure;
            MainWindow.ourMainWindow.staticCmd.CommandText = sproc;
            MainWindow.ourMainWindow.staticCmd.Parameters.Clear();
            MainWindow.ourMainWindow.staticCmd.Parameters.AddWithValue(dtParmName, dtBacking).SqlDbType = SqlDbType.Structured;
            MainWindow.ourMainWindow.staticCmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25).Value = "TRG\\" + MainWindow.uiCurrentUser.sam;

            SqlDataAdapter da = new SqlDataAdapter(MainWindow.ourMainWindow.staticCmd);
            DataSet ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }

        // SCOTT - 1/4/18
        public DataTable runStoredProcedure(string connString, string sproc, string paramName1, string value1, string paramName2 = "", string value2 = "", string paramName3 = "", string value3 = "", string paramName4 = "", string value4="")
        {
            if (MainWindow.ourMainWindow.readOnly == true)
                return new DataTable();

            using (var conn = new SqlConnection(connString))
            {
                using (var cmd = new SqlCommand(sproc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (paramName1 != "")
                        cmd.Parameters.Add(paramName1, SqlDbType.VarChar).Value = value1;
                    if (paramName2 != "")
                        cmd.Parameters.Add(paramName2, SqlDbType.VarChar).Value = value2;
                    if (paramName3 != "")
                        cmd.Parameters.Add(paramName3, SqlDbType.VarChar).Value = value3;
                    if (paramName4 != "")
                        cmd.Parameters.Add(paramName4, SqlDbType.VarChar).Value = value4;

                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25).Value = "TRG\\" + MainWindow.uiCurrentUser.sam;

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataSet ds = new DataSet();
                    da.Fill(ds);
                    return ds.Tables[0];
                }
            }
        }

        public DataTable loadCashFlowForExposure(string exposure)
        {

            if (!exposure.StartsWith("'"))
                exposure = "'" + exposure + "'";

            string sql = "SELECT [WorkMatter],[Exposure],[PolicyNumber],[Year],[Quarter],[ValueName],[Amount],[StartUser],[StartTime],[EndUser],[EndTime] ";
            sql += "FROM [CashFlow].[data].[CashFlowEntry] ";
            sql += "WHERE([Exposure] in (" + exposure + ")) and(EndTime is null)";
            DataTable dtCF = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            //DataTable dtList = new DataTable();
            //dtList.Columns.Add("Exposure");
            //dtList.TableName = "Values";

            //string[] exposures = exposure.Split(',');
            //foreach (string e in exposures)
            //    dtList.Rows.Add(e);

            //DataSet dsTables = se.runScript(ScriptEngine.environemnt.DEV, dtList, "CASH_GETCASHFLOWFOREXP", "OPSCONSOLE", false);
            //DataTable dtCF = dsTables.Tables[0];
            return dtCF;
        }

        public DataTable loadCashFlowForWM(string workmatter)
        {
            string sql = "SELECT TOP 1000 [WorkMatter],[Exposure],[PolicyNumber],[Year],[Quarter],[ValueName],[Amount],[StartUser],[StartTime],[EndUser],[EndTime] ";
            sql += "FROM[CashFlow].[data].[CashFlowEntry] ";
            sql += "WHERE([WorkMatter] = '" + workmatter + "') and(Exposure != '" + workmatter + "')  and(EndTime is null)";
            DataTable dtCF = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

            //DataSet dsTables = se.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "CASH_GETCASHFLOWFORWM", "OPSCONSOLE", false, "@WorkMatter", workmatter);
            //DataTable dtCF = dsTables.Tables[0];
            return dtCF;
        }

        public void rollupExposuresCFIntoWorkMatter(string workmatter)
        {
            DataTable dtCF = loadCashFlowForWM(workmatter);
            Dictionary<string, double> dictRollup = new Dictionary<string, double>();

            ////// STEP 0 - TEMPORARY - APRIL 11 2018 - REMOVE !!! TEMP! //////
            // Added DefExp June 7 2018
            string SQL = "delete from [CashFlow].[data].[CashFlowEntry] where Exposure='" + workmatter + "' and ValueName in ('DefExpIn', 'DefExpOut', 'DefExp')";
            MainWindow.ourMainWindow.executeSQL(MainWindow.ourMainWindow.dsn, SQL);

            ////// STEP 1 - Rollup all the values into buckets of unique Year/Quarter/Value //////
            foreach (DataRow dr in dtCF.Rows)
            {
                string key = dr["Year"].ToString() + "." + dr["Quarter"].ToString() + "." + dr["ValueName"].ToString();
                double value = Convert.ToDouble(dr["Amount"].ToString());

                if (dictRollup.ContainsKey(key))
                    dictRollup[key] = dictRollup[key] + value;
                else
                    dictRollup.Add(key, value);
            }

            ////// STEP 2 - CREATE A BACKING TABLE TO SEND TO RSSE //////
            DataTable dtCashFlowEntry = new DataTable();
            dtCashFlowEntry.Columns.Add("WorkMatter");
            dtCashFlowEntry.Columns.Add("Exposure");
            dtCashFlowEntry.Columns.Add("PolicyNumber");
            dtCashFlowEntry.Columns.Add("Year", typeof(int));
            dtCashFlowEntry.Columns.Add("Quarter", typeof(int));
            dtCashFlowEntry.Columns.Add("ValueName");
            dtCashFlowEntry.Columns.Add("Amount", typeof(decimal));
            dtCashFlowEntry.Columns.Add("Operation");
            dtCashFlowEntry.TableName = "CashFlowEntry";

            ///// STEP 3 - Update all the values //////
            foreach (KeyValuePair<string, double> kvp in dictRollup)
            {
                string[] pieces = kvp.Key.Split(new char[] { '.' });
                dtCashFlowEntry.Rows.Add(workmatter, workmatter, "POL1", pieces[0], pieces[1], pieces[2], kvp.Value, "X");
            }

            ////// STEP 4 - WRITE IT OUT //////
            runStoredProcedure(MainWindow.ourMainWindow.dsn, "data.sp_CRUD_CashFlowEntry", "@CFInput", dtCashFlowEntry);


            //DataTable dtCrap = se.runScript(dtCashFlowEntry, "CASH_CRUD_CASHFLOWENTRY", "OPSCONSOLE");
        }

        #endregion CashFlow

        #region Helper Functions

        public string DataTableToCommaSeparatedList(DataTable dt, string column, string quoteChar = "")
        {
            string retval = "";
            foreach (DataRow dr in dt.Rows)
                retval += quoteChar + dr[column].ToString() + quoteChar + ",";

            retval = retval.TrimEnd();
            retval = retval.TrimEnd(new char[] { ',' });
            return retval;
        }

        public void addError(string adid, string adjname, string feature, string stackTrace)
        {
            string sproc = "data.sp_AddError";

            using (var conn = new SqlConnection(MainWindow.ourMainWindow.dsn))
            {
                using (var cmd = new SqlCommand(sproc, conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ADID", SqlDbType.VarChar, 60).Value = MainWindow.uiCurrentUser.adid;
                    cmd.Parameters.Add("@AdjName", SqlDbType.VarChar, 60).Value = MainWindow.uiCurrentUser.name;
                    cmd.Parameters.Add("@Feature", SqlDbType.VarChar, 25).Value = MainWindow.currentFunction;
                    cmd.Parameters.Add("@StackTrace", SqlDbType.VarChar, -1).Value = stackTrace;
                    cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 25).Value = "TRG\\" + MainWindow.uiCurrentUser.sam;

                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return;
                }
            }

            //se.runScript(ScriptEngine.environemnt.DEV, new DataTable(), "CASH_ADDERROR", "OPSCONSOLE", false, "@ADID", adid, "@AdjName", adjname, "@Feature", feature, "@StackTrace", stackTrace);
        }

        #endregion Helper Functions
    }
}