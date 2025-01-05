using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.HangHoaGroup.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Intfs.HangHoaGroup
{
    public interface IHangHoaGroupAppService : IApplicationService
    {
        Task<PagedResultDto<CM_HANGHOA_GROUP_ENTITY>> CM_HangHoa_Group_Search(CM_HANGHOA_GROUP_ENTITY input);
        Task<List<CM_HANGHOA_GROUP_ENTITY>> CM_HangHoa_Group_GetAll();
        Task<InsertResult> CM_HangHoa_Group_Ins(CM_HANGHOA_GROUP_ENTITY input);
        Task<InsertResult> CM_HangHoa_Group_Upd(CM_HANGHOA_GROUP_ENTITY input);
        Task<CommonResult> CM_HangHoa_Group_Del(string id);
        Task<CommonResult> CM_HangHoa_Group_App(string id, string currentUserName);
        Task<CM_HANGHOA_GROUP_ENTITY> CM_HangHoa_Group_ById(string id);

    }
}
