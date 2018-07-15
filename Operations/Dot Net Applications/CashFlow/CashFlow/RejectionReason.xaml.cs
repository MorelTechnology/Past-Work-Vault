using System;
using System.Collections.Generic;
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
    /// Interaction logic for RejectionReason.xaml
    /// </summary>
    public partial class RejectionReason : UserControl
    {
        MainWindow ourParent = null;
        public enum reasonType { approval, rejection };
        public reasonType eType = reasonType.rejection;


        public RejectionReason()
        {
            InitializeComponent();
            DataObject.AddPastingHandler(ebReason, OnPaste);
        }

        public void setApproveOrDeny(reasonType rt)
        {
            eType = rt;
            lblTitle.Text = (rt == reasonType.approval) ? "Reason for Approval" : "Reason for Denial";
            lblTextTruncated.Visibility = Visibility.Collapsed;
            lblCommentCounter.Text = "";
        }

        public reasonType currentPurpose()
        {
            return eType;
        }

        public void setParent(MainWindow m)
        {
            ourParent = m;
        }


        private void OnPaste(object sender, DataObjectPastingEventArgs e)
        {
            var isText = e.SourceDataObject.GetDataPresent(DataFormats.UnicodeText, true);
            if (!isText) return;

            var text = e.SourceDataObject.GetData(DataFormats.UnicodeText) as string;
            if (text.Length > 200)
                lblTextTruncated.Visibility = Visibility.Visible;
        }

        private void ebFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCommentCounter.Text = (ebReason.Text.Length == 0) ? "" : ebReason.Text.Length.ToString() + " / 200";

            if (ebReason.Text.Length < 200)
                lblTextTruncated.Visibility = Visibility.Collapsed;
        }

        private void btnApply_Click(object sender, RoutedEventArgs e)
        {
            ourParent.completeRejection();
            Visibility = Visibility.Collapsed;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Visibility = Visibility.Collapsed;
        }
    }
}
