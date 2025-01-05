using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.gAMSPro.Application.Intfs.DynamicPrimeTable.Dto;
using Abp.Application.Services.Dto;
namespace Common.gAMSPro.Application.Intfs.DynamicPrimeTable
{
    public interface IDynamicPrimeTableAppService
    {
        Task<PagedResultDto<DYNAMIC_SCREEN_PRIME_TABLE>> DYNAMIC_PRIME_TABLE_Search(DYNAMIC_SCREEN_PRIME_TABLE input);
        Task<List<DYNAMIC_PRIME_TABLE_ENTITY>>  DYNAMIC_PRIME_TABLE_CREATE(DYNAMIC_PRIME_TABLE_ENTITY input);

        Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetScreenName();

        Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetTableName( string input);

         Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_GetAllData( string input);

         Task<List<DYNAMIC_PRIME_TABLE_ENTITY>> DYNAMIC_PRIME_TABLE_UPDATE(DYNAMIC_PRIME_TABLE_ENTITY input);
         
         Task<IDictionary<string, object>> DYNAMIC_PRIME_TABLE_Del(string input);
        Task<IDictionary<string, object>> DYNAMIC_PRIME_TABLE_SendAppr(string input);
    }
}