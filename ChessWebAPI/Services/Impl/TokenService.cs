using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;

namespace AuthWebAPI.Services.Impl
{
    public class TokenService
    {
        public static JwtSecurityToken DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public static bool TokenIsExpired(string token)
        {
            var decodeToken = DecodeToken(token);
            var time = long.Parse(decodeToken.Claims.First(c => c.Type.Equals("exp")).Value);
            var dateTimeOffset = DateTimeOffset.FromUnixTimeSeconds(time);

            return dateTimeOffset < DateTime.UtcNow;
        }
    }
}