using System.Collections.Generic;
using Abp.Runtime.Validation;

namespace gAMSPro.Authorization.Users.Dto
{
    public class GetUsersToExcelInput : IGetUsersInput, IShouldNormalize
    {
        public string Filter { get; set; }
        
        public List<string> Permissions { get; set; }
        
        public List<string> SelectedColumns { get; set; }

        public string Permission { get; set; }

        public int? Role { get; set; }
        
        public bool OnlyLockedUsers { get; set; }
        
        public string Sorting { get; set; }

        public string UserName { get; set; }

        public string AUTH_STATUS { get; set; }

        public string DEP_ID { get; set; }

        public string SubbrId { get; set; }

        public bool IndependentUnit { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "Name,Surname";
            }

            Filter = Filter?.Trim();
        }
    }
}