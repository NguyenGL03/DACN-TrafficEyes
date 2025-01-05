using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_WORKFLOW")]
    public class CM_WORKFLOW : Entity<string>
    {
        [Column("WORKFLOW_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string FUNCTION_ID { get; set; }
        public string APPROVE_LEVEL { get; set; }
        public int? STEP_NUMBERS { get; set; }
        public string NEXT_LEVEL { get; set; }
        public decimal? LIMIT_VALUE_FROM { get; set; }
        public decimal? LIMIT_VALUE_TO { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }

}
