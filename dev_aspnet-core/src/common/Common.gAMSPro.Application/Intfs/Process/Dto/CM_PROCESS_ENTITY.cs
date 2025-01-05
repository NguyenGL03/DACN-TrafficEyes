using gAMSPro.Dto;
using System.Xml.Serialization;

namespace Common.gAMSPro.Process.Dto
{
    [XmlType("XmlDataProcess")]
    public class CM_PROCESS_ENTITY : PagedAndSortedInputDto
    {
        public int ID { get; set; }
        public string REQ_ID { get; set; }
        public string PROCESS_ID { get; set; }
		public string STATUS { get; set; }
		public string ROLE_USER { get; set; }
		public string ROLE { get; set; }
		public string BRANCH_ID { get; set; }
        public string CHECKER_ID { get; set; }
		public DateTime? APPROVE_DT { get; set; }
		public string PARENT_PROCESS_ID { get; set; }
		public string IS_LEAF { get; set; }
		public string COST_ID { get; set; }
		public string DVDM_ID { get; set; }
        public string NOTES { get; set; }
		public string DEP_ID { get; set; }
		public string ACTION { get; set; }
		public string FROM_STATUS { get; set; }
		public string GROUP_STATUS { get; set; }
		public string GROUP_STATUS_NAME { get; set; }
		public string FROM { get; set; }
		public string CONDITION_STATUS { get; set; }
        public string CONDITION { get; set; }
        public string DESCRIBE { get; set; }
		public string NAME_ACTION { get; set; }
		public string PROCESS_KEY { get; set; }
		public string RANGE_PROCESS { get; set; }
		public string DVDM_NAME { get; set; }
        public string TLNAME { get; set; }
		public string TLFullName { get; set; }
        public bool IS_HAS_CHILD { get; set; }
        public bool DONE { get; set; }
        public int ORDER { get; set; }
        public int? TotalCount { get; set; }
        public string DESCRIPTION { get; set; }
		public string HIDDEN_FIELD { get; set; }


    }
}
