using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.RequestTemplate.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;

namespace Common.gAMSPro.Intfs.RequestTemplate
{
    public interface IRequestTemplateAppService : IApplicationService
    {
        Task<PagedResultDto<CM_REQUEST_TEMPLATE_ENTITY>> CM_REQUEST_TEMPLATE_Search(CM_REQUEST_TEMPLATE_ENTITY input);
        Task<PagedResultDto<CM_REQUEST_TEMPLATE_ENTITY>> CM_REQUEST_TEMPLATE_Search_Admin(CM_REQUEST_TEMPLATE_ENTITY input);
        Task<PagedResultDto<CM_APPROVE_GROUP>> CM_WORKFLOW_APPROVE_Search(CM_APPROVE_GROUP input);
        Task<PagedResultDto<CM_APPROVE_GROUP>> CM_APPROVE_GROUP_Search(CM_APPROVE_GROUP input);
        Task<PagedResultDto<CM_TEMPLATE_NOTE>> CM_TEMPLATE_NOTE_Search(CM_TEMPLATE_NOTE input);
        Task<InsertResult> CM_TEMPLATE_NOTE_Ins(CM_TEMPLATE_NOTE input);
        Task<InsertResult> CM_TEMPLATE_FILE_NOTE_Ins(CM_TEMPLATE_NOTE input);
        Task<FileDto> CM_REQUEST_TEMPLATE_ToExcel(CM_REQUEST_TEMPLATE_ENTITY input);
        Task<InsertResult> CM_REQUEST_TEMPLATE_Ins(CM_REQUEST_TEMPLATE_ENTITY input);
        Task<InsertResult> CM_APPROVE_GROUP_Upd_Admin(string Req_id, List<string> GROUP_APPROVES, int CURRENT_STEP, string maker_id);
        Task<InsertResult> CM_REQUEST_TEMPLATE_Upd(CM_REQUEST_TEMPLATE_ENTITY input);
        Task<InsertResult> CM_REQUEST_TEMPLATE_Upd_Admin(CM_REQUEST_TEMPLATE_ENTITY input);
        Task<CommonResult> CM_REQUEST_TEMPLATE_Del(string id);
        Task<CommonResult> CM_REQUEST_TEMPLATE_App(string id, string currentUserName, string note);
        Task<CommonResult> CM_REQUEST_TEMPLATE_Replace_App(string id, string approveUserName, string checker_id, string note);
        Task<CommonResult> CM_REQUEST_TEMPLATE_Reject(string id, string currentUserName, string note);
        Task<CommonResult> CM_REQUEST_TEMPLATE_Handover(string id, string currentUserName, string usernameHandover);
        Task<CommonResult> CM_REQUEST_TEMPLATE_Authority(string id, string currentUserName, string usernameHandover);

        Task<CommonResult> CM_REQUEST_TEMPLATE_Sent_App(string id, string currentUserName);
        Task<CommonResult> CM_REQUEST_TEMPLATE_Check_Workflow(string id, string currentUserName);
        Task<CommonResult> CM_CHECK_USER_ROLE(string currentUserName);
        Task<CommonResult> CM_CHECK_USER_ROLE_HANDOVER(string currentUserName, string role, string templateId);

        Task<CM_REQUEST_TEMPLATE_ENTITY> CM_REQUEST_TEMPLATE_ById(string id);
        Task<List<CM_REQUEST_TEMPLATE_DETAIL_ENTITY>> CM_REQUEST_TEMPLATE_DETAIL_ByTemplateId(string templateId);
        Task<InsertResult> CM_REQUEST_TEMPLATE_DETAIL_Upd(CM_REQUEST_TEMPLATE_DETAIL_ENTITY info);
        Task<CM_REQUEST_TEMPLATE_DETAIL_ENTITY> CM_REQUEST_TEMPLATE_DETAIL_ById(string id);
        Task<CM_REQUEST_TEMPLATE_ENTITY> CM_REQUEST_TEMPLATE_ByCode(string templateCode);
        Task<CommonResult> CM_REQUEST_TEMPLATE_Get_Report_No(string dep_code);
        Task<CommonResult> CM_REQUEST_TEMPLATE_Add_Share_User(string users, string id, string currentUserName);
        Task<PagedResultDto<CM_TEMPALTE_LOG>> CM_TEMPLATE_LOG_Search(CM_APPROVE_GROUP input);

        Task<InsertResult> CM_REQUEST_TEMPLATE_Open_Template(CM_REQUEST_TEMPLATE_ENTITY input);
        Task<InsertResult> CM_REQUEST_TEMPLATE_Close_Template(CM_REQUEST_TEMPLATE_ENTITY input);
        Task<InsertResult> CM_REQUEST_TEMPLATE_Back_date(DateTime? REPORT_DT, string id, string maker_id);

    }
}
