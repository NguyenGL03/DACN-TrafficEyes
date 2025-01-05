using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Departments.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_DEPARTMENT"/>
    /// </summary>
    public class CM_COSTCENTER_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? COST_ID { get; set; }
        public string? COST_CODE { get; set; }
        public string? PLAN_TYPE_ID { get; set; }
        public string? COST_NAME { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? EDITER_ID { get; set; }
        public DateTime? EDIT_DT { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? DVDM_ID { get; set; }
    }
}
