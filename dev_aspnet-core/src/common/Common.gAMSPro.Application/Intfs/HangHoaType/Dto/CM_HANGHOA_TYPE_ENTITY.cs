using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.HangHoaType.Dto
{
    public class CM_HANGHOA_TYPE_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string HH_TYPE_ID { get; set; }
        public string HH_TYPE_CODE { get; set; }
        public string HH_TYPE_NAME { get; set; }
        public string HH_GROUP_ID { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string HH_GROUP_NAME { get; set; }
        public string HH_GROUP_CODE { get; set; }
        public int? TotalCount { get; set; }
        public bool? IsChecked { get; set; }
        public string BRANCH_NAME { get; set; }
        public string BRANCH_CODE { get; set; }



    }
}