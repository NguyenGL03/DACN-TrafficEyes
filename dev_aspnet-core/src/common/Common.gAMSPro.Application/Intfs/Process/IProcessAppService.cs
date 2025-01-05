using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.Process.Dto;
using Common.gAMSPro.Process.Dto;


namespace Common.gAMSPro.Process
{
    public interface IProcessAppService : IApplicationService
    {
        #region Common
        Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Search(CM_PROCESS_ENTITY input);
        Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Reject(CM_PROCESS_ENTITY input);
        Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Current_Search(CM_PROCESS_ENTITY input);
        Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_ByProcess(string process);
        Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Check_create(CM_PROCESS_ENTITY input);
        Task<CM_PROCESS_ENTITY> CM_PROCESS_BYID(string pro_id);
        Task<List<PROCESS_ENTITY>> PL_PROCESS_REJECT_SEARCH(string reQ_ID, string type, string userLogin);
        Task<List<CM_PROCESS_STATUS_ENTITY>> CM_PROCESS_GetStatusByProcess(CM_PROCESS_STATUS_ENTITY input);
        Task<IDictionary<string, object>> CM_PROCESS_DT_Insert_Is_Done(string id, string currentUserName, string reqId, bool? isDone, bool? isReject);
        Task<IDictionary<string, object>> CM_PROCESS_DT_Insert(string id, string currentUserName, string reqId);
        Task<IDictionary<string, object>> CM_PROCESS_DT_Reject(int id, string currentUserName, string reqId, string auth_status, int order, bool isReject, string notes);
        Task<IDictionary<string, object>> CM_PROCESS_DT_Update(string id, string currentUserName, string reqId, bool isReject);
        Task<IDictionary<string, object>> CM_PROCESS_GetHiddenField(string currentUserName, string reqId);
        Task<IDictionary<string, object>> CM_PROCESS_InsertOrUpdate(CM_PROCESS_INS_UPD_ENTITY input);
        #endregion

        #region Màn hình quản lý quy trình
        Task<PagedResultDto<CM_PROCESS_LIST_ENTITY>> CM_PROCESS_LIST_Search(CM_PROCESS_LIST_ENTITY input);
        Task<IDictionary<string, object>> CM_PROCESS_LIST_Ins(CM_PROCESS_LIST_ENTITY input);
        Task<IDictionary<string, object>> CM_PROCESS_LIST_Upd(CM_PROCESS_LIST_ENTITY input);
        Task<IDictionary<string, object>> CM_PROCESS_LIST_Del(string process_key); 
        Task<CM_PROCESS_LIST_ENTITY> CM_PROCESS_LIST_ById(string process_key); 
        #endregion 
    }
}
