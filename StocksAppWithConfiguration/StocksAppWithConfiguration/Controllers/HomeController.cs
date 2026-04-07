using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using ServiceContract;
using StocksAppWithConfiguration.OptionsPatterns;
using StocksAppWithConfiguration.ViewModel;

namespace StocksAppWithConfiguration.Controllers
{
    public class HomeController : Controller
    {
        private readonly IFinnhubService _finnhubService;
        private readonly IOptions<ConfigOptions> _options;

        public HomeController(IFinnhubService finnhubService, IOptions<ConfigOptions> options)
        {
            this._finnhubService = finnhubService;
            this._options = options;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, object>? stockPriceQuote = await this._finnhubService.GetStockPriceQuote(this._options.Value.DefaultStockSymbol);
            Dictionary<string, object>? stockCompanyProfile = await this._finnhubService.GetCompanyProfile(this._options.Value.DefaultStockSymbol);

            StockViewModel stockViewModel = new StockViewModel
            {
                Name = stockCompanyProfile["name"].ToString(),
                Symbol = this._options.Value.DefaultStockSymbol,
                Price = Convert.ToDouble(stockPriceQuote["c"].ToString())
            };

            return View(stockViewModel);
        }
    }
}
