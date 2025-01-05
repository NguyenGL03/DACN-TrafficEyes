using Abp.Application.Services;
using Common.gAMSPro.Intfs.UserManager.Dto;

namespace Common.gAMSPro.Intfs.UserManager
{
    public interface IUserManagerAppService : IApplicationService
    {
        Task<IDictionary<string, object>> MD_USER_MANAGER_Ins(MD_USER_MANAGER_ENTITY input);
        Task<List<MD_USER_MANAGER_ENTITY>> MD_USER_MANAGER_GetByTLName(string tlname); 
    }
}
