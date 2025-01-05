using Abp.Modules;
using Abp.Reflection.Extensions;

namespace gAMSPro
{
    [DependsOn(typeof(gAMSProCoreSharedModule))]
    public class gAMSProApplicationSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProApplicationSharedModule).GetAssembly());
        }
    }
}