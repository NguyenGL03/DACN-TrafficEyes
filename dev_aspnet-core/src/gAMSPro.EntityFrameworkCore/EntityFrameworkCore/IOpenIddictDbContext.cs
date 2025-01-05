using Microsoft.EntityFrameworkCore;
using gAMSPro.OpenIddict.Applications;
using gAMSPro.OpenIddict.Authorizations;
using gAMSPro.OpenIddict.Scopes;
using gAMSPro.OpenIddict.Tokens;

namespace gAMSPro.EntityFrameworkCore
{
    public interface IOpenIddictDbContext
    {
        DbSet<OpenIddictApplication> Applications { get; }

        DbSet<OpenIddictAuthorization> Authorizations { get; }

        DbSet<OpenIddictScope> Scopes { get; }

        DbSet<OpenIddictToken> Tokens { get; }
    }

}