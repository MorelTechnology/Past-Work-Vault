using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SD2
{
    public partial class _Default : Page
    {
        int toolbarheight = 60;
        string userRole = "IT";
        string connstring = "";

        List<string> trashWords = new List<string>() 
        {"a","about","after","all","also","an","and","any","are","as","ask","at","be","because","before","broken","but","by","can","do","find","for","from","get","give","go","have","he",
         "her","him","how","i","if","in","includes","into","is","issue","issues","it","its","just","know","like","make","may","me","my","need","new","no","not","obtain","of","off","on","one",
         "onto","or","our","out","over","please","put","she","should","since","so","than","thank","that","the","their","then","there","this","to","two","until","up","want","was","we",
         "were","what","when","where","which","who","why","will","with","within","without","would","you" };

        List<KeyValuePair<string, string>> swapWords = new List<KeyValuePair<string, string>>() {
            new KeyValuePair<string, string>("mouse", "Laptops"), new KeyValuePair<string, string>("keyboard", "Laptops"),
            new KeyValuePair<string, string>("screen", "Laptops"),
            new KeyValuePair<string, string>("monitor", "Laptops") };

        protected void Page_Load(object sender, EventArgs e)
        {
            connstring = ConfigurationManager.AppSettings["ConnectionString"].ToString();

            string url = HttpContext.Current.Request.Url.AbsoluteUri;

            toolbarheight = 60;
            if (url.ToUpper().IndexOf("CHROME=FALSE") > 0)
                toolbarheight = 0;
            if (url.ToUpper().IndexOf("USER=OPS") > 0)
                userRole = "OPS";
            if (url.ToUpper().IndexOf("USER=BIZ") > 0)
                userRole = "BIZ";
            if (url.ToUpper().IndexOf("USER=IT") > 0)
                userRole = "IT";


            var controlName = Request.Params.Get("__EVENTTARGET");
            var argument = Request.Params.Get("__EVENTARGUMENT");
            if (controlName == "Panel1" && argument == "Click")
            {
                string parameter = hdnfldVariable.Value;
                if (parameter.StartsWith("GROUP="))
                {
                    generateItemScreen(parameter.Substring("GROUP=".Length),"","");
                }
                if (parameter.StartsWith("TASK="))
                {
                    string[] parts = parameter.Split(',');
                    string task = parts[0].Substring("TASK=".Length);
                    string group = parts[1].Substring("GROUP=".Length);
                    string item = parts[2].Substring("ITEM=".Length);
                    generateItemScreen((group.StartsWith("NONE")) ? "" : group, item, task);
                }

                if (parameter.StartsWith("ITEM="))
                {
                    generateItemScreen("",parameter.Substring("ITEM=".Length), "");
                }

                if (parameter.StartsWith("SEARCHTASK="))
                {
                    string[] parts = parameter.Split(',');
                    string task = parts[0].Substring("SEARCHTASK=".Length);
                    string searchString = parts[1].Substring("SEARCH=".Length);
                    BuildSearchResults(searchString, task);
                }

                if (parameter.StartsWith("REQUEST="))
                {
                    generatePhaseIIScreen();
                }

                if (parameter.StartsWith("ASK="))
                {
                    generatePhaseIIScreen();
                }

                if (parameter.StartsWith("SEARCH="))
                {
                    if (parameter.Length == "SEARCH=".Length)
                        return;

                    string searchText = parameter.Substring("SEARCH=".Length);
                    BuildSearchResults(searchText, "");
                }

                if (parameter == "HOME")
                {
                    generateHomeScreen();
                }

                if (parameter == "QUESTION")
                {
                    generateQuestion();
                }

            }
            else
            {
                generateHomeScreen();
            }
        }

        #region Main Screen

        private void generateHomeScreen()
        {
            LoadCatalog();
            LoadItems();
        }

        private void LoadCatalog()
        {
            //if (toolbarheight > 0)
            //{
            //    CreateImage(Panel1, "header.png", 1060, 50, 20, 20);
            //}
            CreateHeader();
            toolbarheight = 115;
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "select * from [Catalog] order by Name";

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    int row = 0;
                    int col = 0;

                    while (reader.Read())
                    {
                        string ID = reader["CatalogID"].ToString();
                        string ButtonTitle = reader["ButtonTitle"].ToString();
                        string ButtonIcon = reader["ButtonIcon"].ToString();
                        string ButtonText = reader["ButtonText"].ToString();

                        if (CatalogAvailableForUser(ID))
                        {
                            CreateServiceButton(row, col, ButtonTitle, ButtonIcon, ButtonText, ID);

                            if (col == 0)
                                col++;
                            else
                            {
                                row++;
                                col = 0;
                            }
                        }
                    }

                }

            }

        }

        private bool CatalogAvailableForUser(string catalogID)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {

                string sqlcomm = "select t.TaskID, i.Name as ItemName, i.ServiceID, t.Name as TaskName, t.ForBusinessAssoc, t.ForOperationsAssoc, t.ForITAssoc from [Service] i  ";
                sqlcomm += "join [CatalogService] gi on gi.ServiceID = i.ServiceID ";
                sqlcomm += "join [Task] T on t.ServiceID = gi.ServiceID ";
                sqlcomm += "where gi.CatalogID = " + catalogID + " ";
                sqlcomm += "order by i.Name, t.Name";

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (reader.Read())
                    {
                        string biz = reader["ForBusinessAssoc"].ToString();
                        string ops = reader["ForOperationsAssoc"].ToString();
                        string it = reader["ForITAssoc"].ToString();
                        if ((userRole == "OPS") && (ops == "True"))
                            return true;
                        if ((userRole == "BIZ") && (biz == "True"))
                            return true;
                        if ((userRole == "IT") && (it == "True"))
                            return true;
                    }
                    return false;
                }
            }
        }


        private bool ServiceAvailableForUser(string serviceID)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {

                string sqlcomm = "select t.TaskID, i.Name as ItemName, i.ServiceID, t.Name as TaskName, t.ForBusinessAssoc, t.ForOperationsAssoc, t.ForITAssoc from [Service] i  ";
                sqlcomm += "join [Task] T on t.ServiceID = i.ServiceID ";
                sqlcomm += "where i.serviceID = " + serviceID + " ";
                sqlcomm += "order by i.Name, t.Name";

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (reader.Read())
                    {
                        string biz = reader["ForBusinessAssoc"].ToString();
                        string ops = reader["ForOperationsAssoc"].ToString();
                        string it = reader["ForITAssoc"].ToString();
                        if ((userRole == "OPS") && (ops == "True"))
                            return true;
                        if ((userRole == "BIZ") && (biz == "True"))
                            return true;
                        if ((userRole == "IT") && (it == "True"))
                            return true;
                    }
                    return false;
                }
            }
        }

        
        private void CreateHeader()
        {
            string user1 = User.Identity.Name;
            string user2 = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            string user3 = System.Web.HttpContext.Current.User.Identity.Name;


            Panel panelHeader = CreatePanel(null, 1060, 100, 20, 20, System.Drawing.Color.FromArgb(242, 241, 235), false, "", "");
            CreateImage(panelHeader, "RS.png", 78, 40, 7, 9);
            //CreateLabel(panelHeader, "Welcome 1" + user1 + "2" + user2 + "3" + user3 +  " to the Riverstone Operations / IT Service Catalog", 600, 15, 94, 7, System.Drawing.Color.DimGray, 14, "", true);
            CreateLabel(panelHeader, "Welcome to the Riverstone Operations / IT Service Catalog", 600, 15, 94, 7, System.Drawing.Color.DimGray, 14, "", true);
            CreateLabel(panelHeader, "This page provides the service catalog for the services offered by the Riverstone Information Technology", 660, 15, 94, 33, System.Drawing.Color.Black, 10, "", false);
            CreateLabel(panelHeader, "department and related services from PMU, BIU, Facilities, and ARO.", 600, 15, 94, 51, System.Drawing.Color.Black, 10, "", false);
            CreateLabel(panelHeader, "Have a question for IT?", 600, 15, 94, 73, System.Drawing.Color.Black, 10, "", false);
            CreateLabel(panelHeader, "Click here to ask!", 0, 15, 234, 73, System.Drawing.Color.DimGray, 10, "PanelClick('QUESTION');", true, "border:0.3pt; border-bottom-width: .5px; border-bottom: thin dotted #BBBBBB;cursor:pointer;");
            CreateLabel(panelHeader, "Search the catalog.", 200, 15, 825, 20, System.Drawing.Color.Black, 9, "", false);
            CreateImage(panelHeader, "Search.png", 24, 24, 1012, 38, "PanelClick('SEARCH=' + document.getElementById(\"FeaturedContent_SEARCHBOX\").value);");
            CreateTextbox(panelHeader, "SEARCHBOX", 170, 14, 825, 26, System.Drawing.Color.DarkRed, 10, "", true, "");
            Panel1.Controls.Add(panelHeader);
        }

        private void CreateListOfItems(List<KeyValuePair<string, string>> items)
        {
            CreateLabel(Panel1, "A TO Z", 260, 60, 800, toolbarheight + 14, System.Drawing.Color.DarkRed, 13);
            Panel newpanel = CreatePanel(null, 280, 1160, 800, toolbarheight + 40, System.Drawing.Color.FromArgb(242, 241, 235), true);

            int line = 0;
            foreach (KeyValuePair<string, string> kvp in items)
            {
                if (ServiceAvailableForUser(kvp.Key.ToString()))
                {
                    CreateLabel(newpanel, kvp.Value.ToString(), 0, 18, 10, (line * 22), System.Drawing.Color.Black, 10, "PanelClick('ITEM=" + kvp.Key.ToString() + "');", false, "border-style: solid; border-bottom-width: .3px; border-bottom: .3px thin dotted #AAAAAA;cursor:pointer;");
                    line++;
                }
            }

            Panel1.Controls.Add(newpanel);
        }

        private void CreateServiceButton(int row, int col, string title, string icon, string text, string ID)
        {
            Panel newpanel = CreatePanel(null, 370, 130, ((col == 0) ? 20 : 410), row * 150 + 20 + toolbarheight, System.Drawing.Color.FromArgb(242, 241, 235), false, "Panel" + (row * 2 + col + 80).ToString(), "PanelClick('GROUP=" + ID + "');", "cursor:pointer;");

            if (icon != "")
            {
                CreateImage(newpanel, icon, 50, 50, 20, 15);
            }

            CreateLabel(newpanel, title, 260, 60, 95, 10, System.Drawing.Color.DarkRed, 13, "", false, "cursor:pointer;");
            CreateLabel(newpanel, text, 260, 37, 95, 37, System.Drawing.Color.Black, 10, "", false, "cursor:pointer;");
            CreateImage(newpanel, "bottom.png", 370, 1, 0, 129);
            Panel1.Controls.Add(newpanel);
        }
        #endregion

        #region Items Screen

        private void generateItemScreen(string groupid, string itemid, string taskid)
        {
            CreateSmallHeader();
            toolbarheight = 68;

            Panel newpanel = CreatePanel(null, 340, 1150, 10, toolbarheight + 160, System.Drawing.Color.FromArgb(242, 241, 235), false);
            CreateImage(newpanel, "redbar.png", 340, 5, 0, 0);

            if (groupid == "")
                LoadItemsForItemScreenWithoutGroup(newpanel, itemid, taskid);
            else
                LoadItemsForItemScreen(newpanel, groupid, taskid);

            if (groupid == "")
                LoadItemNameHeaderInfo(itemid);
            else
                LoadGroupNameHeaderInfo(groupid);



            if (taskid == "")
            {
                if (groupid != "")
                    CreatePlaceholderWithInstructions(GetLargeAreaDescriptionForCatalog(groupid).Replace(Environment.NewLine, "<br />"));
                else if (itemid != "")
                    CreatePlaceholderWithInstructions(GetLargeAreaDescriptionForService(itemid).Replace(Environment.NewLine, "<br />"));
                else
                    CreatePlaceholderWithInstructions("");
            }
            else
                CreateTaskInfo(taskid);

            Panel1.Controls.Add(newpanel);
        }


        private void CreateSmallHeader()
        {
            Panel panelHeader = CreatePanel(null, 1060, 58, 10, 10, System.Drawing.Color.FromArgb(242, 241, 235), false, "", "");
            CreateImage(panelHeader, "RS.png", 78, 40, 7, 9);
            CreateLabel(panelHeader, "Riverstone Operations / IT Service Catalog", 600, 15, 340, 17, System.Drawing.Color.DimGray, 14, "", true);
            CreateImage(panelHeader, "home.png", 24, 24, 1014, 18, "PanelClick('HOME');", "cursor:pointer;");
            Panel1.Controls.Add(panelHeader);
        }

        private void LoadItems()
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "select * from [Service] order by Name";

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (reader.Read())
                        items.Add(new KeyValuePair<string, string> (reader["ServiceID"].ToString(),reader["Name"].ToString()));

                    CreateListOfItems(items);

                }
            }
        }

        private void CreatePlaceholderWithInstructions(string info)
        {
            Panel newpanel = CreatePanel(null, 700, 1150, 370, toolbarheight + 160, System.Drawing.Color.FromArgb(242, 241, 235), false);

            if (info != "")
            {
                CreateImage(newpanel, "bluebar.png", 700, 5, 0, 0);

                string[] columntext = info.Split('|');

                if (columntext.Count() == 1)
                    CreateLabel(newpanel, info, 620, 60, 25, 30, System.Drawing.Color.Black, 12);
                else if (columntext.Count() == 2)
                {
                    CreateLabel(newpanel, columntext[0], 300, 60, 25, 30, System.Drawing.Color.Black, 12);
                    CreateLabel(newpanel, columntext[1], 300, 60, 350, 30, System.Drawing.Color.Black, 12);
                }
                else if (columntext.Count() == 3)
                {
                    CreateLabel(newpanel, columntext[0], 200, 60, 20, 30, System.Drawing.Color.Black, 12);
                    CreateLabel(newpanel, columntext[1], 200, 60, 250, 30, System.Drawing.Color.Black, 12);
                    CreateLabel(newpanel, columntext[2], 200, 60, 470, 30, System.Drawing.Color.Black, 12);
                }
            }
            else
            {
                CreateLabel(newpanel, "Select an item from the pane on the left", 560, 60, 95, 200, System.Drawing.Color.DarkRed, 16);
                CreateLabel(newpanel, "To learn more information about it", 560, 60, 95, 228, System.Drawing.Color.DarkRed, 16);
            }
            Panel1.Controls.Add(newpanel);
        }

        private void CreateTaskInfo(string taskID)
        {
            string taskName = "";
            string taskDescription = "";
            string slaName = "";
            string slaDescription = "";

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "select SLA.Description as slaDescription, SLA.Name as slaName, t.Name as Name, t.Description as Description from [Task] t ";
                sqlcomm += "left join servicelevelagreement SLA on t.ServiceLevelAgreementID = SLA.ServiceLevelAgreementID ";
                sqlcomm += "Where TaskID=" + taskID;

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (reader.Read())
                    {
                        slaDescription = reader["slaDescription"].ToString();
                        slaName = reader["slaName"].ToString();
                        taskName = reader["Name"].ToString();
                        taskDescription = reader["Description"].ToString();
                    }
                }
            }


            Panel panelTaskInfo = CreatePanel(null, 470, 1150, 360, toolbarheight + 160, System.Drawing.Color.FromArgb(242, 241, 235), false, "", "");
            CreateLabel(panelTaskInfo, taskName, 460, 60, 10, 10, System.Drawing.Color.DarkBlue, 14, "", true);
            CreateLabel(panelTaskInfo, taskDescription, 440, 300, 10, 40, System.Drawing.Color.Black, 10);
            CreateImage(panelTaskInfo, "bluebar.png", 470, 5, 0, 0);
            Panel1.Controls.Add(panelTaskInfo);


            Panel panelCreateRequest = CreatePanel(null, 230, 180, 840, toolbarheight + 160, System.Drawing.Color.FromArgb(233, 243, 234), false);

            CreateImage(panelCreateRequest, "greenbar.png", 230, 5, 0, 0);
            CreateLabel(panelCreateRequest, "Create a Request", 230, 60, 10, 10, System.Drawing.Color.DarkGreen, 14, "", true);

            Panel panelRequestButton = CreatePanel(null, 210, 60, 10, 60, System.Drawing.Color.DarkGreen, false, "", "PanelClick('REQUEST=" + taskID + "');");
            if (taskName.Length < 30)
            {
                CreateLabel(panelRequestButton, "Request", 202, 16, 4, 9, System.Drawing.Color.White, 14, "", true, "width: 100%; text-align:center; display:inline-block;");
                CreateLabel(panelRequestButton, taskName, 202, 8, 4, 33, System.Drawing.Color.White, 9, "", false, "width: 100%; text-align:center; display:inline-block;");
            }
            else
            {
                CreateLabel(panelRequestButton, "Request", 202, 16, 4, 0, System.Drawing.Color.White, 14, "", true, "width: 100%; text-align:center; display:inline-block;");
                taskName = taskName.Replace("(", "<br />(");
                CreateLabel(panelRequestButton, taskName, 202, 8, 4, 24, System.Drawing.Color.White, 9, "", false, "width: 100%; text-align:center; display:inline-block;");
            }


            panelCreateRequest.Controls.Add(panelRequestButton);

            Panel1.Controls.Add(panelCreateRequest);

            Panel panelSLA = CreatePanel(null, 230, 1150, 840, toolbarheight + 355, System.Drawing.Color.FromArgb(242, 241, 235), false);
            //CreateLabel(panelSLA, "What is my SLA?", 230, 60, 10, 10, System.Drawing.Color.DimGray, 14, "", true);
            CreateImage(panelSLA, "graybar.png", 230, 5, 0, 0);
            CreateLabel(panelSLA, "SLA " + slaName, 230, 60, 10, 10, System.Drawing.Color.DimGray, 14, "", true);
            //CreateLabel(panelSLA, slaName, 220, 19, 10, 30, System.Drawing.Color.Black, 11, "", true, " display:inline-block;");
            CreateLabel(panelSLA, slaDescription, 210, 300, 10, 37, System.Drawing.Color.Black, 10, "", false, "display:inline-block;");

            Panel1.Controls.Add(panelSLA);
        }

        private string GetLargeAreaDescriptionForCatalog(string groupid)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "select LargeAreaDescription from [Catalog] where CatalogID=" + groupid.ToString();

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    if (reader.Read())
                        return reader["LargeAreaDescription"].ToString();
                }
            }
            return "";
        }

        private string GetLargeAreaDescriptionForService(string serviceid)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "select LargeAreaDescription from [Service] where ServiceID=" + serviceid.ToString();

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    if (reader.Read())
                        return reader["LargeAreaDescription"].ToString();
                }
            }
            return "";
        }

        private void LoadGroupNameHeaderInfo(string groupid)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "select * from [Catalog] where CatalogID=" + groupid.ToString();

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (reader.Read())
                    {
                        string ID = reader["CatalogID"].ToString();
                        string ButtonTitle = reader["ButtonTitle"].ToString();
                        string ButtonIcon = reader["ButtonIcon"].ToString();
                        string PageDesc = reader["PageDescription"].ToString();
                        CreateGroupNameHeader(ButtonTitle, ButtonIcon, PageDesc, ID);
                    }
                }
            }
        }

        private void LoadItemNameHeaderInfo(string serviceid)
        {
            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "select * from [Service] where ServiceID=" + serviceid.ToString();

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (reader.Read())
                    {
                        string ID = reader["ServiceID"].ToString();
                        string Title = reader["Name"].ToString();
                        string Desc = reader["Description"].ToString();
                        CreateGroupNameHeader(Title, "services-65.png", Desc, "");
                    }
                }
            }
        }

        private void CreateGroupNameHeader(string ButtonTitle, string ButtonIcon, string PageDesc, string ID)
        {
            Panel newpanel = CreatePanel(null, 1060, 140, 10, toolbarheight + 10, System.Drawing.Color.FromArgb(242, 241, 235), false, "", (ID == "") ? "" : "PanelClick('GROUP=" + ID + "');");

            if (ButtonIcon != "")
                CreateImage(newpanel, ButtonIcon, 64, 64, 20, 25);

            CreateLabel(newpanel, ButtonTitle, 260, 60, 95, 20, System.Drawing.Color.DarkRed, 18);
            CreateLabel(newpanel, PageDesc, 900, 60, 95, 54, System.Drawing.Color.Black, 10, "", false);
            CreateImage(newpanel, "bottom.png", 1060, 2, 0, 138);
            Panel1.Controls.Add(newpanel);
        }


        private void LoadItemsForItemScreen(Panel panel, string groupid, string taskid)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "";
                sqlcomm = "select t.TaskID, i.Name as ItemName, i.ServiceID, t.Name as TaskName, t.ForBusinessAssoc, t.ForOperationsAssoc, t.ForITAssoc from [Service] i  ";
                sqlcomm += "join [CatalogService] gi on gi.ServiceID = i.ServiceID ";
                sqlcomm += "join [Task] T on t.ServiceID = gi.ServiceID ";
                sqlcomm += "where gi.CatalogID = " + groupid + " ";
                sqlcomm += "order by i.Name, t.Name";

                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    int top = 10;
                    string currentItemName = "";

                    while (reader.Read())
                    {
                        string ID = reader["TaskID"].ToString();
                        string itemName = reader["ItemName"].ToString();
                        string itemID = reader["ServiceID"].ToString();
                        string taskName = reader["TaskName"].ToString();
                        string biz = reader["ForBusinessAssoc"].ToString();
                        string ops = reader["ForOperationsAssoc"].ToString();
                        string it = reader["ForITAssoc"].ToString();

                        bool allow = false;
                        if ((userRole == "OPS") && (ops == "True"))
                            allow = true;
                        if ((userRole == "BIZ") && (biz == "True"))
                            allow = true;
                        if ((userRole == "IT") && (it == "True"))
                            allow = true;

                        if (allow)
                        {
                            if (itemName != currentItemName)
                            {
                                CreateLabel(panel, itemName, 0, 18, 10, top, System.Drawing.Color.DarkRed, 12, "PanelClick('ITEM=" + itemID.ToString() + "');", true, "border-style: solid; border-bottom-width: 15px; border-bottom: none;cursor:pointer;");
                                top += 24;
                                currentItemName = itemName;
                            }


                            CreateLabel(panel, taskName, 0, 18, 24, top, (taskid == ID) ? System.Drawing.Color.Blue : System.Drawing.Color.Black, 10, "PanelClick('TASK=" + ID + ",GROUP=" + groupid + ",ITEM=" + itemID + "');", false, "border-style: solid; border-bottom-width: 15px; border-bottom: thin dotted #AAAAAA;cursor:pointer;");
                            top += 22;
                        }


                    }

                }

            }

        }


        private void LoadItemsForItemScreenWithoutGroup(Panel panel, string serviceid, string taskid)
        {
            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>>();

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "";
                //sqlcomm = "select t.TaskID, i.Name as ItemName, i.ServiceID, t.Name as TaskName, t.ForBusinessAssoc, t.ForOperationsAssoc, t.ForITAssoc from [Service] i  ";
                //sqlcomm += "join [CatalogService] gi on gi.ServiceID = i.ServiceID ";
                //sqlcomm += "join [Task] T on t.ServiceID = gi.ServiceID ";
                //sqlcomm += "where gi.ServiceID = " + serviceid + " ";
                //sqlcomm += "order by i.Name, t.Name";

                sqlcomm = "select t.TaskID, i.Name as ItemName, i.ServiceID, t.Name as TaskName, t.ForBusinessAssoc, t.ForOperationsAssoc, t.ForITAssoc from [Service] i ";
                sqlcomm += "join [Task] T on t.ServiceID = i.ServiceID ";
                sqlcomm += "where i.ServiceID = " + serviceid + " ";
                sqlcomm += "order by i.Name, t.Name";


                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);
                    int top = 10;
                    string currentItemName = "";

                    while (reader.Read())
                    {
                        string ID = reader["TaskID"].ToString();
                        string itemName = reader["ItemName"].ToString();
                        string itemID = reader["ServiceID"].ToString();
                        string taskName = reader["TaskName"].ToString();
                        string biz = reader["ForBusinessAssoc"].ToString();
                        string ops = reader["ForOperationsAssoc"].ToString();
                        string it = reader["ForITAssoc"].ToString();

                        bool allow = false;
                        if ((userRole == "OPS") && (ops == "True"))
                            allow = true;
                        if ((userRole == "BIZ") && (biz == "True"))
                            allow = true;
                        if ((userRole == "IT") && (it == "True"))
                            allow = true;

                        if (allow)
                        {
                            if (itemName != currentItemName)
                            {
                                CreateLabel(panel, itemName, 0, 18, 10, top, System.Drawing.Color.DarkRed, 12, "", true);
                                top += 24;
                                currentItemName = itemName;
                            }


                            CreateLabel(panel, taskName, 0, 18, 24, top, (taskid == ID) ? System.Drawing.Color.Blue : System.Drawing.Color.Black, 10, "PanelClick('TASK=" + ID + ",GROUP=NONE ,ITEM=" + itemID + "');", false, "border-style: solid; border-bottom-width: 15px; border-bottom: thin dotted #AAAAAA;cursor:pointer;");
                            top += 22;
                        }


                    }

                }

            }

        }


        #endregion


        private void BuildSearchResults(string searchfor, string taskid)
        {
            CreateSmallHeader();
            toolbarheight = 68;

            CreateSearchResultsHeader(searchfor);

            if (taskid == "")
                CreatePlaceholderWithInstructions("");
            else
                CreateTaskInfo(taskid);


            Panel newpanel = CreatePanel(null, 340, 1150, 10, toolbarheight + 160, System.Drawing.Color.FromArgb(242, 241, 235), false);
            CreateImage(newpanel, "redbar.png", 340, 5, 0, 0);
            Panel1.Controls.Add(newpanel);

            string originalValue = searchfor;
            searchfor = searchfor.Replace(".", " ");
            searchfor = searchfor.Replace("?", " ");
            searchfor = searchfor.Replace("  ", " ");
            searchfor = searchfor.Replace("  ", " ");
            searchfor = searchfor.Replace("'", "");
            searchfor = searchfor.Replace("\"", "");

            string[] words = searchfor.Split(' ');

            // throw away all the prepositions, articles, interrogative and other trash words
            List <string> searchWords = new List<string>();
            foreach (string potential in words)
            {
                // See if this is a garbage word
                if (!isTrashWord(potential))
                {
                    searchWords.Add(potential);

                    foreach (KeyValuePair<string, string> kv in swapWords)
                        if (kv.Key == potential)
                            searchWords.Add(kv.Value);
                }
            }

            int top = 10;
            string currentItemName = "";

            // Only find each task once
            List<string> foundTasks = new List<string>();

            // Look for the words ORd in the taskname and item name and ANDED in the task description
            int countTN = subsearch(newpanel, searchWords, searchfor, taskid, ref top, ref currentItemName, "taskName", false, foundTasks);
            int countIN = subsearch(newpanel, searchWords, searchfor, taskid, ref top, ref currentItemName, "itemName", false, foundTasks);
            int countTD = subsearch(newpanel, searchWords, searchfor, taskid, ref top, ref currentItemName, "taskDesc", true, foundTasks);

            // If we found no results, try OR in the task description
            if ((countTN == 0) && (countTD == 0) && (countIN == 0))
            {
                subsearch(newpanel, searchWords, searchfor, taskid, ref top, ref currentItemName, "taskDesc", false, foundTasks);
            }
        }

        private int subsearch(Panel panel, List<string> searchWords, string searchfor, string taskid, ref int top, ref string currentItemName, string lookAt, bool andWords, List<string> foundTasks)
        {
            int count = 0;

            if (top >= 1100)
                return 0;

            using (SqlConnection conn = new SqlConnection(connstring))
            {
                string sqlcomm = "select t.TaskID, s.Name as ServiceName, s.ServiceID, t.Name as TaskName, t.Description as TaskDescription, t.ForBusinessAssoc, t.ForOperationsAssoc, t.ForITAssoc from [Service] s join [Task] t on t.ServiceID = s.ServiceID";
                using (SqlCommand cmd = new SqlCommand(sqlcomm, conn))
                {
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.SequentialAccess);

                    while (reader.Read())
                    {
                        string ID = reader["TaskID"].ToString();
                        string itemName = reader["ServiceName"].ToString();
                        string itemID = reader["ServiceID"].ToString();
                        string taskName = reader["TaskName"].ToString();
                        string taskDesc = reader["TaskDescription"].ToString();
                        string biz = reader["ForBusinessAssoc"].ToString();
                        string ops = reader["ForOperationsAssoc"].ToString();
                        string it = reader["ForITAssoc"].ToString();

                        bool found = true;
                        if (andWords == false)
                            found = false;

                        foreach (string word in searchWords)
                        {
                            bool thisWordFound = false;

                            if ((lookAt == "itemName") && (itemName.ToLower().IndexOf(word.ToLower()) >= 0))
                                thisWordFound = true;
                            if ((lookAt == "taskName") && (taskName.ToLower().IndexOf(word.ToLower()) >= 0))
                                thisWordFound = true;
                            if ((lookAt == "taskDesc") && (taskDesc.ToLower().IndexOf(word.ToLower()) >= 0))
                                thisWordFound = true;

                            if (andWords == true)
                            {
                                // When AND each word must be found or we are false
                                if (thisWordFound == false)
                                    found = false;
                            }
                            else
                            {
                                // When OR any word found is a match
                                if (thisWordFound == true)
                                    found = true;
                            }

                        }

                        if ((foundTasks.Contains(ID) == false)  &&  found)
                        {
                            if (itemName != currentItemName)
                            {
                                CreateLabel(panel, itemName, 0, 18, 10, top, System.Drawing.Color.DarkRed, 12, "", true);
                                top += 24;
                                currentItemName = itemName;
                            }


                            CreateLabel(panel, taskName, 0, 18, 24, top, (taskid == ID) ? System.Drawing.Color.Blue : System.Drawing.Color.Black, 10, "PanelClick('SEARCHTASK=" + ID + ",SEARCH=" + searchfor + "');", false, "border-style: solid; border-bottom-width: 15px; border-bottom: thin dotted #AAAAAA;cursor:pointer;");
                            top += 22;
                            count++;
                            foundTasks.Add(ID);

                            if (top >= 1100)
                                return count;
                        }

                    }

                }
            }
            return count;
        }

        private bool isTrashWord(string word)
        {
            foreach (string gw in trashWords)
                if (gw.ToLower() == word.ToLower())
                    return true;
            return false;
        }

        private void CreateSearchResultsHeader(string searchString)
        {
            Panel newpanel = CreatePanel(null, 1060, 140, 10, toolbarheight + 10, System.Drawing.Color.FromArgb(242, 241, 235), false, "", "");

            CreateImage(newpanel, "search3.png", 64, 64, 20, 25);
            CreateLabel(newpanel, "Search Results", 260, 60, 95, 20, System.Drawing.Color.DarkRed, 18);
            CreateLabel(newpanel, "These are the results for searching on: " + searchString, 900, 60, 95, 54, System.Drawing.Color.Black, 10, "", false);
            CreateImage(newpanel, "bottom.png", 1060, 2, 0, 138);
            Panel1.Controls.Add(newpanel);
        }

        private void generateQuestion()
        {
            CreateSmallHeader();
            toolbarheight = 68;

            Panel newpanel = CreatePanel(null, 1060, 540, 10, toolbarheight + 10, System.Drawing.Color.FromArgb(242, 241, 235), false, "", "");

            CreateImage(newpanel, "question.png", 64, 64, 20, 25);
            CreateLabel(newpanel, "Got a Question for IT?", 560, 60, 95, 25, System.Drawing.Color.DarkRed, 24,"",true);
            CreateLabel(newpanel, "Please ask! We are eager to help." , 900, 60, 95, 69, System.Drawing.Color.Black, 18, "", false);
            CreateLabel(newpanel, "Enter your question here", 900, 60, 95, 120, System.Drawing.Color.Black, 10, "", false);
            //CreateImage(newpanel, "bottom.png", 1060, 2, 0, 138);
            CreateTextbox(newpanel, "QUESTIONBOX", 870,300, 95, 127, System.Drawing.Color.DarkRed, 12, "", true, "",true);

            Panel panelRequestButton = CreatePanel(null, 230, 60, 748, 462, System.Drawing.Color.Maroon, false, "", "PanelClick('ASK=" +  "');");
            CreateLabel(panelRequestButton, "Submit", 202, 16, 4, 16, System.Drawing.Color.White, 14, "", true, "width: 100%; text-align:center; display:inline-block;");
            newpanel.Controls.Add(panelRequestButton);

            Panel1.Controls.Add(newpanel);

            Panel response = CreatePanel(null, 1060, 30, 10, toolbarheight + 559, System.Drawing.Color.FromArgb(242, 241, 235), false, "", "");
            CreateLabel(response, "Response will be via e-mail", 600, 20, 10, 5, System.Drawing.Color.DimGray, 11, "", false);
            Panel1.Controls.Add(response);
        }

        private void generatePhaseIIScreen()
        {
            CreateSmallHeader();
            toolbarheight = 68;

            Panel newpanel = CreatePanel(null, 1060, 540, 10, toolbarheight + 10, System.Drawing.Color.FromArgb(242, 241, 235), false, "", "");

            CreateImage(newpanel, "operator-small.png", 64, 64, 20, 25);
            CreateLabel(newpanel, "This feature is coming in Phase II", 560, 60, 95, 25, System.Drawing.Color.DarkRed, 24, "", true);
            Panel1.Controls.Add(newpanel);
        }

        private void doit()
        {
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
        }

        private TextBox CreateTextbox(Panel panel, string id, int width, int height, int left, int top, System.Drawing.Color foreColor, int size, string onclick = "", bool bold = false, string style = "", bool multiline = false)
        {
            TextBox lab = new TextBox();
            lab.ID = id;
            if (width > 0)
                lab.Width = width;
            if (height != 0)
                lab.Height = height;
            lab.Style["left"] = left.ToString() + "px";
            lab.Style["top"] = top.ToString() + "px";
            lab.Style["position"] = "absolute";
            lab.ForeColor = foreColor;
            if (multiline)
                lab.TextMode = TextBoxMode.MultiLine;
            lab.Font.Size = new FontUnit(size);
            if (bold)
                lab.Font.Bold = true;
            if (onclick != "")
                lab.Attributes["onclick"] = onclick;
            if (style != "")
                lab.Style["style"] = style;
            if (panel != null)
                panel.Controls.Add(lab);
            return lab;
        }

        private Label CreateLabel(Panel panel, string text, int width, int height, int left, int top, System.Drawing.Color foreColor, int size, string onclick="", bool bold=false, string style="")
        {
            Label lab = new Label();
            if (width > 0)
                lab.Width = width;
            if (height != 0)
                lab.Height = height;
            lab.Style["left"] = left.ToString() + "px";
            lab.Style["top"] = top.ToString() + "px";
            lab.Style["position"] = "absolute";
            lab.Text = text;
            lab.ForeColor = foreColor;
            lab.Font.Size = new FontUnit(size);
            if (bold)
                lab.Font.Bold = true;
            if (onclick != "")
                lab.Attributes["onclick"] = onclick;
            if (style != "")
                lab.Style["style"] = style;
            if (panel != null)
                panel.Controls.Add(lab);
            return lab;
        }

        private Image CreateImage(Panel panel, string name, int width, int height, int left, int top, string onclick = "", string style = "")
        {
            Image img = new Image();
            img.Width = width;
            img.Height = height;
            img.ImageUrl = "~/Images/" + name;
            img.Style["left"] = left.ToString() + "px";
            img.Style["top"] = top.ToString() + "px";
            img.Style["position"] = "absolute";
            if (style != "")
                img.Style["style"] = style;
            if (onclick != "")
                img.Attributes["onclick"] = onclick;
            if (panel != null)
                panel.Controls.Add(img);
            return img;
        }

        private Panel CreatePanel(Panel parentpanel, int width, int height, int left, int top, System.Drawing.Color backColor, bool verticalSB, string id = "", string onclick = "", string style="")
        {
            Panel panel = new Panel();
            if (id != "")
                panel.ID = id;
            panel.Width = width;
            panel.Height = height;
            panel.Style["left"] = left.ToString() + "px";
            panel.Style["top"] = top.ToString() + "px";
            panel.Style["position"] = "absolute";
            if (style != "")
                panel.Style["style"] = style;
            panel.BackColor = backColor;
            if (onclick != "")
                panel.Attributes["onclick"] = onclick;
            if (parentpanel != null)
                parentpanel.Controls.Add(panel);
            if (verticalSB)
                panel.ScrollBars = ScrollBars.Vertical;
            return panel;
        }


    }
}