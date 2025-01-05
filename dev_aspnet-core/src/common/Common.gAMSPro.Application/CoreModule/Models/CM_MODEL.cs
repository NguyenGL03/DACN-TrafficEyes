using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_MODEL")]
    public class CM_MODEL : Entity<string>
    {
        [Column("MO_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string MO_CODE { get; set; }
        public string CAR_TYPE_ID { get; set; }
        public string MANUFACTURER { get; set; }
        public decimal? POWER_RATE { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string AUTH_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string MO_NAME { get; set; }
    }
}
