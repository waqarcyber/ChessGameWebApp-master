using ChessGameClient;
using ChessGameClient.AuthWebAPI;
using System.Threading.Tasks;

namespace ChessGameClient.Services
{
    public interface IAuthService
    {
        Task<bool> Autorization(AccountRequestModel account);
        Task<bool> TokenAutorization();
        Task LogOut();
    }
}
