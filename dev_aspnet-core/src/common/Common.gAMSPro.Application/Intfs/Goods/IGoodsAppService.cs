using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Goodss.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;

namespace Common.gAMSPro.Goodss
{
    public interface IGoodsAppService : IApplicationService
    {
        Task<PagedResultDto<CM_GOODS_ENTITY>> CM_GOODS_Search(CM_GOODS_ENTITY input);
        Task<FileDto> CM_GOODS_ToExcel(CM_GOODS_ENTITY input);
        Task<InsertResult> CM_GOODS_Ins(CM_GOODS_ENTITY input);
        Task<InsertResult> CM_GOODS_Upd(CM_GOODS_ENTITY input);
        Task<CommonResult> CM_GOODS_Del(string id);
        Task<CommonResult> CM_GOODS_App(string id, string currentUserName);
        Task<CM_GOODS_ENTITY> CM_GOODS_ById(string id);
    }
}
