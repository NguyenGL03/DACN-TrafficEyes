using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.DeptGroups.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;

namespace Common.gAMSPro.DeptGroups
{
    public interface IDeptGroupAppService : IApplicationService
    {
        Task<PagedResultDto<CM_DEPT_GROUP_ENTITY>> CM_DEPT_GROUP_Search(CM_DEPT_GROUP_ENTITY input);
        Task<FileDto> CM_DEPT_GROUP_ToExcel(CM_DEPT_GROUP_ENTITY input);
        Task<InsertResult> CM_DEPT_GROUP_Ins(CM_DEPT_GROUP_ENTITY input);
        Task<InsertResult> CM_DEPT_GROUP_Upd(CM_DEPT_GROUP_ENTITY input);
        Task<CommonResult> CM_DEPT_GROUP_Del(string id);
        Task<CommonResult> CM_DEPT_GROUP_App(string id, string currentUserName);
        Task<CM_DEPT_GROUP_ENTITY> CM_DEPT_GROUP_ById(string id);
    }
}
