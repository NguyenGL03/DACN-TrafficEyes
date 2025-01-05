using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Intfs.HangHoa.Dto
{
    /// <summary>
    /// BẢNG HÀNG HÓA
    /// </summary>
    public class CM_HANGHOA_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string HH_ID { get; set; } // ID HÀNG HÓA
        public string HH_CODE { get; set; } // MÃ HÀNG HÓA
        public string HH_NAME { get; set; } // TÊN HÀNG HÓA
        public string HH_TYPE_ID { get; set; } // ID LOẠI HÀNG HÓA
        public string HH_TYPE_CODE { get; set; } // MÃ LOẠI HÀNG HÓA
        public string HH_TYPE_NAME { get; set; } // TÊN LOẠI HÀNG HÓA
        public string USE_BRANCH { get; set; }
        public decimal? AMORT_RATE { get; set; }
        public string DESCRIPTION { get; set; }
        public decimal? PRICE { get; set; }
        public string UNIT_ID { get; set; } // ID ĐƠN VỊ TÍNH
        public string UNIT_CODE { get; set; } // MÃ ĐƠN VỊ TÍNH
        public string UNIT_NAME { get; set; } // TÊN ĐƠN VỊ TÍNH
        public string NOTES { get; set; }
        public string SUP_ID { get; set; }
        public string SUP_NAME { get; set; }
        public string SUP_CODE { get; set; }
        public string RECORD_STATUS { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public bool? IsChecked { get; set; }
        public int? TotalCount { get; set; }
        public string CHECKER_ID { get; set; }
        public string AUTH_STATUS { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string HH_GROUP_ID { get; set; }
        public string HH_GROUP_CODE { get; set; }
        public string HH_GROUP_NAME { get; set; }
        public string GD_ID { get; set; }
        public string GD_CODE { get; set; }
        public string GD_NAME { get; set; }
        public string TYPE_LIMIT { get; set; }
        public string HHGROUP_GROUP_ID_CDT { get; set; }
        public string HHGROUP_GROUP_ID_TTCT { get; set; }
        public string DVCM_ID { get; set; }
        public string IS_TC { get; set; }
        public string IS_DVKD { get; set; }
        public string IS_CDT { get; set; }
        public string IS_DVCM { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_NAME { get; set; }

        // ==================== QUÁ TRÌNH MUA SẮM =========================
        public string TR_REQ_DOC_ID { get; set; } // ID PHIẾU YÊU CẦU MUA SẮM
        public string PL_REQ_DOC_ID { get; set; } // ID TỜ TRÌNH CHỦ TRƯƠNG
        public string TECHNICAL_SPEC { get; set; } // QUY CÁCH KỸ THUẬT
        public string BUILDING_ID { get; set; } // ID TRỤ SỞ
        public string BUILDING_NAME { get; set; } // TÊN TRỤ SỞ
    }
}
