using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.gAMSPro.Application.Intfs.CarTypes.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_CAR_TYPE"/>
    /// </summary>
    public class CM_CAR_TYPE_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string CAR_TYPE_ID { get; set; }
        public string CAR_TYPE_CODE { get; set; }
        public string CAR_TYPE_NAME { get; set; }
        public string NOTES { get; set; }
        public string RECORD_STATUS { get; set; }
        public string MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string AUTH_STATUS { get; set; }
        public string CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string AUTH_STATUS_NAME { get; set; }
        public string RECORD_STATUS_NAME { get; set; }
        public int? TotalCount { get; set; }
    }
}
