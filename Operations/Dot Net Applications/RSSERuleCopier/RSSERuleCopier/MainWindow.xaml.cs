using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace RSSERuleCopier
{
    class valueCheck
    {
        public string ruleType;
        public string attributeName;
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string connectionString = "Data Source=BIDEVETL01;Initial Catalog=IAFramework;Integrated Security=True";

        DataTable engineProcess = new DataTable();
        DataTable engineRuleAttribute = new DataTable();
        DataTable engineRuleAttributeSet = new DataTable();
        DataTable engineRuleImplementation = new DataTable();
        DataTable engineRuleType = new DataTable();
        DataTable problems = new DataTable();

        static string currentProcessID = "";
        static int StartRuleID = -1;
        static int StopRuleID = -1;
        static bool complete = false;

        int controlCounter = 0;
        string currentRuleForExecution = "";
        List<int> breakpoints = new List<int>();
        string currentExecutingRule = "139";
        double currentAngle = 0d;

        // For spinning wheel
        System.Windows.Threading.DispatcherTimer dtSpinner = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer dtStatus = new System.Windows.Threading.DispatcherTimer();
        string startingBatchRulesEcecutionID = "";

        Thread tExecuteRules;


        List<valueCheck> valueChecks = new List<valueCheck>()
        { new valueCheck() { ruleType="StoredProcedure", attributeName="StoredProcedureName" },
          new valueCheck() { ruleType = "StoredProcedure", attributeName = "ReturnTarget" },
          new valueCheck() { ruleType = "StoredProcedure", attributeName = "TargetRepositoryName" },
          new valueCheck() { ruleType = "StoredProcedure", attributeName = "ConnectionName" } ,

          new valueCheck() { ruleType = "DirectMap", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "DirectMap", attributeName = "TargetRepositoryName" } ,
          new valueCheck() { ruleType = "DirectMap", attributeName = "ReturnTarget" } ,

          new valueCheck() { ruleType = "Generate", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "Generate", attributeName = "TargetRepositoryName" } ,
          new valueCheck() { ruleType = "Generate", attributeName = "ReturnTarget" } ,

          new valueCheck() { ruleType = "Lookup", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "Lookup", attributeName = "TargetRepositoryName" } ,
          new valueCheck() { ruleType = "Lookup", attributeName = "ReturnTarget" } ,
          new valueCheck() { ruleType = "Lookup", attributeName = "SourceRepositoryLookupName" } ,

          new valueCheck() { ruleType = "Concatenation", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "Concatenation", attributeName = "TargetRepositoryName" } ,
          new valueCheck() { ruleType = "Concatenation", attributeName = "ReturnTarget" } ,

          new valueCheck() { ruleType = "RowCount", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "RowCount", attributeName = "AuditKey" } ,

          new valueCheck() { ruleType = "TableFormat", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "TableFormat", attributeName = "TargetRepositoryName" } ,
          new valueCheck() { ruleType = "TableFormat", attributeName = "ReturnTarget" } ,

          new valueCheck() { ruleType = "LookupCheck", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "LookupCheck", attributeName = "SourceRepositoryLookupName" } ,
          new valueCheck() { ruleType = "LookupCheck", attributeName = "KeyColumnName" } ,

          new valueCheck() { ruleType = "ValueCheck", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "ValueCheck", attributeName = "ColumnName" } ,
          new valueCheck() { ruleType = "ValueCheck", attributeName = "KeyColumnName" } ,
          new valueCheck() { ruleType = "ValueCheck", attributeName = "PositiveNegativeCheck" } ,

          new valueCheck() { ruleType = "DuplicationCheck", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "DuplicationCheck", attributeName = "KeyColumnName" } ,

          new valueCheck() { ruleType = "TypeCheck", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "TypeCheck", attributeName = "ColumnName" } ,
          new valueCheck() { ruleType = "TypeCheck", attributeName = "KeyColumnName" } ,
          new valueCheck() { ruleType = "TypeCheck", attributeName = "ColumnType" } ,

          new valueCheck() { ruleType = "Function", attributeName = "SourceRepositoryName" } ,
          new valueCheck() { ruleType = "Function", attributeName = "AuditKey" } ,
          new valueCheck() { ruleType = "Function", attributeName = "AuditFunction" } ,
          new valueCheck() { ruleType = "Function", attributeName = "AuditFunctionColumn" } 


        };
        List<string> localRepositories = new List<string>();

    public MainWindow()
        {
            InitializeComponent();
            showRules();
            getAllData();
            fillProcess();
            showRadioButtonStatus(btnEnvLocal, new Button[] { btnEnvLocal, btnEnvDev, btnEnvUAT, btnEnvTest, btnEnvProd });
            dtSpinner.Tick += new EventHandler(dtSpinner_Tick);
            dtSpinner.Interval = new TimeSpan(0, 0, 0, 0, 100);
            dtSpinner.Start();
            tExecuteRules = new Thread(executeRules);

            dtStatus.Tick += new EventHandler(dtStatus_Tick);
            dtStatus.Interval = new TimeSpan(0, 0, 0, 3, 0);
            dtStatus.Start();

        }

        private void dtStatus_Tick(object sender, EventArgs e)
        {
            if (tExecuteRules.IsAlive)
            {
                updateRuleStatus();
            }


            if (tExecuteRules.ThreadState== ThreadState.Stopped)
            {
                int b = 4;
            }

            if (complete)
            {
                complete = false;
                updateRuleStatus();
                setIPStatus(StopRuleID.ToString(), "ip-breakpoint.png");

                int c = 4;
            }

        }

        private void updateRuleStatus()
        {
            string sql = "SELECT [RuleImplementationID],[CreatedDate],[StatusID],[RuleStart],[RuleFinish] FROM [IAFramework].[rules].[BatchRuleExecution] where [BatchRuleExecutionID] > " + startingBatchRulesEcecutionID + " order by batchruleexecutionid desc";

            DataTable results = getData(sql);
            if (results.Rows.Count > 0)
            {
                foreach (DataRow dr in results.Rows)
                {
                    string impID = dr["RuleImplementationID"].ToString();
                    string ruleFinish = dr["RuleFinish"].ToString();

                    int a = 4;

                    if (ruleFinish == "")
                        setStatus(impID, "spinner.png");
                    else
                        setStatus(impID, "success.png");


                }
            }
        }

        private void dtSpinner_Tick(object sender, EventArgs e)
        {
            foreach (object child in spRules.Children)
            {
                if (child is Border)
                {
                    Border b = (Border)child;
                    foreach (var gridChild in ((Grid)b.Child).Children)
                    {
                        if ((gridChild is Image))
                        {
                            Image img = ((Image)gridChild);
                            if ((img.Source != null) && (img.Visibility == Visibility.Visible))
                            {
                                if (((BitmapImage) img.Source).ToString().IndexOf("spinner") >= 0)
                                {
                                    currentAngle += 30d;
                                    if (currentAngle > 360)
                                        currentAngle = 0;

                                    setImageAngle(img, currentAngle);
//                                    ((RotateTransform)((TransformGroup)img.RenderTransform).Children[0]).Angle = currentAngle;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void setImageAngle(Image img, double angle)
        {
            if ((img.Source != null) && (img.Visibility == Visibility.Visible))
            {
                if (img.RenderTransform != null)
                    ((RotateTransform)((TransformGroup)img.RenderTransform).Children[0]).Angle = angle;
            }

        }

        private void clearAllStatus()
        {
            foreach (object child in spRules.Children)
            {
                if (child is Border)
                {
                    Border b = (Border)child;
                    foreach (var gridChild in ((Grid)b.Child).Children)
                    {
                        if ((gridChild is Image))
                        {
                            Image img = ((Image)gridChild);
                            if (img.Source != null)
                            {
                                string name = ((BitmapImage)img.Source).ToString();
                                if ((name.IndexOf("spinner") >= 0) || (name.IndexOf("success") >= 0))
                                    img.Visibility = Visibility.Collapsed;
                                if ((name.IndexOf("ip") >= 0))
                                    img.Visibility = Visibility.Collapsed;

                            }
                        }
                    }
                }
            }

        }

        private void setStatus(string id, string image)
        {
            bool found = false;
            foreach (object child in spRules.Children)
            {
                if (child is Border)
                {
                    Border b = (Border)child;
                    foreach (var gridChild in ((Grid)b.Child).Children)
                    {
                        if ((gridChild is TextBlock) && (((TextBlock)gridChild).Name.StartsWith("ID")))
                        {
                            if (id == ((TextBlock)gridChild).Text)
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        foreach (var gridChild in ((Grid)b.Child).Children)
                        {
                            if ((gridChild is Image))
                            {
                                Image img = ((Image)gridChild);
                                if (img.Source != null)
                                {
                                    string name = ((BitmapImage)img.Source).ToString();
                                    if ((name.IndexOf("spinner") >= 0) || (name.IndexOf("success") >= 0))
                                    {
                                        setImageAngle(img, 0d);
                                        img.Source = new BitmapImage(new Uri(@"Images\" + image, UriKind.Relative));
                                        img.Visibility = Visibility.Visible;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        private void setIPStatus(string id, string image)
        {
            bool found = false;
            foreach (object child in spRules.Children)
            {
                if (child is Border)
                {
                    Border b = (Border)child;
                    foreach (var gridChild in ((Grid)b.Child).Children)
                    {
                        if ((gridChild is TextBlock) && (((TextBlock)gridChild).Name.StartsWith("ID")))
                        {
                            if (id == ((TextBlock)gridChild).Text)
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        foreach (var gridChild in ((Grid)b.Child).Children)
                        {
                            if ((gridChild is Image))
                            {
                                Image img = ((Image)gridChild);
                                if (img.Source != null)
                                {
                                    string name = ((BitmapImage)img.Source).ToString();
                                    if ((name.IndexOf("ip") >= 0) || (name.IndexOf("break") >= 0))
                                    {
                                        img.Source = new BitmapImage(new Uri(@"Images\" + image, UriKind.Relative));
                                        // setImageAngle(img, 0d);
                                        img.Visibility = Visibility.Visible;
                                        return;
                                    }
                                }
                            }
                        }
                    }
                }
            }

        }

        #region get data

        private void getAllData()
        {
            engineProcess = getData("select * from engine.Process");
            engineRuleAttribute = getData("select * from engine.RuleAttribute");
            engineRuleAttributeSet = getData("select * from engine.RuleAttributeSet");
            engineRuleImplementation = getData("select * from engine.RuleImplementation rules full outer join [IAFramework].[engine].[RuleType] ruletype on rules.RuleTypeID=ruletype.RuleTypeID order by ExecutionOrder");
            engineRuleImplementation.Columns.Add("Color");
            engineRuleImplementation.Columns.Add("StatusSymbol");  
            engineRuleType = getData("select * from engine.RuleType");

            //SIMULATE ERRORS
            //foreach (DataRow dr in engineRuleAttribute.Rows)
            //{
            //    if (dr["RuleImplementationAttributeName"].ToString() == "Mappings.CLAIM_REOPEND_DATE.SourceColumn")
            //        dr["RuleImplementationAttributeName"] = "Mappings.CLAIM_REOPEND_DATE.SourceColumnx";

            //    if (dr["RuleImplementationAttributeName"].ToString() == "Columns.ClaimantPostalCode.OrderNumber")
            //        dr["RuleImplementationAttributeName"] = "Columns.ClaimantPostalCode.OrderNumberx";

            //    if (dr["RuleImplementationAttributeName"].ToString() == "JoinClauses.ASI_COMP_NUM.SourceColumn")
            //        dr["RuleImplementationAttributeName"] = "JoinClauses.ASI_COMP_NUM.SourceColumnx";
            //}
        }

        private void fillProcess()
        {
            engineProcess.Columns.Add("IndentedName");
            foreach (DataRow dr in engineProcess.Rows)
                dr["IndentedName"] = new string(' ', dr["NodeID"].ToString().Length) + dr["Name"].ToString();
            dgProcess.ItemsSource = engineProcess.DefaultView;
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
        #endregion

        private void btnOpenClose_Click(object sender, RoutedEventArgs e)
        {
            // Margin="152,42,10,15"
            if (btnOpenClose.Content.ToString() == "Choose")
            {
                OpenProcessChooser();
                if (dgProcess.SelectedIndex >= 0)
                {
                    DataRow dr = ((System.Data.DataRowView)(dgProcess.SelectedItem)).Row;
                }
            }
            else
            {
                CloseProcessChooser();
            }

        }

        private void OpenProcessChooser()
        {
            gridProcess.Visibility = Visibility.Visible;
            ThicknessAnimation da = new ThicknessAnimation();
            da.From = new Thickness(152, 42, ActualWidth-174d, 15);
            da.To = new Thickness(152, 42, 10, 15);
            da.Duration = new Duration(TimeSpan.FromSeconds(0.8d));
            gridProcess.BeginAnimation(MarginProperty, da);

            btnOpenClose.Content = "Close";
        }

        private void CloseProcessChooser()
        {
            ThicknessAnimation da = new ThicknessAnimation();
            da.To = new Thickness(152, 42, ActualWidth + 1600d, 15);
            da.From = new Thickness(152, 42, 10, 15);
            da.Duration = new Duration(TimeSpan.FromSeconds(1d));
            gridProcess.BeginAnimation(MarginProperty, da);

            btnOpenClose.Content = "Choose";
            //gridProcess.Visibility = Visibility.Collapsed;
        }

        private void FillRules(string processID)
        {
            spRules.Children.Clear();
            currentProcessID = processID;

            if (getRadioButtonStatus(new Button[] { btnLocalTables, btnRules, btnStatus, btnReferenceTables }) == "btnLocalTables")
                tables.setTargetType(true, currentProcessID);

            bool first = true;
            foreach (DataRow drRule in engineRuleImplementation.Rows)
            {
                if (drRule["ProcessID"].ToString() != processID)
                    continue;

                if (first)
                {
                    Image img = new Image();

                    img.Margin = new Thickness(10d);
                    img.Stretch = Stretch.None;
                    spRules.Children.Add(img);
                    first = false;
                }
                else
                {
                    Image img = new Image();

                    img.Margin = new Thickness(0);
                    img.Source = new BitmapImage(new Uri(@"Images\green arrow.png", UriKind.Relative));
                    img.Stretch = Stretch.None;
                    spRules.Children.Add(img);
                }


                string symbol = "";
                int ruleID = Convert.ToInt32(drRule["RuleImplementationID"].ToString());
                if (breakpoints.Contains(ruleID))
                    symbol = (currentRuleForExecution == ruleID.ToString()) ? "ip-breakpoint.png" : "breakpoint.png";
                else if (currentRuleForExecution == ruleID.ToString())
                    symbol = "ip.png";

                string filtertext = tbFilter.Text.ToUpper();
                if (filtertext == "ERROR")
                {
                    if (drRule["Color"].ToString() != "LightCoral")
                        continue;
                }
                else if (filtertext != "FILTER")
                {
                    bool skip = true;
                    if (drRule["RuleDescription"].ToString().ToUpper().IndexOf(filtertext) >= 0)
                        skip = false;
                    if (drRule["RuleName"].ToString().ToUpper().IndexOf(filtertext) >= 0)
                        skip = false;
                    foreach (DataRow drAttr in engineRuleAttribute.Rows)
                    {
                        if (drAttr["RuleAttributeSetID"].ToString() == drRule["RuleAttributeSetID"].ToString())
                        {
                            if (drAttr["RuleImplementationAttributeValue"].ToString().ToUpper().IndexOf(filtertext) >= 0)
                                skip = false;
                        }
                    }

                    if (skip)
                        continue;
                }

                //<Grid Height="60" Margin="10" Background="#FFD6CEE0" HorizontalAlignment="Stretch" Width="638">
                //    <TextBlock Margin="136,5,0,0" Text="Rule 1" FontSize="16" FontWeight="Bold" ></TextBlock>
                //    <TextBlock Margin="136,37,0,0" Text="Stored Procedure: ASI.ASIGetClaimDataValidation" FontWeight="Bold"  FontSize="12" ></TextBlock>
                //    <TextBlock Margin="136,23,0,0" Text="ClaimClaimNoandREPORTED_DATEDuplicationCheck"  FontSize="12" ></TextBlock>
                //    <TextBlock Margin="6,6,0,12" Text="114" FontWeight="Bold"  FontSize="32" Width="65" HorizontalAlignment="Left" VerticalAlignment="Center" TextAlignment="Center"></TextBlock>
                //    <TextBlock Margin="6,42,0,0" Text="RuleImpID"  FontSize="12" Width="65" HorizontalAlignment="Left" TextAlignment="Center" Foreground="#FF898989" ></TextBlock>
                //    <TextBlock Margin="550,6,0,12" Text="1091" FontWeight="Bold"  FontSize="32" Width="80" HorizontalAlignment="Left" VerticalAlignment="Center" TextAlignment="Center"></TextBlock>
                //    <TextBlock Margin="550,42,0,0" Text="Exec Order"  FontSize="12" Width="80" HorizontalAlignment="Left" TextAlignment="Center" Foreground="#FF898989" ></TextBlock>
                //    <Image Margin="78,5,0,0" HorizontalAlignment="Left" Height="50" VerticalAlignment="Top" Width="50"/>
                //</Grid>
                Border border = new Border();

                // was border.BorderBrush = new SolidColorBrush(Colors.White);
                // was border.BorderThickness = new Thickness(3);

                /// NEW ///
                border.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 205, 206, 208));
                border.BorderThickness = new Thickness(1);
                //border.Background = new SolidColorBrush(Colors.White);
                border.Margin = new Thickness(10, 0, 10, 0);
                border.CornerRadius = new CornerRadius(10d);
                border.Height = 64d;
                //border.Margin = new Thickness(0d);
                border.Width = 638d;
                border.HorizontalAlignment = HorizontalAlignment.Stretch;
                border.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(drRule["Color"].ToString()));
                //border.Padding = new Thickness(10, 10, 10, 10);
                /// NEW ///


                Grid grRule = new Grid();
                //grRule.Height = 60d;
                //grRule.Margin = new Thickness(0d);
                //grRule.Width = 638d;
                //grRule.HorizontalAlignment = HorizontalAlignment.Stretch;

                // grRule.Background = new SolidColorBrush(Color.FromArgb(0xff, 0xD6, 0xCE, 0xE0));
                // grRule.Background = new SolidColorBrush(Colors.Transparent);


                grRule.MouseDown += GrRule_MouseUp;

                Color colorActive = (drRule["IsActive"].ToString() == "False") ? Color.FromArgb(0xff, 0x89, 0x89, 0x89) : Colors.Black;

                grRule.Children.Add(createText("",drRule["RuleName"].ToString(), 16d, FontWeights.Bold, new Thickness(136, 5, 0, 0), 800, HorizontalAlignment.Left, VerticalAlignment.Top, TextAlignment.Left, colorActive));
                grRule.Children.Add(createText("", drRule["RuleDescription"].ToString(), 12d, FontWeights.Normal, new Thickness(136, 23, 0, 0), 800, HorizontalAlignment.Left, VerticalAlignment.Top, TextAlignment.Left, colorActive));
                grRule.Children.Add(createText("", drRule["Name"].ToString(), 12d, FontWeights.Bold, new Thickness(136, 37, 0, 0), 800, HorizontalAlignment.Left, VerticalAlignment.Top, TextAlignment.Left, colorActive));
                grRule.Children.Add(createText("ID", drRule["RuleImplementationID"].ToString(), 32d, FontWeights.Bold, new Thickness(26, 6, 0, 12), 65, HorizontalAlignment.Left, VerticalAlignment.Center, TextAlignment.Center, colorActive));
                grRule.Children.Add(createText("EO", drRule["ExecutionOrder"].ToString(), (drRule["ExecutionOrder"].ToString().Length >= 5) ? 28d : 32d, FontWeights.Bold, new Thickness(530, 6, 0, 12), 80, HorizontalAlignment.Left, VerticalAlignment.Center, TextAlignment.Center, colorActive));
                grRule.Children.Add(createText("", "Order", 12d, FontWeights.Normal, new Thickness(530, 42, 0, 0), 80, HorizontalAlignment.Left, VerticalAlignment.Top, TextAlignment.Center, Color.FromArgb(0xff, 0x89, 0x89, 0x89)));
                grRule.Children.Add(createText("", "ID", 12d, FontWeights.Normal, new Thickness(26, 42, 0, 0), 65, HorizontalAlignment.Left, VerticalAlignment.Top, TextAlignment.Center, Color.FromArgb(0xff, 0x89, 0x89, 0x89)));
                grRule.Children.Add(createImage("", symbol, new Thickness(0, 24, 5, 0), 15d, HorizontalAlignment.Right, VerticalAlignment.Top, true, false));

                // grRule.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(drRule["Color"].ToString()));
                grRule.Children.Add(createImage("", "spinner.png", new Thickness(0, -10, -10, 0), 22d, HorizontalAlignment.Right, VerticalAlignment.Top, false, true));
                border.Child = grRule;
                spRules.Children.Add(border);

            }
        }

        private void GrRule_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (!(sender is Grid))
                return;

            ////// Clear others //////
            foreach (object child in spRules.Children)
            {
                if (child is Border)
                {
                    Border b = (Border) child;
                    if (b.Child == sender)
                    {
                        b.BorderBrush = new SolidColorBrush(Colors.Black);
                        b.BorderThickness = new Thickness(3);
                        foreach (var gridChild in ((Grid)b.Child).Children)
                            if ((gridChild is TextBlock) && (((TextBlock)gridChild).Name.StartsWith("ID")))
                            {
                                fillAttributes(((TextBlock)gridChild).Text);
                                showProblems(((TextBlock)gridChild).Text);
                            }
                    }
                    else
                    {
                        b.BorderBrush = new SolidColorBrush(Color.FromArgb(0xff, 205, 206, 208));
                        b.BorderThickness = new Thickness(1);

                    }
                }
                //                    child.BorderBrush = new SolidColorBrush(Colors.White);

            }

        }

        private void showProblems(string id)
        {
            DataRow [] drsubproblems = problems.Select("RuleImplementationID = '" + id + "'");
            if (drsubproblems.Length > 0)
                dgProblems.ItemsSource = drsubproblems.CopyToDataTable().DefaultView;
            else
                dgProblems.ItemsSource = null;
        }

        private void fillAttributes(string id)
        {
            DataTable dtruleAttributes = engineRuleAttribute.Clone();

            foreach (DataRow drRule in engineRuleImplementation.Rows)
            {
                if ((drRule["ProcessID"].ToString() == currentProcessID) && (drRule["RuleImplementationID"].ToString() == id))
                {
                    foreach (DataRow drAttr in engineRuleAttribute.Rows)
                    {
                        if (drAttr["RuleAttributeSetID"].ToString() == drRule["RuleAttributeSetID"].ToString())
                        {
                            dtruleAttributes.ImportRow(drAttr);
                        }
                    }
                }
            }

            dgAttributes.ItemsSource = dtruleAttributes.DefaultView;
        }

        private TextBlock createText(string name, string text, double fontSize, FontWeight weight, Thickness margin, double width, HorizontalAlignment horiz, VerticalAlignment vert, TextAlignment textalign, Color foreground)
        {
            TextBlock textblock = new TextBlock();
            textblock.Text = text;
            textblock.FontSize = fontSize;
            textblock.FontWeight = weight;
            textblock.Margin = margin;
            textblock.Width = width;
            textblock.HorizontalAlignment = horiz;
            textblock.VerticalAlignment = vert;
            textblock.TextAlignment = textalign;
            textblock.Foreground = new SolidColorBrush(foreground);
            textblock.Name = name + "X" + controlCounter++.ToString();
            return textblock;
        }

        private Image createImage(string name, string imagename, Thickness margin, double width, HorizontalAlignment horiz, VerticalAlignment vert, bool visible, bool hasTransformAngle)
        {
            Image img = new Image();

            img.Margin = margin;
            img.Source = new BitmapImage(new Uri(@"Images\" + imagename, UriKind.Relative));
            img.Width = width;
            img.HorizontalAlignment = horiz;
            img.VerticalAlignment = vert;
            img.Name = name + "I" + controlCounter++.ToString();
            img.Visibility = (visible) ? Visibility.Visible : Visibility.Collapsed;

            if (hasTransformAngle)
            {
                RotateTransform rt = new RotateTransform();
                rt.Angle = 120;
                TransformGroup tg = new TransformGroup();
                tg.Children.Add(rt);
                img.RenderTransformOrigin = new Point(0.5, 0.5);
                img.RenderTransform = tg;
            }

            return img;
        }


        private void Grid_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void dgProcess_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOpenClose.Content = "Choose";
            // gridProcess.Visibility = Visibility.Collapsed;
            CloseProcessChooser();
            loadAndCheckRules();
            dgProblems.ItemsSource = null;
            dgAttributes.ItemsSource = null;
        }

        private void loadAndCheckRules()
        {
            problems = new DataTable();
            problems.Columns.Add("RuleImplementationID");
            problems.Columns.Add("Problem");

            if (dgProcess.SelectedIndex >= 0)
            {
                DataRow dr = ((System.Data.DataRowView)(dgProcess.SelectedItem)).Row;
                checkRules(dr["ProcessID"].ToString());
                FillRules(dr["ProcessID"].ToString());
                lblTitle1.Text = "Process " + dr["ProcessID"].ToString() + " - " + dr["Name"].ToString();
            }
        }


        private void checkRules(string processID)
        {
            localRepositories = new List<string>();

            foreach (DataRow drRule in engineRuleImplementation.Rows)
            {
                if (drRule["ProcessID"].ToString() == processID)
                    checkRule(drRule);
            }
        }

        private void checkRule(DataRow drRule)
        {
            string ruleType = drRule["Name"].ToString();
            string ID = drRule["RuleImplementationID"].ToString();

            DataRow []attributes = engineRuleAttribute.Select("RuleAttributeSetID='" + drRule["RuleAttributeSetID"].ToString() + "'");
            //drRule["Color"] = "PeachPuff";
            drRule["Color"] = "White";

            ////// CHECK TO SEE IF SOURCE REPOSITORY EXISTS //////
            string source = getAttribute(attributes, "SourceRepositoryName ");
            if ((source != "") && (localRepositories.Contains(source) == false))
            {
                drRule["Color"] = "LightCoral";
                problems.Rows.Add(ID, "SourceRepositoryName " + source + " does not exist");
            }
            string source2 = getAttribute(attributes, "SourceRepositoryLookupName");
            if ((source2 != "") && (localRepositories.Contains(source2) == false))
            {
                drRule["Color"] = "LightCoral";
                problems.Rows.Add(ID, "SourceRepositoryName " + source2 + " does not exist");
            }

            if (ruleType == "DirectMap")
                checkDirectMap(ID, drRule, attributes);

            if (ruleType == "Lookup")
                checkLookup(ID, drRule, attributes);

            if (ruleType == "TableFormat")
                checkTableFormat(ID, drRule, attributes);

            if (ruleType == "Generate")
                checkGenerate(ID, drRule, attributes);

            ////// STORE AWAY TARGET REPOSITORY IF THE RULE IS ENABLED AND WE ARE LOCAL OR ALL //////
            string target = getAttribute(attributes, "TargetRepositoryName");
            string returntarget = getAttribute(attributes, "ReturnTarget").ToUpper();
            if ((drRule["IsActive"].ToString() == "True") && ((returntarget == "LOCAL") || (returntarget == "ALL")) && (target != ""))
                localRepositories.Add(target);

            ////// Look for required //////
            foreach (valueCheck v in valueChecks)
            {
                if (v.ruleType == ruleType)
                {
                    if (!checkForAttribute(drRule, attributes, ID, v.attributeName))
                    {
                        drRule["Color"] = "LightCoral";
                        problems.Rows.Add(ID, "Attribute " + v.attributeName + " is missing");
                    }

                    else
                    {
                        if (v.attributeName == "ReturnTarget")
                        {
                            if (!checkReturnTarget(getAttribute(attributes, v.attributeName)))
                            {
                                drRule["Color"] = "LightCoral";
                                problems.Rows.Add(ID, "Return attribute must be LOCAL, RETURN, ALL or NONE");
                            }

                        }
                    }
                }
            }
        }

        private void checkDirectMap(string RuleImplementationID, DataRow drRule, DataRow[] attributes)
        {
            foreach (DataRow dr in attributes)
            {
                string attrName = dr["RuleImplementationAttributeName"].ToString();

                if (attrName.IndexOf(".") > 1)
                {
                    if (attrName.StartsWith("Mappings.") == false)
                    {
                        drRule["Color"] = "LightCoral";
                        problems.Rows.Add(RuleImplementationID, "Mapping Attribute " + attrName + " does not start with Mappings.");
                    }

                    if ((attrName.EndsWith(".SourceColumn") == false) && (attrName.EndsWith(".TargetColumn") == false))
                    {
                        drRule["Color"] = "LightCoral";
                        problems.Rows.Add(RuleImplementationID, "Mapping Attribute " + attrName + " does not end with .SourceColumn or .TargetColumn");
                    }

                    if (attrName.EndsWith(".SourceColumn"))
                    {
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".Source",".Target")))
                        {
                            drRule["Color"] = "LightCoral";
                            problems.Rows.Add(RuleImplementationID, "Mapping Attribute " + attrName + " does not have a corresponding .TargetColumn");
                        }
                    }

                    if (attrName.EndsWith(".TargetColumn"))
                    {
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".Target", ".Source")))
                        {
                            drRule["Color"] = "LightCoral";
                            problems.Rows.Add(RuleImplementationID, "Mapping Attribute " + attrName + " does not have a corresponding .SourceColumn");
                        }
                    }
                }


            }

        }

        private void checkTableFormat(string RuleImplementationID, DataRow drRule, DataRow[] attributes)
        {
            foreach (DataRow dr in attributes)
            {
                string attrName = dr["RuleImplementationAttributeName"].ToString();

                if (attrName.IndexOf(".") > 1)
                {
                    if (attrName.StartsWith("Columns.") == false)
                    {
                        drRule["Color"] = "LightCoral";
                        problems.Rows.Add(RuleImplementationID, "TableFormat Attribute " + attrName + " does not start with Columns.");
                    }

                    if ((attrName.EndsWith(".OrderNumber") == false) && (attrName.EndsWith(".ColumnName") == false))
                    {
                        drRule["Color"] = "LightCoral";
                        problems.Rows.Add(RuleImplementationID, "TableFormat Attribute " + attrName + " does not end with .OrderNumber or .ColumnName");
                    }

                    if (attrName.EndsWith(".OrderNumber"))
                    {
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".OrderNumber", ".ColumnName")))
                        {
                            drRule["Color"] = "LightCoral";
                            problems.Rows.Add(RuleImplementationID, "TableFormat Attribute " + attrName + " does not have a corresponding .ColumnName");
                        }
                    }

                    if (attrName.EndsWith(".ColumnName"))
                    {
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".ColumnName", ".OrderNumber")))
                        {
                            drRule["Color"] = "LightCoral";
                            problems.Rows.Add(RuleImplementationID, "TableFormat Attribute " + attrName + " does not have a corresponding .OrderNumber");
                        }
                    }
                }
            }
        }

        private void checkGenerate(string RuleImplementationID, DataRow drRule, DataRow[] attributes)
        {
            foreach (DataRow dr in attributes)
            {
                string attrName = dr["RuleImplementationAttributeName"].ToString();
                string attrValue = dr["RuleImplementationAttributeValue"].ToString();

                if (attrName.IndexOf(".") > 1)
                {
                    if (attrName.StartsWith("Generate.") == false)
                        logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " does not start with Generate.");

                    if ((attrName.EndsWith(".FunctionType") == false) && (attrName.EndsWith(".FunctionValue") == false) && (attrName.EndsWith(".TargetColumn") == false))
                        logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " does not end with .FunctionType, .FunctionValue or .TargetColumn");

                    if (attrName.EndsWith(".FunctionType"))
                    {
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".FunctionType", ".FunctionValue")))
                            logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " does not have a corresponding .FunctionValue");
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".FunctionType", ".TargetColumn")))
                            logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " does not have a corresponding .TargetColumn");
                        if ((attrValue!="STATIC") && (attrValue!="FUNCTION"))
                            logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " value must be STATIC or FUNCTION");
                    }

                    if (attrName.EndsWith(".FunctionValue"))
                    {
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".FunctionValue", ".FunctionType")))
                            logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " does not have a corresponding .FunctionType");
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".FunctionValue", ".TargetColumn")))
                            logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " does not have a corresponding .TargetColumn");
                    }

                    if (attrName.EndsWith(".TargetColumn"))
                    {
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".TargetColumn", ".FunctionValue")))
                            logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " does not have a corresponding .FunctionValue");
                        if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".TargetColumn", ".FunctionType")))
                            logProblem(RuleImplementationID, drRule, "Generate Attribute " + attrName + " does not have a corresponding .FunctionType");
                    }
                }
            }
        }

        private void logProblem(string RuleImplementationID, DataRow drRule, string text)
        {
            drRule["Color"] = "LightCoral";
            problems.Rows.Add(RuleImplementationID, text);
        }


        private void checkLookup(string RuleImplementationID, DataRow drRule, DataRow[] attributes)
        {
            foreach (DataRow dr in attributes)
            {
                string attrName = dr["RuleImplementationAttributeName"].ToString();

                if (attrName.IndexOf(".") > 1)
                {
                    if ((attrName.StartsWith("JoinClauses.") == false) && (attrName.StartsWith("AppendColumns.") == false))
                    {
                        drRule["Color"] = "LightCoral";
                        problems.Rows.Add(RuleImplementationID, "Lookup Attribute " + attrName + " does not start with JoinClauses. or AppendColumns.");
                    }

                    if (attrName.StartsWith("JoinClauses."))
                    {
                        if ((attrName.EndsWith(".SourceColumn") == false) && (attrName.EndsWith(".LookupColumn") == false))
                        {
                            drRule["Color"] = "LightCoral";
                            problems.Rows.Add(RuleImplementationID, "Lookup Attribute Join Clause " + attrName + " does not end with .SourceColumn or .TargetColumn");
                        }

                        if (attrName.EndsWith(".SourceColumn"))
                        {
                            if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".Source", ".Lookup")))
                            {
                                drRule["Color"] = "LightCoral";
                                problems.Rows.Add(RuleImplementationID, "Lookup Join Clause Attribute " + attrName + " does not have a corresponding .TargetColumn");
                            }
                        }

                        if (attrName.EndsWith(".LookupColumn"))
                        {
                            if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".Lookup", ".Source")))
                            {
                                drRule["Color"] = "LightCoral";
                                problems.Rows.Add(RuleImplementationID, "Lookup Join Clause Attribute " + attrName + " does not have a corresponding .SourceColumn");
                            }
                        }
                    }


                    if (attrName.StartsWith("AppendColumns."))
                    {
                        if ((attrName.EndsWith(".SourceColumn") == false) && (attrName.EndsWith(".TargetColumn") == false))
                        {
                            drRule["Color"] = "LightCoral";
                            problems.Rows.Add(RuleImplementationID, "Lookup Attribute Append Columns " + attrName + " does not end with .SourceColumn or .TargetColumn");
                        }

                        if (attrName.EndsWith(".SourceColumn"))
                        {
                            if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".Source", ".Target")))
                            {
                                drRule["Color"] = "LightCoral";
                                problems.Rows.Add(RuleImplementationID, "Lookup Append Attribute " + attrName + " does not have a corresponding .TargetColumn");
                            }
                        }

                        if (attrName.EndsWith(".TargetColumn"))
                        {
                            if (!checkForAttribute(drRule, attributes, RuleImplementationID, attrName.Replace(".Target", ".Source")))
                            {
                                drRule["Color"] = "LightCoral";
                                problems.Rows.Add(RuleImplementationID, "Lookup Append Attribute " + attrName + " does not have a corresponding .SourceColumn");
                            }
                        }
                    }


                }


            }

        }

        private bool checkReturnTarget(string s)
        {
            return (s == "LOCAL" || s == "RETURN" || s == "ALL" || s == "NONE");
        }

        private string getAttribute(DataRow[] attributes, string attrName)
        {
            foreach (DataRow dr in attributes)
                if (dr["RuleImplementationAttributeName"].ToString() == attrName)
                    return dr["RuleImplementationAttributeValue"].ToString();

            return "";
        }

        private bool checkForAttribute(DataRow drRule, DataRow[] attributes, string id, string attrName)
        {
            foreach (DataRow dr in attributes)
                if (dr["RuleImplementationAttributeName"].ToString() == attrName)
                    return true;

            return false;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            getAllData();
            loadAndCheckRules();
            dgProblems.ItemsSource = null;
            dgAttributes.ItemsSource = null;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
        }

        private void tbSearchFor_KeyDown(object sender, KeyEventArgs e)
        {
            if (((SolidColorBrush) tbSearchFor.Foreground).Color != Colors.White)
            {
                tbSearchFor.Text = "";
                tbSearchFor.Foreground = new SolidColorBrush(Colors.White);
            }
            if (e.Key == Key.Return)
            {
                double offset = 0d;
                foreach (object child in spRules.Children)
                {
                    if (child is Border)
                    {
                        Border b = (Border)child;
                        {
                            foreach (var gridChild in ((Grid)b.Child).Children)
                                if (gridChild is TextBlock)
                                {
                                    TextBlock tb = ((TextBlock)gridChild);
                                    if ((tb.Name.StartsWith("ID") || tb.Name.StartsWith("EO")) && (tb.Text == tbSearchFor.Text))
                                    {
                                        sv.ScrollToVerticalOffset(offset);
                                        tbSearchFor.Text = "search";
                                        tbSearchFor.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xB1, 0xB6, 0xC1));
                                        return;
                                    }

                                }
                            offset += 76d;
                        }
                    }
                }
            }
        }

        private void btnShowBad_Click(object sender, RoutedEventArgs e)
        {
            if (btnShowBad.Content.ToString() == "show bad")
            {
                tbFilter.Text = "error";
                tbFilter.Foreground = new SolidColorBrush(Color.FromArgb(0xFF, 0xB1, 0xB6, 0xC1));
                if (dgProcess.SelectedIndex >= 0)
                {
                    DataRow dr = ((System.Data.DataRowView)(dgProcess.SelectedItem)).Row;
                    FillRules(dr["ProcessID"].ToString());
                }

                tbSearchFor.Visibility = Visibility.Collapsed;
                tbFilter.Visibility = Visibility.Collapsed;
                btnClearFilter.Visibility = Visibility.Collapsed;
                btnShowBad.Content = "show all";
            }

            else
            {
                tbFilter.Text = "filter";
                if (dgProcess.SelectedIndex >= 0)
                {
                    DataRow dr = ((System.Data.DataRowView)(dgProcess.SelectedItem)).Row;
                    FillRules(dr["ProcessID"].ToString());
                }
                tbSearchFor.Visibility = Visibility.Visible;
                tbFilter.Visibility = Visibility.Visible;
                btnClearFilter.Visibility = Visibility.Visible;
                btnShowBad.Content = "show bad";
            }


        }

        private void tbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (((SolidColorBrush)tbFilter.Foreground).Color != Colors.White)
            {
                tbFilter.Text = "";
                tbFilter.Foreground = new SolidColorBrush(Colors.White);
            }
            if (e.Key == Key.Return)
            {
                if (dgProcess.SelectedIndex >= 0)
                {
                    DataRow dr = ((System.Data.DataRowView)(dgProcess.SelectedItem)).Row;
                    //checkRules(dr["ProcessID"].ToString());
                    FillRules(dr["ProcessID"].ToString());
                    //lblTitle1.Text = "Process " + dr["ProcessID"].ToString() + " - " + dr["Name"].ToString();
                }
            }
        }

        private void btnClearFilter_Click(object sender, RoutedEventArgs e)
        {
            tbFilter.Text = "filter";
            tbFilter.Foreground = new SolidColorBrush(Color.FromArgb(0xFF,0xB1,0xB6,0xC1));
            if (dgProcess.SelectedIndex >= 0)
            {
                DataRow dr = ((System.Data.DataRowView)(dgProcess.SelectedItem)).Row;
                FillRules(dr["ProcessID"].ToString());
            }
        }

        private void btnEnable_Click(object sender, RoutedEventArgs e)
        {
            string id = selectedRule();
            enableRule(id, "1");
        }

        private void btnDisable_Click(object sender, RoutedEventArgs e)
        {
            string id = selectedRule();
            enableRule(id, "0");
        }

        private void enableRule(string id, string enabled)
        {
            executeSQL("update engine.RuleImplementation set IsActive=" + enabled + " where RuleImplementationID=" + id, connectionString);

            getAllData();
            loadAndCheckRules();
            dgProblems.ItemsSource = null;
            dgAttributes.ItemsSource = null;
        }

        private string selectedRule()
        {
            foreach (object child in spRules.Children)
            {
                if (child is Border)
                {
                    Border b = (Border)child;
                    {
                        if (((SolidColorBrush)b.BorderBrush).Color == Colors.Black)
                        {
                            b.BorderBrush = new SolidColorBrush(Colors.Black);
                            foreach (var gridChild in ((Grid)b.Child).Children)
                                if ((gridChild is TextBlock) && (((TextBlock)gridChild).Name.StartsWith("ID")))
                                    return ((TextBlock)gridChild).Text;
                        }
                    }
                }
            }
            return "";
        }

        private static void executeSQL(string sql, string connectionString)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlCommand command = con.CreateCommand())
                    {
                        command.CommandText = sql;
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            startingBatchRulesEcecutionID = getCurrentBatchRuleExecutionID();

            // from currentRuleForExecution
            StartRuleID = (currentRuleForExecution == "") ? -1 : Convert.ToInt32(currentRuleForExecution);
            StopRuleID = -1;

            bool lookingForStop = false;
            foreach (DataRow drRule in engineRuleImplementation.Rows)
            {
                if (drRule["ProcessID"].ToString() != currentProcessID)
                    continue;

                string implementationID = drRule["RuleImplementationID"].ToString();

                if ((currentRuleForExecution == "") || implementationID == currentRuleForExecution)
                    lookingForStop = true;

                if (lookingForStop)
                {
                    int current = -1;
                    Int32.TryParse(implementationID, out current);

                    if (breakpoints.Contains(current))
                    {
                        StopRuleID = Convert.ToInt32(implementationID);
                        break;
                    }
                }
            }

            int a = 4;
            clearAllStatus();
            complete = false;

            tExecuteRules = new Thread(executeRules);
            tExecuteRules.IsBackground = true;
            tExecuteRules.Start();


            // MessageBox.Show("Process " + currentProcessID + " from rule " + StartRuleID + " to but not including " + StopRuleID);
            //txtResults.Text = results.GetXml();

        }


        private string getCurrentBatchRuleExecutionID()
        {
            string sql = "SELECT TOP 1 [BatchRuleExecutionID] FROM [IAFramework].[rules].[BatchRuleExecution]  order by batchruleexecutionid desc";
            DataTable results = getData(sql);
            if (results.Rows.Count > 0)
                return results.Rows[0]["BatchRuleExecutionID"].ToString();
            return "0";
        }

        static void executeRules()
        {
            int batchid = 100;
            var srRE = new srRulesEngineLocal.RuleEngineSoapEndpointClient();
            srRE.Endpoint.Binding.CloseTimeout = new TimeSpan(0, 20, 0);
            srRE.Endpoint.Binding.ReceiveTimeout = new TimeSpan(0, 20, 0);
            srRE.Endpoint.Binding.SendTimeout = new TimeSpan(0, 20, 0);
            srRE.Endpoint.Binding.OpenTimeout = new TimeSpan(0, 20, 0);

            try
            {
                srRE.ExecuteRulesEx(Convert.ToInt32(currentProcessID), Convert.ToInt32(batchid), false, StartRuleID, StopRuleID, true, true);
            }
            catch (Exception)
            {
            }
            complete = true;

            //code goes here
        }

        private void btnSetCurrentRule(object sender, RoutedEventArgs e)
        {
            currentRuleForExecution = selectedRule();
            DataRow dr = ((System.Data.DataRowView)(dgProcess.SelectedItem)).Row;
            FillRules(dr["ProcessID"].ToString());
        }

        private void btnSetOrClearBreakpoint(object sender, RoutedEventArgs e)
        {
            int current = -1;
            if (Int32.TryParse(selectedRule(), out current) == false)
                return;

            if (breakpoints.Contains(current))
                breakpoints.Remove(current);
            else
                breakpoints.Add(current);

            DataRow dr = ((System.Data.DataRowView)(dgProcess.SelectedItem)).Row;
            FillRules(dr["ProcessID"].ToString());

        }

        private void btnStep_Click(object sender, RoutedEventArgs e)
        {
            if (currentProcessID == "")
                return;

            setStatus("139","spinner.png");
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            setStatus(currentExecutingRule, "success.png");
            return;

            if (currentProcessID == "")
                return;

            DataTable localTables = getData("select * from INFORMATION_SCHEMA.TABLES where TABLE_SCHEMA = 'local' and TABLE_NAME like 'T" + currentProcessID + "[_]%' order by TABLE_NAME");

            foreach (DataRow dr in localTables.Rows)
            {
                DropTable("local."+dr["TABLE_NAME"].ToString());
            }

        }

        private void DropTable(string tableName)
        {
            string sql = "IF OBJECT_ID('" + tableName + "', 'U') IS NOT NULL DROP TABLE " + tableName + "";
            executeSQL(sql, connectionString);
        }

        private void btnLocalTables_Click(object sender, RoutedEventArgs e)
        {
            showTables(local: true);
        }

        private void btnRules_Click(object sender, RoutedEventArgs e)
        {
            showRules();
        }

        private void btnStatus_Click(object sender, RoutedEventArgs e)
        {
            showStatus();
        }

        private void btnReferenceTables_Click(object sender, RoutedEventArgs e)
        {
            showTables(local: false);
        }


        private void showTables(bool local)
        {
            showRadioButtonStatus((local) ? btnLocalTables : btnReferenceTables, new Button[] { btnLocalTables, btnRules, btnStatus, btnReferenceTables });
            gridRules.Visibility = Visibility.Collapsed;
            gridTables.Visibility = Visibility.Visible;
            gridStatus.Visibility = Visibility.Collapsed;
            tables.setTargetType(local, currentProcessID);
        }

        private void showRules()
        {
            showRadioButtonStatus(btnRules, new Button[] { btnLocalTables, btnRules, btnStatus, btnReferenceTables });
            gridRules.Visibility = Visibility.Visible;
            gridTables.Visibility = Visibility.Collapsed;
            gridStatus.Visibility = Visibility.Collapsed;
        }

        private void showStatus()
        {
            showRadioButtonStatus(btnStatus, new Button[] { btnLocalTables, btnRules, btnStatus, btnReferenceTables });
            gridRules.Visibility = Visibility.Collapsed;
            gridTables.Visibility = Visibility.Collapsed;
            gridStatus.Visibility = Visibility.Visible;
            UpdateStatus();
        }

        public void showRadioButtonStatus(Button btnSet, Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                    if (c is Rectangle)
                        ((Rectangle)c).Opacity = (b == btnSet) ? 1d : 0.2d;
        }

        public string getRadioButtonStatus(Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                    if (c is Rectangle)
                        if (((Rectangle)c).Opacity == 1d)
                            return b.Name;
            return "";
        }

        public void UpdateStatus()
        {
            var srRE = new srRulesEngineLocal.RuleEngineSoapEndpointClient();
            DataSet results = srRE.GetConnectionStrings();
            dgConnections.ItemsSource = results.Tables[0].DefaultView;

            DataSet statusResults = srRE.ServerStatus();
            dgStatus.ItemsSource = statusResults.Tables[0].DefaultView;
        }

        private void btnEnvLocal_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnEnvLocal, new Button[] { btnEnvLocal, btnEnvDev, btnEnvUAT, btnEnvTest, btnEnvProd });
        }

        private void btnEnvDev_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnEnvDev, new Button[] { btnEnvLocal, btnEnvDev, btnEnvUAT, btnEnvTest, btnEnvProd });
        }

        private void btnEnvTest_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnEnvTest, new Button[] { btnEnvLocal, btnEnvDev, btnEnvUAT, btnEnvTest, btnEnvProd });
        }

        private void btnEnvUAT_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnEnvUAT, new Button[] { btnEnvLocal, btnEnvDev, btnEnvUAT, btnEnvTest, btnEnvProd });
        }

        private void btnEnvProd_Click(object sender, RoutedEventArgs e)
        {
            showRadioButtonStatus(btnEnvProd, new Button[] { btnEnvLocal, btnEnvDev, btnEnvUAT, btnEnvTest, btnEnvProd });
        }
    }

}
