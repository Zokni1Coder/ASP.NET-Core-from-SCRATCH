using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using StockApp.OptionsPatterns;
using StockApp.ServiceContracts;

namespace StockApp.Controllers
{
    public class HomeController : Controller
    {
        //DIP-et betartva a Service.
        private readonly IStockApiService _stockApiService;
        //Options Pattern alkalmazása DIP-et betartva.
        private readonly IOptions<StockApiOptions> _options;

        //Az IoC Container majd a megfelelő objektumokat fogja létrehozni a felületek alapján és behelyettesíti.
        public HomeController(IStockApiService stockApiService, IOptions<StockApiOptions> options)
        {
            this._stockApiService = stockApiService;
            this._options = options;
        }

        [Route("/")]
        //Mivel egy asszinkron kérést küldünk a külső RESTApi szolgáltatónak, ezért Task lesz a void helyett.
        public async Task<IActionResult> Index()
        {
            //Kimentjük a Service visszatérési értékét/eredményét
            Dictionary<string, object>? GetStockResult = await
                this._stockApiService.GetStocks(this._options.Value.quoteSymbol);

            //Létrehozunk egy Stock egyedet és beletesszük a Service-ből kapott adatokat.
            Stock stock = new Stock()
            {
                StockSymbol = this._options.Value.quoteSymbol,
                //Azért kell a ToDouble-ben string-re konvertálni, mert a ToDouble nem tud JSON elemet konvertálni. 
                CurrentPrice = Convert.ToDouble(GetStockResult["c"].ToString()),
                HighPriceOfDay = Convert.ToDouble(GetStockResult["h"].ToString()),
                LowPriceOfDay = Convert.ToDouble(GetStockResult["l"].ToString()),
                OpenPrice = Convert.ToDouble(GetStockResult["o"].ToString())
            };
            //Átadjuk a View-nak a modellt, azaz Strongly Typed View-vá alakítjuk.
            return View(stock);
        }
    }
}
