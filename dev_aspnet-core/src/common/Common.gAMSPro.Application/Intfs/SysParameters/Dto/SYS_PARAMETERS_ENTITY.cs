using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.SysParameters.Dto
{
    /// <summary>
    /// <see cref="Models.SYS_PARAMETERS"/>
    /// </summary>
    public class SYS_PARAMETERS_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public decimal? Id { get; set; }
        public string? ParaKey { get; set; }
        public string? ParaValue { get; set; }
        public string? DataType { get; set; }
        public string? Description { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public int? TotalCount { get; set; }
    }
}
