using System.Threading.Tasks;

namespace ChessGameClient.Services
{
    public interface IMyLocalStorageService
    {
        Task<string> GetItemAsync(string key = "");
        Task SetItemAsync(string key, string value);
        Task RemoveItemAsync(string key);
    }
}
