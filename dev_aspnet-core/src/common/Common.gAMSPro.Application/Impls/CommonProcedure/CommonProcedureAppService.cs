using Abp.Application.Services.Dto;
using Abp.Authorization;
using Core.gAMSPro.Application;

namespace Common.gAMSPro.CommonProcedure
{
    [AbpAuthorize]
    public class CommonProcedureAppService : gAMSProCoreAppServiceBase, ICommonProcedureAppService
    {
        public Task<List<object>> GetDataFromStoredProcedure(string stored, object input)
        {
            return storeProcedureProvider.GetDataFromStoredProcedureByJSON<object>(stored, input);
        }

        public Task<PagedResultDto<object>> GetPagingData(string stored, object input)
        {
            return storeProcedureProvider.GetPagingDataByJSON<object>(stored, input);
        }
    }
}
