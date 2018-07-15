using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using UltiProConsole.EmployeeAddressService;
using UltiProConsole.LoginService;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace UltiProConsole
{
    /// <summary>
    /// Demonstrates UltiPro Data Retrieval.  This is a console app, but also can be form driven for a user-facing demo.
    /// To enable the form compile and run with command line argument '-form' or set this arguement in this project's
    /// debug property.
    /// </summary>
    public class Program
    {
        [STAThread]
        internal static void Main(string[] args)
        {
            // Setup an Authentication Session

            AuthenticationSession session = new AuthenticationSession();
            session.serviceUser = Configuration.Default.API_Username;
            session.serviceUserPassword = Configuration.Default.API_Password;
            session.customerApiKey = Configuration.Default.API_CustomerKey;
            session.userApiKey = Configuration.Default.API_UserKey;
            Console.WriteLine("Establishing a Web Service session.  One moment...");
            session.open();

            try
            {
                if (args[0].ToString().ToLower() == "-form")
                {
                    ShowWindow(GetConsoleWindow(), CONSOLE_HIDE);
                    Application.EnableVisualStyles();
                    Form ui = new UIForm(session);
                    Application.Run(ui);
                }
                else FindAddresses(session.token);
            }
            catch (Exception)
            {
                FindAddresses(session.token);
            }
        }

        private static void FindAddresses(string token)
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
                Console.WriteLine("Type ONE of the following, then press <Enter>:");
                Console.WriteLine(" - an Employee ID number");
                Console.WriteLine(" - a partial last name");
                Console.WriteLine(" - an entire last name");
                Console.WriteLine(); Console.Write("> ");
                string query = Console.ReadLine();
                // Add the headers for the Customer API key and authentication token:
                using (new OperationContextScope(employeeAddressClient.InnerChannel))
                {
                    OperationContext.Current.OutgoingMessageHeaders.Add(
                        MessageHeader.CreateHeader(
                            "UltiProToken",
                            UltiProTokenNamespace,
                            token));

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
                        //employeeQuery.FullOrPartTime = "=F";
                        employeeQuery.Status = "=A"; //Active Employees Only
                    }
                    // Set paging properties:
                    employeeQuery.PageSize = "10";
                    employeeQuery.PageNumber = "1";

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
                            Console.WriteLine("Error message: " + message.Message);
                        }
                    }
                    else
                    {
                        var pagingInfo = addressFindResponse.OperationResult.PagingInfo;

                        Console.WriteLine(
                            "The employee query returned a total of {0} records.",
                            pagingInfo.TotalItems);

                        // If employee records are returned, loop through the
                        // results and output example data:
                        foreach (EmployeeAddress employeeAddress in
                            addressFindResponse.Results)
                        {
                            foreach (Address address in employeeAddress.Addresses)
                            {
                                Console.WriteLine();
                                Console.WriteLine("Employee Number: " + employeeAddress.EmployeeNumber);
                                Console.WriteLine("Name: {0} {1} Address: {2}, {3}, {4} {5}",
                                    employeeAddress.FirstName,
                                    employeeAddress.LastName,
                                    address.AddressLine1,
                                    address.City,
                                    address.StateOrProvince,
                                    address.ZipOrPostalCode);
                            }
                        }
                    }
                }

                var endInput = Console.ReadKey();
                if (endInput.Key.ToString().ToLower() == "a") Console.Clear(); FindAddresses(token);
                employeeAddressClient.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception: " + ex);
                employeeAddressClient.Abort();
            }
        }

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private const int CONSOLE_HIDE = 0;
        private const int CONSOLE_SHOW = 5;
    }
}