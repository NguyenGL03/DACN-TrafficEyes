using Abp.Application.Services.Dto;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.DVDM;
using Common.gAMSPro.Intfs.DVDM.Dto;
using Core.gAMSPro.Application;

namespace Common.gAMSPro.Impls.DVDM
{
    public class DVDMAppService : gAMSProCoreAppServiceBase, IDVDMAppService
    {
        public async Task<List<CM_DVDM_ENTITY>> CM_DVDM_GetAll()
        {
            var query = "SELECT * FROM CM_DVDM";

            var result = await storeProcedureProvider.GetDataQuery<CM_DVDM_ENTITY>(query);
            return result;
        }

        public async Task<PagedResultDto<CM_DVDM_ENTITY>> CM_DVDM_Search(CM_DVDM_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_DVDM_ENTITY>(CommonStoreProcedureConsts.CM_DVDM_Search, input);
        }

        public async Task<PagedResultDto<CM_DVDM_ENTITY>> CM_DVCM_Search(CM_DVDM_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<CM_DVDM_ENTITY>("CM_DVCM_Search", input);
        }
    }
}
  