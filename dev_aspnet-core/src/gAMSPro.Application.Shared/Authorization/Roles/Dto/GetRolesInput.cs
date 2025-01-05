using System.Collections.Generic;

namespace gAMSPro.Authorization.Roles.Dto
{ 
    public class GetRolesInput
    {
        //public List<string> Permissions { get; set; }
        public string Permission { get; set; }
        public string RoleName { get; set; }
    }
}
