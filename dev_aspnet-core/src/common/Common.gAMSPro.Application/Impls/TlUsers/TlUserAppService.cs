using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.CoreModule.Consts;
using Common.gAMSPro.TlUsers.Dto;
using Core.gAMSPro.Application;
using gAMSPro.Consts;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.TlUsers
{
    [AbpAuthorize]
    public class TlUserAppService : gAMSProCoreAppServiceBase, ITlUserAppService
    {

        public TlUserAppService()
        {
        }

        #region Public Method

        public async Task<PagedResultDto<TL_USER_ENTITY>> TL_USER_Search(TL_USER_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<TL_USER_ENTITY>(CommonStoreProcedureConsts.TL_USER_SEARCH, input);
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, PermissionPageConsts.UserManager, gAMSProCorePermissions.Action.Update)]
        public async Task<IDictionary<string, object>> MD_USER_MANAGER_Upd_AUTHORITYUSER(TL_USER_ENTITY input)
        {

            var result = await storeProcedureProvider
               .GetResultValueFromStore(CommonStoreProcedureConsts.MD_USER_MANAGER_Upd_AUTHORITYUSER, input);

            return result;

        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, PermissionPageConsts.UserManager, gAMSProCorePermissions.Action.Update)]
        public async Task<IDictionary<string, object>> MD_USER_MANAGER_Del_AUTHORITYUSER(TL_USER_ENTITY input)
        {

            var result = await storeProcedureProvider
               .GetResultValueFromStore(CommonStoreProcedureConsts.MD_USER_MANAGER_Del_AUTHORITYUSER, input);

            return result;

        }

        public async Task<List<TL_USER_ENTITY>> TL_USER_GET_List(TL_USER_ENTITY input)
        {
            input.USER_LOGIN = GetCurrentUserName();
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<TL_USER_ENTITY>("TL_USER_GET_List", input);
            return result;
        } 

        public async Task<PagedResultDto<TL_USER_ENTITY>> TL_USER_GET_List_v2(TL_USER_ENTITY input)
        {
            input.USER_LOGIN = GetCurrentUserName();
            var result = await storeProcedureProvider.GetPagingData<TL_USER_ENTITY>("TL_USER_GET_List_v2", input);
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.TlUser, gAMSProCorePermissions.Action.Create)]
        public async Task<IDictionary<string, object>> TL_USER_Ins(TL_USER_ENTITY input)
        {
            SetAuditForInsert(input);
            return (await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.TL_USER_INS, input));
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.TlUser, gAMSProCorePermissions.Action.Update)]
        public async Task<IDictionary<string, object>> TL_USER_Upd(TL_USER_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.TL_USER_UPD, input));
        }

        public async Task<TL_USER_ENTITY> TL_USER_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<TL_USER_ENTITY>(CommonStoreProcedureConsts.TL_USER_BYID, new
            {
                USER_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.TlUser, gAMSProCorePermissions.Action.Approve)]
        public async Task<IDictionary<string, object>> TL_USER_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                                .GetResultValueFromStore(CommonStoreProcedureConsts.TL_USER_APP, new
                                {
                                    USER_ID = id,
                                    CHECKER_ID = currentUserName,
                                    AUTH_STATUS = ApproveStatusConsts.Approve,
                                    APPROVE_DT = GetCurrentDateTime()
                                }));
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.TlUser, gAMSProCorePermissions.Action.Delete)]
        public async Task<IDictionary<string, object>> TL_USER_Del(string id)
        {
            return (await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.TL_USER_UPD, new
                {
                    USER_ID = id
                }));
        }

        public async Task<PagedResultDto<TL_USER_ENTITY>> TLUSER_MANAGER_SEARCH(TL_USER_ENTITY input)
        {
            var a = await storeProcedureProvider.GetPagingData<TL_USER_ENTITY>(CommonStoreProcedureConsts.TLUSER_MANAGER_SEARCH, input);
            return a;
        }

        public async Task<List<TL_USER_ENTITY>> TL_USER_By_DEPARTMENT()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<TL_USER_ENTITY>(CommonStoreProcedureConsts.TL_USER_By_DEPARTMENT, new
            {
                USER_LOGIN = GetCurrentUserName()
            });
        }
        #endregion

        #region Private Method

        #endregion


    }
}
