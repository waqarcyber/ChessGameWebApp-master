using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ChessGameClient.Services.Impl
{
    public class MyLocalStorageServiceImpl : IMyLocalStorageService
    {
        private readonly JsonSerializerOptions options;
        private readonly string path;

        public MyLocalStorageServiceImpl(string path = "refresh.json")
        {
            this.path = path;
            options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
            };
        }
        public async Task<string> GetItemAsync(string key = "")
        {
            var dictionary = await OpenFile();

            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }
            else
                return null;
        }

        public async Task RemoveItemAsync(string key)
        {
            var dictionary = await OpenFile();

            if (dictionary.ContainsKey(key))
            {
                dictionary.Remove(key);
                await SaveFile(dictionary);
            }
        }

        public async Task SetItemAsync(string key, string value)
        {
            var dictionary = await OpenFile();

            if (dictionary.ContainsKey(key))
                dictionary[key] = value;
            else
                dictionary.Add(key, value);
            
            await SaveFile(dictionary);
        }

        private async Task<Dictionary<string, string>> OpenFile()
        {
            if (File.Exists(path))
            {
                await using (FileStream fstream = new FileStream(path, FileMode.Open))
                {
                    return await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(fstream, options);
                }
            }
            else
            {
                return new Dictionary<string, string>(); 
            }
        }

        private async Task SaveFile(Dictionary<string, string> data)
        {
            File.WriteAllText(path, string.Empty);

            await using (FileStream fstream = new FileStream(path, FileMode.OpenOrCreate, FileAccess.ReadWrite))
            {
                await JsonSerializer.SerializeAsync(fstream, data, options);
            }
        }
    }
}
