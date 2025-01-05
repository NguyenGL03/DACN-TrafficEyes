using gAMSPro.Dto;

namespace Common.gAMSPro.Intfs.OrganizationUnit.Dto
{
    public class ORGANIZATION_UNIT_USER_ENTITY : PagedAndSortedInputDto
    {
        public string EMP_ID { get; set; }
        public string EMP_CODE { get; set; }
        public string EMP_NAME { get; set; } 
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string AUTH_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string POS_CODE { get; set; }
        public string POS_NAME { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
        public string ORGANIZATION_UNIT_ID { get; set; }
        public int? TotalCount { get; set; }
    }
}
