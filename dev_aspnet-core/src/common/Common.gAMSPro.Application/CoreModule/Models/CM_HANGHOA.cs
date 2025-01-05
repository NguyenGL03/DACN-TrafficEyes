using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.CoreModule.Models
{
    public class CM_HANGHOA : Entity<string>
    {
        [Column("HH_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string HH_CODE { get; set; }
        public string HH_NAME { get; set; }
        public string HH_TYPE_ID { get; set; }
        public string USE_BRANCH { get; set; }
        public string GD_ID { get; set; }
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
