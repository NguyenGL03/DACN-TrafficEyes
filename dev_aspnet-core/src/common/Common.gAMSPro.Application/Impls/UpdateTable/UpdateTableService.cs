using Abp.Authorization;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.UpdateTable;
using Common.gAMSPro.Intfs.UpdateTable.Dto;
using Common.gAMSPro.Intfs.Utilities;
using Core.gAMSPro;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.UpdateTableMap.Dto;

namespace Common.gAMSPro.Impls.UpdateTable
{
    [AbpAuthorize]
    public class UpdateTableService : gAMSProCoreAppServiceBase, IUpdateTableService
    {
        public UpdateTableService() { }


        public async Task<CommonResult> UpdateTable(UpdateTableDto dto)
        {
            return (await storeProcedureProvider
                .GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.UPDATE_TABLE, new
                {
                    P_REQ_ID = dto.REQ_ID,
                    P_UPDATE_COLUMN_KEY_ID = dto.UPDATE_COLUMN_KEY_ID,
                    P_UPDATE_VALUE = dto.UPDATE_VALUE,
                    P_USER_LOGIN = GetCurrentUserName()
                })).FirstOrDefault();
        }
        public async Task<List<UpdateTableDto>> UPDATE_TABLE_ByLevel(int level)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<UpdateTableDto>(CommonStoreProcedureConsts.UPDATE_TABLE_BYLEVEL, new
            {
                p_UPDATE_LEVEL = level
            });
        }
         public async Task<List<UpdateTableDto>> UPDATE_TABLE_GetList()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<UpdateTableDto>(CommonStoreProcedureConsts.UPDATE_TABLE_GETLIST, null);
        }
        public async Task<IDictionary<string,object>> UPDATE_TABLE_Ins(UpdateTableMapDto input)
        {
            var result = (await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.UPDATE_TABLE_INS, input));
            return result;
        }
        public async Task<IDictionary<string,object>> UPDATE_TABLE_Udp(UpdateTableDto input)
        {
            var result = (await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.UPDATE_TABLE_UDP, input));
            return result;
        }
    }
}
