using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Application.Intfs.Suppliers;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Suppliers.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using gAMSPro.Dto;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Suppliers
{
    [AbpAuthorize]
    public class SupplierAppService : gAMSProCoreAppServiceBase, ISupplierAppService
    {

        public async Task<PagedResultDto<CM_SUPPLIER_ENTITY>> CM_SUPPLIER_Search(CM_SUPPLIER_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_SUPPLIER_ENTITY>(CommonStoreProcedureConsts.CM_SUPPLIER_SEARCH, input);
        }

        public async Task<FileDto> CM_SUPPLIER_ToExcel(CM_SUPPLIER_ENTITY input)
        {
            throw new NotImplementedException();
            //input.MaxResultCount = -1;
            //var result = await CM_SUPPLIER_Search(input);
            //var list = result.Items.ToList();

            //return CreateExcelPackage(
            //    $"BC_NHA_CUNG_CAP_{Guid.NewGuid().ToString()}.xlsx",
            //    excelPackage =>
            //    {
            //        var sheet = excelPackage.Workbook.Worksheets.Add(L("Supplier"));
            //        sheet.OutLineApplyStyle = true;
            //        var no = 1;

            //        AddHeader(
            //            sheet,
            //            L("No"),
            //            L("SupplierCode"),
            //            L("SupplierName"),
            //            L("SupplierTypeCode"),
            //            L("RegionId"),
            //            L("Address"),
            //            L("Email"),
            //            L("TaxNo"),
            //            L("Tel"),
            //            L("ContactPerson"),
            //            L("Note"),
            //            L("IsActive"),
            //            L("AuthStatus")
            //            );

            //        AddObjects(
            //            sheet, 2, list,
            //            item => no++,
            //            item => item.SUP_CODE,
            //            item => item.SUP_NAME,
            //            item => item.SUP_TYPE_NAME,
            //            item => item.REGION_ID,
            //            item => item.ADDR,
            //            item => item.EMAIL,
            //            item => item.TAX_NO,
            //            item => item.TEL,
            //            item => item.CONTACT_PERSON,
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


        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Supplier, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_SUPPLIER_Ins(CM_SUPPLIER_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SUPPLIER_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Supplier, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_SUPPLIER_Upd(CM_SUPPLIER_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_SUPPLIER_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_SUPPLIER_ENTITY> CM_SUPPLIER_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_SUPPLIER_ENTITY>(CommonStoreProcedureConsts.CM_SUPPLIER_BYID, new
            {
                SUP_ID = id
            })).FirstOrDefault();

            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Supplier, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_SUPPLIER_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_SUPPLIER_APP, new
                {
                    P_SUP_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Supplier, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_SUPPLIER_Del(string id, string userLogin)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_SUPPLIER_DEL, new
                {
                    SUP_ID = id,
                    USER_LOGIN = userLogin
                })).FirstOrDefault();
        }
    }
}
