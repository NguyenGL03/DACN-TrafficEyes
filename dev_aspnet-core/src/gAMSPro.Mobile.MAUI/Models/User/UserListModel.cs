using Abp.AutoMapper;
using gAMSPro.Authorization.Users.Dto;

namespace gAMSPro.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(UserListDto))]
    public class UserListModel : UserListDto
    {
        public string Photo { get; set; }

        public string FullName => Name + " " + Surname;
    }
}
