using Abp.Application.Services.Dto;
using Common.gAMSPro.Consts;
using Common.gAMSPro.Intfs.OrganizationUnit;
using Common.gAMSPro.Intfs.OrganizationUnit.Dto;
using Core.gAMSPro.Application;
using Core.gAMSPro.Utils;

namespace Common.gAMSPro.Impls.OrganizationUnit
{
    public class OrganizationUnitAppService : gAMSProCoreAppServiceBase, IOrganizationUnitAppService
    {
        public async Task<List<ORGANIZATION_UNIT_ENTITY>> ORGANIZATION_UNIT_Search()
        {
            return await storeProcedureProvider.GetDataFromStoredProcedure<ORGANIZATION_UNIT_ENTITY>(CommonStoreProcedureConsts.ORGANIZATION_UNIT_Search, new { });
        }

        public async Task<CommonResult> ORGANIZATION_UNIT_Ins(ORGANIZATION_UNIT_ENTITY input)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.ORGANIZATION_UNIT_Ins, input)).FirstOrDefault();
            return result;
        }

        public async Task<CommonResult> ORGANIZATION_UNIT_Upd(ORGANIZATION_UNIT_ENTITY input)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.ORGANIZATION_UNIT_Upd, input)).FirstOrDefault();
            return result;
        }

        public async Task<CommonResult> ORGANIZATION_UNIT_Del(string id)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.ORGANIZATION_UNIT_Del, new { P_ID = id })).FirstOrDefault();
            return result;
        }

        public async Task<CommonResult> ORGANIZATION_UNIT_Move(string id, string newParentId)
        {
            var result = (await storeProcedureProvider.GetDataFromStoredProcedure<CommonResult>(CommonStoreProcedureConsts.ORGANIZATION_UNIT_Move, new
            {
                P_ID = id,
                P_NEW_PARENT_ID = newParentId
            })).FirstOrDefault();
            return result;
        }

        public async Task<PagedResultDto<ORGANIZATION_UNIT_USER_ENTITY>> ORGANIZATION_UNIT_USER_Search(ORGANIZATION_UNIT_USER_ENTITY input)
        {
            return await storeProcedureProvider.GetPagingData<ORGANIZATION_UNIT_USER_ENTITY>(CommonStoreProcedureConsts.ORGANIZATION_UNIT_USER_Search, input);

        }
    }
}
