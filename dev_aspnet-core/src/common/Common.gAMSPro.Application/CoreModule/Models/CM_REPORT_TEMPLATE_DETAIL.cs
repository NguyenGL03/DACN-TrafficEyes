using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_REPORT_TEMPLATE_DETAIL")]

    public class CM_REPORT_TEMPLATE_DETAIL : Entity<string>
    {
        //[Column("REPORT_TEMPLATE_DETAIL_ID")]
        public string REPORT_TEMPLATE_DETAIL_ID { get; set; }

        public string REPORT_TEMPLATE_DETAIL_CONTENT { get; set; }
        public string REPORT_TEMPLATE_ID { get; set; }
        public string REPORT_TEMPLATE_DETAIL_CODE { get; set; }
        public string PAGE_SIZE { get; set; }
        public string NOTES { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string RECORD_STATUS { get; set; }
        public bool IsDefault { get; set; }
    }
}
