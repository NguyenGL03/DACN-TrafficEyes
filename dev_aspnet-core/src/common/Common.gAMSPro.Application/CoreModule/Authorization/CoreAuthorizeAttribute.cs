using Abp.Authorization;

namespace Common.gAMSPro.Authorization
{
    public class CoreAuthorizeAttribute : AbpAuthorizeAttribute
    {
        public CoreAuthorizeAttribute(string prefix, string pageName, string action) : base(prefix + "." + pageName + "." + action)
        {
            base.RequireAllPermissions = false;
        }
        public CoreAuthorizeAttribute(string[] permissions) : base(permissions)
        {
            base.RequireAllPermissions = false;
        }
    }
}
