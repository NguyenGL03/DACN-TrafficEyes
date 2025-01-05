using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Dependency;
using Abp.Runtime.Caching;
using Abp.Runtime.Caching.Memory;
using Common.gAMSPro.Core.Authorization;
using gAMSPro.Authorization;
using gAMSPro.Caching.Dto;
using GSOFTcore.gAMSPro.Authorization.Permissions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace gAMSPro.Caching
{
    //[AbpAuthorize(AppPermissions.Pages_Administration_Host_Maintenance)]
    public class CachingAppService : gAMSProAppServiceBase, ICachingAppService
    {
        private readonly ICacheManager _cacheManager;
        private readonly IPermissionAppHelper _permissionAppHelper;

        public CachingAppService(ICacheManager cacheManager, IPermissionAppHelper permissionAppHelper)
        {
            _cacheManager = cacheManager;
            _permissionAppHelper = permissionAppHelper;
        }

        public ListResultDto<CacheDto> GetAllCaches()
        {
            var caches = _cacheManager.GetAllCaches()
                                        .Select(cache => new CacheDto
                                        {
                                            Name = cache.Name
                                        })
                                        .ToList();

            return new ListResultDto<CacheDto>(caches);
        }

        public async Task ClearCache(EntityDto<string> input)
        {
            var cache = _cacheManager.GetCache(input.Id);
            await cache.ClearAsync();
        }

        public async Task ClearAllCaches()
        {
            if (!CanClearAllCaches())
            {
                throw new ApplicationException("This method can be used only with Memory Cache!");
            }

            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }
        }

        public bool CanClearAllCaches()
        {
            return _cacheManager.GetType() == typeof(AbpMemoryCacheManager);
        }
        public void ClearPermissionAndRole()
        {
            try
            {
                var permission = IocManager.Instance.IocContainer.Resolve<gAMSProCommonAuthorizationProvider>();
                var permissionBase = IocManager.Instance.IocContainer.Resolve<AppAuthorizationProvider>();

                var allPermissions = PermissionManager.GetAllPermissions();

                if (allPermissions != null)
                {
                    foreach (var p in allPermissions)
                    {
                        _permissionAppHelper.RemovePermission(p.Name);
                    }
                }

                var permissionDict = ((Dictionary<string, Permission>)PermissionManager.GetType().GetField("Permissions", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public).GetValue(PermissionManager));
                var method = permissionDict.GetType().GetMethod("AddAllPermissions");

                permissionBase.SetPermissions((IPermissionDefinitionContext)PermissionManager);
                permission.SetPermissions((IPermissionDefinitionContext)PermissionManager);
                method.Invoke(permissionDict, null);


            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}