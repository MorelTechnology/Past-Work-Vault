using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UltiProConsole.EmployeeAddressService;

namespace UltiProConsole
{
    public partial class UIForm : Form
    {
        public UIForm(AuthenticationSession session)
        {
            InitializeComponent();
            if (session != null) status.Text = "Successfully Connected";
            apiUser.Text = session.serviceUser;
            sessionToken.Text = session.token;
            ActiveControl = inputQuery;
            AcceptButton = btnSubmit; // enter will submit the query.
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            const string UltiProTokenNamespace =
                "http://www.ultimatesoftware.com/foundation/authentication/ultiprotoken";

            const string ClientAccessKeyNamespace =
                "http://www.ultimatesoftware.com/foundation/authentication/clientaccesskey";

            string customerApi = Configuration.Default.API_CustomerKey;
            // Create a proxy to the address service:
            var employeeAddressClient = new EmployeeAddressClient("WSHttpBinding_IEmployeeAddress");

            try
            {
                string query = inputQuery.Text;
                // Add the headers for the Customer API key and authentication token:
                using (new OperationContextScope(employeeAddressClient.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(
                        MessageHeader.CreateHeader(
                            "UltiProToken",
                            UltiProTokenNamespace,
                            sessionToken.Text));

                    OperationContext.Current.OutgoingMessageHeaders.Add(
                        MessageHeader.CreateHeader(
                            "ClientAccessKey",
                            ClientAccessKeyNamespace,
                            customerApi));

                    // Create a query object to find the employees:
                    var employeeQuery = new EmployeeQuery();

                    // Set one or more properties to search:
                    int employeeNumber;
                    if (int.TryParse(query, out employeeNumber))
                    {
                        employeeQuery.EmployeeNumber = "=" + employeeNumber.ToString();
                    }
                    else
                    {
                        employeeQuery.LastName = "LIKE (" + query + "%)";
                        try
                        {
                            if (cboFTPT.SelectedItem.ToString() == "Full Time Employees")
                                employeeQuery.FullOrPartTime = "=F";
                            if (cboFTPT.SelectedItem.ToString() == "Part Time Employees")
                                employeeQuery.FullOrPartTime = "=P";
                        }
                        catch (Exception) { } // empty value is ok here...

                        if (chkTerminated.Checked) employeeQuery.Status = "=T";
                        else employeeQuery.Status = "=A";
                    }

                    // Find addresses for employees matching the query criteria:
                    AddressFindResponse addressFindResponse =
                        employeeAddressClient.FindAddresses(employeeQuery);

                    // Check the results of the find to see if there are any errors:
                    if (addressFindResponse.OperationResult.HasErrors)
                    {
                        // Review each error:
                        foreach (OperationMessage message in
                            addressFindResponse.OperationResult.Messages)
                        {
                            results.Text = "Error: " + message.Message;
                        }
                    }
                    else
                    {
                        //  var pagingInfo = addressFindResponse.OperationResult.PagingInfo;

                        status.Text =
                          "The employee query returned a total of " + addressFindResponse.Results.Count() + " records.";
                        status.BackColor = Color.Yellow; status.ForeColor = Color.Red;
                        // If employee records are returned, loop through the
                        // results and output example data:
                        StringBuilder sb = new StringBuilder();

                        foreach (EmployeeAddress employeeAddress in
                            addressFindResponse.Results)
                        {
                            foreach (Address address in employeeAddress.Addresses)
                            {
                                sb.AppendLine(employeeAddress.FirstName + " " + employeeAddress.LastName +
                                    " (" + employeeAddress.EmployeeNumber + ")");
                                sb.AppendLine("Address: "
                                    + address.AddressLine1 + ", "
                                    + address.City + ", "
                                    + address.StateOrProvince + ", "
                                    + address.ZipOrPostalCode.Substring(0, 5));
                                sb.AppendLine();
                            }
                        }
                        results.Text = sb.ToString();
                    }
                }

                employeeAddressClient.Close();
            }
            catch (Exception ex)
            {
                results.Text = "Exception: " + ex;
                employeeAddressClient.Abort();
            }
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            string aboutText = "This application was designed to demonstrate data retrieval " +
                "using the UltiPro Employee Address Web Service. Information displayed in this application " +
                "is considered priviliged and confidential property of RiverStone Resources LLC and/or " +
                "its affiliates. \n\nFor more information, contact Jeremy Morel (Jeremy_Morel@trg.com) or " +
                "Shanna Barnes (Shanna_Barnes@trg.com)";
            MessageBox.Show(aboutText, "UltiPro Web Services Demo", MessageBoxButtons.OK);
        }

        private void inputQuery_TextChanged(object sender, EventArgs e)
        {
            lblQueryHelper.Visible = false;
            if (string.IsNullOrEmpty(inputQuery.Text)) lblQueryHelper.Visible = true;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            foreach (Control field in this.Controls)
            {
                if (field is TextBox)
                    ((TextBox)field).Clear();
                else if (field is ComboBox)
                    ((ComboBox)field).SelectedIndex = 0;
                else if (field is CheckBox)
                    ((CheckBox)field).Checked = false;
                status.BackColor = Control.DefaultBackColor;
                status.ForeColor = Control.DefaultForeColor;
                status.Text = "Waiting on you...";
            }
        }
    }
}