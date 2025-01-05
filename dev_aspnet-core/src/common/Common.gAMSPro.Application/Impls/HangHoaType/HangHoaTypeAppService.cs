using Abp.Application.Services.Dto;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Intfs.HangHoaType;
using Common.gAMSPro.Intfs.HangHoaType.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;

namespace Common.gAMSPro.Impls.HangHoaType
{
    public class HangHoaTypeAppService : gAMSProCoreAppServiceBase, IHangHoaTypeAppService
    {
        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoaType, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_HANGHOA_TYPE_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
               .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_HANGHOA_TYPE_APP, new
               {
                   P_HH_TYPE_ID = id,
                   P_AUTH_STATUS = ApproveStatusConsts.Approve,
                   P_CHECKER_ID = currentUserName,
                   P_APPROVE_DT = GetCurrentDateTime()
               })).FirstOrDefault();
        }

        public async Task<CM_HANGHOA_TYPE_ENTITY> CM_HANGHOA_TYPE_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_HANGHOA_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_HANGHOA_TYPE_BYID, new
            {
                HH_TYPE_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoaType, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_HANGHOA_TYPE_Del(string id)
        {
            return (await storeProcedureProvider
               .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_HANGHOA_TYPE_DEL, new
               {
                   HH_TYPE_ID = id
               })).FirstOrDefault();
        }

        public async Task<List<CM_HANGHOA_TYPE_ENTITY>> CM_HANGHOA_TYPE_GetAll()
        {
            var query = "SELECT * FROM CM_HANGHOA_TYPE";

            var result = await storeProcedureProvider.GetDataQuery<CM_HANGHOA_TYPE_ENTITY>(query);
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoaType, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_HANGHOA_TYPE_Ins(CM_HANGHOA_TYPE_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_HANGHOA_TYPE_INS, input)).FirstOrDefault();
            return result;
        }

        public async Task<PagedResultDto<CM_HANGHOA_TYPE_ENTITY>> CM_HANGHOA_TYPE_Search(CM_HANGHOA_TYPE_ENTITY input)
        {
            var result = await storeProcedureProvider.GetPagingData<CM_HANGHOA_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_HANGHOA_TYPE_SEARCH, input);
            return result;
        }
         
        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoaType, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_HANGHOA_TYPE_Upd(CM_HANGHOA_TYPE_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_HANGHOA_TYPE_UPD, input)).FirstOrDefault();
        }
    }
}
