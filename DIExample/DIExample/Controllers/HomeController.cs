using Microsoft.AspNetCore.Mvc;
using Services;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        //Létrehozol egy readonly mezőt a Service-ből.
        private readonly CitiesService _citiesService;
        public HomeController()
        {
            //A mezőnek létrehozol egy egyedet. Ez nem DI, de ezzel majd később foglalkozunk.
            this._citiesService = new CitiesService();
        }
        [Route("/")]
        public IActionResult Index()
        {
            //Így el tudjuk érni a Service metódusát és értéket fogunk kapni.
            List<string> cities = _citiesService.GetCities;
            return View(cities);
        }
    }
}
