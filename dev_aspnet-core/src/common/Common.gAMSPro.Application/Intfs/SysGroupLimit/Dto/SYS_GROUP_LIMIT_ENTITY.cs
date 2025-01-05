using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.SysGroupLimit.Dto
{
    public class SYS_GROUP_LIMIT_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string GROUP_ID { get; set; }
        public string GROUP_CODE { get; set; }
        public string GROUP_DES { get; set; }
        public string IS_HO { get; set; }
        public string TYPE { get; set; } 
        public string RECORD_STATUS { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; } 
        public int TotalCount { get; set; }
    }
}
