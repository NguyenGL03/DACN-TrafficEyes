using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.SysParameters.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.SysParameters
{
    [AbpAuthorize]
    public class SysParametersAppService : gAMSProCoreAppServiceBase, ISysParametersAppService
    {
        public SysParametersAppService()
        {
        }

        public async Task<PagedResultDto<SYS_PARAMETERS_ENTITY>> SYS_PARAMETERS_Search(SYS_PARAMETERS_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<SYS_PARAMETERS_ENTITY>(CommonStoreProcedureConsts.SYS_PARAMETERS_SEARCH, input);
        }

        public async Task<FileDto> SYS_PARAMETERS_ToExcel(SYS_PARAMETERS_ENTITY input)
        {
            input.MaxResultCount = -1;
            var result = await SYS_PARAMETERS_Search(input);
            var list = result.Items.ToList();
            return null;
            //return CreateExcelPackage(
            //    $"BC_THAM_SO_HE_THONG_{Guid.NewGuid().ToString()}.xlsx",
            //    excelPackage =>
            //    {
            //        var sheet = excelPackage.Workbook.Worksheets.Add(L("SysParameters"));
            //        sheet.OutLineApplyStyle = true;
            //        var no = 1;

            //        AddHeader(
            //            sheet,
            //            L("No"),
            //            L("ParaKey"),
            //            L("ParaValue"),
            //            L("DataType"),
            //            L("Description"),
            //            L("IsActive")
            //            );

            //        AddObjects(
            //            sheet, 2, list,
            //            item => no++,
            //            item => item.ParaKey,
            //            item => item.ParaValue,
            //            item => item.DataType,
            //            item => item.Description,
            //            item => item.RECORD_STATUS_NAME
            //            );

            //        for (var i = 1; i <= 9; i++)
            //        {
            //            sheet.Column(i).AutoFit();
            //        }
            //    });
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysParameters, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> SYS_PARAMETERS_Ins(SYS_PARAMETERS_ENTITY input)
        {
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.SYS_PARAMETERS_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysParameters, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> SYS_PARAMETERS_Upd(SYS_PARAMETERS_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.SYS_PARAMETERS_UPD, input)).FirstOrDefault();
        }

        public async Task<SYS_PARAMETERS_ENTITY> SYS_PARAMETERS_ById(decimal id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<SYS_PARAMETERS_ENTITY>(CommonStoreProcedureConsts.SYS_PARAMETERS_BYID, new
            {
                SYS_PARA_ID = id
            })).FirstOrDefault();

            return model;
        }

        public async Task<SYS_PARAMETERS_ENTITY> SYS_PARAMETERS_ByParaKey(string parakey)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<SYS_PARAMETERS_ENTITY>(CommonStoreProcedureConsts.SYS_PARAMETERS_BYPARAKEY, new
            {
                PARAKEY = parakey
            })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.SysParameters, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> SYS_PARAMETERS_Del(decimal id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.SYS_PARAMETERS_DEL, new
                {
                    SYS_PARA_ID = id
                })).FirstOrDefault();
        }
    }
}
