using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameClient.AuthWebAPI
{
    public class AuthHttpClient : HttpClient
    {
        public AuthHttpClient()
        {
            //BaseAddress = new Uri("https://localhost:7256/");
        }
    }
}
