using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Goodss.Dto;
using Common.gAMSPro.Models;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using gAMSPro.Dto;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Goodss
{
    [AbpAuthorize]
    public class GoodsAppService : gAMSProCoreAppServiceBase, IGoodsAppService
    {
        private IRepository<CM_GOODS, string> goodsRepository;
        private readonly IRepository<CM_GOODSTYPE, string> goodsTypeRepository;
        private IRepository<CM_UNIT, string> unitRepository;
        private IRepository<CM_SUPPLIER, string> supplierRepository;
        private IRepository<CM_ALLCODE> allCodeRepository;
        private IRepository<CM_AUTH_STATUS, string> authStatusRepository;

        public GoodsAppService(IRepository<CM_GOODS, string> goodsRepository,
            IRepository<CM_GOODSTYPE, string> goodsTypeRepository,
            IRepository<CM_UNIT, string> unitRepository,
            IRepository<CM_SUPPLIER, string> supplierRepository,
            IRepository<CM_ALLCODE> allCodeRepository,
            IRepository<CM_AUTH_STATUS, string> authStatusRepository)
        {
            this.goodsRepository = goodsRepository;
            this.goodsTypeRepository = goodsTypeRepository;
            this.unitRepository = unitRepository;
            this.supplierRepository = supplierRepository;
            this.allCodeRepository = allCodeRepository;
            this.authStatusRepository = authStatusRepository;
        }

        #region Public Method

        public async Task<PagedResultDto<CM_GOODS_ENTITY>> CM_GOODS_Search(CM_GOODS_ENTITY input)
        {
            var result = await storeProcedureProvider.GetPagingData<CM_GOODS_ENTITY>(CommonStoreProcedureConsts.CM_GOODS_SEARCH, input);
            return result;
        }

        public async Task<FileDto> CM_GOODS_ToExcel(CM_GOODS_ENTITY input)
        {
            input.MaxResultCount = -1;
            var result = await CM_GOODS_Search(input);
            var list = result.Items.ToList();

            var no = 1;

            return CreateExcelPackage(
                $"BC_HANG_HOA_{Guid.NewGuid().ToString()}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("Goods"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("No"),
                        L("CodeId"),
                        L("GoodsName"),
                        L("GoodsTypeCode"),
                        L("Description"),
                        L("SupplierCode"),
                        L("AssPrice"),
                        L("Unit"),
                        L("Note"),
                        L("IsActive"),
                        L("AuthStatus")
                        );

                    AddObjects(
                        sheet, 2, list,
                        item => no++,
                        item => item.GD_CODE,
                        item => item.GD_NAME,
                        item => item.GD_TYPE_CODE,
                        item => item.DESCRIPTION,
                        item => item.SUP_CODE,
                        item => item.PRICE,
                        item => item.UNIT_CODE,
                        item => item.UNIT_NAME,
                        item => item.NOTES,
                        item => item.RECORD_STATUS_NAME,
                        item => item.AUTH_STATUS_NAME
                        );

                    for (var i = 1; i <= 9; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Goods, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_GOODS_Ins(CM_GOODS_ENTITY input)
        {
            var isApproveFunct = await IsApproveFunct(gAMSProCorePermissions.Page.Goods);
            input.AUTH_STATUS = !isApproveFunct ? ApproveStatusConsts.Approve : ApproveStatusConsts.NotApprove;
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_GOODS_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Goods, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_GOODS_Upd(CM_GOODS_ENTITY input)
        {
            var isApproveFunct = await IsApproveFunct(gAMSProCorePermissions.Page.Goods);
            input.AUTH_STATUS = !isApproveFunct ? ApproveStatusConsts.Approve : ApproveStatusConsts.NotApprove;
            input.MAKER_ID = GetCurrentUser().UserName;
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_GOODS_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_GOODS_ENTITY> CM_GOODS_ById(string id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_GOODS_ENTITY>(CommonStoreProcedureConsts.CM_GOODS_BYID, new
            {
                GD_ID = id
            })).FirstOrDefault();
            if (model == null)
            {
                return null;
            }
            var input = ObjectMapper.Map<CM_GOODS_ENTITY>(model);
            input.GD_ID = id;
            return input;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Goods, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_GOODS_App(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_GOODS_APP, new
                {
                    P_GD_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.Goods, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_GOODS_Del(string id)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_GOODS_DEL, new
                {
                    GD_ID = id
                })).FirstOrDefault();
        }

        #endregion

        #region Private Method

        #endregion


    }
}


/*
    viewState
    httpcontext
    Secsion

    Setup process
    filter .net query
 */