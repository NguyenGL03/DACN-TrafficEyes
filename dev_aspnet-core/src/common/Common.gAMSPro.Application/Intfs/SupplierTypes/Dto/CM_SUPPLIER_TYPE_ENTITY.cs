using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.SupplierTypes.Dto
{
    /// <summary>
    /// <see cref="Models.CM_SUPPLIERTYPE"/>
    /// </summary>
    public class CM_SUPPLIER_TYPE_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? SUP_TYPE_ID { get; set; }
        public string? SUP_TYPE_CODE { get; set; }
        public string? SUP_TYPE_NAME { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public int? TotalCount { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? brancH_CODE { get; set; }


        //Analysis
        public string? AvgYear{ get; set; }
        public string? AvgCategoryName { get; set; }
        public double? AvgVehicleCount { get; set; }


        public int? DetectionID { get; set; }
        public string? ImagePath { get; set; }
        public string? CategoryName { get; set; }
        public string? RegionName { get; set; }
        public int? VehicleCount { get; set; }
        public string? DetectionAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? CreatedDate { get; set; }

        public string? RatioCaregoryName { get; set; }
        public int? RatioTotalCount { get; set; }
        public double? RatioPercentage { get;set; }

        public string? PeakYear { get; set; }
        public string? AvgVehicleDensity { get;set; }    

    }
}
