using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;
using System.Data;
using System.Drawing;
using System.Collections;
using System.IO;
using System.Web.Hosting;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Configuration;

namespace CommWeb
{
    public partial class _Default : System.Web.UI.Page
    {
        //string connstring = "Data Source=DEVSQL4;Initial Catalog=ReinsReporting;Trusted_Connection=True;";
        // string connstring = "Data Source=OPSDW;Initial Catalog=Commutation;Trusted_Connection=True;";
        string connstring = "";
        string fullname = "";
        static DataTable dtAD = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            connstring = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            getUserInfo();
            //fullname = "Scott";

            var controlName = Request.Params.Get("__EVENTTARGET");
            var argument = Request.Params.Get("__EVENTARGUMENT");
            if (controlName == "Table1" && argument == "Click")
            {
                btnClearCommID.Visible = true;

                string parameter = hdnfldVariable.Value;
                Session["selected"] = parameter;

                btnClearCommID.Text = "Remove CommID from " + parameter;
                if (lbSelectedCommID.SelectedItem == null)
                {
                    btnSetCommID.Text = "";
                    btnSetCommID.Visible = false;
                }
                else
                {
                    string selcom = ((ListItem)lbSelectedCommID.SelectedItem).ToString();
                    Session["commVAL"] = ((ListItem)lbSelectedCommID.SelectedItem).Value;
                    Session["commTEXT"] = ((ListItem)lbSelectedCommID.SelectedItem).Text;
                    btnSetCommID.Text = "Set " + parameter + " to CommID " + selcom;
                    btnSetCommID.Visible = true;
                }
            }

            if (Session["tabledata"] != null)
            {
                FillTableFromTableData((System.Data.DataTable)Session["tabledata"]);
            }



