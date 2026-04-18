using Microsoft.Extensions.Configuration;
using ServiceContract;
using System.Text.Json;

namespace Service
{
    public class FinnhubService : IFinnhubService
    {
        private readonly IConfiguration _configuration;

        public FinnhubService(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetCompanyProfile(string stockSymbol)
        {
            HttpClient client = new HttpClient();
            using (client)
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/stock/profile2?symbol={stockSymbol}&token={this._configuration.GetSection("token").Value}");
                HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
                Stream content = httpResponseMessage.Content.ReadAsStream();
                StreamReader reader = new StreamReader(content);
                string? responseString = await reader.ReadToEndAsync();
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);
                return responseDictionary;
            }
        }
        public async Task<Dictionary<string, object>?> GetStockPriceQuote(string stockSymbol)
        {
            HttpClient client = new HttpClient();
            using (client)
            {
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://finnhub.io/api/v1/quote?symbol={stockSymbol}&token={this._configuration.GetSection("token").Value}");
                HttpResponseMessage httpResponseMessage = await client.SendAsync(httpRequestMessage);
                Stream content = await httpResponseMessage.Content.ReadAsStreamAsync();
                StreamReader streamReader = new StreamReader(content);
                string? responseString = await streamReader.ReadToEndAsync();
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseString);
                return responseDictionary;
            }
        }
    }
}
