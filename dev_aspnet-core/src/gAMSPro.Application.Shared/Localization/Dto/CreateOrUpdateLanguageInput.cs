using System.ComponentModel.DataAnnotations;

namespace gAMSPro.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}