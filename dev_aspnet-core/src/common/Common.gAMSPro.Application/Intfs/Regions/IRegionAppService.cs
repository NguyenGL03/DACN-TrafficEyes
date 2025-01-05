using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Regions.Dto;
using Core.gAMSPro.Utils;
using System.Threading.Tasks;

namespace Common.gAMSPro.Regions
{
    public interface IRegionAppService : IApplicationService
    {
        Task<PagedResultDto<CM_REGION_ENTITY>> CM_REGION_Search(CM_REGION_ENTITY? input);
        Task<InsertResult> CM_REGION_Ins(CM_REGION_ENTITY input);
        Task<InsertResult> CM_REGION_Upd(CM_REGION_ENTITY input);
        Task<CommonResult> CM_REGION_Del(string id);
        Task<CommonResult> CM_REGION_App(string id, string currentUserName);
        Task<CM_REGION_ENTITY> CM_REGION_ById(string id);
    }
}
