using Abp.Application.Services.Dto;
using Common.gAMSPro.Application.Intfs.Suppliers;
using Common.gAMSPro.Suppliers;
using Common.gAMSPro.Suppliers.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class SupplierController : CoreAmsProControllerBase
    {
        private readonly ISupplierAppService supplierAppService;

        public SupplierController(ISupplierAppService supplierAppService)
        {
            this.supplierAppService = supplierAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_SUPPLIER_ENTITY>> CM_SUPPLIER_Search([FromBody]CM_SUPPLIER_ENTITY input)
        {
            return await supplierAppService.CM_SUPPLIER_Search(input);
        }

        [HttpPost]
        public async Task<FileDto> CM_SUPPLIER_ToExcel([FromBody]CM_SUPPLIER_ENTITY input)
        {
            return await supplierAppService.CM_SUPPLIER_ToExcel(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_SUPPLIER_Ins([FromBody]CM_SUPPLIER_ENTITY input)
        {
            return await supplierAppService.CM_SUPPLIER_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_SUPPLIER_Upd([FromBody]CM_SUPPLIER_ENTITY input)
        {
            return await supplierAppService.CM_SUPPLIER_Upd(input);
        }

        [HttpGet]
        public async Task<CM_SUPPLIER_ENTITY> CM_SUPPLIER_ById(string id)
        {
            return await supplierAppService.CM_SUPPLIER_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_SUPPLIER_Del(string id, string userLogin)
        {
            return await supplierAppService.CM_SUPPLIER_Del(id, userLogin);
        }

        [HttpPost]
        public async Task<CommonResult> CM_SUPPLIER_App(string id, string currentUserName)
        {
            return await supplierAppService.CM_SUPPLIER_App(id, currentUserName);
        }
    }
}
