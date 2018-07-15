namespace MatterProvisioningLibrary
{
    public class Matter
    {
        #pragma warning disable 0169
        private string lmNumber;
        private string affiliate;
        private string caseCaption;
        private string matterName;
        private string accountName;
        private string litigationManagerName;
        private string litigationManagerUserId;
        private string matterStatus;
        private string docketNumber;
        private string litigationType;
        private string stateFiled;
        private string venue;
        private string country;
        private string workMatterType;
        private string sysCreateDate;
        private bool isMatterActive;
        private bool isMatterProcessed;
        private string isLinkedMatter;
        private bool siteNeeded;
        private Microsoft.SharePoint.SPUser litigationManagerSPUser;
        #pragma warning restore 0169

        public string LMNumber { get; set; }
        public string Affiliate { get; set; }
        public string CaseCaption { get; set; }
        public string MatterName { get; set; }
        public string AccountName { get; set; }
        public string LitigationManagerName { get; set; }
        public string LitigationManagerUserId { get; set; }
        public string MatterStatus { get; set; }
        public string DocketNumber { get; set; }
        public string LitigationType { get; set; }
        public string StateFiled { get; set; }
        public string Venue { get; set; }
        public string Country { get; set; }
        public string WorkMatterType { get; set; }
        public string SysCreateDate { get; set; }
        public bool IsMatterActive { get; set; }
        public bool IsMatterProcessed { get; set; }
        public string IsLinkedMatter { get; set; }
        public bool SiteNeeded { get; set; }
        public Microsoft.SharePoint.SPUser LitigationManagerSPUser { get; set; }
    }
}