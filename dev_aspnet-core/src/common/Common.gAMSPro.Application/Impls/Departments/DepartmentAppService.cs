using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Departments.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using gAMSPro.Dto;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Departments
{
    [AbpAuthorize]
    public class DepartmentAppService : gAMSProCoreAppServiceBase, IDepartmentAppService
    {
        public DepartmentAppService()
        {
        }

        #region Public Method
        public async Task<PagedResultDto<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_COSTCENTER_Search(CM_DEPARTMENT_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_DEPARTMENT_ENTITY>(CommonStoreProcedureConsts.CM_DEPARTMENT_COSTCENTER_Search, input);
        }

        public async Task<PagedResultDto<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_Search(CM_DEPARTMENT_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_DEPARTMENT_ENTITY>(CommonStoreProcedureConsts.CM_DEPARTMENT_SEARCH, input);
        }

        public async Task<List<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_HS_List()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_DEPARTMENT_ENTITY>(CommonStoreProcedureConsts.CM_DEPARTMENT_HS_List, new
            {
                USER_LOGIN = GetCurrentUserName()
            });
        }

        public async Task<FileDto> CM_DEPARTMENT_ToExcel(CM_DEPARTMENT_ENTITY input)
        {
            input.MaxResultCount = -1;
            var result = await CM_DEPARTMENT_Search(input);
            var list = result.Items.ToList();
            return null;
            //return CreateExcelPackage(
            //    $"BC_PHONG_BAN_{Guid.NewGuid().ToString()}.xlsx",
            //    excelPackage =>
            //    {
            //        var sheet = excelPackage.Workbook.Worksheets.Add(L("Department"));
            //        sheet.OutLineApplyStyle = true;
            //        var no = 1;

            //        AddHeader(
            //            sheet,
            //            L("No"),
            //            L("DepartmentCode"),
            //            L("DepartmentName"),
            //            L("BranchCode"),
            //            L("FatherID"),
            //            L("FatherName"),
            //            L("PhoneNumber"),
            //            L("Note"),
            //            L("IsActive"),
            //            L("ApproveStatus")
            //            );

            //        AddObjects(
            //            sheet, 2, list,
            //            item => no++,
            //            item => item.DEP_CODE,
            //            item => item.DEP_NAME,
            //            item => item.BRANCH_CODE,
            //            item => item.FATHER_ID,
            //            item => item.FATHER_NAME,
            //            item => item.TEL,
            //            item => item.NOTES,
            //            item => item.RECORD_STATUS_NAME,
            //            item => item.AUTH_STATUS_NAME
            //            );

            //        for (var i = 1; i <= 9; i++)
            //        {
            //            sheet.Column(i).AutoFit();
            //        }
            //    });
        }


        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Department, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_DEPARTMENT_Ins(CM_DEPARTMENT_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_DEPARTMENT_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Department, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_DEPARTMENT_Upd(CM_DEPARTMENT_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_DEPARTMENT_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_DEPARTMENT_ENTITY> CM_DEPARTMENT_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_DEPARTMENT_ENTITY>(CommonStoreProcedureConsts.CM_DEPARTMENT_BYID, new
            {
                DEP_ID = id
            })).FirstOrDefault();

            return model;
        }

        public async Task<CM_DEPARTMENT_ENTITY> CM_DEPARTMENT_ByCode(string branch_id, string dep_id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_DEPARTMENT_ENTITY>(CommonStoreProcedureConsts.CM_DEPARTMENT_BYCODE, new
            {
                BRANCH_ID = branch_id,
                DEP_CODE = dep_id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Department, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_DEPARTMENT_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_DEPARTMENT_APP, new
                {
                    P_DEP_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Department, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_DEPARTMENT_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_DEPARTMENT_DEL, new
                {
                    DEP_ID = id
                })).FirstOrDefault();
        }

        #endregion

        #region Private Method

        #endregion


    }
}
