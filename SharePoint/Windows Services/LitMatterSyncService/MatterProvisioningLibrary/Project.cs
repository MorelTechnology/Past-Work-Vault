using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SharePoint;

namespace MatterProvisioningLibrary
{
    public class Project
    {

#pragma warning disable 0169
        private string projectId;
        private string projectDescription;
        private string projectLead;
        private string projectName;
        private string projectStatus;
        private SPUser projectLeadSPUser;
#pragma warning restore 0169

        public string ProjectId { get; set; }
        public string ProjectDescription;
        public string ProjectLead;
        public string ProjectName;
        public string ProjectStatus;
        public SPUser ProjectLeadSPUser;
    }
}
