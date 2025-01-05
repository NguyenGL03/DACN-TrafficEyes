using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System;

namespace Common.gAMSPro.GoodsTypeReals.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_GOODSTYPE_REAL"/>
    /// </summary>
    public class CM_GOODSTYPE_REAL_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string GD_RETYPE_ID { get; set; }
        public string GD_RETYPE_CODE { get; set; }
        public string TYPE_NAME { get; set; }
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
    }
}
