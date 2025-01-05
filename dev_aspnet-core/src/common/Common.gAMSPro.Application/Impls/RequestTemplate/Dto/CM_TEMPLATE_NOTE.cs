using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.RequestTemplate.Dto
{
    public class CM_TEMPLATE_NOTE : PagedAndSortedInputDto, IAuditDto
    {
        public string TEMPLATE_NOTE_ID { get; set; }
        public string TEMPLATE_NOTE_USERNAME { get; set; }
        public string TEMPLATE_NOTE_FULLNAME { get; set; }
        public string CONTENT { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string AUTH_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string REQ_ID { get; set; }

        public int? TotalCount { get; set; }

    }
}
