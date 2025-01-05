using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("TL_MENU")]
    public class TL_MENU : Entity<int>
    {
        [Column("MENU_ID")]
        public override int Id { get => base.Id; set => base.Id = value; }
        [NotMapped]
        public string? MENU_ID => Id.ToString();
        public string? MENU_NAME { get; set; }
        public string? MENU_NAME_EL { get; set; }
        public string? MENU_PARENT { get; set; }
        public string? MENU_LINK { get; set; }
        public int? MENU_ORDER { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? DATE_APPROVE { get; set; }
        public string? ISAPPROVE { get; set; }
        public string? ISAPPROVE_FUNC { get; set; }
        public string? MENU_PERMISSION { get; set; }
        public string? MENU_ICON { get; set; }
        public bool? HAS_TOOLBAR { get; set; }
        public string? RECORD_STATUS { get; set; }
    }
}
