using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_HANGHOA_TYPE")]
    public class CM_HANGHOA_TYPE : Entity<string>
    {
        [Column("HH_TYPE_ID")]
        public override string Id { get => base.Id; set => base.Id = value; }
        public string HH_TYPE_CODE { get; set; }
        public string HH_TYPE_NAME { get; set; }
        public string HH_GROUP_ID { get; set; }
        public string NOTES { get; set; }
    }
}
