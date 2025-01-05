using Abp.Application.Services;
using Common.gAMSPro.Intfs.RequestTemplate.Dto;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Intfs.ApproveGroup
{
    public interface IApproveGroupAppService : IApplicationService
    {
        Task<InsertResult> CM_APPROVE_GROUP_NEW_Ins(APPROVE_GROUP_ENTITY input);
        Task<InsertResult> CM_APPROVE_GROUP_NEW_Upd(APPROVE_GROUP_ENTITY input);
        Task<List<string>> CM_APPROVE_GROUP_NEW_ById(string reqId);
        Task<List<string>> CM_APPROVE_GROUP_BY_TITLE_ID(string reqId);
        Task<List<CM_APPROVE_GROUP>> CM_APPROVE_GROUP_WORKFLOW_ById(string reqId);
        Task<CommonResult> CM_APPROVE_GROUP_SEND_App(string reqId);
        Task<IDictionary<string, object>> CM_APPROVE_GROUP_App(string reqId, string note, string location);
        Task<CommonResult> CM_APPROVE_GROUP_Reject(string reqId, string note);
        Task<CommonResult> CM_APPROVE_GROUP_Authority(string reqId, string usernameAuthority);
        Task<List<CM_TEMPLATE_NOTE>> CM_APPROVE_GROUP_NOTE_ById(string reqId);
        Task<InsertResult> CM_APPROVE_GROUP_NOTE_Ins(CM_TEMPLATE_NOTE input);
    }
}
