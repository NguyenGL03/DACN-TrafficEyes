using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Reject;
using Common.gAMSPro.Reject.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class RejectController : CoreAmsProControllerBase
    {
        readonly IRejectAppService rejectAppService;

        public RejectController(IRejectAppService rejectAppService)
        {
            this.rejectAppService = rejectAppService;
        }


        [HttpPost]
        public async Task<PagedResultDto<CM_REJECT_LOG_ENTITY>> CM_REJECT_Search([FromBody] CM_REJECT_LOG_ENTITY input)
        {
            return await rejectAppService.CM_REJECT_LOG_Search(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_REJECT_Ins([FromBody] CM_REJECT_LOG_ENTITY input)
        {
            return await rejectAppService.CM_REJECT_LOG_Ins(input);
        }



        [HttpGet]
        public async Task<CM_REJECT_LOG_ENTITY> CM_REJECT_ById(string trd_id, string stage)
        {
            return await rejectAppService.CM_REJECT_LOG_ById(trd_id, stage);
        }

        [HttpGet]
        public async Task<List<CM_REJECT_LOG_ENTITY>> CM_REJECT_LOG_Hist(string TRN_ID, string STAGE)
        {
            return await rejectAppService.CM_REJECT_LOG_Hist(TRN_ID, STAGE);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_REJECT_LOG_ByType_Ins([FromBody] CM_REJECT_LOG_ENTITY input)
        {
            return await rejectAppService.CM_REJECT_LOG_ByType_Ins(input);
        }


        [HttpGet]
        public async Task<List<CM_REJECT_PROCESS_ENTITY>> CM_REJECT_PROCESS_Search(string reQ_ID, string type, string userLogin)
        {
            return await rejectAppService.CM_REJECT_PROCESS_Search(reQ_ID, type, userLogin);
        }
    }
}
