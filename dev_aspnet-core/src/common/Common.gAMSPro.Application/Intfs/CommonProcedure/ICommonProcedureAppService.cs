using Abp.Application.Services;
using Abp.Application.Services.Dto;

namespace Common.gAMSPro.CommonProcedure
{
    public interface ICommonProcedureAppService : IApplicationService
    {
        Task<List<object>> GetDataFromStoredProcedure(string stored, object input);

        Task<PagedResultDto<object>> GetPagingData(string stored, object input);
    }
}
