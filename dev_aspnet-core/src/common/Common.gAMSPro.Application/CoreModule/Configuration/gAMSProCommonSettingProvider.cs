using Abp.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;


namespace Common.gAMSPro.Configuration
{
    public class gAMSProCommonSettingProvider : SettingProvider
    {
        private readonly IConfigurationRoot _appConfiguration;

        public gAMSProCommonSettingProvider(IWebHostEnvironment env)
        {
            _appConfiguration = env.GetAppConfiguration();
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            return new List<SettingDefinition>();
        }
    }
}
