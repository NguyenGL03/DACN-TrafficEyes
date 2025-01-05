using Core.gAMSPro.CoreModule.Utils;
using Core.gAMSPro.Intfs.ApiESB;
using Core.gAMSPro.Intfs.ApiESB.Dto;
using Core.gAMSPro.Intfs.AssEntriesPostSync;
using gAMSPro.Configuration;
using gAMSPro.ProcedureHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Nancy.Json;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;

namespace Core.gAMSPro.Impls.ApiESB
{
    public class ApiESBAppService : IApiESBAppService
    {
        private readonly IConfigurationRoot _appConfiguration;
        private readonly IAssEntriesPostSyncAppService _assEntriesPostSync;
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected IStoreProcedureProvider storeProcedureProvider;
         
        public ApiESBAppService(IAssEntriesPostSyncAppService assEntriesPostSync, IWebHostEnvironment env, IStoreProcedureProvider storeProcedureProvider)
        {
            _assEntriesPostSync = assEntriesPostSync;
            _appConfiguration = env.GetAppConfiguration();
            this.storeProcedureProvider = storeProcedureProvider;
        }

        public async Task<IDictionary<string, object>> AccountingSync(string id)
        {
            Dictionary<string, object> responseAll = new Dictionary<string, object>();
            var flag = _appConfiguration.GetValue<bool>("FinCore:Flag");
            if (flag)
            {
                string UserName = _appConfiguration["FinCore:UserName"];
                string Password = _appConfiguration["FinCore:Password"];
                string Host = _appConfiguration["FinCore:Host"];
                string Port = _appConfiguration["FinCore:Port"];
                string apiUrl = _appConfiguration["FinCore:AccountingSync"];

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {Base64Encode($"{UserName}:{Password}")}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //;//Url(Host, Port);
                var requestData = await _assEntriesPostSync.ASS_ENTRIES_POST_SYNC_BY_TRN_ID(id);
                var payload = JsonConvert.SerializeObject(requestData, Formatting.Indented);

                log.Info($"requestData: {requestData}");
                log.Info($"payload (out of try - catch): {payload}");

                try
                {
                    log.Info($"payload (in try - catch): {payload}");
                    log.Info($"apiUrl : {apiUrl}");
                    foreach (var body in requestData)
                    {
                        log.Info($"body : {new JavaScriptSerializer().Serialize(body)}");
                        var json = JsonConvert.SerializeObject(body, Formatting.Indented);
                        log.Info($"json : {json}");
                        HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, body);
                        log.Info($"response (using PostAsJsonAsync): {response}");
                        if (response.IsSuccessStatusCode)
                        {
                            // Parse the response content as string
                            var content = await response.Content.ReadAsStringAsync();
                            log.Info($"API CreateFixAssetTran response content: {content}");
                            var data = JsonConvert.DeserializeObject<ReponseAssEntries>(content);
                            if (data.ErrorCode == "0")
                            {
                                data.XmlData = data.TRANS.ToXmlFromList();
                                data.Trn_id = id;
                                var result = await storeProcedureProvider.GetResultValueFromStore("ASS_ENTRIES_Upd", data);
                                responseAll.Add("Result", "0");
                                responseAll.Add("ErrorDesc", "");
                            }
                            else
                            {
                                log.Info($"data convert: {data}");
                                responseAll.Add("Result", "-1");
                                responseAll.Add("ErrorDesc", data.ErrorDesc);
                            }
                        }
                        else
                        {
                            // Handle unsuccessful request
                            log.Info($"API request failed with status code: {response.StatusCode}");
                            responseAll.Add("Result", "-1");
                            responseAll.Add("ErrorDesc", "Đồng bộ hạch toán thất bại");
                        }
                    }
                }
                catch (Exception ex)
                {
                    log.Error($"Exception : {ex}");
                    responseAll.Add("Result", "-1");
                    responseAll.Add("ErrorDesc", "Đồng bộ hạch toán thất bại");
                }
                client.Dispose();
            }
            return responseAll;
        }

        public static string Base64Encode(string textToEncode)
        {
            byte[] textAsBytes = Encoding.UTF8.GetBytes(textToEncode);
            return Convert.ToBase64String(textAsBytes);
        }
        public static string Url(string host, string port)
        {
            string url = "http://" + host + ":" + port + "/FinCore/CreateFixAssetTranRestService/CreateFixAssetTran";
            return url;
        }

        public async Task<IDictionary<string, object>> AccountingPaymentSync(string id)
        {
            Dictionary<string, object> responseAll = new Dictionary<string, object>();
            var flag = _appConfiguration.GetValue<bool>("FinCore:Flag");
            if (flag)
            {
                string UserName = _appConfiguration["FinCore:UserName"];
                string Password = _appConfiguration["FinCore:Password"];
                string Host = _appConfiguration["FinCore:Host"];
                string Port = _appConfiguration["FinCore:Port"];
                string apiUrl = _appConfiguration["FinCore:AccountingSync"];

                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("Authorization", $"Basic {Base64Encode($"{UserName}:{Password}")}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var requestData = await _assEntriesPostSync.PAY_ENTRIES_POST_SYNC_BY_TRN_ID(id);
                var payload = JsonConvert.SerializeObject(requestData, Formatting.Indented);

                log.Info($"requestData: {requestData}");
                log.Info($"payload (out of try - catch): {payload}");

                try
                {
                    log.Info($"payload (in try - catch): {payload}");
                    log.Info($"apiUrl : {apiUrl}");
                    log.Info($"body : {new JavaScriptSerializer().Serialize(requestData)}");
                    var json = JsonConvert.SerializeObject(requestData, Formatting.Indented);
                    log.Info($"json : {json}");
                    HttpResponseMessage response = await client.PostAsJsonAsync(apiUrl, requestData);
                    log.Info($"response (using PostAsJsonAsync): {response}");
                    if (response.IsSuccessStatusCode)
                    {
                        // Parse the response content as string
                        var content = await response.Content.ReadAsStringAsync();
                        log.Info($"API CreateFixAssetTran response content: {content}");
                        var data = JsonConvert.DeserializeObject<ReponseAssEntries>(content);
                        if (data.ErrorCode == "0")
                        {
                            data.Trn_id = id;
                            data.XmlData = data.TRANS.ToList().ToXmlFromList();
                            var result = await storeProcedureProvider.GetResultValueFromStore("PAY_ENTRIES_POST_UpdRef", data);
                            responseAll.Add("Result", "0");
                            responseAll.Add("ErrorDesc", "");
                        }
                        else
                        {
                            log.Info($"data convert: {data}");
                            responseAll.Add("Result", data.ErrorCode);
                            responseAll.Add("ErrorDesc", data.ErrorDesc);
                        }
                    }
                    else
                    {
                        responseAll.Add("Result", "-2");
                        responseAll.Add("ErrorDesc", "Đồng bộ hạch toán thất bại");
                        // Handle unsuccessful request
                        log.Info($"API request failed with status code: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    responseAll.Add("Result", "-3");
                    responseAll.Add("ErrorDesc", "Đồng bộ hạch toán thất bại");
                    log.Error($"Exception : {ex}");
                }
                client.Dispose();
            }
            return responseAll;
        }

    }
}
