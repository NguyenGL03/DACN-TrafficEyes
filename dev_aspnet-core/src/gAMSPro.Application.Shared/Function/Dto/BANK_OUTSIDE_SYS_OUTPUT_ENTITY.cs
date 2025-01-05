namespace Trade.gAMSPro.Intfs.BankOutside.Dto
{
    public class BANK_OUTSIDE_SYS_OUTPUT_ENTITY
    {
        public string ResultResponse { get; set; } // Kết quả trả về
        public string PymtReferenceNumber { get; set; } // Số PO
        public string CPTRANDATE { get; set; } // Ngày hạch toán
        public string CPTRANID { get; set; } // Số giao dịch của core banking
        public string MSNT { get; set; } //
        public string POSTATUS { get; set; } // Trạng thái của PO
        public string RATE { get; set; } // Tỷ giá giao dịch
        public string USERDTLS { get; set; } //
        public string ErrorCode { get; set; } // Mã lỗi
        public string ErrorDesc { get; set; } // Diễn giải lỗi

    }
}
