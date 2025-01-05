using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Core.gAMSPro.Web.Core
{
    public class gAMSProCoreWebCoreModule : AbpModule
    {
        public gAMSProCoreWebCoreModule() { }

        public override void Initialize()
        {

        }

        public override void PreInitialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProCoreWebCoreModule).GetAssembly());
        }
    }
}
