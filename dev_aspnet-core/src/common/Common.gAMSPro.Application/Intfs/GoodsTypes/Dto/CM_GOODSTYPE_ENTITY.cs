using gAMSPro.Dto;
using gAMSPro.ModelHelpers;

namespace Common.gAMSPro.GoodsTypes.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_GOODSTYPE"/>
    /// </summary>
    public class CM_GOODSTYPE_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string GD_TYPE_ID { get; set; }
        public string GD_TYPE_CODE { get; set; }
        public string GD_TYPE_NAME { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public int? TotalCount { get; set; }

        public string ASS_TYPE_ID { get; set; }
        public string ASS_TYPE_NAME { get; set; }
        public string PARENT_NAME { get; set; }
        public string ASS_TYPE_CODE { get; set; }
        public string PARENT_CODE { get; set; }
    }
}
