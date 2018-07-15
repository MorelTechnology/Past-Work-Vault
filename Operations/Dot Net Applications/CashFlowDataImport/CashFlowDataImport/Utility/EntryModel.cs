using System;

namespace CashFlowDataImport.Utility
{
    public class EntryModel
    {
        public string WorkMatter { get; set; }
        public string Exposure { get; set; }
        public string PolicyNumber { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public string ValueName { get; set; }
        public decimal Amount { get; set; }
        public string StartUser { get; set; }
        public DateTime StartTime  { get; set; }

}
}
