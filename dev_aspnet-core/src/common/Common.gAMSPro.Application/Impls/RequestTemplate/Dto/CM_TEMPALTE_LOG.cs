using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System.Xml.Serialization;

namespace Common.gAMSPro.Intfs.RequestTemplate.Dto
{
    public class CM_TEMPALTE_LOG : PagedAndSortedInputDto, IAuditDto
    {
        public string TEMPLATE_PROCESS_ID { get; set; }
        public string APPROVE_GROUP_ID { get; set; }
        [XmlIgnore]
        public string AUTHORITY_NAME { get; set; }
        public string AUTHORITY_FULL_NAME { get; set; }
        public string HANDOVER_NAME { get; set; }
        public string HANDOVER_FULL_NAME { get; set; }
        public string APP_NAME { get; set; }
        public string APP_FULL_NAME { get; set; }

        public string APPROVE_FULL_NAME { get; set; }
        public string CHECKER_FULL_NAME { get; set; }

        public string PROCESS_DESC { get; set; }
        public string REQ_ID { get; set; }
        public string ACTION { get; set; }
        public string NOTES { get; set; }
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

        [XmlIgnore]
        public string XML_SHARE_REMOVE { get; set; }
        [XmlIgnore]
        public string XML_SHARE_ADD { get; set; }

        public List<CM_TEMPALTE_LOG_SHARE> LST_SHARE_REMOVE { get; set; }
        public List<CM_TEMPALTE_LOG_SHARE> LST_SHARE_ADD { get; set; }
    }

    public class CM_TEMPALTE_LOG_SHARE
    {
        public string TL_NAME { get; set; }
        public string TL_FULL_NAME { get; set; }

    }
}
