using Microsoft.SharePoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsToOranges.SharePointUtilities
{
    public class Security
    {
       public static void emptySPGroup(SPGroup group)
        {
            foreach (SPUser user in group.Users) group.RemoveUser(user);
        } 
    }
}
