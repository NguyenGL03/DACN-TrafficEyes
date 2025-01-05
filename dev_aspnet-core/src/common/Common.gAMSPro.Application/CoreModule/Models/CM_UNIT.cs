using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_UNIT")]
    public class CM_UNIT : Entity<string>
    {
        [Column("UNIT_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string UNIT_CODE { get; set; }
        public string UNIT_NAME { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }
}
