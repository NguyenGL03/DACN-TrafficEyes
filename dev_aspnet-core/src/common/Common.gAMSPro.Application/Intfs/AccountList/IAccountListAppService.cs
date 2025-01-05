using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.AccountList.Dto;

namespace Common.gAMSPro.Intfs.AccountList
{
    public interface IAccountListAppService : IApplicationService
    {
        Task<PagedResultDto<CM_ACCOUNT_PAY_ENTITY>> CM_ACCOUNT_PAY_Search(CM_ACCOUNT_PAY_ENTITY input);
        Task<PagedResultDto<CM_ACCOUNT_PAY_ENTITY>> CM_ACCOUNT_Search(CM_ACCOUNT_PAY_ENTITY input);
        Task<IDictionary<string, object>> CM_ACCOUNT_PAY_Ins(CM_ACCOUNT_PAY_ENTITY input);
        Task<IDictionary<string, object>> CM_ACCOUNT_PAY_Upd(CM_ACCOUNT_PAY_ENTITY input);
        Task<IDictionary<string, object>> CM_ACCOUNT_PAY_Del(string id);
        Task<IDictionary<string, object>> CM_ACCOUNT_PAY_App(string id, string accNo, string accType, string currentUserName);
        Task<CM_ACCOUNT_PAY_ENTITY> CM_ACCOUNT_PAY_ById(string id, string refId, string acc_type);
        Task<List<CM_ACCOUNT_PAY_ENTITY>> CM_ACCOUNT_PAY_List();
        Task<CM_ACCOUNT_PAY_ENTITY> CM_ACCOUNT_BYID(string id);
        Task<IDictionary<string, object>> CM_ACCOUNT_Upd(CM_ACCOUNT_PAY_ENTITY input);


    }
}
