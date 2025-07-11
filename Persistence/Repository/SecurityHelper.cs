using Core.ModelDto;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Persistence.Repository;


public class SecurityHelper
{
    private readonly IConfiguration _config;
    public SecurityHelper(IConfiguration config)
    {
        _config = config;
    }

    public string GenerateJSONWebToken(UserInfoDto userInfo)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userInfo.Id),
            new Claim(ClaimTypes.Name, userInfo.UserName),
            new Claim(ClaimTypes.GivenName, userInfo.Name),
            new Claim(ClaimTypes.Email, userInfo.Email),
            new Claim(ClaimTypes.Role, userInfo.Role)
        };
        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"])),
            SecurityAlgorithms.HmacSha256
        );

        var token = new JwtSecurityToken(
           issuer: _config["JWT:Issuer"],
           audience: _config["JWT:Issuer"],
           claims,
           expires: DateTimeBD.GetCurrentBangladeshDateTime().AddMinutes(Convert.ToInt32(_config["JWT:Expires"])),
           signingCredentials: credentials
           );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    public int GetJWTExpires()
    {
        return Convert.ToInt32(_config["JWT:Expires"]);
    }
    public int GetJWTRefreshExpires()
    {
        return Convert.ToInt32(_config["JWT:RefreshToken_Expires"]);
    }

    // refresh token
    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    // get bd date
    public static class DateTimeBD
    {
        private static TimeZoneInfo Bangladesh_Standard_Time = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public static DateTime GetCurrentBangladeshDateTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, Bangladesh_Standard_Time);
        }
    }
}
