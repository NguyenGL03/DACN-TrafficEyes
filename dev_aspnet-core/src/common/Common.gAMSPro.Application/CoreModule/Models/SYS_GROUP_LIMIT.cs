using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("SYS_GROUP_LIMIT")]
    public  class SYS_GROUP_LIMIT : Entity<string>
    {
        [Column("GROUP_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string GROUP_CODE { get; set; }
        public string GROUP_DES { get; set; }
        public bool? IS_HO { get; set; }
        public string TYPE { get; set; }
    }
}
