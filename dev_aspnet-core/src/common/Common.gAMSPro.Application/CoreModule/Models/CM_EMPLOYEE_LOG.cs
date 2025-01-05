using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_EMPLOYEE_LOG")]
    public class CM_EMPLOYEE_LOG : Entity<string>
    {
        [Column("USER_DOMAIN")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string EMP_CODE { get; set; }
        public string POS_CODE { get; set; }
        public string POS_NAME { get; set; }
    }
}
