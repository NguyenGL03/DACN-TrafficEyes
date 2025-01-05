using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_DEPT_GROUP")]
    public class CM_DEPT_GROUP : Entity<string>
    {
        [Column("GROUP_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string GROUP_CODE { get; set; }
        public string GROUP_NAME { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }
}
