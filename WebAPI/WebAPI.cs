using ChessGame;
using System.Net.Http.Json;

namespace WebAPI
{
    public class WebAPI
    {
        private readonly string _host;
        private readonly HttpClient _httpClient;
        public WebAPI(string? host = null, HttpClient? httpClient = null)
        {
            _host = host ?? "https://localhost:7085";
            _httpClient = httpClient ?? new HttpClient();
        }

        public Task<Board?> GetBoard()
        {
            return _httpClient.GetFromJsonAsync<Board>($"{_host}/ChessGame/Board");
        }

    }
}