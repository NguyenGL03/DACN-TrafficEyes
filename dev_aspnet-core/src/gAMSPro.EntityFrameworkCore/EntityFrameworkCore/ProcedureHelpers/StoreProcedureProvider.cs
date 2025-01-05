using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.UI;
using Dapper;
using gAMSPro.Configuration;
using gAMSPro.Consts;
using gAMSPro.Helper;
using gAMSPro.Helpers;
using gAMSPro.ProcedureHelpers;
using gAMSPro.ProcedureHelpers.Models;
using gAMSPro.Procedures.Attributes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using STB.HDDT.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Dapper.SqlMapper;

namespace gAMSPro.EntityFrameworkCore.ProcedureHelpers
{
    public class ReplaceStringResult
    {
        public string Text { get; set; }
        public int IndexBegin { get; set; }
        public int IndexEnd { get; set; }
    }
    public class ArgumentExceptionEx : ArgumentException
    {
        public int ErrorCode { get; }
        public string PropertyName { get; }

        public ArgumentExceptionEx(string paramName, int errorCode, string propertyName)
            : base(paramName)
        {
            ErrorCode = errorCode;
            PropertyName = propertyName;
        }
    }
    public class NullableDateTimeHandler : SqlMapper.TypeHandler<DateTime?>
    {
        public override void SetValue(IDbDataParameter parameter, DateTime? value)
        {
            if (value.HasValue)
                parameter.Value = value.Value;
            else
                parameter.Value = DBNull.Value;
        }

        public override DateTime? Parse(object value)
        {
            if (value == null || value is DBNull) return null;
            var typeofvalue = value.GetType();
            if (typeofvalue != typeof(DateTime) && typeofvalue != typeof(DateTime?))
            {
                return null;
            }
            return (DateTime)value;
        }
    }
    public class StoreProcedureProvider : IStoreProcedureProvider
    {
        private readonly int commandTimeout = 30;
        public string ConnectionString { get; set; }
        public IDetailLoggerHelper detailLoggerHelper;
        public StoreProcedureProvider(IWebHostEnvironment ev, IClientConnection clientConnection, IDetailLoggerHelper detailLoggerHelper)
        {
            this.detailLoggerHelper = detailLoggerHelper;

            var task = clientConnection.GetConnectionString();
            task.Wait();
            ConnectionString = task.Result;

            try
            {
                commandTimeout = ev.GetAppConfiguration().GetValue<int>("App:SqlServerCommandTimeout");
            }
            catch
            {
                commandTimeout = 3600;
            }

            SqlMapper.AddTypeHandler(new NullableDateTimeHandler());
        }

