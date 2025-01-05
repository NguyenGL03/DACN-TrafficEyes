using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.AllCodes.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.AllCodes
{
    public interface IAllCodeAppService : IApplicationService
    {
        Task<List<CM_ALLCODE_ENTITY>> CM_ALLCODE_GetByCDNAME(string cdName, string cdType);
        Task<PagedResultDto<CM_ALLCODE_ENTITY>> CM_ALLCODE_Search(CM_ALLCODE_ENTITY input);
        Task<InsertResult> CM_ALLCODE_Ins(CM_ALLCODE_ENTITY input);
        Task<InsertResult> CM_ALLCODE_Upd(CM_ALLCODE_ENTITY input);
        Task<CommonResult> CM_ALLCODE_Del(int id);
        Task<CM_ALLCODE_ENTITY> CM_ALLCODE_ById(string cdName, string cdType);
        Task<CM_ALLCODE_ENTITY> CM_ALLCODE_ById_v2(string cdType, string cdName, string cdVal);
    }
}
