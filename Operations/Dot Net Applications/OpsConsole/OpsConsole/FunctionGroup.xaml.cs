using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for FunctionGroup.xaml
    /// </summary>
    public partial class FunctionGroup : UserControl
    {
        // constants
        const double buttonWidth = 278d;
        const double buttonHeight = 34d;
        const double buttonExtraLineHeight = 10d;
        const double buttonFontSize = 14d;
        const double innerStackWidth = 240d;
        const double buttonMargin = 10d;
        MainWindow ourMainWindow = null;

        public FunctionGroup()
        {
            InitializeComponent();
        }

        public void setMainWindow(MainWindow our)
        {
            ourMainWindow = our;
        }

        public void addButton(string text1, string text2, string function)
        {
            double ourButtonHeight = buttonHeight;

            Button commandButton = new Button();
            commandButton.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            commandButton.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            commandButton.Width = buttonWidth;
            commandButton.Height = buttonHeight + ((text2 != "") ? buttonExtraLineHeight : 0d);
            commandButton.FontSize = buttonFontSize;
            commandButton.Margin = new Thickness(0d,0d,0d,buttonMargin);

            StackPanel buttonContents = new StackPanel();
            buttonContents.Orientation = Orientation.Vertical;
            buttonContents.Width = innerStackWidth;

            TextBlock tbLine1 = new TextBlock();
            tbLine1.Text = text1;
            tbLine1.FontWeight = FontWeights.Bold;
            buttonContents.Children.Add(tbLine1);

            if (text2 != "")
            {
                TextBlock tbLine2 = new TextBlock();
                tbLine2.Text = text2;
                tbLine2.FontWeight = FontWeights.Normal;
                buttonContents.Children.Add(tbLine2);
                ourButtonHeight += buttonExtraLineHeight;
            }

            commandButton.Content = buttonContents;
            commandButton.Name = function;
            commandButton.Click += ProcessButton;

            spButtons.Children.Add(commandButton);

            spButtons.Height += ourButtonHeight + buttonMargin;
            spButtons.InvalidateMeasure();
            spButtons.InvalidateVisual();
            spButtons.InvalidateArrange();
            InvalidateVisual();
            InvalidateMeasure();

            Height += ourButtonHeight + buttonMargin;
        }

        private void ProcessButton(object sender, RoutedEventArgs e)
        {
            ourMainWindow.processCommand(((Button)sender).Name);
        }


        public void setText(string text)
        {
            lblGroupName.Text = text;
        }

        public void setImage(string image)
        {
            imgLogo.Source = new BitmapImage(new Uri(@"/images/" + image, UriKind.Relative)); 
        }

    }
}
