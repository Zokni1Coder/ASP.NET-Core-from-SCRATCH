using Microsoft.Extensions.Configuration;
using ServiceContract;
using System.Net.Http;
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
        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            Dictionary<string, object>? resultDictionary = new Dictionary<string, object>(); 
            using (HttpClient httpClient = this._httpClientFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={this._configuration.GetValue<string>("token")}"); 

                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                Stream responseStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                StreamReader sr = new StreamReader(responseStream);
                string responseString = sr.ReadToEnd();

                resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);
            }

            return resultDictionary;
        }

        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            Dictionary<string, object>? resultDictionary = new Dictionary<string, object>();
            using (HttpClient httpClient = this._httpClientFactory.CreateClient())
            {
                HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={this._configuration.GetValue<string>("token")}");
                HttpResponseMessage response = httpClient.SendAsync(request).Result;

                Stream stream = response.Content.ReadAsStreamAsync().Result;
                StreamReader reader = new StreamReader(stream);
                string responseData = reader.ReadToEnd();
                resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>?>(responseData);
            }
            return resultDictionary;
        }
    }
}
