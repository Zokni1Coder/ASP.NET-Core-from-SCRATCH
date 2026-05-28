using Microsoft.AspNetCore.Mvc;
using ServiceContract;
using ServiceContract.DTOs;
using StockAppWithCRUD.ViewModels;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;

namespace StockAppWithCRUD.Controllers
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

            ViewBag.Quote = quote;
            ViewBag.Profile = profile;

            StockViewModel stock = new StockViewModel()
            {
                Quotes = quote,
                Profile = profile,
                Name = $"{profile["name"]} ({profile["ticker"]})",
                Price = Convert.ToDouble(quote["c"].ToString())
            };

            return View(stock);
        }
    }
}
