using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Units.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_UNIT"/>
    /// </summary>
    public class CM_UNIT_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string UNIT_ID { get; set; }
        public string UNIT_CODE { get; set; }
        public string UNIT_NAME { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public int? TotalCount { get; set; }
    }
}
