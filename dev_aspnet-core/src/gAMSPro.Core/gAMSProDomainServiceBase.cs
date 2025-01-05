using Abp.Domain.Services;

namespace gAMSPro
{
    public abstract class gAMSProDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected gAMSProDomainServiceBase()
        {
            LocalizationSourceName = gAMSProConsts.LocalizationSourceName;
        }
    }
}
