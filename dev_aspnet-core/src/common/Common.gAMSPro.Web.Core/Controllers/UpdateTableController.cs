using Common.gAMSPro.Intfs.UpdateTable;
using Common.gAMSPro.Intfs.UpdateTable.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.UpdateTableMap.Dto;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class UpdateTableController : CoreAmsProControllerBase
    {
        readonly IUpdateTableService updateTableService;

        public UpdateTableController(IUpdateTableService updateTableService)
        {
            this.updateTableService = updateTableService;
        }

        [HttpPut]
        public async Task<CommonResult> CM_ALLCODE_Upd([FromBody] UpdateTableDto input)
        {
            return await updateTableService.UpdateTable(input);
        }
        [HttpPost]
        public async Task<List<UpdateTableDto>> UPDATE_TABLE_ByLevel(int level)
        {
            return await updateTableService.UPDATE_TABLE_ByLevel(level);
        }
        [HttpGet]
        public async Task<List<UpdateTableDto>> UPDATE_TABLE_GetList()
        {
            return await updateTableService.UPDATE_TABLE_GetList();
        }
        [HttpPost]
        public async Task<IDictionary<string, object>> UPDATE_TABLE_Ins([FromBody] UpdateTableMapDto input)
        {
            return await updateTableService.UPDATE_TABLE_Ins(input);
        }
        [HttpPut]
        public async Task<IDictionary<string, object>> UPDATE_TABLE_Udp([FromBody] UpdateTableDto input)
        {
            return await updateTableService.UPDATE_TABLE_Udp(input);
        }
    }
}
