using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.Process.Dto;
using Common.gAMSPro.Process;
using Common.gAMSPro.Process.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class ProcessController : CoreAmsProControllerBase
    {
        readonly IProcessAppService processAppService;

        public ProcessController(IProcessAppService processAppService)
        {
            this.processAppService = processAppService;
        }

        [HttpGet]
        public async Task<CM_PROCESS_ENTITY> CM_PROCESS_ById(string id)
        {
            return await processAppService.CM_PROCESS_BYID(id);
        }

        [HttpGet]
        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_ByProcess(string process)
        {
            return await processAppService.CM_PROCESS_ByProcess(process);
        }

        [HttpGet]
        public async Task<List<PROCESS_ENTITY>> PL_PROCESS_REJECT_SEARCH(string reQ_ID, string type, string userLogin)
        {
            return await processAppService.PL_PROCESS_REJECT_SEARCH(reQ_ID, type, userLogin);
        }

        [HttpPost]
        public async Task<List<CM_PROCESS_STATUS_ENTITY>> CM_PROCESS_GetStatusByProcess([FromBody] CM_PROCESS_STATUS_ENTITY input)
        {
            return await processAppService.CM_PROCESS_GetStatusByProcess(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Search([FromBody] CM_PROCESS_ENTITY input)
        {
            return await processAppService.CM_PROCESS_Search(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Current_Search([FromBody] CM_PROCESS_ENTITY input)
        {
            return await processAppService.CM_PROCESS_Current_Search(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Reject([FromBody] CM_PROCESS_ENTITY input)
        {
            return await processAppService.CM_PROCESS_Reject(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_DT_Insert_Is_Done(string id, string currentUserName, string reqId, bool? isDone, bool? isReject)
        {
            return await processAppService.CM_PROCESS_DT_Insert_Is_Done(id, currentUserName, reqId, isDone, isReject);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_DT_Insert(string id, string currentUserName, string reqId)
        {
            return await processAppService.CM_PROCESS_DT_Insert(id, currentUserName, reqId);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_DT_Reject(int id, string currentUserName, string reqId, string auth_status, int order, bool isReject, string notes)
        {
            return await processAppService.CM_PROCESS_DT_Reject(id, currentUserName, reqId, auth_status, order, isReject, notes);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_DT_Update(string id, string currentUserName, string reqId, bool isReject)
        {
            return await processAppService.CM_PROCESS_DT_Update(id, currentUserName, reqId, isReject);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_PROCESS_ENTITY>> CM_PROCESS_Check_create([FromBody] CM_PROCESS_ENTITY input)
        {
            return await processAppService.CM_PROCESS_Check_create(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_InsertOrUpdate([FromBody] CM_PROCESS_INS_UPD_ENTITY input)
        {
            return await processAppService.CM_PROCESS_InsertOrUpdate(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_GetHiddenField(string currentUserName, string reqId)
        {
            return await processAppService.CM_PROCESS_GetHiddenField(currentUserName, reqId);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_PROCESS_LIST_ENTITY>> CM_PROCESS_LIST_Search([FromBody] CM_PROCESS_LIST_ENTITY input)
        {
            return await processAppService.CM_PROCESS_LIST_Search(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_LIST_Ins([FromBody] CM_PROCESS_LIST_ENTITY input)
        {
            return await processAppService.CM_PROCESS_LIST_Ins(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_LIST_Upd([FromBody] CM_PROCESS_LIST_ENTITY input)
        {
            return await processAppService.CM_PROCESS_LIST_Upd(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_PROCESS_LIST_Del(string process_key)
        {
            return await processAppService.CM_PROCESS_LIST_Del(process_key);
        }
             
        [HttpPost]
        public async Task<CM_PROCESS_LIST_ENTITY> CM_PROCESS_LIST_ById(string process_key)
        {
            return await processAppService.CM_PROCESS_LIST_ById(process_key);
        }
    }
}
