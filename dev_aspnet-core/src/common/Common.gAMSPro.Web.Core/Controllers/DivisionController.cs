using Abp.Application.Services.Dto;
using Common.gAMSPro.Divisions;
using Common.gAMSPro.Divisions.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DivisionController : CoreAmsProControllerBase
    {
        private readonly IDivisionAppService divisionAppService;

        public DivisionController(IDivisionAppService divisionAppService)
        {
            this.divisionAppService = divisionAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_DIVISION_ENTITY>> CM_DIVISION_Search([FromBody]CM_DIVISION_ENTITY input)
        {
            return await divisionAppService.CM_DIVISION_Search(input);
        }

        [HttpPost]
        public async Task<FileDto> CM_DIVISION_ToExcel([FromBody]CM_DIVISION_ENTITY input)
        {
            return await divisionAppService.CM_DIVISION_ToExcel(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_DIVISION_Ins([FromBody]CM_DIVISION_ENTITY input)
        {
            return await divisionAppService.CM_DIVISION_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_DIVISION_Upd([FromBody]CM_DIVISION_ENTITY input)
        {
            return await divisionAppService.CM_DIVISION_Upd(input);
        }

        [HttpGet]
        public async Task<CM_DIVISION_ENTITY> CM_DIVISION_ById(string id)
        {
            return await divisionAppService.CM_DIVISION_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_DIVISION_Del(string id)
        {
            return await divisionAppService.CM_DIVISION_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_DIVISION_App(string id, string currentUserName)
        {
            return await divisionAppService.CM_DIVISION_App(id, currentUserName);
        }

        [HttpGet]
        public async Task<List<CM_DIVISION_ENTITY>> CM_DIVISION_GETALLCHILD(string parentId)
        {
            return await divisionAppService.CM_DIVISION_GETALLCHILD(parentId);
        }
    }
}
