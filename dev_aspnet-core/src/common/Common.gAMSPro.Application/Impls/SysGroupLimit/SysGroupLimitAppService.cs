using Abp.Application.Services.Dto;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Intfs.SysGroupLimit;
using Common.gAMSPro.Intfs.SysGroupLimit.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;

namespace Common.gAMSPro.Impls.SysGroupLimit
{
    public class SysGroupLimitAppService : gAMSProCoreAppServiceBase, ISysGroupLimitAppService
    {
        #region Hạn mức phê duyệt

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysGroupLimit, gAMSProCorePermissions.Action.Approve)]

        public async Task<CommonResult> SYS_GROUP_LIMIT_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_APP, new
                {
                    P_GROUP_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }
        public async Task<SYS_GROUP_LIMIT_ENTITY> SYS_GROUP_LIMIT_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<SYS_GROUP_LIMIT_ENTITY>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_BYID, new
            {
                GROUP_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysGroupLimit, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> SYS_GROUP_LIMIT_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_DEL, new
                {
                    GROUP_ID = id
                })).FirstOrDefault();
        }


        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysGroupLimit, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> SYS_GROUP_LIMIT_Ins(SYS_GROUP_LIMIT_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_INS, input)).FirstOrDefault();
            return result;
        }

        public async Task<PagedResultDto<SYS_GROUP_LIMIT_ENTITY>> SYS_GROUP_LIMIT_Search(SYS_GROUP_LIMIT_ENTITY input)
        {
            var result = await storeProcedureProvider.GetPagingData<SYS_GROUP_LIMIT_ENTITY>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_SEARCH, input);

            return result;
        }


        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysGroupLimit, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> SYS_GROUP_LIMIT_Upd(SYS_GROUP_LIMIT_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_UPD, input)).FirstOrDefault();
        }

        #endregion

        #region Hạn mức phê duyệt detail


        public async Task<SYS_GROUP_LIMIT_DT_ENTITY> SYS_GROUP_LIMIT_DT_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<SYS_GROUP_LIMIT_DT_ENTITY>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_DT_BYID, new
            {
                GROUP_LM_DTID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysGroupLimitDT, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> SYS_GROUP_LIMIT_DT_Ins(SYS_GROUP_LIMIT_DT_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_DT_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysGroupLimitDT, gAMSProCorePermissions.Action.Update)]

        public async Task<InsertResult> SYS_GROUP_LIMIT_DT_Upd(SYS_GROUP_LIMIT_DT_ENTITY input)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_DT_UPD, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysGroupLimitDT, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> SYS_GROUP_LIMIT_DT_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_DT_DEL, new
                {
                    GROUP_LM_DTID = id
                })).FirstOrDefault();
        }


        public async Task<PagedResultDto<SYS_GROUP_LIMIT_DT_ENTITY>> SYS_GROUP_LIMIT_DT_Search(SYS_GROUP_LIMIT_DT_ENTITY input)
        {
            var result = await storeProcedureProvider.GetPagingData<SYS_GROUP_LIMIT_DT_ENTITY>(CommonStoreProcedureConsts.SYS_GROUP_LIMIT_DT_SEARCH, input);

            return result;
        }


        #endregion


        public async Task<List<SYS_GROUP_LIMIT_ENTITY>> SYS_GROUP_LIMIT_GetAllType()
        {

            var query = "SELECT [TYPE] FROM SYS_GROUP_LIMIT GROUP BY [TYPE]";

            var result = await storeProcedureProvider.GetDataQuery<SYS_GROUP_LIMIT_ENTITY>(query);

            return result;
        }
    }
}
