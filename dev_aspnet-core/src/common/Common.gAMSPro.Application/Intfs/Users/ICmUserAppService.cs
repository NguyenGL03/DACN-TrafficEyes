using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.Users.Dto;

namespace Common.gAMSPro.Intfs.Users
{
    public interface ICmUserAppService : IApplicationService
    {
        Task<PagedResultDto<TLUSER_GETBY_BRANCHID_ENTITY>> TLUSER_GETBY_BRANCHID(TLUSER_GETBY_BRANCHID_ENTITY tlUserFilter);
    }
}
