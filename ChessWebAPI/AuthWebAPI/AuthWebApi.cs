using ChessGameClient.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ChessGameClient.AuthWebAPI
{
    public class AuthWebApi : IAuthWebApi
    {
        public readonly AuthHttpClient _auth;
        public readonly GameHttpClient _gameServer;
        public AuthWebApi(AuthHttpClient auth, GameHttpClient gameServer)
        {
            _auth = auth ?? throw new NullReferenceException(nameof(auth));
            _gameServer = gameServer ?? throw new NullReferenceException(nameof(gameServer));
        }

        public async Task<bool> Registration(AccountRequestModel account)
        {
            try
            {
                var res = await _auth.PostAsJsonAsync($"Auth/Registaration", account);

                if (res.IsSuccessStatusCode)
                    return true;

            }
            catch
            {
                throw;
            }

            return false;
        }

        public async Task<JwtTokens?> Autorization(AccountRequestModel account)
        {
            try
            {
                var request = await _auth.GetAsync($"Auth/Authentication?login={account.Login}&password={account.Password}");

                if (request.IsSuccessStatusCode)
                {
                    var result = await request.Content.ReadFromJsonAsync<JwtTokens>();
                    _auth.DefaultRequestHeaders.Authorization
                        = new AuthenticationHeaderValue("Bearer", result?.AccessToken);
                    _gameServer.DefaultRequestHeaders.Authorization
                        = new AuthenticationHeaderValue("Bearer", result?.AccessToken);

                    return result;
                }

                switch(request.StatusCode)
                {
                    case HttpStatusCode.BadRequest:
                        throw new ArgumentException("Invalid login or password!");
                    default:
                        throw new ArgumentException("Unknown error!");
                }
            }
            catch (HttpRequestException)
            {
                throw new HttpRequestException("No connection!");
            }
        }

        public async Task<JwtTokens?> Autorization(string refreshToken)
        {
            try
            {
                var result = await _auth.GetFromJsonAsync<JwtTokens>($"Auth/AutoAuth?refreshToken={refreshToken}");

                _auth.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", result?.AccessToken);
                _gameServer.DefaultRequestHeaders.Authorization
                    = new AuthenticationHeaderValue("Bearer", result?.AccessToken);

                return result;
            }
            catch
            {
                return null;
            }
        }

        public async Task SingOut()
        {
            await _auth.GetAsync($"Auth/LogOut");

            _auth.DefaultRequestHeaders.Authorization = null;
            _gameServer.DefaultRequestHeaders.Authorization = null;

        }

        public async Task<UserInfo?> GetUserInfo()
        {
            return await _gameServer.GetFromJsonAsync<UserInfo?>("ChessGame/GetUser");
        }

        public async Task<bool> AddOrRemovePlayer(int rivalId = 0)
        {
            return await _gameServer.GetFromJsonAsync<bool>($"ChessGame/AddOrRemovePlayer?rivalId={rivalId}");
        }

        public async Task<int> PlayerCount()
        {
            return await _gameServer.GetFromJsonAsync<int>("ChessGame/PlayerCount");
        }

        public async Task<bool> SessionExists()
        {
            return await _gameServer.GetFromJsonAsync<bool>("ChessGame/SessionExists");
        }

        public async Task<List<AccountDto>> Search(string username)
        {
            return await _auth.GetFromJsonAsync<List<AccountDto>>($"Admin/Search?username={username}");
        }

        public async Task BanOrUnban(string email)
        {
            await _auth.GetAsync($"Admin/Ban?email={email}");
        }
    }
}