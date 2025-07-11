namespace Core.ModelDto
{
    public class LoginResponseDto
    {
        public string UserName { get; set; } = string.Empty;
        public string JwtToken { get; set; } = string.Empty;
        public DateTime Expires { get; set; }
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime RefreshTokenExpires { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Type { get; set; }
    }
}
