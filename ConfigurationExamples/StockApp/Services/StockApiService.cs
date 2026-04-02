
using Microsoft.Extensions.Options;
using StockApp.OptionsPatterns;
using StockApp.ServiceContracts;
using System.Text.Json;

namespace StockApp.Services
{
    public class StockApiService : IStockApiService
    {
        //DIP-et betartva a két mező típusa a felületük lesz.
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;


        //DI-vel megoldjuk a factory példányosítást (IoC Container-ből).
        public StockApiService(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            this._httpClientFactory = httpClientFactory;
            this._configuration = configuration;
        }

        public async Task<Dictionary<string, object>?> GetStocks(string quoteSymbol)
        {
            //using biztosítja, hogy a HttpClient dispose-olódjon, de a tényleges erőforráskezelést az IHttpClientFactory végzi
            using (HttpClient httpClient = this._httpClientFactory.CreateClient())
            {
                //Beállítjuk a request-et és a response-t
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    //Az URI-symbol string-paramétere benne van az appsettings.json fájlban, mivel az publikus
                    //A token viszont egyedi és az titkos, ezért az environment variables megoldást használjuk
                    //token:d77c729r01qp6afl39lgd77c729r01qp6afl39m0
                    RequestUri = new Uri($"https://finnhub.io/api/v1/quote?symbol={quoteSymbol}&token={this._configuration.GetValue<string>("FinnHubToken")}"),
                    Method = HttpMethod.Get
                };

                //Elküldjük a külső RESTApi szolgáltatónak a kérésünket a választ pedig elmentjük a httpResponseMessage objektumba.
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

                //string? a = this._configuration.GetSection("StockAPI")["quoteSymbol"];
                //string? b = this._configuration.GetValue<string>("FinnHubToken");

                //Stream-ként kiolvassul a response tartalmát.
                Stream responseStream = httpResponseMessage.Content.ReadAsStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string response = streamReader.ReadToEnd();
                
                //Mivel a response mező egy JSON struktúrájú string, ezért hogy értelmet nyerjen, átalakítjuk (Deserialize)(másik irányba Serialize) és értelmezve lementjük kulcs-érték párként.
                Dictionary<string, object>? responseDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(response);

                return responseDictionary;
            }
        }
    }
}
