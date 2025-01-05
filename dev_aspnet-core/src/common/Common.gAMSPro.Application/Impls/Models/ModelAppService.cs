using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Common.gAMSPro.Application.Intfs.Models.Dto;
using Common.gAMSPro.Application.Intfs.Models;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Models;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using gAMSPro.ProcedureHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.gAMSPro.Application.Impls.Models
{
    [AbpAuthorize]
    public class ModelAppService : gAMSProCoreAppServiceBase, IModelAppService
    {
        private IRepository<CM_MODEL, string> modelRepository;
        private IRepository<CM_ALLCODE> allCodeRepository;
        private IRepository<CM_AUTH_STATUS, string> authStatusRepository;
        private IRepository<CM_CAR_TYPE, string> carTypeRepository;

        public ModelAppService(IRepository<CM_MODEL, string> modelRepository,
            IRepository<CM_ALLCODE> allCodeRepository,
            IRepository<CM_CAR_TYPE, string> carTypeRepository,
            IRepository<CM_AUTH_STATUS, string> authStatusRepository)
        {
            this.modelRepository = modelRepository;
            this.allCodeRepository = allCodeRepository;
            this.authStatusRepository = authStatusRepository;
            this.carTypeRepository = carTypeRepository;
        }

        #region Public Method


        public async Task<PagedResultDto<CM_MODEL_ENTITY>> CM_MODEL_Search(CM_MODEL_ENTITY input)
        {
            try
            {
                return await storeProcedureProvider.GetPagingData<CM_MODEL_ENTITY>(CommonStoreProcedureConsts.CM_MODEL_SEARCH, input);

            }
            catch (Exception ex) { throw new Exception("", ex); }
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Model, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_MODEL_Ins(CM_MODEL_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_MODEL_INS, input)).FirstOrDefault();
            if (result == null)
            {
                return new InsertResult();
            }
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Model, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_MODEL_Upd(CM_MODEL_ENTITY input)
        {
            SetAuditForUpdate(input);
            var result= (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_MODEL_UPD, input)).FirstOrDefault();
            if(result == null)
            {
                return new InsertResult();
            }
            return result;
        }

        public async Task<CM_MODEL_ENTITY> CM_MODEL_ById(string id)
        {
            var result= (await storeProcedureProvider.GetDataFromStoredProcedure<CM_MODEL_ENTITY>(CommonStoreProcedureConsts.CM_MODEL_BYID, new
            {
                MO_ID = id
            })).FirstOrDefault();
            if (result == null)
            {
                return new CM_MODEL_ENTITY ();
            }
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Model, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_MODEL_App(string id, string currentUserName)
        {
            var result= (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_MODEL_APP, new
                {
                    P_MO_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
            if (result == null) { return new CommonResult(); }
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Model, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_MODEL_Del(string id)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_MODEL_DEL, new
                {
                    MO_ID = id
                })).FirstOrDefault();
            if(result == null)
            {
                return new CommonResult();
            }
            return result;
        }

        #endregion

        #region Private Method

        #endregion


    }
}
