using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Utilities;

namespace SharePointImpersonation
{
    class Program
    {
        static void Main(string[] args)
        {
            string siteString = "https://rivernetqa.trg.com";

            // get temporary site in order to get handle to the system user Token
            SPSite tempSite = new SPSite(siteString);
            SPUserToken systemUserToken = tempSite.SystemAccount.UserToken;

            // using that token, get site as system user
            using (SPSite site = new SPSite(siteString, systemUserToken))
            {
                using (SPWeb web = site.OpenWeb())
                {
                    
                    //right now, logged in as Site System Account
                    Console.WriteLine("Currently logged in as: " + web.CurrentUser.ToString());
                    string user1 = web.EnsureUser("sptest").LoginName.ToString();
                    string user2 = web.EnsureUser("sptest1").LoginName.ToString();
                    switchUser(web, siteString, user1);
                    switchUser(web, siteString, user2);
                    Console.WriteLine("Currently logged in as: " + web.CurrentUser.ToString());
                    Console.ReadKey();
                }
            }
        }

     private static void switchUser(SPWeb web, string siteStr, string user)
        {
            //impersonate somebody else
            SPUserToken userToken = web.AllUsers[user].UserToken;
            SPSite s = new SPSite(siteStr, userToken);
            SPWeb w = s.OpenWeb();
            Console.WriteLine("Currently logged in as: " + w.CurrentUser.ToString() + "(" + w.CurrentUser.Name + ")");
        }
    
    }
}
