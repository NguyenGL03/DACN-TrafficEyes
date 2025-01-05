using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_DEPARTMENT")]
    public class CM_DEPARTMENT : Entity<string>
    {
        [Column("DEP_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string? DEP_CODE { get; set; }
        public string? DEP_NAME { get; set; }
        public string? DAO_CODE { get; set; }
        public string? DAO_NAME { get; set; }
        public string? BRANCH_ID { get; set; }
        public string? GROUP_ID { get; set; }
        public string? TEL { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? FATHER_ID { get; set; }
    }
}
