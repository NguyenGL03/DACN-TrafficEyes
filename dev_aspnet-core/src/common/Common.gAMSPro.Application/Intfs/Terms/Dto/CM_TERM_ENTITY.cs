using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.Terms.Dto
{
    public class CM_TERM_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? TERM_ID { get; set; }
        public string? TERM_CODE { get; set; }
        public string? TERM_NAME { get; set; }
        public string? TERM_TYPE { get; set; }
        public DateTime? TERM_DATE { get; set; }
        public string? NOTES { get; set; }
        public string? BRANCH_CREATE { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public int? TotalCount { get; set; }
    }
}
