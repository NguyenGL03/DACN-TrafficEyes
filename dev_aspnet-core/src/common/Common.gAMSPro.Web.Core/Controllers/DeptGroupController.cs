using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.DeptGroups;
using Common.gAMSPro.DeptGroups.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class DeptGroupController : CoreAmsProControllerBase
    {
        readonly IDeptGroupAppService deptGroupAppService;

        public DeptGroupController(IDeptGroupAppService deptGroupAppService)
        {
            this.deptGroupAppService = deptGroupAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_DEPT_GROUP_ENTITY>> CM_DEPT_GROUP_Search([FromBody] CM_DEPT_GROUP_ENTITY input)
        {
            return await deptGroupAppService.CM_DEPT_GROUP_Search(input);
        }

        [HttpPost]
        public async Task<FileDto> CM_DEPT_GROUP_ToExcel([FromBody] CM_DEPT_GROUP_ENTITY input)
        {
            return await deptGroupAppService.CM_DEPT_GROUP_ToExcel(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_DEPT_GROUP_Ins([FromBody] CM_DEPT_GROUP_ENTITY input)
        {
            return await deptGroupAppService.CM_DEPT_GROUP_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_DEPT_GROUP_Upd([FromBody] CM_DEPT_GROUP_ENTITY input)
        {
            return await deptGroupAppService.CM_DEPT_GROUP_Upd(input);
        }

        [HttpGet]
        public async Task<CM_DEPT_GROUP_ENTITY> CM_DEPT_GROUP_ById(string id)
        {
            return await deptGroupAppService.CM_DEPT_GROUP_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_DEPT_GROUP_Del(string id)
        {
            return await deptGroupAppService.CM_DEPT_GROUP_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_DEPT_GROUP_App(string id, string currentUserName)
        {
            return await deptGroupAppService.CM_DEPT_GROUP_App(id, currentUserName);
        }
    }
}
