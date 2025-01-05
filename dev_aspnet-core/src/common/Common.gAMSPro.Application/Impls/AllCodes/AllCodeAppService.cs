using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.AllCodes.Dto;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.AllCodes
{
    [AbpAuthorize]
    public class AllCodeAppService : gAMSProCoreAppServiceBase, IAllCodeAppService
    {

        public AllCodeAppService()
        {
        }

        public async Task<List<CM_ALLCODE_ENTITY>> CM_ALLCODE_GetByCDNAME(string cdName, string? cdType)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_ALLCODE_ENTITY>(CommonStoreProcedureConsts.CM_ALLCODE_GETBYCDNAME,
                new
                {
                    CD_NAME = cdName,
                    CDTYPE = cdType
                });
            return result;
        }

        public async Task<PagedResultDto<CM_ALLCODE_ENTITY>> CM_ALLCODE_Search(CM_ALLCODE_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_ALLCODE_ENTITY>(CommonStoreProcedureConsts.CM_ALLCODE_SEARCH, input);
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.AllCode, gAMSProCorePermissions.Action.Create)]
        public async Task<InsertResult> CM_ALLCODE_Ins(CM_ALLCODE_ENTITY input)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_ALLCODE_INS, input)).FirstOrDefault();
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.AllCode, gAMSProCorePermissions.Action.Update)]
        public async Task<InsertResult> CM_ALLCODE_Upd(CM_ALLCODE_ENTITY input)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<InsertResult>(CommonStoreProcedureConsts.CM_ALLCODE_UPD, input)).FirstOrDefault();
        }

        public async Task<CM_ALLCODE_ENTITY> CM_ALLCODE_ById(string cdName, string? cdType)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_ALLCODE_ENTITY>(CommonStoreProcedureConsts.CM_ALLCODE_BYID, new
            {
                CDNAME = cdName,
                CDTYPE = cdType
            })).FirstOrDefault();
            return model;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.AllCode, gAMSProCorePermissions.Action.Delete)]
        public async Task<CommonResult> CM_ALLCODE_Del(int id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.CM_ALLCODE_DEL, new
                {
                    ALL_CODE_ID = id
                })).FirstOrDefault();
            return model;
        }

        //BAODNQ 1/7/2022
        public async Task<CM_ALLCODE_ENTITY> CM_ALLCODE_ById_v2(string cdType, string cdName, string cdVal)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_ALLCODE_ENTITY>(CommonStoreProcedureConsts.CM_ALLCODE_BYID_V2, new
            {
                P_CDNAME = cdName,
                P_CDTYPE = cdType,
                P_CDVAL = cdVal
            })).FirstOrDefault();
            return result;
        }

    }
}
