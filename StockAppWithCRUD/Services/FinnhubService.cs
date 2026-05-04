using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceContract;
using StockAppWithCRUD.Option_Pattern;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly Config_OptionPattern _options;
        private readonly IHttpClientFactory _httpClienFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IOptions<Config_OptionPattern> options, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this._options = options.Value;
            this._httpClienFactory = httpClientFactory;
            this._configuration = configuration;
        }
        public async Task<Dictionary<string, object>?> GetProfile()
        {
            using (HttpClient httpClient = this._httpClienFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/stock/profile2?symbol={this._options.symbol}&token={this._configuration.GetValue<string>("token")}");

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader streamReader = new StreamReader(responseStream);

                string responseString = await streamReader.ReadToEndAsync();

                Dictionary<string, object>? profileResponse = JsonSerializer.Deserialize<Dictionary<string, object>?>(responseString);

                return profileResponse;
            }
        }

        public async Task<Dictionary<string, object>?> GetQuote()
        {
            using (HttpClient httpClient = this._httpClienFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/quote?symbol={this._options.symbol}&token={this._configuration.GetValue<string>("token")}");

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();

                StreamReader streamReader = new StreamReader(responseStream);

                string responseString = await streamReader.ReadToEndAsync();

                Dictionary<string, object>? qouteResponse = JsonSerializer.Deserialize<Dictionary<string, object>?>(responseString);

                return qouteResponse;
            }
        }
    }
}
