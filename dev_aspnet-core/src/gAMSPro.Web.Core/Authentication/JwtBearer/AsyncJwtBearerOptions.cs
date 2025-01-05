using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace gAMSPro.Web.Authentication.JwtBearer
{
    public class AsyncJwtBearerOptions : JwtBearerOptions
    {
        public readonly List<IAsyncSecurityTokenValidator> AsyncSecurityTokenValidators;
        
        private readonly gAMSProAsyncJwtSecurityTokenHandler _defaultAsyncHandler = new gAMSProAsyncJwtSecurityTokenHandler();

        public AsyncJwtBearerOptions()
        {
            AsyncSecurityTokenValidators = new List<IAsyncSecurityTokenValidator>() {_defaultAsyncHandler};
        }
    }

}
