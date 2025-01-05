using Abp.Application.Services.Dto;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Intfs.HangHoa;
using Common.gAMSPro.Intfs.HangHoa.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;

namespace Common.gAMSPro.Impls.HangHoa
{
    public class HangHoaAppService : gAMSProCoreAppServiceBase, IHangHoaAppService
    {
        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoa, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_HANGHOA_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_HANGHOA_APP, new
                {
                    P_HH_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        public async Task<CM_HANGHOA_ENTITY> CM_HANGHOA_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_HANGHOA_ENTITY>(CommonStoreProcedureConsts.CM_HANGHOA_BYID, new
            {
                HH_ID = id
            }));

            if (model == null)
            {
                return null;
            }

            var inputModel = model.OrderBy(p => p.TYPE_LIMIT).FirstOrDefault();

            inputModel.HHGROUP_GROUP_ID_TTCT = model?.Where(p => p.TYPE_LIMIT == "TTCT").Select(p => p.HHGROUP_GROUP_ID_TTCT).FirstOrDefault();


            var input = ObjectMapper.Map<CM_HANGHOA_ENTITY>(inputModel);
            input.HH_ID = id;
            return input;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoa, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_HANGHOA_Del(string id, string userLogin)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_HANGHOA_DEL, new
                {
                    HH_ID = id,
                    USER_LOGIN = userLogin
                })).FirstOrDefault();
        }


        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoa, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_HANGHOA_Ins(CM_HANGHOA_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_HANGHOA_INS, input)).FirstOrDefault();
            return result;
        }

        public async Task<PagedResultDto<CM_HANGHOA_ENTITY>> CM_HANGHOA_Search(CM_HANGHOA_ENTITY input)
        {
            var result = await storeProcedureProvider.GetPagingData<CM_HANGHOA_ENTITY>(CommonStoreProcedureConsts.CM_HANGHOA_SEARCH, input);
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.HangHoa, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_HANGHOA_Upd(CM_HANGHOA_ENTITY input)
        {
            input.MAKER_ID = GetCurrentUser().UserName;
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_HANGHOA_UPD, input)).FirstOrDefault();
        }
    }
}
