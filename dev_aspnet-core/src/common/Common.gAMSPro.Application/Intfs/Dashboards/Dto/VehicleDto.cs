using Abp.Application.Services.Dto;
using gAMSPro.Dto;

namespace Common.gAMSPro.Application.Intfs.Dashboards.Dto
{
    public class DB_VEHICLE_ENTITY: PagedAndSortedInputDto
    {
        public string? Area { get; set; }        // Khu vực
        public double? AvgMotorbikeQuantity { get; set; }
        public double? AvgCarQuantity { get; set; }

        //VehicleStatisticsByYear_Top8 
        public string? RegionNameByYear { get; set; }
        public double? AvgCarCountByYear { get; set; }
        public double? AvgMotorbikeCountByYear { get; set; }

        //VehicleStatistics_Top8
        public string? RegionName { get; set; }
        public double? AvgCarCount { get; set; }
        public double? AvgMotorbikeCount { get; set; }

        //VehicleDashboard_Summary
        public int? TotalVehicles { get; set; }
        public string? TopRegion { get; set; }
        public double? AverageVehicleDensity { get; set; }
        public string? PeakYear { get; set; }

        //VehicleDetection_CategoryRatio
        public string? CategoryName { get; set; }
        public int? TotalCount { get; set; }
        public double? Percentage { get; set; }
    }
}
