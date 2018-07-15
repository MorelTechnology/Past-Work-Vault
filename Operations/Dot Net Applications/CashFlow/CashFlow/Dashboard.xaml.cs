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
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : UserControl
    {
        #region Animation state
        public class optionAnimation
        {
            public enum oaStatus { open, opening, closed, closing };
            public oaStatus eStatus = oaStatus.closed;
            public double progress = 0;
            public double closedWidth = 26d;
            public double fullWidth = 242d;
        }
        optionAnimation oa = new optionAnimation();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        #endregion


        public Dashboard()
        {
            InitializeComponent();
            // TEMP FIX
            //return;

            rt.Angle = 0d;
            superColumnOne.Width = new GridLength(26d);

            List<string> filters = new List<string>() { "", "Portfolio", "Claims Team Lead", "OGC Team Lead" , "Analyst", "Insured", "WM Description", "Entered larger than", "Actual larger than", "RWC larger than", "Jurisdiction" };
            cbFilterBy.ItemsSource = filters;

            List<string> groupby = new List<string>() { "", "Team Lead", "Analyst", "Portfolio", "Jurisdiction" };
            cbGroupPrimary.ItemsSource = groupby;
            cbGroupSecondary.ItemsSource = groupby;

            List<string> reportType = new List<string>() { "", "Qtr/Qtr Entry by Portfolio", "Top 100 movers", "Ad hoc" };
            cbDashboardType.ItemsSource = reportType;

            List<string> departmentType = new List<string>() { "", "Claims", "OGC" };
            cbDepartment.ItemsSource = departmentType;

            cbDashboardType.SelectedIndex = 0;

            List<string> quarter = new List<string>() { "", "Q2 2018", "Q3 2018", "Q4 2018", "Q1 2019" };
            cbCompareQuarter1.ItemsSource = quarter;
            cbCompareQuarter2.ItemsSource = quarter;
            cbCompareQuarter1.SelectedIndex = 2;
            cbCompareQuarter2.SelectedIndex = 3;


            DataTable dtGraphData = new DataTable();
            dtGraphData.Columns.Add("group1");
            dtGraphData.Columns.Add("bar", typeof(double));
            dtGraphData.Columns.Add("line", typeof(double));
            dtGraphData.Columns.Add("portfolio");
            dtGraphData.Columns.Add("value", typeof(double));

            dtGraphData.Rows.Add("CFI", 10, 20, "CFI", 11823078);
            dtGraphData.Rows.Add("IIC", 10, 20, "IIC", 8066037);
            dtGraphData.Rows.Add("MMK", 10, 20, "MMK", 7980487);
            dtGraphData.Rows.Add("TIG", 10, 20, "TIG", 898452);
            dtGraphData.Rows.Add("FSG", 10, 20, "FSG", 615898);
            dtGraphData.Rows.Add("MKL", 10, 20, "MKL", 121191);
            dtGraphData.Rows.Add("ASI", 10, 20, "ASI", 13032);
            dtGraphData.Rows.Add("VAL", 10, 20, "VAL", 1300);
            dtGraphData.Rows.Add("GFIC", 10, 20, "GFIC", 0);
            dtGraphData.Rows.Add("SDI", 10, 20, "SDI", 0);


            graphMain.setDataDateSeries("Title", dtGraphData, "group1", "value", "line", "portfolio");


            //DataTable dtGraphData = new DataTable();
            //dtGraphData.Columns.Add("group1");
            //dtGraphData.Columns.Add("bar", typeof(double));
            //dtGraphData.Columns.Add("line", typeof(double));
            //dtGraphData.Columns.Add("portfolio");
            //dtGraphData.Columns.Add("value", typeof(double));

            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "ASI", 871516.00);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "CFI", 48743545);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "FSG", 3781126);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "GFIC", 0);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "IIC", 86502157);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "MKL", 8361611);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "MMK", 51654810);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "SDI", 0);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "TIG", 4581528);
            //dtGraphData.Rows.Add("3Q Ent", 10, 20, "VAL", 1000);

            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "ASI", 884548);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "CFI", 60566624);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "FSG", 4397021);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "GFIC", 0);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "IIC", 94568195);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "MKL", 8482802);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "MMK", 59635297);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "SDI", 0);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "TIG", 5479981);
            //dtGraphData.Rows.Add("4Q Ent", 10, 20, "VAL", 2300);

            //dtGraphData.Rows.Add("Diff", 10, 20, "ASI", 13032);
            //dtGraphData.Rows.Add("Diff", 10, 20, "CFI", 11823078);
            //dtGraphData.Rows.Add("Diff", 10, 20, "FSG", 615898);
            //dtGraphData.Rows.Add("Diff", 10, 20, "GFIC", 0);
            //dtGraphData.Rows.Add("Diff", 10, 20, "IIC", 8066037);
            //dtGraphData.Rows.Add("Diff", 10, 20, "MKL", 121191);
            //dtGraphData.Rows.Add("Diff", 10, 20, "MMK", 7980487);
            //dtGraphData.Rows.Add("Diff", 10, 20, "SDI", 0);
            //dtGraphData.Rows.Add("Diff", 10, 20, "TIG", 898452);
            //dtGraphData.Rows.Add("Diff", 10, 20, "VAL", 1300);


            //graphMain.setDataDateSeries("Title", dtGraphData, "group1", "value", "line", "portfolio");


            //DataTable dtGraphData = new DataTable();
            //dtGraphData.Columns.Add("Quarter", typeof(DateTime));
            //dtGraphData.Columns.Add("bar", typeof(double));
            //dtGraphData.Columns.Add("line", typeof(double));

            //dtGraphData.Rows.Add(DateTime.Today.AddMonths(-3), 10, 20);
            //dtGraphData.Rows.Add(DateTime.Today.AddMonths(-2), 20, 30);
            //dtGraphData.Rows.Add(DateTime.Today.AddMonths(-1), 30, 10);
            //dtGraphData.Rows.Add(DateTime.Today.AddMonths(-0), 40, 50);

            //graphMain.setDataDateSeries("Title", dtGraphData, "Quarter", "bar", "line");


            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 25);
            dispatcherTimer.Start();

            // FIX
            borderCompareQuarter1.Visibility = Visibility.Collapsed;
            borderCompareQuarter2.Visibility = Visibility.Collapsed;
            borderDepartment.Visibility = Visibility.Collapsed;
            borderFilterBy.Visibility = Visibility.Collapsed;
            borderGroupPrimary.Visibility = Visibility.Collapsed;
            borderGroupSecondary.Visibility = Visibility.Collapsed;
            borderSumOn.Visibility = Visibility.Collapsed;
            borderSumOn2.Visibility = Visibility.Collapsed;

            graphMain.Visibility = Visibility.Collapsed;
            graphBaby.Visibility = Visibility.Collapsed;


            showFilterText(false);

            adhocData();
        }


        private void qbyqData()
        {
            DataTable dtItemData = new DataTable();
            dtItemData.Columns.Add("Portfolio");
            dtItemData.Columns.Add("Three", typeof(double));
            dtItemData.Columns.Add("Four", typeof(double));
            dtItemData.Columns.Add("Diff", typeof(double));

            dtItemData.Rows.Add("CFI", 48743545.73, 60566624.33, 11823078.60);
            dtItemData.Rows.Add("IIC", 86502157.24, 94568195.05, 8066037.81);
            dtItemData.Rows.Add("MMK", 51654810.56, 59635297.76, 7980487.20);
            dtItemData.Rows.Add("TIG", 4581528.94, 5479981.43, 898452.49);
            dtItemData.Rows.Add("FSG", 3781126.00, 4397021.00, 615895.00);
            dtItemData.Rows.Add("MKL", 8361611.17, 8482802.97, 121191.80);
            dtItemData.Rows.Add("ASI", 871516.00, 884548.00, 13032.00);
            dtItemData.Rows.Add("VAL", 1000.00, 2300.00, 1300.00);
            dtItemData.Rows.Add("GFIC", 0.00, 0.00, 0.00);
            dtItemData.Rows.Add("SDI", 0.00, 0.00, 0.00);

            dgData.ItemsSource = dtItemData.DefaultView;
            dgData.Visibility = Visibility.Visible;
            dgDataTop.Visibility = Visibility.Collapsed;
            dgDataAdHoc.Visibility = Visibility.Collapsed;
        }

        private void top100Data()
        {
            DataTable dtItemData = new DataTable();
            dtItemData.Columns.Add("WM");
            dtItemData.Columns.Add("Desc");
            dtItemData.Columns.Add("Q3", typeof(double));
            dtItemData.Columns.Add("Q4", typeof(double));
            dtItemData.Columns.Add("Diff", typeof(double));
            dtItemData.Columns.Add("Color");

            dtItemData.Rows.Add("WM000024776", "RT VANDERBILT	", 4043976, 10121382, 6077406,"");
            dtItemData.Rows.Add("WM000000395", "BECHTEL GROUP, INC. / ASBESTOS	", 11748960, 15091146, 3342186,"");
            dtItemData.Rows.Add("WM000117187", "ROCKWELL INTERNATIONAL / ASBESTOS / NON-AOA	", 14599000, 17940000, 3341000,"");
            dtItemData.Rows.Add("WM000022301", "INSULATION DISTRIBUTORS / INSULATION DISTRIBUTORS - COVERAGE	", 0, 2525001, 2525001,"");
            dtItemData.Rows.Add("WM000001246", "TISHMAN LIQUIDATING CORP. / ASBESTOS	", 497317.98, 1507603.98, 1010286,"");
            dtItemData.Rows.Add("WM000000242", "UNION PACIFIC CORP. / ASBESTOS	", 3251496, 4251496, 1000000,"");
            dtItemData.Rows.Add("WM000000109", "HARRIS CORP / ASBESTOS	", 132600, 1025141, 892541,"");
            dtItemData.Rows.Add("WM000024758", "UNION CARBIDE/Coverage/MMIC	", 0, 869061, 869061,"");
            dtItemData.Rows.Add("WM000000087", "DURIRON COMPANY, INC. / ASBESTOS	", 809352, 1674352, 865000,"");
            dtItemData.Rows.Add("WM000024742", "INGERSOLL RAND / COVERAGE - MMIC	", 6150000, 7000000.2, 850000.2,"");
            dtItemData.Rows.Add("WM000113604", "GENUINE AUTO PARTS	", 542572, 1207001, 664429,"");
            dtItemData.Rows.Add("WM000000509", "MCMASTER CARR SUPPLY / ASBESTOS	", 1838000, 2501523, 663523,"");
            dtItemData.Rows.Add("WM000000665", "GRAND AUTO / ASBESTOS	", 2400000, 3028972, 628972,"");
            dtItemData.Rows.Add("WM000001509", "PLASTICS ENGINEERING CO. / ASBESTOS	", 5458856, 6002000, 543144,"");
            dtItemData.Rows.Add("WM000000237", "UNION CARBIDE / ASBESTOS	", 0, 440064, 440064,"");
            dtItemData.Rows.Add("WM000000181", "PARKER HANNIFIN / ASBESTOS	", 445260, 870514.98, 425254.98,"");
            dtItemData.Rows.Add("WM000000259", "WHITE CONSOLIDATED IND / ASBESTOS	", 4800000, 5186016, 386016,"");
            dtItemData.Rows.Add("WM000117552", "Pacificorp \\ Coverage	", 50000, 427500, 377500,"");
            dtItemData.Rows.Add("WM000053409", "Various BI	", 226000, 594926, 368926,"");
            dtItemData.Rows.Add("WM000053090", "SCOTT & FETZER	", 0, 339383, 339383,"");
            dtItemData.Rows.Add("WM000052986", "GOODYEAR TIRE & RUBBER / ASBESTOS / Non-AOA	", 2611500, 2930000, 318500,"");
            dtItemData.Rows.Add("WM000016540", "CROWN INDUSTRIES / ASBESTOS	", 726000, 1024100, 298100,"");
            dtItemData.Rows.Add("WM000000477", "INSULATION DISTRIBUTORS / ASBESTOS	", 798800, 1081000, 282200,"");
            dtItemData.Rows.Add("WM000022359", "ROCKWELL INTERNATIONAL / ROCKWELL INTERNATIONAL - COVERAGE	", 624999.69, 899999.73, 275000.04,"");
            dtItemData.Rows.Add("WM000000752", "FRONTIER INSULATION / ASBESTOS	", 445256, 707432, 262176,"");
            dtItemData.Rows.Add("WM000001277", "UNITED INDUSTRIAL SYNDICA / ASBESTOS	", 99000, 351000, 252000,"");
            dtItemData.Rows.Add("WM000000587", "VENTFABRICS INC / ASBESTOS	", 832500, 1082996, 250496,"");
            dtItemData.Rows.Add("WM000000559", "SQUIRES BELT MATERIAL CO. / ASBESTOS	", 255400.08, 481217.1, 225817.02,"");
            dtItemData.Rows.Add("WM000016457", "UNION ELECTRIC COMPANY / ASBESTOS	", 756432, 968184, 211752,"");
            dtItemData.Rows.Add("WM000000667", "BAKERS PRIDE OVEN CO. / ASBESTOS	", 451662.03, 660330.03, 208668,"");
            dtItemData.Rows.Add("WM000000012", "AMERICAN CYANAMID CO / ASBESTOS / CIP	", 79600, 284400, 204800,"");
            dtItemData.Rows.Add("WM000000173", "OCCIDENTAL PETROLEUM / ASBESTOS	", 308988, 511988, 203000,"");
            dtItemData.Rows.Add("WM000121286", "ASHLAND OIL \\ AB	", 0, 200000, 200000,"");
            dtItemData.Rows.Add("WM000000585", "UNITED PLUMBING CO. / ASBESTOS	", 32836.02, 225702, 192865.98,"");
            dtItemData.Rows.Add("WM000000492", "LA RUBBER COMPANY / ASBESTOS	", 346035.84, 536038.86, 190003.02,"");
            dtItemData.Rows.Add("WM000117580", "Various BI	", 0, 185230, 185230,"");
            dtItemData.Rows.Add("WM000000685", "CALIF. SAFETY & SUPPLY / ASBESTOS	", 0, 178666, 178666,"");
            dtItemData.Rows.Add("WM000000487", "KOHLER COMPANY / ASBESTOS	", 425580, 590580, 165000,"");
            dtItemData.Rows.Add("WM000024494", "Various BI	", 7200, 164000, 156800,"");
            dtItemData.Rows.Add("WM000000463", "GRAYBAR ELECTRIC CO. / ASBESTOS	", 0, 144500, 144500,"");
            dtItemData.Rows.Add("WM000000501", "M. SLAYEN ASSOCIATES / ASBESTOS	", 288004, 420000, 131996,"");
            dtItemData.Rows.Add("WM000022571", "DURO DYNE NATIONAL CORP / COVERAGE AB	", 75000, 200000.04, 125000.04,"");
            dtItemData.Rows.Add("WM000117013", "Various BI	", 0, 120000, 120000,"");
            dtItemData.Rows.Add("WM000009020", "MUELLER CO. / ASBESTOS	", 356000, 470000, 114000,"");
            dtItemData.Rows.Add("WM000000088", "E.E. ZIMMERMAN COMPANY / ASBESTOS	", 420000, 528000, 108000,"");
            dtItemData.Rows.Add("WM000053313", "RT Vanderbilt / Coverage	", 282500, 389500, 107000,"");
            dtItemData.Rows.Add("WM000053406", "Various BI	", 350000, 452000, 102000,"");
            dtItemData.Rows.Add("WM000115732", "Various BI	", 91000, 186000, 95000,"");
            dtItemData.Rows.Add("WM000000248", "W R GRACE & CO / ASBESTOS	", 762836, 853224, 90388,"");
            dtItemData.Rows.Add("WM000001847", "SANTA FE BRAUN INC / ASBESTOS	", 150000, 240000, 90000,"");
            dtItemData.Rows.Add("WM000000432", "DURAMETALLIC CORP. / ASBESTOS	", 935000, 1019380, 84380,"");
            dtItemData.Rows.Add("WM000000859", "ASTRA FLOORING CO. / ASBESTOS	", 193041.96, 269288.97, 76247.01,"");
            dtItemData.Rows.Add("WM000023299", "JOHNSON & JOHNSON / ASBESTOS	", 100000, 174556, 74556,"");
            dtItemData.Rows.Add("WM000001567", "AERCO INTERNATIONAL INC / ASBESTOS	", 620776, 692776, 72000,"");
            dtItemData.Rows.Add("WM000001010", "FRICTION MATERIAL / ASBESTOS	", 48240, 120000, 71760,"");
            dtItemData.Rows.Add("WM000116067", "Various BI	", 0, 65000, 65000,"");
            dtItemData.Rows.Add("WM000000420", "COLUMBIA BOILER COMPANY / ASBESTOS	", 96000, 152000, 56000,"");
            dtItemData.Rows.Add("WM000001115", "MARLEY-WYLAIN COMPANY / ASBESTOS	", 2510000, 2565240, 55240,"");
            dtItemData.Rows.Add("WM000022268", "WALTER E. CAMPBELL CO. / COVERAGE	", 0, 50000, 50000,"");
            dtItemData.Rows.Add("WM000000713", "GOLDEN GATE DRYWALL / ASBESTOS	", 77196, 126844, 49648,"");
            dtItemData.Rows.Add("WM000001660", "MURCO WALL PRODUCTS INC / ASBESTOS	", 304126, 351858, 47732,"");
            dtItemData.Rows.Add("WM000000500", "LUSE-STEVENSON / ASBESTOS	", 448000, 495704, 47704,"");
            dtItemData.Rows.Add("WM000016549", "SIX ROBBLEES INC / ASBESTOS	", 0, 40500, 40500,"");
            dtItemData.Rows.Add("WM000001079", "PIONEER INSULATIONS CONTR / ASBESTOS	", 0, 39744, 39744,"");
            dtItemData.Rows.Add("WM000001041", "REW/KC WALL / ASBESTOS	", 114004, 151581, 37577,"");
            dtItemData.Rows.Add("WM000000569", "SWINERTON & WALBERG / ASBESTOS	", 180000, 212499.96, 32499.96,"");
            dtItemData.Rows.Add("WM000119708", "Various BI	", 0, 31000, 31000,"");
            dtItemData.Rows.Add("WM000113553", "C. F. BRAUN & COMPANY / INS. CO. ST. PA.	", 50000, 80000, 30000,"");
            dtItemData.Rows.Add("WM000000994", "ALLIED REFRIGERATION / ASBESTOS	", 0, 30000, 30000,"");
            dtItemData.Rows.Add("WM000022307", "SANTA FE BRAUN INC / SANTA FE BRAUN INC - COVERAGE	", 142999.94, 172999.79, 29999.85,"");
            dtItemData.Rows.Add("WM000024808", "CHAMPION INTL	", 190500, 219811, 29311,"");
            dtItemData.Rows.Add("WM000001857", "IMRIE GIELOW INC / ASBESTOS	", 0, 28000, 28000,"");
            dtItemData.Rows.Add("WM000000631", "PIERCE ENTERPRISES / ASBESTOS	", 345600, 373500, 27900,"");
            dtItemData.Rows.Add("WM000114092", "ANDERSON GREENWOOD	", 80000, 105000, 25000,"");
            dtItemData.Rows.Add("WM000029141", "WESTINGHOUSE ELECTRI	", 40308, 64998, 24690,"");
            dtItemData.Rows.Add("WM000000558", "SPRINKMANN SONS WISCONSIN / ASBESTOS	", 416000, 440000, 24000,"");
            dtItemData.Rows.Add("WM000000829", "A.J. FRIEDMAN SUPPLY CO / ASBESTOS	", 24000, 46000.02, 22000.02,"");
            dtItemData.Rows.Add("WM000001027", "NORTHERN PLUMBING / ASBESTOS	", 10000, 32000, 22000,"");
            dtItemData.Rows.Add("WM000001015", "DELAVAL TURBINE IN / ASBESTOS	", 7803218, 7782305, -20913,"Red");
            dtItemData.Rows.Add("WM000000617", "CURTISS-WRIGHT CORP. / ASBESTOS / AOA	", 23270, 270, -23000,"Red");
            dtItemData.Rows.Add("WM000000406", "CAHILL ENTITIES / ASBESTOS	", 277000, 253000, -24000,"Red");
            dtItemData.Rows.Add("WM000023409", "Carrier Corp/Coverage	", 275000.04, 250000.1, -24999.94,"Red");
            dtItemData.Rows.Add("WM000024746", "Carrier Corp/Coverage MMIC	", 275000, 250000, -25000,"Red");
            dtItemData.Rows.Add("WM000000061", "CHICAGO PNEUMATIC TOOL / ASBESTOS	", 78786, 52534.08, -26251.92,"Red");
            dtItemData.Rows.Add("WM000016604", "ABCO WELDING / ASBESTOS	", 48000, 21000, -27000,"Red");
            dtItemData.Rows.Add("WM000000963", "HAMPDEN AUTOMOTIVE SALES / ASBESTOS	", 427200.32, 397001.28, -30199.04,"Red");
            dtItemData.Rows.Add("WM000000855", "MARTIN BROTHERS / ASBESTOS	", 70000, 36000, -34000,"Red");
            dtItemData.Rows.Add("WM000000959", "HUDSON PLASTERING CORP. / ASBESTOS	", 49584, 3000, -46584,"Red");
            dtItemData.Rows.Add("WM000006557", "ROBINSON INDUSTRIES INC / ASBESTOS	", 116000, 61652, -54348,"Red");
            dtItemData.Rows.Add("WM000002290", "DULUTH PLUMBING SUPPLIES / ASBESTOS	", 62100, 1200, -60900,"Red");
            dtItemData.Rows.Add("WM000001035", "GENERAL GASKET / ASBESTOS	", 220038, 155638, -64400,"Red");
            dtItemData.Rows.Add("WM000000892", "SIMAKAS COMPANY INC. / ASBESTOS	", 121340, 26652, -94688,"Red");
            dtItemData.Rows.Add("WM000024374", "TULANE EDUCATIONAL FUND / COVERAGE	", 950000, 850000, -100000,"Red");
            dtItemData.Rows.Add("WM000000753", "SAN FRANCISCO GRAVEL / ASBESTOS	", 375691, 216260, -159431,"Red");
            dtItemData.Rows.Add("WM000000908", "E F BRADY CO INC / ASBESTOS	", 657000.16, 424948.06, -232052.1,"Red");
            dtItemData.Rows.Add("WM000000750", "KING BEARING CO. / ASBESTOS	", 249999.99, 0, -249999.99,"Red");
            dtItemData.Rows.Add("WM000001896", "EDWARD ORTON JR. CERAMIC / ASBESTOS	", 1080000.12, 823166.04, -256834.08,"Red");
            dtItemData.Rows.Add("WM000000057", "CHAMPION INTERNATIONAL / ASBESTOS	", 4718507, 4419676, -298831,"Red");
            dtItemData.Rows.Add("WM000002982", "THE SCOTT FETZER COMPANY / ASBESTOS	", 362344, 0, -362344,"Red");
            dtItemData.Rows.Add("WM000113664", "Safety First Industries Inc. E	", 3050000, 2078800, -971200,"Red");

            dgDataTop.ItemsSource = dtItemData.DefaultView;


            dgData.Visibility = Visibility.Collapsed;
            dgDataTop.Visibility = Visibility.Visible;
            dgDataAdHoc.Visibility = Visibility.Collapsed;
        }


        private void adhocData()
        {
            dgData.Visibility = Visibility.Collapsed;
            dgDataTop.Visibility = Visibility.Collapsed;
            dgDataAdHoc.Visibility = Visibility.Visible;
        }

        #region Animation
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (oa.eStatus == optionAnimation.oaStatus.opening)
            {
                rt.Angle = rt.Angle - 4d;
                superColumnOne.Width = new GridLength(superColumnOne.Width.Value + 9);

                if (superColumnOne.Width.Value >= oa.fullWidth)
                {
                    oa.eStatus = optionAnimation.oaStatus.open;
                    rt.Angle = -90d;
                    superColumnOne.Width = new GridLength(240d);
                }
            }

            if (oa.eStatus == optionAnimation.oaStatus.closing)
            {
                rt.Angle = rt.Angle + 4d;
                superColumnOne.Width = new GridLength(superColumnOne.Width.Value - 9);

                if (superColumnOne.Width.Value <= oa.closedWidth)
                {
                    oa.eStatus = optionAnimation.oaStatus.closed;
                    rt.Angle = 0d;
                    superColumnOne.Width = new GridLength(26d);
                }
            }

        }
        #endregion Animation


        private void btnClose2_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void graphMain_SizeChanged(object sender, SizeChangedEventArgs e)
        {

        }

        private void imgExpandCollapse_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (superColumnOne.Width.Value > 100)
                oa.eStatus = optionAnimation.oaStatus.closing;
            else
                oa.eStatus = optionAnimation.oaStatus.opening;
        }


        private void showFilterText(bool show)
        {
            filterText.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            filterText2.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            filterText3.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            filterText4.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            lblOr2.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            lblOr3.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;
            lblOr4.Visibility = (show) ? Visibility.Visible : Visibility.Collapsed;

            filterLB.Visibility = (show) ? Visibility.Collapsed : Visibility.Visible;
        }

        private void cbFilterBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            showFilterText(false);

            if (cbFilterBy.SelectedIndex <= 0)
            {
                filterLB.ItemsSource = null;
                return;
            }


            string filterOn = cbFilterBy.SelectedItem.ToString();

            if (filterOn == "Portfolio")
            {
                List<string> portfolios = new List<string>() { "ASI", "CFI", "FSG", "GFIC", "IIC", "MKL", "MMK", "SDI", "TIG", "VAL" };
                filterLB.ItemsSource = portfolios;
            }

            if (filterOn == "Claims Team Lead")
            {
                List<string> portfolios = new List<string>() { "Erin Voyik", "John Hatch", "Kate Vaughn", "Kevin Chenelle", "Paul Ziska", "Steward Richmond"};
                filterLB.ItemsSource = portfolios;
            }

            if (filterOn == "OGC Team Lead")
            {
                List<string> portfolios = new List<string>() { "Brendan Clifford", "Craig Brown" };
                filterLB.ItemsSource = portfolios;
            }

            if (filterOn == "WM Description")
            {
                showFilterText(true);
                filterText.Text = "";
                filterText2.Text = "";
                filterText3.Text = "";
                filterText4.Text = "";
                filterText.CaptureMouse();
            }

        }

        private void graphMain_BarClicked(object sender, graph.BarEventArgs e)
        {
            string bar = e.barname;

            if (bar == "BAR_CFI")
            {
                DataTable dtGraphBaby = new DataTable();
                dtGraphBaby.Columns.Add("group1");
                dtGraphBaby.Columns.Add("bar", typeof(double));
                dtGraphBaby.Columns.Add("line", typeof(double));
                dtGraphBaby.Columns.Add("portfolio");
                dtGraphBaby.Columns.Add("value", typeof(double));

                dtGraphBaby.Rows.Add("3Q", 10, 20, "CFI", 48743545);
                dtGraphBaby.Rows.Add("4Q", 10, 20, "CFI", 60566624);
                dtGraphBaby.Rows.Add("DIFF", 10, 20, "CFI", 11823078);

                graphBaby.setDataDateSeries("CFI", dtGraphBaby, "group1", "value", "line", "portfolio");

            }
            if (bar == "BAR_IIC")
            {
                DataTable dtGraphBaby = new DataTable();
                dtGraphBaby.Columns.Add("group1");
                dtGraphBaby.Columns.Add("bar", typeof(double));
                dtGraphBaby.Columns.Add("line", typeof(double));
                dtGraphBaby.Columns.Add("portfolio");
                dtGraphBaby.Columns.Add("value", typeof(double));

                dtGraphBaby.Rows.Add("3Q", 10, 20, "IIC", 86502157);
                dtGraphBaby.Rows.Add("4Q", 10, 20, "IIC", 94568195);
                dtGraphBaby.Rows.Add("DIFF", 10, 20, "IIC", 8066037);

                graphBaby.setDataDateSeries("IIC", dtGraphBaby, "group1", "value", "line", "portfolio");

            }
        }

        private void cbDashboardType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            borderCompareQuarter1.Visibility = Visibility.Collapsed;
            borderCompareQuarter2.Visibility = Visibility.Collapsed;
            borderDepartment.Visibility = Visibility.Collapsed;
            borderFilterBy.Visibility = Visibility.Collapsed;
            borderGroupPrimary.Visibility = Visibility.Collapsed;
            borderGroupSecondary.Visibility = Visibility.Collapsed;
            borderSumOn.Visibility = Visibility.Collapsed;
            borderSumOn2.Visibility = Visibility.Collapsed;

            graphMain.Visibility = Visibility.Collapsed;
            graphBaby.Visibility = Visibility.Collapsed;


            string graph = cbDashboardType.SelectedItem.ToString();

            //             List<string> reportType = new List<string>() { "", "Qtr/Qtr Entry by Portfolio", "Top 100 movers" };

            if (graph == "Qtr/Qtr Entry by Portfolio")
            {
                borderCompareQuarter1.Visibility = Visibility.Visible;
                borderCompareQuarter2.Visibility = Visibility.Visible;
                borderDepartment.Visibility = Visibility.Collapsed;
                borderFilterBy.Visibility = Visibility.Collapsed;
                graphMain.Visibility = Visibility.Visible;
                graphBaby.Visibility = Visibility.Visible;
                qbyqData();
            }

            if (graph == "Top 100 movers")
            {
                borderDepartment.Visibility = Visibility.Visible;
                borderFilterBy.Visibility = Visibility.Visible;
                top100Data();
            }

            if (graph == "Ad hoc")
            {
                borderDepartment.Visibility = Visibility.Visible;
                borderFilterBy.Visibility = Visibility.Visible;
                borderGroupPrimary.Visibility = Visibility.Visible;
                borderGroupSecondary.Visibility = Visibility.Visible;
                borderSumOn.Visibility = Visibility.Visible;
                borderSumOn2.Visibility = Visibility.Visible;
                adhocData();
            }

        }
    }
}
