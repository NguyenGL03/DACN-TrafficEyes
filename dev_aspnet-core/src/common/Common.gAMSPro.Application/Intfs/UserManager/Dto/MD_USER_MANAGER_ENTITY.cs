using gAMSPro.Dto;

namespace Common.gAMSPro.Intfs.UserManager.Dto
{
    public class MD_USER_MANAGER_ENTITY : PagedAndSortedInputDto
    {
        public string MANAGER_ID { get; set; }
        public string TLNAME { get; set; }
        public string TLFULLNAME { get; set; }
        public string TLSUBBRID { get; set; }
        public string BRANCH_NAME { get; set; }
        public string BRANCH_CODE { get; set; }
        public string REGION_ID { get; set; }
        public string REGION_NAME { get; set; }
        public string REGION_CODE { get; set; }
        public string BRANCH_TYPE { get; set; }
        public string ROLENAME { get; set; }
        public string ROLE_ID { get; set; }
        public int? TotalCount { get; set; }
        public string BRANCH_ID { get; set; }

        public string XmlData { get; set; }
        public string XmlDataRegion { get; set; }
        public List<MD_USER_MANAGER_DT_ENTITY> BranchManager { get; set; }
        public List<MD_USER_MANAGER_REGION_DT_ENTITY> RegionManager { get; set; }
        public List<MD_USER_MANAGER_TLUSER_DT_ENTITY> UserManager { get; set; }
    }
}
