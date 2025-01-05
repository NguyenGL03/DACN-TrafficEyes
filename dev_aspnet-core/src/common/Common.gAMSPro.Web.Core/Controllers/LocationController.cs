using Common.gAMSPro.Intfs.Locations;
using Common.gAMSPro.Intfs.Locations.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class LocationController : CoreAmsProControllerBase
    {
        private ILocationAppService locationAppService;

        public LocationController(ILocationAppService locationAppService)
        {
            this.locationAppService = locationAppService;
        }

        [HttpPost]
        public async Task<CM_LOCATION_ENTITY> CM_LOCATION_AllData()
        {
            return await locationAppService.CM_LOCATION_AllData();
        }
    }
}
