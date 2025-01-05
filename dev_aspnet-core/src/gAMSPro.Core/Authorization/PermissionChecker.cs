using Abp.Authorization;
using gAMSPro.Authorization.Roles;
using gAMSPro.Authorization.Users;

namespace gAMSPro.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
