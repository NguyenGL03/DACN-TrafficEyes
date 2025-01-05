using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_HANGHOA_GROUP")]
    public class CM_HANGHOA_GROUP : Entity<string>
    {
        [Column("HH_GROUP_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string HH_GROUP_CODE { get; set; }
        public string HH_GROUP_NAME { get; set; }
        public string NOTES { get; set; }
    }
}
