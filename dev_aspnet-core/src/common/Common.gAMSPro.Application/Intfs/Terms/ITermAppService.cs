using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.Terms.Dto;

namespace Common.gAMSPro.Intfs.Terms
{
    public interface ITermAppService
    {
        Task<PagedResultDto<CM_TERM_ENTITY>> CM_TERM_Search(CM_TERM_ENTITY input);
    }
}
