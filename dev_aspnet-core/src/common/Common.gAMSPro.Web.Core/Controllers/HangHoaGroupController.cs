using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.HangHoaGroup;
using Common.gAMSPro.Intfs.HangHoaGroup.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class HangHoaGroupController : CoreAmsProControllerBase
    {
        readonly IHangHoaGroupAppService hangHoaGroupAppService;

        public HangHoaGroupController(IHangHoaGroupAppService hangHoaGroupAppService)
        {
            this.hangHoaGroupAppService = hangHoaGroupAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_HANGHOA_GROUP_ENTITY>> CM_HANGHOA_GROUP_Search([FromBody] CM_HANGHOA_GROUP_ENTITY input)
        {
            return await hangHoaGroupAppService.CM_HangHoa_Group_Search(input);
        }

        [HttpGet]
        public async Task<List<CM_HANGHOA_GROUP_ENTITY>> CM_HANGHOA_GROUP_GetAll()
        {
            return await hangHoaGroupAppService.CM_HangHoa_Group_GetAll();
        }

        [HttpPost]
        public async Task<InsertResult> CM_HANGHOA_GROUP_Ins([FromBody] CM_HANGHOA_GROUP_ENTITY input)
        {
            return await hangHoaGroupAppService.CM_HangHoa_Group_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_HANGHOA_GROUP_Upd([FromBody] CM_HANGHOA_GROUP_ENTITY input)
        {
            return await hangHoaGroupAppService.CM_HangHoa_Group_Upd(input);
        }

        [HttpGet]
        public async Task<CM_HANGHOA_GROUP_ENTITY> CM_HANGHOA_GROUP_ById(string id)
        {
            return await hangHoaGroupAppService.CM_HangHoa_Group_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_HANGHOA_GROUP_Del(string id)
        {
            return await hangHoaGroupAppService.CM_HangHoa_Group_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_HANGHOA_GROUP_App(string id, string currentUserName)
        {
            return await hangHoaGroupAppService.CM_HangHoa_Group_App(id, currentUserName);
        }
    }
}
