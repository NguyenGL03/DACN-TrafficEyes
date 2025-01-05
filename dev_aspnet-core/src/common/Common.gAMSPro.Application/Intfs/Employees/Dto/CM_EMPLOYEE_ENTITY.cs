using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Employees.Dto
{
    public class CM_EMPLOYEE_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string EMP_ID { get; set; }
        public string EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string BRANCH_ID { get; set; }
        public string DEP_ID { get; set; }
        public string BRANCH_TYPE { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string AUTH_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string DEP_NAME { get; set; }
        public string DEP_NAME2 { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }
        public string KHU_VUC { get; set; }
        public string CHI_NHANH { get; set; }
        public string PGD { get; set; }
        public string LEVEL { get; set; }
        public string POS_CODE { get; set; }
        public string POS_NAME { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public string PHONE_NUMBER { get; set; }
        public string USER_DOMAIN { get; set; }
        public string ACTION { get; set; }
        public int? TotalCount { get; set; }
        public string CODE { get; set; }
        public string NAME { get; set; }
        public string DEP_CODE { get; set; }
        public string MA_CHUC_DANH { get; set; }
        public string TEN_CHUC_DANH { get; set; }
        public string STORED_NAME { get; set; }
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
    }
}
