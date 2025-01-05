using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Branchs.Dto;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro;
using gAMSPro.Consts;
using gAMSPro.Dto;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Branchs
{
    [AbpAuthorize]
    public class BranchAppService : gAMSProCoreAppServiceBase, IBranchAppService
    {
        #region Public Method

        public async Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_Search(CM_BRANCH_ENTITY input)
        {
            if (input.IsLoadAll != true)
            {
                input.BRANCH_LOGIN = GetCurrentBranchId();
            }
            return await storeProcedureProvider.GetPagingData<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_SEARCH, input);
        }

        public async Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_GET_ALL(CM_BRANCH_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_GET_ALL, input);
        }
        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Branch, gAMSProCorePermissions.Action.Search)]


        public async Task<FileDto> CM_BRANCH_ToExcel(CM_BRANCH_ENTITY input)
        {
            input.MaxResultCount = -1;
            var result = await CM_BRANCH_Search(input);
            var list = result.Items.ToList();

            return CreateExcelPackage(
                $"BC_DONVI_{Guid.NewGuid().ToString()}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Users"));
                    sheet.OutLineApplyStyle = true;
                    var no = 1;

                    AddHeader(
                        sheet,
                        L("No"),
                        L("BranchCode"),
                        L("BranchName"),
                        L("Address"),
                        L("Tel"),
                        L("IsActive"),
                        L("AuthStatus")
                        );

                    AddObjects(
                        sheet, 2, list,
                        item => no++,
                        item => item.BRANCH_CODE,
                        item => item.BRANCH_NAME,
                        item => item.ADDR,
                        item => item.TEL,
                        item => item.RECORD_STATUS_NAME,
                        item => item.AUTH_STATUS_NAME
                        );

                    for (var i = 1; i <= 9; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }

        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_Combobox()
        {
            var user = GetCurrentUser();
            string branchId = user.SubbrId;
            var result = await storeProcedureProvider
                .GetDataFromStoredProcedure<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_COMBOBOX, new
                {
                    BRANCH_ID = branchId
                });

            return result;
        }

        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_USER_Combobox()
        {
            var user = GetCurrentUser();
            string UserName = user.UserName;
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_USER_Combobox, new { USER_LOGIN = UserName });

            return result;
        }

        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_USER_Combobox_2()
        {
            var user = GetCurrentUser();
            string UserName = user.UserName;
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_BRANCH_ENTITY>("CM_BRANCH_USER_Combobox_2", new { USER_LOGIN = UserName });

            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Branch, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_BRANCH_Ins(CM_BRANCH_ENTITY input)
        {
            SetAuditForInsert(input);
            return (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_BRANCH_INS, input)).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Branch, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_BRANCH_Upd(CM_BRANCH_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_BRANCH_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_BRANCH_ENTITY> CM_BRANCH_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_BYID, new { BRANCH_ID = id })).FirstOrDefault();

            return model;
        }

        public async Task<CM_BRANCH_ENTITY> CM_BRANCH_ByCode(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_ByCode, new
            {
                BRANCH_CODE = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Branch, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_BRANCH_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_BRANCH_APP, new
            {
                P_BRANCH_ID = id,
                P_AUTH_STATUS = ApproveStatusConsts.Approve,
                P_CHECKER_ID = currentUserName,
                P_APPROVE_DT = GetCurrentDateTime()
            })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Branch, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_BRANCH_Del(string id)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_BRANCH_DEL, new
            {
                BRANCH_ID = id
            })).FirstOrDefault();
        }

        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_GetFatherList(string regionId, string branchType)
        {
            string branchId = GetCurrentBranchId();
            var result = await storeProcedureProvider
                .GetDataFromStoredProcedure<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_GETFATHERLIST, new
                {
                    BRANCH_ID = branchId,
                    REGION_ID = regionId,
                    BRANCH_TYPE = branchType
                });

            return result;
        }

        public async Task<CM_BRANCH_LEV_ENTITY> CM_BRANCH_Lev(string branchId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_GetAllChild(string parentId)
        {
            if (parentId == null)
            {
                parentId = GetCurrentBranchId();
            }
            return await storeProcedureProvider
                .GetDataFromStoredProcedure<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_GETALLCHILD, new
                {
                    PARENT_ID = parentId
                });
        }



        #endregion

        #region Private Method

        private string GetCurrentBranchId()
        {
            return GetClaimValue(AppConsts.USER_TLSUBBRID);
        }

        public async Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_Dep_Search(CM_BRANCH_ENTITY input)
        {
            if (input.IsLoadAll != true)
            {
                input.BRANCH_LOGIN = GetCurrentBranchId();
            }
           
            return await storeProcedureProvider.GetPagingData<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_DEP_SEARCH, input);
        }

        public async Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_Dep_Search_v2(CM_BRANCH_ENTITY input)
        {
            if (input.IsLoadAll != true)
            {
                input.BRANCH_LOGIN = GetCurrentBranchId();
            }
            return await storeProcedureProvider.GetPagingData<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_DEP_SEARCH_v2, input);
        }

        public async Task<CM_BRANCH_ENTITY> CM_BRANCH_Dep_ById(string branchId, string depId)
        {

            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_BRANCH_ENTITY>(CommonStoreProcedureConsts.CM_BRANCH_DEP_ById, new
            {
                p_BRANCH_ID = branchId,
                p_DEP_ID = depId
            })).FirstOrDefault();

            return model;

        }

        #endregion
    }
}
