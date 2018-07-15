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
    /// Interaction logic for graph.xaml
    /// </summary>
    public partial class graph : UserControl
    {

        #region events definitions
        // EVENT - SELECTION
        public class BarEventArgs : EventArgs
        {
            public string barname;
            // public int barindex;
        }
        public event EventHandler<BarEventArgs> BarClicked;
        #endregion


        Rect graphArea = new Rect();
        Rect leftAxisArea = new Rect();
        Rect rightAxisArea;
        Rect bottomAxisArea;
        Rect leftAxisLegend;
        Rect rightAxisLegend;
        Rect graphTitle;
        double barWidth;
        double barSpacer;


        string title = "";
        DataTable dtData = new DataTable();
        string group1 = "";
        string valueBarColumn = "";
        string valueLineColumn = "";
        string barSegmentColumn = "";




        public graph()
        {
            InitializeComponent();
        }

        // TEMPORARY - Will be replaced with a PROPERTY
        public void setTitle(string title)
        {
        }


        private double barValue(DataTable dtData, string group1, string group1val, string valueColumn)
        {
            double total = 0d;

            foreach (DataRow dr in dtData.Rows)
            {
                if (dr[group1].ToString() == group1val)
                    total += ((double)dr[valueColumn]);
            }

            return total;
        }

        // TEMPORARY - Will be replaced with a PROPERTY
        public void setDataDateSeries(string title, DataTable dtData, string group1, string valueBarColumn, string valueLineColumn, string barSegmentColumn)
        {
            this.title = title;
            this.dtData = dtData;
            this.group1 = group1;
            this.valueBarColumn = valueBarColumn;
            this.valueLineColumn = valueLineColumn;
            this.barSegmentColumn = barSegmentColumn;
            redraw();
        }

        private void redraw()
        {
            if (dtData.Rows.Count == 0)
                return;

            if ((ActualHeight == double.NaN) || (ActualHeight == 0d))
                return;

            if ((ActualWidth == double.NaN) || (ActualWidth == 0d))
                return;

            canvasGraph.Children.Clear();

            ////// Get distinct group 1 names //////
            DataView view = new DataView(dtData);
            DataTable dtBars = view.ToTable(true, group1);

            int nBars = dtBars.Rows.Count;

            ////// Determine min and max values for Y1 range ////
            double y1min = 0, y1max = 0;
            double y2min = 0, y2max = 0;

            foreach (DataRow dr in dtBars.Rows)
            {
                double barval = barValue(dtData, group1, dr[group1].ToString(), valueBarColumn);
                if (barval > y1max)
                    y1max = barval;

                if (barval < y1min)
                    y1min = barval;

                //if (((double)dr[valueLineColumn]) > y2max)
                //    y2max = ((double)dr[valueLineColumn]);

                //if (((double)dr[valueLineColumn]) < y2min)
                //    y2min = ((double)dr[valueLineColumn]);
            }

            // determine the left scale
            double scale = getScale(y1min, y1max, 5d) * 5d;
            double scaleLine = getScale(y2min, y2max, 5d) * 5d;

            // determine graph area
            determineGraphLayout(nBars);

            // Add title
            TextBlock tbTitle = new TextBlock();
            tbTitle.Text = title;
            tbTitle.FontFamily = new FontFamily("Arial");
            tbTitle.FontSize = (leftAxisArea.Width) / 5d;
            tbTitle.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            tbTitle.TextAlignment = TextAlignment.Center;
            tbTitle.Width = ActualWidth;
            tbTitle.Height = 30d;
            Canvas.SetLeft(tbTitle, 0d);
            Canvas.SetTop(tbTitle, 2d);
            canvasGraph.Children.Add(tbTitle);

            // 
            for (int i = 0; i < 6; i++)
            {
                Rectangle l = new Rectangle();
                Canvas.SetLeft(l, graphArea.Left);
                Canvas.SetTop(l, graphArea.Bottom - (i * (graphArea.Height / 5d)));
                l.Width = graphArea.Width;
                l.Height = 3d;
                l.StrokeThickness = 1d;
                l.Fill = new SolidColorBrush(Colors.Silver);
                l.Stroke = new SolidColorBrush(Colors.Silver);
                canvasGraph.Children.Add(l);

                TextBlock tb = new TextBlock();
                tb.Text = legendNumber(scale / 5d * i);
                tb.FontFamily = new FontFamily("Arial");
                tb.FontSize = (leftAxisArea.Width) / 5d;
                tb.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                tb.TextAlignment = TextAlignment.Right;
                tb.Width = leftAxisArea.Width - 5d;
                tb.Height = 30d;
                Canvas.SetLeft(tb, 0);
                Canvas.SetTop(tb, graphArea.Bottom - (i * (graphArea.Height / 5d)) - (tb.FontSize / 2d));
                canvasGraph.Children.Add(tb);

                TextBlock tbr = new TextBlock();
                tbr.Text = legendNumber(scaleLine / 5d * i);
                tbr.FontFamily = new FontFamily("Arial");
                tbr.FontSize = (leftAxisArea.Width) / 5d;
                tbr.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                tbr.TextAlignment = TextAlignment.Left;
                tbr.Width = leftAxisArea.Width - 5d;
                tbr.Height = 30d;
                Canvas.SetLeft(tbr, rightAxisArea.Left + 5d);
                Canvas.SetTop(tbr, graphArea.Bottom - (i * (graphArea.Height / 5d)) - (tb.FontSize / 2d));
                canvasGraph.Children.Add(tbr);

            }

            int iCol = 0;
            foreach (DataRow dr in dtBars.Rows)
            {
                Rectangle rBar = new Rectangle();


                // TEMP
                double barval = barValue(dtData, group1, dr[group1].ToString(), valueBarColumn);


                rBar.Height = (barval / scale) * graphArea.Height;
                rBar.Width = barWidth;
                //rBar.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 1, 183, 169));
                rBar.Fill = new SolidColorBrush(Color.FromArgb(0xFF, 223, 204, 172));
                rBar.MouseLeftButtonDown += RBar_MouseLeftButtonDown;
                rBar.Name = "BAR_" + dr[group1].ToString();

                Canvas.SetLeft(rBar, graphArea.Left + (iCol * (barWidth + barSpacer)));
                Canvas.SetTop(rBar, graphArea.Bottom - rBar.Height);
                canvasGraph.Children.Add(rBar);

                TextBlock tb = new TextBlock();
                // FIX tb.Text = ((DateTime)dr[dateColumn]).ToString("MMM");
                tb.Text = dr[group1].ToString();
                tb.FontFamily = new FontFamily("Arial");
                tb.FontSize = (leftAxisArea.Width) / 5d;
                tb.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                tb.TextAlignment = TextAlignment.Center;

                tb.Width = barWidth;
                tb.Height = 30d;
                Canvas.SetLeft(tb, graphArea.Left + (iCol * (barWidth + barSpacer)));
                Canvas.SetTop(tb, graphArea.Bottom + 6);
                canvasGraph.Children.Add(tb);

                iCol++;
            }

            double lastX = 0, lasty = 0;
            iCol = 0;
            //foreach (DataRow dr in dtData.Rows)
            //{
            //    Line lLine = new Line();

            //    lLine.StrokeThickness = 2d;
            //    lLine.StrokeStartLineCap = PenLineCap.Round;
            //    lLine.StrokeEndLineCap = PenLineCap.Round;
            //    lLine.Stroke = new SolidColorBrush(Color.FromArgb(0xFF, 108, 50, 67));

            //    if (iCol == 0)
            //    {
            //        lLine.X1 = graphArea.Left + 1;
            //        lLine.Y1 = graphArea.Bottom - (((double)dr[valueLineColumn]) / scaleLine) * graphArea.Height;
            //    }
            //    else
            //    {
            //        lLine.X1 = lastX;
            //        lLine.Y1 = lasty;
            //    }

            //    lLine.Y2 = graphArea.Bottom - (((double)dr[valueLineColumn]) / scaleLine) * graphArea.Height;
            //    lLine.X2 = graphArea.Left + (iCol * (barWidth + barSpacer)) + barWidth / 2d;
            //    lastX = lLine.X2;
            //    lasty = lLine.Y2;

            //    canvasGraph.Children.Add(lLine);
            //    iCol++;
            //}


        }

        private void RBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string text = "";
            if (sender is Rectangle)
                text = ((Rectangle)sender).Name;

            if (BarClicked != null)
                BarClicked(this, new BarEventArgs() { barname = text });


            // throw new NotImplementedException();
        }

        private string legendNumber(double number)
        {
            if (number > 1000000000d)
                return (number / 1000000000d).ToString("0") + "B";

            if (number > 1000000d)
                return (number / 1000000d).ToString("0") + "M";

            if (number > 1000d)
                return (number / 1000d).ToString("0.0") + "K";

            return (number).ToString("0.0");
        }

        private void determineGraphLayout(double columns)
        {
            double w = ActualWidth;
            double h = ActualHeight;

            //double w = Width;
           // double h = Height;

            // Left graph area = 10%
            leftAxisArea = new Rect(0, h * .1d, w * .1d, h * .8d);
            graphArea = new Rect(w * .1d, h * .1d, w * .8d, h * .8d);
            rightAxisArea = new Rect(w * .9d, h * .1d, w * .1d, h * .8d);
            bottomAxisArea = new Rect(w * .1d, h * .9d, w * .8d, h * .1d);
            graphTitle = new Rect(0, 0, w, h * .05d);

            barWidth = (graphArea.Width / columns) * .8d;
            barSpacer = (graphArea.Width - (columns * barWidth)) / (columns - 1d);

            int b = 5;
            //Rect rightAxisArea;
            //Rect bottomAxisArea;
            //Rect leftAxisLegend;
            //Rect rightAxisLegend;
            //Rect graphTitle;


        }

        private double getScale(double min, double max, double ticks)
        {
            double dRange = max - min;

            double t5 = getTickSize(dRange, ticks);
            return t5;
            //double t6 = getTickSize(dRange, 6);
            //double t7 = getTickSize(dRange, 7);
        }

        private double getTickSize(double range, double ticks)
        {
            int ticksize = (int)(range / ticks);
            double simple = (Convert.ToInt32(ticksize.ToString()[0].ToString()) + 1) * Math.Pow(10d, ticksize.ToString().Length - 1);
            return simple;
        }

        private void canvasGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            redraw();
        }
    }
}
