using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Employees.Dto;
using Common.gAMSPro.Intfs.Employees.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Employees
{
    public interface IEmployeeAppService : IApplicationService
    {
        Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_Search(CM_EMPLOYEE_ENTITY input);
        Task<CM_EMPLOYEE_LOG_ENTITY> CM_EMPLOYEE_LOG_ByUserName(string username);
        Task<InsertResult> CM_EMPLOYEE_Ins(CM_EMPLOYEE_ENTITY input);
        Task<InsertResult> CM_EMPLOYEE_Upd(CM_EMPLOYEE_ENTITY input);
        Task<CommonResult> CM_EMPLOYEE_Del(string id);
        Task<CommonResult> CM_EMPLOYEE_App(string id, string currentUserName);
        Task<CM_EMPLOYEE_ENTITY> CM_EMPLOYEE_ById(string id);
        Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_Search_NotMapping(CM_EMPLOYEE_ENTITY input);
        Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_MODAL(CM_EMPLOYEE_ENTITY input);
        Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> ASS_TRANSFER_REPRESENT_SEARCH(CM_EMPLOYEE_ENTITY input);
        Task<PagedResultDto<CM_EMPLOYEE_ENTITY>> CM_EMPLOYEE_EXIST_IN_TLUSER_MODAL(CM_EMPLOYEE_ENTITY input);
    }
}
