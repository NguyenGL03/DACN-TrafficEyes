using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Runtime.Validation;
using Common.gAMSPro.Application.Intfs.CarTypes;
using Common.gAMSPro.Application.Intfs.CarTypes.Dto;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Models;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Application.Impls.CarTypes
{
    [AbpAuthorize]
    public class CarTypeAppService : gAMSProCoreAppServiceBase, ICarTypeAppService
    {
        private IRepository<CM_CAR_TYPE, string> carTypeRepository;

        public CarTypeAppService(IRepository<CM_CAR_TYPE, string> carTypeRepository)
        {
            this.carTypeRepository = carTypeRepository;
        }

        #region Public Method

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarType, gAMSProCorePermissions.Action.Search)]
        public async Task<PagedResultDto<CM_CAR_TYPE_ENTITY>> CM_CAR_TYPE_Search([FromForm] CM_CAR_TYPE_ENTITY input)
        {
            var query = carTypeRepository.GetAll()
                .Where(x => input.CAR_TYPE_ID.IsNullOrWhiteSpace() || x.Id.Equals(input.CAR_TYPE_ID))
                .Where(x => input.CAR_TYPE_NAME.IsNullOrWhiteSpace() || x.CAR_TYPE_NAME.Trim().ToLower().Contains(input.CAR_TYPE_NAME.Trim().ToLower()))
                .Where(x => input.RECORD_STATUS.IsNullOrWhiteSpace() || x.RECORD_STATUS.Equals(input.RECORD_STATUS))
                .Where(x => input.AUTH_STATUS.IsNullOrWhiteSpace() || x.AUTH_STATUS.Equals(input.AUTH_STATUS));

            var totalRows = await query.CountAsync();


            if (!input.Sorting.IsNullOrWhiteSpace())
            {
                query = query.OrderBy(input.Sorting);
            }

            if (input.MaxResultCount == -1)
            {
                input.SkipCount = 0;
                input.MaxResultCount = totalRows;
            }

            if (input.MaxResultCount > 0)
            {
                query = query.PageBy(input);
            }

            var items = query.ToList()
                .Select(x => ObjectMapper.Map<CM_CAR_TYPE_ENTITY>(x))
                .ToList();

            return new PagedResultDto<CM_CAR_TYPE_ENTITY>(totalRows, items);
        }

        public async Task<List<CM_CAR_TYPE_ENTITY>> CM_CAR_TYPE_List(CM_CAR_TYPE_ENTITY input)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_CAR_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_CAR_TYPE_LIST, input);
        }

        public async Task<FileDto> CM_CAR_TYPE_ToExcel(CM_CAR_TYPE_ENTITY input)
        {
            input.MaxResultCount = -1;
            var result = await CM_CAR_TYPE_Search(input);
            var list = result.Items.ToList();

            return CreateExcelPackage(
                $"BC_LOAI_XE_{Guid.NewGuid().ToString()}.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.Workbook.Worksheets.Add(L("CarType"));
                    sheet.OutLineApplyStyle = true;

                    AddHeader(
                        sheet,
                        L("CarTypeCode"),
                        L("CarTypeName"),
                        L("Note"),
                        L("IsActive"),
                        L("AuthStatus")
                        );

                    AddObjects(
                        sheet, 2, list,
                        item => item.CAR_TYPE_CODE,
                        item => item.CAR_TYPE_NAME,
                        item => item.NOTES,
                        item => item.RECORD_STATUS,
                    item => item.AUTH_STATUS
                        );

                    for (var i = 1; i <= 9; i++)
                    {
                        sheet.Column(i).AutoFit();
                    }
                });
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarType, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_CAR_TYPE_Ins(CM_CAR_TYPE_ENTITY input)
        {
            // sua
            SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CAR_TYPE_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarType, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_CAR_TYPE_Upd(CM_CAR_TYPE_ENTITY input)
        {
            // sua
            SetAuditForUpdate(input);
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_CAR_TYPE_UPD, input)).FirstOrDefault();
            if (result == null)
            {
                return new InsertResult();
            }
            return result;
        }

        public async Task<CM_CAR_TYPE_ENTITY> CM_CAR_TYPE_ById(string id)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_CAR_TYPE_ENTITY>(CommonStoreProcedureConsts.CM_CAR_TYPE_BYID, new
            {
                CAR_TYPE_ID = id
            })).FirstOrDefault();
            if (result == null)
            {
                return new CM_CAR_TYPE_ENTITY();
            }
            return result;
        }

        // sua
        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarType, gAMSProCorePermissions.Action.Approve)]
        public async Task<CommonResult> CM_CAR_TYPE_App(string id, string currentUserName)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_CAR_TYPE_APP, new
                {
                    P_CAR_TYPE_ID = id,
                    P_AUTH_STATUS = ApproveStatusConsts.Approve,
                    P_CHECKER_ID = currentUserName,
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();
            if (result == null)
            {
                return new CommonResult();
            }
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarType, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_CAR_TYPE_Del(string id)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_CAR_TYPE_DEL, new
                {
                    CAR_TYPE_ID = id
                })).FirstOrDefault();
            if (result == null) { return new CommonResult(); }
            return result;
        }



        #endregion

        #region Private Method

        #endregion


    }
}
