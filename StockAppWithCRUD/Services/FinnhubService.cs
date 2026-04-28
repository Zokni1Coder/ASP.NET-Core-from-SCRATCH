using Microsoft.Extensions.Configuration;
using ServiceContract;
using System.Net.Http;
using System.Text.Json;

namespace Services
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IHttpClientFactory _httpClienFactory;
        private readonly IConfiguration _configuration;

        public FinnhubService(IConfiguration _configuration, IHttpClientFactory httpClientFactory)
        {
            this._configuration = _configuration;
            this._httpClienFactory = httpClientFactory;
        }
        public async Task<Dictionary<string, object>?> GetProfile()
        {
            using (HttpClient httpClient = this._httpClienFactory.CreateClient())
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/stock/profile2?symbol={this._configuration.GetSection("FinnhubService").GetValue<string>("symbol")}&token={this._configuration.GetValue<string>("token")}");

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
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/quote?symbol={this._configuration.GetSection("FinnhubService").GetValue<string>("symbol")}&token={this._configuration.GetValue<string>("token")}");

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
