using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.CommonProcedure;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class CommonProcedureController : CoreAmsProControllerBase
    {
        private readonly ICommonProcedureAppService commonProcedure;
        public CommonProcedureController(ICommonProcedureAppService commonProcedure)
        {
            this.commonProcedure = commonProcedure;
        }

        [HttpPost]
        public async Task<List<object>> GetDataFromStoredProcedure(string stored, [FromBody] object input)
        {
            return await commonProcedure.GetDataFromStoredProcedure
                (stored, input);
        }

        [HttpPost]
        public async Task<PagedResultDto<object>> GetPagingData(string stored, [FromBody] object input)
        {
            return await commonProcedure.GetPagingData(stored, input);
        }

    }
}
