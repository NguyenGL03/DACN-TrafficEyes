using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.gAMSPro.Application.Intfs.DynamicTable.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;
using Core.gAMSPro.CoreModule.Utils;


namespace Common.gAMSPro.Application.Intfs.DynamicTable
{
    public interface IDynamicTableAppService
    {

        Task<List<DYNAMIC_TABLE_MAP>> DYNAMIC_TABLE_GetTableName();
    
        Task<List<DYNAMIC_TABLE_MAP>> DYNAMIC_TABLE_GetColName(string input);

        Task<List<DYNAMIC_TABLE_NEW_ENTITY>> DYNAMIC_TABLE_NEW_CREATE(DYNAMIC_TABLE_INPUT input);

        Task<List<DYNAMIC_TABLE_UPDATE_ENTITY>> DYNAMIC_TABLE_NEW_UPDATE(DYNAMIC_TABLE_UPDATE_INPUT input);

        Task<List<DYNAMIC_PROC_MAP>> DYNAMIC_EXECUTE_PROC(DYNAMIC_PROC_MAP input);

        Task<List<DYNAMIC_PROC_ENTITY>> DYNAMIC_PROC_GetName();

        Task<IDictionary<string, object>> DYNAMIC_PROC_GetProcCode(string input);
        Task<List<DYNAMIC_TRIGGER_ENTITY>> DYNAMIC_TRIGGER_GetName();
    }

}