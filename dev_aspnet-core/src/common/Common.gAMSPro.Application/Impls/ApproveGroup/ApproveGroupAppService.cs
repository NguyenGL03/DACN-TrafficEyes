using Abp.Authorization;
using Common.gAMSPro.Intfs.ApproveGroup;
using Common.gAMSPro.Intfs.RequestTemplate.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Impls.ApproveGroup
{
    [AbpAuthorize]
    public class ApproveGroupAppService : gAMSProCoreAppServiceBase, IApproveGroupAppService
    {


        public ApproveGroupAppService()
        {
        }

        #region Public Method

        public async Task<InsertResult> CM_APPROVE_GROUP_NEW_Ins(APPROVE_GROUP_ENTITY input)
        {
            if (input.GROUP_APPROVE != null)
            {
                input.GROUP_APPROVES = string.Join(",", input.GROUP_APPROVE);
            }
            if (input.TITLE != null)
            {
                input.TITLES = string.Join(",", input.TITLE);
            }
            return (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>("CM_APPROVE_GROUP_NEW_Ins", input)).FirstOrDefault();
        }

        public async Task<InsertResult> CM_APPROVE_GROUP_NEW_Upd(APPROVE_GROUP_ENTITY input)
        {
            if (input.GROUP_APPROVE != null)
            {
                input.GROUP_APPROVES = string.Join(",", input.GROUP_APPROVE);
            }
            if (input.TITLE != null)
            {
                input.TITLES = string.Join(",", input.TITLE);
            }
            return (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>("CM_APPROVE_GROUP_NEW_Upd", input)).FirstOrDefault();
        }

        public async Task<List<string>> CM_APPROVE_GROUP_NEW_ById(string reqId)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<string>("CM_APPROVE_GROUP_NEW_ById", new
            {
                p_REQ_ID = reqId
            })).ToList<string>();

            return result;
        }

        public async Task<List<string>> CM_APPROVE_GROUP_BY_TITLE_ID(string reqId)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<string>("CM_APPROVE_GROUP_BY_TITLE_ID", new
            {
                p_REQ_ID = reqId
            })).ToList<string>();

            return result;
        }

        public async Task<List<CM_APPROVE_GROUP>> CM_APPROVE_GROUP_WORKFLOW_ById(string reqId)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_APPROVE_GROUP>("CM_APPROVE_GROUP_WORKFLOW_ById", new
            {
                p_REQ_ID = reqId
            }));

            return result;
        }

        public async Task<CommonResult> CM_APPROVE_GROUP_SEND_App(string reqId)
        {
            var result = (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>("CM_APPROVE_GROUP_SEND_App", new
                {
                    P_REQ_ID = reqId,
                    P_CHECKER_ID = GetCurrentUserName(),
                    P_APPROVE_DT = GetCurrentDateTime()
                })).FirstOrDefault();

            if (result.Result.Equals("0"))
            {
                // NF_MESSAGE_GetContent
                const string nfMessageType = "APPROVE_GROUP_SEND_App";
                // TR_ROLE_NOTIFI_ID
                const string roleTifiType = "APPROVE_GROUP_SEND_App";
                await SendEmailAndNotify(reqId, null, nfMessageType, roleTifiType);
            }

            return result;
        }

        public async Task<IDictionary<string, object>> CM_APPROVE_GROUP_App(string reqId, string note, string location)
        {
            var result = await storeProcedureProvider.GetResultValueFromStore("CM_APPROVE_GROUP_App", new
            {
                P_REQ_ID = reqId,
                P_CHECKER_ID = GetCurrentUserName(),
                P_APPROVE_DT = GetCurrentDateTime(),
                P_NOTE = note,
                P_LOCATION = location
            });

            if (result["Result"].Equals("0"))
            {
                // NF_MESSAGE_GetContent
                const string nfMessageType = "APPROVE_GROUP_App";
                // TR_ROLE_NOTIFI_ID
                const string roleTifiType = "APPROVE_GROUP_App";
                await SendEmailAndNotify(reqId, null, nfMessageType, roleTifiType);
            }

            return result;
        }

        public async Task<CommonResult> CM_APPROVE_GROUP_Reject(string reqId, string note)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>("CM_APPROVE_GROUP_Reject", new
            {
                P_REQ_ID = reqId,
                P_CHECKER_ID = GetCurrentUserName(),
                P_APPROVE_DT = GetCurrentDateTime(),
                P_NOTE = note
            })).FirstOrDefault();

            if (result.Result.Equals("0"))
            {
                // NF_MESSAGE_GetContent
                const string nfMessageType = "APPROVE_GROUP_Reject";
                // TR_ROLE_NOTIFI_ID
                const string roleTifiType = "APPROVE_GROUP_Reject";
                await SendEmailAndNotify(reqId, null, nfMessageType, roleTifiType);
            }

            return result;
        }

        public async Task<CommonResult> CM_APPROVE_GROUP_Authority(string reqId, string usernameAuthority)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>("CM_APPROVE_GROUP_Authority", new
            {
                P_REQ_ID = reqId,
                P_AUTHORITY_USERNAME = usernameAuthority,
                P_APPROVE_USERNAME = GetCurrentUserName(),
                P_CHECKER_ID = GetCurrentUserName(),
                P_APPROVE_DT = GetCurrentDateTime()
            })).FirstOrDefault();

            if (result.Result.Equals("0"))
            {
                // NF_MESSAGE_GetContent
                const string nfMessageType = "APPROVE_GROUP_Authority";
                // TR_ROLE_NOTIFI_ID
                const string roleTifiType = "APPROVE_GROUP_Authority";
                await SendEmailAndNotify(reqId, null, nfMessageType, roleTifiType);
            }

            return result;
        }

        public async Task<List<CM_TEMPLATE_NOTE>> CM_APPROVE_GROUP_NOTE_ById(string reqId)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_TEMPLATE_NOTE>("CM_APPROVE_GROUP_NOTE_ById", new
            {
                P_REQ_ID = reqId
            });

            return result;
        }

        public async Task<InsertResult> CM_APPROVE_GROUP_NOTE_Ins(CM_TEMPLATE_NOTE input)
        {
            SetAuditForInsert(input);
            input.MAKER_ID = GetCurrentUserName();
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<InsertResult>("CM_APPROVE_GROUP_NOTE_Ins", input)).FirstOrDefault();
            return result;
        }
        #endregion

        #region Private Method
        #endregion

    }
}
