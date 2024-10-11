using ChessGame;
using ChessGameWebApp.Server.Models;

namespace ChessGameWebApp.Server.Services
{
    public interface IGameSessionService
    {
        Task<GameSession> GetSession(int accountId);
        Task<GameSession?> FindSession(int accountId);
        Task<bool> CloseSession(int accountId);
    }
}
