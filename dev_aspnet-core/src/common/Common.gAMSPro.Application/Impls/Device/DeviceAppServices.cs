using Abp.Application.Services.Dto;
using Abp.Authorization;
using Common.gAMSPro.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Core.Authorization;
using Common.gAMSPro.Intfs.Device;
using Common.gAMSPro.Intfs.Device.Dto;
using Core.gAMSPro.Application;
using gAMSPro.Consts;

namespace Common.gAMSPro.Impls.Device
{
    [AbpAuthorize]
    public class DeviceAppService : gAMSProCoreAppServiceBase, IDeviceAppServices
    {
        public DeviceAppService()
        {

        }

        #region Public Method

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarDevice, gAMSProCorePermissions.Action.Search)]
        public async Task<PagedResultDto<CM_DEVICE_ENTITY>> CM_DEVICE_Search(CM_DEVICE_ENTITY input)
        {
            input.BRANCH_ID = input.BRANCH_CREATE;
            input.LEVEL = "ALL";
            var items = await storeProcedureProvider
                            .GetDataFromStoredProcedure<CM_DEVICE_ENTITY>
                            (CommonStoreProcedureConsts.CM_DEVICE_SEARCH, input);

            return await storeProcedureProvider.GetPagingData<CM_DEVICE_ENTITY>(CommonStoreProcedureConsts.CM_DEVICE_SEARCH, input);
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarDevice, gAMSProCorePermissions.Action.Create)]
        public async Task<IDictionary<string, object>> CM_DEVICE_Ins(CM_DEVICE_ENTITY input)
        {
            SetAuditForInsert(input);
            input.CREATE_DT = System.DateTime.Now;
            var result = (await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.CM_DEVICE_INS, input));
            return result;
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarDevice, gAMSProCorePermissions.Action.Update)]
        public async Task<IDictionary<string, object>> CM_DEVICE_Upd(CM_DEVICE_ENTITY input)
        {
            SetAuditForUpdate(input);
            return (await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.CM_DEVICE_UPD, input));
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarDevice, gAMSProCorePermissions.Action.Approve)]
        public async Task<IDictionary<string, object>> CM_DEVICE_Appr(string id, string currentUserName)
        {
            return (await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.CM_DEVICE_APPR, new
                {
                    DEVICE_ID = id,
                    AUTH_STATUS = ApproveStatusConsts.Approve,
                    CHECKER_ID = currentUserName,
                    APPROVE_DT = GetCurrentDateTime()
                }));
        }

        [CoreAuthorize(gAMSProCorePermissions.Prefix.Main, gAMSProCorePermissions.Page.CarDevice, gAMSProCorePermissions.Action.Delete)]
        public async Task<IDictionary<string, object>> CM_DEVICE_Del(string id, string currentUserName)
        {
            //Console.WriteLine(id);
            return (await storeProcedureProvider
                .GetResultValueFromStore(CommonStoreProcedureConsts.CM_DEVICE_DEL, new
                {
                    DEVICE_ID = id,
                    AUTH_STATUS = ApproveStatusConsts.Approve,
                    CHECKER_ID = currentUserName,
                    APPROVE_DT = GetCurrentDateTime()
                }));
        }

        [CoreAuthorize(new string[] {   (gAMSProCorePermissions.Prefix.Main + "." + gAMSProCorePermissions.Page.CarDevice + "." + gAMSProCorePermissions.Action.Update),
                                        (gAMSProCorePermissions.Prefix.Main + "." + gAMSProCorePermissions.Page.CarDevice + "." + gAMSProCorePermissions.Action.View)})]
        public async Task<CM_DEVICE_ENTITY> CM_DEVICE_ById(string id)
        {
            var projectList = await storeProcedureProvider.GetDataFromStoredProcedure<CM_DEVICE_ENTITY>(CommonStoreProcedureConsts.CM_DEVICE_BYID, new
            {
                DEVICE_CODE = id
            });
            return projectList.FirstOrDefault();
        }

        #endregion

        #region Private Method



        #endregion
    }
}
