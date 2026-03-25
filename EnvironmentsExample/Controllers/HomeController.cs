using Microsoft.AspNetCore.Mvc;

namespace EnvironmentsExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        [Route("something")]
        public IActionResult Index()
        {
            return View();
        }
        //A következő endpointot ugyanarra a routra állítjuk mint az Indexet. Figyeld meg hogy ugyanazt az errort a böngészőben hogyan jeleníti meg Development, Staging és Production környezetben!
        [Route("something")]
        public IActionResult Other()
        {
            return View();
        }
    }
}
