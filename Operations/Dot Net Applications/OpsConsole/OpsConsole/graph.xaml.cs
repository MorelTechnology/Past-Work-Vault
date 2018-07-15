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

namespace RiverStoneMagicDashboard
{
    /// <summary>
    /// Interaction logic for graph.xaml
    /// </summary>
    public partial class graph : UserControl
    {
        Rect graphArea = new Rect();
        Rect leftAxisArea = new Rect();
        Rect rightAxisArea;
        Rect bottomAxisArea;
        Rect leftAxisLegend;
        Rect rightAxisLegend;
        Rect graphTitle;
        double barWidth;
        double barSpacer;

        string s_title="";
        DataTable s_dtData = new DataTable();
        string s_dateColumn = "";
        string s_valueBarColumn = "";

        public graph()
        {
            InitializeComponent();
        }

        // TEMPORARY - Will be replaced with a PROPERTY
        public void setTitle(string title)
        {
        }

        public void setDataDateSeries(string title, DataTable dtData, string dateColumn, string valueBarColumn, string valueLineColumn)
        {
            s_title = title;
            s_dtData = dtData;
            s_dateColumn = dateColumn;
            s_valueBarColumn = valueBarColumn;

            draw();
        }

        
        // TEMPORARY - Will be replaced with a PROPERTY
        public void draw()
        {
            canvasGraph.Children.Clear();

            if ((ActualHeight == double.NaN) || (ActualWidth == double.NaN) || (ActualWidth < 220d) || (s_dtData.Rows.Count == 0) )
            {
                Opacity = 0d;
                //Visibility = System.Windows.Visibility.Collapsed;
                return;
            }

            Opacity = 1d;
            //Visibility = System.Windows.Visibility.Visible;

            // Determine number of date columns
            int nBars = s_dtData.Rows.Count;

            // Determine min and max values for Y1 range
            double y1min = 0, y1max = 0;
            double y2min = 0, y2max = 0;
            foreach (DataRow dr in s_dtData.Rows)
            {
                if (((double)dr[s_valueBarColumn]) > y1max)
                    y1max = ((double)dr[s_valueBarColumn]);

                if (((double)dr[s_valueBarColumn]) < y1min)
                    y1min = ((double)dr[s_valueBarColumn]);

                //if (((double)dr[valueLineColumn]) > y2max)
                //    y2max = ((double)dr[valueLineColumn]);

                //if (((double)dr[valueLineColumn]) < y2min)
                //    y2min = ((double)dr[valueLineColumn]);
            }

            // determine the left scale
            double scale = getScale(y1min, y1max, 5d) * 5d;
            double scaleLine = getScale(y2min, y2max, 5d) * 5d;

            // determine graph area
            determineGraphLayout(s_dtData.Rows.Count);

            // Add title
            TextBlock tbTitle = new TextBlock();
            tbTitle.Text = s_title;
            tbTitle.FontFamily = new FontFamily("Segoe UI");

            double fontsize = (leftAxisArea.Width) / 5d;
            tbTitle.FontSize = fontsize;
            //tbTitle.Foreground = new SolidColorBrush(Colors.DimGray);
            tbTitle.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
            tbTitle.TextAlignment = TextAlignment.Center;
            tbTitle.Width = ActualWidth;
            tbTitle.Height = 30d;
            Canvas.SetLeft(tbTitle,0d);
            double top = 2d-(fontsize/6d);
            //if (top < 0)
            //    top = 0;
            Canvas.SetTop(tbTitle, top);
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
                //l.Fill = new SolidColorBrush(Colors.DimGray);
                //l.Stroke = new SolidColorBrush(Colors.DarkGray);
                l.Fill = new SolidColorBrush(Colors.LightGray);
                l.Stroke = new SolidColorBrush(Colors.LightGray);
                canvasGraph.Children.Add(l);

                TextBlock tb = new TextBlock();
                tb.Text = legendNumber(scale/5d*i);
                tb.FontFamily = new FontFamily("Arial");


                tb.FontSize = fontsize;
                //tb.Foreground = new SolidColorBrush(Colors.DimGray);
                tb.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                tb.TextAlignment = TextAlignment.Right;
                tb.Width = leftAxisArea.Width-5d;
                //tb.Width = 30d; // TEMP
                //tb.Height = 30d;
                Canvas.SetLeft(tb, 0);
                Canvas.SetTop(tb, graphArea.Bottom - (i * (graphArea.Height / 5d)) - (tb.FontSize/2d));
                canvasGraph.Children.Add(tb);

                //TextBlock tbr = new TextBlock();
                //tbr.Text = legendNumber(scaleLine / 5d * i);
                //tbr.FontFamily = new FontFamily("Arial");
                //tbr.FontSize = fontsize;
                //tbr.Foreground = new SolidColorBrush(Colors.DimGray);
                //tbr.TextAlignment = TextAlignment.Left;
                //tbr.Width = leftAxisArea.Width - 5d;
                //tbr.Height = 30d;
                //Canvas.SetLeft(tbr, rightAxisArea.Left + 5d);
                //Canvas.SetTop(tbr, graphArea.Bottom - (i * (graphArea.Height / 5d)) - (tb.FontSize / 2d));
                //canvasGraph.Children.Add(tbr);

            }

            int iCol = 0;
            foreach (DataRow dr in s_dtData.Rows)
            {
                Rectangle rBar= new Rectangle();


                rBar.Height = (((double)dr[s_valueBarColumn]) / scale) * graphArea.Height;
                rBar.Width = barWidth;
                rBar.Fill = new SolidColorBrush(Color.FromArgb(0xFF,153,217,234));

                Canvas.SetLeft(rBar, graphArea.Left + (iCol * (barWidth + barSpacer)));
                Canvas.SetTop(rBar, graphArea.Bottom-rBar.Height);
                canvasGraph.Children.Add(rBar);

                TextBlock tb = new TextBlock();
                tb.Text = ((DateTime)dr[s_dateColumn]).ToString("M/d");

                //tb.Text = ((DateTime)dr[dateColumn]).ToString("MMM");
                tb.FontFamily = new FontFamily("Arial");
                tb.FontSize = (leftAxisArea.Width) / 5.5d;

                if (s_dtData.Rows.Count >= 18)
                    tb.FontSize = tb.FontSize * 0.7d;

                tb.Foreground = new SolidColorBrush(Colors.WhiteSmoke);
                tb.TextAlignment = TextAlignment.Center;

                tb.Width = barWidth + barSpacer + barSpacer;
                tb.Height = 30d;
               // Canvas.SetLeft(tb, graphArea.Left + (iCol * (barWidth)));
                Canvas.SetLeft(tb, graphArea.Left + (iCol * (barWidth + barSpacer)));
                Canvas.SetLeft(tb, graphArea.Left + (iCol * (barWidth + barSpacer)) - barSpacer);

                Canvas.SetTop(tb, graphArea.Bottom + 3);
                canvasGraph.Children.Add(tb);

                iCol++;
            }

            //double lastX=0, lasty=0;
            //iCol=0;
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
            int ticksize = (int) (range / ticks);
            double simple = (Convert.ToInt32(ticksize.ToString()[0].ToString()) +1) * Math.Pow(10d, ticksize.ToString().Length - 1);
            return simple;
        }

        private void canvasGraph_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            draw();
        }


    }
}
