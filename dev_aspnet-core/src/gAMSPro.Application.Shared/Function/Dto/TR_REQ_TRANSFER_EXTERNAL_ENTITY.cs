namespace GSOFTcore.gAMSPro.Function.Dto
{
    public class TR_REQ_TRANSFER_EXTERNAL_ENTITY
    {
        public string sourceRef { set; get; }
        public string brn { set; get; }
        public string offsetBrn { set; get; }
        public string offsetAcc { set; get; }
        public string offsetCcy { set; get; }
        public string offsetAmt { set; get; }
        public string narrative { set; get; }
        public string awi1 { set; get; }
        public string awi2 { set; get; }
        public string awi3 { set; get; }
        public string awi4 { set; get; }
        public string utlbnf1 { set; get; }
        public string utlbnf2 { set; get; }
        public string remBank { set; get; }
        public string econt1 { set; get; }
        public string econt2 { set; get; }
        public string econt3 { set; get; }
        public string feeAmt { set; get; }
        public string vatAmt { set; get; }
    }
    public class RESPONSE_TRANSFER_EXTERNAL
    {
        public string trnRefNo { get; set; }
        public string resultCode { get; set; }
        public string resultDesc { get; set; }
        public string Result { get; set; }
        public string ErrorDesc { get; set; }
    }
}
