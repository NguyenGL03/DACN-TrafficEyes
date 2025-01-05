using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.TlUsers.Dto;

namespace Common.gAMSPro.TlUsers
{
    public interface ITlUserAppService : IApplicationService
    {
        Task<PagedResultDto<TL_USER_ENTITY>> TL_USER_Search(TL_USER_ENTITY input);
        Task<List<TL_USER_ENTITY>> TL_USER_GET_List(TL_USER_ENTITY input);
        Task<PagedResultDto<TL_USER_ENTITY>> TL_USER_GET_List_v2(TL_USER_ENTITY input);
        Task<PagedResultDto<TL_USER_ENTITY>> TLUSER_MANAGER_SEARCH(TL_USER_ENTITY input);
        Task<IDictionary<string, object>> TL_USER_Ins(TL_USER_ENTITY input);
        Task<IDictionary<string, object>> TL_USER_Upd(TL_USER_ENTITY input);
        Task<IDictionary<string, object>> TL_USER_Del(string id);
        Task<IDictionary<string, object>> TL_USER_App(string id, string currentUserName);
        Task<IDictionary<string, object>> MD_USER_MANAGER_Upd_AUTHORITYUSER(TL_USER_ENTITY input);
        Task<IDictionary<string, object>> MD_USER_MANAGER_Del_AUTHORITYUSER(TL_USER_ENTITY input);
        Task<TL_USER_ENTITY> TL_USER_ById(string id);
        Task<List<TL_USER_ENTITY>> TL_USER_By_DEPARTMENT();
    }
}
