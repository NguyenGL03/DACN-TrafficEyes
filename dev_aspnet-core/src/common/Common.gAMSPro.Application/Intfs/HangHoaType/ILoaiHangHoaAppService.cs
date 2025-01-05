using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.HangHoaType.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Intfs.HangHoaType
{
    public interface IHangHoaTypeAppService : IApplicationService
    {
        Task<PagedResultDto<CM_HANGHOA_TYPE_ENTITY>> CM_HANGHOA_TYPE_Search(CM_HANGHOA_TYPE_ENTITY input);
        Task<List<CM_HANGHOA_TYPE_ENTITY>> CM_HANGHOA_TYPE_GetAll();
        Task<InsertResult> CM_HANGHOA_TYPE_Ins(CM_HANGHOA_TYPE_ENTITY input);
        Task<InsertResult> CM_HANGHOA_TYPE_Upd(CM_HANGHOA_TYPE_ENTITY input);
        Task<CommonResult> CM_HANGHOA_TYPE_Del(string id);
        Task<CommonResult> CM_HANGHOA_TYPE_App(string id, string currentUserName);
        Task<CM_HANGHOA_TYPE_ENTITY> CM_HANGHOA_TYPE_ById(string id); 
    }
}
