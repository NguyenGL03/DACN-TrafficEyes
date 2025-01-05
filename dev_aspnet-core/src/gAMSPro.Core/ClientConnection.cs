using gAMSPro.Configuration;
using gAMSPro.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace STB.HDDT.EntityFramework
{
    public interface IClientConnection
    {
        Task<string> GetConnectionString();
    }

    public class ClientConnection : IClientConnection
    {
        private static string _connectionStringResult;
        private readonly string _clientRequest;
        private readonly string _clientRequestContent;
        private readonly bool _isRequestServer;
        private readonly string _localConnectionName;
        private readonly bool _isBypassCert;
        private string localConnectionString;
        private string decryptConnectionString;
        private readonly IDetailLoggerHelper detailLoggerHelper;


        public ClientConnection(IWebHostEnvironment env, IDetailLoggerHelper detailLoggerHelper, string localConnectionName = "Default", string uamClientRequestContentKey = "uam:ClientRequestContent")
        {
            this.detailLoggerHelper = detailLoggerHelper;
            var appConfiguration = env.GetAppConfiguration();
            _clientRequest = appConfiguration["App:uam_ClientRequestUrl"];
            this.detailLoggerHelper.StartLog("App:uam_ClientRequestUrl:" + _clientRequest);
            _clientRequestContent = appConfiguration["App:uam_ClientRequestContent"];
            this.detailLoggerHelper = detailLoggerHelper;
            this.detailLoggerHelper.StartLog("App:uam_ClientRequestContent:" + _clientRequestContent);

            var strValue = appConfiguration["App:uam_LocalConnection"];
            if (strValue == "false")
            {
                _isRequestServer = true;
            }
            this.detailLoggerHelper.StartLog("App:uam_LocalConnection:" + strValue);

            strValue = appConfiguration["App:uam_ByPassCert"];
            if (strValue == "true")
            {
                _isBypassCert = true;
            }
            _localConnectionName = localConnectionName;
            this.detailLoggerHelper.StartLog("App:uam_ByPassCert:" + strValue);

            localConnectionString = env.GetAppConfiguration().GetConnectionString("Default");
           // decryptConnectionString = DeEncryptTwoWay(localConnectionString);
        }

        public async Task<string> GetConnectionString()
        {
            if (!string.IsNullOrEmpty(_connectionStringResult))
            {
                return _connectionStringResult;
            }

            if (_isRequestServer)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        if (_isBypassCert)
                        {
                            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
                        }

                        var content = new StringContent(_clientRequestContent, Encoding.UTF8, "application/x-www-form-urlencoded");

                        var post = await client.PostAsync(_clientRequest, content, new CancellationToken(false));

                        var resultContent = await post.Content.ReadAsStringAsync();
                        this.detailLoggerHelper.StartLog("Result:" + resultContent);

                        var xdoc = new XmlDocument();
                        xdoc.LoadXml(resultContent);
                        var resultNode = xdoc.SelectSingleNode("//response//result");
                        var contentNode = xdoc.SelectSingleNode("//response//content");
                        if (resultNode == null || contentNode == null)
                        {
                            return resultContent;
                        }

                        if (resultNode.InnerText == "000")
                            _connectionStringResult = contentNode.InnerText;
                        return _connectionStringResult;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return localConnectionString;


            //if (string.IsNullOrEmpty(_localConnectionName))
            //         {
            //             return string.Empty;
            //         }

            //         var strValue = ConfigurationManager.ConnectionStrings[_localConnectionName];
            //         _connectionStringResult = strValue.ConnectionString;
            //         return _connectionStringResult;
        }
        public static string DeEncryptTwoWay(string cipherString)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //System.Configuration.AppSettingsReader settingsReader = new AppSettingsReader();
            //Get your key from config file to open the lock!
            string key = "GsoftGlobal@239";
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            hashmd5.Clear();


            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            var response = UTF8Encoding.UTF8.GetString(resultArray);
            return response;
        }
    }
}
