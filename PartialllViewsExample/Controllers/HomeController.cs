using Microsoft.AspNetCore.Mvc;
using PartialllViewsExample.Models;

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
        [Route("/zastava-models")]
        public IActionResult FetchingPartilView()
        {
            ListItems listItems = new ListItems()
            {
                Items = { "101", "Yugo", "128" }
            };
            return PartialView("_ListPartialView", listItems);
        }
        [Route("/zastavas")]
        public IActionResult Zastavas()
        {
            ViewData["ListTitle"] = "Zastava";
            return View();
        }
    }
}
