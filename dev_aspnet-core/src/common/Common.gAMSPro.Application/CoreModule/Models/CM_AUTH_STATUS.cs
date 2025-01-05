using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_AUTH_STATUS")]
    public class CM_AUTH_STATUS : Entity<string>
    {
        [Column("AUTH_STATUS")]
        public override string Id { get => base.Id; set => base.Id = value; }
        [MaxLength(100)]
        public string AUTH_STATUS_NAME { get; set; }
    }
}
