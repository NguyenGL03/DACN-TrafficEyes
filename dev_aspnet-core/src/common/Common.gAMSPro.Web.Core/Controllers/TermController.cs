using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Controllers;
using Common.gAMSPro.Intfs.Terms;
using Common.gAMSPro.Intfs.Terms.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class TermController : AbpController
    {
        private readonly ITermAppService termAppService;

        public TermController(ITermAppService termAppService)
        {
            this.termAppService = termAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_TERM_ENTITY>> CM_TERM_Search([FromBody] CM_TERM_ENTITY filterInput)
        {
            return await termAppService.CM_TERM_Search(filterInput);
        }
    }
}
