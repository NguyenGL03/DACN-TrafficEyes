using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_EMPLOYEE")]
    public class CM_EMPLOYEE : Entity<string>
    {
        [Column("EMP_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string EMP_CODE { get; set; }
        public string EMP_NAME { get; set; }
        public string BRANCH_ID { get; set; }
        public string DEP_ID { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string AUTH_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }

        //19-06-23
        public string POS_CODE { get; set; }
        public string POS_NAME { get; set; }

        //09-08-23
        public string EMAIL { get; set; }
        public string PHONE { get; set; }
    }
}
