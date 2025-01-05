using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.DVDM.Dto;

namespace Common.gAMSPro.Intfs.DVDM
{
    public interface IDVDMAppService : IApplicationService
    {
        Task<List<CM_DVDM_ENTITY>> CM_DVDM_GetAll();
        Task<PagedResultDto<CM_DVDM_ENTITY>> CM_DVDM_Search(CM_DVDM_ENTITY input);
        Task<PagedResultDto<CM_DVDM_ENTITY>> CM_DVCM_Search(CM_DVDM_ENTITY input); // 30/11/22 Search Popup DVCM PYC
    }
}
