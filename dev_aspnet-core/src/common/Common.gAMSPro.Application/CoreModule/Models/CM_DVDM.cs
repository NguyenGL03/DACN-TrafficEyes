using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_DVDM")]
    public class CM_DVDM : Entity<string>
    {
        [Column("DVDM_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string DVDM_CODE { get; set; }
        public string DVDM_NAME { get; set; }
        public string NOTES { get; set; }
        public string EDITER_ID { get; set; }
        public string EDIT_DT { get; set; }
        public bool IS_DVDM { get; set; }
        public bool IS_KHOI { get; set; }
        public bool IS_DVCM { get; set; }
        public bool IS_PTGD { get; set; }
        public bool IS_GDK { get; set; }


        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }
}
