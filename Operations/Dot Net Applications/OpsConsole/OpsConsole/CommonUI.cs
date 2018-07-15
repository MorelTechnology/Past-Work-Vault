using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace OpsConsole
{
    class CommonUI
    {

        public static bool setDataGridToValue(DataGrid dg, string s)
        {
            int i = 0;
            foreach (System.Data.DataRowView dr in dg.ItemsSource)
            {
                if (dr["Name"].ToString() == s)
                {
                    dg.SelectedIndex = i;
                    dg.ScrollIntoView(dg.Items[i]);
                    return true;
                }
                i++;
            }

            return false;
        }

        public static bool setDataGridByColumn(DataGrid dg, string col, string val)
        {
            int i = 0;
            foreach (System.Data.DataRowView dr in dg.ItemsSource)
            {
                if (dr[col].ToString() == val)
                {
                    dg.SelectedIndex = i;
                    return true;
                }
                i++;
            }
            return false;
        }

        public static bool setDropdownFromValue(ComboBox c, string key, string value)
        {
            int index = 0;
            foreach (System.Data.DataRowView drv in c.Items)
            {
                if (drv[key].ToString() == value)
                {
                    c.SelectedIndex = index;
                    return true;
                }
                index++;
            }
            return false;
        }

        public static void setDropdownFromValue(ComboBox c, string value)
        {
            int index = 0;
            foreach (ComboBoxItem ci in c.Items)
            {
                if (ci.Content.ToString() == value)
                {
                    c.SelectedIndex = index;
                    return;
                }
                index++;
            }
        }

        public static void showRadioButtonStatus(Button btnSet, Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                    if (c is Rectangle)
                        ((Rectangle)c).Opacity = (b == btnSet) ? 1d : 0.2d;
        }

        public static string getRadioButtonStatus(Button[] buttons)
        {
            foreach (Button b in buttons)
                foreach (Object c in ((StackPanel)b.Content).Children)
                    if (c is Rectangle)
                        if (((Rectangle)c).Opacity == 1d)
                            return b.Name;
            return "";
        }

        public static void setCheckboxButtonStatus(Button btnSet, bool on)
        {
            foreach (Object c in ((StackPanel)btnSet.Content).Children)
                if (c is Rectangle)
                    ((Rectangle)c).Opacity = (on) ? 1d : 0.2d;
        }

        public static bool getCheckboxButtonStatus(Button btnSet)
        {
            foreach (Object c in ((StackPanel)btnSet.Content).Children)
                if (c is Rectangle)
                    if (((Rectangle)c).Opacity == 1d)
                        return true;
            return false;
        }

    }
}
