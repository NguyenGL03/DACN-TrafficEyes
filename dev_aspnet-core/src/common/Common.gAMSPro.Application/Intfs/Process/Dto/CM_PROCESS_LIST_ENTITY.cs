using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Process.Dto
{
    public class CM_PROCESS_LIST_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public int ID { get; set; }
        public string PROCESS_KEY { get; set; }
        public string PROCESS_NAME { get; set; }
        public string PROCESS_COUNT { get; set; }
        public string PREV_PROCESS_KEY { get; set; }
        public string PREV_PROCESS_NAME { get; set; }
        public string PREV_GROUP_STATUS_DONE { get; set; } // Trạng thái kết thúc quy trình bắt đầu
        public string PREV_GROUP_STATUS_DONE_NAME { get; set; } // Tên trạng thái kết thúc quy trình bắt đầu
        public string JSON_WORKFLOW { get; set; }
        public string GROUP_STATUS_DONE { get; set; } // Trạng thái kết thúc quy trình
        public string GROUP_STATUS_DONE_NAME { get; set; } // Tên trạng thái kết thúc quy trình
        public List<CM_PROCESS_ENTITY> LIST_PROCESS_ITEMS { get; set; }
        public string PROCESS_ITEMS { get; set; }
        public List<CM_PROCESS_STATUS_ENTITY> LIST_PROCESS_STATUS_ITEMS { get; set; }
        public string PROCESS_STATUS_ITEMS { get; set; }


        #region AUDIT PROPERTIES
        public int? TotalCount { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        #endregion AUDIT PROPERTIES
    }
}
