using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("INSU_COMPANY")]
    public class CM_INSU_COMPANY : Entity<string>
    {
        [Column("INSU_COMPANY_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string INSU_COMPANY_CODE { get; set; }
        public string NAME { get; set; }
        public string ADDR { get; set; }
        public string EMAIL { get; set; }
        public string TEL { get; set; }
        public string TAX_NO { get; set; }
        public string CONTACT_PERSON { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }
}
