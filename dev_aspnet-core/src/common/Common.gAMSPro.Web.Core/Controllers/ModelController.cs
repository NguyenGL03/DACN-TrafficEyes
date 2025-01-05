using Abp.Application.Services.Dto;
using Common.gAMSPro.Application.Intfs.Models.Dto;
using Common.gAMSPro.Application.Intfs.Models;
using Common.gAMSPro.Web.Controllers;
using Core.gAMSPro.Utils;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Runtime.Validation;

namespace Common.gAMSPro.Web.Core.Controllers
{
    [DisableValidation]
    [Route("api/[controller]/[action]")]
    public class ModelController : CoreAmsProControllerBase
    {
        readonly IModelAppService modelAppService;

        public ModelController(IModelAppService modelAppService)
        {
            this.modelAppService = modelAppService;
        }

        [HttpPost]
        public async Task<PagedResultDto<CM_MODEL_ENTITY>> CM_MODEL_Search([FromBody] CM_MODEL_ENTITY input)
        {
            var result= await modelAppService.CM_MODEL_Search(input);
            if (result == null)
            {
                return new PagedResultDto<CM_MODEL_ENTITY>();
            }
            return result;
        }

        [HttpPost]
        public async Task<InsertResult> CM_MODEL_Ins([FromBody] CM_MODEL_ENTITY input)
        {
            return await modelAppService.CM_MODEL_Ins(input);
        }

        [HttpPost]
        public async Task<InsertResult> CM_MODEL_Upd([FromBody] CM_MODEL_ENTITY input)
        {
            return await modelAppService.CM_MODEL_Upd(input);
        }

        [HttpGet]
        public async Task<CM_MODEL_ENTITY> CM_MODEL_ById(string id)
        {
            return await modelAppService.CM_MODEL_ById(id);
        }

        [HttpDelete]
        public async Task<CommonResult> CM_MODEL_Del(string id)
        {
            return await modelAppService.CM_MODEL_Del(id);
        }

        [HttpPost]
        public async Task<CommonResult> CM_MODEL_App(string id, string currentUserName)
        {
            return await modelAppService.CM_MODEL_App(id, currentUserName);
        }
    }
}
