using Abp.Application.Services;
using GSOFTcore.gAMSPro.Function.Dto;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Trade.gAMSPro.Intfs.BankOutside.Dto;

namespace GSOFTcore.gAMSPro.Functions
{
    public interface IPushCore : IApplicationService
    {
        Task<string> PushCore(string refId, string trnType);

        Task<string> PushCoreMW(string refId, string trnType);

        Task<string> PushCoreMWVAT(string refId, string trnType);

        Task<string> PushCorePAY(string refId, string trnType);

        Task<string> PustToCoreAmort(string amortId);

        Task<string> UpdateRefId_Tool(string ET_ID, string RefNo);

        Task<GlListRespone.GlLstLstGl[]> GetGLAccountList();
        Task<CasaListRespone.ListOutListCasa[]> CasaListCheck(List<string> lstAccNo);
        Task<RESPONSE_NAPAS> CHECK_ACCOUNT_IS_NAPAS(TR_REQ_ACCOUNT_NAPAS_ENTITY input);
        Task<RESPONSE_NAPAS> TR_REQ_CHECK_ACCOUNT_IS_NAPAS(TR_REQ_ACCOUNT_NAPAS_ENTITY input);
        Task<RESPONSE_TRANSFER_EXTERNAL> TRANSFER_EXTERNAL(TR_REQ_TRANSFER_EXTERNAL_ENTITY input);
        Task<RESPONSE_GET_BILL_API_ENTITY> TR_REQ_PAY_AUTO_GET_BILL_BVB(string billcode, string prvcode, string sevcode);
        Task<RESPONSE_REGISTER_BILL_API_ENTITY> TR_REQ_PAY_AUTO_REGISTER_BILL(REQUEST_REGISTER_BILL_API_ENTITY input);
        Task<RESPONSE_REGISTER_BILL_API_ENTITY> TR_REQ_PAY_AUTO_DELL_BILL(string id);
        Task<DateTime> CheckHolidayBVB(DateTime date);
        Task<RESPONSE_BILL_TYPE_B_ENTITY> GET_HISTORY_BILL_FROM_BVB(REQUEST_BILL_TYPE_B_ENTITY input);
        Task<RESPONSE_GET_REF_NO_OF_BILL_ENTITY> GET_REF_NO_OF_BILL_FROM_BVB(REQUEST_GET_REF_NO_OF_BILL_ENTITY input);
        Task<string> SyncInventoryImport(DateTime date, bool isStringXml);
        Task<string> SyncInventoryTranfer(DateTime date, bool isStringXml);
        Task<REPONSE_INVENTORY_IMPORT_ENTITY> GET_INVENTORY_IMPORT(REQ_INVENTORY_ENTITY input);
        Task<REPONSE_INFO_INVENTORY_ENTITY> GET_INFO_INVENTORY(REQ_INVENTORY_ENTITY input);
        Task<REPONSE_INVENTORY_TRANFER_ENTITY> GET_INVENTORY_TRANFER(REQ_INVENTORY_ENTITY input);
        Task<BANK_OUTSIDE_SYS_OUTPUT_ENTITY> BANK_OUTSIDE_SYS(BANK_OUTSIDE_SYS_INPUT_ENTITY input);

    }



}