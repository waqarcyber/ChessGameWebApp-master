using ChessGameClient.AuthWebAPI;
using ChessGameClient.Models;
using ChessGame;
using AuthWebAPI.Services.Impl;
using System;
using System.Threading.Tasks;

namespace ChessGameClient.Services.Impl
{
    public class AuthServiceImpl : IAuthService, IChessObserver, IDisposable
    {
        protected readonly IAuthWebApi _authWebApi;
        protected readonly SiteUserInfo _siteUserInfo;
        protected readonly IMyLocalStorageService _localStorageService;
        protected readonly TimeUpdater _timeUpdater;
        public AuthServiceImpl(
                           IAuthWebApi authWebApi,
                           SiteUserInfo siteUserInfo,
                           IMyLocalStorageService localStorageService,
                           TimeUpdater timeUpdater)
        {
            _authWebApi = authWebApi ?? throw new ArgumentNullException(nameof(authWebApi));
            _siteUserInfo = siteUserInfo ?? throw new ArgumentNullException(nameof(siteUserInfo));
            _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
            _timeUpdater = timeUpdater ?? throw new ArgumentNullException(nameof(timeUpdater));

            ((IChessObservable)timeUpdater).Subscribe(this);
        }

        public async Task<bool> Autorization(AccountRequestModel account)
        {
            var result = await _authWebApi.Autorization(account);

            if (result != null)
            {
                var access = TokenService.DecodeToken(result.AccessToken);
                _siteUserInfo.Update(access.Claims);

                await SetTokenToLocalStorage(result.RefreshToken);

                return true;
            }

            return false;
        }

        private async Task<string> GetTokenFromLocalStorage()
        {
            return await _localStorageService.GetItemAsync("refresh" + _siteUserInfo.Id);
        }

        private async Task SetTokenToLocalStorage(string token)
        {
            await _localStorageService.SetItemAsync("refresh" + _siteUserInfo.Id, token);
        }

        private async Task RemoveTokenFromLocalStorage()
        {
            await _localStorageService.RemoveItemAsync("refresh" + _siteUserInfo.Id);
        }

        private async Task<string?> GetFirstTokenFromLocalStorage()
        {
            return await _localStorageService.GetItemAsync();
        }

        public async Task<bool> TokenAutorization()
        {
            var token = await GetFirstTokenFromLocalStorage();

            if (token == null)
                return false;

            var result = await _authWebApi.Autorization(token);

            if (result != null)
            {
                var access = TokenService.DecodeToken(result.AccessToken);

                _siteUserInfo.Update(access.Claims);

                await SetTokenToLocalStorage(result.RefreshToken);

                return true;
            }

            return false;
        }

        private object _lock = new object();
        public async void TryUpdateToken()
        {
            if (_siteUserInfo.AccessTokenExpire - DateTime.UtcNow > TimeSpan.FromSeconds(10))
                return;

            lock (_lock)
            {
                if (_siteUserInfo.AccessTokenExpire - DateTime.UtcNow > TimeSpan.FromSeconds(10))
                    return;
            }

            await GetTokens();
        }

        public virtual async Task LogOut()
        {
            await RemoveTokenFromLocalStorage();
            await _authWebApi.SingOut();
            _siteUserInfo.Default();
            //_navigationManager.NavigateTo("/", true);
        }

        private async Task GetTokens()
        {
            var refreshToken = await GetTokenFromLocalStorage();

            if (refreshToken == null)
            {
                await LogOut();
            }
            var result = await _authWebApi.Autorization(refreshToken);

            var access = TokenService.DecodeToken(result.AccessToken);

            _siteUserInfo.Update(access.Claims);

            if (result.RefreshToken != refreshToken)
                await SetTokenToLocalStorage(result.RefreshToken);
        }

        public Task UpdateAsync()
        {
            TryUpdateToken();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            ((IChessObservable)_timeUpdater).Remove(this);
        }
    }
}
