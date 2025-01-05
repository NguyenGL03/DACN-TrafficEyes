namespace Trade.gAMSPro.Intfs.BankOutside.Dto
{
    public class BANK_OUTSIDE_SYS_INPUT_ENTITY
    {
        public string TranDate { get; set; } // Ngày khách hàng thực hiện lệnh
        public string TranRef { get; set; } // Số giao dịch
        public string OriginatorID { get; set; } // Kênh thực hiện giao dịch
        public string DebAcctName { get; set; } //Tên khách hàng ghi nợ
        public string DebAcctId { get; set; }  // Tài khoản ghi nợ
        public string DebAcctIdentificationType1 { get; set; }
        public string DebAcctIdentificationReference1 { get; set; }
        public string CreAcctName { get; set; }  // Tên cá nhân / đơn vị thụ hưởng
        public string CreAcctId { get; set; }   // Số tài khoản thụ hưởng (TK nhận)
        public string CreAcctIdentificationType2 { get; set; }
        public string CreAcctIdentificationReference2 { get; set; }
        public string CoreFeeID { get; set; } // Mã code phí
        public string CoreFee { get; set; } // Số tiền phí
        public string PaysysId { get; set; } // Mã Paysys
        public string RequestedExecutionDate { get; set; } // Ngày thực hiện dưới core 
        public string Amount { get; set; } // Số tiền chuyển khoản
        public string AmountCCYCD { get; set; } // Loại tiền của số tiền chuyển
        public string RecipientCCYCD { get; set; } // Loại tiền của tài khoản nhận
        public string WaiveChargeFlag { get; set; } // Cờ thu phí
        public string NetOffCharges { get; set; } // 
        public string RelateRemitInfo1 { get; set; } // Diễn giải nội dung giao dịch 1
        public string RelateRemitInfo2 { get; set; }// Diễn giải nội dung giao dịch 2
        public string RelateRemitInfo3 { get; set; }// Diễn giải nội dung giao dịch 3
        public string RelateRemitInfo4 { get; set; }// Diễn giải nội dung giao dịch 4
        public string ChargeType { get; set; } // 
        public string ChargeAcctId { get; set; } // Số tài khoản thu phí
        public string CreBankId { get; set; } // Số tài khoản thu phí
        public string CreBankName { get; set; } // Tên ngân hàng - chi nhánh
        public string InitModuleId { get; set; } // PAYSYS tại quầy
        public string CitadCode { get; set; } // Mã ngân hàng trích nợ
        public string DebBranchCode { get; set; } // Mã chi nhánh
        public string SecretCode { get; set; }
        public string TrankId { get; set; }
        public string Confirm { get; set; }
        public string TypeTransfer { get; set; }
        public string UserMaker { get; set; } //
        public string UserBranch { get; set; } //
        public string UserChecker { get; set; } //
        public string StateBudget { get; set; } //

    }
}
