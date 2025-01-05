using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.Users;
using Common.gAMSPro.Intfs.Users.Dto;
using Core.gAMSPro.Application;

namespace Common.gAMSPro.Impls.Users
{
    [AbpAuthorize]
    public class CmUserAppService : gAMSProCoreAppServiceBase, ICmUserAppService
    {
        public async Task<PagedResultDto<TLUSER_GETBY_BRANCHID_ENTITY>> TLUSER_GETBY_BRANCHID(TLUSER_GETBY_BRANCHID_ENTITY tlUserFilter)
        {
            return await storeProcedureProvider
    .GetPagingData<TLUSER_GETBY_BRANCHID_ENTITY>(CommonStoreProcedureConsts.TLUSER_GETBY_BRANCHID, tlUserFilter);
        }
    }
}
