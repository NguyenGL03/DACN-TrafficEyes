using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.RequestTemplate.Dto
{
    public class CM_REQUEST_TEMPLATE_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string REQUEST_TEMPLATE_ID { get; set; }
        public string TYPE_TEMPLATE_ID { get; set; }
        public string TYPE_TEMPLATE_NAME { get; set; }
        public string REQUEST_TEMPLATE_NAME { get; set; }
        public string REQUEST_TEMPLATE_STORE { get; set; }
        public string REQUEST_TEMPLATE_CODE { get; set; }
        public string CREATE_LOCATION { get; set; }
        public string BRANCH_ID { get; set; }
        public string BRANCH_NAME { get; set; }
        public int CURRENT_STEP { get; set; }
        public string TITLE { get; set; }
        public string REPORT_NO { get; set; }
        public bool SCHEME_IN { get; set; }
        public string SCHEME_OUT { get; set; }
        public string CONTENT { get; set; }
        public string HEADER { get; set; }
        public string NOTES { get; set; }
        public List<string> SENT_TO { get; set; }
        public string REQUEST_TEMPLATE_XML { get; set; }
        public string GROUP_APPROVE1 { get; set; }
        public string GROUP_APPROVE2 { get; set; }
        public string GROUP_APPROVE3 { get; set; }
        public string GROUP_APPROVE4 { get; set; }
        public List<string> GROUP_APPROVE { get; set; }
        public string GROUP_APPROVES { get; set; }
        public string SHARE_USER { get; set; }
        public string USER_APPROVES { get; set; }
        public string MAKER_ID { get; set; }
        public string MAKER_FULLNAME { get; set; }
        public DateTime? REPORT_DT { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string AUTH_STATUS_APP { get; set; }
        public string AUTH_STATUS_APP_NAME { get; set; }
        public bool IS_SENT_APPROVE { get; set; }
        public bool IS_BACK_DATE { get; set; }
        public string USER_LOGIN { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string RECORD_STATUS { get; set; }
        public List<CM_REQUEST_TEMPLATE_DETAIL_ENTITY> REQUEST_TEMPLATE_DETAILs { get; set; }
        public string REQUEST_TEMPLATE_DETAIL_XML { get; set; }
        public string TYPE_SEARCH { get; set; }
        public int? TotalCount { get; set; }
        public DateTime? FROM_DATE { get; set; }
        public DateTime? TO_DATE { get; set; }
    }
}
