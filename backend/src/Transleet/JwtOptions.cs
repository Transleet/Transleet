using Microsoft.IdentityModel.Tokens;

namespace Transleet
{
    public class JwtOptions
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public SymmetricSecurityKey Key { get; set; }
    }
}
