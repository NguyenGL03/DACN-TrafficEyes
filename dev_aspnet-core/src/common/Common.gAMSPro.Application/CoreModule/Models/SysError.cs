using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("SYS_ERROR")]
    public class SysError : Entity<string>
    {
        [Column("ErrorCode")]
        [MaxLength(20)]
        public override string Id { get => base.Id; set => base.Id = value; }

        [MaxLength(1000)]
        public string ErrorDesc { get; set; }

        [MaxLength(100)]
        public string Form { get; set; }
    }
}
