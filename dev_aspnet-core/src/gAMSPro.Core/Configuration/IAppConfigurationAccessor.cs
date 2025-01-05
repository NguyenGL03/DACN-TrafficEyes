using Microsoft.Extensions.Configuration;

namespace gAMSPro.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
