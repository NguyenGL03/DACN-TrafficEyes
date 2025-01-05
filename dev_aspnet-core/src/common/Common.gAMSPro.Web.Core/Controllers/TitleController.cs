using Abp.AspNetCore.Mvc.Controllers;
using Common.gAMSPro.Intfs.Title;
using Common.gAMSPro.Intfs.Title.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TitleController : AbpController
    {
        private readonly ITitleAppService titleAppService;
        public TitleController(ITitleAppService titleAppService)
        {
            this.titleAppService = titleAppService;
        }

        [HttpGet]
        public async Task<List<CM_TITLE_ENTITY>> CM_TITLE_SEARCH(string? titleType)
        {
            return await titleAppService.CM_TITLE_SEARCH(titleType);
        }
    }
}
