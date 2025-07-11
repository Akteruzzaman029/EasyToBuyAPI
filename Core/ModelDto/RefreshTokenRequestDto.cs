using System.ComponentModel.DataAnnotations;

namespace Core.ModelDto
{
    public class RefreshTokenRequestDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "RefreshToken is required")]
        public string RefreshToken { get; set; } = string.Empty;
    }
}
