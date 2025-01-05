using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Employees.Dto;
using Common.gAMSPro.Intfs.Employees.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Employees
{
    [AbpAuthorize]
    public class EmployeeAppService : gAMSProCoreAppServiceBase, IEmployeeAppService
    {
        public EmployeeAppService()
        {
        }

        #region Public Method
        
        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_Search(CM_EMPLOYEE_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_EMPLOYEE_ENTITY>(CommonStoreProcedureConsts.CM_EMPLOYEE_SEARCH, input);
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Employee, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_EMPLOYEE_Ins(CM_EMPLOYEE_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Employee, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_EMPLOYEE_Upd(CM_EMPLOYEE_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_EMPLOYEE_ENTITY> CM_EMPLOYEE_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_EMPLOYEE_ENTITY>(CommonStoreProcedureConsts.CM_EMPLOYEE_BYID, new
            {
                EMP_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Employee, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_EMPLOYEE_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_APP, new
                {
                    P_EMP_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Employee, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_EMPLOYEE_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_EMPLOYEE_DEL, new
                {
                    EMP_ID = id
                })).FirstOrDefault();
        }

        public async Task<CM_EMPLOYEE_LOG_ENTITY> CM_EMPLOYEE_LOG_ByUserName(string username)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CM_EMPLOYEE_LOG_ENTITY>(CommonStoreProcedureConsts.CM_EMPLOYEE_LOG_ByUserName, new
                {
                    p_UserName = username
                })).FirstOrDefault(); ;
        } 

        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_Search_NotMapping(CM_EMPLOYEE_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_EMPLOYEE_ENTITY>("CM_EMPLOYEE_Search_NotMapping", input);
        }

        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_MODAL(CM_EMPLOYEE_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_EMPLOYEE_ENTITY>(input.STORED_NAME, input);
        }
        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_EXIST_IN_TLUSER_MODAL(CM_EMPLOYEE_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_EMPLOYEE_ENTITY>(CommonStoreProcedureConsts.CM_EMPLOYEE_EXIST_IN_TLUSER_MODAL, input);
        }

        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> ASS_TRANSFER_REPRESENT_SEARCH(CM_EMPLOYEE_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_EMPLOYEE_ENTITY>("ASS_TRANSFER_REPRESENT_SEARCH", input);
        }
        #endregion

        #region Private Method

        #endregion
    }
}
