using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.Reflection.Extensions;
using gAMSPro.ApiClient;
using gAMSPro.Mobile.MAUI.Core.ApiClient;

namespace gAMSPro
{
    [DependsOn(typeof(gAMSProClientModule), typeof(AbpAutoMapperModule))]

    public class gAMSProMobileMAUIModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Localization.IsEnabled = false;
            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;

            Configuration.ReplaceService<IApplicationContext, MAUIApplicationContext>();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProMobileMAUIModule).GetAssembly());
        }
    }
}