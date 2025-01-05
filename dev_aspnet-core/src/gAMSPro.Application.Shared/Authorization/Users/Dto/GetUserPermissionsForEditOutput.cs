using System.Collections.Generic;
using gAMSPro.Authorization.Permissions.Dto;

namespace gAMSPro.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}