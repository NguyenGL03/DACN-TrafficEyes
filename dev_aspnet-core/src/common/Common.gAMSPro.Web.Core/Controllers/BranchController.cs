using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Branchs;
using Common.gAMSPro.Branchs.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class BranchController : CoreAmsProControllerBase
    {
        private readonly IBranchAppService branchAppService;
        public BranchController(IBranchAppService branchAppService)
        {
            this.branchAppService = branchAppService;
        }

        [HttpGet]
        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_Combobox()
        {
            return await branchAppService.CM_BRANCH_Combobox();
        }
        [HttpGet]
        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_USER_Combobox()
        {
            return await branchAppService.CM_BRANCH_USER_Combobox();
        }

        [HttpGet]
        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_USER_Combobox_2()
        {
            return await branchAppService.CM_BRANCH_USER_Combobox_2();
        }


        [HttpPost]
        public async Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_Search([FromBody] CM_BRANCH_ENTITY input)
        {
            return await branchAppService.CM_BRANCH_Search(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_GET_ALL([FromBody] CM_BRANCH_ENTITY input)
        {
            return await branchAppService.CM_BRANCH_GET_ALL(input);
        }

        [HttpPost]
        public async Task<FileDto> CM_BRANCH_ToExcel([FromBody] CM_BRANCH_ENTITY input)
        {
            return await branchAppService.CM_BRANCH_ToExcel(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_BRANCH_Ins([FromBody] CM_BRANCH_ENTITY input)
        {
            return await branchAppService.CM_BRANCH_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_BRANCH_Upd([FromBody] CM_BRANCH_ENTITY input)
        {
            return await branchAppService.CM_BRANCH_Upd(input);
        }

        [HttpGet]
        public async Task<CM_BRANCH_ENTITY> CM_BRANCH_ById(string id)
        {
            return await branchAppService.CM_BRANCH_ById(id);
        }

        [HttpGet]
        public async Task<CM_BRANCH_ENTITY> CM_BRANCH_ByCode(string id)
        {
            return await branchAppService.CM_BRANCH_ByCode(id);
        }

        [HttpGet]
        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_GetFatherList(string regionId, string branchType)
        {
            return await branchAppService.CM_BRANCH_GetFatherList(regionId, branchType);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_BRANCH_Del(string id)
        {
            return await branchAppService.CM_BRANCH_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_BRANCH_App(string id, string currentUserName)
        {
            return await branchAppService.CM_BRANCH_App(id, currentUserName);
        }

        [HttpGet]
        public async Task<CM_BRANCH_LEV_ENTITY> CM_BRANCH_Lev(string branchId)
        {
            return await branchAppService.CM_BRANCH_Lev(branchId);
        }

        [HttpGet]
        public async Task<List<CM_BRANCH_ENTITY>> CM_BRANCH_GetAllChild(string parentId)
        {
            return await branchAppService.CM_BRANCH_GetAllChild(parentId);
        }

        [HttpPost]
        public IDictionary<string, List<List<int>>> GetListData([FromBody] DataSet dts)
        {
            return null;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_DEP_Search([FromBody] CM_BRANCH_ENTITY input)
        {
            return await branchAppService.CM_BRANCH_Dep_Search(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_BRANCH_ENTITY>> CM_BRANCH_DEP_Search_v2([FromBody] CM_BRANCH_ENTITY input)
        {
            return await branchAppService.CM_BRANCH_Dep_Search_v2(input);
        }

        [HttpGet]
        public async Task<CM_BRANCH_ENTITY> CM_BRANCH_Dep_ById(string branchId, string depId)
        {
            return await branchAppService.CM_BRANCH_Dep_ById(branchId, depId);
        }


    }
}
