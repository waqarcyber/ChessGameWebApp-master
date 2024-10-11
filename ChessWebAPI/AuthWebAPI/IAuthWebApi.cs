using ChessGameClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGameClient.AuthWebAPI
{
    public interface IAuthWebApi
    {
        Task<bool> Registration(AccountRequestModel account);
        Task<JwtTokens?> Autorization(AccountRequestModel account);
        Task<JwtTokens?> Autorization(string refreshToken);
        Task<UserInfo?> GetUserInfo();
        Task SingOut();
        Task<bool> AddOrRemovePlayer(int rivalId = 0);
        Task<int> PlayerCount();
        Task<bool> SessionExists();
        Task<List<AccountDto>> Search(string username);
        Task BanOrUnban(string email);
    }
}
