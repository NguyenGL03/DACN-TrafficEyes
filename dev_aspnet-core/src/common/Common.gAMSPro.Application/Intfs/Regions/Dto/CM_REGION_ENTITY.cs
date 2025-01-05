using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System;

namespace Common.gAMSPro.Regions.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_REGION"/>
    /// </summary>
    public class CM_REGION_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? REGION_ID { get; set; }
        public string? REGION_CODE { get; set; }
        public string? REGION_NAME { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public int? TotalCount { get; set; }

        public string? BRANCH_ID { get; set; }
        public string? REGION_PARENT { get; set; }
    }
}
