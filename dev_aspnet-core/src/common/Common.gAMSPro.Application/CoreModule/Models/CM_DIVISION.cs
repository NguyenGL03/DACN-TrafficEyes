using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_DIVISION")]
    public class CM_DIVISION : Entity<string>
    {
        [Column("DIV_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string DIV_CODE { get; set; }
        public string DIV_NAME { get; set; }
        public string ADDR { get; set; }
        public string BRANCH_ID { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }
}
