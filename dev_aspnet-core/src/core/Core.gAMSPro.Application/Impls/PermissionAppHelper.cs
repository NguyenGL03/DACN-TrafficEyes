using Abp.Application.Features;
using Abp.Application.Services;
using Abp.Authorization;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Runtime.Validation;
using Common.gAMSPro.Core.Authorization;
using Core.gAMSPro.Core.Authorization;
using gAMSPro.Authorization.Roles;
using gAMSPro.Core.Authorization.Permissions;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GSOFTcore.gAMSPro.Authorization.Permissions
{
    [AbpAuthorize]
    public class PermissionAppHelper : ApplicationService, IPermissionAppHelper
    {
        private readonly RoleManager roleManager;

        public PermissionAppHelper(RoleManager roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<Permission> CreatePermission(string name, string displayName = null, string description = null, MultiTenancySides multiTenancySides = MultiTenancySides.Tenant | MultiTenancySides.Host, IFeatureDependency featureDependency = null)
        {
            //Get parent permission and add like a child permission
            var pages = GetPermissionOrNull(gAMSProCorePermissions.Pages);
            var childPermission = pages.CreateChildPermission(name, L(displayName));
            await AddToDictPermission(childPermission);
            return childPermission;
        }

        public async Task AddToDictPermission(Permission permission)
        {
            //Add child permission to root list permission
            var permissionDict = ((Dictionary<string, Permission>)PermissionManager.GetType().GetField("Permissions", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(PermissionManager));
            permissionDict.Add(permission.Name, permission);

            var lstPermission = PermissionManager.GetAllPermissions().AsQueryable().Select(x => x.Name).ToList();

            var adminRole = roleManager.Roles.Where(x => x.Name.ToLower() == "admin" && x.IsStatic == true).FirstOrDefault();

            await UpdateGrantedPermissions(adminRole, lstPermission);
        }

        private async Task UpdateGrantedPermissions(Role role, List<string> grantedPermissionNames)
        {
            var grantedPermissions = GetPermissionsFromNamesByValidating(PermissionManager, grantedPermissionNames);
            await roleManager.SetGrantedPermissionsAsync(role, grantedPermissions);
        }
        private static IEnumerable<Permission> GetPermissionsFromNamesByValidating(IPermissionManager permissionManager, IEnumerable<string> permissionNames)
        {
            var permissions = new List<Permission>();
            var undefinedPermissionNames = new List<string>();

            foreach (var permissionName in permissionNames)
            {
                var permission = permissionManager.GetPermissionOrNull(permissionName);
                if (permission == null)
                {
                    undefinedPermissionNames.Add(permissionName);
                }

                permissions.Add(permission);
            }

            if (undefinedPermissionNames.Count > 0)
            {
                throw new AbpValidationException($"There are {undefinedPermissionNames.Count} undefined permission names.")
                {
                    ValidationErrors = undefinedPermissionNames.Select(permissionName => new ValidationResult("Undefined permission: " + permissionName)).ToList()
                };
            }

            return permissions;
        }
        public Permission GetPermissionOrNull(string name)
        {
            var createMethod = PermissionManager.GetType().GetMethod(PermissionConst.GetPermissionOrNull);

            return (Permission)createMethod.Invoke(PermissionManager, new object[] { name });
        }

        public void RemovePermission(string name)
        {
            //Remove permission in root list permission
            try
            {
                var createMethod = PermissionManager.GetType().GetMethod(PermissionConst.RemovePermission);

                createMethod.Invoke(PermissionManager, new object[] { name });

                //Remove child permission from parent node
                Permission permissionParent = new List<Permission>(PermissionManager.GetAllPermissions()).Find(x => x.Name == "gAMSProCorePermissions.Pages");
                if (permissionParent != null)
                {
                    permissionParent.RemoveChildPermission(name);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, gAMSProConsts.LocalizationSourceName);
        }
    }
}
