using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Common.gAMSPro.Application.Impls.DynamicTable;
using Common.gAMSPro.Application.Intfs.DynamicTable;
using Common.gAMSPro.Web.Controllers;
using Core.gAMSPro.Utils;
using Common.gAMSPro.Application.Intfs.DynamicTable.Dto;



namespace Common.gAMSPro.Web.Core.Controllers
{
    [Route("api/[controller]/[action]")]
    public class DynamicTableController : CoreAmsProControllerBase
    {
        readonly IDynamicTableAppService dynamicTableAppService;

        public DynamicTableController (IDynamicTableAppService dynamicTableAppService)
        {
            this.dynamicTableAppService = dynamicTableAppService;
        }

        [HttpGet]
        public async Task<List<DYNAMIC_TABLE_MAP>> DYNAMIC_TABLE_GetTableName()
        {
            return await dynamicTableAppService.DYNAMIC_TABLE_GetTableName();
        }
        [HttpGet]
        public async Task<List<DYNAMIC_TABLE_MAP>> DYNAMIC_TABLE_GetColName(string input)
        {
            return await dynamicTableAppService.DYNAMIC_TABLE_GetColName(input);
        }
       [HttpPost]
       public async Task<List<DYNAMIC_TABLE_NEW_ENTITY>> DYNAMIC_TABLE_NEW_CREATE([FromBody] DYNAMIC_TABLE_INPUT input)
       {
            return await dynamicTableAppService.DYNAMIC_TABLE_NEW_CREATE(input);
       }

        [HttpPost]
        public async Task<List<DYNAMIC_TABLE_UPDATE_ENTITY>> DYNAMIC_TABLE_NEW_UPDATE([FromBody] DYNAMIC_TABLE_UPDATE_INPUT input)
        {
            return await dynamicTableAppService.DYNAMIC_TABLE_NEW_UPDATE(input);
        }

        [HttpPost]
        public async Task<List<DYNAMIC_PROC_MAP>> DYNAMIC_EXECUTE_PROC([FromBody] DYNAMIC_PROC_MAP input)
        {
            return await dynamicTableAppService.DYNAMIC_EXECUTE_PROC(input);
        }
        [HttpGet]
        public async Task<List<DYNAMIC_PROC_ENTITY>> DYNAMIC_PROC_GetName()
        {
            return await dynamicTableAppService.DYNAMIC_PROC_GetName();
        }
        [HttpGet]
        public async Task<IDictionary<string, object>> DYNAMIC_PROC_GetProcCode(string input)
        {
            return await dynamicTableAppService.DYNAMIC_PROC_GetProcCode(input);
        }
        [HttpGet]
        public async Task<List<DYNAMIC_TRIGGER_ENTITY>> DYNAMIC_TRIGGER_GetName()
        {
            return await dynamicTableAppService.DYNAMIC_TRIGGER_GetName();
        }
    }
}