        #region
        public async Task<List<TModel>> GetDataFromStoredProcedure<TModel>(string storedProcName, object parameters) where TModel : class
        {
            var parameterInfos = await GetParameterInfos(storedProcName);
            var dapperParams = new DynamicParameters();
            var outputPropertyTable = new Dictionary<string, PropertyInfo>();

            if (parameters != null)
            {
                var properties = parameters.GetType().GetProperties().Where(x => x != null);

                List<StoreParameterInfoDto> procedureInfoInProperties = new List<StoreParameterInfoDto>();

                foreach (var property in properties)
                {
                    var paramName = GetParameterName(property);

                    var parameterInfo = GetParameterInfo(parameterInfos, paramName);

                    procedureInfoInProperties.Add(parameterInfo);

                    if (parameterInfo == null)
                    {
                        continue;
                    }

                    var direction = GetParameterDirection(parameterInfo);

                    if (direction == ParameterDirection.InputOutput || direction == ParameterDirection.Output)
                    {
                        outputPropertyTable.Add(parameterInfo.PARAMETER_NAME, property);
                    }

                    var parameterValue = GetParameterValue(property, parameters);

                    dapperParams.Add(parameterInfo.PARAMETER_NAME, parameterValue, null, direction);
                }

                var names = dapperParams.ParameterNames.ToList();
                foreach (var parameterInfo in parameterInfos)
                {
                    if (!names.Any(x => "@" + x == parameterInfo.PARAMETER_NAME))
                    {
                        dapperParams.Add(parameterInfo.PARAMETER_NAME, null, null, GetParameterDirection(parameterInfo));
                    }
                }

            }
            try
            {
                foreach (var item in dapperParams.ParameterNames)
                {
                    var tmp = item;
                    var value = dapperParams.Get<object>(tmp);
                }
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var rr = (List<TModel>)conn.Query<TModel>(storedProcName, dapperParams, null, true, commandTimeout, System.Data.CommandType.StoredProcedure);
                    foreach (var pair in outputPropertyTable)
                    {
                        pair.Value.SetValue(parameters, dapperParams.Get<object>(pair.Key));
                    }
                    return rr;
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        public async Task<IDictionary<string, object>> GetResultValueFromStore(string storedProcName, object parameters)
        {
            try
            {
                var model = (IDictionary<string, object>)(await GetDataFromStoredProcedure<dynamic>(storedProcName, parameters)).FirstOrDefault();
                return model;
            }
            catch (ArgumentExceptionEx e)
            {
                if (e.ErrorCode == 101)
                {
                    Dictionary<string, object> error = new Dictionary<string, object>();

                    error.Add("Result", "-2");
                    error.Add("ErrorDesc", e.Message);
                    error.Add("PropertyName", e.PropertyName);

                    return error;
                }
                else
                {
                    Dictionary<string, object> error = new Dictionary<string, object>();

                    error.Add("Result", "-1");
                    error.Add("ErrorDesc", e.Message);

                    return error;
                }

            }
        }
        public async Task<DataSet> GetMultiDataFromStoredProcedure(string storedProcName, List<ReportParameter> parameters)
        {
            var key = detailLoggerHelper.StartLog("/api/Aspose/GetReport[GetMultiDataFromStoredProcedure]");

            var parameterInfos = await GetParameterInfos(storedProcName);
            detailLoggerHelper.ActionLog(key, "GetParamInfo");
            var dapperParams = new DynamicParameters();

            if (parameters != null)
            {
                List<StoreParameterInfoDto> procedureInfoInProperties = new List<StoreParameterInfoDto>();
                foreach (var property in parameters)
                {

                    var parameterInfo = GetParameterInfo(parameterInfos, property.Name);

                    if (parameterInfo == null)
                    {
                        continue;
                    }

                    procedureInfoInProperties.Add(parameterInfo);

                    dapperParams.Add(parameterInfo.PARAMETER_NAME, GetParameterValue(property.Value));
                }
            }

            detailLoggerHelper.ActionLog(key, "ParseParam");

            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var da = new SqlDataAdapter(storedProcName, conn);
                    var ds = new DataSet();

                    da.SelectCommand.CommandType = CommandType.StoredProcedure;

                    da.SelectCommand.CommandTimeout = commandTimeout;

                    foreach (var item in dapperParams.ParameterNames)
                    {
                        da.SelectCommand.Parameters.Add(new SqlParameter(item, dapperParams.Get<object>(item)));
                    }
                    //da.SelectCommand.CommandTimeout
                    da.Fill(ds);

                    detailLoggerHelper.ActionLog(key, "ExecuteSqlQuery");

                    detailLoggerHelper.EndLog(key);

                    return ds;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        public async Task<List<dynamic>> GetMultiResultValueFromStore(string storedProcName, object parameters)
        {
            var list = await GetDataFromStoredProcedure<dynamic>(storedProcName, parameters);
            return list;
        }
        public async Task<PagedResultDto<TModel>> GetPagingData<TModel>(string storedProcName, object parameters) where TModel : class
        {
            try
            {
                var parameterInfos = await GetParameterInfos(storedProcName);
                if (parameters != null)
                {
                    var properties = parameters.GetType().GetProperties().Where(x => x != null);
                    int maxResultCount = (int?)properties.Where(x => x.Name == "MaxResultCount").FirstOrDefault()?.GetValue(parameters) ?? 0;

                    if (maxResultCount == -1)
                    {
                        Stopwatch st = new Stopwatch();

                        st.Start();

                        var items = await GetDataFromStoredProcedure<TModel>(storedProcName, parameters);

                        st.Stop();

                        return new PagedResultDto<TModel>()
                        {
                            Items = items,
                            TotalCount = items.Count
                        };
                    }

                    int totalCount = 0, skipCount = 0;
                    string sorting = "";

                    var totalCountProperty = properties.Where(x => x.Name == "TotalCount").FirstOrDefault();

                    totalCount = (int?)totalCountProperty?.GetValue(parameters) ?? 0;
                    skipCount = (int?)properties.Where(x => x.Name == "SkipCount").FirstOrDefault()?.GetValue(parameters) ?? 0;
                    sorting = (string)properties.Where(x => x.Name == "Sorting").FirstOrDefault()?.GetValue(parameters) ?? "";

                    string sortingInParam = sorting;

                    List<ReplaceStringResult> stringReplacers = new List<ReplaceStringResult>();

                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        var procedureContent = (string)((IDictionary<string, object>)conn.Query("SELECT OBJECT_DEFINITION (OBJECT_ID(N'" + storedProcName + "')) as CONTENT", null, null, true, commandTimeout, System.Data.CommandType.Text).First())["CONTENT"];

                        procedureContent = ExtractFromString(procedureContent, "BEGIN -- PAGING", "END -- PAGING").First().Text;

                        foreach (var text in ExtractFromString(procedureContent, "-- PAGING BEGIN", "-- PAGING END"))
                        {
                            var stringReplacer = new ReplaceStringResult();
                            stringReplacer.IndexBegin = text.IndexBegin;
                            stringReplacer.IndexEnd = text.IndexEnd;

                            var orderBy = ExtractFromString(text.Text, "ORDER BY", "\n").Where(x => x.Text.IndexOf(")") == -1).FirstOrDefault();

                            if (orderBy != null)
                            {
                                if (sorting.IsNullOrWhiteSpace())
                                {
                                    sorting = orderBy.Text;
                                }
                            }

                            if (sorting.IsNullOrWhiteSpace())
                            {
                                sorting = "(SELECT(1))";
                            }

                            if (orderBy != null)
                            {

                                var beginIndex = text.Text.IndexOf("select", StringComparison.CurrentCultureIgnoreCase);
                                var endIndex = text.Text.IndexOf("top", beginIndex, StringComparison.CurrentCultureIgnoreCase);
                                if (endIndex == -1 || text.Text.Substring(beginIndex + 6, endIndex - beginIndex - 6).Trim().Length != 0)
                                {
                                    text.Text = text.Text.Substring(0, orderBy.IndexBegin - 8) + text.Text.Substring(orderBy.IndexEnd);
                                }
                                else
                                {
                                    text.Text = text.Text.Substring(0, orderBy.IndexBegin - 8) + "ORDER BY " + sorting + text.Text.Substring(orderBy.IndexEnd);
                                }
                            }

                            int index = text.Text.IndexOf("-- SELECT END");

                            var textBetweenTop = ExtractFromString(text.Text.Substring(0, index).ToUpper(), "TOP", ")").FirstOrDefault();

                            if (totalCount == 0)
                            {
                                if (textBetweenTop == null)
                                {
                                    stringReplacer.Text = "\r\nBEGIN\r\nSELECT COUNT(*) " + text.Text.Substring(index);
                                }
                                else
                                {
                                    stringReplacer.Text = "\r\nBEGIN\r\nSELECT COUNT(*) FROM(" + text.Text + ") COUNTER_TOP";
                                }
                            }
                            else
                            {
                                stringReplacer.Text = "\r\nBEGIN" + stringReplacer.Text;
                            }


                            if (!string.IsNullOrWhiteSpace(sortingInParam))
                            {
                                text.Text = "SELECT A.*, ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM FROM (" + text.Text + " ) A";
                            }
                            else
                            {
                                text.Text = text.Text.Insert(index, ", ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM");
                            }

                            text.Text = ";WITH QUERY_DATA AS ( " + text.Text +
                                ") SELECT * FROM QUERY_DATA WHERE __ROWNUM > " + skipCount + " AND __ROWNUM <= " + (skipCount + maxResultCount) + "\r\nEND";

                            stringReplacer.Text += text.Text;
                            stringReplacers.Add(stringReplacer);
                        }


                        stringReplacers.Reverse();

                        foreach (var item in stringReplacers)
                        {
                            procedureContent = procedureContent.Substring(0, item.IndexBegin) + item.Text + procedureContent.Substring(item.IndexEnd);
                        }

                        var declareParam = "DECLARE " + string.Join(",\r\n", parameterInfos.Select(x =>
                        {

                            object parameterValue = null;
                            var property = properties.Where(p => CompareName(p.Name, x.PARAMETER_NAME)).FirstOrDefault();

                            if (property != null)
                            {
                                parameterValue = GetParameterValue(property, parameters);

                            }
                            return GetValuePaging(x, parameterValue);
                        }));

                        procedureContent = declareParam + "\r\n" + procedureContent;

                        var result = conn.QueryMultiple("-- PROCEDURE NAME: " + storedProcName + "\r\n\r\n" + procedureContent, null, null, commandTimeout);

                        if (totalCount == 0)
                        {
                            totalCount = result.Read<int>().FirstOrDefault();
                        }

                        return new PagedResultDto<TModel>()
                        {
                            Items = result.Read<TModel>().ToList(),
                            TotalCount = totalCount
                        };
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        public string GetProcedureContent(string procedureName)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                return (string)((IDictionary<string, object>)conn.Query("SELECT OBJECT_DEFINITION (OBJECT_ID(N'" + procedureName + "')) as CONTENT").First())["CONTENT"];
            }
        }
        public async Task<List<T>> GetDataQuery<T>(string query)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                var rr = await conn.QueryAsync<T>(query);
                return rr.ToList();
            }
        }
        public async Task<int> ExecuteNonQuery(string storedProcName, object parameters)
        {
            return 0;
        }
        public async Task<GridReader> GetMultiData2(string storedProcName, object parameters, Func<GridReader, bool> setValueFunct, List<StoreParameterInfoDto> parameterInfos = null)
        {
            if (parameterInfos == null)
            {
                parameterInfos = await GetParameterInfos(storedProcName);
            }
            var dapperParams = new DynamicParameters();
            var outputPropertyTable = new Dictionary<string, PropertyInfo>();

            if (parameters != null)
            {
                var properties = parameters.GetType().GetProperties().Where(x => x != null);

                List<StoreParameterInfoDto> procedureInfoInProperties = new List<StoreParameterInfoDto>();

                foreach (var property in properties)
                {
                    var paramName = GetParameterName(property);

                    var parameterInfo = GetParameterInfo(parameterInfos, paramName);

                    procedureInfoInProperties.Add(parameterInfo);

                    if (parameterInfo == null)
                    {
                        continue;
                    }

                    var direction = GetParameterDirection(parameterInfo);

                    if (direction == ParameterDirection.InputOutput || direction == ParameterDirection.Output)
                    {
                        outputPropertyTable.Add(parameterInfo.PARAMETER_NAME, property);
                    }

                    var parameterValue = GetParameterValue(property, parameters);

                    dapperParams.Add(parameterInfo.PARAMETER_NAME, parameterValue, null, direction);
                }


                // add property not include in class parameters
                //foreach (var parameterInfo in parameterInfos.Where(x => x!=null && !procedureInfoInProperties.Any(pi => pi != null &&  x.PARAMETER_NAME.ToLower().Replace("@", "").Replace("p_", "") == pi.PARAMETER_NAME.ToLower().Replace("@", "").Replace("p_", ""))))
                //{
                //    dapperParams.Add(parameterInfo.PARAMETER_NAME);
                //}

                var names = dapperParams.ParameterNames.ToList();
                foreach (var parameterInfo in parameterInfos)
                {
                    if (!names.Any(x => "@" + x == parameterInfo.PARAMETER_NAME))
                    {
                        dapperParams.Add(parameterInfo.PARAMETER_NAME, null, null, GetParameterDirection(parameterInfo));
                    }
                }

            }
            try
            {
                foreach (var item in dapperParams.ParameterNames)
                {
                    var tmp = item;
                    var value = dapperParams.Get<object>(tmp);
                }
                using (var conn = new SqlConnection(ConnectionString))
                {
                    //          var rr = await conn.QueryAsync<TModel>(storedProcName, dapperParams, null, null, System.Data.CommandType.StoredProcedure);
                    var rr = await conn.QueryMultipleAsync(storedProcName, dapperParams, null, commandTimeout, System.Data.CommandType.StoredProcedure);
                    foreach (var pair in outputPropertyTable)
                    {
                        pair.Value.SetValue(parameters, dapperParams.Get<object>(pair.Key));
                    }

                    setValueFunct?.Invoke(rr);
                    return rr;
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }



            //var parameterInfos = await GetParameterInfos(storedProcName);
            //var dapperParams = new DynamicParameters();

            //if (parameters != null)
            //{

            //    foreach (var property in parameters)
            //    {

            //        var parameterInfo = GetParameterInfo(parameterInfos, property.Name);

            //        if (parameterInfo == null)
            //        {
            //            continue;
            //        }


            //        dapperParams.Add(parameterInfo.PARAMETER_NAME, property.Value);
            //    }


            //    var names = dapperParams.ParameterNames.ToList();
            //    foreach (var parameterInfo in parameterInfos)
            //    {
            //        if (!names.Any(x => "@" + x == parameterInfo.PARAMETER_NAME))
            //        {
            //            dapperParams.Add(parameterInfo.PARAMETER_NAME, null);
            //        }
            //    }
            //}
            //try
            //{
            //    foreach (var item in dapperParams.ParameterNames)
            //    {
            //        var tmp = item;
            //        var value = dapperParams.Get<object>(tmp);
            //    }
            //    using (var conn = new SqlConnection(ConnectionString))
            //    {
            //        var rr = await conn.QueryMultipleAsync(storedProcName, dapperParams, null, commandTimeout, System.Data.CommandType.StoredProcedure);
            //        return rr;

            //    }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}
        }

        public async Task<dynamic> GetMultiSelect(string storedProcName, object parameters)
        {
            var parameterInfos = await GetParameterInfos(storedProcName);
            var dapperParams = new DynamicParameters();

            if (parameters != null)
            {
                var convertParameters = JObject.FromObject(parameters).ToObject<Dictionary<string, object>>();
                List<StoreParameterInfoDto> procedureInfoInProperties = new List<StoreParameterInfoDto>();
                foreach (var property in convertParameters)
                {

                    var parameterInfo = GetParameterInfo(parameterInfos, property.Key);

                    if (parameterInfo == null)
                    {
                        continue;
                    }

                    procedureInfoInProperties.Add(parameterInfo);

                    dapperParams.Add(parameterInfo.PARAMETER_NAME, GetParameterValue(property.Value));
                }
            }
            try
            {
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var da = new SqlDataAdapter(storedProcName, conn);
                    var ds = new DataSet();

                    da.SelectCommand.CommandType = CommandType.StoredProcedure; // type procedure

                    da.SelectCommand.CommandTimeout = commandTimeout; // set timeout

                    foreach (var item in dapperParams.ParameterNames)
                    {
                        da.SelectCommand.Parameters.Add(new SqlParameter(item, dapperParams.Get<object>(item)));
                    }
                    da.Fill(ds);
                    return ds;

                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion
        // ================================================================================
        #region
        private static IEnumerable<ReplaceStringResult> ExtractFromString(string source, string start, string end)
        {
            ReplaceStringResult result = new ReplaceStringResult();

            result.IndexBegin = 0;
            result.IndexEnd = 0;
            result.Text = "";

            while ((result.IndexBegin = source.ToUpper().IndexOf(start.ToUpper(), result.IndexBegin)) >= 0 && (result.IndexEnd = source.ToUpper().IndexOf(end.ToUpper(), result.IndexBegin)) >= 0)
            {
                result.IndexBegin += start.Length;
                result.Text = source.Substring(result.IndexBegin, result.IndexEnd - result.IndexBegin);
                yield return result;
                result.IndexBegin = result.IndexEnd;
            }


        }
        private ParameterDirection GetParameterDirection(StoreParameterInfoDto parameterInfo)
        {
            switch (parameterInfo.PARAMETER_MODE)
            {
                case ParameterSqlDirection.Input:
                    return ParameterDirection.Input;
                case ParameterSqlDirection.InputOutput:
                    return ParameterDirection.InputOutput;
                case ParameterSqlDirection.Output:
                    return ParameterDirection.Output;
            }
            return ParameterDirection.Input;
        }
        private StoreParameterInfoDto GetParameterInfo(List<StoreParameterInfoDto> parameterInfos, string paramName)
        {
            var result = parameterInfos
                .Where(x => x.PARAMETER_NAME.Replace("@", "").ToLower().Equals(paramName.Replace("@", "").ToLower())
                || x.PARAMETER_NAME.Replace("@", "").ToLower().Equals("p_" + paramName.Replace("@", "").ToLower())
                || x.PARAMETER_NAME.Replace("@", "").ToLower().Equals("l_" + paramName.Replace("@", "").ToLower()))
                .SingleOrDefault();
            return result;
        }
        private async Task<List<StoreParameterInfoDto>> GetParameterInfos(string storeProcName)
        {

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                var rr = conn.Query<StoreParameterInfoDto>($"select PARAMETER_NAME, PARAMETER_MODE, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH from information_schema.parameters where specific_name = @storeProcName", new
                {
                    storeProcName
                });
                return rr.ToList();
            }
        }
        private bool CompareName(string propertyName, string parameterName)
        {
            return parameterName.Replace("@", "").ToLower().Equals(propertyName.Replace("@", "").ToLower())
                 || parameterName.Replace("@", "").ToLower().Equals("p_" + propertyName.Replace("@", "").ToLower());
        }
        private string GetParameterName(PropertyInfo property)
        {
            var paramName = "";

            var storeParameterAttribute = (StoreParamAttribute)property.GetCustomAttributes(typeof(StoreParamAttribute), false).FirstOrDefault();

            if (storeParameterAttribute == null)
            {
                paramName = property.Name;
            }
            else
            {
                paramName = storeParameterAttribute.Name;
            }
            return "@" + paramName;
        }

        private object GetParameterValue(object value)
        {
            if (value == null)
            {
                return null;
            }
            if (value.GetType() == typeof(DateTime?))
            {
                return ((DateTime?)value).Value.ToString(gAMSProCoreConst.DateTimeFormat);
            }
            if (value.GetType() == typeof(DateTime))
            {
                return ((DateTime)value).ToString(gAMSProCoreConst.DateTimeFormat);
            }
            return value;
        }
        private object GetParameterValue(PropertyInfo property, object obj)
        {
            var value = property.GetValue(obj);
            return GetParameterValue(value);
        }
        private string GetValuePaging(StoreParameterInfoDto paramInfo, object value)
        {
            switch (paramInfo.DATA_TYPE.ToLower())
            {
                case "bit":
                    return paramInfo.PARAMETER_NAME + " " + paramInfo.DATA_TYPE + " = " + (value != null ? ((bool)value ? "1" : "0") : "NULL");
                case "int":
                case "numeric":
                case "decimal":
                    return paramInfo.PARAMETER_NAME + " " + paramInfo.DATA_TYPE + " = " + (value != null ? value.ToString() : "NULL");
                case "xml":
                    return paramInfo.PARAMETER_NAME + " " + paramInfo.DATA_TYPE + " = " + (value != null ? "'" + value.ToString() + "'" : "NULL");
                case "varchar":
                case "varchar2":
                case "nchar":
                case "char":
                case "nvarchar":
                    return paramInfo.PARAMETER_NAME + " " + paramInfo.DATA_TYPE + "(" + (paramInfo.CHARACTER_MAXIMUM_LENGTH == -1 ? "MAX" : paramInfo.CHARACTER_MAXIMUM_LENGTH.ToString()) + ")" + " = " + (value != null ? "N'" + value.ToString().Replace("'", "''") + "'" : "NULL");
                default:
                    throw new Exception("Not support Exception " + paramInfo.DATA_TYPE);

            }
        }
        #endregion
        public async Task<List<TModel>> GetDataFromStoredProcedureByJSON<TModel>(string storedProcName, object parameters) where TModel : class
        {
            var parameterInfos = await GetParameterInfos(storedProcName);
            var dapperParams = new DynamicParameters();
            var outputPropertyTable = new Dictionary<string, PropertyInfo>();

            if (parameters != null)
            {
                string body = JsonConvert.SerializeObject(parameters);
                JObject obj = JObject.Parse(body);

                IEnumerable<JProperty> jProperties = obj.Properties();
                IEnumerable<PropertyInfo> properties = jProperties.Select(jp => new JsonPropertyInfo(jp.Name, jp.Value));

                List<StoreParameterInfoDto> procedureInfoInProperties = new List<StoreParameterInfoDto>();

                foreach (var property in properties)
                {
                    var paramName = GetParameterName(property);

                    var parameterInfo = GetParameterInfo(parameterInfos, paramName);

                    procedureInfoInProperties.Add(parameterInfo);

                    if (parameterInfo == null)
                    {
                        continue;
                    }

                    var direction = GetParameterDirection(parameterInfo);

                    if (direction == ParameterDirection.InputOutput || direction == ParameterDirection.Output)
                    {
                        outputPropertyTable.Add(parameterInfo.PARAMETER_NAME, property);
                    }

                    var parameterValue = GetParameterValue(property, parameters);

                    dapperParams.Add(parameterInfo.PARAMETER_NAME, parameterValue, null, direction);
                }

                var names = dapperParams.ParameterNames.ToList();
                foreach (var parameterInfo in parameterInfos)
                {
                    if (!names.Any(x => "@" + x == parameterInfo.PARAMETER_NAME))
                    {
                        dapperParams.Add(parameterInfo.PARAMETER_NAME, null, null, GetParameterDirection(parameterInfo));
                    }
                }

            }
            try
            {
                foreach (var item in dapperParams.ParameterNames)
                {
                    var tmp = item;
                    var value = dapperParams.Get<object>(tmp);
                }
                using (var conn = new SqlConnection(ConnectionString))
                {
                    var rr = (List<TModel>)conn.Query<TModel>(storedProcName, dapperParams, null, true, commandTimeout, System.Data.CommandType.StoredProcedure);
                    foreach (var pair in outputPropertyTable)
                    {
                        pair.Value.SetValue(parameters, dapperParams.Get<object>(pair.Key));
                    }
                    return rr;
                }
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
        public async Task<PagedResultDto<TModel>> GetPagingDataByJSON<TModel>(string storedProcName, object parameters) where TModel : class
        {
            try
            {
                var parameterInfos = await GetParameterInfos(storedProcName);
                if (parameters != null)
                {
                    string body = JsonConvert.SerializeObject(parameters);

                    // Convert JSON string to JObject
                    JObject obj = JObject.Parse(body);

                    IEnumerable<JProperty> jProperties = obj.Properties();
                    IEnumerable<PropertyInfo> properties = jProperties.Select(jp => new JsonPropertyInfo(jp.Name, jp.Value));

                    int maxResultCount = Convert.ToInt32(properties.Where(x => x.Name.Equals("maxresultcount", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault()?.GetValue(null) ?? 0);

                    if (maxResultCount == -1)
                    {
                        Stopwatch st = new Stopwatch();

                        st.Start();

                        var items = await GetDataFromStoredProcedure<TModel>(storedProcName, parameters);

                        st.Stop();

                        return new PagedResultDto<TModel>()
                        {
                            Items = items,
                            TotalCount = items.Count
                        };
                    }

                    int totalCount = 0, skipCount = 0;
                    string sorting = "";

                    var totalCountProperty = properties.Where(x => x.Name == "TotalCount").FirstOrDefault();

                    totalCount = (int?)totalCountProperty?.GetValue(parameters) ?? 0;
                    skipCount = (int?)properties.Where(x => x.Name == "SkipCount").FirstOrDefault()?.GetValue(parameters) ?? 0;
                    sorting = (string)properties.Where(x => x.Name == "Sorting").FirstOrDefault()?.GetValue(parameters) ?? "";

                    string sortingInParam = sorting;

                    List<ReplaceStringResult> stringReplacers = new List<ReplaceStringResult>();

                    using (var conn = new SqlConnection(ConnectionString))
                    {
                        var procedureContent = (string)((IDictionary<string, object>)conn.Query("SELECT OBJECT_DEFINITION (OBJECT_ID(N'" + storedProcName + "')) as CONTENT", null, null, true, commandTimeout, System.Data.CommandType.Text).First())["CONTENT"];

                        procedureContent = ExtractFromString(procedureContent, "BEGIN -- PAGING", "END -- PAGING").First().Text;

                        foreach (var text in ExtractFromString(procedureContent, "-- PAGING BEGIN", "-- PAGING END"))
                        {
                            var stringReplacer = new ReplaceStringResult();
                            stringReplacer.IndexBegin = text.IndexBegin;
                            stringReplacer.IndexEnd = text.IndexEnd;

                            var orderBy = ExtractFromString(text.Text, "ORDER BY", "\n").Where(x => x.Text.IndexOf(")") == -1).FirstOrDefault();

                            if (orderBy != null)
                            {
                                if (sorting.IsNullOrWhiteSpace())
                                {
                                    sorting = orderBy.Text;
                                }
                            }

                            if (sorting.IsNullOrWhiteSpace())
                            {
                                sorting = "(SELECT(1))";
                            }

                            if (orderBy != null)
                            {

                                var beginIndex = text.Text.IndexOf("select", StringComparison.CurrentCultureIgnoreCase);
                                var endIndex = text.Text.IndexOf("top", beginIndex, StringComparison.CurrentCultureIgnoreCase);
                                if (endIndex == -1 || text.Text.Substring(beginIndex + 6, endIndex - beginIndex - 6).Trim().Length != 0)
                                {
                                    text.Text = text.Text.Substring(0, orderBy.IndexBegin - 8) + text.Text.Substring(orderBy.IndexEnd);
                                }
                                else
                                {
                                    text.Text = text.Text.Substring(0, orderBy.IndexBegin - 8) + "ORDER BY " + sorting + text.Text.Substring(orderBy.IndexEnd);
                                }
                            }

                            int index = text.Text.IndexOf("-- SELECT END");

                            var textBetweenTop = ExtractFromString(text.Text.Substring(0, index).ToUpper(), "TOP", ")").FirstOrDefault();

                            if (totalCount == 0)
                            {
                                if (textBetweenTop == null)
                                {
                                    stringReplacer.Text = "\r\nBEGIN\r\nSELECT COUNT(*) " + text.Text.Substring(index);
                                }
                                else
                                {
                                    stringReplacer.Text = "\r\nBEGIN\r\nSELECT COUNT(*) FROM(" + text.Text + ") COUNTER_TOP";
                                }
                            }
                            else
                            {
                                stringReplacer.Text = "\r\nBEGIN" + stringReplacer.Text;
                            }


                            if (!string.IsNullOrWhiteSpace(sortingInParam))
                            {
                                text.Text = "SELECT A.*, ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM FROM (" + text.Text + " ) A";
                            }
                            else
                            {
                                text.Text = text.Text.Insert(index, ", ROW_NUMBER() OVER (ORDER BY " + sorting + ") AS __ROWNUM");
                            }

                            text.Text = ";WITH QUERY_DATA AS ( " + text.Text +
                                ") SELECT * FROM QUERY_DATA WHERE __ROWNUM > " + skipCount + " AND __ROWNUM <= " + (skipCount + maxResultCount) + "\r\nEND";

                            stringReplacer.Text += text.Text;
                            stringReplacers.Add(stringReplacer);
                        }


                        stringReplacers.Reverse();

                        foreach (var item in stringReplacers)
                        {
                            procedureContent = procedureContent.Substring(0, item.IndexBegin) + item.Text + procedureContent.Substring(item.IndexEnd);
                        }

                        var declareParam = "DECLARE " + string.Join(",\r\n", parameterInfos.Select(x =>
                        {

                            object parameterValue = null;
                            var property = properties.Where(p => CompareName(p.Name, x.PARAMETER_NAME)).FirstOrDefault();

                            if (property != null)
                            {
                                parameterValue = GetParameterValue(property, parameters);

                            }
                            return GetValuePaging(x, parameterValue);
                        }));

                        procedureContent = declareParam + "\r\n" + procedureContent;

                        var result = conn.QueryMultiple("-- PROCEDURE NAME: " + storedProcName + "\r\n\r\n" + procedureContent, null, null, commandTimeout);

                        if (totalCount == 0)
                        {
                            totalCount = result.Read<int>().FirstOrDefault();
                        }

                        return new PagedResultDto<TModel>()
                        {
                            Items = result.Read<TModel>().ToList(),
                            TotalCount = totalCount
                        };
                    }
                }

                return null;
            }
            catch (Exception e)
            {
                throw new UserFriendlyException(e.Message);
            }
        }
    }
}
