using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Units.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Units
{
    public interface IUnitAppService : IApplicationService
    {
        Task<PagedResultDto<CM_UNIT_ENTITY>> CM_UNIT_Search(CM_UNIT_ENTITY input);
        Task<InsertResult> CM_UNIT_Ins(CM_UNIT_ENTITY input);
        Task<InsertResult> CM_UNIT_Upd(CM_UNIT_ENTITY input);
        Task<CommonResult> CM_UNIT_Del(string id);
        Task<CommonResult> CM_UNIT_App(string id, string currentUserName);
        Task<CM_UNIT_ENTITY> CM_UNIT_ById(string id);
        Task<List<CM_UNIT_ENTITY>> CM_UNIT_List();

    }
}
