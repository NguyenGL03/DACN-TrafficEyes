using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Common.gAMSPro.Application.Intfs.DynamicPrimeTable;
using Common.gAMSPro.Application.Intfs.DynamicPrimeTable.Dto;
using Common.gAMSPro.Web.Controllers;
using Core.gAMSPro.Utils;
using Abp.Application.Services.Dto;
using Abp.Runtime.Validation;
namespace Common.gAMSPro.Web.Core.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class DynamicPrimeTableController : CoreAmsProControllerBase
    {
        readonly IDynamicPrimeTableAppService dynamicPrimeTableAppService;

        public DynamicPrimeTableController(IDynamicPrimeTableAppService dynamicPrimeTableAppService)
        {
            this.dynamicPrimeTableAppService = dynamicPrimeTableAppService;
        }
        [HttpPost]
        public async Task<PagedResultDto<DYNAMIC_SCREEN_PRIME_TABLE>> DYNAMIC_PRIME_TABLE_Search([FromBody] DYNAMIC_SCREEN_PRIME_TABLE input)
        {
            return await dynamicPrimeTableAppService.DYNAMIC_PRIME_TABLE_Search(input);
        }
        [HttpPost]
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_CREATE([FromBody] DYNAMIC_PRIME_TABLE_ENTITY input)
        {
            return await dynamicPrimeTableAppService.DYNAMIC_PRIME_TABLE_CREATE(input);
        }
        [HttpGet]
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetScreenName()
        {
            return await dynamicPrimeTableAppService.DYNAMIC_PRIME_TABLE_GetScreenName();
        }
        [HttpGet]
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetTableName(string input)
        {
            return await dynamicPrimeTableAppService.DYNAMIC_PRIME_TABLE_GetTableName(input);
        }
        [HttpGet]
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetAllData(string input)
        {
            return await dynamicPrimeTableAppService.DYNAMIC_PRIME_TABLE_GetAllData(input);
        }
        [HttpPost]
        public async Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_UPDATE([FromBody] DYNAMIC_PRIME_TABLE_ENTITY input)
        {
            return await dynamicPrimeTableAppService.DYNAMIC_PRIME_TABLE_UPDATE(input);
        }
        [HttpDelete]
        public async Task<IDictionary<string, object>> DYNAMIC_PRIME_TABLE_Del(string input)
        {
            return await dynamicPrimeTableAppService.DYNAMIC_PRIME_TABLE_Del(input);
        }
        [HttpPost]
        public async Task<IDictionary<string, object>> DYNAMIC_PRIME_TABLE_SendAppr(string input)
        {
            return await dynamicPrimeTableAppService.DYNAMIC_PRIME_TABLE_SendAppr(input);
        }
    }
}