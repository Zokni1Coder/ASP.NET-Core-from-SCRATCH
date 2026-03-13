using Microsoft.AspNetCore.Mvc;

namespace PartialllViewsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["ListTitle"] = "Cities";
            ViewData["ListItems"] = new List<string> { "Szabadka", "Kanizsa", "Kelebia", "Horgos", "Palics", "Hajdujárás" };
            return View();
        }
        [Route("/about")]
        public IActionResult About()
        {
            ViewData["ListTitle"] = "About";
            return View();
        }
    }
}
