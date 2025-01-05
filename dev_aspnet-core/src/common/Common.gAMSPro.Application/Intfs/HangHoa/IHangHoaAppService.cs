using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.HangHoa.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Intfs.HangHoa
{
    public interface IHangHoaAppService : IApplicationService
    {
        Task<PagedResultDto<CM_HANGHOA_ENTITY>> CM_HANGHOA_Search(CM_HANGHOA_ENTITY input);
        Task<InsertResult> CM_HANGHOA_Ins(CM_HANGHOA_ENTITY input);
        Task<InsertResult> CM_HANGHOA_Upd(CM_HANGHOA_ENTITY input);
        Task<CommonResult> CM_HANGHOA_Del(string id, string userLogin);
        Task<CommonResult> CM_HANGHOA_App(string id, string currentUserName);
        Task<CM_HANGHOA_ENTITY> CM_HANGHOA_ById(string id);
    }
}
