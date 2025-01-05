using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.gAMSPro.Intfs.UserInfo.Dto
{
    public class UserInfoResponseEntity
    {
        public UserInforEntity[] userInfor { get; set; }
        public string message { get; set; }
        public int status { get; set; }
    }
}
