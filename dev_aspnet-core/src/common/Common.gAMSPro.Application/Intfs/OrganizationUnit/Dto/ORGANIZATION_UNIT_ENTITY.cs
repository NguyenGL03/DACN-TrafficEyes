using gAMSPro.Dto;

namespace Common.gAMSPro.Intfs.OrganizationUnit.Dto
{
    public class ORGANIZATION_UNIT_ENTITY : PagedAndSortedInputDto
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public DateTime? CreationTime { get; set; }
        public string CreatorUserId { get; set; }
        public string DeleterUserId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string DisplayName { get; set; }
        public string IsDeleted { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string LastModifierUserId { get; set; }
        public string ParentId { get; set; }
        public string TenantId { get; set; }
        public string OrganizationType { get; set; }
        public string OrganizationTypeName { get; set; }
        public string OrganizationCode { get; set; } 
        public string Notes { get; set; }
        public string ExtendType { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
    }
}
