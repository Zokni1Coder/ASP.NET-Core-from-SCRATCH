using Microsoft.Extensions.Configuration;
using ServiceContracts;

namespace Services
{
    public class ConfigService : IConfigService
    {
        private readonly IConfiguration _configuration;
        public ConfigService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public string GetConfiguration(string key)
        {
            return this._configuration.GetValue<string>(key, $"Key: {key} not found in the appsettings.json.");
        }
    }
}
