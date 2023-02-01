using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DotNetApi.Config
{
    public class JwtSettings
    {
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty; // Store in user-secrets or key vault
        public SymmetricSecurityKey SecretSymmetricSecurityKey => new(Encoding.ASCII.GetBytes(SecretKey));
        public int AccessTokenLifeSpanInMinutes { get; set; }
        public int RefreshTokenLifeSpanInDays { get; set; }
    }
}
