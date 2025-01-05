using System.ComponentModel.DataAnnotations;

namespace gAMSPro.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
