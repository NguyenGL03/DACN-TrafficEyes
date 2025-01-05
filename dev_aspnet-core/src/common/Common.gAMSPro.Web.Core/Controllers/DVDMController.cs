using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Intfs.DVDM;
using Common.gAMSPro.Intfs.DVDM.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class DVDMController : CoreAmsProControllerBase
    {
        private readonly IDVDMAppService dvdmAppService;

        public DVDMController(IDVDMAppService dvdmAppService)
        {
            this.dvdmAppService = dvdmAppService;
        }

        [HttpGet]
        public async Task<List<CM_DVDM_ENTITY>> CM_DVDM_GetAll()
        {
            return await dvdmAppService.CM_DVDM_GetAll();
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_DVDM_ENTITY>> CM_DVDM_Search([FromBody] CM_DVDM_ENTITY input)
        {
            return await dvdmAppService.CM_DVDM_Search(input);
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_DVDM_ENTITY>> CM_DVCM_Search([FromBody] CM_DVDM_ENTITY input)
        {
            return await dvdmAppService.CM_DVCM_Search(input);
        }
    }
}
