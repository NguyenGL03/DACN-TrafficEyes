using System.Collections.Generic;
using Abp.Dependency;
using gAMSPro.Dto;

namespace gAMSPro.DataImporting.Excel;

public interface IExcelInvalidEntityExporter<TEntityDto> : ITransientDependency
{
    FileDto ExportToFile(List<TEntityDto> entities);
}