using Microsoft.AspNetCore.Identity;

namespace Core.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public int Type { get; set; }
        public string? RefreshToten { get; set; }
        public DateTime? RefreshTokenExpires { get; set; }
    }

}
