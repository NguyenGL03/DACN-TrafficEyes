using System.Threading.Tasks;
using gAMSPro.Security.Recaptcha;

namespace gAMSPro.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
