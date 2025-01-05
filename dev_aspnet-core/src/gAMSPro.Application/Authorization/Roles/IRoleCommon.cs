using Common.gAMSPro.AppMenus.Dto;
using gAMSPro.Authorization.Permissions.Dto;
using System.Collections.Generic;

namespace gAMSPro.Authorization.Roles
{
    public interface IRoleCommon
    {
        List<FlatPermissionDto> MergeMenuPermissionWithPermissionRole(List<FlatPermissionDto> permissionFlatDto, List<AppMenuDto> appMenu, List<string> actions);
    }
}
