using Blazored.LocalStorage;
using ChessGameClient.Services;

namespace ChessGameWebApp.Client.Services.Impl
{
    public class MyLocalStorageServiceV2 : IMyLocalStorageService
    {
        private readonly ILocalStorageService _localStorageService;

        public MyLocalStorageServiceV2(ILocalStorageService localStorageService) 
        {
            _localStorageService = localStorageService ?? throw new ArgumentNullException(nameof(localStorageService));
        }
        public async Task<string> GetItemAsync(string key = "")
        {
            return await _localStorageService.GetItemAsync<string>(key);
        }

        public async Task RemoveItemAsync(string key)
        {
            await _localStorageService.RemoveItemAsync(key);
        }

        public async Task SetItemAsync(string key, string value)
        {
            await _localStorageService.SetItemAsync(key, value);
        }
    }
}
