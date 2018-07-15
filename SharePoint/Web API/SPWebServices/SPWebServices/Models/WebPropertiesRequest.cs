using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SPWebServices.Models
{
    public class WebPropertiesRequest
    {
        public string Url { get; set; }
        public string[] Properties { get; set; }
        public string[] ExcludeKeysWhichContain { get; set; }
    }
}