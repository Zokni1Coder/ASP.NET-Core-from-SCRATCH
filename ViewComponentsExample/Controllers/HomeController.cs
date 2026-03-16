using Microsoft.AspNetCore.Mvc;

namespace ViewComponentsExample.Controllers
{
    public class HomeController : Controller
    {

        /*
            Futási sorrend szemléltetve:    

            Controller
               ↓
            View
               ├─ Partial
               ├─ ViewComponent
               ↓
            Layout

         */

        [Route("/")]
        public IActionResult Index()
        {
            return View();
        }
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }
    }
}
