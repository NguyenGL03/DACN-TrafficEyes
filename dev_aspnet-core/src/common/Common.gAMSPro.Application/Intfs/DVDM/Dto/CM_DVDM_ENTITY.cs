using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.DVDM.Dto
{
    public class CM_DVDM_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? DVDM_ID { get; set; }
        public string? DVDM_CODE { get; set; }
        public string? DVDM_NAME { get; set; }
        public string? NOTES { get; set; }
        public string? EDITER_ID { get; set; }
        public string? EDIT_DT { get; set; }
        public bool IS_DVDM { get; set; }
        public bool IS_KHOI { get; set; }
        public bool IS_DVCM { get; set; }
        public bool IS_PTGD { get; set; }
        public bool IS_GDK { get; set; }
        public string? TYPE { set; get; }
        public string? COST_NAME { set; get; }
        public string? COST_CODE { set; get; }
        public string? STATUS_NAME { set; get; }
        public string? RECORD_STATUS { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public int? TOP { set; get; }
        public int? TotalCount { set; get; }
    }
}
