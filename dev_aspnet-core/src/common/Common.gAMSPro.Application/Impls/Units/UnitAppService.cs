using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Units.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Units
{
    [AbpAuthorize]
    public class UnitAppService : gAMSProCoreAppServiceBase, IUnitAppService
    {
        public UnitAppService()
        {
        }

        #region Public Method


        public async Task<PagedResultDto<CM_UNIT_ENTITY>> CM_UNIT_Search(CM_UNIT_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_UNIT_ENTITY>(CommonStoreProcedureConsts.CM_UNIT_SEARCH, input);
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Unit, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_UNIT_Ins(CM_UNIT_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_UNIT_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Unit, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_UNIT_Upd(CM_UNIT_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_UNIT_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_UNIT_ENTITY> CM_UNIT_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_UNIT_ENTITY>(CommonStoreProcedureConsts.CM_UNIT_BYID, new
            {
                UNIT_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Unit, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_UNIT_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_UNIT_APP, new
                {
                    P_UNIT_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Unit, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_UNIT_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_UNIT_DEL, new
                {
                    UNIT_ID = id
                })).FirstOrDefault();
        }
        public async Task<List<CM_UNIT_ENTITY>> CM_UNIT_List()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_UNIT_ENTITY>(CommonStoreProcedureConsts.CM_UNIT_LIST, null);
        }

        #endregion

        #region Private Method

        #endregion


    }
}
