using Abp.Runtime.Caching;
using gAMSPro.Authentication.TwoFactor;

namespace gAMSPro.Web.Authentication.TwoFactor
{
    public static class TwoFactorCodeCacheExtensions
    {
        public static ITypedCache<string, TwoFactorCodeCacheItem> GetTwoFactorCodeCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, TwoFactorCodeCacheItem>(TwoFactorCodeCacheItem.CacheName);
        }
    }
}
