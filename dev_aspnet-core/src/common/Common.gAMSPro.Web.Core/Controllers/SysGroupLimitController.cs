using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.SysGroupLimit;
using Common.gAMSPro.Intfs.SysGroupLimit.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;


namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class SysGroupLimitController : CoreAmsProControllerBase
    {
        private readonly ISysGroupLimitAppService sysGroupLimitAppService;
        public SysGroupLimitController(ISysGroupLimitAppService sysGroupLimitAppService)
        {
            this.sysGroupLimitAppService = sysGroupLimitAppService;
        }


        [HttpPost]
        public async Task<PagedResultDto<SYS_GROUP_LIMIT_ENTITY>> Sys_Group_Limit_Search([FromBody] SYS_GROUP_LIMIT_ENTITY input)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_Search(input);
        }


        [HttpPost]
        public async Task<PagedResultDto<SYS_GROUP_LIMIT_DT_ENTITY>> Sys_Group_Limit_DT_Search([FromBody] SYS_GROUP_LIMIT_DT_ENTITY input)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_DT_Search(input);
        }

        [HttpPost]
        public async Task<InsertResult> Sys_Group_Limit_Ins([FromBody] SYS_GROUP_LIMIT_ENTITY input)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> Sys_Group_Limit_DT_Ins([FromBody] SYS_GROUP_LIMIT_DT_ENTITY input)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_DT_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> Sys_Group_Limit_DT_Upd([FromBody] SYS_GROUP_LIMIT_DT_ENTITY input)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_DT_Upd(input);
        }

        [HttpPost]
        public async Task<CommonResult> Sys_Group_Limit_App(string id, string currentUserName)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_App(id, currentUserName);
        }

        [HttpPost]
        public async Task<InsertResult> Sys_Group_Limit_Upd([FromBody] SYS_GROUP_LIMIT_ENTITY input)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_Upd(input);
        }

        [HttpGet]
        public async Task<SYS_GROUP_LIMIT_ENTITY> Sys_Group_Limit_ById(string id)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_ById(id);
        }

        [HttpGet]
        public async Task<SYS_GROUP_LIMIT_DT_ENTITY> Sys_Group_Limit_DT_ById(string id)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_DT_ById(id);
        }

        [HttpGet]
        public async Task<List<SYS_GROUP_LIMIT_ENTITY>> Sys_Group_Limit_GetAllType()
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_GetAllType();
        }

        [HttpDelete]
        public async Task<CommonResult> Sys_Group_Limit_Del(string id)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_Del(id);
        }

        [HttpDelete]
        public async Task<CommonResult> Sys_Group_Limit_DT_Del(string id)
        {
            return await sysGroupLimitAppService.SYS_GROUP_LIMIT_DT_Del(id);
        }
    }
}
