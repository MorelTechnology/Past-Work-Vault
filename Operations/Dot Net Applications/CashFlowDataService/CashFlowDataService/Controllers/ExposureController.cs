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
    public class ExposureController : ApiController
    {
        [HttpPost()]
        [Route("data/Exposure/GetCashFlow")]
        public DataTable GetCashFlow(Dictionary<string, object> parameters)
        { return Exposure.GetCashFlow(parameters); }

        [HttpPost()]
        [Route("data/Exposure/GetHistory")]
        public DataTable GetHistory(Dictionary<string, object> parameters)
        { return Exposure.GetHistory(parameters); }

        [HttpPost()]
        [Route("data/Exposure/GetExposuresForWM")]
        public DataTable GetExposuresForWM(Dictionary<string, object> parameters)
        { return Exposure.GetExposuresForWM(parameters); }
    }
}