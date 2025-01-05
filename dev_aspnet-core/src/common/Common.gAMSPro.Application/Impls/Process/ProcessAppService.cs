using Abp.Application.Services.Dto;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.Process.Dto;
using Common.gAMSPro.Process.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.CoreModule.Utils;

namespace Common.gAMSPro.Process
{
    public class ProcessAppService : gAMSProCoreAppServiceBase, IProcessAppService
    {

        #region Common
        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Search(CM_PROCESS_ENTITY input)
        {
            var items = await storeProcedureProvider
                              .GetDataFromStoredProcedure<CM_PROCESS_ENTITY>(CommonStoreProcedureConsts.CM_PROCESS_Search, input);

            return new PagedResultDto<CM_PROCESS_ENTITY>()
            {
                Items = items,
                TotalCount = items.FirstOrDefault()?.TotalCount ?? 0
            };
        }

        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Current_Search(CM_PROCESS_ENTITY input)
        {
            var items = await storeProcedureProvider.GetDataFromStoredProcedure<CM_PROCESS_ENTITY>(CommonStoreProcedureConsts.CM_PROCESS_Current_Search, input);

            return new PagedResultDto<CM_PROCESS_ENTITY>()
            {
                Items = items,
                TotalCount = items.FirstOrDefault()?.TotalCount ?? 0
            };
        }

        public async Task<List<PROCESS_ENTITY>> PL_PROCESS_REJECT_SEARCH(string reQ_ID, string type, string userLogin)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<PROCESS_ENTITY>(CommonStoreProcedureConsts.PL_PROCESS_REJECT_SEARCH, new
            {
                p_REQ_ID = reQ_ID,
                p_USER_LOGIN = userLogin,
                p_TYPE = type
            });

            return result;
        }

        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Reject(CM_PROCESS_ENTITY input)
        {
            var items = await storeProcedureProvider.GetDataFromStoredProcedure<CM_PROCESS_ENTITY>(CommonStoreProcedureConsts.CM_PROCESS_Reject, input);

            return new PagedResultDto<CM_PROCESS_ENTITY>()
            {
                Items = items,
                TotalCount = items.FirstOrDefault()?.TotalCount ?? 0
            };
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_InsertOrUpdate(CM_PROCESS_INS_UPD_ENTITY input)
        {
            return (await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.CM_PROCESS_InsertOrUpdate, new
            {
                PROCESS_KEY_INPUT = input.PROCESS_KEY,
                PROCESS_INS_UPD_ITEMS = (input.PROCESS_INS_UPD_ITEMS == null || input.PROCESS_INS_UPD_ITEMS.Count == 0)
                        ? null : XmlHelper.ToXmlFromList(input.PROCESS_INS_UPD_ITEMS),
                PROCESS_DEL_ITEMS = (input.PROCESS_DEL_ITEMS == null || input.PROCESS_DEL_ITEMS.Count == 0)
                        ? null : XmlHelper.ToXmlFromList(input.PROCESS_DEL_ITEMS)
            }));
        }

        public async Task<CM_PROCESS_ENTITY> CM_PROCESS_BYID(string pro_id)
        {
            var model = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_PROCESS_ENTITY>(CommonStoreProcedureConsts.CM_REJECT_LOG_ByID, new
            {
                PROCESS_ID = pro_id
            })).FirstOrDefault();

