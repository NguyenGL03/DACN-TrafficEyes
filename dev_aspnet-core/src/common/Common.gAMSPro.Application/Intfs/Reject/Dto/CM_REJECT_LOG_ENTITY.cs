using gAMSPro.Dto;

namespace Common.gAMSPro.Reject.Dto
{
    public class CM_REJECT_LOG_ENTITY : PagedAndSortedInputDto
    {
        public string LOG_ID { get; set; }
        public string STAGE { get; set; }
        public string TRN_ID { get; set; }
        public string TRN_TYPE { get; set; }
        public DateTime? LOG_DT { get; set; }
        public string AUTH_STAT { get; set; }
        public string REASON { get; set; }
        public string IS_LATEST { get; set; }
        public string REJECTED_BY { get; set; }
        public DateTime? REJECTED_DT { get; set; }
        public string LASTED_REASON { get; set; }
        public string EMP_NAME { get; set; }
        public int? TotalCount { get; set; }
        public string PROCESS_ID { get; set; }
        public string TYPE { get; set; }
        public int REF_ID { get; set; }
        public bool? IS_SEND_MAIL { get; set; }
    }
}
