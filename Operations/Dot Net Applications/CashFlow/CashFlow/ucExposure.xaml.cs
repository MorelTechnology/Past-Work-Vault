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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CashFlow
{
    /// <summary>
    /// Interaction logic for ucExposure.xaml
    /// </summary>
    public partial class ucExposure : UserControl
    {
        MainWindow ourParent = null;
        string selectedPeriod = "";
        string selectedValue = "";
        double dolval = 0d;
        public DataTable dtEmptyNumbersWMforSplit = null;
        bool inAllocationMode = false;

        // for now.... it's public :(
        public Dictionary<string, int> dicFound = new Dictionary<string, int>();
        TextBlock[] labelPercentages;
        Label[] labelPortfolios;

        #region Animation state
        public class optionAnimation
        {
            public enum oaStatus { open, opening, closed, closing };
            public oaStatus eStatus = oaStatus.closed;
            public double progress = 0;
            public double closedHeight = 0d;
            public double fullHeight = 180d;
        }
        optionAnimation oa = new optionAnimation();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        #endregion

        
        public ucExposure()
        {
            InitializeComponent();

            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            dispatcherTimer.Start();

            labelPercentages = new TextBlock[] { labelPercentage1, labelPercentage2, labelPercentage3, labelPercentage4, labelPercentage5 };
            labelPortfolios = new Label[] { labelPortfolio1, labelPortfolio2, labelPortfolio3, labelPortfolio4, labelPortfolio5 };

        }

        public void setupActualLabels(int year1, int qtr1, int year2, int qtr2)
        {
            colQ1DE.Header = year1.ToString() + " Q" + qtr1.ToString() + Environment.NewLine + "Paid Def Exp";
            colQ1DEDelta.Header = year1.ToString() + " Q" + qtr1.ToString() + Environment.NewLine + "Paid Def Exp";

            colQ2DE.Header = year2.ToString() + " Q" + qtr2.ToString() + Environment.NewLine + "Paid Def Exp";
            colQ2DEDelta.Header = year2.ToString() + " Q" + qtr2.ToString() + Environment.NewLine + "Paid Def Exp";
        }

        public double getSliderValeByPortfolio(string portfolio)
        {
            for (int i = 0; i < labelPortfolios.Count(); i++)
                if (labelPortfolios[i].Content.ToString() == portfolio)
                    return Convert.ToDouble(labelPercentages[i].Text.ToString().Replace("%","")) / getTotalSliderValues();
            return 0d;
        }

        public double getTotalSliderValues()
        {
            double total = 0d;
            for (int i = 0; i < labelPortfolios.Count(); i++)
                if (labelPortfolios[i].Visibility == Visibility.Visible)
                    total += Convert.ToDouble(labelPercentages[i].Text.ToString().Replace("%", ""));
            return total;
        }

        public void setParent(MainWindow mw)
        {
            ourParent = mw;
        }

        #region Animation

        public void blowOpen(bool thenSave)
        {
            //rowDefTop.BeginAnimation(RowDefinition.HeightProperty, null);

            //DoubleAnimation da = new DoubleAnimation();
            //da.From = 0d;
            //da.To = 180d;
            //da.Duration = new Duration(TimeSpan.FromSeconds(2d));
            //rowDefTop.BeginAnimation(RowDefinition.HeightProperty, da);
            lblSlidersAreShowing.Content = "";
            lblEnterWorkMatter.Visibility = Visibility.Visible;
            dgNumbers.Visibility = Visibility.Visible;
            gridSplitters.Visibility = Visibility.Visible;

            ThicknessAnimation ta = new ThicknessAnimation();
            if (dgExposures.Margin.Top <= 10d)
            {
                //ta.From = new Thickness(10, 10, 10, 11);
                //ta.To = new Thickness(10, 200, 10, 11);
                ta.From = new Thickness(10, 10, 10, 38);
                ta.To = new Thickness(10, 200, 10, 38);
            }
            else
            {
                //ta.To = new Thickness(10, 10, 10, 11);
                //ta.From = new Thickness(10, 200, 10, 11);
                ta.To = new Thickness(10, 10, 10, 38);
                ta.From = new Thickness(10, 200, 10, 38);
            }

            ta.Completed += (ss, ee) =>
            {
                ourParent.showOrObscureForAllocations();

                if (dgExposures.Margin.Top > 20)
                {
                    lblEnterWorkMatter.Visibility = Visibility.Visible;
                    dgNumbers.Visibility = Visibility.Visible;
                    gridSplitters.Visibility = Visibility.Visible;
                    handleExposureSelectionChangesInAllocation();
                    //dgExposures.IsEnabled = false;
                    inAllocationMode = true;
                }
                else
                {
                    lblEnterWorkMatter.Visibility = Visibility.Collapsed;
                    dgNumbers.Visibility = Visibility.Collapsed;
                    gridSplitters.Visibility = Visibility.Collapsed;
                    if (thenSave)
                        ourParent.saveAction();
                    inAllocationMode = false;
                    dgExposures.IsEnabled = true;
                    //handleExposureSelectionChanges();
                }

            };

            dgExposures.BeginAnimation(MarginProperty, ta);

            //if (rowDefTop.Height.Value > 0d)
            //    oa.eStatus = optionAnimation.oaStatus.closing;
            //else
            //    oa.eStatus = optionAnimation.oaStatus.opening;

        }

        public void showSplit()
        {
            //         <Grid x:Name="gridSplitters" Height="167" Margin="362,10,10,0" VerticalAlignment="Top" >
            showLoss();

            bool open = false;

            ThicknessAnimation ta = new ThicknessAnimation();
            if (gridSplitters.Margin.Top <= 10d)
            {
                ta.From = new Thickness(362, 10, 10, 0);
                ta.To = new Thickness(362, 47, 10, 0);
                open = true;
            }
            else
            {
                ta.From = new Thickness(362, 47, 10, 0);
                ta.To = new Thickness(362, 10, 10, 0);
            }
            gridSplitters.BeginAnimation(MarginProperty, ta);

            DoubleAnimation da = new DoubleAnimation();
            if (open)
            {
                da.From = 167;
                da.To = 130;
            }
            else
            {
                da.From = 130;
                da.To = 167;
            }
            gridSplitters.BeginAnimation(HeightProperty, da);



        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //if (oa.eStatus == optionAnimation.oaStatus.opening)
            //{
            //    rowDefTop.Height = new GridLength(rowDefTop.Height.Value + 20d);
            //    if (rowDefTop.Height.Value >= 180d)
            //        oa.eStatus = optionAnimation.oaStatus.open;
            //}

            //if (oa.eStatus == optionAnimation.oaStatus.closing)
            //{
            //    GridLength gl = rowDefTop.Height;
            //    double subAmount = (gl.Value < 5d) ? gl.Value : 20d;
            //    rowDefTop.Height = new GridLength(rowDefTop.Height.Value - subAmount);
            //    if (rowDefTop.Height.Value <= 0)
            //        oa.eStatus = optionAnimation.oaStatus.closed;
            //}

        }
        #endregion Animation


        public void fillExposures(string workmatter)
        {
            lblNotInOrOutWarning.Visibility = Visibility.Collapsed;

            MainWindow.countIt("");
            DataTable dtExp = ourParent.ourData.getExposuresForWorkMatter(workmatter);
            MainWindow.countIt("EE ourParent.ourData.getExposuresForWorkMatter");

            dgExposures.ItemsSource = dtExp.DefaultView;

            labelPercentage1.Visibility = Visibility.Collapsed;
            labelPercentage2.Visibility = Visibility.Collapsed;
            labelPercentage3.Visibility = Visibility.Collapsed;
            labelPercentage4.Visibility = Visibility.Collapsed;
            labelPercentage5.Visibility = Visibility.Collapsed;

            labelPortfolio1.Visibility = Visibility.Collapsed;
            labelPortfolio2.Visibility = Visibility.Collapsed;
            labelPortfolio3.Visibility = Visibility.Collapsed;
            labelPortfolio4.Visibility = Visibility.Collapsed;
            labelPortfolio5.Visibility = Visibility.Collapsed;

            slider1.Visibility = Visibility.Collapsed;
            slider2.Visibility = Visibility.Collapsed;
            slider3.Visibility = Visibility.Collapsed;
            slider4.Visibility = Visibility.Collapsed;
            slider5.Visibility = Visibility.Collapsed;

            labelDollar1.Visibility = Visibility.Collapsed;
            labelDollar2.Visibility = Visibility.Collapsed;
            labelDollar3.Visibility = Visibility.Collapsed;
            labelDollar4.Visibility = Visibility.Collapsed;
            labelDollar5.Visibility = Visibility.Collapsed;

            // TEMP
            //List<string> lsFound = new List<string>();
            MainWindow.countIt("EE Point A");
            dicFound = new Dictionary<string, int>();
            int count = 0;
            // MAY 23 2018 foreach (DataRow dr in dtExp.Rows)
            foreach (DataRow dr in dtExp.Rows)
            {
                string portfolioName = dr["Portfolio"].ToString();
                if (dicFound.ContainsKey(portfolioName) == true)
                {
                    dicFound[portfolioName] += 1;
                }

                else
                {
                    dicFound.Add(portfolioName, 1);
                    count++;
                    if (count == 1)
                    {
                        labelPercentage1.Visibility = Visibility.Visible;
                        labelPortfolio1.Visibility = Visibility.Visible;
                        slider1.Visibility = Visibility.Visible;
                        labelDollar1.Visibility = Visibility.Visible;
                        labelPortfolio1.Content = portfolioName;
                    }
                    if (count == 2)
                    {
                        labelPercentage2.Visibility = Visibility.Visible;
                        labelPortfolio2.Visibility = Visibility.Visible;
                        slider2.Visibility = Visibility.Visible;
                        labelDollar2.Visibility = Visibility.Visible;
                        labelPortfolio2.Content = portfolioName;
                    }
                    if (count == 3)
                    {
                        labelPercentage3.Visibility = Visibility.Visible;
                        labelPortfolio3.Visibility = Visibility.Visible;
                        slider3.Visibility = Visibility.Visible;
                        labelDollar3.Visibility = Visibility.Visible;
                        labelPortfolio3.Content = portfolioName;
                    }
                    if (count == 4)
                    {
                        labelPercentage4.Visibility = Visibility.Visible;
                        labelPortfolio4.Visibility = Visibility.Visible;
                        slider4.Visibility = Visibility.Visible;
                        labelDollar4.Visibility = Visibility.Visible;
                        labelPortfolio4.Content = portfolioName;
                    }
                    if (count == 5)
                    {
                        labelPercentage5.Visibility = Visibility.Visible;
                        labelPortfolio5.Visibility = Visibility.Visible;
                        slider5.Visibility = Visibility.Visible;
                        labelDollar5.Visibility = Visibility.Visible;
                        labelPortfolio5.Content = portfolioName;
                    }
                }
            }
            MainWindow.countIt("EE ForLoop");


            slider2.Value = 0d;
            slider3.Value = 0d;
            slider4.Value = 0d;
            slider5.Value = 0d;

            if (count == 1)
            {
                labelPercentage1.Text = "100%";
                slider1.Value = 10d;
            }

            if (count ==2 )
            {
                labelPercentage1.Text = "50%";
                labelPercentage2.Text = "50%";
                slider1.Value = 5d;
                slider2.Value = 5d;
            }
            if (count == 3)
            {
                labelPercentage1.Text = "33%";
                labelPercentage2.Text = "33%";
                labelPercentage3.Text = "34%";
                slider1.Value = 3.3d;
                slider2.Value = 3.3d;
                slider3.Value = 3.4d;
            }
            if (count == 4)
            {
                labelPercentage1.Text = "25%";
                labelPercentage2.Text = "25%";
                labelPercentage3.Text = "25%";
                labelPercentage4.Text = "25%";
                slider1.Value = 2.5d;
                slider2.Value = 2.5d;
                slider3.Value = 2.5d;
                slider4.Value = 2.5d;
            }
            MainWindow.countIt("EE Aftermath");


        }


        public void recalcDicFound()
        {




            labelPercentage1.Visibility = Visibility.Collapsed;
            labelPercentage2.Visibility = Visibility.Collapsed;
            labelPercentage3.Visibility = Visibility.Collapsed;
            labelPercentage4.Visibility = Visibility.Collapsed;
            labelPercentage5.Visibility = Visibility.Collapsed;

            labelPortfolio1.Visibility = Visibility.Collapsed;
            labelPortfolio2.Visibility = Visibility.Collapsed;
            labelPortfolio3.Visibility = Visibility.Collapsed;
            labelPortfolio4.Visibility = Visibility.Collapsed;
            labelPortfolio5.Visibility = Visibility.Collapsed;

            slider1.Visibility = Visibility.Collapsed;
            slider2.Visibility = Visibility.Collapsed;
            slider3.Visibility = Visibility.Collapsed;
            slider4.Visibility = Visibility.Collapsed;
            slider5.Visibility = Visibility.Collapsed;

            labelDollar1.Visibility = Visibility.Collapsed;
            labelDollar2.Visibility = Visibility.Collapsed;
            labelDollar3.Visibility = Visibility.Collapsed;
            labelDollar4.Visibility = Visibility.Collapsed;
            labelDollar5.Visibility = Visibility.Collapsed;

            // TEMP
            //List<string> lsFound = new List<string>();
            MainWindow.countIt("EE Point A");
            dicFound = new Dictionary<string, int>();
            int count = 0;
            // MAY 23 2018 foreach (DataRow dr in dtExp.Rows)
            foreach (DataRowView drv in dgExposures.SelectedItems)
            {
                string portfolioName = drv["Portfolio"].ToString();
                if (dicFound.ContainsKey(portfolioName) == true)
                {
                    dicFound[portfolioName] += 1;
                }

                else
                {
                    dicFound.Add(portfolioName, 1);
                    count++;
                    if (count == 1)
                    {
                        labelPercentage1.Visibility = Visibility.Visible;
                        labelPortfolio1.Visibility = Visibility.Visible;
                        slider1.Visibility = Visibility.Visible;
                        labelDollar1.Visibility = Visibility.Visible;
                        labelPortfolio1.Content = portfolioName;
                    }
                    if (count == 2)
                    {
                        labelPercentage2.Visibility = Visibility.Visible;
                        labelPortfolio2.Visibility = Visibility.Visible;
                        slider2.Visibility = Visibility.Visible;
                        labelDollar2.Visibility = Visibility.Visible;
                        labelPortfolio2.Content = portfolioName;
                    }
                    if (count == 3)
                    {
                        labelPercentage3.Visibility = Visibility.Visible;
                        labelPortfolio3.Visibility = Visibility.Visible;
                        slider3.Visibility = Visibility.Visible;
                        labelDollar3.Visibility = Visibility.Visible;
                        labelPortfolio3.Content = portfolioName;
                    }
                    if (count == 4)
                    {
                        labelPercentage4.Visibility = Visibility.Visible;
                        labelPortfolio4.Visibility = Visibility.Visible;
                        slider4.Visibility = Visibility.Visible;
                        labelDollar4.Visibility = Visibility.Visible;
                        labelPortfolio4.Content = portfolioName;
                    }
                    if (count == 5)
                    {
                        labelPercentage5.Visibility = Visibility.Visible;
                        labelPortfolio5.Visibility = Visibility.Visible;
                        slider5.Visibility = Visibility.Visible;
                        labelDollar5.Visibility = Visibility.Visible;
                        labelPortfolio5.Content = portfolioName;
                    }
                }
            }
            MainWindow.countIt("EE ForLoop");


            slider2.Value = 0d;
            slider3.Value = 0d;
            slider4.Value = 0d;
            slider5.Value = 0d;

            if (count == 1)
            {
                labelPercentage1.Text = "100%";
                slider1.Value = 10d;
            }

            if (count == 2)
            {
                labelPercentage1.Text = "50%";
                labelPercentage2.Text = "50%";
                slider1.Value = 5d;
                slider2.Value = 5d;
            }
            if (count == 3)
            {
                labelPercentage1.Text = "33%";
                labelPercentage2.Text = "33%";
                labelPercentage3.Text = "34%";
                slider1.Value = 3.3d;
                slider2.Value = 3.3d;
                slider3.Value = 3.4d;
            }
            if (count == 4)
            {
                labelPercentage1.Text = "25%";
                labelPercentage2.Text = "25%";
                labelPercentage3.Text = "25%";
                labelPercentage4.Text = "25%";
                slider1.Value = 2.5d;
                slider2.Value = 2.5d;
                slider3.Value = 2.5d;
                slider4.Value = 2.5d;
            }




        }


        public void fillExposuresOLD(string workmatter)
        {
            DataTable dtExposures = ourParent.ourData.getExposuresForWorkMatter(workmatter);




            // OLD CODE

            string sql = "";
            sql += "SELECT ";

            sql += "CASE Portfolio ";

            sql += "WHEN 10005 THEN 'CFI' ";
            sql += "WHEN 10013 THEN 'MMK' ";
            sql += "WHEN 10015 THEN 'Markel' ";
            sql += "WHEN 10007 THEN 'GFIC' ";
            sql += "WHEN 10012 THEN 'Valiant' ";
            sql += "WHEN 10001 THEN '2112 - ISG' ";
            sql += "WHEN 10002 THEN '2112 - WFT' ";
            sql += "WHEN 10011 THEN 'TIG' ";
            sql += "WHEN 10014 THEN 'ASI' ";
            sql += "WHEN 10006 THEN 'Firmont\\Ranger' ";
            sql += "WHEN 10010 THEN 'RiverStone UK' ";
            sql += "WHEN 10008 THEN 'IIC' ";
            sql += "ELSE 'Unknown' ";
            sql += "END AS PortfolioName, ";

            sql += "CCClaimNumber as Exposure, ";
            sql += "childpol.PolicyNumber, childpol.EffectiveDate, trg_AttachmentPointAmt as AttachPoint, ";
            sql += "CASE SUBSTRING(ct.Name,CHARINDEX('-',ct.Name)+2,LEN(ct.Name)) ";
            sql += "WHEN 'BI' THEN 'Bodily Injury' ";
            sql += "WHEN 'AI'  THEN 'Advertising Injury' ";
            sql += "WHEN 'FLL' THEN 'Fire Legal Liability' ";
            sql += "WHEN 'MP'  THEN 'Medical Payments' ";
            sql += "WHEN 'PD'  THEN 'Property Damage' ";
            sql += "WHEN 'PI'  THEN 'Personal Injury' ";
            sql += "END AS Type, ";
            sql += "ct.Name as Coverage, ";
            sql += "cusr.FirstName + ' ' + cusr.LastName as Adjuster, ";
            sql += "expo.id, expo.IncidentID, expo.trg_Claim, expo.CoverageID ";
            sql += "FROM[ClaimCenter].dbo.cc_claim a ";
            sql += "left join[ClaimCenter].dbo.cc_user usr on usr.CredentialID = AssignedUserID ";
            sql += "left join[ClaimCenter].dbo.cc_contact cusr on usr.ContactID = cusr.ID ";
            sql += "left join[ClaimCenter].dbo.cc_incident inc on inc.ClaimID = a.ID ";
            sql += "left join[ClaimCenter].dbo.cc_exposure expo on expo.IncidentID = inc.ID ";
            sql += "left join[ClaimCenter].dbo.ccx_trg_claim policy on policy.ID = expo.trg_Claim ";
            sql += "left join[ClaimCenter].dbo.ccx_trg_childpolicy childpol on childpol.ID = policy.trg_ChildPolicy ";
            sql += "left join[ClaimCenter].dbo.cc_policy pol2 on pol2.ID = childpol.ShellPolicyID ";
            sql += "left join[ClaimCenter].dbo.cc_coverage cov on cov.ID = expo.CoverageID ";
            sql += "left join[ClaimCenter].dbo.cc_riskunit risk on risk.ID = cov.RiskUnitID ";
            sql += "left join[ClaimCenter].dbo.cctl_coveragetype ct on ct.ID = cov.Type ";
            sql += "where a.ClaimNumber = '" + workmatter + "' and expo.CloseDate is null ";
            sql += "order by ";
            sql += "policy.CCClaimNumber,  ";
            sql += "expo.trg_ClaimOrder ";

            DataTable dtExp = ourParent.getData("Data Source=devsql01;Initial Catalog=ClaimCenter;Integrated Security=True", sql);
            dgExposures.ItemsSource = dtExp.DefaultView;

            labelPercentage1.Visibility = Visibility.Collapsed;
            labelPercentage2.Visibility = Visibility.Collapsed;
            labelPercentage3.Visibility = Visibility.Collapsed;
            labelPercentage4.Visibility = Visibility.Collapsed;
            labelPercentage5.Visibility = Visibility.Collapsed;

            labelPortfolio1.Visibility = Visibility.Collapsed;
            labelPortfolio2.Visibility = Visibility.Collapsed;
            labelPortfolio3.Visibility = Visibility.Collapsed;
            labelPortfolio4.Visibility = Visibility.Collapsed;
            labelPortfolio5.Visibility = Visibility.Collapsed;

            slider1.Visibility = Visibility.Collapsed;
            slider2.Visibility = Visibility.Collapsed;
            slider3.Visibility = Visibility.Collapsed;
            slider4.Visibility = Visibility.Collapsed;
            slider5.Visibility = Visibility.Collapsed;

            labelDollar1.Visibility = Visibility.Collapsed;
            labelDollar2.Visibility = Visibility.Collapsed;
            labelDollar3.Visibility = Visibility.Collapsed;
            labelDollar4.Visibility = Visibility.Collapsed;
            labelDollar5.Visibility = Visibility.Collapsed;

            // TEMP
            //List<string> lsFound = new List<string>();
            dicFound = new Dictionary<string, int>();
            int count = 0;
            foreach (DataRow dr in dtExp.Rows)
            {
                string portfolioName = dr["PortfolioName"].ToString();
                if (dicFound.ContainsKey(portfolioName) == true)
                {
                    dicFound[portfolioName] += 1;
                }

                else
                {
                    dicFound.Add(portfolioName, 1);
                    count++;
                    if (count == 1)
                    {
                        labelPercentage1.Visibility = Visibility.Visible;
                        labelPortfolio1.Visibility = Visibility.Visible;
                        slider1.Visibility = Visibility.Visible;
                        labelDollar1.Visibility = Visibility.Visible;
                        labelPortfolio1.Content = portfolioName;
                    }
                    if (count == 2)
                    {
                        labelPercentage2.Visibility = Visibility.Visible;
                        labelPortfolio2.Visibility = Visibility.Visible;
                        slider2.Visibility = Visibility.Visible;
                        labelDollar2.Visibility = Visibility.Visible;
                        labelPortfolio2.Content = portfolioName;
                    }
                    if (count == 3)
                    {
                        labelPercentage3.Visibility = Visibility.Visible;
                        labelPortfolio3.Visibility = Visibility.Visible;
                        slider3.Visibility = Visibility.Visible;
                        labelDollar3.Visibility = Visibility.Visible;
                        labelPortfolio3.Content = portfolioName;
                    }
                    if (count == 4)
                    {
                        labelPercentage4.Visibility = Visibility.Visible;
                        labelPortfolio4.Visibility = Visibility.Visible;
                        slider4.Visibility = Visibility.Visible;
                        labelDollar4.Visibility = Visibility.Visible;
                        labelPortfolio4.Content = portfolioName;
                    }
                    if (count == 5)
                    {
                        labelPercentage5.Visibility = Visibility.Visible;
                        labelPortfolio5.Visibility = Visibility.Visible;
                        slider5.Visibility = Visibility.Visible;
                        labelDollar5.Visibility = Visibility.Visible;
                        labelPortfolio5.Content = portfolioName;
                    }
                }
            }


            slider2.Value = 0d;
            slider3.Value = 0d;
            slider4.Value = 0d;
            slider5.Value = 0d;

            if (count == 1)
            {
                labelPercentage1.Text = "100%";
                slider1.Value = 10d;
            }

            if (count == 2)
            {
                labelPercentage1.Text = "50%";
                labelPercentage2.Text = "50%";
                slider1.Value = 5d;
                slider2.Value = 5d;
            }
            if (count == 3)
            {
                labelPercentage1.Text = "33%";
                labelPercentage2.Text = "33%";
                labelPercentage3.Text = "34%";
                slider1.Value = 3.3d;
                slider2.Value = 3.3d;
                slider3.Value = 3.4d;
            }
            if (count == 4)
            {
                labelPercentage1.Text = "25%";
                labelPercentage2.Text = "25%";
                labelPercentage3.Text = "25%";
                labelPercentage4.Text = "25%";
                slider1.Value = 2.5d;
                slider2.Value = 2.5d;
                slider3.Value = 2.5d;
                slider4.Value = 2.5d;
            }


        }


        private void dgExposures_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgExposures.SelectedIndex < 0)
                return;

            string exposure = ((DataRowView)dgExposures.SelectedItem)["Exposure"].ToString().Trim();
            ourParent.loadNumbersForExposure(exposure);



            string withinLimits = ((DataRowView)dgExposures.SelectedItem)["WithinLimits"].ToString().Trim();
            string inLimitsRO = (withinLimits == "Y") ? "False" : "True";
            string outLimitsRO = (withinLimits == "N") ? "False" : "True";


            ourParent.buildNumberGrid(inLimitsRO, outLimitsRO);

            ourParent.populateNumberGrid();

            DataTable dtEHistory = ourParent.ourData.getExposureHistory(exposure);
            ourParent.history.dgExposureHistory.ItemsSource = dtEHistory.DefaultView;

            //calc();
            // lblWMentry.Text = "WM ";
        }

        public void clear()
        {
            dgExposures.ItemsSource = null;
        }

        private void btnCovDJ_Click(object sender, RoutedEventArgs e)
        {
            ourParent.showRadioButtonStatus(btnCovDJ, new Button[] { btnCovDJ, btnDefExp, btnLoss });
        }

        private void btnDefExp_Click(object sender, RoutedEventArgs e)
        {
            ourParent.showRadioButtonStatus(btnDefExp, new Button[] { btnCovDJ, btnDefExp, btnLoss });
        }

        private void btnLoss_Click(object sender, RoutedEventArgs e)
        {
            showLoss();
        }

        void showLoss()
        {
            ourParent.showRadioButtonStatus(btnLoss, new Button[] { btnCovDJ, btnDefExp, btnLoss });
        }

        private void slider1_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            relabelSliders();
        }

        private void slider2_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            relabelSliders();
        }

        private void slider3_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            relabelSliders();
        }

        private void slider4_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            relabelSliders();
        }

        private void slider5_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            relabelSliders();
        }

        private void relabelSliders()
        {
            // Fast fail on all sliders set to 0
            double totalValue = (slider1.Value + slider2.Value + slider3.Value + slider4.Value + slider5.Value);
            if (totalValue == 0d)
            {
                labelPercentage1.Text = "0%";
                labelPercentage2.Text = "0%";
                labelPercentage3.Text = "0%";
                labelPercentage4.Text = "0%";
                labelPercentage5.Text = "0%";
                return;
            }

            double factor = 10 / (slider1.Value + slider2.Value + slider3.Value + slider4.Value + slider5.Value);

            labelPercentage1.Text = ((slider1.Value * factor) * 10d).ToString("0") + "%";
            labelPercentage2.Text = ((slider2.Value * factor) * 10d).ToString("0") + "%";
            labelPercentage3.Text = ((slider3.Value * factor) * 10d).ToString("0") + "%";
            labelPercentage4.Text = ((slider4.Value * factor) * 10d).ToString("0") + "%";
            labelPercentage5.Text = ((slider5.Value * factor) * 10d).ToString("0") + "%";


            double dol1val = ((slider1.Value * factor) / 10d) * dolval;
            labelDollar1.Text = "$" +numToString(dol1val);

            double dol2val = ((slider2.Value * factor) / 10d) * dolval;
            labelDollar2.Text = "$" + numToString(dol2val);

            double dol3val = ((slider3.Value * factor) / 10d) * dolval;
            labelDollar3.Text = "$" + numToString(dol3val);

        }

        private string numToString(double dol)
        {
            if (dol < 1000)
                return dol.ToString("n1");
            if (dol < 1000000)
            {
                dol = (dol / 1000);
                return ((dol).ToString("n1") + "K");
            }

            dol = (dol / 1000000);
            return ((dol).ToString("n1") + "M");

        }

        private void dgExposures_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (inAllocationMode)
            {
                handleExposureSelectionChangesInAllocation();
                return;
            }

            handleExposureSelectionChanges();
        }

        private void handleExposureSelectionChangesInAllocation()
        {
            bool bad = false;
            foreach (DataRowView drv in dgExposures.SelectedItems)
                if (drv["WithinLimitsDisplay"].ToString() == "")
                    bad = true;

            System.Windows.Visibility vis = (!bad) ? Visibility.Visible : Visibility.Collapsed;
            //lblSelectExposure1.Visibility = lblSelectExposure2.Visibility = lblSelectExposure3.Visibility = lblSelectExposure4.Visibility = lblSelectExposure5.Visibility = lblSelectExposure6.Visibility = vis;
            // lblSelectExposure1.Visibility = lblSelectExposure2.Visibility = lblSelectExposure3.Visibility = lblSelectExposure4.Visibility = vis;
            //dgtcDefExp.IsReadOnly = (bad) ? true : false;
            dgtcDefExp.Visibility = vis;
            lblSelectExposure1.Visibility = (bad) ? Visibility.Visible : Visibility.Collapsed;
            dgNumbers.Width = (bad) ? 362 : 462;

            if (bad)
            {
                clearDefExpenses();
                calc();
            }

            ourParent.calc();
            recalcDicFound();
        }

        private void handleExposureSelectionChanges()
        {
            ourParent.lblError.Text = "";
            lblNotInOrOutWarning.Visibility = Visibility.Collapsed;

            if (dgExposures.SelectedIndex < 0)
                return;

            ////// Auto-save if previous exposure not saved //////
            if ((ourParent.isDirty()))
            {
                if (ourParent.currentExposure == "")
                {
                    //  MessageBox.Show("Error saving exposure - Current exposure is set to blank");
                }
                else
                    ourParent.saveNumberChanges();
            }

            ////// SCOTT - JAN 19 2018 - HANDLE MULTIPLE SELECTION //////
            if (dgExposures.SelectedItems.Count > 1)
            {

                ourParent.buildNumberGrid("True", "True");

                string exp = "";
                foreach (DataRowView drv in dgExposures.SelectedItems)
                {
                    exp = "'" + drv["ExpID"].ToString().Trim() + "'";

                    MainWindow.ourMainWindow.dtCurrentExposure = MainWindow.ourMainWindow.ourData.loadCashFlowForExposure(exp);
                    MainWindow.ourMainWindow.currentExposure = "";

                    ourParent.populateNumberGrid();

                    // JUNE 6 2018 - Always close unknown def exp on multiple exposures selected - for now!

                    // JUNE 11 2018
                    // ourParent.popOpenDefExpUnknownCoumn(unknownOpen: false);
                }
                exp = exp.TrimEnd(new char[] { ',' });


                //MainWindow.ourMainWindow.dtCurrentExposure = MainWindow.ourMainWindow.ourData.loadCashFlowForExposure(exp);
                MainWindow.ourMainWindow.currentExposure = "";
                MainWindow.ourMainWindow.lblExpentry.Text = "Total of selected exposures";


                // ourParent.loadNumbersForExposure(exp);

                // WTF, why was this set to true?????? Changing to false - Scott May 10, 2018
                // ourParent.enableCashFlowEntry(true);
                ourParent.enableCashFlowEntry(false, false);
                ourParent.calc();
                return;
            }

            string exposure = ((DataRowView)dgExposures.SelectedItem)["ExpID"].ToString().Trim();
            string withinLimits = ((DataRowView)dgExposures.SelectedItem)["WithinLimits"].ToString().Trim();
            ourParent.loadNumbersForExposure(exposure);

            string inLimitsRO = (withinLimits == "Y") ? "False" : "True";
            string outLimitsRO = (withinLimits == "N") ? "False" : "True";

            if (withinLimits.Trim() == "")
                lblNotInOrOutWarning.Visibility = Visibility.Visible;

            ourParent.buildNumberGrid(inLimitsRO, outLimitsRO);


            ourParent.populateNumberGrid();
            ourParent.enableCashFlowEntry(true);

            DataTable dtEHistory = ourParent.ourData.getExposureHistory(exposure);
            ourParent.history.dgExposureHistory.ItemsSource = dtEHistory.DefaultView;
            ourParent.lblExpentry.Text = exposure;

            ourParent.calc();

            // June 6 2018
            // Determine if we need to show or hide the unknown def exp column

            // JUNE 11 2018
            // ourParent.popOpenDefExpUnknownCoumn(ourParent.currentExposureHasUnknownDefExp());
        }

        private void dgNumbers_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (dgNumbers.SelectedCells.Count == 0)
                return;

            calc();

            //selectedPeriod = ((DataRowView)dgNumbers.SelectedItem)["Period"].ToString().Trim();

            var drv = ((System.Data.DataRowView)(dgNumbers.SelectedCells[0]).Item).DataView;
            //selectedPeriod = ((System.Data.DataRowView)(dgNumbers.SelectedCells[0]).Item).DataView["Period"].ToString().Trim();
            selectedPeriod = ((System.Data.DataRowView)(dgNumbers.SelectedCells[0]).Item).Row[0].ToString();
            selectedValue = (dgNumbers.SelectedCells[0]).Column.Header.ToString();

            string dollars = "";
            if (selectedValue.StartsWith("Lo"))
                dollars = ((System.Data.DataRowView)(dgNumbers.SelectedCells[0].Item)).Row.ItemArray[1].ToString();
            else if (selectedValue.StartsWith("De"))
                dollars = ((System.Data.DataRowView)(dgNumbers.SelectedCells[0].Item)).Row.ItemArray[2].ToString();
            else if (selectedValue.StartsWith("Co"))
                dollars = ((System.Data.DataRowView)(dgNumbers.SelectedCells[0].Item)).Row.ItemArray[3].ToString();


            //foreach (System.Data.DataRowView drvg in dgNumbers.ItemsSource)
            //{
            //    if (drvg["LossRO"].ToString() == "False")
            //    {
            //        if (drvg[""])
            //        ProcessValue(portfolio, exp, dtCashFlowEntry, "Loss", drvg);
            //        ProcessValue(portfolio, exp, dtCashFlowEntry, "DefExp", drvg);
            //        ProcessValue(portfolio, exp, dtCashFlowEntry, "CovDJ", drvg);
            //    }
            //}

            dolval = 0d;
            double.TryParse(dollars.Replace("$","").Replace(",",""), out dolval);

            lblSlidersAreShowing.Content = "Showing dollar amounts for " + selectedPeriod + " " + selectedValue + " $" + dolval.ToString("n2");

            relabelSliders();
        }

        private void clearDefExpenses()
        {
            foreach (DataRow dr in dtEmptyNumbersWMforSplit.Rows)
                dr["DefExpIn"] = "";

            dgNumbers.ItemsSource = dtEmptyNumbersWMforSplit.DefaultView;
        }

        private void calc()
        {
            lblError.Text = "";

            foreach (DataRow dr in dtEmptyNumbersWMforSplit.Rows)
            {
                string loss = dr["Loss"].ToString();
                string defexp = dr["DefExpIn"].ToString();
                string covdj = dr["CovDJ"].ToString();

                string lossb = loss.Replace(",", "");
                lossb = lossb.Replace("$", "");
                lossb = lossb.ToUpper();
                lossb = lossb.Replace("K", "000");
                lossb = lossb.Replace("M", "000000");
                int iloss = 0;
                if (loss != "")
                    if (Int32.TryParse(lossb, out iloss))
                    {
                        dr["Loss"] = iloss.ToString("C0");
                    }
                    else
                    {
                        lblError.Text = lossb + " is not a valid number";
                    }

                string defexpb = defexp.Replace(",", "");
                defexpb = defexpb.Replace("$", "");
                defexpb = defexpb.ToUpper();
                defexpb = defexpb.Replace("K", "000");
                defexpb = defexpb.Replace("M", "000000");
                int idefexp = 0;
                if (defexp != "")
                    if (Int32.TryParse(defexpb, out idefexp))
                        dr["DefExpIn"] = idefexp.ToString("C0");
                    else
                    {
                        lblError.Text = defexpb + " is not a valid number";
                    }

                string covdjb = covdj.Replace(",", "");
                covdjb = covdjb.Replace("$", "");
                covdjb = covdjb.ToUpper();
                covdjb = covdjb.Replace("K", "000");
                covdjb = covdjb.Replace("M", "000000");
                int icovdj = 0;
                if (covdj != "")
                    if (Int32.TryParse(covdjb, out icovdj))
                        dr["CovDJ"] = icovdj.ToString("C0");
                    else
                    {
                        lblError.Text = covdjb + " is not a valid number";
                    }

                int itotal = iloss + idefexp + icovdj;
                if (itotal > 0)
                    dr["Total"] = itotal.ToString("C0");
                else
                    dr["Total"] = "";
            }

            ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit, 7, 1, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 0, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 1, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 2, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 3, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 4, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 5, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 6, 1) );
            ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit, 7, 2, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 0, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 1, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 2, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 3, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 4, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 5, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 6, 2) );
            ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit, 7, 5, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 0, 5) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 1, 5) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 2, 5) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 3, 5) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 4, 5) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 5, 5) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 6, 5) );
            ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit, 7, 6, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 7, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 7, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 7, 5));

            int totalRow = dtEmptyNumbersWMforSplit.Rows.Count - 1;

            //ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit,totalRow, 1, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 0, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 1, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 2, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 3, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 4, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 5, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 6, 1) );
            //ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit,totalRow, 2, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 0, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 1, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 2, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 3, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 4, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 5, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 6, 2) );
            //// ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit,totalRow, 3, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 0, 3) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 1, 3) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 2, 3) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 3, 3) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 4, 3) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 5, 3) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 6, 3) );
            //ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit, totalRow, 4, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 0, 4) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 1, 4) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 2, 4) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 3, 4) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 4, 4) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 5, 4) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, 6, 4));
            //ourParent.PutValueInRowCol(ref dtEmptyNumbersWMforSplit,totalRow, 5, ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, totalRow, 1) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, totalRow, 2) + ourParent.GetValueFromRowCol(dtEmptyNumbersWMforSplit, totalRow, 4));


            dgNumbers.ItemsSource = dtEmptyNumbersWMforSplit.DefaultView;

        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            // June 6 2018
            ourParent.handleGridKeysLikeExcel(MainWindow.cfeGrid.Allocation, ref e);
        }

        private void TextBox_PreviewKeyDown_1(object sender, KeyEventArgs e)
        {
            ourParent.handleGridKeysLikeExcel(MainWindow.cfeGrid.Allocation, ref e);
        }

        private void TextBox_PreviewKeyDown_2(object sender, KeyEventArgs e)
        {
            ourParent.handleGridKeysLikeExcel(MainWindow.cfeGrid.Allocation, ref e);
        }

        private void btnSetWithinLimits_Click(object sender, RoutedEventArgs e)
        {
            setExposureInOrOut("Y");
        }


        private void btnSetOutsideLimits_Click(object sender, RoutedEventArgs e)
        {
            setExposureInOrOut("N");
        }

        private void setExposureInOrOut(string val)
        {
            if (dgExposures.SelectedItems.Count < 1)
            {
                MessageBox.Show("You must select at least one exposure");
                return;
            }

            //if (inAllocationMode)
            //{
            //    MessageBox.Show("Exposures cannot be set to Within or Outside of Policy Limits while in allocation mode. Please cancel allocation then set.");
            //    return;
            //}

            string wm = ((DataRowView)ourParent.dgWM.SelectedItem)["WorkMatter"].ToString();

            if (!isEditable())
            {
                string status = ((DataRowView)ourParent.dgWM.SelectedItem)["Status"].ToString();

                MessageBox.Show("WorkMatter " + wm + " is not editable because of its status (" + status + ")");
                return;
            }

            List<string> exposures = new List<string>();
            string exp = "";
            bool showWarning = false;
            foreach (DataRowView drv in dgExposures.SelectedItems)
            {
                ////// They are only allowed to set in/out if it is already not set //////
                if ((drv["WithinLimitsDisplay"].ToString().Trim() == "") ||
                    (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl)))
                    exposures.Add(drv["ExpID"].ToString().Trim());
                else
                    showWarning = true;
            }

            foreach (string e in exposures)
            {
                MainWindow.ourMainWindow.ourData.setExposureWithinOrOutOfLimits(e, val);

                // June 6 2018
                // Added this code
                string valuname = (val == "N") ? "DefExpOut" : "DefExpIn";
                // First pass seems wrong!
                //string sql = "update [CashFlow].[data].[CashFlowEntry] set ValueName='" + valuname + "' where Exposure='" + e + "' and EndUser is null and ValueName = 'DefExp'";

                // We should move all current CFE values 
                string sql = "update [CashFlow].[data].[CashFlowEntry] set ValueName='" + valuname + "' where Exposure='" + e + "' and EndUser is null and ValueName in ('DefExp', 'DefExpIn', 'DefExpOut')";
                ourParent.executeSQL(ourParent.dsn, sql);
                // END June 6 2018
            }

            // June 7 2018
            ourParent.ourData.rollupExposuresCFIntoWorkMatter(wm);

            MainWindow.ourMainWindow.processWMSelection();

            // May 25 2018
            if (inAllocationMode)
            {
                recalcDicFound();
                handleExposureSelectionChangesInAllocation();
            }

            if (showWarning)
                MessageBox.Show("Only exposures that were not previously set have been changed.");
        }


        private bool isEditable()
        {
            string wm = ((DataRowView) ourParent.dgWM.SelectedItem)["WorkMatter"].ToString();
            string aa = ((DataRowView) ourParent.dgWM.SelectedItem)["AssignedAdjuster"].ToString();
            string status = ((DataRowView) ourParent.dgWM.SelectedItem)["Status"].ToString();

            bool isTeamLead = ourParent.ourData.isUserTeamLead(MainWindow.uiCurrentUser.adid);
            bool isUnitLead = ourParent.ourData.isUserUnitLead(MainWindow.uiCurrentUser.adid);
            return ourParent.ourData.canUserEditWorkmatter(isUnitLead, isTeamLead, MainWindow.uiCurrentUser.name, MainWindow.uiCurrentUser.dept, aa, wm, status);
        }

    }
}
