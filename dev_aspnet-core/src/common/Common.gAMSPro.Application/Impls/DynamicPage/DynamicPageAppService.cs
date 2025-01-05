using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Application.Intfs.DynamicPage;
using Common.gAMSPro.Application.Intfs.DynamicPage.Dto;
using Common.gAMSPro.Consts;
using Core.gAMSPro.Application;
using Newtonsoft.Json;

namespace Common.gAMSPro.Application.Impls.DynamicPage
{
    [AbpAuthorize]
    public class DynamicPageAppService : gAMSProCoreAppServiceBase, IDynamicPageAppService
    {
        public async Task<DYNAMIC_PAGE_ENTITY> DYNAMIC_PAGE_ById(string input)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_PAGE_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_PAGE_ById, new { PAGE_ID = input })).FirstOrDefault();

            if (result != null)
            {
                result.inputs = await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_PAGE_INPUT_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_PAGE_INPUT_Search, new { PAGE_ID = input });
            }

            return result;
        }

        public async Task<IDictionary<string, object>> DYNAMIC_PAGE_Del(string input)
        {
            return await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.DYNAMIC_PAGE_Del, new { PAGE_ID = input });
        }

        public async Task<IDictionary<string, object>> DYNAMIC_PAGE_Ins(DYNAMIC_PAGE_ENTITY input)
        {
            input.inputsJson = JsonConvert.SerializeObject(input.inputs);
            return await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.DYNAMIC_PAGE_Ins, input);
        }

        public async Task<PagedResultDto<DYNAMIC_PAGE_ENTITY>> DYNAMIC_PAGE_Search(DYNAMIC_PAGE_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<DYNAMIC_PAGE_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_PAGE_Search, input);
        }

        public async Task<IDictionary<string, object>> DYNAMIC_PAGE_Upd(DYNAMIC_PAGE_ENTITY input)
        {
            input.inputsJson = JsonConvert.SerializeObject(input.inputs);
            return await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.DYNAMIC_PAGE_Upd, input);
        }
    }
}