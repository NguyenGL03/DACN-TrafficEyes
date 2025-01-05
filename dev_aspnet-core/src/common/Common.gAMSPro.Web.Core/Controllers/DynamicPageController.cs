using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Application.Intfs.DynamicPage;
using Common.gAMSPro.Application.Intfs.DynamicPage.Dto;
using Common.gAMSPro.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
namespace Common.gAMSPro.Web.Core.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class DynamicPageController : CoreAmsProControllerBase
    {
        readonly IDynamicPageAppService dynamicPageAppService;

        public DynamicPageController(IDynamicPageAppService dynamicPageAppService)
        {
            this.dynamicPageAppService = dynamicPageAppService;
        }
        [HttpPost]
        public async Task<PagedResultDto<DYNAMIC_PAGE_ENTITY>> DYNAMIC_PAGE_Search([FromBody] DYNAMIC_PAGE_ENTITY input)
        {
            return await dynamicPageAppService.DYNAMIC_PAGE_Search(input);
        }
        [HttpPost]
        public async Task<DYNAMIC_PAGE_ENTITY> DYNAMIC_PAGE_ById(string input)
        {

            return await dynamicPageAppService.DYNAMIC_PAGE_ById(input);
        }
        [HttpPost]
        public async Task<IDictionary<string, object>> DYNAMIC_PAGE_Ins([FromBody] DYNAMIC_PAGE_ENTITY input)
        {

            return await dynamicPageAppService.DYNAMIC_PAGE_Ins(input);
        }
        [HttpPost]
        public async Task<IDictionary<string, object>> DYNAMIC_PAGE_Upd([FromBody] DYNAMIC_PAGE_ENTITY input)
        {
            return await dynamicPageAppService.DYNAMIC_PAGE_Upd(input);
        }
        [HttpPost]
        public async Task<IDictionary<string, object>> DYNAMIC_PAGE_Del(string input)
        {
            return await dynamicPageAppService.DYNAMIC_PAGE_Del(input);
        }
    }
}