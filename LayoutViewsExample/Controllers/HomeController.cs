using Microsoft.AspNetCore.Mvc;

namespace LayoutViewsExample.Controllers
{
    public class HomeController : Controller
    {
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
        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        //Hozd létre a megadott mappa-rendszert, ahogy szokás. A _Layout.cshtml az mindig/általában a sharedben kap helyet.
        //Létrehozás: new item -> Razor Layout.

        //Layout-on belül a dinamikus tartalom mindig oda kerül, ahol szerepel a "@RenderBody()" rész, a többi az mind fix elem.
    }
}
