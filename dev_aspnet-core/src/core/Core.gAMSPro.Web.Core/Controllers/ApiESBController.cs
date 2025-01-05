using Abp.AspNetCore.Mvc.Controllers;
using Core.gAMSPro.Intfs.ApiESB;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace Core.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ApiESBController : AbpController
    {
        private readonly IApiESBAppService apiESBAppService;
        private static readonly ILog log = LogManager.GetLogger(typeof(ApiESBController));

        public ApiESBController(IApiESBAppService apiESBAppService)
        {
            this.apiESBAppService = apiESBAppService;
        }
        [HttpPost]
        public async Task<IDictionary<string, object>> AccountingSync(string id)
        {
            try
            {
                return await apiESBAppService.AccountingSync(id);
            }
            catch (Exception ex)
            {
                Dictionary<string, object> error = new Dictionary<string, object>();
                error.Add("Result", "-1");
                return error;
            }
        }
    }
}
