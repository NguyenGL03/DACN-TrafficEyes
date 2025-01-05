using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("SYS_CODEMASTERS")]
    public class SYS_CODEMASTERS : Entity<string>
    {
        [Column("Prefix")]
        [MaxLength(10)]
        public override string Id { get => base.Id; set => base.Id = value; }

        public decimal CurValue { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [MaxLength(1)]
        public string Active { get; set; }
    }
}
