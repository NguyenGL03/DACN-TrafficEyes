using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using gAMSPro.Authorization;

namespace gAMSPro
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(gAMSProApplicationSharedModule),
        typeof(gAMSProCoreModule),
        typeof(global::Core.gAMSPro.gAMSProCoreApplicationModule),
        typeof(global::Common.gAMSPro.gAMSProCommonApplicationModule)
    )]
    public class gAMSProApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProApplicationModule).GetAssembly());
        }
    }
}