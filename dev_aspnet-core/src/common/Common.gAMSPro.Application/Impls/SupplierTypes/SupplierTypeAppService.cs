using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Application.Intfs.Dashboards.Dto;
using Common.gAMSPro.Application.Intfs.SupplierTypes;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.SupplierTypes.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;

namespace Common.gAMSPro.SupplierTypes
{
    [AbpAuthorize]
    public class SupplierTypeAppService : gAMSProCoreAppServiceBase, ISupplierTypeAppService
    {
        #region Public Method
        public async Task<PagedResultDto<CM_SUPPLIER_TYPE_ENTITY>> CM_SUPPLIERTYPE_Search(CM_SUPPLIER_TYPE_ENTITY? input)
        {
            return await storeProcedureProvider.GetPagingData<CM_SUPPLIER_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_SUPPLIERTYPE_SEARCH, input);
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SupplierType, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_SUPPLIERTYPE_Ins(CM_SUPPLIER_TYPE_ENTITY input)
        {

            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SUPPLIERTYPE_INS, input)).FirstOrDefault();
            if (result == null)
            {
                //return new InsertResult();
            }    
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SupplierType, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_SUPPLIERTYPE_Upd(CM_SUPPLIER_TYPE_ENTITY input)
        {
            SetAuditForUpdate(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SUPPLIERTYPE_UPD, input)).FirstOrDefault();
            if (result == null)
            {
                return new InsertResult();
            }
            return result;
        }

        public async Task<CM_SUPPLIER_TYPE_ENTITY> CM_SUPPLIERTYPE_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_SUPPLIER_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_SUPPLIERTYPE_BYID, new
            {
                SUP_TYPE_ID = id
            })).FirstOrDefault();
            if (model == null)
            {
                return new CM_SUPPLIER_TYPE_ENTITY();
            }
            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SupplierType, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_SUPPLIERTYPE_App(string id, string currentUserName)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_SUPPLIERTYPE_APP, new
                {
                    P_SUP_TYPE_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
            if (result == null)
            {
                return new CommonResult();
            }
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SupplierType, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_SUPPLIERTYPE_Del(string id)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_SUPPLIERTYPE_DEL, new
                {
                    SUP_TYPE_ID = id
                })).FirstOrDefault();
            if (result == null) { return new CommonResult(); }
            return result;
        }

        #endregion

        #region Private Method


        #endregion

        //Nguyen

        public async Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_View_ByRegion(string regionInput)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_SUPPLIER_TYPE_ENTITY>(CommonStoreProcedureConsts.VehicleDetection_View_ByRegion, new
            {
                RegionName = regionInput
            });

        }

        public async Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleStatistic_OfRegion_AvgByYear(string regionInput)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_SUPPLIER_TYPE_ENTITY>(CommonStoreProcedureConsts.VehicleStatistic_OfRegion_AvgByYear, new
            {
                RegionName = regionInput
            });

        }

        public async Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_Ratio_ByRegion(string regionInput)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_SUPPLIER_TYPE_ENTITY>(CommonStoreProcedureConsts.VehicleDetection_Ratio_ByRegion, new
            {
                RegionName = regionInput
            });

        }

        public async Task<List<CM_SUPPLIER_TYPE_ENTITY>> VehicleDetection_PeakYear_ByRegion(string regionInput)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_SUPPLIER_TYPE_ENTITY>(CommonStoreProcedureConsts.VehicleDetection_PeakYear_ByRegion, new
            {
                RegionName = regionInput
            });

        }
    }
}
