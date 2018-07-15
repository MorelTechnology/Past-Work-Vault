using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppsToOranges.UtilityTests
{
    [TestClass()]
    public class EventLoggerTests
    {

        [TestMethod()]
        public void sendEmailMessageTest()
        {
            AppsToOranges.Utility.EventLogger log = new Utility.EventLogger("Unit Test", "Application");
            var smtp = "smtp.appstooranges.com";
            var port = 25;
            var from = "jeremy@appstooranges.com";
            var to = "jeremy@appstooranges.com";
            var subject = "This is a subject";
            var body = "This is the body of the message.  Voila!";

            log.sendEmailMessage(smtp, port, from, to, subject, body);
        }

    }
}