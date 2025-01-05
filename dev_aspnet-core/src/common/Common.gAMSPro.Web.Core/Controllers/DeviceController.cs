using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
using Common.gAMSPro.Impls.Device;
using Common.gAMSPro.Intfs.Device;
using Common.gAMSPro.Intfs.Device.Dto;
using Common.gAMSPro.Web.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Common.gAMSPro.Web.Core.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class DeviceController : CoreAmsProControllerBase
    {
        private readonly IDeviceAppServices deviceAppService;

        public DeviceController(DeviceAppService deviceAppService)
        {
            this.deviceAppService = deviceAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_DEVICE_ENTITY>> CM_DEVICE_Search([FromBody] CM_DEVICE_ENTITY input)
        {
            return await deviceAppService.CM_DEVICE_Search(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_DEVICE_Ins([FromBody] CM_DEVICE_ENTITY input)
        {
            return await deviceAppService.CM_DEVICE_Ins(input);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_DEVICE_Upd([FromBody] CM_DEVICE_ENTITY input)
        {
            return await deviceAppService.CM_DEVICE_Upd(input);
        }

        [HttpGet]
        public async Task<CM_DEVICE_ENTITY> DEVICE_ById(string id)
        {
            return await deviceAppService.CM_DEVICE_ById(id);
        }

        [HttpDelete]
        public async Task<IDictionary<string, object>> CM_DEVICE_Del(string id, string currentUsername)
        {
            return await deviceAppService.CM_DEVICE_Del(id, currentUsername);
        }

        [HttpPost]
        public async Task<IDictionary<string, object>> CM_DEVICE_Appr(string id, string currentUserName)
        {
            return await deviceAppService.CM_DEVICE_Appr(id, currentUserName);
        }
    }
}
