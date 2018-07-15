using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace PINSPayments
{
    class UIHelpers
    {
        public static void showRadioButtonStatus(Button btnSet, Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                {
                    if (c is Rectangle)
                        ((Rectangle)c).Opacity = (b == btnSet) ? 1d : 0.2d;
                    if (c is Ellipse)
                        ((Ellipse)c).Opacity = (b == btnSet) ? 1d : 0.2d;
                }
        }

        public static string getRadioButtonStatus(Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                {
                    if (c is Rectangle)
                        if (((Rectangle)c).Opacity == 1d)
                            return b.Name;

                    if (c is Ellipse)
                        if (((Ellipse)c).Opacity == 1d)
                            return b.Name;
                }
            return "";
        }

    }
}
