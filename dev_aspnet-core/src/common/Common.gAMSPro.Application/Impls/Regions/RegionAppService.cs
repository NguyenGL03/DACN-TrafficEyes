using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Regions.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Regions
{
    [AbpAuthorize]
    public class RegionAppService : gAMSProCoreAppServiceBase, IRegionAppService
    {
        public async Task<PagedResultDto<CM_REGION_ENTITY>> CM_REGION_Search(CM_REGION_ENTITY? input)
        {
            return await storeProcedureProvider.GetPagingData<CM_REGION_ENTITY>(CommonStoreProcedureConsts.CM_REGIONS_SEARCH, input);
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Region, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_REGION_Ins(CM_REGION_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_REGION_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Region, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_REGION_Upd(CM_REGION_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_REGION_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_REGION_ENTITY> CM_REGION_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_REGION_ENTITY>(CommonStoreProcedureConsts.CM_REGION_BYID, new
            {
                REGION_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Region, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_REGION_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_REGION_APP, new
                {
                    P_REGION_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Region, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_REGION_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_REGION_DEL, new
                {
                    REGION_ID = id
                })).FirstOrDefault();
        }
    }
}
