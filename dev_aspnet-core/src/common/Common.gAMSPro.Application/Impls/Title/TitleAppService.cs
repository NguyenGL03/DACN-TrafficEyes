using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.Title;
using Common.gAMSPro.Intfs.Title.Dto;
using Core.gAMSPro.Application;

namespace Common.gAMSPro.Impls.Title
{
    public class TitleAppService : gAMSProCoreAppServiceBase, ITitleAppService
    {
        public async Task<List<CM_TITLE_ENTITY>> CM_TITLE_SEARCH(string titleType)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<CM_TITLE_ENTITY>(CommonStoreProcedureConsts.CM_TITLE_SEARCH, new
            {
                USER_LOGIN = GetCurrentUserName(),
                TITLE_TYPE = titleType
            });
        }
    }
}
