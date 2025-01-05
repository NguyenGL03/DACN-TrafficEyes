using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.AppMenus.Dto
{
    public class TL_MENU_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public int? MENU_ID { get; set; }
        public string? MENU_NAME { get; set; }
        public string? MENU_NAME_EL { get; set; }
        public string? MENU_PARENT { get; set; }
        public string? MENU_LINK { get; set; }
        public int? MENU_ORDER { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? DATE_APPROVE { get; set; }
        public string? ISAPPROVE { get; set; }
        public string? ISAPPROVE_FUNC { get; set; }
        public string? MENU_PERMISSION { get; set; }
        public string? MENU_ICON { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public int? TotalCount { get; set; }
        public string? ROLE_ID { get; set; }
        public string? PREFIX { get; set; }
    }
}
