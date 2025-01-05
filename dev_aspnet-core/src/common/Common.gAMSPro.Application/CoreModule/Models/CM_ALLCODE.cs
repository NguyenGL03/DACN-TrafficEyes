using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_ALLCODE")]
    public class CM_ALLCODE : Entity
    {
        public string CDNAME { get; set; }
        public string CDVAL { get; set; }
        public string CONTENT { get; set; }
        public string CDTYPE { get; set; }
        public int? LSTODR { get; set; }
    }
}
