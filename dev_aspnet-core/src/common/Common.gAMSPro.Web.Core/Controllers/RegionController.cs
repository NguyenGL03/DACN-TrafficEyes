using Abp.Application.Services.Dto;
using Common.gAMSPro.Regions;
using Common.gAMSPro.Regions.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RegionController : CoreAmsProControllerBase
    {
        readonly IRegionAppService regionAppService;

        public RegionController(IRegionAppService regionAppService)
        {
            this.regionAppService = regionAppService;
        }


        [HttpPost]
        public async Task<PagedResultDto<CM_REGION_ENTITY>> CM_REGION_Search([FromBody]CM_REGION_ENTITY? input)
        {
            return await regionAppService.CM_REGION_Search(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_REGION_Ins([FromBody]CM_REGION_ENTITY input)
        {
            return await regionAppService.CM_REGION_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_REGION_Upd([FromBody]CM_REGION_ENTITY input)
        {
            return await regionAppService.CM_REGION_Upd(input);
        }

        [HttpGet]
        public async Task<CM_REGION_ENTITY> CM_REGION_ById(string id)
        {
            return await regionAppService.CM_REGION_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_REGION_Del(string id)
        {
            return await regionAppService.CM_REGION_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_REGION_App(string id, string currentUserName)
        {
            return await regionAppService.CM_REGION_App(id, currentUserName);
        }
    }
}
