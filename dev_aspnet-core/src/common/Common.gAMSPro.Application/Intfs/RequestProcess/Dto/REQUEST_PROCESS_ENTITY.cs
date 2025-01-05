using Core.gAMSPro.CoreModule.Utils;
using System;

namespace Common.gAMSPro.Intfs.RequestProcess.Dto
{
    public class REQUEST_PROCESS_ENTITY : EdiableTableRowDto
    {
        public int ID { get; set; }
        public string REQ_ID { get; set; }
        public string PROCESS_ID { get; set; }
        public string STATUS { get; set; }
        public string ROLE_USER { get; set; }
        public string BRANCH_ID { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string PARENT_PROCESS_ID { get; set; }
        public string IS_LEAF { get; set; }
        public string COST_ID { get; set; }
        public string DVDM_ID { get; set; }
        public string NOTES { get; set; }
        public bool IS_HAS_CHILD { get; set; }
        public string DEP_ID { get; set; }

        public string DVDM_NAME { get; set; }
        public string TLNAME { get; set; }
        public string TLFullName { get; set; }
    }
}
