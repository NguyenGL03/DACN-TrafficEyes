using Core.gAMSPro.CoreModule.Utils;
using gAMSPro.ModelHelpers;
using System.Xml.Serialization;

namespace Common.gAMSPro.Intfs.RequestTemplate.Dto
{
    public class CM_REQUEST_TEMPLATE_DETAIL_ENTITY : EdiableTableRowDto, IAuditDto
    {
        public string REQUEST_TEMPLATE_DETAIL_ID { get; set; }
        public string REQUEST_TEMPLATE_DETAIL_CONTENT { get; set; }
        [XmlIgnore]
        public string REQUEST_TEMPLATE_ID { get; set; }
        public string REQUEST_TEMPLATE_DETAIL_CODE { get; set; }
        public string PAGE_SIZE { get; set; }
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
    }
}
