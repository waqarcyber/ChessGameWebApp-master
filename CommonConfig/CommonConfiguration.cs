using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;

namespace CommonConfig
{
    public static class CommonConfiguration
    {
        public static IConfiguration Create()
        {
            var root = Directory.GetParent(Directory.GetCurrentDirectory());
            var configPath = Path.Combine(root.FullName,"CommonConfig");

            if (!Directory.Exists(configPath))
            {
                root = root.Parent;
                configPath = Path.Combine(root.FullName, "CommonConfig");
            }

            if (Directory.Exists(configPath))
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(configPath)
                    .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                    .AddJsonFile("appSettings.Development.json", optional: true, reloadOnChange: true);
                return builder.Build();
            }

            throw new Exception("Common config dont found!");
        }
    }
}