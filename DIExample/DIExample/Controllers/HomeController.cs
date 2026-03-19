using Microsoft.AspNetCore.Mvc;
using Services;
using ServiceContracts;

namespace DIExample.Controllers
{
    public class HomeController : Controller
    {
        //Létrehozol egy readonly mezőt a Service-ből.
        //DIP-et betartva, a mező, az egy Interface lesz, amit a Service implementál. Ahhoz, hogy ez működjön, állítsd be a DIExmaple-nek a a ServiceContracts-ot mint Dependency. 
        private readonly ICitiesService _citiesService;
        //Paraméterül kap egy ICitiesService-t, de a IoC Container egy CitiesService egyedet fog küldeni és az kerül bele a mezőbe.
        public HomeController(ICitiesService citiesService)
        {
            //A mezőnek létrehozol egy egyedet. Ez nem DI, de ezzel majd később foglalkozunk.
            //new CitiesService();
            this._citiesService = citiesService; 
        }
        [Route("/")]
        public IActionResult Index()
        {
            //Így el tudjuk érni a Service metódusát és értéket fogunk kapni.
            //Mivel módosítottuk a CitiesService-t és nem gett property van, hanem metódus, ezért itt metódusként fogjuk meghívni.
            List<string> cities = _citiesService.GetCities();
            return View(cities);
        }
    }
}
