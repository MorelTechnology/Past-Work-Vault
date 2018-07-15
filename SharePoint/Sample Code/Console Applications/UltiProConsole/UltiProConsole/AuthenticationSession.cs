using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UltiProConsole.LoginService;
using System.Security.Authentication;

namespace UltiProConsole
{
    public class AuthenticationSession
    {
        public string serviceUser { get; set; }
        public string serviceUserPassword { get; set; }
        public string customerApiKey { get; set; }
        public string userApiKey { get; set; }

        public string token;
        public string message;

        public void open()
        {
            using (var client = new LoginServiceClient("WSHttpBinding_ILoginService"))
            {
                try
                {
                    AuthenticationStatus request =
                    client.Authenticate(customerApiKey, serviceUserPassword, userApiKey, serviceUser, out message, out token);
                    if (request == AuthenticationStatus.Ok) client.Close();
                    else client.Abort(); // Authentication failed, see message property for details.
                }
                catch (Exception ex)
                {
                    message = "Unexpected error occured while establishing the UltiPro authentication session.  " + ex.ToString();
                }
            }
        }
    }
}