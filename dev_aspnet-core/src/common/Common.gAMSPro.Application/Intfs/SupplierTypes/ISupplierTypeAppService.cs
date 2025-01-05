using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Application.Intfs.Dashboards.Dto;
using Common.gAMSPro.SupplierTypes.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Application.Intfs.SupplierTypes
{
    public interface ISupplierTypeAppService : IApplicationService
    {
        Task<PagedResultDto<CM_SUPPLIER_TYPE_ENTITY>> CM_SUPPLIERTYPE_Search(CM_SUPPLIER_TYPE_ENTITY? input);
        Task<InsertResult> CM_SUPPLIERTYPE_Ins(CM_SUPPLIER_TYPE_ENTITY input);
        Task<InsertResult> CM_SUPPLIERTYPE_Upd(CM_SUPPLIER_TYPE_ENTITY input);
        Task<CommonResult> CM_SUPPLIERTYPE_Del(string id);
        Task<CommonResult> CM_SUPPLIERTYPE_App(string id, string currentUserName);
        Task<CM_SUPPLIER_TYPE_ENTITY> CM_SUPPLIERTYPE_ById(string id);

        //Nguyen
        Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_View_ByRegion(string regionInput);
        Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleStatistic_OfRegion_AvgByYear(string regionInput);
        Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_Ratio_ByRegion (string regionInput);
        Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_PeakYear_ByRegion(string regionInput);
    }
}
