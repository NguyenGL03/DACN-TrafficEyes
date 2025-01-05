using Abp.Modules;
using Abp.Reflection.Extensions;
using Core.gAMSPro.Configuration;

namespace Core.gAMSPro
{
    public class gAMSProCoreModule : AbpModule
    {
        public gAMSProCoreModule() { }

        public override void Initialize()
        {
        }

        public override void PreInitialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProCoreModule).GetAssembly());
            Configuration.Settings.Providers.Add<gAMSProCoreSettingProvider>();
        }
    }
}