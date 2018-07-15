using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashFlowService.Properties
{
    public class CashFlowServiceConfigurationDefinition
    {
        public string strHost { get; set; }
        public int? iPort { get; set; }
        public string strScheme { get; set; }
        public string strSubject { get; set; }
        public string strStoreName { get; set; }
        public string strStoreLocation { get; set; }
        public string strFilePath { get; set; }
        //public string strPassword { get; set; }
        public string strSerialNumber { get; set; }
        public string strThumbPrint { get; set; }
    }
}
