using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Castle.Core.Logging;
using Common.gAMSPro.AppMenus;
using Common.gAMSPro.AppMenus.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [DisableValidation]
    public class AppMenuController : CoreAmsProControllerBase
    {
        private readonly IAppMenuAppService appMenuAppService;
        private readonly ILogger logger;
        public AppMenuController(IAppMenuAppService appMenuAppService, ILogger logger)
        {
            this.appMenuAppService = appMenuAppService;
            this.logger = logger;
        }

        [HttpGet]
        public List<AppMenuDto> GetAllMenus()
        {
            return appMenuAppService.GetAllMenus();
        }

        [HttpPost]
        public async Task<PagedResultDto<TL_MENU_ENTITY>> TL_MENU_Search([FromBody] TL_MENU_ENTITY input)
        {
            return await appMenuAppService.TL_MENU_Search(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<TL_MENU_ENTITY>> TL_MENU_Search_By_RoleID([FromBody] TL_MENU_ENTITY input)
        {
            return await appMenuAppService.TL_MENU_Search_By_RoleID(input);
        }

        [HttpPost]
        public async Task<List<TL_MENU_ENTITY>> TL_MENU_Search_Menu_Parent()
        {
            return await appMenuAppService.TL_MENU_Get_Menu_Parents();
        }

        [HttpPost]
        public async Task<InsertResult> TL_MENU_Ins([FromBody] TL_MENU_ENTITY input)
        {
            return await appMenuAppService.TL_MENU_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> TL_MENU_Upd([FromBody] TL_MENU_ENTITY input)
        {
            return await appMenuAppService.TL_MENU_Upd(input);
        }

        [HttpGet]
        public async Task<TL_MENU_ENTITY> TL_MENU_ById(int id)
        {
            return await appMenuAppService.TL_MENU_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> TL_MENU_Del(int id)
        {
            return await appMenuAppService.TL_MENU_Del(id);

        }

        [HttpPost]
        public async Task<CommonResult> TL_MENU_App(int id, string currentUserName)
        {
            return await appMenuAppService.TL_MENU_App(id, currentUserName);
        }
    }
}
