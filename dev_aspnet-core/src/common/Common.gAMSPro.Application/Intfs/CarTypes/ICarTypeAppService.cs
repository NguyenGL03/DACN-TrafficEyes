using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Application.Intfs.CarTypes.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;

namespace Common.gAMSPro.Application.Intfs.CarTypes
{
    public interface ICarTypeAppService : IApplicationService
    {
        Task<PagedResultDto<CM_CAR_TYPE_ENTITY>> CM_CAR_TYPE_Search(CM_CAR_TYPE_ENTITY input);
        Task<List<CM_CAR_TYPE_ENTITY>> CM_CAR_TYPE_List(CM_CAR_TYPE_ENTITY input);
        Task<FileDto> CM_CAR_TYPE_ToExcel(CM_CAR_TYPE_ENTITY input);
        Task<InsertResult> CM_CAR_TYPE_Ins(CM_CAR_TYPE_ENTITY input);
        Task<InsertResult> CM_CAR_TYPE_Upd(CM_CAR_TYPE_ENTITY input);
        Task<CommonResult> CM_CAR_TYPE_Del(string id);
        Task<CommonResult> CM_CAR_TYPE_App(string id, string currentUserName);
        Task<CM_CAR_TYPE_ENTITY> CM_CAR_TYPE_ById(string id);
    }
}
