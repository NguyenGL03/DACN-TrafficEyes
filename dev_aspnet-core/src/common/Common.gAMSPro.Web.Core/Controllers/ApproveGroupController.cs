using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.ApproveGroup;
using Common.gAMSPro.Intfs.RequestTemplate.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;
namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]

    public class ApproveGroupController : CoreAmsProControllerBase
    {
        private readonly IApproveGroupAppService ApproveGroupAppService;

        public ApproveGroupController(IApproveGroupAppService ApproveGroupAppService)
        {
            this.ApproveGroupAppService = ApproveGroupAppService;
        }

        [HttpPost]
        public async Task<InsertResult> CM_APPROVE_GROUP_NEW_Ins([FromBody] APPROVE_GROUP_ENTITY input)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_NEW_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_APPROVE_GROUP_NEW_Upd([FromBody] APPROVE_GROUP_ENTITY input)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_NEW_Upd(input);
        }

        [HttpGet]
        public async Task<List<String>> CM_APPROVE_GROUP_NEW_ById(string reqId)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_NEW_ById(reqId);
        }

        [HttpGet]
        public async Task<List<String>> CM_APPROVE_GROUP_BY_TITLE_ID(string reqId)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_BY_TITLE_ID(reqId);
        }

        [HttpGet]
        public async Task<List<CM_APPROVE_GROUP>> CM_APPROVE_GROUP_WORKFLOW_ById(string reqId)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_WORKFLOW_ById(reqId);
        }

        [HttpPost]
        public async Task<CommonResult> CM_APPROVE_GROUP_SEND_App(string reqId)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_SEND_App(reqId);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_APPROVE_GROUP_App(string reqId, string location, [FromBody] string note)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_App(reqId, note, location);
        }

        [HttpPost]
        public async Task<CommonResult> CM_APPROVE_GROUP_Reject(string reqId, [FromBody] string note)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_Reject(reqId, note);
        }

        [HttpPost]
        public async Task<CommonResult> CM_APPROVE_GROUP_Authority(string reqId, string usernameAuthority)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_Authority(reqId, usernameAuthority);
        }

        [HttpGet]
        public async Task<List<CM_TEMPLATE_NOTE>> CM_APPROVE_GROUP_NOTE_ById(string reqId)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_NOTE_ById(reqId);
        }

        [HttpPost]
        public async Task<InsertResult> CM_APPROVE_GROUP_NOTE_Ins([FromBody] CM_TEMPLATE_NOTE input)
        {
            return await ApproveGroupAppService.CM_APPROVE_GROUP_NOTE_Ins(input);
        }
    }
}
