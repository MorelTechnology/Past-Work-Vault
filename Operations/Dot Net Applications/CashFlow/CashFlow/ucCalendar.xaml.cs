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
    /// Interaction logic for ucCalendar.xaml
    /// </summary>
    public partial class ucCalendar : UserControl
    {
        bool bStartsOnMonday = true;
        int year = 2018;
        int month = 1;
        int quarter = 1;
        string[] monDayNames = { "M", "T", "W", "R", "F", "S", "S" };
        string[] sunDayNames = { "S", "M", "T", "W", "R", "F", "S" };
        qtrInfo[] quarters;
        DataGrid[] calenders;
        TextBlock[] headers;
        DataTable dtValCal = new DataTable();
        bool bUseColor = true;
        bool initialized = false;

        public class qtrInfo
        {
            public TextBlock starts;
            public TextBlock ends;
            public TextBlock name;
        }

        public ucCalendar()
        {
            InitializeComponent();
        }


        public void initialize()
        {
            if (!initialized)
            {
                calenders = new DataGrid[] { dgM1, dgM2, dgM3, dgM4, dgM5, dgM6, dgM7, dgM8, dgM9, dgM10, dgM11, dgM12, dgM13, dgM14, dgM15, dgM16, dgM17, dgM18, dgM19, dgM20, dgM21, dgM22, dgM23, dgM24 };
                headers = new TextBlock[] { tbM1, tbM2, tbM3, tbM4, tbM5, tbM6, tbM7, tbM8, tbM9, tbM10, tbM11, tbM12, tbM13, tbM14, tbM15, tbM16, tbM17, tbM18, tbM19, tbM20, tbM21, tbM22, tbM23, tbM24 };
                quarters = new qtrInfo[] { new qtrInfo() {starts=Q1Starts, ends=Q1Ends, name=Q1Name },
                    new qtrInfo() {starts=Q2Starts, ends=Q2Ends, name=Q2Name },
                    new qtrInfo() {starts=Q3Starts, ends=Q3Ends, name=Q3Name },
                    new qtrInfo() {starts=Q4Starts, ends=Q4Ends, name=Q4Name },
                    new qtrInfo() {starts=Q5Starts, ends=Q5Ends, name=Q5Name },
                    new qtrInfo() {starts=Q6Starts, ends=Q6Ends, name=Q6Name },
                    new qtrInfo() {starts=Q7Starts, ends=Q7Ends, name=Q7Name },
                    new qtrInfo() {starts=Q8Starts, ends=Q8Ends, name=Q8Name } };

                string sql = "SELECT* FROM [CashFlow].[data].[ValuationPeriod] where BeginDate > dateadd(year, -1, getdate())  order by begindate";
                dtValCal = MainWindow.ourMainWindow.getData(MainWindow.ourMainWindow.dsn, sql);

                fillStartAndCloseDates();
                fillGrids();

                initialized = true;
            }
        }

        private void determineStartingYearAndQuarter()
        {
            DataRow dr = infoGivenDate(DateTime.Today);
            year = Convert.ToDateTime(dr["ValuationDate"].ToString()).Year;
            quarter = (Convert.ToDateTime(dr["ValuationDate"].ToString()).Month-1)/3 + 1;

            quarter -= 2;
            if (quarter < 1)
            {
                quarter = quarter + 4;
                year = year - 1;
            }
        }

        private DateTime determineValDateFromDate(DateTime date)
        {
            foreach (DataRow dr in dtValCal.Rows)
                if ((date.Date >= Convert.ToDateTime(dr["BeginDate"].ToString())) &&
                     (date.Date <= Convert.ToDateTime(dr["EndDate"].ToString())))
                    return Convert.ToDateTime(dr["ValuationDate"].ToString());
            return DateTime.Today;
        }


        private DataRow infoGivenDate(DateTime date)
        {
            foreach (DataRow dr in dtValCal.Rows)
                if ((date.Date >= Convert.ToDateTime(dr["BeginDate"].ToString())) &&
                     (date.Date <= Convert.ToDateTime(dr["EndDate"].ToString())))
                    return dr;
            return null;
        }

        private DataRow infoForValDate(DateTime date)
        {
            foreach (DataRow dr in dtValCal.Rows)
                if (date.Date == Convert.ToDateTime(dr["ValuationDate"].ToString()))
                    return dr;
            return null;
        }


        private void fillStartAndCloseDates()
        {
            determineStartingYearAndQuarter();

            int y = year;
            int q = quarter;

            for (int qi=0; qi<8; qi++)
            {
                int startMonth = (q - 1) * 3 + 1;
                int endMonth = (q - 1) * 3 + 3;

                quarters[qi].name.Text = y.ToString() + " Q" + q.ToString();
                DataRow drStart = infoForValDate(new DateTime(y, startMonth, DateTime.DaysInMonth(y, startMonth)));
                DataRow drEnd = infoForValDate(new DateTime(y, endMonth, DateTime.DaysInMonth(y, endMonth)));

                quarters[qi].starts.Text = Convert.ToDateTime(drStart["BeginDate"].ToString()).ToString("MM/dd/yyyy");
                quarters[qi].ends.Text = Convert.ToDateTime(drEnd["EndDate"].ToString()).ToString("MM/dd/yyyy");

                q++;
                if (q == 5)
                {
                    q = 1;
                    y++;
                }

                //string quarter = dr["QuarterName"].ToString();
                //string start = Convert.ToDateTime(dr["FirstDate"].ToString()).ToString("MM/dd/yyyy");
                //string close = Convert.ToDateTime(dr["LastDate"].ToString()).ToString("MM/dd/yyyy");


            }
        }

        private int periodFromDate(DateTime date)
        {
            for (int qi = 0; qi < 8; qi++)
            {
                if ((date >= Convert.ToDateTime(quarters[qi].starts.Text)) && (date <= Convert.ToDateTime(quarters[qi].ends.Text)))
                    return qi;
            }
            return -1;
        }

        public void fillGrids()
        {
            int year2 = year;
            int month2 = month;

            for (int iMonth = 0; iMonth < 24; iMonth++)
            {
                fillGridForMonth(calenders[iMonth], year2, month2);
                setupCalednarColumns(calenders[iMonth]);

                calenders[iMonth].CanUserAddRows = false;
                calenders[iMonth].CanUserDeleteRows = false;
                calenders[iMonth].CanUserReorderColumns = false;
                calenders[iMonth].CanUserResizeColumns = false;
                calenders[iMonth].CanUserResizeRows = false;
                calenders[iMonth].CanUserSortColumns = false;

                headers[iMonth].Text = new DateTime(year2, month2, 1).ToString("MMMMM yyyy");

                month2++;
                if (month2 == 13)
                {
                    month2 = 1;
                    year2++;
                }

            }
        }

        private void setupCalednarColumns(DataGrid dg)
        {
            int col = 1;
            foreach (var o in dg.Columns)
            {
                if (o is DataGridTemplateColumn)
                {
                    if ( ((bStartsOnMonday) && (col == 6 || col == 7)) ||
                            ((!bStartsOnMonday) && (col == 1 || col == 7)))
                    {
                        ((DataGridTemplateColumn) o).HeaderStyle = (Style)FindResource("CenterGridHeaderStyleRed");
                    }
                    else
                        ((DataGridTemplateColumn)o).HeaderStyle = (Style)FindResource("CenterGridHeaderStyle");

                    o.Header = (bStartsOnMonday) ? monDayNames[col - 1] : sunDayNames[col - 1];

                    col++;
                }
            }
            //chDay1.HeaderStyle = (Style)FindResource("CenterGridHeaderStyle");
            //chDay2.HeaderStyle = (Style)FindResource("CenterGridHeaderStyle");
            //chDay3.HeaderStyle = (Style)FindResource("CenterGridHeaderStyle");
            //chDay4.HeaderStyle = (Style)FindResource("CenterGridHeaderStyle");
            //chFriday.HeaderStyle = (Style)FindResource("CenterGridHeaderStyle");
            //chSaturday.HeaderStyle = (Style)FindResource("CenterGridHeaderStyleRed");
            //chSunday.HeaderStyle = (Style)FindResource("CenterGridHeaderStyleRed");
        }

        private DataTable createBackingTableForMonth()
        {
            DataTable dtMonth = new DataTable();

            for (int day=1; day<= 7; day++)
            {
                dtMonth.Columns.Add("Text" + day.ToString());
                dtMonth.Columns.Add("BackColor" + day.ToString());
                dtMonth.Columns.Add("ForeColor" + day.ToString());
                dtMonth.Columns.Add("Weight" + day.ToString());
            }

            return dtMonth;
        }

        private void fillGridForMonth(DataGrid dgGrid, int Year, int Month)
        {
            DataTable dt = createBackingTableForMonth();

            DayOfWeek firstDay = new DateTime(Year, Month, 1).DayOfWeek;
            DataRow dr = dt.NewRow();
            string column = columnFromDate(firstDay, firstDayIsMonday: bStartsOnMonday);

            for (int iday=1; iday <= DateTime.DaysInMonth(Year, Month); iday++)
            {
                dr[columnName("Text", column)] = iday.ToString();


                int period = periodFromDate(new DateTime(Year, Month, iday));

                if (bUseColor)
                {
                    if (period == 0)
                        dr[columnName("BackColor", column)] = "LemonChiffon";
                    if (period == 1)
                        dr[columnName("BackColor", column)] = "PowderBlue";
                    if (period == 2)
                        dr[columnName("BackColor", column)] = "LightGreen";
                    if (period == 3)
                        dr[columnName("BackColor", column)] = "Thistle";
                    if (period == 4)
                        dr[columnName("BackColor", column)] = "#FF7DBBD3";
                    if (period == 5)
                        dr[columnName("BackColor", column)] = "#FFEAC48C";
                    if (period == 6)
                        dr[columnName("BackColor", column)] = "#FFECBAB5";
                    if (period == 7)
                        dr[columnName("BackColor", column)] = "#FFB7CBAF";
                }

                if (new DateTime(Year, Month, iday) == DateTime.Today)
                {
                    dr[columnName("Weight", column)] = "Bold";
                    dr[columnName("ForeColor", column)] = "Red";
                }

                //if (iday%2 == 1)
                //    dr[columnName("BackColor", column)] = "Yellow";
                //if (iday % 3 == 1)
                //    dr[columnName("Weight", column)] = "Bold";

                if (column == "7")
                {
                    dt.Rows.Add(dr);
                    dr = dt.NewRow();
                    column = "1";
                }
                else
                    column = (Convert.ToInt32(column) + 1).ToString();
            }
            dt.Rows.Add(dr);
            dgGrid.ItemsSource = dt.DefaultView;

        }

        private string columnName(string prefix, string downum)
        {
            return prefix + downum;
        }

        private string columnFromDate(DayOfWeek dow, bool firstDayIsMonday = true)
        {
            switch (dow)
            {
                case DayOfWeek.Monday:
                    return (firstDayIsMonday) ? "1" : "2";

                case DayOfWeek.Tuesday:
                    return (firstDayIsMonday) ? "2" : "3";

                case DayOfWeek.Wednesday:
                    return (firstDayIsMonday) ? "3" : "4";

                case DayOfWeek.Thursday:
                    return (firstDayIsMonday) ? "4" : "5";

                case DayOfWeek.Friday:
                    return (firstDayIsMonday) ? "5" : "6";

                case DayOfWeek.Saturday:
                    return (firstDayIsMonday) ? "6" : "7";

                case DayOfWeek.Sunday:
                    return (firstDayIsMonday) ? "7" : "1";
            }

            return "";
        }



        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
        }

        private void btnMonday_Click(object sender, RoutedEventArgs e)
        {
            ovalMonday.Opacity = 1d;
            ovalSunday.Opacity = 0.2d;
            bStartsOnMonday = true;
            //setupCalednarColumns(dgM1);
            //fillGridForMonth(dgM1, year, month);

            fillGrids();
        }

        private void btnSunday_Click(object sender, RoutedEventArgs e)
        {
            ovalMonday.Opacity = 0.2d;
            ovalSunday.Opacity = 1d;
            bStartsOnMonday = false;
            //setupCalednarColumns(dgM1);
            //fillGridForMonth(dgM1, year, month);
            fillGrids();
        }

        private void btnUseColor_Click(object sender, RoutedEventArgs e)
        {
            if (rectUseColor.Opacity < 0.5d)
            {
                rectUseColor.Opacity = 1d;
                bUseColor = true;
            }
            else
            {
                rectUseColor.Opacity = 0.2;
                bUseColor = false;
            }

            fillGrids();
        }
    }
}
