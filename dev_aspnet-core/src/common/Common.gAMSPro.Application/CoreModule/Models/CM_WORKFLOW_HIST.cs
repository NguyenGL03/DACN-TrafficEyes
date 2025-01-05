using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("CM_WORKFLOW_HIST")]
    public class CM_WORKFLOW_HIST : Entity<int>
    {
        public string MODEL_ID { get; set; }
        public string FUNCTION_ID { get; set; }
        public string LEVEL { get; set; }
        public int? STEP { get; set; }
        public string CHECKER_ID { get; set; }
        public bool? IS_APPROVE { get; set; }
        public string BRANCH_ID { get; set; }
    }

}
