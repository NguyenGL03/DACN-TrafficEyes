using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System.Xml.Serialization;

namespace Common.gAMSPro.Departments.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_DEPARTMENT"/>
    /// </summary>
    [XmlType("Departments")]
    /// 
    public class CM_DEPARTMENT_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? DEP_ID { get; set; }
        public string? DEP_CODE { get; set; }
        public string? DEP_NAME { get; set; }
        public string? DAO_CODE { get; set; }
        public string? DAO_NAME { get; set; }
        public string? BRANCH_ID { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? BRANCH_CODE { get; set; }
        public string? BRANCH_TYPE { get; set; }
        public string? GROUP_ID { get; set; }
        public string? TEL { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public int? TotalCount { get; set; }
        public string? KHOI_ID { get; set; }
        public int? TOP { set; get; }
        public string? FATHER_ID { get; set; }
        public string? GROUP_NAME { get; set; }
        public string? FATHER_NAME { get; set; }
        public string? KHOI_NAME { get; set; }
        public string? USER_LOGIN { get; set; }
        public string? DVDM_NAME { get; set; }
        public string? DVDM_CODE { get; set; }
        //Phucvh:
        public string? TYPE { get; set; }
        public string? BLOCK_ID { get; set; } // Khối 
        [XmlIgnore]
        public string? BLOCK_NAME { get; set; }// Khối 
        [XmlIgnore]
        public string? BLOCK_CODE { get; set; }// Khối 

        public string? CENTER_ID { get; set; } // Trung tâm 
        [XmlIgnore]
        public string? CENTER_NAME { get; set; } // Trung tâm 
        [XmlIgnore]
        public string? CENTER_CODE { get; set; } // Trung tâm 
        public bool? IsLoadAll { get; set; }

        //sonnq 12-3-24
        public string? UNIT_REGULAR { get; set; }

    }
}
