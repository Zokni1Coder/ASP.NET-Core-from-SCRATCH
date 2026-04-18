using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContract;
using StocksAppWithxUnit.OptionsPatterns;
using StocksAppWithxUnit.ViewModels;

namespace StocksAppWithxUnit.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly TradeOptions _options;

        public HomeController(IFinnhubService finnhubService, IOptions<TradeOptions> options)
        {
            this._finnhubService = finnhubService;
            this._options = options.Value;
        }

        /// <summary>
        /// A StockApi-val együttműködő srevice-ket itt hívjuk meg a controllerben.
        /// </summary>
        /// <returns>Egy típus szigorú View-t ad vissza.</returns>
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, object>? stockResponse = await this._finnhubService.GetStockPriceQuote(this._options.symbol);

            Dictionary<string, object>? profileResponse = await this._finnhubService.GetCompanyProfile(this._options.symbol);

            StockTradeViewModel stockWM = new StockTradeViewModel()
            {
                StockName = profileResponse["name"].ToString(),
                StockSymbol = profileResponse["ticker"].ToString(),
                Price = Convert.ToDouble(stockResponse["c"].ToString())
            };
            return View(stockWM);
        }
    }
}
