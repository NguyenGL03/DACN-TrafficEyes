using Abp.Application.Services.Dto;
using Common.gAMSPro.SysParameters;
using Common.gAMSPro.SysParameters.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SysParametersController : CoreAmsProControllerBase
    {
        private readonly ISysParametersAppService sysParametersAppService;

        public SysParametersController(ISysParametersAppService sysParametersAppService)
        {
            this.sysParametersAppService = sysParametersAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<SYS_PARAMETERS_ENTITY>> SYS_PARAMETERS_Search([FromBody] SYS_PARAMETERS_ENTITY input)
        {
            return await sysParametersAppService.SYS_PARAMETERS_Search(input);
        }

        [HttpPost]
        public async Task<FileDto> SYS_PARAMETERS_ToExcel([FromBody] SYS_PARAMETERS_ENTITY input)
        {
            return await sysParametersAppService.SYS_PARAMETERS_ToExcel(input);
        }

        [HttpPost]
        public async Task<InsertResult> SYS_PARAMETERS_Ins([FromBody] SYS_PARAMETERS_ENTITY input)
        {
            return await sysParametersAppService.SYS_PARAMETERS_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> SYS_PARAMETERS_Upd([FromBody] SYS_PARAMETERS_ENTITY input)
        {
            return await sysParametersAppService.SYS_PARAMETERS_Upd(input);
        }

        [HttpGet]
        public async Task<SYS_PARAMETERS_ENTITY> SYS_PARAMETERS_ById(decimal id)
        {
            return await sysParametersAppService.SYS_PARAMETERS_ById(id);
        }

        [HttpGet]
        public async Task<SYS_PARAMETERS_ENTITY> SYS_PARAMETERS_ByParaKey(string parakey)
        {
            return await sysParametersAppService.SYS_PARAMETERS_ByParaKey(parakey);
        }

        [HttpDelete]
        public async Task<CommonResult> SYS_PARAMETERS_Del(decimal id)
        {
            return await sysParametersAppService.SYS_PARAMETERS_Del(id);
        }

    }
}
