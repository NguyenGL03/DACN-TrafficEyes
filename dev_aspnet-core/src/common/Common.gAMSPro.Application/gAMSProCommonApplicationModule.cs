using Abp.AutoMapper;
using Abp.EntityFrameworkCore;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Common.gAMSPro.Core.Authorization;
//using Common.gAMSPro.CoreModule.EfCore;

namespace Common.gAMSPro
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(AbpEntityFrameworkCoreModule)
    )]
    public class gAMSProCommonApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<gAMSProCommonAuthorizationProvider>();

            //Adding custom AutoMapper configuration 
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProCommonApplicationModule).GetAssembly());
        }
    }
}

