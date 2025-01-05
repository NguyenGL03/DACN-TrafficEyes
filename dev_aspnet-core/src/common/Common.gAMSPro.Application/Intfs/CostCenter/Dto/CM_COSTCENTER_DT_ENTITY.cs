using gAMSPro.Dto;

namespace Common.gAMSPro.Departments.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_DEPARTMENT"/>
    /// </summary>
    public class CM_COSTCENTER_DT_ENTITY : PagedAndSortedInputDto
    {
        public string? COSTDT_ID { get; set; }
        public string? COST_ID { get; set; }
        public string? BRANCH_ID { get; set; }
        public string? DEP_ID { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }

    }
}
