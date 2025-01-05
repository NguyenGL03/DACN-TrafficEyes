using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace GSOFTcore.gAMSPro.Function.Dto
{
    public class RESPONSE_GET_BILL_API_ENTITY
    {
        public string billRef { set; get; }
        public string billCode { set; get; }
        public string billNumber { set; get; }
        public string customerName { set; get; }
        public string customerAddress { set; get; }
        public string billDate { set; get; }
        public string amount { set; get; }
        public string month { set; get; }
        public string billRoute { set; get; }
        public string registerStatus { set; get; }
        public string registerStatusName { set; get; }
        public string resultCode { set; get; }
        public string resultDesc { set; get; }
        public string billSchool { set; get; }
        public string billClass { set; get; }
        public string billDetail { set; get; }
        public string amountUp { set; get; }
    }

    public class REQUEST_REGISTER_BILL_API_ENTITY
    {
        public string accountNo { set; get; }
        public string accountType { set; get; }
        public Boolean autoPayment { set; get; }
        public string billAlias { set; get; }
        public string billCode { set; get; }
        public string channel { set; get; }
        public string customerNo { set; get; }
        public string prvCode { set; get; }
        public string sevCode { set; get; }
    }

    public class RESPONSE_REGISTER_BILL_API_ENTITY
    {
        public string resultCode { set; get; }
        public List<string> resultDesc { set; get; }
        public string time { set; get; }
        public RESPONSE_DATA_REGISTER_BILL_API_ENTITY data { set; get; }
    }

    public class RESPONSE_DATA_REGISTER_BILL_API_ENTITY
    {
        public string id { set; get; }
        public string departmentCode { set; get; }
        public string departmentName { set; get; }
        public string sevCode { set; get; }
        public string sevName { set; get; }
        public string prvCode { set; get; }
        public string prvName { set; get; }
        public string billCode { set; get; }
        public string billAlias { set; get; }
        public string billCustomerName { set; get; }
        public string customerNo { set; get; }
        public string customerName { set; get; }
        public string customerType { set; get; }
        public string accountNo { set; get; }
        public string accountType { set; get; }
        public string accountTypeName { set; get; }
        public Boolean autoPayment { set; get; }
    }

    public class REQUEST_BILL_TYPE_B_ENTITY
    {
        public string billCode { set; get; }
        public string prvCode { set; get; }
        public string sevCode { set; get; }
        public string year { set; get; }
        public string resultCode { set; get; }
        public string resultDesc { set; get; }
        public string time { set; get; }
        public List<RESPONSE_BILL_TYPE_B_ENTITY> data { set; get; }
    }

    [XmlType("XmlAutoRecurring")]
    public class RESPONSE_BILL_TYPE_B_ENTITY
    {
        public string resultCode;
        public List<RESPONSE_BILL_TYPE_B_ENTITY> data;
        public string ID { set; get; }
        public string billCode { set; get; }
        public string billCodeAlias { set; get; }
        public string prvCode { set; get; }
        public string sevCode { set; get; }
        public string year { set; get; }
        public string customerName { set; get; }
        public string customerAddress { set; get; }
        public decimal? amount { set; get; }
        public string month { set; get; }
        public string accountNo { set; get; }
        public string accountName { set; get; }
        public string accountNoRec { set; get; }
        public string accountNameRec { set; get; }
        public string billDate { set; get; }
    }

    public class REQUEST_GET_REF_NO_OF_BILL_ENTITY
    {
        public string ID { set; get; }
    }

    public class RESPONSE_GET_REF_NO_OF_BILL_ENTITY
    {
        public string REF_NO { set; get; }
        public string accountNo { set; get; }
        public string accountName { set; get; }
        public string accountNoRec { set; get; }
        public string accountNameRec { set; get; }
    }
}
