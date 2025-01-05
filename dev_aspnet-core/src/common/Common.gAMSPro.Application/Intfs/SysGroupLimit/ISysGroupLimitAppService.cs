using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.SysGroupLimit.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Intfs.SysGroupLimit
{
    public interface ISysGroupLimitAppService : IApplicationService
    {
        #region Hạn mức phê duyệt

        Task<PagedResultDto<SYS_GROUP_LIMIT_ENTITY>> SYS_GROUP_LIMIT_Search(SYS_GROUP_LIMIT_ENTITY input);
        Task<List<SYS_GROUP_LIMIT_ENTITY>> SYS_GROUP_LIMIT_GetAllType();
        Task<InsertResult> SYS_GROUP_LIMIT_Ins(SYS_GROUP_LIMIT_ENTITY input);
        Task<InsertResult> SYS_GROUP_LIMIT_Upd(SYS_GROUP_LIMIT_ENTITY input);
        Task<CommonResult> SYS_GROUP_LIMIT_Del(string id);
        Task<CommonResult> SYS_GROUP_LIMIT_App(string id, string currentUserName);
        Task<SYS_GROUP_LIMIT_ENTITY> SYS_GROUP_LIMIT_ById(string id);

        #endregion

        #region Hạn mức phê duyệt chi tiết

        Task<InsertResult> SYS_GROUP_LIMIT_DT_Ins(SYS_GROUP_LIMIT_DT_ENTITY input);
        Task<InsertResult> SYS_GROUP_LIMIT_DT_Upd(SYS_GROUP_LIMIT_DT_ENTITY input);
        Task<SYS_GROUP_LIMIT_DT_ENTITY> SYS_GROUP_LIMIT_DT_ById(string id);
        Task<PagedResultDto<SYS_GROUP_LIMIT_DT_ENTITY>> SYS_GROUP_LIMIT_DT_Search(SYS_GROUP_LIMIT_DT_ENTITY input);
        Task<CommonResult> SYS_GROUP_LIMIT_DT_Del(string id);

        #endregion

    }
}
