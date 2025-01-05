using Abp.Application.Services.Dto;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Reject.Dto;
using Core.gAMSPro.Application;
using System.Linq.Dynamic.Core;

namespace Common.gAMSPro.Reject
{
    public class RejectAppService : gAMSProCoreAppServiceBase, IRejectAppService
    {

        #region Public Method

        public async Task<PagedResultDto<CM_REJECT_LOG_ENTITY>> CM_REJECT_LOG_Search(CM_REJECT_LOG_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_REJECT_LOG_ENTITY>(CommonStoreProcedureConsts.CM_REJECT_LOG_SEARCH, input);
        }

        public async Task<IDictionary<string, object>> CM_REJECT_LOG_Ins(CM_REJECT_LOG_ENTITY input)
        {
            //SetAuditForInsert(input);
            var result = (await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.CM_REJECT_LOG_INS, input));

            // Trả về NT
            if (result["Result"].Equals("0") && input.IS_SEND_MAIL == true && (input.STAGE == "HC" || input.STAGE == "KT_HC"))
            {
                // NF_MESSAGE_GetContent
                const string nfMessageType = "ASS_REJECT_NT";
                // TR_ROLE_NOTIFI_ID
                const string roleTifiType = "ASS_SEND_NT";
                await SendEmailAndNotify(input.TRN_ID, null, nfMessageType, roleTifiType);
            }
            //Trả về GDV
            else if (result["Result"].Equals("0") && input.IS_SEND_MAIL == true && input.STAGE == "KT")
            {
                // NF_MESSAGE_GetContent
                const string nfMessageType = "ASS_REJECT_GDV";
                // TR_ROLE_NOTIFI_ID
                const string roleTifiType = "ASS_REJECT_GDV";
                await SendEmailAndNotify(input.TRN_ID, null, nfMessageType, roleTifiType);
            }

            return result;
        }

        public async Task<CM_REJECT_LOG_ENTITY> CM_REJECT_LOG_ById(string trn_id, string stage)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_REJECT_LOG_ENTITY>(CommonStoreProcedureConsts.CM_REJECT_LOG_ByID, new
            {
                TRN_ID = trn_id,
                STAGE = stage
            })).FirstOrDefault();

            return model;
        }

        public async Task<List<CM_REJECT_LOG_ENTITY>> CM_REJECT_LOG_Hist(string TRN_ID, string STAGE)
        {
            return (await storeProcedureProvider.GetDataFromStoredProcedure<CM_REJECT_LOG_ENTITY>(CommonStoreProcedureConsts.CM_REJECT_LOG_Hist, new
            {
                p_TRN_ID = TRN_ID,
                p_STAGE = STAGE,
            }));
        }

        public async Task<IDictionary<string, object>> CM_REJECT_LOG_ByType_Ins(CM_REJECT_LOG_ENTITY input)
        {
            var result = (await storeProcedureProvider
               .GetResultValueFromStore(CommonStoreProcedureConsts.CM_REJECT_LOG_ByType_Ins, input));

            if (result["Result"].Equals("0") && input.TYPE == "CREATE")
            {
                // NF_MESSAGE_GetContent
                const string nfMessageType = "CM_REJECT_LOG_ByType_Ins_CREATE";
                // TR_ROLE_NOTIFI_ID
                const string roleTifiType = "CM_REJECT_LOG_ByType_Ins_CREATE";
                await SendEmailAndNotify(input.TRN_ID.ToString(), null, nfMessageType, roleTifiType);
            }
            else if (result["Result"].Equals("0"))
            {
                // NF_MESSAGE_GetContent
                const string nfMessageType = "CM_REJECT_LOG_ByType_Ins_NVXL";
                // TR_ROLE_NOTIFI_ID
                const string roleTifiType = "CM_REJECT_LOG_ByType_Ins_NVXL";
                await SendEmailAndNotify(input.TRN_ID.ToString(), null, nfMessageType, roleTifiType);
            }

            return result;
        }

        public async Task<List<CM_REJECT_PROCESS_ENTITY>> CM_REJECT_PROCESS_Search(string reQ_ID, string type, string userLogin)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<CM_REJECT_PROCESS_ENTITY>(CommonStoreProcedureConsts.PL_PROCESS_REJECT_SEARCH, new
            {
                p_REQ_ID = reQ_ID,
                p_USER_LOGIN = userLogin,
                p_TYPE = type
            });

            return result;
        }

        #endregion
    }
}
