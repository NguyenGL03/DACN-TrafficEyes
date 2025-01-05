using Abp.Application.Services.Dto;
using Abp.Extensions;
using Common.gAMSPro.Departments;
using Common.gAMSPro.Departments.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Consts;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DepartmentController : CoreAmsProControllerBase
    {
        readonly IDepartmentAppService departmentAppService;

        public DepartmentController(IDepartmentAppService departmentAppService)
        {
            this.departmentAppService = departmentAppService;
        }

        [HttpGet]
        public async Task<List<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_Combobox(string subbrId)
        {
            if (subbrId.IsNullOrWhiteSpace())
            {
                return new List<CM_DEPARTMENT_ENTITY>();
            }
            var input = new CM_DEPARTMENT_ENTITY()
            {
                MaxResultCount = -1,
                BRANCH_ID = subbrId,
                RECORD_STATUS = RecordStatusConsts.Active,
                AUTH_STATUS = ApproveStatusConsts.Approve
            };
            return (await departmentAppService.CM_DEPARTMENT_Search(input)).Items.ToList();
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_Search([FromBody] CM_DEPARTMENT_ENTITY input)
        {
            return await departmentAppService.CM_DEPARTMENT_Search(input);
        }

        [HttpGet]
        public async Task<List<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_HS_List()
        {
            return await departmentAppService.CM_DEPARTMENT_HS_List();
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_COSTCENTER_Search([FromBody] CM_DEPARTMENT_ENTITY input)
        {
            return await departmentAppService.CM_DEPARTMENT_COSTCENTER_Search(input);
        }
        [HttpPost]
        public async Task<FileDto> CM_DEPARTMENT_ToExcel([FromBody] CM_DEPARTMENT_ENTITY input)
        {
            return await departmentAppService.CM_DEPARTMENT_ToExcel(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_DEPARTMENT_Ins([FromBody] CM_DEPARTMENT_ENTITY input)
        {
            return await departmentAppService.CM_DEPARTMENT_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_DEPARTMENT_Upd([FromBody] CM_DEPARTMENT_ENTITY input)
        {
            return await departmentAppService.CM_DEPARTMENT_Upd(input);
        }

        [HttpGet]
        public async Task<CM_DEPARTMENT_ENTITY> CM_DEPARTMENT_ById(string id)
        {
            return await departmentAppService.CM_DEPARTMENT_ById(id);
        }

        [HttpGet]
        public async Task<CM_DEPARTMENT_ENTITY> CM_DEPARTMENT_ByCode(string branch_id, string dep_id)
        {
            return await departmentAppService.CM_DEPARTMENT_ByCode(branch_id, dep_id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_DEPARTMENT_Del(string id)
        {
            return await departmentAppService.CM_DEPARTMENT_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_DEPARTMENT_App(string id, string currentUserName)
        {
            return await departmentAppService.CM_DEPARTMENT_App(id, currentUserName);
        }
    }
}
