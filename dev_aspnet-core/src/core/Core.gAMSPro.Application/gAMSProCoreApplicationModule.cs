using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Core.gAMSPro.Core.Authorization;

namespace Core.gAMSPro
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    public class gAMSProCoreApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<gAMSProCoreAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
            Configuration.Auditing.IsEnabled = true;
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProCoreApplicationModule).GetAssembly());
        }
    }
}

