using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.gAMSPro.Application.Intfs.DynamicPrimeTable.Dto;
using Abp.Authorization;
using Common.gAMSPro.Consts;
using Core.gAMSPro.CoreModule.Utils;
using Common.gAMSPro.Application.Intfs.DynamicPrimeTable;
using System.Linq.Dynamic.Core;
using Core.gAMSPro.CoreModule.Utils;
using Core.gAMSPro.Application;
using Newtonsoft.Json;
using Abp.Application.Services.Dto;

namespace Common.gAMSPro.Application.Impls.DynamicPrimeTable
{
    [AbpAuthorize]
    public class DynamicPrimeTableAppService : gAMSProCoreAppServiceBase, IDynamicPrimeTableAppService
    {
        public DynamicPrimeTableAppService() { }
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_CREATE(DYNAMIC_PRIME_TABLE_ENTITY input)
        {
            var parameters = new
            {
                tableName = input.tableName,
                p_screenName = input.ScreenName,
                p_screenId = input.ScreenId,
                p_authStatus = input.AUTH_STATUS,
                p_columnsJson = JsonConvert.SerializeObject(input.Columns),
                p_configJson = JsonConvert.SerializeObject(input.Config)
            };

            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_PRIME_TABLE_ENTITY>(
                CommonStoreProcedureConsts.DYNAMIC_PRIME_TABLE_CREATE, parameters);
        }
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetScreenName()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_PRIME_TABLE_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_PRIME_TABLE_GetScreenName, null);
        }

        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetTableName(string input)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_PRIME_TABLE_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_PRIME_TABLE_GetTableName, new
            {
                p_screenId = input
            });
        }
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetAllData(string input)
        {
            var result = await storeProcedureProvider.GetDataFromStoredProcedure<dynamic>(
                CommonStoreProcedureConsts.DYNAMIC_PRIME_TABLE_GetAllData, new
                {
                    p_tableName = input
                });

            var entities = new List<DYNAMIC_PRIME_TABLE_ENTITY>();

            foreach (var item in result)
            {
                var entity = new DYNAMIC_PRIME_TABLE_ENTITY
                {
                    tableName = item.tableName,
                    ScreenName = item.screenName,
                    ScreenId = item.screenId,
                    AUTH_STATUS = item.AUTH_STATUS,
                    Columns = JsonConvert.DeserializeObject<List<DYNAMIC_COLUMN_PRIME_TABLE>>(item.columns.ToString()),
                    Config = JsonConvert.DeserializeObject<List<DYNAMIC_CONFIG_PRIME_TABLE>>(item.config.ToString())[0]
                };
                entities.Add(entity);
            }
            return entities;
        }
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_UPDATE(DYNAMIC_PRIME_TABLE_ENTITY input)
        {
            var parameters = new
            {
                tableName = input.tableName,
                p_screenName = input.ScreenName,
                p_screenId = input.ScreenId,
                p_authStatus = input.AUTH_STATUS,
                p_columnsJson = JsonConvert.SerializeObject(input.Columns),
                p_configJson = JsonConvert.SerializeObject(input.Config)
            };
            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_PRIME_TABLE_ENTITY>(
                CommonStoreProcedureConsts.DYNAMIC_PRIME_TABLE_UPDATE, parameters);
        }
        public async Task<PagedResultDto<DYNAMIC_SCREEN_PRIME_TABLE>> DYNAMIC_PRIME_TABLE_Search(DYNAMIC_SCREEN_PRIME_TABLE input)
        {
            return await storeProcedureProvider.GetPagingData<DYNAMIC_SCREEN_PRIME_TABLE>(CommonStoreProcedureConsts.DYNAMIC_PRIME_TABLE_Search, input);
        }

        public async Task<IDictionary<string, object>> DYNAMIC_PRIME_TABLE_Del(string input)
        {
             return await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.DYNAMIC_PRIME_TABLE_Del, new
            {
                p_tableName = input
            });
        }
         public async Task<IDictionary<string, object>> DYNAMIC_PRIME_TABLE_SendAppr(string input)
        {
             return await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.DYNAMIC_PRIME_TABLE_SendAppr, new
            {
                p_tableName = input
            });
        }
    }
}