using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.Terms;
using Common.gAMSPro.Intfs.Terms.Dto;
using Core.gAMSPro.Application;

namespace Common.gAMSPro.Impls.Terms
{
    [AbpAuthorize]
    public class TermAppService : gAMSProCoreAppServiceBase, ITermAppService
    {
        public async Task<PagedResultDto<CM_TERM_ENTITY>> CM_TERM_Search(CM_TERM_ENTITY input)
        {
            var items = await storeProcedureProvider.GetDataFromStoredProcedure<CM_TERM_ENTITY>(CommonStoreProcedureConsts.CM_TERM_SEARCH, input);

            return new PagedResultDto<CM_TERM_ENTITY>()
            {
                Items = items,
                TotalCount = items.FirstOrDefault()?.TotalCount ?? 0
            };
        }
    }
}
