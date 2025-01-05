using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.gAMSPro.Intfs.UserInfo.Dto
{
    public class UserInforEntity
    {
        public string fullName { get; set; }
        public string deptName { get; set; }
        public string userName { get; set; }
        public int userId { get; set; }
        public int deptId { get; set; }
        public int gender { get; set; }
        public int status { get; set; }
        public string staffCode { get; set; }
        public int signAttachId { get; set; }
        public bool isCms { get; set; }
        public int signType { get; set; }
        public string certType { get; set; }
        public string caCloudUserId { get; set; }
        public string signImage { get; set; }
        public string signImageApostrophe { get; set; }
    }
}
