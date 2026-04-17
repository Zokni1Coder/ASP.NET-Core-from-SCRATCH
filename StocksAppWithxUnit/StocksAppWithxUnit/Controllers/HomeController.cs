using Microsoft.AspNetCore.Mvc;

namespace StocksAppWithxUnit.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
