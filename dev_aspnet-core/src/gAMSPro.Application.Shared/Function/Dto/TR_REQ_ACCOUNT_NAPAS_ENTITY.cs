namespace GSOFTcore.gAMSPro.Function.Dto
{
    public class TR_REQ_ACCOUNT_NAPAS_ENTITY
    {
        public string accType { set; get; }
        public string channelCode { set; get; }
        public string custName { set; get; }
        public string destAccNo { set; get; }
        public string destBankCode { set; get; }
        public string destBankName { set; get; }
        public string merchant { set; get; }
        public string remark { set; get; }
        public string srcAccNo { set; get; }
        public string srcBankLocation { set; get; }
        public string tranAMT { set; get; }
    }
    public class RESPONSE_NAPAS
    {
        public string resultCode { get; set; }
        public string resultDesc { get; set; }
        public string thirdParty { get; set; }
        public string beneficiary { get; set; }
        public string refNo { get; set; }
    }
}
