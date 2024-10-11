using ChessGame;
using System.Threading.Tasks;

namespace ChessGameClient.Services
{
    public interface IGameHubService
    {
        Task GetBoard();
        bool IsConnected { get; }
        Task SendTryMove(ChessCellDto from, ChessCellDto to);
        Task StartGame();
        Task MoveBack();
        Task GameOver();
        Task AddOrRemovePlayer(int rivalId = 0);
        Task SendInvite(int rivalId, string rivalName);
        Task CloseInvite(int rivalId);
        Task Help();
        Task<bool> InitConnection();
    }
}
