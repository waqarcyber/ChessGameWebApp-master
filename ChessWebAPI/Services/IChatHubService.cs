using System.Threading.Tasks;
using static ChessGameClient.Services.Impl.ChatHubServiceImpl;

namespace ChessGameClient.Services
{
    public interface IChatHubService
    {
        bool IsConnected { get; }
        Task Send(string message);
        ValueTask DisposeAsync();
        Task Start();
        void SetUpdater(Updater updater);
    }
}
