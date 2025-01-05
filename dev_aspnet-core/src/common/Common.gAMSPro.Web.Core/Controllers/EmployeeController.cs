using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Employees;
using Common.gAMSPro.Employees.Dto;
using Common.gAMSPro.Intfs.Employees.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class EmployeeController : CoreAmsProControllerBase
    {
        readonly IEmployeeAppService employeeAppService;

        public EmployeeController(IEmployeeAppService employeeAppService)
        {
            this.employeeAppService = employeeAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_Search([FromBody] CM_EMPLOYEE_ENTITY input)
        {
            return await employeeAppService.CM_EMPLOYEE_Search(input);
        }
        [HttpPost]
        public async Task<CM_EMPLOYEE_LOG_ENTITY> CM_EMPLOYEE_LOG_ByUserName(string username)
        {
            return await employeeAppService.CM_EMPLOYEE_LOG_ByUserName(username);
        }
        [HttpPost]
        public async Task<InsertResult> CM_EMPLOYEE_Ins([FromBody] CM_EMPLOYEE_ENTITY input)
        {
            return await employeeAppService.CM_EMPLOYEE_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_EMPLOYEE_Upd([FromBody] CM_EMPLOYEE_ENTITY input)
        {
            return await employeeAppService.CM_EMPLOYEE_Upd(input);
        }

        [HttpGet]
        public async Task<CM_EMPLOYEE_ENTITY> CM_EMPLOYEE_ById(string id)
        {
            return await employeeAppService.CM_EMPLOYEE_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_EMPLOYEE_Del(string id)
        {
            return await employeeAppService.CM_EMPLOYEE_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_EMPLOYEE_App(string id, string currentUserName)
        {
            return await employeeAppService.CM_EMPLOYEE_App(id, currentUserName);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_Search_NotMapping([FromBody] CM_EMPLOYEE_ENTITY input)
        {
            return await employeeAppService.CM_EMPLOYEE_Search_NotMapping(input);
        }
        [HttpPost]
        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_MODAL([FromBody] CM_EMPLOYEE_ENTITY input)
        {
            return await employeeAppService.CM_EMPLOYEE_MODAL(input);
        }
        [HttpPost]
        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> ASS_TRANSFER_REPRESENT_SEARCH([FromBody] CM_EMPLOYEE_ENTITY input)
        {
            return await employeeAppService.ASS_TRANSFER_REPRESENT_SEARCH(input);
        }
        [HttpPost]
        public async Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_EXIST_IN_TLUSER_MODAL([FromBody] CM_EMPLOYEE_ENTITY input)
        {
            return await employeeAppService.CM_EMPLOYEE_EXIST_IN_TLUSER_MODAL(input);
        }
    }
}
