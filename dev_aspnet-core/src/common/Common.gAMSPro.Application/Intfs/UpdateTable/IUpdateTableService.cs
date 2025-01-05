using Common.gAMSPro.Intfs.UpdateTable.Dto;
using Core.gAMSPro.Utils;
using System.Threading.Tasks;
using gAMSPro.Dto;
using Abp.Application.Services.Dto;
using Common.gAMSPro.Intfs.UpdateTableMap.Dto;

namespace Common.gAMSPro.Intfs.UpdateTable
{
    public interface IUpdateTableService

    {
        Task<CommonResult> UpdateTable(UpdateTableDto dto);
        Task<List<UpdateTableDto>> UPDATE_TABLE_ByLevel(int level);
        Task<List<UpdateTableDto>> UPDATE_TABLE_GetList();
        Task<IDictionary<string, object>> UPDATE_TABLE_Ins(UpdateTableMapDto dto);
        Task<IDictionary<string, object>> UPDATE_TABLE_Udp(UpdateTableDto dto);
    }
}
