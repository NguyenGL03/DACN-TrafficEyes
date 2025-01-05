using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Common.gAMSPro.Web.Core
{
    public class gAMSProCommonWebCoreModule : AbpModule
    {
        public gAMSProCommonWebCoreModule() { }

        public override void Initialize() { }

        public override void PreInitialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProCommonWebCoreModule).GetAssembly());
        }
    }
}
