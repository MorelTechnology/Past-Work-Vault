using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
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

namespace OpsConsole
{
    /// <summary>
    /// Interaction logic for Admin2ResourceAndOrTicket.xaml
    /// </summary>
    public partial class Admin2ResourceAndOrTicket : UserControl
    {
        #region class variables and constructor
        Admin2 parent;
        string mode = "";
        string associate = "";
        string function = "";
        bool ticketRequired = false;

        public Admin2ResourceAndOrTicket()
        {
            InitializeComponent();
        }
        #endregion

        #region set parent
        public void setParent(Admin2 p)
        {
            parent = p;
        }
        #endregion

        #region event handlers
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            parent.closeResourceOrTicket();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ticketRequired && (txtTicket.Text == ""))
            {
                MessageBox.Show("You must enter a Service Desk Ticket Number");
                return;
            }

            if ((mode == "ADD") && (lbADResults.SelectedIndex == -1))
            {
                MessageBox.Show("You must select a user to add");
                return;
            }

            if (mode == "ADD")
            {
                string assoc = lbADResults.SelectedValue.ToString();
                string perm = (rectModify.Opacity == 1d) ? "Modify" : "Read";
                parent.AddAssociateToFunction(perm, txtTicket.Text, assoc, function);
            }
            else
            {
                parent.ModifyPermissions(mode, txtTicket.Text, associate, function);
            }

            parent.closeResourceOrTicket();
        }

        private void txtADSearchFor_TextChanged(object sender, TextChangedEventArgs e)
        {
            DataTable dtResults = new DataTable();
            dtResults.Columns.Add("Name");
            dtResults.Columns.Add("SAM");

            lbADResults.Items.Clear();
            foreach (DataRow dr in parent.dtAssociates.Rows)
            {
                string name = dr["AdjustedName"].ToString();
                if (name.ToLower().IndexOf(txtADSearchFor.Text.ToLower()) >= 0)
                    lbADResults.Items.Add(name);
            }
        }

        private void btnLookupTicket(object sender, RoutedEventArgs e)
        {
            lookupTicket();
        }

        private void btnReadOnly_Click(object sender, RoutedEventArgs e)
        {
            setIndicatorReadOnlyOrModify(readOnly: true);
        }

        private void btnModify_Click(object sender, RoutedEventArgs e)
        {
            setIndicatorReadOnlyOrModify(readOnly: false);
        }

        private void setIndicatorReadOnlyOrModify(bool readOnly)
        {
            rectModify.Opacity = (readOnly) ? 0.2d : 1d;
            rectReadOnly.Opacity = (readOnly) ? 1d : 0.2d;
        }

        private void txtTicket_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
                lookupTicket();
        }
        #endregion

        #region screen logic
        private void showTicketArea(bool showTicketArea)
        {
            if (showTicketArea)
            {
                gridTicket.Visibility = System.Windows.Visibility.Visible;
                gridAssociate.Margin = new Thickness(390d, gridAssociate.Margin.Top, gridAssociate.Margin.Right, gridAssociate.Margin.Bottom);
                gridAccess.Margin = new Thickness(390d, gridAccess.Margin.Top, gridAccess.Margin.Right, gridAccess.Margin.Bottom);
                gridOuter.Width = 752;
            }
            else
            {
                gridTicket.Visibility = System.Windows.Visibility.Collapsed;
                gridAssociate.Margin = new Thickness(10d, gridAssociate.Margin.Top, gridAssociate.Margin.Right, gridAssociate.Margin.Bottom);
                gridAccess.Margin = new Thickness(10d, gridAccess.Margin.Top, gridAccess.Margin.Right, gridAccess.Margin.Bottom);
                gridOuter.Width = 372d;
            }
        }

        public void AddUserMode(bool tktRequired, string title, string func)
        {
            ticketRequired = tktRequired;

            showTicketArea(tktRequired);
            gridAssociate.Visibility = System.Windows.Visibility.Visible;
            //gridOuter.Width = 752d;
            lblTitle.Text = (tktRequired) ? ("Add associate to " + title) : "Add associate";

            txtADSearchFor.Text = "";
            lbADResults.Items.Clear();

            txtTicket.Text = "";
            tbTicketInfo.Text = "";

            setIndicatorReadOnlyOrModify(readOnly: false);

            mode = "ADD";
            function = func;
        }

        public void EnterTicketMode(string change, string title, string func, string user)
        {
            txtTicket.Text = "";
            tbTicketInfo.Text = "";
            ticketRequired = true;

            gridTicket.Visibility = System.Windows.Visibility.Visible;
            gridAssociate.Visibility = System.Windows.Visibility.Collapsed;
            gridAccess.Visibility = System.Windows.Visibility.Collapsed;
            gridOuter.Width = 393;
            lblTitle.Text = "Modify permissions";

            associate = user;
            mode = change;
            function = func;
        }


        private void lookupTicket()
        {
            if (txtTicket.Text == "")
            {
                MessageBox.Show("You must enter a ticket number");
                return;
            }

            showTicket(tbTicketInfo, txtTicket.Text);
        }


        public bool showTicket(TextBlock tbNote, string ticketNumber)
        {
            SDTicket.RequestLongForm ticketInfo = SDTicket.getServiceTicketEntry(ticketNumber);
            if (ticketInfo != null)
            {
                tbNote.Inlines.Clear();
                tbNote.Inlines.Add(new Run("Category: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CATEGORY + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("By: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CREATEDBY + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Time: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.CREATEDTIME + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Dept: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.DEPARTMENT + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Requester: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.REQUESTER + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("E-Mail: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.REQUESTEREMAIL + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Subject: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.SUBJECT + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Desc: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.SHORTDESCRIPTION.TrimStart().Replace("&nbsp;", " ") + Environment.NewLine) { Foreground = Brushes.MidnightBlue });

                tbNote.Inlines.Add(new Run("Technician: ") { FontWeight = FontWeights.Bold });
                tbNote.Inlines.Add(new Run(ticketInfo.TECHNICIAN + Environment.NewLine) { Foreground = Brushes.MidnightBlue });
                return true;
            }
            else
            {
                tbNote.Inlines.Clear();
                tbNote.Inlines.Add(new Run("Unable to find ticket") { FontWeight = FontWeights.Bold });
                return false;
                // MarkTicketAsBad(ticketNumber);
            }
        }
        #endregion

    }
}
