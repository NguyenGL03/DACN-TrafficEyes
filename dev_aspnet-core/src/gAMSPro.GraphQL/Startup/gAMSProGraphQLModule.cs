using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace gAMSPro.Startup
{
    [DependsOn(typeof(gAMSProCoreModule))]
    public class gAMSProGraphQLModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProGraphQLModule).GetAssembly());
        }

        public override void PreInitialize()
        {
            base.PreInitialize();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
        }
    }
}