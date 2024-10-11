using Models;
using System.Security.Claims;

namespace JwtToken;

public interface ITokenService
{
    string GenerateToken(Account account);
    public ClaimsPrincipal ValidateToken(string jwtToken);
}