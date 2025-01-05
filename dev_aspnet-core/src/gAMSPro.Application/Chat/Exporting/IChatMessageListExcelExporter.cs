using System.Collections.Generic;
using Abp;
using gAMSPro.Chat.Dto;
using gAMSPro.Dto;

namespace gAMSPro.Chat.Exporting
{
    public interface IChatMessageListExcelExporter
    {
        FileDto ExportToFile(UserIdentifier user, List<ChatMessageExportDto> messages);
    }
}
