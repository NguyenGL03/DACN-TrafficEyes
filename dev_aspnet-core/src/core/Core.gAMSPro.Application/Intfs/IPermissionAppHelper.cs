using Abp.Application.Features;
using Abp.Authorization;
using Abp.MultiTenancy;

namespace GSOFTcore.gAMSPro.Authorization.Permissions
{
    public interface IPermissionAppHelper
    {
        Task<Permission> CreatePermission(string name, string displayName = null, string description = null, MultiTenancySides multiTenancySides = MultiTenancySides.Tenant | MultiTenancySides.Host, IFeatureDependency featureDependency = null);

        // Token: 0x06000C78 RID: 3192 RVA: 0x0001740C File Offset: 0x0001560C
        Permission GetPermissionOrNull(string name);

        Task AddToDictPermission(Permission permission);

        // Token: 0x06000C79 RID: 3193 RVA: 0x0001741A File Offset: 0x0001561A
        void RemovePermission(string name);
    }
}
