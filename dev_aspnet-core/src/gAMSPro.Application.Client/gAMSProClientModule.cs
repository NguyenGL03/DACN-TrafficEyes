using Abp.Modules;
using Abp.Reflection.Extensions;

namespace gAMSPro
{
    public class gAMSProClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProClientModule).GetAssembly());
        }
    }
}
