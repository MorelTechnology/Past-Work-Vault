using CashFlowDataService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CashFlowDataService.Controllers
{
    public class NotificationController : ApiController
    {
        [HttpPost()]
        [Route("data/Notification/GetNotificationsForAnalysts")]
        public DataTable GetNotificationsForAnalysts(Dictionary<string, object> parameters)
        { return Notification.GetNotificationsForAnalysts(parameters); }

        [HttpPost()]
        [Route("data/Notification/Modify")]
        public void Modify([FromBody]string input)
        {
            Notification.Modify(input);
        }
    }
}