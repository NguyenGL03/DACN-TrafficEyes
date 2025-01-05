using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Suppliers.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_SUPPLIER"/>
    /// </summary>
    public class CM_SUPPLIER_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? SUP_ID { get; set; }
        public string? SUP_CODE { get; set; }
        public string? ACC_NUM { get; set; }
        public string? ACC_NUM_OUT { get; set; }
        public string? SUP_NAME { get; set; }
        public string? SUP_TYPE_ID { get; set; }
        public string? SUP_TYPE_NAME { get; set; }
        public string? REGION_ID { get; set; }
        public string? REGION_NAME { get; set; }
        public string? ADDR { get; set; }
        public string? EMAIL { get; set; }
        public string? TAX_NO { get; set; }
        public string? TEL { get; set; }
        public string? CONTACT_PERSON { get; set; }
        public string? DISCIPLINES { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public string? DISCIPLINES_NAME { get; set; }
        public bool? IsChecked { get; set; }
        public int? TotalCount { get; set; }

        public decimal? LAST_RATE_AVG { get; set; }
        public DateTime? LAST_RATE_DT { get; set; }
        public string? ACC_NAME { get; set; }
        public string? BANK_NAME { get; set; }
        public string? ACC_NAME_OUT { get; set; }
        public string? BANK_NAME_OUT { get; set; }
        public string? TLFullName { get; set; }
        public DateTime? EDITOR_DT { get; set; }
        public string? EDITOR_ID { get; set; }
        public string? USER_LOGIN { get; set; }

	    public string? TRN_TYPE { get; set; }
        public string? MA_CONG_NO { get; set; }
        public string? REF_ID { get; set; }
        public string? TOTAL_AMT { get; set; }
        public int? TOP { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? BRANCH_CODE { get; set; }


    }
}
