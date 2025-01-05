using Abp.Application.Services.Dto;
using Common.gAMSPro.GoodsTypes;
using Common.gAMSPro.GoodsTypes.Dto;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;
using STB.HDDT.EntityFramework;

namespace Common.gAMSPro.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class GoodsTypeController : CoreAmsProControllerBase
    {
        private readonly IGoodsTypeAppService goodsTypeAppService;

        //private readonly IEncryptDataGS encryptDataGS;
		IClientConnection clientConnection;


		public GoodsTypeController(IGoodsTypeAppService goodsTypeAppService, IClientConnection clientConnection)
        {
            this.goodsTypeAppService = goodsTypeAppService;
            //this.encryptDataGS = encryptDataGS;
			this.clientConnection = clientConnection;
		}

		

		[HttpPost]
        public async Task<PagedResultDto<CM_GOODSTYPE_ENTITY>> CM_GOODSTYPE_Search([FromBody]CM_GOODSTYPE_ENTITY input)
        {
            return await goodsTypeAppService.CM_GOODSTYPE_Search(input);
        }
        [HttpPost]
        public async Task<List<CM_GOODSTYPE_ENTITY>> CM_GOODSTYPE_List()
        {
            return await goodsTypeAppService.CM_GOODSTYPE_List();
        }
        [HttpPost]
        public async Task<InsertResult> CM_GOODSTYPE_Ins([FromBody]CM_GOODSTYPE_ENTITY input)
        {
            return await goodsTypeAppService.CM_GOODSTYPE_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_GOODSTYPE_Upd([FromBody]CM_GOODSTYPE_ENTITY input)
        {
            return await goodsTypeAppService.CM_GOODSTYPE_Upd(input);
        }

        [HttpGet]
        public async Task<CM_GOODSTYPE_ENTITY> CM_GOODSTYPE_ById(string id)
        {
            return await goodsTypeAppService.CM_GOODSTYPE_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_GOODSTYPE_Del(string id)
        {
            return await goodsTypeAppService.CM_GOODSTYPE_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_GOODSTYPE_App(string id, string currentUserName)
        {
            return await goodsTypeAppService.CM_GOODSTYPE_App(id, currentUserName);
        }

        [HttpGet]
        public async Task<string> GetConnectionStringDecryption()
        {
            return await clientConnection.GetConnectionString();
        }

        //[HttpGet]
        //public string GetSampleConfig(string key)
        //{
        //    return encryptDataGS.AppSettings(key);
        //}

        [HttpGet]
        public string GetClientIpAddress()
        {
            return Request.HttpContext.Connection.RemoteIpAddress.ToString();
        }
    }
}
