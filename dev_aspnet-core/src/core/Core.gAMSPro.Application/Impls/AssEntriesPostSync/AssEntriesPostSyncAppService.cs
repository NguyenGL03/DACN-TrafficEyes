using Core.gAMSPro.Intfs.AssEntriesPostSync;
using Core.gAMSPro.Intfs.AssEntriesPostSync.Dto;
using gAMSPro.ProcedureHelpers;
using Newtonsoft.Json;

namespace Core.gAMSPro.Impls.AssEntriesPostSync
{
    public class AssEntriesPostSyncAppService : IAssEntriesPostSyncAppService
    {
        protected IStoreProcedureProvider storeProcedureProvider;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public AssEntriesPostSyncAppService(IStoreProcedureProvider _storeProcedureProvider)
        {
            this.storeProcedureProvider = _storeProcedureProvider;
        }
        public async Task<List<ASS_ENTRIES_POST_SYNC_ENTITY>> ASS_ENTRIES_POST_SYNC_BY_TRN_ID(string id)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<ASS_ENTRIES_POST_SYNC_ENTITY>("ASS_ENTRIES_POST_SYNC_BY_TRN_ID", new
            {
                p_TRN_ID = id
            });
            foreach (var item in result)
            {
                item.pTR = await storeProcedureProvider.GetDataFromStoredProcedure<PTR_ENTITY>("ASS_ENTRIES_POST_SYNC_PTR", new
                {
                    p_TRN_ID = id,
                    p_AC_BRANCH = item.solId
                });
            }
            var resultAsJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            log.Info($"ASS_ENTRIES_POST_SYNC_BY_TRN_ID API result : {resultAsJson}");
            return result;
        }

        public async Task<ASS_ENTRIES_POST_SYNC_ENTITY> PAY_ENTRIES_POST_SYNC_BY_TRN_ID(string id)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<ASS_ENTRIES_POST_SYNC_ENTITY>("PAY_ENTRIES_POST_SYNC_BY_TRN_ID", new
            {
                p_TRN_ID = id
            })).FirstOrDefault();

            result.pTR = await storeProcedureProvider.GetDataFromStoredProcedure<PTR_ENTITY>("PAY_ENTRIES_POST_SYNC_PTR", new
            {
                p_TRN_ID = id
            });
            var resultAsJson = JsonConvert.SerializeObject(result, Formatting.Indented);
            log.Info($"PAY_ENTRIES_POST_SYNC_BY_TRN_ID API result : {resultAsJson}");
            return result;
        }
    }
}
