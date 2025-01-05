using gAMSPro.Dto;
using gAMSPro.ModelHelpers;
using System.Xml.Serialization;

namespace Common.gAMSPro.Branchs.Dto
{
    /// <summary>
    /// <see cref="Common.gAMSPro.Models.CM_BRANCH"/>
    /// </summary>
    public class CM_BRANCH_ENTITY : PagedAndSortedInputDto, IAuditDto
    {
        public string? BRANCH_ID { get; set; }
        public string? FATHER_ID { get; set; }
        public string? IS_POTENTIAL { get; set; }
        public string? BRANCH_CODE { get; set; }
        public string? BRANCH_NAME { get; set; }
        public string? DAO_CODE { get; set; }
        public string? DAO_NAME { get; set; }
        public string? REGION_ID { get; set; }
        public string? REGION_NAME { get; set; }
        public string? BRANCH_TYPE { get; set; }
        public string? ADDR { get; set; }
        public string? PROVICE { get; set; }
        public string? TEL { get; set; }
        public string? TAX_NO { get; set; }
        public string? NOTES { get; set; }
        public string? RECORD_STATUS { get; set; }
        public string? MAKER_ID { get; set; }
        public DateTime? CREATE_DT { get; set; }
        public string? AUTH_STATUS { get; set; }
        public string? CHECKER_ID { get; set; }
        public DateTime? APPROVE_DT { get; set; }
        public string? F_BRANCH_CODE { get; set; }
        public string? F_BRANCH_NAME { get; set; }
        public string? AUTH_STATUS_NAME { get; set; }
        public string? RECORD_STATUS_NAME { get; set; }

        public string? BRANCH_FILTER { get; set; }
        public string? FATHER_CODE { get; set; }
        public string? REGION_CODE { get; set; }
        public Nullable<int> TOP { get; set; }
        public string? BRANCH_LOGIN { get; set; }
        public int? TotalCount { get; set; }

        public bool? INDEPENDENT_UNIT { get; set; }

        public bool? IsChecked { get; set; }

        public bool? IsLoadAll { get; set; }
        public bool? IsLoadHsAll { get; set; }

        public string? DEP_CODE { get; set; }
        public string? DEP_NAME { get; set; }
        public string? DEP_ID { get; set; }
        public string? ACTIVE_STATUS_NAME { get; set; }

        public string? USER_LOGIN { get; set; }
        public string? BRACH_NAME_FATHER { get; set; }
        public string? BR_FULL_CODE_NAME { get; set; }
        public string? TYPE_SEARCH { get; set; }
        [XmlIgnore]
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
        public string? DEPT_ID_1 { get; set; } // Phòng ban
        [XmlIgnore]
        public string? DEP_NAME_1 { get; set; } // Phòng ban
        [XmlIgnore]
        public string? DEP_CODE_1 { get; set; } // Phòng ban 
        public string? CLUSTER_ID { get; set; }
        public string? CLUSTER_NAME { get; set; }
    }
}
