using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using gAMSPro.Authorization.Users.Dto;

namespace gAMSPro.Authorization.Users
{
    public interface IUserLoginAppService : IApplicationService
    {
        Task<PagedResultDto<UserLoginAttemptDto>> GetUserLoginAttempts(GetLoginAttemptsInput input);
    }
}
