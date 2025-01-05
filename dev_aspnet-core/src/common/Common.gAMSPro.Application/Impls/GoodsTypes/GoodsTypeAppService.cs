using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.GoodsTypes.Dto;
using Common.gAMSPro.Models;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.GoodsTypes
{
    [AbpAuthorize]
    public class GoodsTypeAppService : gAMSProCoreAppServiceBase, IGoodsTypeAppService
    {
        private IRepository<CM_GOODSTYPE, string> goodsTypeRepository;
        private IRepository<CM_ALLCODE> allCodeRepository;
        private IRepository<CM_AUTH_STATUS, string> authStatusRepository;

        public GoodsTypeAppService(IRepository<CM_GOODSTYPE, string> goodsTypeRepository,
            IRepository<CM_ALLCODE> allCodeRepository,
            IRepository<CM_AUTH_STATUS, string> authStatusRepository)
        {
            this.goodsTypeRepository = goodsTypeRepository;
            this.allCodeRepository = allCodeRepository;
            this.authStatusRepository = authStatusRepository;
        }

        #region Public Method

        public async Task<PagedResultDto<CM_GOODSTYPE_ENTITY>> CM_GOODSTYPE_Search(CM_GOODSTYPE_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_GOODSTYPE_ENTITY>(CommonStoreProcedureConsts.CM_GOODSTYPE_SEARCH, input);
        }
        

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.GoodsType, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_GOODSTYPE_Ins(CM_GOODSTYPE_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_GOODSTYPE_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.GoodsType, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_GOODSTYPE_Upd(CM_GOODSTYPE_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_GOODSTYPE_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_GOODSTYPE_ENTITY> CM_GOODSTYPE_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_GOODSTYPE_ENTITY>(CommonStoreProcedureConsts.CM_GOODSTYPE_BYID, new
            {
                GD_TYPE_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.GoodsType, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_GOODSTYPE_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_GOODSTYPE_APP, new
                {
                    P_GD_TYPE_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.GoodsType, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_GOODSTYPE_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_GOODSTYPE_DEL, new
                {
                    GD_TYPE_ID = id
                })).FirstOrDefault();
        }

        public async Task<List<CM_GOODSTYPE_ENTITY>> CM_GOODSTYPE_List()
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CM_GOODSTYPE_ENTITY>(CommonStoreProcedureConsts.CM_GOODSTYPE_LIST,new { }));
        }

        #endregion

        #region Private Method

        #endregion


    }
}
