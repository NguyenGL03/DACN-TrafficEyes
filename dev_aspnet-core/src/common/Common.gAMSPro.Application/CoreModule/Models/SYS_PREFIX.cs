using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("SYS_PREFIX")]
    public class SYS_PREFIX : Entity<string>
    {
        [MaxLength(100)]
        public override string Id { get => base.Id; set => base.Id = value; }

        [MaxLength(10)]
        public string Prefix { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }
    }
}
