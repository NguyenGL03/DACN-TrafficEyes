using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System.Xml.Serialization;

namespace Common.gAMSPro.Intfs.RequestTemplate.Dto
{
    public class CM_APPROVE_GROUP : PagedAndSortedInputDto, IAuditDto
    {
        public string APPROVE_GROUP_ID { get; set; }
        public string APPROVE_USERNAME { get; set; }
        [XmlIgnore]
        public string AUTHORITY_NAME { get; set; }
        public string AUTHORITY_FULL_NAME { get; set; }
        public string APPROVE_FULL_NAME { get; set; }
        public int STEP_LEVEL { get; set; }
        public Boolean PROCESS_STATUS { get; set; }
        public string REQ_ID { get; set; }
        public string CLASS { get; set; }
        public string ICON { get; set; }
        public Boolean DONE { get; set; }
        public string ACTION { get; set; }
        public Boolean IS_REJECT { get; set; }
        public string NOTES { get; set; }
        public string TYPE { get; set; }
        public string POS_NAME { get; set; }
        public bool IsDefault { get; set; }
        [XmlIgnore]
        public string MAKER_ID { get; set; }
        [XmlIgnore]
        public DateTime? CREATE_DT { get; set; }
        [XmlIgnore]
        public string AUTH_STATUS { get; set; }
        [XmlIgnore]
        public string CHECKER_ID { get; set; }
        [XmlIgnore]
        public DateTime? APPROVE_DT { get; set; }
        [XmlIgnore]
        public string RECORD_STATUS { get; set; }
        public int? TotalCount { get; set; }
        public string TITLE_NAME { get; set; }
        public DateTime? AUTH_FROM_DATE { get; set; }
        public DateTime? AUTH_TO_DATE { get; set; }

    }
}
