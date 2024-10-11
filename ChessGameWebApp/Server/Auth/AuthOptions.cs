using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChessGameWebApp.Server.Auth
{
    public class AuthOptions
    {
        public const string ISSUER = "ChessServer"; // издатель токена
        public const string AUDIENCE = "ChesshClient"; // потребитель токена
        const string KEY = "chessgame";   // ключ для шифрации
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
