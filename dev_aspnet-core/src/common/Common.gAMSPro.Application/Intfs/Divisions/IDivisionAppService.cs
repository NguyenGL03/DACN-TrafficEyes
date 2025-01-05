using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Divisions.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.gAMSPro.Divisions
{
    public interface IDivisionAppService : IApplicationService
    {
        Task<PagedResultDto<CM_DIVISION_ENTITY>> CM_DIVISION_Search(CM_DIVISION_ENTITY input);
        Task<FileDto> CM_DIVISION_ToExcel(CM_DIVISION_ENTITY input);
        Task<InsertResult> CM_DIVISION_Ins(CM_DIVISION_ENTITY input);
        Task<InsertResult> CM_DIVISION_Upd(CM_DIVISION_ENTITY input);
        Task<CommonResult> CM_DIVISION_Del(string id);
        Task<CommonResult> CM_DIVISION_App(string id, string currentUserName);
        Task<CM_DIVISION_ENTITY> CM_DIVISION_ById(string id);
        Task<List<CM_DIVISION_ENTITY>> CM_DIVISION_GETALLCHILD(string parentId);
    }
}
