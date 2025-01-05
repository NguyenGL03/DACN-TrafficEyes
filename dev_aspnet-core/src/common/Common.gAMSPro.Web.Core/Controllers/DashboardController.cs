using Common.gAMSPro.Application.Intfs.Dashboards;
using Common.gAMSPro.Application.Intfs.Dashboards.Dto;
using Common.gAMSPro.CoreModule.Helper;
using Common.gAMSPro.Departments;
using Common.gAMSPro.Intfs.Dashboards;
using Common.gAMSPro.Intfs.Dashboards.Dto;
using Common.gAMSPro.Web.Controllers;
using GSOFTcore.gAMSPro.Helpers;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Common.gAMSPro.Web.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DashboardController: CoreAmsProControllerBase
    {
        readonly IDashboardAppService dashboardAppService;

        public DashboardController(IDashboardAppService dashboardAppService)
        {
            this.dashboardAppService = dashboardAppService;
        }

        [HttpPost]
        public async Task<Dictionary<string, object>> DB_STATUS_ASSET_VALUE_BAR([FromBody] DashboardParent<DB_STATUS_ASSET_BAR> input)
        {
            return await this.dashboardAppService.DB_STATUS_ASSET_VALUE_BAR(input);
        }

        [HttpPost]
        public async Task<Dictionary<string, object>> DB_STATUS_ASSET_QUANTITY_BAR([FromBody] DashboardParent<DB_STATUS_ASSET_BAR> input)
        {
            return await this.dashboardAppService.DB_STATUS_ASSET_QUANTITY_BAR(input);
        }

        [HttpPost]
        public async Task<List<DB_STATUS_ASSET_PIE>> DB_STATUS_ASSET_STATUS_PIE([FromBody] DashboardParent<DB_STATUS_ASSET_PIE> input)
        {
            return await this.dashboardAppService.DB_STATUS_ASSET_STATUS_PIE(input);
        }

        [HttpPost]
        public async Task<List<DB_STATUS_ASSET_PIE>> DB_STATUS_ASSET_QUANTITY_PIE([FromBody] DashboardParent<DB_STATUS_ASSET_PIE> input)
        {
            return await this.dashboardAppService.DB_STATUS_ASSET_QUANTITY_PIE(input);
        }
        [HttpPost]
        public async Task<List<DB_ASSET_PIE>> DB_AMORT_ASSET_PIE([FromBody] DashboardParent<DB_ASSET_PIE> input)
        {
            return await this.dashboardAppService.DB_AMORT_ASSET_PIE(input);
        }
        [HttpPost]
        public async Task<List<DB_ASSET_PIE>> DB_VALUE_ASSET_PIE([FromBody] DashboardParent<DB_ASSET_PIE> input)
        {
            return await this.dashboardAppService.DB_VALUE_ASSET_PIE(input);
        }
        [HttpPost]
        public async Task<Dictionary<string, object>> DB_BUDGET_BAR([FromBody] DashboardParent<DB_BUGET> input)
        {
            return await this.dashboardAppService.DB_BUDGET_BAR(input);
        }
        [HttpPost]
        public async Task<List<DB_BUGET>> DB_BUDGET_PIE([FromBody] DashboardParent<DB_BUGET> input)
        {
            return await this.dashboardAppService.DB_BUDGET_PIE(input);
        }
        [HttpPost]
        public async Task<Dictionary<string, object>> DB_BUY_VALUE_BAR([FromBody] DashboardParent<DB_BUY_VALUE> input)
        {
            return await this.dashboardAppService.DB_BUY_VALUE_BAR(input);
        }
        [HttpPost]
        public async Task<List<DB_BUY_VALUE>> DB_BUY_VALUE_PLAN_PIE([FromBody] DashboardParent<DB_BUY_VALUE> input)
        {
            return await this.dashboardAppService.DB_BUY_VALUE_PLAN_PIE(input);
        }

        //Nguyen
        [HttpGet]
        public async Task<List<DB_VEHICLE_ENTITY>> GetVehicleData(string yearInput)
        {
            return await dashboardAppService.GetVehicleDataAsync(yearInput);
        }
        [HttpGet]
        public async Task<List<DB_VEHICLE_ENTITY>> VehicleStatisticsByYear_Top8(string yearInput)
        {
            return await dashboardAppService.VehicleStatisticsByYear_Top8(yearInput);
        }
        [HttpGet]
        public async Task<List<DB_VEHICLE_ENTITY>> VehicleStatistics_Top8()
        {
            Console.WriteLine("VehicleStatistics_Top8 runnnig");
            return await dashboardAppService.VehicleStatistics_Top8();
        }
        [HttpGet]
        public async Task<List<DB_VEHICLE_ENTITY>> VehicleDashboard_Summary()
        {
            return await dashboardAppService.VehicleDashboard_Summary();
        }
        [HttpGet]
        public async Task<List<DB_VEHICLE_ENTITY>> VehicleDetection_CategoryRatio()
        {
            return await dashboardAppService.VehicleDetection_CategoryRatio();
        }


    }
}
