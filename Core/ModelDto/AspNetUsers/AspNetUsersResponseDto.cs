using Core.Model;
using System.ComponentModel.DataAnnotations;

namespace Core.ModelDto.AspNetUsers
{
    public class AspNetUsersResponseDto
    {
        public string Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        [MaxLength(256)]
        public string UserName { get; set; } = string.Empty;

        [MaxLength(256)]
        public string NormalizedUserName { get; set; } = string.Empty;

        [MaxLength(256)]
        public string Email { get; set; } = string.Empty;

        [MaxLength(256)]
        public string NormalizedEmail { get; set; } = string.Empty;

        [Required]
        public bool EmailConfirmed { get; set; }


        [Required]
        [MaxLength(15)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        public bool PhoneNumberConfirmed { get; set; }

        [Required]
        public bool TwoFactorEnabled { get; set; }

        public DateTimeOffset? LockoutEnd { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; }

        [Required]
        public int AccessFailedCount { get; set; }

        public int? Type { get; set; }
        public string? RefreshToten { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
    }
}
