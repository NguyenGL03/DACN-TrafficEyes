using Abp.Application.Services.Dto;
using Common.gAMSPro.Application.Intfs.DynamicPage.Dto;

namespace Common.gAMSPro.Application.Intfs.DynamicPage
{
    public interface IDynamicPageAppService
    {
        Task<PagedResultDto<DYNAMIC_PAGE_ENTITY>> DYNAMIC_PAGE_Search(DYNAMIC_PAGE_ENTITY input);
        Task<DYNAMIC_PAGE_ENTITY> DYNAMIC_PAGE_ById(string input);
        Task<IDictionary<string, object>> DYNAMIC_PAGE_Ins(DYNAMIC_PAGE_ENTITY input);
        Task<IDictionary<string, object>> DYNAMIC_PAGE_Upd(DYNAMIC_PAGE_ENTITY input);
        Task<IDictionary<string, object>> DYNAMIC_PAGE_Del(string input);
    }
}