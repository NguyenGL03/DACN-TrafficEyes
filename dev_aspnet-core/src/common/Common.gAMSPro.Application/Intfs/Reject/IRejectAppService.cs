using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Reject.Dto;

namespace Common.gAMSPro.Reject
{
    public interface IRejectAppService : IApplicationService
    {
        Task<PagedResultDto<CM_REJECT_LOG_ENTITY>> CM_REJECT_LOG_Search(CM_REJECT_LOG_ENTITY input);
        Task<IDictionary<string, object>> CM_REJECT_LOG_Ins(CM_REJECT_LOG_ENTITY input);
        Task<CM_REJECT_LOG_ENTITY> CM_REJECT_LOG_ById(string trd_id, string stage);
        Task<List<CM_REJECT_LOG_ENTITY>> CM_REJECT_LOG_Hist(string TRN_ID, string STAGE);
        Task<IDictionary<string, object>> CM_REJECT_LOG_ByType_Ins(CM_REJECT_LOG_ENTITY input);
        Task<List<CM_REJECT_PROCESS_ENTITY>> CM_REJECT_PROCESS_Search(string reQ_ID, string type, string userLogin);

    }
}
