using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.Goodss.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_GOODS"/>
    /// </summary>
    public class CM_GOODS_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string GD_ID { get; set; }
        public string GD_CODE { get; set; }
        public string GD_NAME { get; set; }
        public string DVDM_ID { get; set; }
        public string GD_TYPE_ID { get; set; }
        public string USE_BRANCH { get; set; }
        public decimal? AMORT_RATE { get; set; }
        public string DESCRIPTION { get; set; }
        public string SUP_ID { get; set; }
        public decimal? PRICE { get; set; }
        public string UNIT_ID { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string UNIT_CODE { get; set; }
        public string UNIT_NAME { get; set; }
        public string GD_TYPE_NAME { get; set; }
        public string GD_TYPE_CODE { get; set; }
        public string SUP_CODE { get; set; }
        public string SUP_NAME { get; set; }
        public string CD_ID { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public bool? IsChecked { get; set; }
        public int? TotalCount { get; set; }
        public string CONTENT { get; set; }
        public string MONTHLY_ALLOCATED { get; set; }
        public string GD_ID_LEVEL_ONE { get; set; }
        public string YEAR { get; set; }
        public string BRANCH_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
        public string BRANCH_ID { get; set; }
        public decimal? AMT_APPROVE { get; set; } // NS được duyệt
        public decimal? AMT_REMAIN { get; set; } // NS sử dụng thực tế
        public decimal? AMT_REMAIN_ETM { get; set; } // NS còn lại thực tế
        public string DVDM_NAME { get; set; }
        public string ACC_NUM { get; set; }
    }
}
