using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using StockApp.Models;

namespace StockApp.Controllers
{
    [Route("[controller]")]
    public class StockController : Controller
    {
        private readonly IFinnhubService _finnhubService;

        public StockController(IFinnhubService finnhubService)
        {
            this._finnhubService = finnhubService;
        }

        [HttpGet("[action]")]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, object>? quote = await this._finnhubService.GetQuote();

            Dictionary<string, object>? profile = await this._finnhubService.GetProfile();

            StockTrade stockTrade = new StockTrade()
            {
                stockName = profile?["name"].ToString(),
                stockSymbol = profile?["ticker"].ToString(),
                price = Convert.ToDouble(quote?["c"].ToString())
            };

            return View(stockTrade);
        }
    }
}
