using Abp.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.CoreModule.Helper;
using Common.gAMSPro.Intfs.Locations;
using Common.gAMSPro.Intfs.Locations.Dto;
using Core.gAMSPro.Application;

namespace Common.gAMSPro.Impls.Locations
{
    [AbpAuthorize]
    public class LocationAppService : gAMSProCoreAppServiceBase, ILocationAppService
    {
        public async Task<CM_LOCATION_ENTITY> CM_LOCATION_AllData()
        {
            CM_LOCATION_ENTITY model = new CM_LOCATION_ENTITY();
            await storeProcedureProvider.GetMultiData2(CommonStoreProcedureConsts.CM_LOCATION_ALLDATA, null, (result) =>
            {
                model.DISTRICTs = result.Read<CM_DISTRICT>().ToList();
                model.NATIONs = result.Read<CM_NATION>().ToList();
                model.PROVINCEs = result.Read<CM_PROVINCE>().ToList();
                model.WARDs = result.Read<CM_WARD>().ToList();
                return true;
            });
            return model;
        }
    }
}
