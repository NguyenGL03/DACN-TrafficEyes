using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Common.gAMSPro.Core.Authorization;

namespace Core.gAMSPro.Core.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class gAMSProCoreAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public gAMSProCoreAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public gAMSProCoreAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)
            Permission pages = context.GetPermissionOrNull(gAMSProCorePermissions.Pages) ?? context.CreatePermission(gAMSProCorePermissions.Pages, L("Pages"));

        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, gAMSProConsts.LocalizationSourceName);
        }

        private Permission GetPermission(Permission parent, string childName)
        {
            return parent.Children.Where(x => x.Name.Equals(childName)).FirstOrDefault();
        }
    }
}
