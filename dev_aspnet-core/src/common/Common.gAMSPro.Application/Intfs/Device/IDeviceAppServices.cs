using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.Device.Dto;

namespace Common.gAMSPro.Intfs.Device
{
    public interface IDeviceAppServices
    {
        Task<PagedResultDto<CM_DEVICE_ENTITY>> CM_DEVICE_Search(CM_DEVICE_ENTITY input);
        Task<IDictionary<string, object>> CM_DEVICE_Ins(CM_DEVICE_ENTITY input);
        Task<IDictionary<string, object>> CM_DEVICE_Upd(CM_DEVICE_ENTITY input);
        Task<IDictionary<string, object>> CM_DEVICE_Del(string id, string currentUserName);
        Task<IDictionary<string, object>> CM_DEVICE_Appr(string id, string currentUserName);
        Task<CM_DEVICE_ENTITY> CM_DEVICE_ById(string id);
    }
}
