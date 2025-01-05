using Abp.Application.Services.Dto;
using Common.gAMSPro.TlUsers;
using Common.gAMSPro.TlUsers.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TlUserController : CoreAmsProControllerBase
    {
        private readonly ITlUserAppService tlUserAppService;

        public TlUserController(ITlUserAppService tlUserAppService)
        {
            this.tlUserAppService = tlUserAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<TL_USER_ENTITY>> TL_USER_Search([FromBody] TL_USER_ENTITY input)
        {
            return await tlUserAppService.TL_USER_Search(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<TL_USER_ENTITY>> TLUSER_MANAGER_SEARCH([FromBody] TL_USER_ENTITY input)
        {
            return await tlUserAppService.TLUSER_MANAGER_SEARCH(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> MD_USER_MANAGER_Upd_AUTHORITYUSER([FromBody] TL_USER_ENTITY input)
        {
            return await tlUserAppService.MD_USER_MANAGER_Upd_AUTHORITYUSER(input);

        }

        [HttpPost]
        public async Task<IDictionary<string, object>> MD_USER_MANAGER_Del_AUTHORITYUSER([FromBody] TL_USER_ENTITY input)
        {
            return await tlUserAppService.MD_USER_MANAGER_Del_AUTHORITYUSER(input);

        }

        [HttpPost]
        public async Task<List<TL_USER_ENTITY>> TL_USER_GET_List([FromBody] TL_USER_ENTITY input)
        {
            return await tlUserAppService.TL_USER_GET_List(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<TL_USER_ENTITY>> TL_USER_GET_List_v2([FromBody] TL_USER_ENTITY input)
        {
            return await tlUserAppService.TL_USER_GET_List_v2(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> TL_USER_Ins([FromBody] TL_USER_ENTITY input)
        {
            return await tlUserAppService.TL_USER_Ins(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> TL_USER_Upd([FromBody] TL_USER_ENTITY input)
        {
            return await tlUserAppService.TL_USER_Upd(input);
        }

        [HttpGet]
        public async Task<TL_USER_ENTITY> TL_USER_ById(string id)
        {
            return await tlUserAppService.TL_USER_ById(id);
        }

        [HttpDelete]
        public async Task<IDictionary<string, object>> TL_USER_Del(string id)
        {
            return await tlUserAppService.TL_USER_Del(id);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> TL_USER_App(string id, string currentUserName)
        {
            return await tlUserAppService.TL_USER_App(id, currentUserName);
        }
        [HttpPost]
        public async Task<List<TL_USER_ENTITY>> TL_USER_By_DEPARTMENT()
        {
            return await tlUserAppService.TL_USER_By_DEPARTMENT();
        }
    }
}
