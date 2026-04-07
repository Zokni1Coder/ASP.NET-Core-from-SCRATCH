using Microsoft.Extensions.Configuration;
using ServiceContract;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;

namespace Service
{
    public class FinnhubService : IFinnhubService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this._httpClientFactory = httpClientFactory;
            this._configuration = configuration;
        }
        public async Task<Dictionary<string, object>?> GetStockProfile(string stockSymbol)
        {
            using (HttpClient httpClient = this._httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={this._configuration.GetValue<string>("StockApiToken")}");

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                StreamReader reader = new StreamReader(responseStream);
                string responseString = await reader.ReadToEndAsync();

                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);

                return responseDictionary;
            }
        }

        public async Task<Dictionary<string, object>?> GetStockQuote(string stockSymbol)
        {
            using (HttpClient httpClient = this._httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={this._configuration.GetValue<string>("StockApiToken")}");

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
                Stream responseStream = httpResponseMessage.Content.ReadAsStream();
                StreamReader reader = new StreamReader(responseStream);
                string responseString = await reader.ReadToEndAsync();

                return JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);
            }
        }
    }
}
