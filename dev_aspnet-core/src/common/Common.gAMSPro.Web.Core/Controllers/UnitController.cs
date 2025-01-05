using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Units;
using Common.gAMSPro.Units.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class UnitController : CoreAmsProControllerBase
    {
        readonly IUnitAppService unitAppService;

        public UnitController(IUnitAppService unitAppService)
        {
            this.unitAppService = unitAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_UNIT_ENTITY>> CM_UNIT_Search([FromBody] CM_UNIT_ENTITY input)
        {
            return await unitAppService.CM_UNIT_Search(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_UNIT_Ins([FromBody] CM_UNIT_ENTITY input)
        {
            return await unitAppService.CM_UNIT_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_UNIT_Upd([FromBody] CM_UNIT_ENTITY input)
        {
            return await unitAppService.CM_UNIT_Upd(input);
        }

        [HttpGet]
        public async Task<CM_UNIT_ENTITY> CM_UNIT_ById(string id)
        {
            return await unitAppService.CM_UNIT_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_UNIT_Del(string id)
        {
            return await unitAppService.CM_UNIT_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_UNIT_App(string id, string currentUserName)
        {
            return await unitAppService.CM_UNIT_App(id, currentUserName);
        }
        [HttpGet]
        public async Task<List<CM_UNIT_ENTITY>> CM_UNIT_List()
        {
            return await unitAppService.CM_UNIT_List();
        }
    }
}
