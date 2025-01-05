using Abp.Application.Services;
using Common.gAMSPro.Application.Intfs.Dashboards.Dto;
using Common.gAMSPro.AppMenus.Dto;
using Common.gAMSPro.Intfs.Dashboards.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.gAMSPro.Application.Intfs.Dashboards
{
    public interface IDashboardAppService: IApplicationService
    {
        // Dashboard 01
        Task<Dictionary<string, object>> DB_STATUS_ASSET_VALUE_BAR(DashboardParent<DB_STATUS_ASSET_BAR> input);
        Task<Dictionary<string, object>> DB_STATUS_ASSET_QUANTITY_BAR(DashboardParent<DB_STATUS_ASSET_BAR> input);
        Task<List<DB_STATUS_ASSET_PIE>> DB_STATUS_ASSET_STATUS_PIE(DashboardParent<DB_STATUS_ASSET_PIE> input);
        Task<List<DB_STATUS_ASSET_PIE>> DB_STATUS_ASSET_QUANTITY_PIE(DashboardParent<DB_STATUS_ASSET_PIE> input);

        // Dashboard 02
        Task<List<DB_ASSET_PIE>> DB_AMORT_ASSET_PIE(DashboardParent<DB_ASSET_PIE> input);
        Task<List<DB_ASSET_PIE>> DB_VALUE_ASSET_PIE(DashboardParent<DB_ASSET_PIE> input);

        // Dashboard 03
        Task<Dictionary<string, object>> DB_BUDGET_BAR(DashboardParent<DB_BUGET> input);
        Task<List<DB_BUGET>> DB_BUDGET_PIE(DashboardParent<DB_BUGET> input);

        // Dashboard 04
        Task<Dictionary<string, object>> DB_BUY_VALUE_BAR(DashboardParent<DB_BUY_VALUE> input);
        Task<List<DB_BUY_VALUE>> DB_BUY_VALUE_PLAN_PIE(DashboardParent<DB_BUY_VALUE> input);


        //Nguyen
        Task<List<DB_VEHICLE_ENTITY>> GetVehicleDataAsync(string yearInput);
        Task<List<DB_VEHICLE_ENTITY>> VehicleStatisticsByYear_Top8(string yearInput);
        Task<List<DB_VEHICLE_ENTITY>> VehicleStatistics_Top8();
        Task<List<DB_VEHICLE_ENTITY>> VehicleDashboard_Summary();
        Task<List<DB_VEHICLE_ENTITY>> VehicleDetection_CategoryRatio();
    }
}
