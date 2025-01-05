using Abp.Application.Services.Dto;
using Common.gAMSPro.Suppliers.Dto;
using Core.gAMSPro.Utils;
using gAMSPro.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.gAMSPro.Application.Intfs.Suppliers
{
    public interface ISupplierAppService
    {
        Task<PagedResultDto<CM_SUPPLIER_ENTITY>> CM_SUPPLIER_Search(CM_SUPPLIER_ENTITY input);
        Task<FileDto> CM_SUPPLIER_ToExcel(CM_SUPPLIER_ENTITY input);
        Task<InsertResult> CM_SUPPLIER_Ins(CM_SUPPLIER_ENTITY input);
        Task<InsertResult> CM_SUPPLIER_Upd(CM_SUPPLIER_ENTITY input);
        Task<CommonResult> CM_SUPPLIER_Del(string id, string userLogin);
        Task<CommonResult> CM_SUPPLIER_App(string id, string currentUserName);
        Task<CM_SUPPLIER_ENTITY> CM_SUPPLIER_ById(string id);
    }
}