            return model;
        }

        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_ByProcess(string process)
        {
            var items = await storeProcedureProvider.GetDataFromStoredProcedure<CM_PROCESS_ENTITY>(CommonStoreProcedureConsts.CM_PROCESS_ByProcess, new
            {
                PROCESS_KEY = process
            });

            return new PagedResultDto<CM_PROCESS_ENTITY>()
            {
                Items = items,
                TotalCount = items.FirstOrDefault()?.TotalCount ?? 0
            };
        }
        public async Task<List<CM_PROCESS_STATUS_ENTITY>> CM_PROCESS_GetStatusByProcess(CM_PROCESS_STATUS_ENTITY input)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_PROCESS_STATUS_ENTITY>(CommonStoreProcedureConsts.CM_PROCESS_GetStatusByProcess, input);
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_DT_Insert_Is_Done(string id, string currentUserName, string reqId, bool? isDone, bool? isReject)
        {
            var result = await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.CM_PROCESS_DT_Insert_Is_Done, new
                {
                    ID = id,
                    REQ_ID = reqId,
                    TLNAME = currentUserName,
                    IS_DONE = isDone,
                    IS_REJECT = isReject
                });
            return result;
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_DT_Insert(string id, string currentUserName, string reqId)
        {

            var result = await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.CM_PROCESS_DT_Create, new
                {
                    ID = id,
                    REQ_ID = reqId,
                    TLNAME = currentUserName
                });
            if (result["Result"].Equals("0"))
            {
                const string nfMessageType = "CM_PROCESS_DT_Insert_LIQ";
                const string roleTifiType = "CM_PROCESS_DT_Insert_LIQ";
                await SendEmailAndNotify(id, "", nfMessageType, roleTifiType);
            }
            return result;
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_DT_Reject(int id, string currentUserName, string reqId, string auth_status, int order, bool isReject, string note)
        {

            var result = await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.CM_PROCESS_DT_Reject, new
            {
                ID = id,
                REQ_ID = reqId,
                TLNAME = currentUserName,
                IS_REJECT = isReject,
                NOTES = note,
                ORDER = order,
                AUTH_STATUS = auth_status
            });

            if (result["Result"].Equals("0"))
            {
                const string nfMessageType = "CM_PROCESS_DT_Reject_LIQ";
                const string roleTifiType = "CM_PROCESS_DT_Reject_LIQ";
                await SendEmailAndNotify(id.ToString(), "", nfMessageType, roleTifiType);
            }

            return result;
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_DT_Update(string id, string currentUserName, string reqId, bool isReject)
        {

            var result = await storeProcedureProvider.GetResultValueFromStore("CM_PROCESS_DT_Update", new
            {
                ID = id,
                REQ_ID = reqId,
                TLNAME = currentUserName,
                IS_REJECT = isReject
            });

            if (result["Result"].Equals("0"))
            {
                const string nfMessageType = "CM_PROCESS_DT_Update_LIQ";
                const string roleTifiType = "CM_PROCESS_DT_Update_LIQ";
                await SendEmailAndNotify(id, "", nfMessageType, roleTifiType);
            }

            return result;
        }

        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Check_create(CM_PROCESS_ENTITY input)
        {
            var items = await storeProcedureProvider.GetDataFromStoredProcedure<CM_PROCESS_ENTITY>("CM_PROCESS_CheckCreate", input);

            return new PagedResultDto<CM_PROCESS_ENTITY>()
            {
                Items = items,
                TotalCount = items.FirstOrDefault()?.TotalCount ?? 0
            };
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_GetHiddenField(string currentUserName, string reqId)
        {
            return (await storeProcedureProvider
               .GetResultValueFromStore(CommonStoreProcedureConsts.CM_PROCESS_GetHiddenField, new
               {
                   TLNAME = currentUserName,
                   REQ_ID = reqId
               }));
        }
        #endregion


        #region Màn hình quản lý quy trình
        public async Task<PagedResultDto<CM_PROCESS_LIST_ENTITY>> CM_PROCESS_LIST_Search(CM_PROCESS_LIST_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_PROCESS_LIST_ENTITY>(CommonStoreProcedureConsts.CM_PROCESS_LIST_Search, input);
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_LIST_Ins(CM_PROCESS_LIST_ENTITY input)
        {
            return (await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.CM_PROCESS_LIST_Ins, input));
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_LIST_Upd(CM_PROCESS_LIST_ENTITY input)
        {
            input.PROCESS_ITEMS = XmlHelper.ToXmlFromList(input.LIST_PROCESS_ITEMS);
            return (await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.CM_PROCESS_LIST_Upd, input));
        }

        public async Task<IDictionary<string, object>> CM_PROCESS_LIST_Del(string process_key)
        {
            return (await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.CM_PROCESS_LIST_Del, new { PROCESS_KEY = process_key }));
        }

        public async Task<CM_PROCESS_LIST_ENTITY> CM_PROCESS_LIST_ById(string process_key)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CM_PROCESS_LIST_ENTITY>(CommonStoreProcedureConsts.CM_PROCESS_LIST_ById, new { PROCESS_KEY = process_key }));
         
            return result.FirstOrDefault();
        }
        #endregion
    }
}