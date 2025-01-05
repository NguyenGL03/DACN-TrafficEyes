using System.Collections.Generic;
using gAMSPro.Authorization.Users.Dto;
using gAMSPro.Dto;

namespace gAMSPro.Authorization.Users.Exporting
{
    public interface IUserListExcelExporter
    {
        FileDto ExportToFile(List<UserListDto> userListDtos, List<string> selectedColumns);
    }
}