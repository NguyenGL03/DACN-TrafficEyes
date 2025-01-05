using Abp.Application.Services.Dto;
using gAMSPro.Helpers;
using gAMSPro.Procedures.Attributes;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace gAMSPro.ProcedureHelpers
{
    public interface IStoreProcedureProvider
    {
        string ConnectionString { get; set; }
        Task<List<TModel>> GetDataFromStoredProcedure<TModel>(string storedProcName, object parameters) where TModel : class;
        Task<IDictionary<string, object>> GetResultValueFromStore(string storedProcName, object parameters);
        Task<DataSet> GetMultiDataFromStoredProcedure(string storedProcName, List<ReportParameter> parameters);
        Task<List<dynamic>> GetMultiResultValueFromStore(string storedProcName, object parameters);
        Task<PagedResultDto<TModel>> GetPagingData<TModel>(string storedProcName, object parameters) where TModel : class;
        string GetProcedureContent(string procedureName);
        Task<GridReader> GetMultiData2(string storedProcName, object parameters = null, Func<GridReader, bool> setValueFunct = null, List<StoreParameterInfoDto> parameterInfos = null);
        Task<int> ExecuteNonQuery(string storedProcName, object parameters);
        Task<List<T>> GetDataQuery<T>(string query);
        Task<dynamic> GetMultiSelect(string storedProcName, object parameters);
        Task<List<TModel>> GetDataFromStoredProcedureByJSON<TModel>(string storedProcName, object parameters) where TModel : class;
        Task<PagedResultDto<TModel>> GetPagingDataByJSON<TModel>(string storedProcName, object parameters) where TModel : class;
    }
}