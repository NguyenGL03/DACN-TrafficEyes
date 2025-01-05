using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.Process.Dto;
using Common.gAMSPro.Intfs.RequestProcess;
using Common.gAMSPro.Intfs.RequestProcess.Dto;
using Common.gAMSPro.Process.Dto;
using Core.gAMSPro.Application;

namespace Common.gAMSPro.Impls.RequestProcess
{
    public class RequestProcessAppService : gAMSProCoreAppServiceBase, IRequestProcessAppService
    {


        public async Task<List<REQUEST_PROCESS_ENTITY>> PROCESS_CURRENT_SEARCH(string reQ_ID, string type, string userLogin)
        {

            var result = await storeProcedureProvider.GetDataFromStoredProcedure<REQUEST_PROCESS_ENTITY>(CommonStoreProcedureConsts.PL_PROCESS_CURRENT_SEARCH, new
            {
                p_REQ_ID = reQ_ID,
                p_USER_LOGIN = userLogin,
                p_TYPE = type
            });
            return result;
        }
        public async Task<List<PROCESS_ENTITY>> PROCESS_SEARCH(string reQ_ID, string type, string userLogin)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<PROCESS_ENTITY>(CommonStoreProcedureConsts.PL_PROCESS_SEARCH, new
            {
                p_REQ_ID = reQ_ID,
                p_USER_LOGIN = userLogin,
                p_TYPE = type
            });

            return result;
        }
    }
}
