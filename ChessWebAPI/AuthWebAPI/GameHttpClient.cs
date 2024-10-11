using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameClient.AuthWebAPI
{
    public class GameHttpClient : HttpClient
    {
        public GameHttpClient()
        {
            //BaseAddress = new Uri("https://localhost:7084/");
        }
    }
}
