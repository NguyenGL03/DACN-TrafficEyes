using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.DeptGroups.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using gAMSPro.Dto;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.DeptGroups
{
    [AbpAuthorize]
    public class DeptGroupAppService : gAMSProCoreAppServiceBase, IDeptGroupAppService
    {

        #region Public Method
        public async Task<PagedResultDto<CM_DEPT_GROUP_ENTITY>> CM_DEPT_GROUP_Search(CM_DEPT_GROUP_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_DEPT_GROUP_ENTITY>(CommonStoreProcedureConsts.CM_DEPT_GROUP_SEARCH, input);
        }

        public async Task<FileDto> CM_DEPT_GROUP_ToExcel(CM_DEPT_GROUP_ENTITY input)
        {
            input.MaxResultCount = -1;
            var result = await CM_DEPT_GROUP_Search(input);
            var list = result.Items.ToList();
            return null;
            //return CreateExcelPackage(
            //    $"BC_LOAI_PHONG_BAN_{Guid.NewGuid().ToString()}.xlsx",
            //    excelPackage =>
            //    {
            //        var sheet = excelPackage.Workbook.Worksheets.Add(L("DeptGroup"));
            //        sheet.OutLineApplyStyle = true;
            //        var no = 1;

            //        AddHeader(
            //            sheet,
            //            L("No"),
            //            L("DeptGroupCode"),
            //            L("DeptGroupName"),
            //            L("Note"),
            //            L("IsActive"),
            //            L("ApproveStatus")
            //            );

            //        AddObjects(
            //            sheet, 2, list,
            //            item => no++,
            //            item => item.GROUP_CODE,
            //            item => item.GROUP_NAME,
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


        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.DeptGroup, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_DEPT_GROUP_Ins(CM_DEPT_GROUP_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_DEPT_GROUP_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.DeptGroup, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_DEPT_GROUP_Upd(CM_DEPT_GROUP_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_DEPT_GROUP_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_DEPT_GROUP_ENTITY> CM_DEPT_GROUP_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_DEPT_GROUP_ENTITY>(CommonStoreProcedureConsts.CM_DEPT_GROUP_BYID, new
            {
                GROUP_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.DeptGroup, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_DEPT_GROUP_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_DEPT_GROUP_APP, new
                {
                    P_GROUP_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.DeptGroup, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_DEPT_GROUP_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_DEPT_GROUP_DEL, new
                {
                    GROUP_ID = id
                })).FirstOrDefault();
        }

        #endregion

        #region Private Method

        #endregion


    }
}
