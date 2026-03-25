using Microsoft.AspNetCore.Mvc;

namespace EnvironmentsExample.Controllers
{
    public class HomeController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        //Az IWebHostEnvironment-et azért nem kell regisztrálni a DI/IoC Containerbe, mert amikor az app elindul, automatikusan bele kerül egy csomó service-t és a IWebHostEnvironment is egy ilyen. 
        public HomeController(IWebHostEnvironment webHostEnvironment)
        {
             this._webHostEnvironment = webHostEnvironment;
        }

        [Route("/")]
        //[Route("something")]
        public IActionResult Index()
        {
            ViewBag.Environment = this._webHostEnvironment.EnvironmentName;
            return View();
        }
        //A következő endpointot ugyanarra a routra állítjuk mint az Indexet. Figyeld meg hogy ugyanazt az errort a böngészőben hogyan jeleníti meg Development, Staging és Production környezetben!
        //[Route("something")]
        //public IActionResult Other()
        //{
        //    return View();
        //}
    }
}
