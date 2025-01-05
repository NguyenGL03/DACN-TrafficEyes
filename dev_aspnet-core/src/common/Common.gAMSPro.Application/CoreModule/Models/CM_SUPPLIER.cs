using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_SUPPLIER")]
    public class CM_SUPPLIER : Entity<string>
    {
        [Column("SUP_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string SUP_CODE { get; set; }
        public string SUP_NAME { get; set; }
        public string SUP_TYPE_ID { get; set; }
        public string REGION_ID { get; set; }
        public string ADDR { get; set; }
        public string EMAIL { get; set; }
        public string TAX_NO { get; set; }
        public string TEL { get; set; }
        public string CONTACT_PERSON { get; set; }
        public string DISCIPLINES { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }
}
