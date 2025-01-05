using Abp.Domain.Entities;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.gAMSPro.Models
{
    [Table("SYS_PARAMETERS")]
    public class SYS_PARAMETERS : Entity<decimal>
    {
        public string ParaKey { get; set; }
        public string ParaValue { get; set; }
        public string DataType { get; set; }
        public string Description { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
    }

}
