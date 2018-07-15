using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeOffCalendarSync
{
    public class ADUser
    {
        public string displayName { get; set; }
        public string primaryMail { get; set; }
        public string department { get; set; }
        public string description { get; set; }
        public string manager { get; set; }
        public string distinguishedName { get; set; }
    }
}
