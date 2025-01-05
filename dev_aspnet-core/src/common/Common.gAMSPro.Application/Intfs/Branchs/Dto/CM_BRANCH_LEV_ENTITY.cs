using System.Collections.Generic;

namespace Common.gAMSPro.Branchs.Dto
{
    public class CM_BRANCH_LEV_ENTITY
    {
        public List<CM_BRANCH_ENTITY> Areas { get; set; }
        public List<CM_BRANCH_ENTITY> Branchs { get; set; }
        public List<CM_BRANCH_ENTITY> SubBranchs { get; set; }
    }
}
