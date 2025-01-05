using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.SysParameters.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;

namespace Common.gAMSPro.SysParameters
{
    public interface ISysParametersAppService : IApplicationService
    {
        Task<PagedResultDto<SYS_PARAMETERS_ENTITY>> SYS_PARAMETERS_Search(SYS_PARAMETERS_ENTITY input);
        Task<FileDto> SYS_PARAMETERS_ToExcel(SYS_PARAMETERS_ENTITY input);
        Task<InsertResult> SYS_PARAMETERS_Ins(SYS_PARAMETERS_ENTITY input);
        Task<InsertResult> SYS_PARAMETERS_Upd(SYS_PARAMETERS_ENTITY input);
        Task<CommonResult> SYS_PARAMETERS_Del(decimal id);
        Task<SYS_PARAMETERS_ENTITY> SYS_PARAMETERS_ById(decimal id);
        Task<SYS_PARAMETERS_ENTITY> SYS_PARAMETERS_ByParaKey(string parakey);
    }
}
