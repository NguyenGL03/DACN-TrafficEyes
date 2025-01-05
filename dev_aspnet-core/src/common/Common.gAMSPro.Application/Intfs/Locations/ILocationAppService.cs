using Abp.Application.Services;
using Common.gAMSPro.Intfs.Locations.Dto;

namespace Common.gAMSPro.Intfs.Locations
{
    public interface ILocationAppService : IApplicationService
    {
        Task<CM_LOCATION_ENTITY> CM_LOCATION_AllData();
    }
}
