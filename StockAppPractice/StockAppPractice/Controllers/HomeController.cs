using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ServiceContract;
using StockAppPractice.OptionsPatterns;
using StockAppPractice.ViewModels;

namespace StockAppPractice.Controllers
{
    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IOptions<StockApiOptions> _options;
        private readonly IFinnhubService _finnhubService;

        public HomeController(IConfiguration configuration, IOptions<StockApiOptions> options, IFinnhubService finnhubService)
        {
            this._configuration = configuration;
            this._options = options;
            this._finnhubService = finnhubService;
        }
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, object>? stockQuote = await this._finnhubService.GetStockQuote(this._options.Value.DefaultStockSymbol);

            Dictionary<string, object>? stockProfile = await this._finnhubService.GetStockProfile(this._options.Value.DefaultStockSymbol);

            StockViewModel stockViewModel = new StockViewModel
            {
                Name = stockProfile["name"].ToString(),
                Symbol = this._options.Value.DefaultStockSymbol,
                Price = Convert.ToDouble(stockQuote["c"].ToString())
            };

            return View(stockViewModel);
        }
    }
}
