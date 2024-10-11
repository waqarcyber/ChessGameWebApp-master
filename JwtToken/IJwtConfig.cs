using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JwtToken
{
    public interface IJwtConfig
    {
        string SigningKey { get; set; }
        TimeSpan LifeTime { get; set; }
        string Audience { get; set; }
        string Issuer { get; set; }
        byte[] SigningKeyBytes { get; }
    }
}
