using CashFlowDataService.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web.Http;

namespace CashFlowDataService.Controllers
{
    public class WorkMatterController : ApiController
    {
        [HttpPost()]
        [Route("data/WorkMatter/GetAssociatedWorkMatters")]
        public DataTable GetAssociatedWorkMatters(Dictionary<string, object> parameters)
        { return WorkMatter.GetAssociatedWorkMatters(parameters); }

        [HttpPost()]
        [Route("data/WorkMatter/GetCashFlow")]
        public DataTable GetCashFlow(Dictionary<string, object> parameters)
        { return WorkMatter.GetCashFlow(parameters); }

        [HttpPost()]
        [Route("data/WorkMatter/GetHistory")]
        public DataTable GetHistory(Dictionary<string, object> parameters)
        { return WorkMatter.GetHistory(parameters); }

        [HttpPost()]
        [Route("data/WorkMatter/GetPreviousCashFlow")]
        public DataTable GetPreviousCashFlow(Dictionary<string, object> parameters)
        { return WorkMatter.GetPreviousCashFlow(parameters); }
    }
}