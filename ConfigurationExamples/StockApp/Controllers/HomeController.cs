using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using StockApp.Models;
using StockApp.OptionsPatterns;
using StockApp.ServiceContracts;

namespace StockApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly IStockApiService _stockApiService;
        private readonly IOptions<StockApiOptions> _options;

        public HomeController(IStockApiService stockApiService, IOptions<StockApiOptions> options)
        {
            this._stockApiService = stockApiService;
            this._options = options;
        }

        [Route("/")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, object>? GetStockResult = await
                this._stockApiService.GetStocks(this._options.Value.quoteSymbol);

            Stock stock = new Stock()
            {
                StockSymbol = this._options.Value.quoteSymbol,
                CurrentPrice = Convert.ToDouble(GetStockResult["c"].ToString()),
                HighPriceOfDay = Convert.ToDouble(GetStockResult["h"].ToString()),
                LowPriceOfDay = Convert.ToDouble(GetStockResult["l"].ToString()),
                OpenPrice = Convert.ToDouble(GetStockResult["o"].ToString())
            };
            return View(stock);
        }
    }
}
