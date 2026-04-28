using Microsoft.AspNetCore.Mvc;
using ServiceContract;

namespace StockAppWithCRUD.Controllers
{
    [Route("stock")]
    public class HomeController : Controller
    {
        private readonly IFinnhubService _finnhubService;

        public HomeController(IFinnhubService finnhubService)
        {
            this._finnhubService = finnhubService;
        }

        [HttpGet("[action]")]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            Dictionary<string, object>? quotes = await this._finnhubService.GetQuote();
            
            Dictionary<string, object>? profile = await this._finnhubService.GetProfile();

            return View();
        }
    }
}
