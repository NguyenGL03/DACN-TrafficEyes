using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.Device.Dto
{
    public class CM_DEVICE_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? DEVICE_ID { get; set; }
        public string? DEVICE_CODE { get; set; }
        public string? DEVICE_NAME { get; set; }
        public string? NOTES { get; set; }
        public string? BRANCH_CREATE { get; set; }
        public string? BRANCH_ID { get; set; }
        public string? BRANCH_CODE { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? DEP_CREATE { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? EDITOR_ID { get; set; }
        public DateTime? EDITOR_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? LEVEL { get; set; }
        public int? TotalCount { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
    }
}
