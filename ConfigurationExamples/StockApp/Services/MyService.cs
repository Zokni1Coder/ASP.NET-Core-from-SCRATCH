
namespace StockApp.Services
{
    public class MyService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        //DI-vel megoldjuk a factory példányosítást (IoC Container-ből).
        public MyService(IHttpClientFactory httpClientFactory)
        {
            this._httpClientFactory = httpClientFactory;
        }

        public async Task GetMethod()
        {
            //using biztosítja, hogy a HttpClient dispose-olódjon, de a tényleges erőforráskezelést az IHttpClientFactory végzi
            using (HttpClient httpClient = this._httpClientFactory.CreateClient())
            {
                //Beállítjuk a request-et és a response-t
                HttpRequestMessage httpRequestMessage = new HttpRequestMessage()
                {
                    RequestUri = new Uri("url"),
                    Method = HttpMethod.Get
                };
                
                HttpResponseMessage httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);
            }
        }
    }
}
