using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.Windsor.MsDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using gAMSPro.Configure;
using gAMSPro.Startup;
using gAMSPro.Test.Base;

namespace gAMSPro.GraphQL.Tests
{
    [DependsOn(
        typeof(gAMSProGraphQLModule),
        typeof(gAMSProTestBaseModule))]
    public class gAMSProGraphQLTestModule : AbpModule
    {
        public override void PreInitialize()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddAndConfigureGraphQL();

            WindsorRegistrationHelper.CreateServiceProvider(IocManager.IocContainer, services);
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(gAMSProGraphQLTestModule).GetAssembly());
        }
    }
}