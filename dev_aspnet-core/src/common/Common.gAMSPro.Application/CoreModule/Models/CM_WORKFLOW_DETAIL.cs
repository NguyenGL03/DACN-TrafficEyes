using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.CoreModule.Models
{
    [Table("CM_WORKFLOW_DETAIL")]
    public class CM_WORKFLOW_DETAIL : Entity
    {
        public string MODEL_ID { get; set; }
        public string FUNCTION_ID { get; set; }
        public string CURRENT_LEVEL { get; set; }
        public int? CURRENT_STEP { get; set; }
    }

}
