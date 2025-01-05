using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.AppMenus.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.AppMenus
{
    public interface IAppMenuAppService : IApplicationService
    {
        Task<PagedResultDto<TL_MENU_ENTITY>> TL_MENU_Search(TL_MENU_ENTITY input);
        Task<PagedResultDto<TL_MENU_ENTITY>> TL_MENU_Search_By_RoleID(TL_MENU_ENTITY input);
        Task<List<TL_MENU_ENTITY>> TL_MENU_Get_Menu_Parents();
        Task<InsertResult> TL_MENU_Ins(TL_MENU_ENTITY input);
        Task<InsertResult> TL_MENU_Upd(TL_MENU_ENTITY input);
        Task<CommonResult> TL_MENU_Del(int id);
        Task<CommonResult> TL_MENU_App(int id, string currentUserName);
        Task<TL_MENU_ENTITY> TL_MENU_ById(int id);
        List<AppMenuDto> GetAllMenus();
    }
}
