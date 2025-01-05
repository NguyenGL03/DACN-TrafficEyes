using Abp.Zero.Ldap.Authentication;
using Abp.Zero.Ldap.Configuration;
using gAMSPro.Authorization.Users;
using gAMSPro.MultiTenancy;

namespace gAMSPro.Authorization.Ldap
{
    public class AppLdapAuthenticationSource : LdapAuthenticationSource<Tenant, User>
    {
        public AppLdapAuthenticationSource(ILdapSettings settings, IAbpZeroLdapModuleConfig ldapModuleConfig)
            : base(settings, ldapModuleConfig)
        {
        }
    }
}