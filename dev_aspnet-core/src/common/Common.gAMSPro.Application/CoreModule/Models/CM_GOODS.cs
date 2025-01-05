using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_GOODS")]
    public class CM_GOODS : Entity<string>
    {
        [Column("GD_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
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
    }
}
