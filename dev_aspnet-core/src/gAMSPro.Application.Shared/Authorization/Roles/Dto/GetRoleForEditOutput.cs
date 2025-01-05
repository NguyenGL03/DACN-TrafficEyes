using System.Collections.Generic;
using gAMSPro.Authorization.Permissions.Dto;

namespace gAMSPro.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        public RoleEditDto Role { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }

        public List<string> Actions { get; set; }
    }
}