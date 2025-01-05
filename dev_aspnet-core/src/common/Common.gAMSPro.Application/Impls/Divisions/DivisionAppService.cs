using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Divisions.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using gAMSPro.Dto;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Divisions
{
    [AbpAuthorize]
    public class DivisionAppService : gAMSProCoreAppServiceBase, IDivisionAppService
    {
        #region Public Method

        public async Task<PagedResultDto<CM_DIVISION_ENTITY>> CM_DIVISION_Search(CM_DIVISION_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_DIVISION_ENTITY>(CommonStoreProcedureConsts.CM_DIVISION_SEARCH, input);
        }

        public async Task<FileDto> CM_DIVISION_ToExcel(CM_DIVISION_ENTITY input)
        {
            input.MaxResultCount = -1;
            var result = await CM_DIVISION_Search(input);
            var list = result.Items.ToList();

            var no = 1;
            return null;
            //return CreateExcelPackage(
            //    $"BC_DIA_DIEM_{Guid.NewGuid().ToString()}.xlsx",
            //    excelPackage =>
            //    {
            //        var sheet = excelPackage.Workbook.Worksheets.Add(L("Division"));
            //        sheet.OutLineApplyStyle = true;

            //        AddHeader(
            //            sheet,
            //            L("No"),
            //            L("DivisionCode"),
            //            L("DivisionName"),
            //            L("BranchCode"),
            //            L("Address"),
            //            L("Note"),
            //            L("IsActive"),
            //            L("AuthStatus")
            //            );

            //        AddObjects(
            //            sheet, 2, list,
            //            item => no++,
            //            item => item.DIV_CODE,
            //            item => item.DIV_NAME,
            //            item => item.BRANCH_CODE,
            //            item => item.ADDR,
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

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Division, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_DIVISION_Ins(CM_DIVISION_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_DIVISION_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Division, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_DIVISION_Upd(CM_DIVISION_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_DIVISION_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_DIVISION_ENTITY> CM_DIVISION_ById(string id)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<CM_DIVISION_ENTITY>(CommonStoreProcedureConsts.CM_DIVISION_BYID, new
            {
                DIV_ID = id
            })).FirstOrDefault();
        }

        public async Task<List<CM_DIVISION_ENTITY>> CM_DIVISION_GETALLCHILD(string parentId)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<CM_DIVISION_ENTITY>(CommonStoreProcedureConsts.CM_DIVISION_GETALLCHILD, new
            {
                PARENT_ID = parentId
            }));
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Division, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_DIVISION_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_DIVISION_APP, new
                {
                    P_DIV_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Division, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_DIVISION_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_DIVISION_DEL, new
                {
                    DIV_ID = id
                })).FirstOrDefault();
        }

        #endregion

        #region Private Method

        #endregion

    }
}
