using Abp.Modules;
using Abp.Reflection.Extensions;

namespace gAMSPro
{
    public class gAMSProCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProCoreSharedModule).GetAssembly());
        }
    }
}