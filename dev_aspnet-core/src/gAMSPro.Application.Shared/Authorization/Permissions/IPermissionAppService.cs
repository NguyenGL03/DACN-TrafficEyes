using Abp.Application.Services;
using Abp.Application.Services.Dto;
using gAMSPro.Authorization.Permissions.Dto;

namespace gAMSPro.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
