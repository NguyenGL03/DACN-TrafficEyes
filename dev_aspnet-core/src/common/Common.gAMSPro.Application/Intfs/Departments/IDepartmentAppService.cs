using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Departments.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;

namespace Common.gAMSPro.Departments
{
    public interface IDepartmentAppService : IApplicationService
    {
        Task<PagedResultDto<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_Search(CM_DEPARTMENT_ENTITY input);
        Task<PagedResultDto<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_COSTCENTER_Search(CM_DEPARTMENT_ENTITY input);
        Task<List<CM_DEPARTMENT_ENTITY>> CM_DEPARTMENT_HS_List();
        Task<FileDto> CM_DEPARTMENT_ToExcel(CM_DEPARTMENT_ENTITY input);
        Task<InsertResult> CM_DEPARTMENT_Ins(CM_DEPARTMENT_ENTITY input);
        Task<InsertResult> CM_DEPARTMENT_Upd(CM_DEPARTMENT_ENTITY input);
        Task<CommonResult> CM_DEPARTMENT_Del(string id);
        Task<CommonResult> CM_DEPARTMENT_App(string id, string currentUserName);
        Task<CM_DEPARTMENT_ENTITY> CM_DEPARTMENT_ById(string id);
        Task<CM_DEPARTMENT_ENTITY> CM_DEPARTMENT_ByCode(string branch_id, string dep_id);
    }
}
