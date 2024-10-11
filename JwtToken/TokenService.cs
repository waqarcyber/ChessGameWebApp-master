using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Models;
using Microsoft.IdentityModel.Tokens;

namespace JwtToken;
public class TokenService : IAccessTokenService, IRefreshTokenService, ITokenService
{
    private readonly IJwtConfig _jwtConfig;
    public TokenService(IJwtConfig jwtConfig)
    {
        _jwtConfig = jwtConfig ?? throw new ArgumentException(nameof(jwtConfig));
    }
    public string GenerateToken(Account account)
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.NameId, account.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, account.Email),
            new Claim(JwtRegisteredClaimNames.GivenName, account.Username)
        };
        // Add roles as multiple claims
        foreach (var role in account.Roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role.Name));
        }

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.Add(_jwtConfig.LifeTime),
            Audience = _jwtConfig.Audience,
            Issuer = _jwtConfig.Issuer,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_jwtConfig.SigningKeyBytes),
                SecurityAlgorithms.HmacSha256Signature
            )
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    public ClaimsPrincipal ValidateToken(string jwtToken)
    {
        SecurityToken validatedToken;
        TokenValidationParameters validationParameters = new TokenValidationParameters();

        validationParameters.ValidateLifetime = true;

        validationParameters.ValidAudience = _jwtConfig.Audience;
        validationParameters.ValidIssuer = _jwtConfig.Issuer;
        validationParameters.IssuerSigningKey = new SymmetricSecurityKey(_jwtConfig.SigningKeyBytes);

        ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);

        return principal;
    }

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