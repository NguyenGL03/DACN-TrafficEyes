using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Models;
using gAMSPro.Consts;
using System.Reflection;

namespace Common.gAMSPro.Core.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class gAMSProCommonAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public gAMSProCommonAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public gAMSProCommonAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        [UnitOfWork]
        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            try
            {
                //TODO Add Menu Permission
                //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)
                var pages = context.GetPermissionOrNull(gAMSProCorePermissions.Pages) ?? context.CreatePermission(gAMSProCorePermissions.Pages, L("Pages"));
                var appMenuQuery = IocManager.Instance.IocContainer.Resolve<IRepository<TL_MENU>>().GetAll();
                var appMenus = appMenuQuery
                            .Where(x => x.AUTH_STATUS == ApproveStatusConsts.Approve && x.RECORD_STATUS == RecordStatusConsts.Active)
                            .Select(x => x.MENU_PERMISSION)
                            .Distinct()
                .Select(x => new PermissionModel()
                {
                    MENU_PERMISSION = x
                })
                .Where(x => !string.IsNullOrWhiteSpace(x.MENU_PERMISSION))
                .ToList().OrderBy(x => x.MENU_PERMISSION.Length);

                var appMenuDict = appMenus.ToDictionary(x => x.MENU_PERMISSION);
                var permissionSetted = new HashSet<string>();

                foreach (var appMenu in appMenus)
                {
                    if (permissionSetted.Contains(appMenu.MENU_PERMISSION))
                    {
                        continue;
                    }
                    else
                    {
                        permissionSetted.Add(appMenu.MENU_PERMISSION);
                    }

                    var length = appMenu.MENU_PERMISSION.LastIndexOf(".");
                    var name = appMenu.MENU_PERMISSION.Substring(appMenu.MENU_PERMISSION.LastIndexOf(".") + 1);
                    var parentKey = "";
                    if (length >= 0)
                    {
                        parentKey = appMenu.MENU_PERMISSION.Substring(0, length);
                    }
                    if (parentKey != "" && appMenuDict.Keys.Contains(parentKey))
                    {
                        var parent = appMenuDict[parentKey];
                        if (parent.Permission != null)
                        {
                            appMenu.Permission = parent.Permission.CreateChildPermission(appMenu.MENU_PERMISSION, L(name));
                        }
                        else
                        {
                            appMenu.Permission = pages.CreateChildPermission(appMenu.MENU_PERMISSION, L(name));
                        }
                    }
                    else
                    {
                        appMenu.Permission = pages.CreateChildPermission(appMenu.MENU_PERMISSION, L(name));
                    }

                    CreatePageWithToolbarPermissions(appMenu.Permission);
                    appMenu.Permission.Description = L("HasToolbar");
                }
            }
            catch
            {

            }
        }

        public static void CreatePageWithToolbarPermissions(Permission parentPermission)
        {
            var actions = typeof(gAMSProCorePermissions.Action)
              .GetFields(BindingFlags.Public | BindingFlags.Static)
              .Select(f => f.GetValue(null)).ToList();

            foreach (var action in actions)
            {
                var premissionName = parentPermission.Name + "." + action;
                parentPermission.CreateChildPermission(premissionName, L(action.ToString()));
            }
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