            if (!Page.IsPostBack)
            {
                lblUser.Text = fullname;
                if (fullname == "")
                {
                    lblUserLegend.Text = "Unknown User";
                    return;
                }

                LabelWait.Text = "";
                //btnExposure.Attributes.Add("onclick", ClientScript.GetPostBackEventReference(btnExposure, "") + ";document.getElementById('FeaturedContent_LabelWait').innerText = 'Please Wait...';FeaturedContent_LabelWait.innerText = 'Please Wait...';");

                SortedList sorted = new SortedList();

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    //string sqlcomm = "select distinct CommID, CommName from [ReinsReporting].[dbo].[_CommutationID] order by CommID";
                    string sqlcomm = "select distinct CommID, CommName from [_CommutationID] order by CommID";
                    using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                        while (reader.Read())
                        {
                            string ID = reader["CommID"].ToString();
                            if (ID != "")
                            {
                                string name = reader["CommName"].ToString();
                                sorted.Add(ID + ((name == "") ? "" : (" - " + name)), ID);
                            }
                        }

                        lbAvailableCommID.Items.Clear();

                        foreach (String key in sorted.Keys)
                        {
                            lbAvailableCommID.Items.Add(new ListItem(key, sorted[key].ToString()));
                        }

                    }
                }

            }
        }

        protected void btnAddCommID_Click(object sender, EventArgs e)
        {
            if (lbAvailableCommID.SelectedItem != null)
            {
                System.Collections.SortedList sorted = new SortedList();

                foreach (ListItem ll in lbSelectedCommID.Items)
                    sorted.Add(ll.Text, ll.Value);

                ListItem li = lbAvailableCommID.SelectedItem;
                sorted.Add(li.Text, li.Value);

                lbAvailableCommID.Items.Remove(li);
                lbSelectedCommID.Items.Clear();

                foreach (String key in sorted.Keys)
                    lbSelectedCommID.Items.Add(new ListItem(key, sorted[key].ToString()));
            }


        }

        protected void btnRemoveCommID_Click(object sender, EventArgs e)
        {
            if (lbSelectedCommID.SelectedItem != null)
            {
                System.Collections.SortedList sorted = new SortedList();

                foreach (ListItem ll in lbAvailableCommID.Items)
                    sorted.Add(ll.Text, ll.Value);

                ListItem li = lbSelectedCommID.SelectedItem;
                sorted.Add(li.Text, li.Value);

                lbSelectedCommID.Items.Remove(li);
                lbAvailableCommID.Items.Clear();

                foreach (String key in sorted.Keys)
                    lbAvailableCommID.Items.Add(new ListItem(key, sorted[key].ToString()));
            }
        }

        protected void btnExposure_Click(object sender, EventArgs e)
        {
            if (fullname == "")
                return;

            if (lbSelectedCommID.Items.Count <= 0)
                return;

            Row xrow = null;

            //string localFile = Guid.NewGuid().ToString() + ".xlsx";
            //string docName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, Guid.NewGuid().ToString() + ".xlsx");

            //Microsoft.Office.Interop.Excel.Application xlApp;
            //Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
            //Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
            //object misValue = System.Reflection.Missing.Value;
            //xlApp = new Microsoft.Office.Interop.Excel.Application();
            // xlWorkBook = xlApp.Workbooks.Open(@"C:\Scott\Commutations\Exposure-Blank.xlsx", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            //xlWorkBook = xlApp.Workbooks.Open(Path.Combine(HostingEnvironment.ApplicationPhysicalPath , @"Exposure-Blank.xlsx"), 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
            //xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
            string fn = "";

            //string docName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, localFile);
            string docName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, Guid.NewGuid().ToString() + ".xlsx");

            File.Delete(docName);
            File.Copy(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"Exposure-Blank.xlsx"), docName);

            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
            {
                ////// Get the SharedStringTablePart. If it does not exist, create a new one.
                SharedStringTablePart shareStringPart;
                if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                else
                    shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();

                ////// Grab the one and only worksheet //////
                WorksheetPart worksheetPart = spreadSheet.WorkbookPart.WorksheetParts.First();

                string sql = "SELECT [PARTY_KEY]";
                sql += "     ,[SYSTEM]";
                sql += "      ,[BUSINESS_TYPE]";
                sql += "      ,[PORTFOLIO]";
                sql += "      ,[WORKMATTER_NO]";
                sql += "      ,[ACCOUNT_NAME]";
                sql += "      ,[WORKMATTER_DESCRIPTION]";
                sql += "      ,[WORKMATTER_STATUS]";
                sql += "      ,[WORKMATTER_TYPE]";
                sql += "      ,[SPECIAL_TRACKING_GROUP]";
                sql += "      ,[ISSUING_COMPANY]";
                sql += "      ,[POLICY_NO]";
                sql += "      ,[POLICY_EFF_DATE]";
                sql += "      ,[DATE_OF_LOSS]";
                sql += "      ,[CLAIM_ID]";
                sql += "      ,[RELATED_CLAIM_ID]";
                sql += "      ,[CONTRACT_NO]";
                sql += "      ,[CONTRACT_SECTION]";
                sql += "      ,[CONTRACT_EFF_DATE]";
                sql += "      ,[CONTRACT_NAME]";
                sql += "      ,[CONTRACT_TYPE]";
                sql += "      ,[CONTRACT_ATTACHMENT]";
                sql += "      ,[CONTRACT_LIMIT]";
                sql += "      ,[SECTION_REF_NO]";
                sql += "      ,[OLD_TTY_REF_NO]";
                sql += "      ,[PARTY_ID]";
                sql += "      ,[PARTY_NAME]";
                sql += "      ,[OLD_REINS_REF_NO]";
                sql += "      ,[AFFILIATE]";
                sql += "      ,[BROKER_ID]";
                sql += "      ,[BROKER_NAME]";
                sql += "      ,[POOL_ID]";
                sql += "      ,[POOL_NAME]";
                sql += "      ,[BALANCE]";
                sql += "      ,[CURRENT_BALANCE_UNBILLED]";
                sql += "      ,[CURRENT_BALANCE_UNDER_THRESHOLD]";
                sql += "      ,[CASE_RESERVE]";
                sql += "      ,[IBNR_LOW]";
                sql += "      ,[IBNR_CENTRAL]";
                sql += "      ,[IBNR_HIGH]";
                sql += "      ,[CRED_PRV_PCT]";
                sql += "      ,[DISPUTE_PRV_PCT]";
                sql += "      ,[DISPUTE_LEVEL]";
                sql += "      ,[CommID]";
                sql += "      ,[CommName]";
                //sql += "  FROM [ReinsReporting].[dbo].[CommutationExposureView]";
                sql += "  FROM [CommutationExposureView]";

                string where = "";

                buildWhereListFromSelected(ref where, ref fn);
                sql += "  WHERE (" + where + ")";

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string sqlcomm = sql;
                    using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 60 * 10;
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                        uint row = 2;
                        while (reader.Read())
                        {
                            xrow = null;
                            string strPARTY_KEY = reader["PARTY_KEY"].ToString();
                            string strSYSTEM = reader["SYSTEM"].ToString();
                            string strBUSINESS_TYPE = reader["BUSINESS_TYPE"].ToString();
                            string strPORTFOLIO = reader["PORTFOLIO"].ToString();
                            string strWORKMATTER_NO = reader["WORKMATTER_NO"].ToString();
                            string strACCOUNT_NAME = reader["ACCOUNT_NAME"].ToString();
                            string strWORKMATTER_DESCRIPTION = reader["WORKMATTER_DESCRIPTION"].ToString();
                            string strWORKMATTER_STATUS = reader["WORKMATTER_STATUS"].ToString();
                            string strWORKMATTER_TYPE = reader["WORKMATTER_TYPE"].ToString();
                            string strSPECIAL_TRACKING_GROUP = reader["SPECIAL_TRACKING_GROUP"].ToString();
                            string strISSUING_COMPANY = reader["ISSUING_COMPANY"].ToString();
                            string strPOLICY_NO = reader["POLICY_NO"].ToString();
                            string strPOLICY_EFF_DATE = reader["POLICY_EFF_DATE"].ToString();
                            string strDATE_OF_LOSS = reader["DATE_OF_LOSS"].ToString();
                            string strCLAIM_ID = reader["CLAIM_ID"].ToString();
                            string strRELATED_CLAIM_ID = reader["RELATED_CLAIM_ID"].ToString();
                            string strCONTRACT_NO = reader["CONTRACT_NO"].ToString();
                            string strCONTRACT_SECTION = reader["CONTRACT_SECTION"].ToString();
                            string strCONTRACT_EFF_DATE = reader["CONTRACT_EFF_DATE"].ToString();
                            string strCONTRACT_NAME = reader["CONTRACT_NAME"].ToString();
                            string strCONTRACT_TYPE = reader["CONTRACT_TYPE"].ToString();
                            string strCONTRACT_ATTACHMENT = reader["CONTRACT_ATTACHMENT"].ToString();
                            string strCONTRACT_LIMIT = reader["CONTRACT_LIMIT"].ToString();
                            string strSECTION_REF_NO = reader["SECTION_REF_NO"].ToString();
                            string strOLD_TTY_REF_NO = reader["OLD_TTY_REF_NO"].ToString();
                            string strPARTY_ID = reader["PARTY_ID"].ToString();
                            string strPARTY_NAME = reader["PARTY_NAME"].ToString();
                            string strOLD_REINS_REF_NO = reader["OLD_REINS_REF_NO"].ToString();
                            string strAFFILIATE = reader["AFFILIATE"].ToString();
                            string strBROKER_ID = reader["BROKER_ID"].ToString();
                            string strBROKER_NAME = reader["BROKER_NAME"].ToString();
                            string strPOOL_ID = reader["POOL_ID"].ToString();
                            string strPOOL_NAME = reader["POOL_NAME"].ToString();
                            string strBALANCE = reader["BALANCE"].ToString();
                            string strCURRENT_BALANCE_UNBILLED = reader["CURRENT_BALANCE_UNBILLED"].ToString();
                            string strCURRENT_BALANCE_UNDER_THRESHOLD = reader["CURRENT_BALANCE_UNDER_THRESHOLD"].ToString();
                            string strCASE_RESERVE = reader["CASE_RESERVE"].ToString();
                            string strIBNR_LOW = reader["IBNR_LOW"].ToString();
                            string strIBNR_CENTRAL = reader["IBNR_CENTRAL"].ToString();
                            string strIBNR_HIGH = reader["IBNR_HIGH"].ToString();
                            string strCRED_PRV_PCT = reader["CRED_PRV_PCT"].ToString();
                            string strDISPUTE_PRV_PCT = reader["DISPUTE_PRV_PCT"].ToString();
                            string strDISPUTE_LEVEL = reader["DISPUTE_LEVEL"].ToString();
                            string strCommID = reader["CommID"].ToString();
                            string strCommName = reader["CommName"].ToString();

                            setTextCell(worksheetPart, shareStringPart, "A", row, strPARTY_KEY, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "B", row, strSYSTEM, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "C", row, strBUSINESS_TYPE, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "D", row, strPORTFOLIO, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "E", row, strWORKMATTER_NO, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "F", row, strACCOUNT_NAME, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "G", row, strWORKMATTER_DESCRIPTION, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "H", row, strWORKMATTER_STATUS, ref xrow);    // ok to here
                            setTextCell(worksheetPart, shareStringPart, "I", row, strWORKMATTER_TYPE, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "J", row, strSPECIAL_TRACKING_GROUP, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "K", row, strISSUING_COMPANY, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "L", row, strPOLICY_NO, ref xrow);
                            setDateCell(worksheetPart, shareStringPart, "M", row, strPOLICY_EFF_DATE, ref xrow);
                            setDateCell(worksheetPart, shareStringPart, "N", row, strDATE_OF_LOSS, ref xrow); // ok to here
                            setTextCell(worksheetPart, shareStringPart, "O", row, strCLAIM_ID, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "P", row, strRELATED_CLAIM_ID, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "Q", row, strCONTRACT_NO, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "R", row, strCONTRACT_SECTION, ref xrow);
                            setDateCell(worksheetPart, shareStringPart, "S", row, strCONTRACT_EFF_DATE, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "T", row, strCONTRACT_NAME, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "U", row, strCONTRACT_TYPE, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "V", row, strCONTRACT_ATTACHMENT, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "W", row, strCONTRACT_LIMIT, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "X", row, strSECTION_REF_NO, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "Y", row, strOLD_TTY_REF_NO, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "Z", row, strPARTY_ID, ref xrow);  // OK to here
                            setTextCell(worksheetPart, shareStringPart, "AA", row, strPARTY_NAME, ref xrow);
                            //setTextCell(worksheetPart, shareStringPart, "AA", row, "Dog", ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "AB", row, strOLD_REINS_REF_NO, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "AC", row, strAFFILIATE, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "AD", row, strBROKER_ID, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "AE", row, strBROKER_NAME, ref xrow);  // BAD!
                            setTextCell(worksheetPart, shareStringPart, "AF", row, strPOOL_ID, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "AG", row, strPOOL_NAME, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "AH", row, strBALANCE, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "AI", row, strCURRENT_BALANCE_UNBILLED, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "AJ", row, strCURRENT_BALANCE_UNDER_THRESHOLD, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "AK", row, strCASE_RESERVE, ref xrow); // BAD HERE!!
                            setNumbCell(worksheetPart, shareStringPart, "AL", row, strIBNR_LOW, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "AM", row, strIBNR_CENTRAL, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "AN", row, strIBNR_HIGH, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "AO", row, strCRED_PRV_PCT, ref xrow);
                            setNumbCell(worksheetPart, shareStringPart, "AP", row, strDISPUTE_PRV_PCT, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "AQ", row, strDISPUTE_LEVEL, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "AR", row, strCommID, ref xrow);
                            setTextCell(worksheetPart, shareStringPart, "AS", row, strCommName, ref xrow);

                            //xlWorkSheet.get_Range("A" + row.ToString()).Value2 = strPARTY_KEY;
                            //xlWorkSheet.get_Range("B" + row.ToString()).Value2 = strSYSTEM);
                            //xlWorkSheet.get_Range("C" + row.ToString()).Value2 = strBUSINESS_TYPE;
                            //xlWorkSheet.get_Range("D" + row.ToString()).Value2 = strPORTFOLIO;
                            //xlWorkSheet.get_Range("E" + row.ToString()).Value2 = strWORKMATTER_NO;
                            //xlWorkSheet.get_Range("F" + row.ToString()).Value2 = strACCOUNT_NAME;
                            //xlWorkSheet.get_Range("G" + row.ToString()).Value2 = strWORKMATTER_DESCRIPTION;
                            //xlWorkSheet.get_Range("H" + row.ToString()).Value2 = strWORKMATTER_STATUS;
                            //xlWorkSheet.get_Range("I" + row.ToString()).Value2 = strWORKMATTER_TYPE;
                            //xlWorkSheet.get_Range("J" + row.ToString()).Value2 = strSPECIAL_TRACKING_GROUP;
                            //xlWorkSheet.get_Range("K" + row.ToString()).Value2 = strISSUING_COMPANY;
                            //xlWorkSheet.get_Range("L" + row.ToString()).Value2 = strPOLICY_NO;
                            //xlWorkSheet.get_Range("M" + row.ToString()).Value2 = strPOLICY_EFF_DATE;
                            //xlWorkSheet.get_Range("N" + row.ToString()).Value2 = strDATE_OF_LOSS;
                            //xlWorkSheet.get_Range("O" + row.ToString()).Value2 = strCLAIM_ID;
                            //xlWorkSheet.get_Range("P" + row.ToString()).Value2 = strRELATED_CLAIM_ID;
                            //xlWorkSheet.get_Range("Q" + row.ToString()).Value2 = strCONTRACT_NO;
                            //xlWorkSheet.get_Range("R" + row.ToString()).Value2 = strCONTRACT_SECTION;
                            //xlWorkSheet.get_Range("S" + row.ToString()).Value2 = strCONTRACT_EFF_DATE;
                            //xlWorkSheet.get_Range("T" + row.ToString()).Value2 = strCONTRACT_NAME;
                            //xlWorkSheet.get_Range("U" + row.ToString()).Value2 = strCONTRACT_TYPE;
                            //xlWorkSheet.get_Range("V" + row.ToString()).Value2 = strCONTRACT_ATTACHMENT;
                            //xlWorkSheet.get_Range("W" + row.ToString()).Value2 = strCONTRACT_LIMIT;
                            //xlWorkSheet.get_Range("X" + row.ToString()).Value2 = strSECTION_REF_NO;
                            //xlWorkSheet.get_Range("Y" + row.ToString()).Value2 = strOLD_TTY_REF_NO;
                            //xlWorkSheet.get_Range("Z" + row.ToString()).Value2 = strPARTY_ID;
                            //xlWorkSheet.get_Range("AA" + row.ToString()).Value2 = strPARTY_NAME;
                            //xlWorkSheet.get_Range("AB" + row.ToString()).Value2 = strOLD_REINS_REF_NO;
                            //xlWorkSheet.get_Range("AC" + row.ToString()).Value2 = strAFFILIATE;
                            //xlWorkSheet.get_Range("AD" + row.ToString()).Value2 = strBROKER_ID;
                            //xlWorkSheet.get_Range("AE" + row.ToString()).Value2 = strBROKER_NAME;
                            //xlWorkSheet.get_Range("AF" + row.ToString()).Value2 = strPOOL_ID;
                            //xlWorkSheet.get_Range("AG" + row.ToString()).Value2 = strPOOL_NAME;
                            //xlWorkSheet.get_Range("AH" + row.ToString()).Value2 = strBALANCE;
                            //xlWorkSheet.get_Range("AI" + row.ToString()).Value2 = strCURRENT_BALANCE_UNBILLED;
                            //xlWorkSheet.get_Range("AJ" + row.ToString()).Value2 = strCURRENT_BALANCE_UNDER_THRESHOLD;
                            //xlWorkSheet.get_Range("AK" + row.ToString()).Value2 = strCASE_RESERVE;
                            //xlWorkSheet.get_Range("AL" + row.ToString()).Value2 = strIBNR_LOW;
                            //xlWorkSheet.get_Range("AM" + row.ToString()).Value2 = strIBNR_CENTRAL;
                            //xlWorkSheet.get_Range("AN" + row.ToString()).Value2 = strIBNR_HIGH;
                            //xlWorkSheet.get_Range("AO" + row.ToString()).Value2 = strCRED_PRV_PCT;
                            //xlWorkSheet.get_Range("AP" + row.ToString()).Value2 = strDISPUTE_PRV_PCT;
                            //xlWorkSheet.get_Range("AQ" + row.ToString()).Value2 = strDISPUTE_LEVEL;
                            //xlWorkSheet.get_Range("AR" + row.ToString()).Value2 = strCommID;
                            //xlWorkSheet.get_Range("AS" + row.ToString()).Value2 = strCommName;
                            row++;
                        }


                    }

                    //xlWorkBook.SaveAs(Path.Combine(HostingEnvironment.ApplicationPhysicalPath ,@"Exposure.xlsx"));
                    //xlWorkBook.Close(true, misValue, misValue);
                    //xlApp.Quit();
                }
                worksheetPart.Worksheet.Save();
            }


            ////// Send down the file //////
            Response.ContentType = "application/excel";
            Response.AppendHeader("content-disposition", "attachment; filename=" + @"Exposure-" + fn + ".xlsx");
            Response.TransmitFile(docName);
            Response.End();
        }

        private void setTextCell(WorksheetPart worksheetPart, SharedStringTablePart shareStringPart, string column, uint row, string text, ref Row xrow)
        {
            //if (text == "")
            //    return;

            if (text == null)
                return;

            Cell cell = InsertCellInWorksheet(column, row, worksheetPart, ref xrow);
            //int index = InsertSharedStringItem(text, shareStringPart);
            //cell.CellValue = new CellValue(index.ToString());
            //cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            cell.CellValue = new CellValue(text.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
        }

        private void setDateCell(WorksheetPart worksheetPart, SharedStringTablePart shareStringPart, string column, uint row, string date, ref Row xrow)
        {
            //if (text == "")
            //    return;
            string text = "";

            DateTime dt = new DateTime();
            if (DateTime.TryParse(date, out dt))
                text = dt.ToString("yyyy-MM-dd");

            Cell cell = InsertCellInWorksheet(column, row, worksheetPart, ref xrow);
            //int index = InsertSharedStringItem(text, shareStringPart);
            //cell.CellValue = new CellValue(index.ToString());
            //cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            cell.CellValue = new CellValue(text.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.String);
        }

        private void setNumbCell(WorksheetPart worksheetPart, SharedStringTablePart shareStringPart, string column, uint row, string number, ref Row xrow)
        {
            double d = 0d;
            double.TryParse(number, out d);

            Cell cell = InsertCellInWorksheet(column, row, worksheetPart, ref xrow);
            cell.CellValue = new CellValue(d.ToString());
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);
        }


        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
        // If the cell already exists, returns it. 
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart, ref Row row)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            //Row row;
 
            //if (sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).Count() != 0)
            //{
            //    row = sheetData.Elements<Row>().Where(r => r.RowIndex == rowIndex).First();
           // }
            //else
            if (row == null)
            {
                row = new Row() { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            // If there is not a cell with the specified column name, insert one.  
//            if (row.Elements<Cell>().Where(c => c.CellReference.Value == columnName + rowIndex).Count() > 0)
//            {
//                return row.Elements<Cell>().Where(c => c.CellReference.Value == cellReference).First();
//            }
//            else
            {
                // Cells must be in sequential order according to CellReference. Determine where to insert the new cell.
                Cell refCell = null;
                foreach (Cell cell in row.Elements<Cell>())
                {
                    // scott if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    if (false)
                    {
                        refCell = cell;
                        break;
                    }
                }

                Cell newCell = new Cell() { CellReference = cellReference };
                row.InsertBefore(newCell, refCell);

                // scott 6/22
                // worksheet.Save();
                return newCell;
            }
        }

        // Given text and a SharedStringTablePart, creates a SharedStringItem with the specified text 
        // and inserts it into the SharedStringTablePart. If the item already exists, returns its index.
        private static int InsertSharedStringItem(string text, SharedStringTablePart shareStringPart)
        {
            // If the part does not contain a SharedStringTable, create one.
            if (shareStringPart.SharedStringTable == null)
            {
                shareStringPart.SharedStringTable = new SharedStringTable();
            }

            int i = 0;

            // Iterate through all the items in the SharedStringTable. If the text already exists, return its index.
            foreach (SharedStringItem item in shareStringPart.SharedStringTable.Elements<SharedStringItem>())
            {
                if (item.InnerText == text)
                {
                    return i;
                }

                i++;
            }

            // The text does not exist in the part. Create the SharedStringItem and return its index.
            shareStringPart.SharedStringTable.AppendChild(new SharedStringItem(new DocumentFormat.OpenXml.Spreadsheet.Text(text)));
            shareStringPart.SharedStringTable.Save();

            return i;
        }



        private void buildWhereListFromSelected(ref string where, ref string fn)
        {
            where = "";
            fn = "";
            foreach (ListItem ll in lbSelectedCommID.Items)
            {
                if (where != "")
                {
                    where += " or ";
                    fn += "-";
                }
                where += "CommID = '" + ll.Value + "'";
                fn += ll.Value;
            }
        }

        protected void btnViewCommIDS_Click(object sender, EventArgs e)
        {
            FindCommIDS("");
        }

        private void FindCommIDS(string searchString)
        {
            if (fullname == "")
                return;

            string where = "";
            string human = "";
            string sanitized = searchString;

            if (searchString != "")
            {
                where = "PARTY_KEY like '%" + sanitized + "%' or ";
                where += "  Sapiens_Internal_Reco_CD like '%" + sanitized + "%' or ";
                where += "  Reco_Desc like '%" + sanitized + "%' or ";
                where += "  FEIN_Id like '%" + sanitized + "%' or ";
                where += "  NAIC_Cd like '%" + sanitized + "%' or ";
                where += "  Portfolio_Cd like '%" + sanitized + "%' or ";
                where += "  System_Of_Record like '%" + sanitized + "%' or ";
                where += "  Party_ID like '%" + sanitized + "%' ";
                lblGridHeader.Text = "Listing for Comm ID's where data contains: " + searchString;
            }

            else
            {
                buildWhereListFromSelected(ref where, ref human);
                lblGridHeader.Text = "Listing for Comm ID's " + human.Replace("-", ", ");
            }

            string sql = "SELECT TOP 1000 [Party_Key]";
            sql += "      ,[Portfolio_Cd]";
            sql += "      ,[System_Of_Record]";
            sql += "      ,[Party_ID]";
            sql += "      ,[Affiliate]";
            sql += "      ,[Sapiens_Internal_Reco_Cd]";
            sql += "      ,[Reco_Desc]";
            sql += "      ,[FEIN_Id]";
            sql += "      ,[NAIC_Cd]";
            sql += "      ,[Domicile_State_Cd]";
            sql += "      ,[Domicile_Country_Cd]";
            sql += "      ,[Credit_Provision_Percent]";
            sql += "      ,[CommID]";
            sql += "      ,[CommName]";
            //sql += "  FROM [ReinsReporting].[dbo].[_CommutationID]";
            sql += "  FROM [_CommutationID]";
            sql += "  WHERE (" + where + ")";

            System.Data.DataTable dtSelectedCommID = new System.Data.DataTable();

            using (SqlDataAdapter dataAdapter = new SqlDataAdapter(sql, connstring))
            {
                DataSet dataSet = new DataSet();
                dataAdapter.Fill(dataSet);
                dtSelectedCommID = dataSet.Tables[0];
            }

            Session["tabledata"] = dtSelectedCommID;
            FillTableFromTableData(dtSelectedCommID);
        }

        private void FillTableFromTableData(System.Data.DataTable dtSelectedCommID)
        {
            Table1.Rows.Clear();
            TableRow tr = new TableRow();
            addColumnToRow(tr, "PARTY_KEY", highlight: true);
            addColumnToRow(tr, "Portfolio_Cd", highlight: true);
            addColumnToRow(tr, "System_Of_Record", highlight: true);
            addColumnToRow(tr, "Party_ID", highlight: true);
            addColumnToRow(tr, "Affiliate", highlight: true);
            addColumnToRow(tr, "Sapiens_Internal_Reco_Cd", highlight: true);
            addColumnToRow(tr, "Reco_Desc", highlight: true);
            addColumnToRow(tr, "FEIN_Id", highlight: true);
            addColumnToRow(tr, "NAIC_Cd", highlight: true);
            addColumnToRow(tr, "Domicile_State_Cd", highlight: true);
            addColumnToRow(tr, "Domicile_Country_Cd", highlight: true);
            addColumnToRow(tr, "Credit_Provision_Percent", highlight: true);
            addColumnToRow(tr, "CommID", highlight: true);
            addColumnToRow(tr, "CommName", highlight: true);
            Table1.Rows.Add(tr);

            string selected = "";
            if (Session["selected"] != null)
                selected = (string) Session["selected"];

            foreach (DataRow dr in dtSelectedCommID.Rows)
            {
                bool isselected = false;
                if (dr["PARTY_KEY"].ToString() == selected)
                    isselected = true;

                tr = new TableRow();
                addColumnToRow(tr, dr["PARTY_KEY"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["Portfolio_Cd"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["System_Of_Record"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["Party_ID"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["Affiliate"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["Sapiens_Internal_Reco_Cd"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["Reco_Desc"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["FEIN_Id"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["NAIC_Cd"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["Domicile_State_Cd"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["Domicile_Country_Cd"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["Credit_Provision_Percent"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["CommID"].ToString(), highlight: false, selected: isselected);
                addColumnToRow(tr, dr["CommName"].ToString(), highlight: false, selected: isselected);
                Table1.Rows.Add(tr);
            }
        }

        private void addColumnToRow(TableRow tr, string text, bool highlight = false, bool selected = false)
        {
            TableCell tc1 = new TableCell();

            if (text == "")
                text = "&nbsp;";

            tc1.Text = text;
            if (highlight)
            {
                tc1.BackColor = System.Drawing.Color.FromArgb(230,227,214);  // 33, 137, 163
                tc1.ForeColor = System.Drawing.Color.Black;
            }
            if (selected)
            {
                tc1.BackColor = System.Drawing.Color.FromArgb(160, 208, 255);
                tc1.ForeColor = System.Drawing.Color.Black;
            }

            tr.Cells.Add(tc1);

            tr.Attributes["onmouseover"] = "highlight(this, true);";
            tr.Attributes["onmouseout"] = "highlight(this, false);";
            tr.Attributes["onclick"] = "select(this);";

            Table1.Rows.Add(tr);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (txtSearchBox.Text == "")
                return;

            FindCommIDS(txtSearchBox.Text);

        }

        protected void lbSelectedCommID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lbSelectedCommID.SelectedItem == null)
            {
                btnSetCommID.Text = "";
                btnSetCommID.Visible = false;
            }
            else
            {
                string selcom = ((ListItem)lbSelectedCommID.SelectedItem).ToString();
                Session["commVAL"] = ((ListItem)lbSelectedCommID.SelectedItem).Value;
                Session["commTEXT"] = ((ListItem)lbSelectedCommID.SelectedItem).Text;
                btnSetCommID.Text = "Set " + ((string)Session["selected"]) + " to CommID " + selcom;
                btnSetCommID.Visible = true;
            }
        }

        protected void btnSetCommID_Click(object sender, EventArgs e)
        {
            if (Session["selected"] == null)
                return;

            string selected = ((string)Session["selected"]);
            if (selected == "")
                return;

            //string sql = "Update [ReinsReporting].[dbo].[_CommutationID] ";
            string sql = "Update [_CommutationID] ";
            sql += "set [CommID]='" + ((string)Session["commVAL"]) + "', ";
            sql += "[CommName]='" + ((string) Session["commTEXT"]).Substring(((string) Session["commVAL"]).Length + 3) + "' ";
            sql += "where Party_Key='" + selected + "'";

            executeSQL(sql);
        }

        protected void btnClearCommID_Click(object sender, EventArgs e)
        {
            if (Session["selected"] == null)
                return;

            //string sql = "Update [ReinsReporting].[dbo].[_CommutationID] ";
            string sql = "Update [_CommutationID] ";
            sql += "set [CommID]='', ";
            sql += "[CommName]='' ";
            sql += "where Party_Key='" + ((string)Session["selected"]) + "'";

            executeSQL(sql);
        }

        public void executeSQL(string sql)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    conn.Open();
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Error processing SQL" + Environment.NewLine + Environment.NewLine + ex.ToString() + Environment.NewLine + Environment.NewLine + "SQL: " + sql);
            }
        }

        protected void btnContract_Click(object sender, EventArgs e)
        {
            if (fullname == "")
                return;

            if (lbSelectedCommID.Items.Count <= 0)
                return;

            Row xrow = null;

            string docName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, Guid.NewGuid().ToString() + ".xlsx");
            string fn = "";

            File.Delete(docName);
            File.Copy(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"Contract-Blank.xlsx"), docName);

            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
            {
                ////// Get the SharedStringTablePart. If it does not exist, create a new one.
                SharedStringTablePart shareStringPart;
                if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                else
                    shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();

                ////// Grab the one and only worksheet //////
                WorksheetPart worksheetPart = spreadSheet.WorkbookPart.WorksheetParts.First();

                string sql = "SELECT [PARTY_KEY]";
                sql += "      ,[Business_Type]";
                sql += "      ,[Portfolio_Cd]";
                sql += "      ,[System_Of_Record]";
                sql += "      ,[Issuing_Company]";
                sql += "      ,[Contract_No]";
                sql += "      ,[Contract_Name]";
                sql += "      ,[Contract_Type_Cd]";
                sql += "      ,[Contract_Effective_Dt]";
                sql += "      ,[Contract_Expiration_Dt]";
                sql += "      ,[UW_Year]";
                sql += "      ,[Attachment_Point_Amt]";
                sql += "      ,[Contract_Limit_Amt]";
                sql += "      ,[Old_Contract_Ref_No]";
                sql += "      ,[Party_ID]";
                sql += "      ,[Party_Name]";
                sql += "      ,[Participant_Pct]";
                sql += "      ,[Pool_ID]";
                sql += "      ,[Pool_Name]";
                sql += "      ,[Broker_ID]";
                sql += "      ,[Broker_Name]";
                sql += "      ,[Paid_To_Date]";
                sql += "      ,[Balance]";
                sql += "      ,[Current_Balance_Unbilled]";
                sql += "      ,[Curent_Balance_Under_Threshold]";
                sql += "      ,[Case_Reserve]";
                sql += "      ,[IBNR_Low]";
                sql += "      ,[IBNR_Central]";
                sql += "      ,[IBNR_High]";
                sql += "      ,[CommID]";
                sql += "      ,[CommName]";
                //sql += "  FROM [ReinsReporting].[dbo].[CommutationContractView]";
                sql += "  FROM [CommutationContractView]";

                string where = "";

                buildWhereListFromSelected(ref where, ref fn);
                sql += "  WHERE (" + where + ")";

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string sqlcomm = sql;
                    using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 60 * 10;
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                        uint row = 2;
                        while (reader.Read())
                        {
                            xrow = null;

                            string strPARTY_KEY = reader["PARTY_KEY"].ToString();
                            string strBusiness_Type = reader["Business_Type"].ToString();
                            string strPortfolio_Cd = reader["Portfolio_Cd"].ToString();
                            string strSystem_Of_Record = reader["System_Of_Record"].ToString();
                            string strIssuing_Company = reader["Issuing_Company"].ToString();
                            string strContract_No = reader["Contract_No"].ToString();
                            string strContract_Name = reader["Contract_Name"].ToString();
                            string strContract_Type_Cd = reader["Contract_Type_Cd"].ToString();
                            string strContract_Effective_Dt = reader["Contract_Effective_Dt"].ToString();
                            string strContract_Expiration_Dt = reader["Contract_Expiration_Dt"].ToString();
                            string strUW_Year = reader["UW_Year"].ToString();
                            string strAttachment_Point_Amt = reader["Attachment_Point_Amt"].ToString();
                            string strContract_Limit_Amt = reader["Contract_Limit_Amt"].ToString();
                            string strOld_Contract_Ref_No = reader["Old_Contract_Ref_No"].ToString();
                            string strParty_ID = reader["Party_ID"].ToString();
                            string strParty_Name = reader["Party_Name"].ToString();
                            string strParticipant_Pct = reader["Participant_Pct"].ToString();
                            string strPool_ID = reader["Pool_ID"].ToString();
                            string strPool_Name = reader["Pool_Name"].ToString();
                            string strBroker_ID = reader["Broker_ID"].ToString();
                            string strBroker_Name = reader["Broker_Name"].ToString();
                            string strPaid_To_Date = reader["Paid_To_Date"].ToString();
                            string strBalance = reader["Balance"].ToString();
                            string strCurrent_Balance_Unbilled = reader["Current_Balance_Unbilled"].ToString();
                            string strCurent_Balance_Under_Threshold = reader["Curent_Balance_Under_Threshold"].ToString();
                            string strCase_Reserve = reader["Case_Reserve"].ToString();
                            string strIBNR_Low = reader["IBNR_Low"].ToString();
                            string strIBNR_Central = reader["IBNR_Central"].ToString();
                            string strIBNR_High = reader["IBNR_High"].ToString();
                            string strCommID = reader["CommID"].ToString();
                            string strCommName = reader["CommName"].ToString();

                                setTextCell(worksheetPart, shareStringPart, "A", row, strPARTY_KEY, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "B", row, strBusiness_Type, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "C", row, strPortfolio_Cd, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "D", row, strSystem_Of_Record, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "E", row, strIssuing_Company, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "F", row, strContract_No, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "G", row, strContract_Name, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "H", row, strContract_Type_Cd, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "I", row, strContract_Effective_Dt, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "J", row, strContract_Expiration_Dt, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "K", row, strUW_Year, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "L", row, strAttachment_Point_Amt, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "M", row, strContract_Limit_Amt, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "N", row, strOld_Contract_Ref_No, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "O", row, strParty_ID, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "P", row, strParty_Name, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "Q", row, strParticipant_Pct, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "R", row, strPool_ID, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "S", row, strPool_Name, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "T", row, strBroker_ID, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "U", row, strBroker_Name, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "V", row, strPaid_To_Date, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "W", row, strBalance, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "X", row, strCurrent_Balance_Unbilled, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "Y", row, strCurent_Balance_Under_Threshold, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "Z", row, strCase_Reserve, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AA", row, strIBNR_Low, ref xrow); 
                                setNumbCell(worksheetPart, shareStringPart, "AB", row, strIBNR_Central, ref xrow); 
                                setNumbCell(worksheetPart, shareStringPart, "AC", row, strIBNR_High, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AD", row, strCommID, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AE", row, strCommName, ref xrow); 

                            row++;
                        }
                    }

                }
                worksheetPart.Worksheet.Save();
            }

            Response.ContentType = "application/excel";
            Response.AppendHeader("content-disposition", "attachment; filename=" + @"Contract-" + fn + ".xlsx");
            //Response.TransmitFile(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"Contract.xlsx"));
            Response.TransmitFile(docName);
            Response.End();

        }

        protected void btnOpenBalance_Click(object sender, EventArgs e)
        {
            if (fullname == "")
                return;

            if (lbSelectedCommID.Items.Count <= 0)
                return;

            Row xrow = null;

            //string docName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath ,@"OpenBalance.xlsx");
            string docName = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, Guid.NewGuid().ToString() + ".xlsx");

            
            string fn = "";

            File.Delete(docName);
            File.Copy(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"OpenBalance-Blank.xlsx"), docName);

            using (SpreadsheetDocument spreadSheet = SpreadsheetDocument.Open(docName, true))
            {
                ////// Get the SharedStringTablePart. If it does not exist, create a new one.
                SharedStringTablePart shareStringPart;
                if (spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().Count() > 0)
                    shareStringPart = spreadSheet.WorkbookPart.GetPartsOfType<SharedStringTablePart>().First();
                else
                    shareStringPart = spreadSheet.WorkbookPart.AddNewPart<SharedStringTablePart>();

                ////// Grab the one and only worksheet //////
                WorksheetPart worksheetPart = spreadSheet.WorkbookPart.WorksheetParts.First();

                string sql = "SELECT [PARTY_KEY]";
                sql += "     ,[SYSTEM]";
                sql += "      ,[BUSINESS_TYPE]";
                sql += "      ,[PORTFOLIO]";
                sql += "      ,[WORKMATTER_NO]";
                sql += "      ,[ACCOUNT_NAME]";
                sql += "      ,[WORKMATTER_DESCRIPTION]";
                sql += "      ,[WORKMATTER_STATUS]";
                sql += "      ,[WORKMATTER_TYPE]";
                sql += "      ,[SPECIAL_TRACKING_GROUP]";
                sql += "      ,[LDF_CLAIM_TYPE]";   // new
                sql += "      ,[ISSUING_COMPANY]";
                sql += "      ,[POLICY_NO]";
                sql += "      ,[POLICY_EFF_DATE]";
                sql += "      ,[DATE_OF_LOSS]";
                sql += "      ,[CLAIM_ID]";
                sql += "      ,[RELATED_CLAIM_ID]";
                sql += "      ,[CONTRACT_NO]";
                sql += "      ,[CONTRACT_SECTION]";
                sql += "      ,[CONTRACT_EFF_DATE]";
                sql += "      ,[CONTRACT_NAME]";
                sql += "      ,[CONTRACT_TYPE]";
                sql += "      ,[CONTRACT_ATTACHMENT]";
                sql += "      ,[CONTRACT_LIMIT]";
                sql += "      ,[SECTION_REF_NO]";
                sql += "      ,[OLD_TTY_REF_NO]";
                sql += "      ,[PARTICIPATION_PCT]";    // new
                sql += "      ,[PARTY_ID]";
                sql += "      ,[PARTY_NAME]";
                sql += "      ,[OLD_REINS_REF_NO]";
                sql += "      ,[AFFILIATE]";
                sql += "      ,[BROKER_ID]";
                sql += "      ,[BROKER_NAME]";
                sql += "      ,[POOL_ID]";
                sql += "      ,[POOL_NAME]";
                sql += "      ,[PAID_TO_DATE]"; // new
                sql += "      ,[BALANCE]"; // new
                sql += "      ,[BILLED_DATE]"; // new
                sql += "      ,[DUE_DATE]"; // new
                sql += "      ,[DAYS_OVER_DUE]"; // new
                sql += "      ,[CURRENT_BALANCE_UNBILLED]"; // new
                sql += "      ,[UNBILLED_DATE]"; // new
                sql += "      ,[DAYS_UNBILLED]"; // new
                sql += "      ,[CURRENT_BALANCE_UNDER_THRESHOLD]"; // new
                sql += "      ,[CEDED_DATE]"; // new
                sql += "      ,[DAYS_UNDER_THRESHOLD]"; // new
                sql += "      ,[CRED_PRV_PCT]"; // new
                sql += "      ,[DISPUTE_PRV_PCT]"; // new
                sql += "      ,[DISPUTE_LEVEL]"; // new
                sql += "      ,[DISPUTE_DATE]"; // new
                sql += "      ,[FEIN_Id]"; // new
                sql += "      ,[NAIC_Cd]"; // new
                sql += "      ,[Domicile_State_Cd]"; // new
                sql += "      ,[Domicile_Country_Cd]"; // new
                sql += "      ,[Credit_Provision_Percent]"; // new
                sql += "      ,[CommID]"; // new
                sql += "      ,[CommName]"; // new
                //sql += "  FROM [ReinsReporting].[dbo].[CommutationOpenBalanceView]"; // new
                sql += "  FROM [CommutationOpenBalanceView]"; // new

                string where = "";

                buildWhereListFromSelected(ref where, ref fn);
                sql += "  WHERE (" + where + ")";

                using (SqlConnection conn = new SqlConnection(connstring))
                {
                    string sqlcomm = sql;
                    using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 60 * 10;
                        conn.Open();
                        SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                        uint row = 2;
                        while (reader.Read())
                        {
                            xrow = null;
                            string strPARTY_KEY = reader["PARTY_KEY"].ToString();
                            string strSYSTEM = reader["SYSTEM"].ToString();
                            string strBUSINESS_TYPE = reader["BUSINESS_TYPE"].ToString();
                            string strPORTFOLIO = reader["PORTFOLIO"].ToString();
                            string strWORKMATTER_NO = reader["WORKMATTER_NO"].ToString();
                            string strACCOUNT_NAME = reader["ACCOUNT_NAME"].ToString();
                            string strWORKMATTER_DESCRIPTION = reader["WORKMATTER_DESCRIPTION"].ToString();
                            string strWORKMATTER_STATUS = reader["WORKMATTER_STATUS"].ToString();
                            string strWORKMATTER_TYPE = reader["WORKMATTER_TYPE"].ToString();
                            string strSPECIAL_TRACKING_GROUP = reader["SPECIAL_TRACKING_GROUP"].ToString();
                            string strLDF_CLAIM_TYPE = reader["LDF_CLAIM_TYPE"].ToString(); // new
                            string strISSUING_COMPANY = reader["ISSUING_COMPANY"].ToString();
                            string strPOLICY_NO = reader["POLICY_NO"].ToString();
                            string strPOLICY_EFF_DATE = reader["POLICY_EFF_DATE"].ToString();
                            string strDATE_OF_LOSS = reader["DATE_OF_LOSS"].ToString();
                            string strCLAIM_ID = reader["CLAIM_ID"].ToString();
                            string strRELATED_CLAIM_ID = reader["RELATED_CLAIM_ID"].ToString();
                            string strCONTRACT_NO = reader["CONTRACT_NO"].ToString();
                            string strCONTRACT_SECTION = reader["CONTRACT_SECTION"].ToString();
                            string strCONTRACT_EFF_DATE = reader["CONTRACT_EFF_DATE"].ToString();
                            string strCONTRACT_NAME = reader["CONTRACT_NAME"].ToString();
                            string strCONTRACT_TYPE = reader["CONTRACT_TYPE"].ToString();
                            string strCONTRACT_ATTACHMENT = reader["CONTRACT_ATTACHMENT"].ToString();
                            string strCONTRACT_LIMIT = reader["CONTRACT_LIMIT"].ToString();
                            string strSECTION_REF_NO = reader["SECTION_REF_NO"].ToString();
                            string strOLD_TTY_REF_NO = reader["OLD_TTY_REF_NO"].ToString();
                            string strPARTICIPATION_PCT = reader["PARTICIPATION_PCT"].ToString(); // new
                            string strPARTY_ID = reader["PARTY_ID"].ToString();
                            string strPARTY_NAME = reader["PARTY_NAME"].ToString();
                            string strOLD_REINS_REF_NO = reader["OLD_REINS_REF_NO"].ToString();
                            string strAFFILIATE = reader["AFFILIATE"].ToString();
                            string strBROKER_ID = reader["BROKER_ID"].ToString();
                            string strBROKER_NAME = reader["BROKER_NAME"].ToString();
                            string strPOOL_ID = reader["POOL_ID"].ToString();
                            string strPOOL_NAME = reader["POOL_NAME"].ToString();

                            string strPAID_TO_DATE = reader["PAID_TO_DATE"].ToString(); // new
                            string strBALANCE = reader["BALANCE"].ToString(); // new
                            string strBILLED_DATE = reader["BILLED_DATE"].ToString(); // new
                            string strDUE_DATE = reader["DUE_DATE"].ToString(); // new
                            string strDAYS_OVER_DUE = reader["DAYS_OVER_DUE"].ToString(); // new
                            string strCURRENT_BALANCE_UNBILLED = reader["CURRENT_BALANCE_UNBILLED"].ToString(); // new
                            string strUNBILLED_DATE = reader["UNBILLED_DATE"].ToString(); // new
                            string strDAYS_UNBILLED = reader["DAYS_UNBILLED"].ToString(); // new
                            string strCURRENT_BALANCE_UNDER_THRESHOLD = reader["CURRENT_BALANCE_UNDER_THRESHOLD"].ToString(); // new
                            string strCEDED_DATE = reader["CEDED_DATE"].ToString(); // new
                            string strDAYS_UNDER_THRESHOLD = reader["DAYS_UNDER_THRESHOLD"].ToString(); // new
                            string strCRED_PRV_PCT = reader["CRED_PRV_PCT"].ToString(); // new
                            string strDISPUTE_PRV_PCT = reader["DISPUTE_PRV_PCT"].ToString(); // new
                            string strDISPUTE_LEVEL = reader["DISPUTE_LEVEL"].ToString(); // new
                            string strDISPUTE_DATE = reader["DISPUTE_DATE"].ToString(); // new
                            string strFEIN_Id = reader["FEIN_Id"].ToString(); // new
                            string strNAIC_Cd = reader["NAIC_Cd"].ToString(); // new
                            string strDomicile_State_Cd = reader["Domicile_State_Cd"].ToString(); // new
                            string strDomicile_Country_Cd = reader["Domicile_Country_Cd"].ToString(); // new
                            string strCredit_Provision_Percent = reader["Credit_Provision_Percent"].ToString(); // new
                            string strCommID = reader["CommID"].ToString(); // new
                            string strCommName = reader["CommName"].ToString(); // new

                                setTextCell(worksheetPart, shareStringPart, "A", row, strPARTY_KEY, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "B", row, strSYSTEM, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "C", row, strBUSINESS_TYPE, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "D", row, strPORTFOLIO, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "E", row, strWORKMATTER_NO, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "F", row, strACCOUNT_NAME, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "G", row, strWORKMATTER_DESCRIPTION, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "H", row, strWORKMATTER_STATUS, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "I", row, strWORKMATTER_TYPE, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "J", row, strSPECIAL_TRACKING_GROUP, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "K", row, strLDF_CLAIM_TYPE, ref xrow); // new
                                setTextCell(worksheetPart, shareStringPart, "L", row, strISSUING_COMPANY, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "M", row, strPOLICY_NO, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "N", row, strPOLICY_EFF_DATE, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "O", row, strDATE_OF_LOSS, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "P", row, strCLAIM_ID, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "Q", row, strRELATED_CLAIM_ID, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "R", row, strCONTRACT_NO, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "S", row, strCONTRACT_SECTION, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "T", row, strCONTRACT_EFF_DATE, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "U", row, strCONTRACT_NAME, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "V", row, strCONTRACT_TYPE, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "W", row, strCONTRACT_ATTACHMENT, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "X", row, strCONTRACT_LIMIT, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "Y", row, strSECTION_REF_NO, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "Z", row, strOLD_TTY_REF_NO, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AA", row, strPARTICIPATION_PCT, ref xrow); // new 
                                setTextCell(worksheetPart, shareStringPart, "AB", row, strPARTY_ID, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AC", row, strPARTY_NAME, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AD", row, strOLD_REINS_REF_NO, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AE", row, strAFFILIATE, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AF", row, strBROKER_ID, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AG", row, strBROKER_NAME, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AH", row, strPOOL_ID, ref xrow); 
                                setTextCell(worksheetPart, shareStringPart, "AI", row, strPOOL_NAME, ref xrow); 
                                setNumbCell(worksheetPart, shareStringPart, "AJ", row, strPAID_TO_DATE, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AK", row, strBALANCE, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "AL", row, strBILLED_DATE, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "AM", row, strDUE_DATE, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AN", row, strDAYS_OVER_DUE, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AO", row, strCURRENT_BALANCE_UNBILLED, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "AP", row, strUNBILLED_DATE, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AQ", row, strDAYS_UNBILLED, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AR", row, strCURRENT_BALANCE_UNDER_THRESHOLD, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "AS", row, strCEDED_DATE, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AT", row, strDAYS_UNDER_THRESHOLD, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AU", row, strCRED_PRV_PCT, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AV", row, strDISPUTE_PRV_PCT, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "AW", row, strDISPUTE_LEVEL, ref xrow);
                                setDateCell(worksheetPart, shareStringPart, "AX", row, strDISPUTE_DATE, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "AY", row, strFEIN_Id, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "AZ", row, strNAIC_Cd, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "BA", row, strDomicile_State_Cd, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "BB", row, strDomicile_Country_Cd, ref xrow);
                                setNumbCell(worksheetPart, shareStringPart, "BC", row, strCredit_Provision_Percent, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "BD", row, strCommID, ref xrow);
                                setTextCell(worksheetPart, shareStringPart, "BE", row, strCommName, ref xrow);


                            //    xlWorkSheet.get_Range("A" + row.ToString()).ValstrCommName = reader["CommName"].Tue2 = ID;
                            //    xlWorkSheet.get_Range("B" + row.ToString()).Value2 = Convert.ToDouble(itemName);
                            row++;
                        }
                    }


                }
                worksheetPart.Worksheet.Save();
            }

            Response.ContentType = "application/excel";
            Response.AppendHeader("content-disposition", "attachment; filename=" + @"OpenBalance-" + fn + ".xlsx");
            //Response.TransmitFile(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, docName));
            Response.TransmitFile(docName);
            Response.End();
        }

        protected void btnAddNewCommID_Click(object sender, EventArgs e)
        {
            if (fullname == "")
                return;

            lblCommNameMayNotBeBlank.Visible = false;
            lblCommNameTooLong.Visible = false;
            lblCommIDMayNotBeBlank.Visible = false;

            txtCommID.Text = "";
            txtCommName.Text = "";

            pnlNewCommID.Visible = true;
            pnlControls.Visible = false;
        }

        protected void btnCreateCommID_Click(object sender, EventArgs e)
        {
            lblCommNameMayNotBeBlank.Visible = false;
            lblCommNameTooLong.Visible = false;
            lblCommIDMayNotBeBlank.Visible = false;

            if (txtCommID.Text.Trim() == "")
            {
                lblCommIDMayNotBeBlank.Visible = true;
                return;
            }
            if (txtCommName.Text.Trim() == "")
            {
                lblCommNameMayNotBeBlank.Visible = true;
                return;
            }
            if (txtCommName.Text.Length > 10)
            {
                lblCommNameTooLong.Visible = true;
                return;
            }

            System.Collections.SortedList sorted = new SortedList();

            foreach (ListItem ll in lbSelectedCommID.Items)
                sorted.Add(ll.Text, ll.Value);

            sorted.Add(txtCommID.Text + " - " + txtCommName.Text, txtCommID.Text);

            lbSelectedCommID.Items.Clear();
            foreach (String key in sorted.Keys)
                lbSelectedCommID.Items.Add(new ListItem(key, sorted[key].ToString()));

            pnlNewCommID.Visible = false;
            pnlControls.Visible = true;
        }

        protected void btnCancelCreateCommID_Click(object sender, EventArgs e)
        {
            pnlNewCommID.Visible = false;
            pnlControls.Visible = true;
        }

        private void getUserInfo()
        {
            string additionalusers = ConfigurationManager.AppSettings["Additional"].ToString().ToUpper();

            // Do nothing if we already have it!
            if (fullname != "")
                return;

            // Grab it from the session variables if we have those
            if (Session["fullname"] != null)
            {
                fullname = Session["fullname"].ToString();
                return;
            }

            // Grab AD info from RSSE if we don't have it
            if (dtAD.Rows.Count == 0)
                dtAD = getADUsers();

            // Get rid of TRG, limit to 5 letters for Lisa Volonte
            string user = User.Identity.Name.ToUpper();
            int backslash = User.Identity.Name.ToUpper().IndexOf("\\");
            if (backslash >= 0)
                user = User.Identity.Name.ToUpper().Substring(backslash + 1);

            if (user.Length > 5)
                user = user.Substring(0, 5);

            if (dtAD.Columns[0].ColumnName == "Message")
            {
                fullname = dtAD.Rows[0]["Message"].ToString();
                Session["fullname"] = fullname;
            }

            else
            {
                // Find the matching SamAccountName in AD
                foreach (DataRow dr in dtAD.Rows)
                {
                    string acct = dr["SamAccountName"].ToString().ToUpper();
                    if (acct.Length > 5)
                        acct = acct.Substring(0, 5);

                    if (acct == user)
                    {
                        fullname = dr["DisplayName"].ToString();
                        if (fullname.IndexOf(", ") > 0)
                        {
                            fullname = fullname.Substring(fullname.IndexOf(", ") + 2) + " " + fullname.Substring(0, fullname.IndexOf(", "));
                            Session["fullaname"] = fullname;
                        }
                    }
                }
            }

            if ((fullname == "") && (additionalusers.IndexOf(user) >= 0))
            {
                fullname = user;
                Session["fullaname"] = user;
            }
        }

        private static DataTable getADUsers()
        {
            string environment = ConfigurationManager.AppSettings["RSSE"].ToString().ToUpper();

            DataTable myParams = new DataTable("Params");
            myParams.Columns.Add("name");
            myParams.Columns.Add("value");
            myParams.Rows.Add("@Script", "GET_COMMUTATION_ASSOCIATES");
            myParams.Rows.Add("@App", "ROC_JOBS");
            myParams.Rows.Add("@UserName", @"trg\smarc");
            myParams.Rows.Add("@Environment", @"DEFAULT");

            DataSet mySet = new DataSet("test");
            mySet.Tables.Add(myParams);

            StringWriter writer = new StringWriter();
            mySet.WriteXml(writer, XmlWriteMode.WriteSchema);
            string xml = writer.ToString();

            DataSet result = new DataSet();
            if (environment == "DEV")
            {
                RSSEDev se = new RSSEDev();
                result = se.ExecuteScript(xml);
            }
            if (environment == "TEST")
            {
                RSSETest se = new RSSETest();
                result = se.ExecuteScript(xml);
            }
            if (environment == "PROD")
            {
                RSSEProd se = new RSSEProd();
                result = se.ExecuteScript(xml);
            }
            return result.Tables[0];
        }





    }
}