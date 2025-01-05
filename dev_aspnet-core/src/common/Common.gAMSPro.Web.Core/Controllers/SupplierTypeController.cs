using Abp.Application.Services.Dto;
using Common.gAMSPro.Application.Impls.Dashboard;
using Common.gAMSPro.Application.Intfs.Dashboards;
using Common.gAMSPro.Application.Intfs.Dashboards.Dto;
using Common.gAMSPro.Application.Intfs.SupplierTypes;
using Common.gAMSPro.SupplierTypes;
using Common.gAMSPro.SupplierTypes.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SupplierTypeController : CoreAmsProControllerBase
    {
        readonly ISupplierTypeAppService supplierTypeAppService;

        public SupplierTypeController(ISupplierTypeAppService supplierTypeAppService)
        {
            this.supplierTypeAppService = supplierTypeAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_SUPPLIER_TYPE_ENTITY>> CM_SUPPLIERTYPE_Search([FromBody] CM_SUPPLIER_TYPE_ENTITY? input)
        {
            return await supplierTypeAppService.CM_SUPPLIERTYPE_Search(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_SUPPLIERTYPE_Ins([FromBody] CM_SUPPLIER_TYPE_ENTITY input)
        {
            try
            {
                return await supplierTypeAppService.CM_SUPPLIERTYPE_Ins(input);

            }
            catch (Exception ex) { throw new Exception("",ex); }
        }

        [HttpPost]
        public async Task<InsertResult> CM_SUPPLIERTYPE_Upd([FromBody] CM_SUPPLIER_TYPE_ENTITY input)
        {
            return await supplierTypeAppService.CM_SUPPLIERTYPE_Upd(input);
        }

        [HttpGet]
        public async Task<CM_SUPPLIER_TYPE_ENTITY> CM_SUPPLIERTYPE_ById(string id)
        {
            return await supplierTypeAppService.CM_SUPPLIERTYPE_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_SUPPLIERTYPE_Del(string id)
        {
            return await supplierTypeAppService.CM_SUPPLIERTYPE_Del(id);
        }

        //Nguyen
        [HttpGet]
        public async Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_View_ByRegion(string regionInput)
        {
            return await supplierTypeAppService.VehicleDetection_View_ByRegion(regionInput);
        }

        [HttpGet]
        public async Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleStatistic_OfRegion_AvgByYear(string regionInput)
        {
            return await supplierTypeAppService.VehicleStatistic_OfRegion_AvgByYear(regionInput);
        }

        [HttpGet]
        public async Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_Ratio_ByRegion(string regionInput)
        {
            return await supplierTypeAppService.VehicleDetection_Ratio_ByRegion(regionInput);
        }

        [HttpGet]
        public async Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_PeakYear_ByRegion(string regionInput)
        {
            return await supplierTypeAppService.VehicleDetection_PeakYear_ByRegion(regionInput);
        }
    }
}
