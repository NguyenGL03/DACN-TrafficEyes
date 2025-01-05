using System.ComponentModel.DataAnnotations;

namespace gAMSPro.Authorization.Users.Profile.Dto
{
    public class UpdateLogoPictureInput
    {
        [Required]
        [MaxLength(400)]
        public string FileToken { get; set; }

        public int X { get; set; }

        public int Y { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }
    }
}