using Abp.AspNetCore.Mvc.Controllers;
using Abp.Dependency;
using Microsoft.AspNetCore.Http;

namespace Common.gAMSPro.Web.Controllers
{
    public class CoreAmsProControllerBase : AbpController
    {
        protected static IHttpContextAccessor httpContextAccessor;
        public CoreAmsProControllerBase()
        {
            httpContextAccessor = IocManager.Instance.IocContainer.Resolve<IHttpContextAccessor>();
        }
    }
}
