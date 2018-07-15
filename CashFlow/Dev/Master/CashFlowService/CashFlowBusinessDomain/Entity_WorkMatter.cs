using System;
using System.Collections.Generic;

namespace CashFlowBusinessDomain
{
    public class Entity_WorkMatter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string strWorkMatter { get; set; }

        public string strSpecialTrackingGroup { get; set; }

        public string strWorkMatterDescription { get; set; }

        public string strInsuredName { get; set; }

        public string strAssignedAdjuster { get; set; }

        public string strAssignedManager { get; set; }

        public string strStartUser { get; set; }

        public string Department { get; set; }

        public string Portfolio { get; set; }

        public bool bHasAssociations { get; set; }

        public DateTime dtiStartTime { get; set; }

        public string strEndUser { get; set; }

        public DateTime dtiEndTime { get; set; }

        public string strWMClosed { get; set; }

        public DateTime dtiWMClosedDate { get; set; }

        public bool bIsEditable { get; set; }

        public bool bIsApprovable { get; set; }

        public bool bIsSubmittable { get; set; }

        public bool bIsRecallable { get; set; }

        public List<Entity_Exposure> Exposures { get; set; }

        /// <summary>
        /// Default constructor. Ex
        /// </summary>
        public Entity_WorkMatter()
        {
            Exposures = new List<Entity_Exposure>();
        }
    }
}
