using System.Collections.Generic;
using gAMSPro.Auditing.Dto;
using gAMSPro.Dto;

namespace gAMSPro.Auditing.Exporting
{
    public interface IAuditLogListExcelExporter
    {
        FileDto ExportToFile(List<AuditLogListDto> auditLogListDtos);

        FileDto ExportToFile(List<EntityChangeListDto> entityChangeListDtos);
    }
}
