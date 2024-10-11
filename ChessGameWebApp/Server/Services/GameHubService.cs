using ChessGameWebApp.Server.Models;
using ChessGameWebApp.Server.SignalRHub;
using Microsoft.AspNetCore.SignalR;

namespace ChessGameWebApp.Server.Services
{
    public class GameHubService : IGameHubService
    {
        private readonly IHubContext<GameHub> _hubContext;
        private readonly IConnectionService _connectionService;
        public GameHubService(IHubContext<GameHub> hubContext, IConnectionService connectionService)
        {
            _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
            _connectionService = connectionService ?? throw new ArgumentNullException(nameof(connectionService));
        }

        public async Task StartGame(IEnumerable<Player> players)
        {
            var connectionIds = await _connectionService.GetConnections(players.Select(p => p.Id).ToArray()); 
            await _hubContext.Clients.Clients(connectionIds).SendAsync("StartGame", true);
        }
    }
}
