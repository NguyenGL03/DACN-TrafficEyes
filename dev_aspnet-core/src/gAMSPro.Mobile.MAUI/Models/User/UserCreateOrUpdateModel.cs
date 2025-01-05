using Abp.AutoMapper;
using gAMSPro.Authorization.Users.Dto;

namespace gAMSPro.Mobile.MAUI.Models.User
{
    [AutoMapFrom(typeof(CreateOrUpdateUserInput))]
    public class UserCreateOrUpdateModel : CreateOrUpdateUserInput
    {

    }
}
