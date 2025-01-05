using System.Collections.Generic;
using Abp.Dependency;

namespace gAMSPro.DataImporting.Excel;

public interface IExcelDataReader<TEntityDto> : ITransientDependency
{
    List<TEntityDto> GetEntitiesFromExcel(byte[] fileBytes);
}