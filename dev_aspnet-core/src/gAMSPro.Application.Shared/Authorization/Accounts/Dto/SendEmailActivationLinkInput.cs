using System.ComponentModel.DataAnnotations;

namespace gAMSPro.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}