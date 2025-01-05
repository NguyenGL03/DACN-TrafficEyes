using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System.Diagnostics.CodeAnalysis;

namespace Common.gAMSPro.Application.Intfs.Models.Dto
{
    public class CM_MODEL_ENTITY : PagedAndSortedInputDto, IAuditDto
    {

        public string? MO_ID { get; set; }
        public string? MO_CODE { get; set; }
        public string? CAR_TYPE_ID { get; set; }
        public string? MANUFACTURER { get; set; }
        public decimal? POWER_RATE { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? MO_NAME { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public string? CAR_TYPE_NAME { get; set; }
        public string? MANUFACTURER_NAME { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public int? TotalCount { get; set; }
    }
}
