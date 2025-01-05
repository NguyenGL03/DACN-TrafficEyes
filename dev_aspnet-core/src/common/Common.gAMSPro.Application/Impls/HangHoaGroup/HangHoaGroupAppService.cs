using Abp.Application.Services.Dto;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Intfs.HangHoaGroup;
using Common.gAMSPro.Intfs.HangHoaGroup.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;

namespace Common.gAMSPro.Impls.HangHoaGroup
{
    public class HangHoaGroupAppService : gAMSProCoreAppServiceBase, IHangHoaGroupAppService
    {
        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoaGroup, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_HangHoa_Group_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
              .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_HANGHOA_GROUP_APP, new
              {
                  P_HH_GROUP_ID = id,
                  P_AUTH_STATUS = ApproveStatusConsts.Approve,
                  P_CHECKER_ID = currentUserName,
                  P_APPROVE_DT = GetCurrentDateTime()
              })).FirstOrDefault();
        }

        public async Task<CM_HANGHOA_GROUP_ENTITY> CM_HangHoa_Group_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_HANGHOA_GROUP_ENTITY>(CommonStoreProcedureConsts.CM_HANGHOA_GROUP_BYID, new
            {
                HH_GROUP_ID = id
            })).FirstOrDefault();

            return model;
        }
        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoaGroup, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_HangHoa_Group_Del(string id)
        {
            return (await storeProcedureProvider
              .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_HANGHOA_GROUP_DEL, new
              {
                  HH_GROUP_ID = id
              })).FirstOrDefault();
        }

        public async Task<List<CM_HANGHOA_GROUP_ENTITY>> CM_HangHoa_Group_GetAll()
        {
            var query = "SELECT * FROM CM_HANGHOA_GROUP where RECORD_STATUS = '1'";

            var result = await storeProcedureProvider.GetDataQuery<CM_HANGHOA_GROUP_ENTITY>(query);
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoaGroup, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_HangHoa_Group_Ins(CM_HANGHOA_GROUP_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_HANGHOA_GROUP_INS, input)).FirstOrDefault();
            return result;
        }

        public async Task<PagedResultDto<CM_HANGHOA_GROUP_ENTITY>> CM_HangHoa_Group_Search(CM_HANGHOA_GROUP_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_HANGHOA_GROUP_ENTITY>(CommonStoreProcedureConsts.CM_HANGHOA_GROUP_SEARCH, input);
        }



        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoaGroup, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_HangHoa_Group_Upd(CM_HANGHOA_GROUP_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_HANGHOA_GROUP_UPD, input)).FirstOrDefault();
        }
    }
}
