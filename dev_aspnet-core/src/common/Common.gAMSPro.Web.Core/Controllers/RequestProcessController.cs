using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.Process.Dto;
using Common.gAMSPro.Intfs.RequestProcess;
using Common.gAMSPro.Intfs.RequestProcess.Dto;
using Common.gAMSPro.Process.Dto;
using Common.gAMSPro.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controler
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class RequestProcessController : CoreAmsProControllerBase
    {
        private readonly IRequestProcessAppService _requestProcessAppService;
        public RequestProcessController(IRequestProcessAppService requestDocAppService)
        {
            _requestProcessAppService = requestDocAppService;
        }

        [HttpGet]
        public async Task<List<REQUEST_PROCESS_ENTITY>> PROCESS_CURRENT_SEARCH(string reQ_ID, string type, string userLogin)
        {
            return await _requestProcessAppService.PROCESS_CURRENT_SEARCH(reQ_ID, type, userLogin);
        }
        [HttpGet]
        public async Task<List<PROCESS_ENTITY>> PROCESS_SEARCH(string reQ_ID, string type, string userLogin)
        {
            return await _requestProcessAppService.PROCESS_SEARCH(reQ_ID, type, userLogin);
        }
    }
}
