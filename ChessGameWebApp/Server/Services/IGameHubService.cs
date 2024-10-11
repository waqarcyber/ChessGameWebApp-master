using ChessGameWebApp.Server.Models;

namespace ChessGameWebApp.Server.Services
{
    public interface IGameHubService
    {
        Task StartGame(IEnumerable<Player> players);
    }
}