using Abp.Modules;
using Abp.Reflection.Extensions;
using Common.gAMSPro.Configuration;

namespace Common.gAMSPro
{
    public class gAMSProCommonModule : AbpModule
    {

        public gAMSProCommonModule()
        {
        }

        public override void Initialize()
        {
        }

        public override void PreInitialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProCommonModule).GetAssembly());
            Configuration.Settings.Providers.Add<gAMSProCommonSettingProvider>();
        }
    }
}