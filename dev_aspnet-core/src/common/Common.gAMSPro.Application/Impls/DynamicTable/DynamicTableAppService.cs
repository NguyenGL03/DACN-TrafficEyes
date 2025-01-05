using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.gAMSPro.Utils;
using Common.gAMSPro.Application.Intfs.DynamicTable.Dto;
using Common.gAMSPro.Application.Intfs.DynamicTable;
using Abp.Application.Services.Dto;
using Core.gAMSPro.Application;
using Abp.Authorization;
using Common.gAMSPro.Consts;
using Core.gAMSPro.CoreModule.Utils;
using System.Linq.Dynamic.Core;


namespace Common.gAMSPro.Application.Impls.DynamicTable
{
    [AbpAuthorize]
    public class DynamicTableAppService : gAMSProCoreAppServiceBase, IDynamicTableAppService
    {

        public DynamicTableAppService() { }

        public async Task<List<DYNAMIC_TABLE_MAP>> DYNAMIC_TABLE_GetTableName()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_TABLE_MAP>(CommonStoreProcedureConsts.DYNAMIC_TABLE_GETTABLENAME, null);
        }
        public async Task<List<DYNAMIC_TABLE_MAP>> DYNAMIC_TABLE_GetColName(string input)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_TABLE_MAP>(CommonStoreProcedureConsts.DYNAMIC_TABLE_GETCOLNAME, new
            {
                p_TableName = input
            });
        }

        public async Task<List<DYNAMIC_TABLE_NEW_ENTITY>> DYNAMIC_TABLE_NEW_CREATE(DYNAMIC_TABLE_INPUT input)
        {
            var XmlData = XmlHelper.ToXmlFromList(input.Entities);
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_TABLE_NEW_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_TABLE_NEW_CREATE, new {
                XmlData = XmlData
        }));
            return result;
        }
        public async Task<List<DYNAMIC_TABLE_UPDATE_ENTITY>> DYNAMIC_TABLE_NEW_UPDATE(DYNAMIC_TABLE_UPDATE_INPUT input)
        {
            var XmlData = XmlHelper.ToXmlFromList(input.Entities);
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_TABLE_UPDATE_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_TABLE_NEW_UPDATE, new {
                XmlData = XmlData
            }));
            return result;
        }

        public async  Task<List<DYNAMIC_PROC_MAP>> DYNAMIC_EXECUTE_PROC(DYNAMIC_PROC_MAP input)
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_PROC_MAP>(CommonStoreProcedureConsts.EXECUTE_QUERY,input);
        }

        public async Task<List<DYNAMIC_PROC_ENTITY>> DYNAMIC_PROC_GetName()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_PROC_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_PROC_GetName,null);
        }

        public async Task<IDictionary<string, object>> DYNAMIC_PROC_GetProcCode(string input)
        {
            return await storeProcedureProvider.GetResultValueFromStore(CommonStoreProcedureConsts.DYNAMIC_PROC_GetProcCode, new{
                ProcName = input
            });
        }
        public async Task<List<DYNAMIC_TRIGGER_ENTITY>> DYNAMIC_TRIGGER_GetName()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<DYNAMIC_TRIGGER_ENTITY>(CommonStoreProcedureConsts.DYNAMIC_TRIGGER_GetName,null);
        }

    }
}